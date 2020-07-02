using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic XmlHtmlLocalizer based on speificed type
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlHtmlLocalizer<TResource> : XmlHtmlLocalizer, IHtmlLocalizer<TResource>
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize a new instance of XmlHtmlLocalizer with the specified resource type
        /// </summary>
        /// <param name="localizer"></param>
        public XmlHtmlLocalizer(IStringLocalizer<TResource> localizer)
            : base(localizer)
        {
        }
    }

    /// <summary>
    /// XmlHtmlLocalizer
    /// </summary>
    public class XmlHtmlLocalizer : IHtmlLocalizer
    {
        private readonly IStringLocalizer _localizer;

        /// <summary>
        /// Initialize new instance of XmlStringLocalizer
        /// </summary>
        public XmlHtmlLocalizer(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }        

        /// <summary>
        /// Get localized string value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetLocalizedHtmlString(name);

        /// <summary>
        /// Get localized string value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] => GetLocalizedHtmlString(name, arguments);

        /// <summary>
        /// Get LocalizedString value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name) => _localizer[name];
        
        /// <summary>
        /// Get LocalizedString value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments) => _localizer[name, arguments];
        
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
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a localized string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var locVal = _localizer[name, arguments];

            var val = string.Format(locVal, arguments);

            return new LocalizedHtmlString(name, val, locVal.ResourceNotFound);
        }
    }
}