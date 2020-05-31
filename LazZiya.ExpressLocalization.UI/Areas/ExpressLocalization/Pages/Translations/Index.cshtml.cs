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

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Translations
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;
        private readonly IEnumerable<ITranslationService> TranslationServices;

        public readonly SelectListItem[] TranslationProviders;

        public IndexModel(IEFGenericDataManager dataManager, IEnumerable<ITranslationService> translationServices)
        {
            DataManager = dataManager;
            TranslationServices = translationServices;
            
            // get all registered translation services names
            TranslationProviders = translationServices.Select(x => x.ServiceName).OrderBy(x => x).Select(x => new SelectListItem() { Text = x, Value = x }).ToArray();
        }

        [BindProperty(SupportsGet = true)]
        public string DefaultCulture { get; set; }

        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;
        public int TotalRecords { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public int ResourceID { get; set; }

        public XLResource Resource { get; set; }

        [BindProperty]
        public XLTranslation Translation { get; set; }

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

            var include = new List<Expression<Func<XLResource, object>>> { x => x.Translations.OrderBy(t => t.CultureName).Skip((P - 1) * S).Take(S) };
            Resource = await DataManager.GetAsync<XLResource>(x => x.ID == ResourceID, include);

            if (Resource == null)
            {
                TempData.Danger("Resource not found!");
                return RedirectToPage("Index");
            }

            (Cultures, TotalRecords) = await DataManager.ListAsync<XLCulture>(1, int.MaxValue, null, null);

            DefaultCulture = (await DataManager.GetAsync<XLCulture>(x => x.IsDefault == true)).ID;

            return Page();
        }

        public async Task<JsonResult> OnPostSaveTranslationAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode= HttpStatusCode.BadRequest, Target = Translation.CultureName });
            }

            // get translation from DB
            var entity = await DataManager.GetAsync<XLTranslation>(x => x.ResourceID == Translation.ResourceID && x.CultureName == Translation.CultureName);

            bool success;

            if (entity == null)
            {
                // create a new translation if no record exist for this resource and culture 
                success = await DataManager.AddAsync<XLTranslation>(Translation);
            }
            else
            {
                // Update existing translation if record is exist for this resource and culture 
                entity.Value = Translation.Value;
                success = await DataManager.UpdateAsync<XLTranslation, int>(entity);
            }

            return success
                ? new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.OK, Target = Translation.CultureName })
                : new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.InternalServerError, Target = Translation.CultureName });
        }

        public async Task<JsonResult> OnPostDeleteTranslationAsync()
        {
            if (Translation.ResourceID == 0)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.BadRequest, Target = Translation.CultureName });
            }

            if (string.IsNullOrWhiteSpace(Translation.CultureName))
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.BadRequest, Target = Translation.CultureName });
            }

            var entity = await DataManager.GetAsync<XLTranslation>(x => x.ResourceID == Translation.ResourceID && x.CultureName == Translation.CultureName);

            if (entity == null)
            {
                return new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.NotFound, Target = Translation.CultureName });
            }

            return await DataManager.DeleteAsync<XLTranslation>(entity)
                ? new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.OK, Target = Translation.CultureName })
                : new JsonResult(new CRUDAjaxReponse { StatusCode = HttpStatusCode.InternalServerError, Target = Translation.CultureName });
        }

        public async Task<JsonResult> OnPostOnlineTranslateAsync(string provider, string text, string source, string target, string format)
        {
            var service = TranslationServices.FirstOrDefault(x => x.ServiceName == provider);
            var result = await service.TranslateAsync(source, target, text, format);

            return new JsonResult(result);
        }
    }
}
