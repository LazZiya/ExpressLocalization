using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Express Localization translation
    /// </summary>
    public class XLTranslation : IXLTranslation
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
        /// Relevant culture name
        /// </summary>
        [ForeignKey("Culture")]
        public string CultureName { get; set; }

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
        public XLResource Resource { get; set; }
    }
}
