using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlHtmlLocalizerFactory
    /// </summary>
    public class XmlHtmlLocalizerFactory<T> : IHtmlExpressLocalizerFactory
        where T : IXmlResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        public XmlHtmlLocalizerFactory(IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new XmlHtmlLocalizer(typeof(T), _options);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            return new XmlHtmlLocalizer(resourceSource, _options);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return new XmlHtmlLocalizer(baseName, location, _options);
        }
    }
}
