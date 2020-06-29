using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// DB based string localization
    /// </summary>
    public class DbStringLocalizer<TResource, TTranslation> : IStringLocalizer
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IEFGenericDataManager _dataManager;
        private readonly IStringTranslator _stringTranslator;

        /// <summary>
        /// Initialize a new instance of DbStringLocalizer
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataManager"></param>
        /// <param name="stringTranslator"></param>
        public DbStringLocalizer(IOptions<ExpressLocalizationOptions> options,
                                 IEFGenericDataManager dataManager,
                                 IStringTranslator stringTranslator)
        {
            _options = options.Value;
            _dataManager = dataManager;
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
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
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
            var cultureId = CultureInfo.CurrentCulture.Name;
            var val = _dataManager.GetAsync<TTranslation>(x => x.Resource.Key == name && x.CultureID == cultureId).Result;

            var locStr = val == null
                ? new LocalizedString(name, name, true)
                : new LocalizedString(name, val.Value, false);

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
                    // Check if the resource entity exists
                    var keyExist = _dataManager.Count<TResource>(x => x.Key == name).Result;

                    if (keyExist == 0)
                    {
                        var res = DynamicObjectCreator.DbResource<TResource>(name);
                        var success = _dataManager.AddAsync<TResource>(res).Result;

                        if (success)
                        {
                            var trans = DynamicObjectCreator.DbTranslation<TTranslation>(res.ID, locStr.Value);
                            success = _dataManager.AddAsync<TTranslation>(trans).Result;
                        }
                    }
                }
            }

            return arguments == null
                ? locStr
                : new LocalizedString(name, string.Format(locStr.Value, arguments), locStr.ResourceNotFound);
        }
    }
}
