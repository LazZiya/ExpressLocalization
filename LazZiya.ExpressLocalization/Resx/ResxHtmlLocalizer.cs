using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.ResxTools;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Resource file based HtmlLocalizer
    /// </summary>
    public class ResxHtmlLocalizer<TResource> : ResxHtmlLocalizer, IHtmlLocalizer<TResource>
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize new instance of ResxHtmlLocalizer
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cache"></param>
        public ResxHtmlLocalizer(ExpressMemoryCache cache, IExpressResourceManager manager)
            : base(cache, manager)
        {
        }
    }

    /// <summary>
    /// Resource file based HtmlLocalizer
    /// </summary>
    public class ResxHtmlLocalizer : IHtmlLocalizer
    {
        private readonly string _baseName;
        private readonly string _location;
        private readonly ExpressMemoryCache _cache;
        private readonly IExpressResourceManager _manager;

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="resxType"></param>
        /// <param name="location"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        public ResxHtmlLocalizer(ExpressMemoryCache cache, IExpressResourceManager manager)
        {
            _cache = cache;
            _manager = manager;
        }

        /// <summary>
        /// Get LocalizedHtmlString
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetLocalizedHtmlString(name);

        /// <summary>
        /// Get LocalizedHtmlString with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] => GetLocalizedHtmlString(name, arguments);

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get LocalizedString
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name)
        {
            return GetLocalizedString(name);
        }

        /// <summary>
        /// Get LocalizedString with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return GetLocalizedString(name, arguments);
        }

        /// <summary>
        /// NOT IMPLEMENTED, Use <see cref="CultureSwitcher"/> instead.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var isResourceFound = TryGetValue(name, out string val);

            return new LocalizedString(name, string.Format(val ?? name, arguments), resourceNotFound: !isResourceFound);
        }

        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var isResourceFound = TryGetValue(name, out string val);

            return new LocalizedHtmlString(name, string.Format(val ?? name, arguments), isResourceNotFound: !isResourceFound);
        }

        private bool TryGetValue(string name, out string value)
        {
            // Look for the localized value in the cache
            var success = _cache.TryGetValue(name, out value);

            // If not available in cache, look in the resx file
            if (!success)
            {
                success = _manager.TryGetValue(name, out value);

                // If value is found in the resource file
                // save it to the cache
                if (success)
                    _cache.Set(name, value);
            }

            return success;
        }
    }
}
