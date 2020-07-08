using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic <see cref="XmlStringLocalizer{TResource}"/> for localization based on generic xml type
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlStringLocalizer<TResource> : XmlStringLocalizer, IStringLocalizer<TResource>
        where TResource : IExpressResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="XmlStringLocalizer{TResource}"/>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        /// <param name="loggerFactory"></param>
        public XmlStringLocalizer(ExpressMemoryCache cache,
                                  IOptions<ExpressLocalizationOptions> options,
                                  IExpressTranslator translator,
                                  ILoggerFactory loggerFactory)
            : base(typeof(TResource), cache, options, translator, loggerFactory)
        {
        }
    }

    /// <summary>
    /// An <see cref="XmlStringLocalizer"/> for localization based on Xml files
    /// This localizer is based on the default type defined in startup.
    /// </summary>
    public class XmlStringLocalizer : IStringLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly ExpressMemoryCache _cache;
        private readonly XmlResourceReaderWriter _rw;
        private readonly IExpressTranslator _translator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialzie new instance of <see cref="XmlStringLocalizer"/> 
        /// based on the default resource type defined in startup
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="resourceSource"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        /// <param name="loggerFactory"></param>
        public XmlStringLocalizer(Type resourceSource,
                                  ExpressMemoryCache cache,
                                  IOptions<ExpressLocalizationOptions> options,
                                  IExpressTranslator translator,
                                  ILoggerFactory loggerFactory)
        {
            if (resourceSource == null)
                throw new NotImplementedException(nameof(resourceSource));

            _cache = cache;
            _options = options.Value;
            _rw = new XmlResourceReaderWriter(resourceSource, _options.ResourcesPath, loggerFactory);
            _translator = translator;
            _logger = loggerFactory.CreateLogger<XmlStringLocalizer>();
        }

        /// <summary>
        /// Get localized string value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name] => GetLocalizedString(name);

        /// <summary>
        /// Get localized string value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);

        /// <summary>
        /// Get all localized string values
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED! use <see cref="CultureSwitcher"/> instead.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var availableInTranslate = false;

            // Option 1: Look in the cache
            bool availableInCache = _cache.TryGetValue(name, out string value);

            if (!availableInCache)
            {
                // Option 2: Look in XML resource
                bool availableInResource = _rw.TryGetValue(name, out value);

                if (!availableInResource && _options.AutoTranslate)
                {
                    // Option 3: Online translate
                    availableInTranslate = _translator.TryTranslate(name, "text", out value);
                    _logger.LogInformation($"Auto translation result: {availableInTranslate}");
                }

                if (!availableInResource && _options.AutoAddKeys)
                {
                    // Save value to XML resource regardless the value has been translated or not
                    // If the value is not translated, the default "name" will be assigned to the "value"
                    // Anyhow, the saved values needs to be checked and confirmed one by one
                    bool savedToResource = _rw.TrySetValue(name, value ?? name, "Auto created by ExpressLocalization", false);
                    _logger.LogInformation($"Auto save resource key result: {savedToResource}");
                }

                if (availableInResource || availableInTranslate)
                {
                    // Save to cache
                    _cache.Set(name, value);

                    // Set availability to true
                    availableInCache = true;
                }
            }

            var val = string.Format(value ?? name, arguments);

            return new LocalizedString(name, val, resourceNotFound: !availableInCache, _rw.TypeName);
        }
    }
}
