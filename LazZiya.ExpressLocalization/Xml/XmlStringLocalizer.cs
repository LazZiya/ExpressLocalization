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
    public class XmlStringLocalizer<TResource> : XmlStringLocalizer, IStringLocalizer<TResource>
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize a new instance of XmlStringLocalizer with the specified resource type
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="rw"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public XmlStringLocalizer(ExpressMemoryCache cache, 
                                  IExpressResourceReaderWriter<TResource> rw,
                                  IOptions<ExpressLocalizationOptions> options, 
                                  IStringTranslator stringTranslator)
            : base(cache, rw, options, stringTranslator)
        {

        }
    }

    /// <summary>
    /// Xml localizer
    /// </summary>
    public class XmlStringLocalizer : IStringLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly ExpressMemoryCache _cache;
        private readonly IStringTranslator _stringTranslator;
        private readonly IExpressResourceReaderWriter _rw;

        /// <summary>
        /// Initialzie new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="rw"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public XmlStringLocalizer(ExpressMemoryCache cache, 
                                  IExpressResourceReaderWriter rw,
                                  IOptions<ExpressLocalizationOptions> options,
                                  IStringTranslator stringTranslator)
        {
            _cache = cache;
            _options = options.Value;
            _stringTranslator = stringTranslator;
            _rw = rw;
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
            var resourceFound = TryGetValue(name, out string val);

            if (!resourceFound && _options.AutoTranslate)
            {
                val = _stringTranslator[name];
            }

            if (!resourceFound && _options.AutoAddKeys)
            {
                var added = _rw.TrySetValue(name, val, "Auto created by ExpressLocalization", false);

                if (added)
                    _cache.Set(name, val);
            }

            var value = string.Format(val ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: !resourceFound);
        }

        private bool TryGetValue(string name, out string value)
        {
            // Look for the localized value in the cache
            var success = _cache.TryGetValue(name, out value);

            if (!success)
            {
                // Look in the xml file
                success = _rw.TryGetValue(name, out value);

                // save it to the cache
                if (success)
                    _cache.Set(name, value);
            }

            return success;
        }
    }
}
