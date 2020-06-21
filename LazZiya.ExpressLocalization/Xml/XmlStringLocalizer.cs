using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic XmlStringLocalizer based on speificed type
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlStringLocalizer<TResource> : XmlStringLocalizer, IStringLocalizer<TResource>
        where TResource : class
    {
        /// <summary>
        /// Initialize a new instance of XmlStringLocalizer with the specified resource type
        /// </summary>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public XmlStringLocalizer(IOptions<ExpressLocalizationOptions> options, IStringTranslator stringTranslator) : base(typeof(TResource), options, stringTranslator)
        {

        }
    }

    /// <summary>
    /// Xml localizer
    /// </summary>
    public class XmlStringLocalizer : IStringLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IStringTranslator _stringTranslator;
        private readonly string _baseName;
        private readonly string _location;

        /// <summary>
        /// Initialze new instance of XmlStringLocalizer
        /// </summary>
        public XmlStringLocalizer(Type xmlResType, IOptions<ExpressLocalizationOptions> options, IStringTranslator stringTranslator)
            : this(xmlResType.Name, options.Value.ResourcesPath, options, stringTranslator)
        {
        }

        /// <summary>
        /// Initialzie new instance of XmlStringLocalizer
        /// </summary>
        /// <param name="xmlBaseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public XmlStringLocalizer(string xmlBaseName, string location, IOptions<ExpressLocalizationOptions> options, IStringTranslator stringTranslator)
        {
            _options = options.Value;

            _baseName = xmlBaseName;
            _location = location;

            _stringTranslator = stringTranslator;
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
            var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
            var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);
            return _xmlDoc.Root.Descendants("data").Select(x => new LocalizedString(x.Element("key").Value, x.Element("value").Value, false));
        }

        /// <summary>
        /// NOT IMPLEMENTED! use <see cref="CultureSwitcher"/> instead.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var _path = XmlLocalizerHelper.XmlDocumentFullPath(_baseName, _location);
            var _xmlDoc = XmlLocalizerHelper.GetXmlDocument(_path);

            var elmnt = _xmlDoc.Root.Descendants("data").FirstOrDefault(x => x.Element("key").Value.Equals(name, StringComparison.OrdinalIgnoreCase));

            var locStr = new LocalizedString(name, string.Format(name, arguments), true);

            if (elmnt != null)
            {
                var value = string.Format(elmnt.Element("value").Value, arguments);

                locStr = new LocalizedString(name, value, true);
            }
            else
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

            // rebind the format to value
            return arguments == null
                ? locStr
                : new LocalizedString(name, string.Format(locStr.Value, arguments), locStr.ResourceNotFound);
        }
    }
}
