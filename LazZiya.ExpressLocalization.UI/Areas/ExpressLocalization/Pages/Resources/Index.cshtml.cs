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

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Resources
{
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        public IndexModel(IEFGenericDataManager dataManager)
        {
            DataManager = dataManager;
        }

        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;
        public int TotalRecords { get; set; } = 0;

        /// <summary>
        /// use for resource search 
        /// </summary>
        [BindProperty(SupportsGet =true)]
        public ResourceSearchModel SearchModel { get; set; }

        public ICollection<ResourceListItem> Resources { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            (Resources, TotalRecords) = await ListResourcesAsync();

            return Page();
        }

        private async Task<(ICollection<ResourceListItem> resources, int totalRecords)> ListResourcesAsync()
        {
            var searchExpressions = new List<Expression<Func<XLResource, bool>>> { };
            
            if (!string.IsNullOrWhiteSpace(SearchModel.Key))
            {
                searchExpressions.Add(x => x.Key != null && x.Key.Contains(SearchModel.Key));
            }
            
            if (!string.IsNullOrWhiteSpace(SearchModel.Comment))
            {
                searchExpressions.Add(x => x.Comment!=null && x.Comment.Contains(SearchModel.Comment));
            }

            if(SearchModel.ID != null && SearchModel.ID.Value > 0)
            {
                searchExpressions.Add(x => x.ID == SearchModel.ID);
            }

            var orderByList = new List<OrderByExpression<XLResource>> { };
            orderByList.Add(new OrderByExpression<XLResource> { Expression = x => x.Key, OrderByDir = OrderByDir.ASC });

            var includes = new List<Expression<Func<XLResource, object>>> { };
            includes.Add(x => x.Translations);

            Expression<Func<XLResource, ResourceListItem>> select = x => new ResourceListItem
            {
                ID = x.ID,
                Key = x.Key,
                Comment = x.Comment,
                Cultures = x.Translations.Select(t => t.CultureID).ToList()
            };

            return await DataManager.ListAsync(P, S, searchExpressions, orderByList, includes, select);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (id == 0)
            {
                TempData.Danger("Resource ID can't be empty");
                return RedirectToPage("Index");
            }

            var entity = await DataManager.GetAsync<XLResource>(x => x.ID == id);
            if (entity == null)
            {
                TempData.Danger("Resource not found!");
                return RedirectToPage("Index");
            }

            if (await DataManager.DeleteAsync(entity))
                TempData.Success("Deleted!");
            else
                TempData.Danger("Unknown error occord!");

            return RedirectToPage("Index");
        }
    }
}
