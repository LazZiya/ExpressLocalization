using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxHtmlLocalizerFactory
    /// </summary>
    public class ResxHtmlLocalizerFactory<TResource> : IHtmlExpressLocalizerFactory
        where TResource : class
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IStringTranslator _stringTranslator;
        private readonly IHtmlTranslator _htmlTranslator;

        /// <summary>
        /// Initialize a new instance of ResxHtmlLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        /// <param name="stringLocalizerFactory"></param>
        /// <param name="htmlLocalizerFactory"></param>
        public ResxHtmlLocalizerFactory(IOptions<ExpressLocalizationOptions> options,
                                        IStringTranslator stringTranslator,
                                        IHtmlTranslator htmlTranslator)
        {
            _options = options;
            _stringTranslator = stringTranslator;
            _htmlTranslator = htmlTranslator;
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the default shared resource
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new ResxHtmlLocalizer<TResource>(_options, _stringTranslator, _htmlTranslator);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            return new ResxHtmlLocalizer(resourceSource, _options, _stringTranslator, _htmlTranslator);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given base name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return new ResxHtmlLocalizer(baseName, location, _options, _stringTranslator, _htmlTranslator);
        }
    }
}
