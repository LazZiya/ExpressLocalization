using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// ExpressLocalization default entity model
    /// </summary>
    public class XLResource : IXLResource
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// The resource key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Comment if any...
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Collection of translations
        /// </summary>
        public ICollection<IXLTranslation> Translations { get; set; }
    }
}
