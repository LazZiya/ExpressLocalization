using Microsoft.AspNetCore.Authentication.Cookies;
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

        /// <summary>
        /// The path to the resources folder e.g. "LocalizationResources"
        /// </summary>
        public string ResourcesPath { get; set; } = "";

        /// <summary>
        /// Configure application cookie opitons,
        /// <para>mainly used to add culture value to redirect to login path and access denied path</para>
        /// <para>if not set manually, the default settings will be applied by adding culture parameter to the relevant paths</para>
        /// </summary>
        public Action<CookieAuthenticationOptions> CookieAuthenticationOptions { get; set; }
        /// <summary>
        /// Add culture parameter to login path.
        /// Done by configuring application cookie options event
        /// <para>default value = true</para>
        /// <para>set to false if you need to configure the application cookie options manually</para>
        /// </summary>
        public bool ConfigureRedirectToLoginPath { get; set; } = true;

        /// <summary>
        /// The default culture to set during redirection to login event if no culture was set.
        /// </summary>
        public string DefaultCultureName { get; set; } = "en";

        /// <summary>
        /// The default login path
        /// <para>default value = "/Identity/Account/Login/"</para>
        /// </summary>
        public string LoginPath { get; set; } = "/Identity/Account/Login/";
    }
}
