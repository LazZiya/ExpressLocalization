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
        /// IMPORTANT: This property setup moved to startup. For more details see <a href="https://github.com/LazZiya/ExpressLocalization/wiki/Migration-4.x-to-5.0\">Migration 4.x to 5.0</a>
        /// </summary>
        [Obsolete("This property is deprected. Please use regular setup instead. See <a href=\"https://github.com/LazZiya/ExpressLocalization/wiki/Migration-4.x-to-5.0\">Migration 4.x to 5.0</a>")]
        public Action<RequestLocalizationOptions> RequestLocalizationOptions { get; set; }

        /// <summary>
        /// IMPORTANT: This property setup moved to startup. For more details see <a href="https://github.com/LazZiya/ExpressLocalization/wiki/Migration-4.x-to-5.0\">Migration 4.x to 5.0</a>
        /// </summary>
        [Obsolete("This property is deprected. See <a href=\"https://github.com/LazZiya/ExpressLocalization/wiki/Migration-4.x-to-5.0\">Migration 4.x to 5.0</a>")]
        public bool UseAllCultureProviders { get; set; } = true;

        /// <summary>
        /// The path to the resources folder e.g. "LocalizationResources"
        /// </summary>
        public string ResourcesPath { get; set; } = "LocalizationResources";

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
        /// Serve auto online translations. False by default
        /// </summary>
        public bool ServeUnApprovedTranslations { get; set; } = false;
    }
}
