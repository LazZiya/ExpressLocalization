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
                                 IHtmlTranslator htmlTranslator, 
                                 IStringLocalizerFactory stringLocalizerFactory, 
                                 IHtmlLocalizerFactory htmlLocalizerFactory)
            : base(typeof(TResource), options, stringTranslator, htmlTranslator, stringLocalizerFactory, htmlLocalizerFactory)
        {

        }
    }

    /// <summary>
    /// Resource file based HtmlLocalizer
    /// </summary>
    public class ResxHtmlLocalizer : IHtmlLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IHtmlLocalizer _htmlLocalizer;
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
        /// <param name="stringLocalizerFactory"></param>
        /// <param name="htmlLocalizerFactory"></param>
        public ResxHtmlLocalizer(Type resxType, 
                                 IOptions<ExpressLocalizationOptions> options, 
                                 IStringTranslator stringTranslator, 
                                 IHtmlTranslator htmlTranslator, 
                                 IStringLocalizerFactory stringLocalizerFactory, 
                                 IHtmlLocalizerFactory htmlLocalizerFactory)
            : this(resxType.Name, options.Value.ResourcesPath, options, stringTranslator, htmlTranslator, stringLocalizerFactory, htmlLocalizerFactory)
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
        /// <param name="stringLocalizerFactory"></param>
        /// <param name="htmlLocalizerFactory"></param>
        public ResxHtmlLocalizer(string baseName, 
                                 string location, 
                                 IOptions<ExpressLocalizationOptions> options, 
                                 IStringTranslator stringTranslator, 
                                 IHtmlTranslator htmlTranslator, 
                                 IStringLocalizerFactory stringLocalizerFactory, 
                                 IHtmlLocalizerFactory htmlLocalizerFactory)
        {
            _options = options.Value;
            _baseName = baseName;
            _location = location;
            _stringTranslator = stringTranslator;
            _htmlTranslator = htmlTranslator;
            _stringLocalizer = stringLocalizerFactory.Create(baseName, location);
            _htmlLocalizer = htmlLocalizerFactory.Create(baseName, location);
        }

        /// <summary>
        /// Get LocalizedHtmlString
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => throw new NotImplementedException();

        /// <summary>
        /// Get LocalizedHtmlString with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get LocalizedString with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            throw new NotImplementedException();
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
            var locStr = arguments == null ? _stringLocalizer[name] : _stringLocalizer[name, arguments];

            if (locStr.ResourceNotFound)
            {
                if (_options.OnlineTranslation)
                {
                    // Call the translator function without arguments, 
                    // so we can insert the raw string in xml file
                    // requrired to keep placeholders {0} in the raw string
                    locStr = _stringTranslator[name];
                }

                if (_options.AutoAddKeys)
                {
                    var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
                    var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

                    // Add LocaizedString to a temporary xml file 
                    // temp xml file has the same name and directory
                    // it only has .xml extension instead of .resx
                    locStr.WriteTo(_xmlDoc, _path);
                }
            }

            return arguments == null
                ? locStr
                : new LocalizedString(name, string.Format(locStr.Value), locStr.ResourceNotFound);
        }
        
        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var locStr = arguments == null ? _htmlLocalizer[name] : _htmlLocalizer[name, arguments];

            if (locStr.IsResourceNotFound)
            {
                if (_options.OnlineTranslation)
                {
                    // Call the translator function without arguments, 
                    // so we can insert the raw string in xml file
                    // requrired to keep placeholders {0} in the raw string
                    locStr = _htmlLocalizer[name];
                }

                if (_options.AutoAddKeys)
                {
                    var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
                    var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

                    // Add LocaizedString to a temporary xml file 
                    // temp xml file has the same name and directory
                    // it only has .xml extension instead of .resx
                    locStr.WriteTo(_xmlDoc, _path);
                }
            }

            return arguments == null
                ? locStr
                : new LocalizedHtmlString(name, string.Format(locStr.Value), locStr.IsResourceNotFound);
        }
    }
}
