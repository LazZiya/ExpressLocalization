using LazZiya.EFGenericDataManager.Models;
using System.Collections;
using System.Collections.Generic;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface for Express Localization culture
    /// </summary>
    public interface IXLCulture : IHasId<string>, IDefault, IActive
    {
        /// <summary>
        /// English name of the culture
        /// </summary>
        string EnglishName { get; set; }

        /// <summary>
        /// Collection of translations
        /// </summary>
        ICollection<IXLDbTranslation> Translations { get; set; }
    }
}
