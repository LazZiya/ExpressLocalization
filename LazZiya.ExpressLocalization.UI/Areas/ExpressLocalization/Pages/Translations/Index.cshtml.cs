using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.DB.TranslationTools;
using LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Translations
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;
        private readonly IXLTranslateApiClient xlClient;
        

        public IndexModel(IEFGenericDataManager dataManager, IXLTranslateApiClient client)
        {
            DataManager = dataManager;
            xlClient = client;
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

        public async Task<ContentResult> OnPostSaveTranslationAsync()
        {
            if (!ModelState.IsValid)
            {
                return Content("Wrong input!");
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


            return success ? Content("Saved") : Content("Not saved!");
        }

        public async Task<StatusCodeResult> OnPostDeleteTranslationAsync()
        {
            if (Translation.ResourceID == 0)
            {
                return StatusCode(400);
            }

            if (string.IsNullOrWhiteSpace(Translation.CultureName))
            {
                return StatusCode(400);
            }

            var entity = await DataManager.GetAsync<XLTranslation>(x => x.ResourceID == Translation.ResourceID && x.CultureName == Translation.CultureName);

            if (entity == null)
            {
                return StatusCode(404);
            }

            return await DataManager.DeleteAsync<XLTranslation>(entity)
                ? StatusCode(200)
                : StatusCode(500);
        }

        public async Task<ContentResult> OnPostOnlineTranslateAsync(TranslationProvider provider, string text, string source, string target, string format)
        {
            string result;
            switch (provider)
            {
                case TranslationProvider.Google:
                    result = await xlClient.GoogleTranslateAsync(source, target, text, format);
                    break;

                case TranslationProvider.Yandex:
                    result = await xlClient.YandexTranslateAsync(source, target, text, format);
                    break;

                case TranslationProvider.MyMemory:
                    result = await xlClient.MyMemoryTranslateAsync(source, target, text);
                    break;

                case TranslationProvider.Systran:
                    result = await xlClient.SystranTranslateAsync(source, target, text);
                    break;

                default: result = "no provider..."; break;
            }

            return Content(result);
        }
    }
}
