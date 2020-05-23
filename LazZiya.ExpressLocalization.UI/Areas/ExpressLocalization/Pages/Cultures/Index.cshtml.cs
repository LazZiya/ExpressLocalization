using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.EFGenericDataManager.Models;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Cultures
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        public IndexModel(IEFGenericDataManager manager)
        {
            DataManager = manager;
        }

        public ICollection<ExpressLocalizationCulture> SupportedCultures { get; set; }

        // Page number
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        // Page size
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;

        public int TotalRecords { get; set; } = 0;

        // Search keywords
        [BindProperty(SupportsGet = true)]
        public string Q { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public bool? Def { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Act { get; set; }

        public ICollection<CultureModel> SystemCultures { get; set; }

        public async void OnGet()
        {
            (SupportedCultures, TotalRecords) = await ListSupportedCulturesAsync();
        }

        public async Task<IActionResult> OnPostAddNewAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var culture = new ExpressLocalizationCulture
            {
                ID = name,
                EnglishName = CultureInfo.GetCultureInfo(name).EnglishName,
                IsDefault = false,
                IsActive = false
            };

            if (await DataManager.Count<ExpressLocalizationCulture>(x => x.ID == name) > 0)
                TempData.Warning("Culture already exists!");
            else if (await DataManager.AddAsync(culture))
                TempData.Success("New culture added");
            else
                TempData.Danger("Culture not added!");
            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<IActionResult> OnPostDeleteAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await DataManager.GetAsync<ExpressLocalizationCulture>(x => x.ID == name);
            if (entity == null)
            {
                TempData.Danger("Culture not found!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            if (entity.IsDefault)
            {
                TempData.Warning("Default culture can't be deleted!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            if (await DataManager.DeleteAsync(entity))
                TempData.Success("Deleted!");
            else
                TempData.Danger("Unknown error occord!");

            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<IActionResult> OnPostActivateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await DataManager.GetAsync<ExpressLocalizationCulture>(x => x.ID == name);
            if (entity == null)
            {
                TempData.Danger("Culture not found!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            if (entity.IsDefault && entity.IsActive)
            {
                TempData.Warning("Default culture can't be disabled!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            entity.IsActive = !entity.IsActive;

            if (await DataManager.UpdateAsync<ExpressLocalizationCulture, string>(entity))
            {
                if (entity.IsActive)
                    TempData.Success("Culture enabled!");
                else
                    TempData.Success("Culture disabled!");
            }
            else
                TempData.Danger("Unknown error occord!");

            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<IActionResult> OnPostSetDefaultAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await DataManager.GetAsync<ExpressLocalizationCulture>(x => x.ID == name);
            if (entity == null)
            {
                TempData.Danger("Culture not found!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            if (await DataManager.SetAsDefault<ExpressLocalizationCulture, string>(entity.ID))
                TempData.Success("Culture enabled and set as default!");
            else
                TempData.Danger("Unknown error occord!");

            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<(ICollection<ExpressLocalizationCulture> items, int total)> ListSupportedCulturesAsync()
        {
            var expList = new List<Expression<Func<ExpressLocalizationCulture, bool>>> { };
            if (!string.IsNullOrWhiteSpace(Q))
            {
                // split the search text
                var keyWords = Q.Split(new[] { ' ', ',', ':' });
#if NETCOREAPP2_0
                // add search expression
                expList.Add(x => keyWords.Any(kw => x.ID != null && x.ID.StartsWith(kw, StringComparison.OrdinalIgnoreCase)));
#else
                // add search expression
                expList.Add(x => keyWords.Any(kw => x.ID != null && x.ID.Contains(kw, StringComparison.OrdinalIgnoreCase)));
#endif
            }

            if (Def != null)
            {
                expList.Add(x => x.IsDefault == Def);
            }

            if (Act != null)
            {
                expList.Add(x => x.IsActive == Act);
            }

            var orderByList = new List<OrderByExpression<ExpressLocalizationCulture>> { };

            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.ID, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.IsDefault, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.IsActive, OrderByDir = OrderByDir.ASC });

            return await DataManager.ListAsync(P, S, expList, orderByList);
        }

        /// <summary>
        /// Return a list of cultures
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public JsonResult OnGetSystemCultures(string search)
        {
            var keyWords = search.Split(' ');
#if NETCOREAPP2_0
            SystemCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(x => keyWords.All(kw => x.Name.StartsWith(kw, StringComparison.OrdinalIgnoreCase) 
                        || x.EnglishName.StartsWith(kw, StringComparison.OrdinalIgnoreCase) 
                        || x.DisplayName.StartsWith(kw, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new CultureModel { ID = x.Name, Text = x.EnglishName }).ToList();
#else
            SystemCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                            .Where(x => keyWords.All(kw => x.Name.StartsWith(kw, StringComparison.OrdinalIgnoreCase)
                                    || x.EnglishName.Contains(kw, StringComparison.OrdinalIgnoreCase)
                                    || x.DisplayName.Contains(kw, StringComparison.OrdinalIgnoreCase)))
                            .Select(x => new CultureModel { ID = x.Name, Text = x.EnglishName }).ToList();
#endif
            return new JsonResult(SystemCultures);
        }
    }
}
