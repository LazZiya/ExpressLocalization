using LazZiya.EFGenericDataManager.Models;

namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Interface for express localization culture
    /// </summary>
    public interface IExpressLocalizationCulture : IHasId<string>, IDefault, IActive
    {
        /// <summary>
        /// English name of the culture
        /// </summary>
        string EnglishName { get; set; }
    }
}
