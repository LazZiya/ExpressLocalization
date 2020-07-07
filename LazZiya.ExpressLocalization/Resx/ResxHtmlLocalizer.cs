using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Resource file based HtmlLocalizer
    /// </summary>
    public class ResxHtmlLocalizer<TResource> : ResxHtmlLocalizer, IHtmlLocalizer<TResource>
        where TResource : IExpressResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ResxHtmlLocalizer{TResource}"/>
        /// </summary>
        /// <param name="factory"></param>
        public ResxHtmlLocalizer(IStringLocalizerFactory factory)
            : base(factory.Create(typeof(TResource)))
        {

        }
    }

    /// <summary>
    /// A <see cref="ResxHtmlLocalizer"/> based on the default resource type
    /// </summary>
    public class ResxHtmlLocalizer : IHtmlLocalizer
    {
        private readonly IStringLocalizer _localizer;

        /// <summary>
        /// Initialize new instance of <see cref="ResxHtmlLocalizer"/> based on the default resource type
        /// </summary>
        /// <param name="localizer"></param>
        public ResxHtmlLocalizer(IStringLocalizer localizer)
        {
            _localizer = localizer;
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
            return _localizer[name];
        }

        /// <summary>
        /// Get LocalizedString with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return _localizer[name, arguments];
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

        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var val = _localizer[name, arguments];

            return new LocalizedHtmlString(name, val.Value, val.ResourceNotFound);
        }
    }
}