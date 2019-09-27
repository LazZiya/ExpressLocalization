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
        /// Optional : Add culture parameter to login, logout and access denied paths.
        /// <para>default value = true</para>
        /// <para>set to false if you need to configure the application cookie options manually</para>
        /// </summary>
        public bool ConfigureRedirectPaths { get; set; } = true;

        /// <summary>
        /// The default culture to set during redirection to login event if no culture was set.
        /// </summary>
        public string DefaultCultureName { get; set; } = "en";

        /// <summary>
        /// The default login path
        /// <para>default value = "/Identity/Account/Login/"</para>
        /// </summary>
        public string LoginPath { get; set; } = "/Identity/Account/Login/";
        
        /// <summary>
        /// The default logout path
        /// <para>default value = "/Identity/Account/Logout/"</para>
        /// </summary>
        public string LogoutPath { get; set; } = "/Identity/Account/Logout/";
        
        /// <summary>
        /// The default access denied path
        /// <para>default value = "/Identity/Account/AccessDenied/"</para>
        /// </summary>
        public string AccessDeniedPath { get; set; } = "/Identity/Account/AccessDenied/";
    }
}
