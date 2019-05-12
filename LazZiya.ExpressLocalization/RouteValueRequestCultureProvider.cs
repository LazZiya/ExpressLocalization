using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization
{
    public class RouteValueRequestCultureProvider : IRequestCultureProvider
    {
        private IList<CultureInfo> SupportedCultures { get; set; }

        private string DefaultCulture { get; set; }

        public RouteValueRequestCultureProvider(IList<CultureInfo> supportedCultures, string defaultCulture)
        {
            SupportedCultures = supportedCultures;
            DefaultCulture = defaultCulture;
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;

            if (string.IsNullOrWhiteSpace(path))
            {
                // Path is empty! returning default culture
                return Task.FromResult(new ProviderCultureResult(DefaultCulture));
            }

            var routeValues = httpContext.Request.Path.Value.Split('/');
            if (routeValues.Count() <= 1)
            {
                // No path parameter detected! returning default culture
                return Task.FromResult(new ProviderCultureResult(DefaultCulture));
            }

            if (!SupportedCultures.Any(x =>
                 x.TwoLetterISOLanguageName.ToLower() == routeValues[1].ToLower() ||
                 x.Name.ToLower() == routeValues[1].ToLower()))
            {
                // Path culture not ercognized! returning default culture
                return Task.FromResult(new ProviderCultureResult(DefaultCulture));
            }

            // culture selected successfuly
            return Task.FromResult(new ProviderCultureResult(routeValues[1]));
        }
    }
}
