using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using LazZiya.ExpressLocalization.ResxTools;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SampleProject.LocalizationResources;

namespace SampleProject.Pages
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHtmlLocalizer _loc;

        public IndexModel(ILogger<IndexModel> logger, IHtmlLocalizer localizer)
        {
            _logger = logger;

            _loc = localizer;
        }

        public void OnGet()
        {
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
