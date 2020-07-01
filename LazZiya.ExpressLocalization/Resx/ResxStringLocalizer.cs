using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.ResxTools;
using Microsoft.Extensions.Localization;
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
        where TResource : IXLResource
    {

        /// <summary>
        /// Initialize a new instance of ResxSteingLocalizer
        /// </summary>
        public ResxStringLocalizer(ExpressMemoryCache cache, IOptions<ExpressLocalizationOptions> options)
            : base(cache, typeof(TResource), options.Value.ResourcesPath, options)
        {
        }
    }

    /// <summary>
    /// String localizer based on .resx resource files 
    /// </summary>
    public class ResxStringLocalizer : IStringLocalizer
    {
        private readonly string _baseName;
        private readonly string _location;
        private readonly ExpressLocalizationOptions _options;
        private readonly ExpressMemoryCache _cache;

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="resxType"></param>
        /// <param name="location"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        public ResxStringLocalizer(ExpressMemoryCache cache, Type resxType, string location, IOptions<ExpressLocalizationOptions> options)
            : this(cache, resxType.Name, location, options)
        {
        }

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="manager"></param>
        public ResxStringLocalizer(ExpressMemoryCache cache, string baseName, string location, IOptions<ExpressLocalizationOptions> options)
        {
            _baseName = baseName;
            _location = location;
            _cache = cache;
            _options = options.Value;
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
            var isResourceFound = TryGetValue(name, out string val);

            return new LocalizedString(name, string.Format(val ?? name, arguments), resourceNotFound: !isResourceFound);
        }

        private bool TryGetValue(string name, out string value)
        {
            // Look for the localized value in the cache
            var success = _cache.TryGetValue(name, out value);

            // If not available in cache, look in the resx file
            if (!success)
            {
                var resxManager = new ResxManager(_baseName, _location, CultureInfo.CurrentCulture.Name);
                success = resxManager.TryGetValue(name, out value);

                // If value is found in the resource file
                // save it to the cache
                if (success)
                    _cache.Set(name, value);
            }

            return success;
        }
    }
}
