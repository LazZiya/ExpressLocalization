using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic XmlHtmlLocalizer based on speificed type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlHtmlLocalizer<T> : XmlHtmlLocalizer
        where T : IXmlResource
    {
        /// <summary>
        /// Initialize a new instance of XmlHtmlLocalizer with the specified resource type
        /// </summary>
        /// <param name="options"></param>
        public XmlHtmlLocalizer(IOptions<ExpressLocalizationOptions> options)
            : base(typeof(T), options)
        {
        }
    }

    /// <summary>
    /// XmlHtmlLocalizer
    /// </summary>
    public class XmlHtmlLocalizer : IHtmlLocalizer
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly string _xmlResPath;
        private readonly string _xmlResName;
        private readonly XDocument _xmlDoc;
        
        /// <summary>
        /// Initialize new instance of XmlStringLocalizer
        /// </summary>
        public XmlHtmlLocalizer(Type type, IOptions<ExpressLocalizationOptions> options)
            : this(type.Name, options.Value.ResourcesPath, options)
        {
        }

        /// <summary>
        /// Initialize new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        public XmlHtmlLocalizer(string baseName, string location, IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;

            _xmlResPath = location;

            _xmlResName = baseName;

            var xmlResPathAndName = Path.Combine(_xmlResPath, $"{_xmlResName}.{CultureInfo.CurrentCulture.Name}.xml");

            _xmlDoc = XDocument.Load(xmlResPathAndName);
        }        

        /// <summary>
        /// Get localized string value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetLocalizedHtmlString(name, null, null);

        /// <summary>
        /// Get localized string value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] => GetLocalizedHtmlString(name, null, null, arguments);

        /// <summary>
        /// Get LocalizedString value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name) => GetLocalizedString(name, null, null);
        
        /// <summary>
        /// Get LocalizedString value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments) => GetLocalizedString(name, null, null, arguments);
        
        /// <summary>
        /// Get all localized string values
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _xmlDoc.Root.Descendants("data").Select(x => new LocalizedString(x.Attribute("name").Value, x.Element("value").Value, false));
        }

        /// <summary>
        /// Change localization culture
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
        /// <param name="key"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private LocalizedHtmlString GetLocalizedHtmlString(string key, params object[] arguments)
        {
            var elmnt = _xmlDoc.Root.XPathSelectElement($"data[@name='{HttpUtility.HtmlEncode(key)}']");

            if (elmnt != null)
            {
                var value = string.Format(elmnt.Element("value").Value, arguments);
                return new LocalizedHtmlString(key, value, false);
            }

            return new LocalizedHtmlString(key, key, true);
        }

        private LocalizedString GetLocalizedString(string key, params object[] arguments)
        {
            var elmnt = _xmlDoc.Root.XPathSelectElement($"data[name='{HttpUtility.HtmlEncode(key)}']");

            if (elmnt != null)
            {
                var value = string.Format(elmnt.Element("value").Value, arguments);
                return new LocalizedString(key, value, false);
            }

            return new LocalizedString(key, key, true);
        }
    }
}
