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
        private readonly IExpressResourceReaderWriter _rw;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        /// <param name="rw"></param>
        public XmlStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                                         IExpressTranslator translator,
                                         IExpressResourceReaderWriter<TResource> rw,
                                         ExpressMemoryCache cache)
        {
            _options = options;
            _translator = translator;
            _cache = cache;
            _rw = rw;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new XmlStringLocalizer<TResource>(_cache, _rw, _options, _translator);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return Create();
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
