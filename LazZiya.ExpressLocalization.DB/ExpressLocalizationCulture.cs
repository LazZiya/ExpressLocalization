using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Interface to implement ExpressLocalization custom culture model
    /// </summary>
    /// <typeparam name="TKey">IEquatable key type for ExpressLocalization entity model (int, string, ...etc.)</typeparam>
    public interface IExpressLocalizationCulture<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Culture name e.g. en, en-us, ..etc.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        string Name { get; set; }

        /// <summary>
        /// Get or set a value if this is the default request culture
        /// </summary>
        bool IsDefault { get; set; }

        /// <summary>
        /// Get or set a value if this culture is active
        /// </summary>
        bool IsActive { get; set; }
    }

    /// <summary>
    /// ExpressLocalization default culture model
    /// </summary>
    /// <typeparam name="TKey">IEquatable ID type for ExpressLocalization entity model (int, string, ...etc.)</typeparam>
    public class ExpressLocalizationCulture<TKey> : IExpressLocalizationCulture<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Culture name e.g. en, en-us, ..etc.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
        
        /// <summary>
        /// Get or set a value if this is the default request culture
        /// </summary>
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// Get or set a value if this culture is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// List of relevant entities (localized resources) for the culture
        /// </summary>
        public virtual ICollection<ExpressLocalizationEntity<TKey>> Items { get; set; }
    }
}
