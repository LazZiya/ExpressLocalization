using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.ResxTools;
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
                                   IStringTranslator stringTranslator)
            : base(typeof(TResource), options, stringTranslator)
        {
        }
    }

    /// <summary>
    /// String localizer based on .resx resource files 
    /// </summary>
    public class ResxStringLocalizer : IStringLocalizer
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IStringTranslator _stringTranslator;
        private readonly string _baseName;
        private readonly string _location;
        
        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="resxType"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public ResxStringLocalizer(Type resxType, 
                                   IOptions<ExpressLocalizationOptions> options, 
                                   IStringTranslator stringTranslator)
            :this(resxType.Name, options.Value.ResourcesPath, options, stringTranslator)
        {
        }

        /// <summary>
        /// Initialize new instance of ResxStringLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        public ResxStringLocalizer(string baseName, 
                                   string location, 
                                   IOptions<ExpressLocalizationOptions> options, 
                                   IStringTranslator stringTranslator)
        {
            _options = options.Value;
            _baseName = baseName;
            _location = location;
            _stringTranslator = stringTranslator;
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
            var resxManager = new ResxManager(_baseName, _location, CultureInfo.CurrentCulture.Name);
            var resElement = resxManager.FindAsync(name).Result;
            
            LocalizedString locStr;

            if(resElement == null)
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
    }
}
