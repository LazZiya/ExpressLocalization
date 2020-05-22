using LazZiya.EFGenericDataManager.Models;
using System;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface for express localization culture
    /// </summary>
    public interface IExpressLocalizationCulture : IHasId<string>, IDefault
    {
        /// <summary>
        /// Indicate if the current culture is enabled or disabled
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// English name of the culture
        /// </summary>
        string EnglishName { get; set; }
    }
}
