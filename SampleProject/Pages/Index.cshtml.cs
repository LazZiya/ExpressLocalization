using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using LazZiya.ExpressLocalization;
using LazZiya.ExpressLocalization.Common;
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

        public IndexModel(ILogger<IndexModel> logger, IHtmlExpressLocalizerFactory factory)
        {
            _logger = logger;

            _loc = factory.Create();
        }

        public void OnGet()
        {
            using(var cs = new CultureSwitcher("tr"))
                TempData.Success(_loc["Welcome"].Value);
        }

        public async void OnPostAsync()
        {
            var xdo = XDocument.Load(@".\LocalizationResources\DummyResource.tr.xml");
            var resxMan = new ResxManager(typeof(LocSource), "LocalizationResources", CultureInfo.CurrentCulture.Name);
            var elements = xdo.Root.Elements("data").Select(x => new ResxElement { Key = x.Attribute("name").Value, Value = x.Element("value").Value, Comment = x.Element("comment").Value }).ToList();
            var added = await resxMan.AddRangeAsync(elements);
            if (added > 0)
            {
                var success = await resxMan.SaveAsync();
                _logger.LogInformation("Transfer complete status : " + added + " - " + success);
            }
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
