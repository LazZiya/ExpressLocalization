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
        /// Optional, default value true.
        ///<para>true to register all culture providers (Route, QueryString, Cookie, AcceptLanguageHeader) </para>
        ///<para>false to use only Route culture provider.</para>
        /// </summary>
        public bool UseAllCultureProviders { get; set; } = true;

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
        /// The default culture to use for online translation.
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

        /// <summary>
        /// Express valdiation attributes provides already localized error messages.
        /// Set to true by default. 
        /// Set to false if you don't want to use express validation attributes.
        /// </summary>
        public bool UseExpressValidationAttributes { get; set; } = true;

        /// <summary>
        /// If the key string is not found in the DB, it will be inserted autoamtically to the DB.
        /// default: false
        /// </summary>
        public bool AutoAddKeys { get; set; } = false;

        /// <summary>
        /// If the translation string is not found in the DB, it will be inserted autoamtically to the DB.
        /// default: false
        /// </summary>
        public bool OnlineTranslation { get; set; } = false;

        /// <summary>
        /// Type of the translation service to use for OnlineLocalization
        /// </summary>
        public Type TranslationService { get; set; }
    }
}
