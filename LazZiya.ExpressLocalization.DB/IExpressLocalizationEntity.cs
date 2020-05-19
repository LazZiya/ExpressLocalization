using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Interface to implement custom ExpressLocalization model
    /// </summary>
    /// <typeparam name="TKey">IEquatable ID type for ExpressLocalization entity model (int, string, ...etc.)</typeparam>
    public interface IExpressLocalizationEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// ID
        /// </summary>
        TKey ID { get; set; }
        
        /// <summary>
        /// The resource key
        /// </summary>
        string Key { get; set; }
        
        /// <summary>
        /// The resource localized value
        /// </summary>
        string Value { get; set; }
        
        /// <summary>
        /// Comment if any...
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Relevant culture name, must be the key of ExpressLocalizationCulture model
        /// </summary>
        string CultureName { get; set; }
    }

    /// <summary>
    /// ExpressLocalization default entity model
    /// </summary>
    /// <typeparam name="TKey">IEquatable ID type for ExpressLocalization entity model (int, string, ...etc.)</typeparam>
    public class ExpressLocalizationEntity<TKey> : IExpressLocalizationEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// ID
        /// </summary>
        public TKey ID { get; set; }
        
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
        public ExpressLocalizationCulture<TKey> Culture { get; set; }
    }
}
