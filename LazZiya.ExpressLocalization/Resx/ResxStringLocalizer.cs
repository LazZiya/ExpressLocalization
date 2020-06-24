using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.ExpressLocalization.Xml;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Resouece file based StringLocalizer
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxStringLocalizer<TResource> : ResxStringLocalizer
        where TResource : class
    {

        /// <summary>
        /// Initialize a new instance of ResxSteingLocalizer
        /// </summary>
        public ResxStringLocalizer(IOptions<ExpressLocalizationOptions> options, 
                                   IStringTranslator stringTranslator, 
                                   IStringLocalizerFactory factory)
            : base(typeof(TResource), options, stringTranslator, factory)
        {
        }
    }

    /// <summary>
    /// String localizer based on .resx resource files 
    /// </summary>
    public class ResxStringLocalizer : IStringLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IStringLocalizer _localizer;
        private readonly IStringTranslator _stringTranslator;
        private readonly string _baseName;
        private readonly string _location;
        
        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="resxType"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="factory"></param>
        public ResxStringLocalizer(Type resxType, 
                                   IOptions<ExpressLocalizationOptions> options, 
                                   IStringTranslator stringTranslator, 
                                   IStringLocalizerFactory factory)
            :this(resxType.Name, options.Value.ResourcesPath, options, stringTranslator, factory)
        {
        }

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="factory"></param>
        public ResxStringLocalizer(string baseName, 
                                   string location, 
                                   IOptions<ExpressLocalizationOptions> options, 
                                   IStringTranslator stringTranslator, 
                                   IStringLocalizerFactory factory)
        {
            _options = options.Value;
            _baseName = baseName;
            _location = location;
            _stringTranslator = stringTranslator;
            _localizer = factory.Create(baseName, location);
        }

        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name] => GetLocalizedString(name);

        /// <summary>
        /// Get localized string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);

        /// <summary>
        /// Get all localized strings
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED, Use <see cref="CultureSwitcher"/> instead.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var locStr = arguments == null ? _localizer[name] : _localizer[name, arguments];

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
    }
}
