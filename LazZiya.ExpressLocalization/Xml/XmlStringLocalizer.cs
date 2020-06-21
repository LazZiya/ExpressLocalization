using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
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
        private readonly string _xmlResPath;
        private readonly XDocument _xmlDoc;
        private readonly IStringTranslator _stringTranslator;

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

            _xmlResPath = Path.Combine(location, $"{xmlBaseName}.{CultureInfo.CurrentCulture.Name}.xml");

            if (!File.Exists(_xmlResPath))
            {
                try
                {
                    // Create a copy of the template xml resource
                    var path = typeof(XmlTemplate).Assembly.Location;
                    var folder = path.Substring(0, path.LastIndexOf('\\'));
                    File.Copy($"{folder}\\Xml\\XmlTemplate.xml", _xmlResPath);
                }
                catch (Exception e)
                {
                    throw new FileLoadException($"Can't load or create resource file. {e.Message}");
                }
            }

            _xmlDoc = XDocument.Load(_xmlResPath);

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
                    locStr = _stringTranslator[name, arguments];
                }

                if (_options.AutoAddKeys)
                {
                    if (_options.AutoAddKeys)
                    {
                        locStr.WriteTo(_xmlDoc, _xmlResPath);
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
