using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlStringLocalizerFactory
    /// </summary>
    public class XmlStringLocalizerFactory<TResource> : IExpressStringLocalizerFactory
        where TResource : IXLResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IStringTranslator _stringTranslator;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public XmlStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                                         IStringTranslator stringTranslator)
        {
            _options = options;
            _stringTranslator = stringTranslator;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new XmlStringLocalizer(typeof(TResource), _options, _stringTranslator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return new XmlStringLocalizer(resourceSource, _options, _stringTranslator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new XmlStringLocalizer(baseName, location, _options, _stringTranslator);
        }
    }
}
