using LazZiya.EFGenericDataManager.Models;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface to implement ExpressLocalization resource translation models
    /// </summary>
    public interface IXLTranslation : IHasId<int>, IActive
    {
        /// <summary>
        /// The resource localized value
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Express Localization resource ID
        /// </summary>
        int ResourceID { get; set; }

        /// <summary>
        /// Relevant culture name, must be the key of ExpressLocalizationCulture model
        /// </summary>
        string CultureID { get; set; }
    }
}
