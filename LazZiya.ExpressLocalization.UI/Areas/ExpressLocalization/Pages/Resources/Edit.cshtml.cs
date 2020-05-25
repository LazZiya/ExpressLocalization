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
    public class EditModel : PageModel
    {
        private readonly IEFGenericDataManager DataManager;

        public EditModel(IEFGenericDataManager dataManager)
        {
            DataManager = dataManager;
        }

        [BindProperty]
        public ResourceInputModel InputModel { get; set; }

        [BindProperty(SupportsGet =true)]
        public int ID { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var entity = await DataManager.GetAsync<XLResource>(x => x.ID == ID);

            if(entity == null)
            {
                TempData.Warning("Resource not found!");
                return RedirectToPage("Index");
            }

            InputModel = new ResourceInputModel
            {
                Key = entity.Key,
                Comment = entity.Comment
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if(await DataManager.Count<XLResource>(x => x.Key == InputModel.Key && x.ID != ID) > 0)
            {
                TempData.Warning("Resource already exists!");
                return Page();
            }

            var entity = new XLResource
            {
                ID = ID,
                Key = InputModel.Key,
                Comment = InputModel.Comment
            };

            if (await DataManager.UpdateAsync<XLResource, int>(entity))
                TempData.Success("Resource updated.");
            else
                TempData.Danger("Resource not updated");

            return RedirectToPage("Index");
        }
    }
}
