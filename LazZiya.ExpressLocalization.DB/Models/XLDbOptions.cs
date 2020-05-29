using LazZiya.TranslationServices;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Define options for XL DB localization
    /// </summary>
    public class XLDbOptions
    {
        /// <summary>
        /// If the key string is not found in the DB it will be inserted autoamtically to the DB.
        /// Enabling recursive mode will cause a bit more processing cycles, that mean the request will take a bit longer to process.
        /// default: false
        /// </summary>
        public RecursiveMode RecursiveMode { get; set; } = RecursiveMode.None;

        /// <summary>
        /// Dummy entity to be inserted while recursive mode is enabled
        /// </summary>
        public IXLResource DummyResourceEntity { get; set; }

        /// <summary>
        /// Dummy entity to create translations for recursive mode
        /// </summary>
        public IXLTranslation DummyTranslationEntity { get; set; }

        /// <summary>
        /// Translation provider for recursive mode.
        /// Default is "Yandex"
        /// </summary>
        public TranslationProvider TranslationProvider { get; set; } = TranslationProvider.Yandex;
    }
}
