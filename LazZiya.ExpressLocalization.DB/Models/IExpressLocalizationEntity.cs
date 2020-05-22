using LazZiya.EFGenericDataManager.Models;
using System;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface to implement custom ExpressLocalization model
    /// </summary>
    public interface IExpressLocalizationEntity : IHasId<int>
    {
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
}
