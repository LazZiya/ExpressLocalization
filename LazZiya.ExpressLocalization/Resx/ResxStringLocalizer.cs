using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Resouece file based StringLocalizer
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxStringLocalizer<TResource> : ResxStringLocalizer, IStringLocalizer<TResource>
        where TResource : IExpressResource
    {
        /// <summary>
        /// Initialzie a new instance of <see cref="ResxStringLocalizer{TResource}"/>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public ResxStringLocalizer(ExpressMemoryCache cache, IOptions<ExpressLocalizationOptions> options, ILoggerFactory loggerFactory)
            : base(typeof(TResource), cache, options, loggerFactory)
        {

        }
    }

    /// <summary>
    /// An <see cref="ResxStringLocalizer"/> based on the default resource type
    /// </summary>
    public class ResxStringLocalizer : IStringLocalizer
    {
        private readonly ResxResourceReader _reader;
        private readonly ExpressMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly ExpressLocalizationOptions _options;

        /// <summary>
        /// Initialize a new instance of ResxSteingLocalizer
        /// </summary>
        public ResxStringLocalizer(Type resourceSource, ExpressMemoryCache cache, IOptions<ExpressLocalizationOptions> options, ILoggerFactory loggerFactory)
        {
            if (resourceSource == null)
                throw new NotImplementedException(nameof(resourceSource));

            _cache = cache;
            _options = options.Value;
            _reader = new ResxResourceReader(resourceSource, _options.ResourcesPath, loggerFactory);
            _logger = loggerFactory.CreateLogger<ResxStringLocalizer>();
        }

        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name] => GetLocalizedString(name);

        /// <summary>
        /// Get localized string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);

        /// <summary>
        /// Get all localized strings
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED, Use <see cref="CultureSwitcher"/> instead.
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
                // Option 2: Look in resx resource
                bool availableInResource = _reader.TryGetValue(name, out value);

                if (!availableInResource && _options.AutoTranslate)
                {
                    // Option 3: Online translate
                    _logger.LogInformation($"Auto translation is not available with resx mode, switch to Db or Xml mode for online translation.");
                }

                if (!availableInResource && _options.AutoAddKeys)
                {
                    _logger.LogInformation($"Auto key adding is not available with resx mode, switch to Db or Xml mode for auto key adding.");
                }

                if (availableInResource || availableInTranslate)
                {
                    // Save to cache
                    _cache.Set(name, value);

                    // Set availability to true
                    availableInCache = true;
                }
            }

            //_logger.LogInformation($"Available in cache: '{name}', '{availableInCache}'");

            var val = string.Format(value ?? name, arguments);

            return new LocalizedString(name, val, resourceNotFound: !availableInCache, _reader.TypeName);
        }
    }
}