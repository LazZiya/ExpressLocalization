using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Detects the current culture result depending on currently registered culture providers
    /// </summary>
    public class ProviderCultureDetector
    {

        /// <summary>
        /// Detect the culture for the current request according to the available cultures and their availability in the registered culture providers, 
        /// if no culture is detected it will return default culture.
        /// Use to detect the current culture for cookie authentication events.
        /// </summary>
        /// <param name="providers">List of currently registered culture providers</param>
        /// <param name="httpContext">requests HttpContext</param>
        /// <param name="cultures">List of supported cultures</param>
        /// <param name="defCulture">default culture name</param>
        /// <returns>ProviderCultureResult</returns>
        public static async Task<string> DetectCurrentCulture(IList<IRequestCultureProvider> providers, HttpContext httpContext, IList<CultureInfo> cultures, string defCulture)
        {
            foreach (var p in providers)
            {
                var availableCultures = await p.DetermineProviderCultureResult(httpContext);

                if (availableCultures != null)
                {
                    foreach (var c in availableCultures.Cultures)
                    {
                        var available = cultures.FirstOrDefault(x => x.Name == c);
                        if (available != null)
                            return available.Name;
                    }
                }
            }

            return defCulture;
        }
    }
}
