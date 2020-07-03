using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic XmlStringLocalizer based on speificed type
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlStringLocalizer<TResource> : IStringLocalizer<TResource>
        where TResource : IXLResource
    {            
        private readonly ExpressLocalizationOptions _options;
        private readonly ExpressMemoryCache _cache;
        private readonly IExpressResourceReaderWriter _rw;
        private readonly IExpressTranslator _translator;

        /// <summary>
        /// Initialzie new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="rw"></param>
        /// <param name="options"></param>
        /// <param name="translator"></param>
        public XmlStringLocalizer(ExpressMemoryCache cache,
                                  IExpressResourceReaderWriter rw,
                                  IOptions<ExpressLocalizationOptions> options,
                                  IExpressTranslator translator)
        {
            _cache = cache;
            _options = options.Value;
            _rw = rw;
            _translator = translator;
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
                }
                
                if (!availableInResource && _options.AutoAddKeys)
                {
                    // Save value to XML resource regardless the value has been translated or not
                    // If the value is not translated, the default "name" will be assigned to the "value"
                    // Anyhow, the saved values needs to be checked and confirmed one by one
                    bool savedToResource = _rw.TrySetValue(name, value ?? name, "Auto created by ExpressLocalization", false);
                }
            
                if(availableInResource || availableInTranslate)
                {
                    // Save to cache
                    _cache.Set(name, value);

                    // Set availability to true
                    availableInCache = true;
                }
            }

            var val = string.Format(value, arguments);

            return new LocalizedString(name, val, resourceNotFound: !availableInCache);
        }
    }
}
