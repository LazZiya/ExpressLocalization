using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Culture translations page
    /// </summary>
    public class TranslationsModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        /// <summary>
        /// Initialize culture translations page
        /// </summary>
        /// <param name="dataManager"></param>
        public TranslationsModel(IEFGenericDataManager dataManager)
        {
            DataManager = dataManager;
        }

        /// <summary>
        /// Page no
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;

        /// <summary>
        /// Total records
        /// </summary>
        public int TotalRecords { get; set; } = 0;

        /// <summary>
        /// Culture name to retrive translations for
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string CultureID { get; set; }

        /// <summary>
        /// Search by resource key 
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string QKey { get; set; }

        /// <summary>
        /// Search by translation
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string QValue { get; set; }

        /// <summary>
        /// Search by ID
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int? QID { get; set; }

        /// <summary>
        /// List of translations
        /// </summary>
        public ICollection<TranslationItemModel> Translations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(CultureID))
            {
                TempData.Danger("Culture name can't be empty!");
                return Page();
            }

            if (await DataManager.Count<XLCulture>(x => x.ID == CultureID) == 0)
            {
                TempData.Danger("Culture not found!");
                return Page();
            }

            (Translations, TotalRecords) = await ListTranslationsAsync();

            return Page();
        }

        public async Task<(ICollection<TranslationItemModel> items, int total)> ListTranslationsAsync()
        {
            var searchExpressions = new List<Expression<Func<XLTranslation, bool>>> { };
            searchExpressions.Add(x => x.CultureID == CultureID);

            if (QID != null && QID.Value != 0)
            {
                searchExpressions.Add(x => x.ID == QID);
            }

            if (!string.IsNullOrWhiteSpace(QKey))
            {
                searchExpressions.Add(x => x.Resource.Key != null && x.Resource.Key.Contains(QKey) || x.Resource.Comment != null && x.Resource.Comment.Contains(QKey));
            }

            if (!string.IsNullOrWhiteSpace(QValue))
            {
                searchExpressions.Add(x => x.Value != null && x.Value.Contains(QValue));
            }

            var includes = new List<Expression<Func<XLTranslation, object>>> { };
            includes.Add(x => x.Resource);
            
            var orderBy = new List<OrderByExpression<XLTranslation>> { };
            orderBy.Add(new OrderByExpression<XLTranslation> { Expression = x => x.Value, OrderByDir = OrderByDir.ASC });

            Expression<Func<XLTranslation, TranslationItemModel>> select = x => new TranslationItemModel
            {
                ID = x.ID,
                Key = x.Resource.Key.Length > 50 ? x.Resource.Key.Substring(0, 50) + " ..." : x.Resource.Key,
                Value = x.Value.Length > 50 ? x.Value.Substring(0, 50) + " ..." : x.Value,
                KeyID = x.ResourceID,
                CultureID = x.CultureID
            };

            return await DataManager.ListAsync(P, S, searchExpressions, orderBy, includes, select);
        }
    }
}
