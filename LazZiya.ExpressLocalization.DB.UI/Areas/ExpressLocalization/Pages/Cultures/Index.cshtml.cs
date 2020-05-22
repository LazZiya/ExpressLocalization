using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LazZiya.EFGenericDataManager;
using LazZiya.EFGenericDataManager.Models;
using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LazZiya.ExpressLocalization.DB.UI.Areas.ExpressLocalization.Pages.Cultures
{
    public class IndexModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        public IndexModel(IEFGenericDataManager manager)
        {
            DataManager = manager;
        }

        public ICollection<ExpressLocalizationCulture> Cultres { get; set; }

        // Page number
        [BindProperty(SupportsGet =true)]
        public int P { get; set; } = 1;

        // Page size
        [BindProperty(SupportsGet =true)]
        public int S { get; set; }

        public int TotalRecords { get; set; }

        // Search keywords
        [BindProperty(SupportsGet = true)]
        public string Q { get; set; }

        [BindProperty(SupportsGet =true)]
        public bool? Def { get; set; }
        
        [BindProperty(SupportsGet =true)]
        public bool? Act { get; set; }

        public async void OnGet()
        {
            var expList = new List<Expression<Func<ExpressLocalizationCulture, bool>>> { };
            if (Q != null)
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

            if(Act != null)
            {
                expList.Add(x => x.IsActive == Act);
            }

            var orderByList = new List<OrderByExpression<ExpressLocalizationCulture>> { };

            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.ID, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.IsDefault, OrderByDir = OrderByDir.ASC });
            orderByList.Add(new OrderByExpression<ExpressLocalizationCulture> { Expression = x => x.IsActive, OrderByDir = OrderByDir.ASC });

            (Cultres, TotalRecords) = await DataManager.ListAsync(P, S, expList, orderByList);
        }
    }
}
