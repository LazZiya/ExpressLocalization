using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.ResxTools;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.ExpressLocalization.Xml;
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
    public class ResxHtmlLocalizer<TResource> : ResxHtmlLocalizer
        where TResource : class
    {
        /// <summary>
        /// Initialize new instance of ResxHtmlLocalizer
        /// </summary>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        /// <param name="stringLocalizerFactory"></param>
        /// <param name="htmlLocalizerFactory"></param>
        public ResxHtmlLocalizer(IOptions<ExpressLocalizationOptions> options, 
                                 IStringTranslator stringTranslator, 
                                 IHtmlTranslator htmlTranslator)
            : base(typeof(TResource), options, stringTranslator, htmlTranslator)
        {
        }
    }

    /// <summary>
    /// Resource file based HtmlLocalizer
    /// </summary>
    public class ResxHtmlLocalizer : IHtmlLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IStringTranslator _stringTranslator;
        private readonly IHtmlTranslator _htmlTranslator;
        private readonly string _baseName;
        private readonly string _location;

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="resxType"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        public ResxHtmlLocalizer(Type resxType, 
                                 IOptions<ExpressLocalizationOptions> options, 
                                 IStringTranslator stringTranslator, 
                                 IHtmlTranslator htmlTranslator)
            : this(resxType.Name, options.Value.ResourcesPath, options, stringTranslator, htmlTranslator)
        {
        }

        /// <summary>
        /// Initialize new instance of ResxHtmlLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        public ResxHtmlLocalizer(string baseName, 
                                 string location, 
                                 IOptions<ExpressLocalizationOptions> options, 
                                 IStringTranslator stringTranslator, 
                                 IHtmlTranslator htmlTranslator)
        {
            _options = options.Value;
            _baseName = baseName;
            _location = location;
            _stringTranslator = stringTranslator;
            _htmlTranslator = htmlTranslator;
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
            var resxManager = new ResxManager(_baseName, _location, CultureInfo.CurrentCulture.Name);
            var resElement = resxManager.FindAsync(name).Result;

            LocalizedString locStr;

            if (resElement == null)
            {
                locStr = arguments == null
                    ? new LocalizedString(name, name, true)
                    : new LocalizedString(name, string.Format(name, arguments), true);
            }
            else
            {
                locStr = arguments == null
                    ? new LocalizedString(name, resElement.Element("value").Value, false)
                    : new LocalizedString(name, string.Format(resElement.Element("value").Value, arguments), false);
            }

            return locStr;
        }
        
        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var resxManager = new ResxManager(_baseName, _location, CultureInfo.CurrentCulture.Name);
            var resElement = resxManager.FindAsync(name).Result;

            LocalizedHtmlString locStr;

            if (resElement == null)
            {
                locStr = arguments == null
                    ? new LocalizedHtmlString(name, name, true)
                    : new LocalizedHtmlString(name, string.Format(name, arguments), true);
            }
            else
            {
                locStr = arguments == null
                    ? new LocalizedHtmlString(name, resElement.Element("value").Value, false)
                    : new LocalizedHtmlString(name, string.Format(resElement.Element("value").Value, arguments), false);
            }

            return locStr;
        }
    }
}
