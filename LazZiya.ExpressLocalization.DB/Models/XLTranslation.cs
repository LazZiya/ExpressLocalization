using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Express Localization translation
    /// </summary>
    public class XLTranslation : IXLDbTranslation
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The resource localized value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Culture ID (two letter ISO name)
        /// </summary>
        [ForeignKey("Culture")]
        public string CultureID { get; set; }

        /// <summary>
        /// Relevant ExpressLocalization culture entity
        /// </summary>
        public XLCulture Culture { get; set; }

        /// <summary>
        /// Express Localization resource ID
        /// </summary>
        [ForeignKey("Resource")]
        public int ResourceID { get; set; }

        /// <summary>
        /// Express Localization Resource
        /// </summary>
        public IXLDbResource Resource { get; set; }

        /// <summary>
        /// Enable, disable translation.
        /// Use for marking auto-translations before approval as false
        /// </summary>
        public bool IsActive { get; set; }
    }
}
