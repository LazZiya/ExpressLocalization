using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Pages.Resources
{
    public class CreateModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        public CreateModel(IEFGenericDataManager dataManager)
        {
            DataManager = dataManager;
        }

        [BindProperty]
        public ResourceInputModel InputModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if(await DataManager.Count<XLResource>(x => x.Key == InputModel.Key) > 0)
            {
                TempData.Warning("Resource already exists!");
                return Page();
            }

            var entity = new XLResource
            {
                Key = InputModel.Key,
                Comment = InputModel.Comment
            };

            if (await DataManager.AddAsync<XLResource>(entity))
                TempData.Success("Resource added.");
            else
                TempData.Danger("Resource not added");

            return RedirectToPage("Index");
        }
    }
}
