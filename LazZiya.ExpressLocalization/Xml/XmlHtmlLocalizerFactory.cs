using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlHtmlLocalizerFactory
    /// </summary>
    public class XmlHtmlLocalizerFactory<T> : IHtmlExpressLocalizerFactory
        where T : IXLResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IHtmlTranslator _htmlTranslator;
        private readonly IStringTranslator _stringTranslator;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="htmlTranslator"></param>
        /// <param name="stringTranslator"></param>
        public XmlHtmlLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                                       IHtmlTranslator htmlTranslator, 
                                       IStringTranslator stringTranslator)
        {
            _options = options;
            _htmlTranslator = htmlTranslator;
            _stringTranslator = stringTranslator;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new XmlHtmlLocalizer(typeof(T), _options, _htmlTranslator, _stringTranslator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            return new XmlHtmlLocalizer(resourceSource, _options, _htmlTranslator, _stringTranslator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return new XmlHtmlLocalizer(baseName, location, _options, _htmlTranslator, _stringTranslator);
        }
    }
}
