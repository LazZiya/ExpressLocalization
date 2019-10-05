using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Detects the current culture result depending on currently registered culture providers
    /// </summary>
    public class ProviderCultureDetector
    {
        private readonly RequestLocalizationOptions _ops;

        /// <summary>
        /// Detects the current culture result depending on currently registered culture providers
        /// </summary>
        public ProviderCultureDetector(RequestLocalizationOptions ops)
        {
            _ops = ops;
        }

        /// <summary>
        /// Detect current culture according to the registered culture providers, if no culture is detected it will return default culture.
        /// </summary>
        /// <param name="httpContext">requests HttpContext</param>
        /// <returns>ProviderCultureResult</returns>
        public async Task<ProviderCultureResult> DetectCurrentCulture(HttpContext httpContext)
        {
            foreach(var p in _ops.RequestCultureProviders)
            {
                var culture = await p.DetermineProviderCultureResult(httpContext);

                if (culture != null)
                    return culture;
            }

            return new ProviderCultureResult(_ops.DefaultRequestCulture.Culture.Name);
        }
    }
}
