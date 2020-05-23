using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// ExpressLocalization default culture model
    /// </summary>
    public class ExpressLocalizationCulture : IExpressLocalizationCulture
    {
        /// <summary>
        /// Culture name e.g. en, en-us, ..etc.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        
        /// <summary>
        /// Get or set a value if this is the default request culture
        /// </summary>
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// Get or set a value if this culture is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// English name of the culture (required for DB dearch)
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// A collection of resources with the same culture
        /// </summary>
        public virtual ICollection<ExpressLocalizationEntity> Resources { get; set; }
    }
}
