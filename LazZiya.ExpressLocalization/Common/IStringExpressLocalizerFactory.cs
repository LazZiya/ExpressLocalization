using Microsoft.Extensions.Localization;

namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Interface to create IStringExpressLocalizerFactory
    /// </summary>
    public interface IStringExpressLocalizerFactory : IStringLocalizerFactory
    {
        /// <summary>
        /// Create new StringLocalizer based on default type
        /// </summary>
        /// <returns></returns>
        IStringLocalizer Create();
    }
}
