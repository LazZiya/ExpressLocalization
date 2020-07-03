using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// DB based HtmlLocalizer
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public class DbHtmlLocalizer<TResource, TTranslation> : IDbHtmlLocalizer<TResource, TTranslation>
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly IDbStringLocalizer<TResource, TTranslation> _localizer;

        /// <summary>
        /// Initialize a new instance of DbHtmlLocalizer
        /// </summary>
        /// <param name="localizer"></param>
        public DbHtmlLocalizer(IDbStringLocalizer<TResource, TTranslation> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Get localized html string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetLocalizedHtmlString(name);

        /// <summary>
        /// Get localized html string with arguments
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
        /// Get localized string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name)
        {
            return _localizer[name];
        }

        /// <summary>
        /// Get localized string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return _localizer[name, arguments];
        }

        /// <summary>
        /// NOT IMPLEMENTED! use <see cref="CultureSwitcher"/> instead.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var val = _localizer[name, arguments];

            return new LocalizedHtmlString(name, val, val.ResourceNotFound);
        }
    }
}
