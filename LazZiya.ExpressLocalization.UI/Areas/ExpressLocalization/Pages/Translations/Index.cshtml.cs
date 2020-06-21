using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.TranslationServices;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Microsoft.Extensions.Logging;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Translations
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;
        private readonly ITranslationServiceFactory _translationServiceFactory;
        private readonly ILogger Log;

        public readonly List<SelectListItem> TranslationProviders;

        public IndexModel(IEFGenericDataManager dataManager, ITranslationServiceFactory translationServiceFactory, ILogger<IndexModel> log)
        {
            Log = log;
            DataManager = dataManager;
            _translationServiceFactory = translationServiceFactory;
            
            // get all registered translation services names
            TranslationProviders = new List<SelectListItem>();
            
            foreach(var ts in translationServiceFactory.ServiceNames())
            {
                TranslationProviders.Add(new SelectListItem { Text = ts, Value = ts });
            }
        }

        [BindProperty(SupportsGet = true)]
        public string DefaultCulture { get; set; }

        /// <summary>
        /// optionally set target culture 
        /// to navigate directly to  the culture edit box
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string TargetCulture { get; set; }

        /// <summary>
        /// just in case coming from different pages (Resources or Cultures...)
        /// to return to the desired page.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Resource ID
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int ResourceID { get; set; }

        /// <summary>
        /// Resource entity including related translations
        /// </summary>
        public XLResource Resource { get; set; }

        /// <summary>
        /// Model item to be used for add/update
        /// </summary>
        [BindProperty]
        public XLTranslation Translation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<XLCulture> Cultures { get; set; }

        private class CRUDAjaxReponse
        {
            public HttpStatusCode StatusCode { get; set; }
            /// <summary>
            /// Target culture
            /// </summary>
            public string Target { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (ResourceID == 0)
            {
                TempData.Warning("Resource ID can't be zero!");
                return RedirectToPage("Resources");
            }

            var include = new List<Expression<Func<XLResource, object>>> { x => x.Translations };
            Resource = await DataManager.GetAsync<XLResource>(x => x.ID == ResourceID, include);

            if (Resource == null)
            {
                TempData.Danger("Resource not found!");
                return RedirectToPage("Index");
            }

            var culturesExp = new List<Expression<Func<XLCulture, bool>>> { };
            if (!string.IsNullOrWhiteSpace(TargetCulture))
            {
                culturesExp.Add(x => x.ID == TargetCulture);
            }

            int _ = 0;
            (Cultures, _) = await DataManager.ListAsync<XLCulture>(1, int.MaxValue, culturesExp, null);

            DefaultCulture = (await DataManager.GetAsync<XLCulture>(x => x.IsDefault == true)).ID;

            return Page();
        }

        public async Task<JsonResult> OnPostSaveTranslationAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.BadRequest, Target = Translation.CultureID });
            }

            // get translation from DB
            var entity = await DataManager.GetAsync<XLTranslation>(x => x.ResourceID == Translation.ResourceID && x.CultureID == Translation.CultureID);

            bool success;

            if (entity == null)
            {
                // create a new translation if no record exist for this resource and culture 
                success = await DataManager.AddAsync<XLTranslation>(Translation);
            }
            else
            {
                Log.LogInformation($"UPDATE TRANSLATION: {entity.IsActive}  {Translation.IsActive}");
                // Update existing translation if record is exist for this resource and culture 
                entity.Value = Translation.Value;
                entity.IsActive = Translation.IsActive;
                success = await DataManager.UpdateAsync<XLTranslation, int>(entity);
            }

            return success
                ? new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.OK, Target = Translation.CultureID })
                : new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.InternalServerError, Target = Translation.CultureID });
        }

        public async Task<JsonResult> OnPostDeleteTranslationAsync()
        {
            if (Translation.ResourceID == 0)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.BadRequest, Target = Translation.CultureID });
            }

            if (string.IsNullOrWhiteSpace(Translation.CultureID))
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.BadRequest, Target = Translation.CultureID });
            }

            var entity = await DataManager.GetAsync<XLTranslation>(x => x.ResourceID == Translation.ResourceID && x.CultureID == Translation.CultureID);

            if (entity == null)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.NotFound, Target = Translation.CultureID });
            }

            return await DataManager.DeleteAsync<XLTranslation>(entity)
                ? new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.OK, Target = Translation.CultureID })
                : new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.InternalServerError, Target = Translation.CultureID });
        }

        public async Task<JsonResult> OnPostOnlineTranslateAsync(string provider, string text, string source, string target, string format)
        {
            var service = _translationServiceFactory.Create(provider);
            var result = await service.TranslateAsync(source, target, text, format);

            return new JsonResult(result);
        }
    }
}
