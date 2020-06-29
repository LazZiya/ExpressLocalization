using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.EFGenericDataManager.Models;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.ResxTools;
using LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Cultures
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager _dataManager;
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;
        private readonly ExpressLocalizationOptions _options;
        public IndexModel(IEFGenericDataManager manager, IApplicationLifetime lifetime, IOptions<ExpressLocalizationOptions> options, ILogger<IndexModel> logger)
        {
            _dataManager = manager;
            _applicationLifetime = lifetime;
            _options = options.Value;
            _logger = logger;
        }

        public IActionResult OnPostRestartApp()
        {
            _applicationLifetime.StopApplication();
            return new EmptyResult();
        }

        public ICollection<XLCulture> SupportedCultures { get; set; }

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

        public ICollection<CultureItemModel> SystemCultures { get; set; }

        // total number of items exported to resource file
        private int GrossTotalNewItems { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            (SupportedCultures, TotalRecords) = await ListSupportedCulturesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddNewAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var culture = new XLCulture
            {
                ID = ID,
                EnglishName = CultureInfo.GetCultureInfo(ID).EnglishName,
                IsDefault = false,
                IsActive = false
            };

            if (await _dataManager.Count<XLCulture>(x => x.ID == ID) > 0)
                TempData.Warning("Culture already exists!");
            else if (await _dataManager.AddAsync(culture))
                TempData.Success("New culture added");
            else
                TempData.Danger("Culture not added!");
            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<IActionResult> OnPostDeleteAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await _dataManager.GetAsync<XLCulture>(x => x.ID == ID);
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

            if (await _dataManager.DeleteAsync(entity))
                TempData.Success("Deleted!");
            else
                TempData.Danger("Unknown error occord!");

            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<IActionResult> OnPostActivateAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await _dataManager.GetAsync<XLCulture>(x => x.ID == ID);
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

            if (await _dataManager.UpdateAsync<XLCulture, string>(entity))
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

        public async Task<IActionResult> OnPostSetDefaultAsync(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                TempData.Danger("Culture name can't be empty");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            var entity = await _dataManager.GetAsync<XLCulture>(x => x.ID == ID);
            if (entity == null)
            {
                TempData.Danger("Culture not found!");
                return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
            }

            if (await _dataManager.SetAsDefault<XLCulture, string>(entity.ID))
                TempData.Success("Culture enabled and set as default!");
            else
                TempData.Danger("Unknown error occord!");

            return LocalRedirect(Url.Page("Index", new { area = "ExpressLocalization" }));
        }

        public async Task<(ICollection<XLCulture> items, int total)> ListSupportedCulturesAsync()
        {
            var expList = new List<Expression<Func<XLCulture, bool>>> { };
            if (!string.IsNullOrWhiteSpace(Q))
            {
                expList.Add(x => x.ID != null && x.ID.Contains(Q) || x.EnglishName != null && x.EnglishName.Contains(Q));
            }

            if (Def != null)
            {
                expList.Add(x => x.IsDefault == Def);
            }

            if (Act != null)
            {
                expList.Add(x => x.IsActive == Act);
            }

            var orderByList = new List<OrderByExpression<XLCulture>> { };

            orderByList.Add(new OrderByExpression<XLCulture> { Expression = x => x.ID, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<XLCulture> { Expression = x => x.IsDefault, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<XLCulture> { Expression = x => x.IsActive, OrderByDir = OrderByDir.ASC });

            return await _dataManager.ListAsync(P, S, expList, orderByList);
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
                        || x.NativeName.StartsWith(kw, StringComparison.OrdinalIgnoreCase)
                        || x.DisplayName.StartsWith(kw, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new CultureItemModel { ID = x.Name, Text = x.EnglishName }).ToList();
#else
            SystemCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                            .Where(x => keyWords.All(kw => x.Name.StartsWith(kw, StringComparison.OrdinalIgnoreCase)
                                    || x.EnglishName.Contains(kw, StringComparison.OrdinalIgnoreCase)
                                    || x.NativeName.Contains(kw, StringComparison.OrdinalIgnoreCase)
                                    || x.DisplayName.Contains(kw, StringComparison.OrdinalIgnoreCase)))
                            .Select(x => new CultureItemModel { ID = x.Name, Text = x.EnglishName }).ToList();
#endif
            return new JsonResult(SystemCultures);
        }

        /// <summary>
        /// Generate resx file for the selected culture
        /// </summary>
        /// <param name="id">Culture id (two letter name)</param>
        /// <param name="approvedOnly">export all or only approved items</param>
        /// <param name="overwrite">overwrite existing items</param>
        /// <returns></returns>
        /*public async Task<IActionResult> OnPostGenerateResxFile(string id, bool approvedOnly, bool overwrite)
        {
            var searchExp = new List<Expression<Func<XLTranslation, bool>>> { };
            searchExp.Add(x => x.CultureID == id);

            if (approvedOnly)
            {
                searchExp.Add(x => x.IsActive == true);
            }

            var orderByExp = new List<OrderByExpression<XLTranslation>> { };
            orderByExp.Add(new OrderByExpression<XLTranslation> { Expression = x => x.ID, OrderByDir = OrderByDir.ASC });

            Expression<Func<XLTranslation, ResxElement>> select = x => new ResxElement
            {
                Key = x.Resource.Key,
                Comment = x.Resource.Comment,
                Value = x.Value
            };

            var budleSize = 100;

            var newElements = await Export(1, budleSize, id, overwrite, searchExp, orderByExp, select);

            if (newElements != null)
                TempData.Success($"Exporting to resource file finished successfully. Total new items {newElements}.");
            else
                TempData.Danger("Error while creating resource file!");

            return RedirectToPage("Index");
        }*/
        /*
        private async Task<int?> Export(int p, int s, string cultureId, bool overwrite,
            List<Expression<Func<XLTranslation, bool>>> searchExp,
            List<OrderByExpression<XLTranslation>> orderByExp,
            Expression<Func<XLTranslation, ResxElement>> select)
        {
            (var items, var resxElementCount) = await _dataManager.ListAsync<XLTranslation, ResxElement>(p, s, searchExp, orderByExp, null, select);

            // Create resx manager that will do element addition operation
            var resxManager = new ResxManager(_options.ResourceType, _options.ResourcesPath, cultureId);

            // add range of elements and return total number of affected rows
            var totalNew = await resxManager.AddRangeAsync(items, overwrite);
            _logger.LogInformation($"Total new {totalNew}");

            // add new total to the gross total
            GrossTotalNewItems += totalNew;
            _logger.LogInformation($"Total gross {GrossTotalNewItems}");

            // continue exporting next bundle if any...
            if (resxElementCount > (p * s))
                await Export(p++, s, cultureId, overwrite, searchExp, orderByExp, select);

            // save the resource file for changes to take effect
            var saved = GrossTotalNewItems > 0 ? await resxManager.SaveAsync() : false;

            if (GrossTotalNewItems > 0 && !saved)
            {
                _logger.LogError("Error saving resource file!");
                return null;
            }

            _logger.LogInformation($"Resource file '{resxManager.TargetResourceFile}' - Gross Total New items {GrossTotalNewItems} - Save process {saved}.");

            return GrossTotalNewItems;
        }*/
    }
}
