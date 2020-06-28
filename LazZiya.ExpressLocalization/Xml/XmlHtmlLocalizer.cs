using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        /// <param name="options"></param>
        /// <param name="htmlTranslator"></param>
        /// <param name="stringTranslator"></param>
        public XmlHtmlLocalizer(IOptions<ExpressLocalizationOptions> options, 
                                IHtmlTranslator htmlTranslator, 
                                IStringTranslator stringTranslator)
            : base(typeof(TResource), options, htmlTranslator, stringTranslator)
        {
        }
    }

    /// <summary>
    /// XmlHtmlLocalizer
    /// </summary>
    public class XmlHtmlLocalizer : IHtmlLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly string _baseName;
        private readonly string _location;
        private readonly IHtmlTranslator _htmlTranslator;
        private readonly IStringTranslator _stringTranslator;
        
        /// <summary>
        /// Initialize new instance of XmlStringLocalizer
        /// </summary>
        public XmlHtmlLocalizer(Type type, 
                                IOptions<ExpressLocalizationOptions> options, 
                                IHtmlTranslator htmlTranslator, 
                                IStringTranslator stringTranslator)
            : this(type.Name, options.Value.ResourcesPath, options, htmlTranslator, stringTranslator)
        {
        }

        /// <summary>
        /// Initialize new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        /// <param name="htmlTranslator"></param>
        /// <param name="stringTranslator"></param>
        public XmlHtmlLocalizer(string baseName, 
                                string location, 
                                IOptions<ExpressLocalizationOptions> options, 
                                IHtmlTranslator htmlTranslator, 
                                IStringTranslator stringTranslator)
        {
            _options = options.Value;
            _baseName = baseName;
            _location = location;
            _htmlTranslator = htmlTranslator;
            _stringTranslator = stringTranslator;
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
        public LocalizedString GetString(string name) => GetLocalizedString(name);
        
        /// <summary>
        /// Get LocalizedString value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments) => GetLocalizedString(name, arguments);
        
        /// <summary>
        /// Get all localized string values
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
            var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

            return _xmlDoc.Root.Descendants("data").Select(x => new LocalizedString(x.Element("key").Value, x.Element("value").Value, false));
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
            var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
            var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

            var elmnt = _xmlDoc.Root.Descendants("data").FirstOrDefault(x => x.Element("key").Value.Equals(name, StringComparison.OrdinalIgnoreCase));

            var locStr = elmnt == null
                ? new LocalizedHtmlString(name, name, true)
                : new LocalizedHtmlString(name, elmnt.Element("value").Value, false);

            if(locStr.IsResourceNotFound)
            {
                if (_options.OnlineTranslation)
                {
                    // Call the translator function without arguments, 
                    // so we can insert the raw string in xml file
                    // requrired to keep placeholders {0} in the raw string
                    locStr = _htmlTranslator[name];                    
                }

                if (_options.AutoAddKeys)
                {
                    locStr.WriteTo(_xmlDoc, _path);
                    
                }
            }

            // rebind the arguments to value string
            return arguments == null
                ? locStr
                : new LocalizedHtmlString(name, string.Format(locStr.Value, arguments), locStr.IsResourceNotFound);
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
            var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

            var elmnt = _xmlDoc.Root.Descendants("data").FirstOrDefault(x => x.Element("key").Value.Equals(name, StringComparison.OrdinalIgnoreCase));

            var locStr = elmnt == null
                ? new LocalizedString(name, name, true)
                : new LocalizedString(name, elmnt.Element("value").Value, false);

            if(locStr.ResourceNotFound)
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
                    if (_options.AutoAddKeys)
                    {
                        locStr.WriteTo(_xmlDoc, _path);
                    }
                }
            }

            // rebind the arguments to value string
            return arguments == null
                ? locStr
                : new LocalizedString(name, string.Format(locStr.Value, arguments), locStr.ResourceNotFound);
        }
    }
}