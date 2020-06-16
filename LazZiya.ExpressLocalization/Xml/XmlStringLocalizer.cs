using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic XmlStringLocalizer based on speificed type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlStringLocalizer<T> : XmlStringLocalizer
        where T : IXmlResource
    {
        /// <summary>
        /// Initialize a new instance of XmlStringLocalizer with the specified resource type
        /// </summary>
        /// <param name="options"></param>
        public XmlStringLocalizer(IOptions<ExpressLocalizationOptions> options) : base(typeof(T), options)
        {

        }
    }

    /// <summary>
    /// Xml localizer
    /// </summary>
    public class XmlStringLocalizer : IStringLocalizer
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly string _xmlResPath;
        private readonly string _xmlResName;
        private readonly XDocument _xmlDoc;

        /// <summary>
        /// Initialze new instance of XmlStringLocalizer
        /// </summary>
        public XmlStringLocalizer(Type xmlResType, IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;

            _xmlResPath = _options.Value.ResourcesPath;

            _xmlResName = xmlResType.Name;

            var xmlResPathAndName = Path.Combine(_xmlResPath, $"{_xmlResName}.{CultureInfo.CurrentCulture.Name}.xml");

            _xmlDoc = XDocument.Load(xmlResPathAndName);
        }

        /// <summary>
        /// Initialzie new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="xmlBaseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        public XmlStringLocalizer(string xmlBaseName, string location, IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;

            _xmlResPath = location;

            _xmlResName = xmlBaseName;

            var xmlResPathAndName = Path.Combine(_xmlResPath, $"{_xmlResName}.{CultureInfo.CurrentCulture.Name}.xml");

            _xmlDoc = XDocument.Load(xmlResPathAndName);
        }

        /// <summary>
        /// Get localized string value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name] => GetLocalizedString(name);

        /// <summary>
        /// Get localized string value with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);

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
        /// Change localization culture. 
        /// THIS METHOD IS NOT AVAILABLE FOR .NET5
        /// USE CultureSwitcher INSTEAD
        /// <see cref="CultureSwitcher"/>
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;

            return new XmlStringLocalizer(_xmlResName, _xmlResPath, _options);
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var elmnt = _xmlDoc.Root.XPathSelectElement($"data[@name='{name}']");

            if (elmnt == null)
            {
                return new LocalizedString(name, name, true);
            }

            var value = string.Format(elmnt.Element("value").Value, arguments);

            return new LocalizedString(name, value, false);
        }
    }
}
