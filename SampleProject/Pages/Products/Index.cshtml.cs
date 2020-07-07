using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace SampleProject.Pages.Products
{
    public class IndexModel : PageModel
    {
        private IStringLocalizer localizer;

        public IndexModel(IStringLocalizerFactory factory)
        {
            localizer = factory.Create(typeof(IndexModel));
        }
        public void OnGet()
        {
            var loc = localizer["Welcome dear"];
            TempData.Success($"{loc}, {typeof(IndexModel).Assembly.GetName().Name}");
        }
    }
}
