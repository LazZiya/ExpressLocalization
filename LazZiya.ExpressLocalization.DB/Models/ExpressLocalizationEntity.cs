using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// ExpressLocalization default entity model
    /// </summary>
    public class ExpressLocalizationEntity : IExpressLocalizationEntity
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
        /// The resource localized value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Comment if any...
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Relevant culture name
        /// </summary>
        [ForeignKey("Culture")]
        public string CultureName { get; set; }
        
        /// <summary>
        /// Relevant ExpressLocalization culture entity
        /// </summary>
        public ExpressLocalizationCulture Culture { get; set; }
    }
}
