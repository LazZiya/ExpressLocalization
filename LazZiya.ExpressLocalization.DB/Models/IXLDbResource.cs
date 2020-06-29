using LazZiya.EFGenericDataManager.Models;
using System.Collections.Generic;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface to implement ExpressLocalization resources
    /// </summary>
    public interface IXLDbResource : IHasId<int>
    {
        /// <summary>
        /// The resource key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Comment if any...
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Collection of translations
        /// </summary>
        ICollection<IXLDbTranslation> Translations { get; set; }
    }
}
