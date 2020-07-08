using System;
using System.Globalization;
using System.Threading.Tasks;
using LazZiya.ExpressLocalization.ResxTools;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using SampleProject.LocalizationResources;

namespace SampleProject.Pages
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer localizer;

        public IndexModel(IStringLocalizerFactory factory)
        {
            //localizer = factory.Create(typeof(IndexModel));
            localizer = factory.Create("SampleProject.Pages.IndexModel", "SampleProject");
        }

        public void OnGet()
        {
            var loc = localizer["Welcome home"];
            TempData.Success($"{loc}, {loc.SearchedLocation}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resxMan = new ResxManager(typeof(LocSource), "LocalizationResources", CultureInfo.CurrentCulture.Name);
            var done = await resxMan.ToResxAsync(false);
            TempData.Info($"Total transferred items: {done}");
            return Page();
        }

        /// <summary>
        /// Set culture cookie value
        /// </summary>
        /// <param name="cltr">Culture name</param>
        /// <param name="returnUrl">The url to return to after setting the cookie</param>
        public IActionResult OnGetSetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
