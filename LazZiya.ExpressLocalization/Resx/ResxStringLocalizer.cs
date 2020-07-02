using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
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
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize a new instance of ResxSteingLocalizer
        /// </summary>
        public ResxStringLocalizer(ExpressMemoryCache cache, IExpressResourceManager<TResource> manager)
            : base(cache, manager)
        {
        }
    }

    /// <summary>
    /// String localizer based on .resx resource files 
    /// </summary>
    public class ResxStringLocalizer : IStringLocalizer
    {
        private readonly IExpressResourceManager _manager;
        private readonly ExpressMemoryCache _cache;

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="manager"></param>
        public ResxStringLocalizer(ExpressMemoryCache cache, IExpressResourceManager manager)
        {
            _cache = cache;
            _manager = manager;
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
            var resourceFound = TryGetValue(name, out string val);

            var value = string.Format(val ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: !resourceFound);
        }

        private bool TryGetValue(string name, out string value)
        {
            // Look for the localized value in the cache
            var success = _cache.TryGetValue(name, out value);

            if (!success)
            {
                // Look in the resx file
                success = _manager.TryGetValue(name, out value);

                // save it to the cache
                if (success)
                    _cache.Set(name, value);
            }

            return success;
        }
    }
}