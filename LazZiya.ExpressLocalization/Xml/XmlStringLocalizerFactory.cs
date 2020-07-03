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
        private readonly IExpressTranslator _translator;
        private readonly ExpressMemoryCache _cache;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        public XmlStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                                         IExpressTranslator translator,
                                         ExpressMemoryCache cache)
        {
            _options = options;
            _translator = translator;
            _cache = cache;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            var reader = new XmlResourceReaderWriter(typeof(TResource), _options.Value.ResourcesPath);

            return new XmlStringLocalizer(_cache, reader, _options, _translator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            var reader = new XmlResourceReaderWriter(resourceSource, _options.Value.ResourcesPath);

            return new XmlStringLocalizer(_cache, reader, _options, _translator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return Create();
        }
    }
}
