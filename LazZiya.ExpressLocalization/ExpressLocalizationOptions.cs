using Microsoft.AspNetCore.Builder;
using System;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Express localization options
    /// </summary>
    public class ExpressLocalizationOptions
    {
        /// <summary>
        /// Specific options for the Mİcrosoft.AspNetCore.Localizarion.RequestLocalizatonMiddleware
        /// </summary>
        public Action<RequestLocalizationOptions> RequestLocalizationOptions { get; set; }
    }
}
