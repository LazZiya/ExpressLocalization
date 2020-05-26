using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Translations
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;
        private readonly IConfiguration Configuration;

        public IndexModel(IEFGenericDataManager dataManager, IConfiguration configuration)
        {
            DataManager = dataManager;
            Configuration = configuration;
        }

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

        public async Task<ContentResult> OnPostYandexTranslateAsync(string text, string source, string target, string format)
        {
            // right click on the main project
            // select "Manage use secrets"
            // add "yandex-translate-api-key" to user secrets
            var yandexkey = Configuration["yandex-translate-api-key"];

            if (string.IsNullOrWhiteSpace(yandexkey))
            {
                throw new NullReferenceException(nameof(yandexkey));
            }

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync($"https://translate.yandex.net/api/v1.5/tr.json/translate?key={yandexkey}&text={text}&lang={source}-{target}&format={format}");
                var _txt = await result.Content.ReadAsStringAsync();
                return Content(_txt);
            }
        }

        public async Task<ContentResult> OnPostGoogleTranslateAsync(string text, string source, string target, string format)
        {
            return Content("Google: " + text);
        }

        public async Task<ContentResult> OnPostMicrosoftTranslateAsync(string text, string source, string target, string format)
        {
            return Content("Microsoft: " + text);
        }
    }
}
