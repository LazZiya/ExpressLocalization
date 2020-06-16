using Microsoft.AspNetCore.Mvc.Localization;

namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Interface to create IHtmlExpressLocalizerFactory
    /// </summary>
    public interface IHtmlExpressLocalizerFactory : IHtmlLocalizerFactory
    {
        /// <summary>
        /// Create new HtmlLocalizer based on default type and default translation service
        /// </summary>
        /// <returns></returns>
        IHtmlLocalizer Create();
    }
}
