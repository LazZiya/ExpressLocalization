using System;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Define options for XL DB localization
    /// </summary>
    public class XLDbOptions
    {
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

        /// <summary>
        /// Online translations is marked as false by default, and it should be approved manually to be set to true.
        /// Enable this option to serve unapproved translations we well.
        /// </summary>
        public bool ServeUnapprovedTranslations { get; set; } = false;
    }
}
