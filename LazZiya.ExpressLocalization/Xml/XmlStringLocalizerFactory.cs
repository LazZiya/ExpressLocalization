using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlStringLocalizerFactory
    /// </summary>
    public class XmlStringLocalizerFactory<TResource> : IExpressStringLocalizerFactory
        where TResource : IExpressResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IExpressTranslator _translator;
        private readonly ExpressMemoryCache _cache;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, XmlStringLocalizer> _localizerCache = new ConcurrentDictionary<string, XmlStringLocalizer>();

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        /// <param name="loggerFactory"></param>
        public XmlStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options,
                                         IExpressTranslator translator,
                                         ExpressMemoryCache cache,
                                         ILoggerFactory loggerFactory)
        {
            _options = options;
            _translator = translator;
            _cache = cache;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return Create(typeof(TResource));
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            return _localizerCache.GetOrAdd(resourceSource.FullName, _ => CreateXmlStringLocalizer(resourceSource));
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="baseName">type full name</param>
        /// <param name="location">Assembly name</param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            return _localizerCache.GetOrAdd($"B={baseName},L={location}", _ =>
            {
                var type = ResourceTypeHelper.GetResourceType(baseName, location);
                return CreateXmlStringLocalizer(type);
            });
        }

        /// <summary>
        /// Creates a <see cref="XmlStringLocalizer"/> for the given input.
        /// </summary>
        /// <param name="resourceSource">The assembly to create a <see cref="XmlStringLocalizer"/> for.</param>
        private XmlStringLocalizer CreateXmlStringLocalizer(Type resourceSource)
        {
            return new XmlStringLocalizer(resourceSource, _cache, _options, _translator, _loggerFactory);
        }
    }
}