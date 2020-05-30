using LazZiya.TranslationServices;

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
        public bool KeysRecursiveMode { get; set; } = false;
        
        /// <summary>
        /// If the translation string is not found in the DB, it will be inserted autoamtically to the DB.
        /// default: false
        /// </summary>
        public bool TranslationRecursiveMode { get; set; } = false;

        /// <summary>
        /// Dummy entity to be inserted while recursive mode is enabled
        /// </summary>
        public IXLResource DummyResourceEntity { get; set; }

        /// <summary>
        /// Dummy entity to create translations for recursive mode
        /// </summary>
        public IXLTranslation DummyTranslationEntity { get; set; }

        /// <summary>
        /// Translation service name for recursive mode.
        /// Default is "Yandex"
        /// </summary>
        public string TranslationServiceName { get; set; }
    }
}
