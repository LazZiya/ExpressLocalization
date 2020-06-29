using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// DB based HtmlLocalizer
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public class DbHtmlLocalizer<TResource, TTranslation> : IHtmlLocalizer
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly ExpressLocalizationOptions _options;
        private readonly IEFGenericDataManager _dataManager;
        private readonly IStringTranslator _stringTranslator;
        private readonly IHtmlTranslator _htmlTranslator;

        /// <summary>
        /// Initialize a new instance of DbHtmlLocalizer
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataManager"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        public DbHtmlLocalizer(IOptions<ExpressLocalizationOptions> options, 
                               IEFGenericDataManager dataManager, 
                               IStringTranslator stringTranslator, 
                               IHtmlTranslator htmlTranslator)
        {
            _options = options.Value;
            _dataManager = dataManager;
            _stringTranslator = stringTranslator;
            _htmlTranslator = htmlTranslator;
        }

        /// <summary>
        /// Get localized html string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetLocalizedHtmlString(name);

        /// <summary>
        /// Get localized html string with arguments
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
        /// Get localized string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name)
        {
            return GetLocalizedString(name);
        }

        /// <summary>
        /// Get localized string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return GetLocalizedString(name, arguments);
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

        private LocalizedHtmlString GetLocalizedHtmlString(string name, params object[] arguments)
        {
            var cultureId = CultureInfo.CurrentCulture.Name;
            var val = _dataManager.GetAsync<TTranslation>(x => x.Resource.Key == name && x.CultureID == cultureId).Result;

            var locStr = val == null
                ? new LocalizedHtmlString(name, name, true)
                : new LocalizedHtmlString(name, val.Value, false);

            if (locStr.IsResourceNotFound)
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
                : new LocalizedHtmlString(name, string.Format(locStr.Value, arguments), locStr.IsResourceNotFound);
        }
    }
}
