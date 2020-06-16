using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlStringLocalizerFactory
    /// </summary>
    public class XmlStringLocalizerFactory<T> : IStringExpressLocalizerFactory
        where T : IXmlResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        public XmlStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new XmlStringLocalizer(typeof(T), _options);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return new XmlStringLocalizer(resourceSource, _options);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new XmlStringLocalizer(baseName, location, _options);
        }
    }
}
