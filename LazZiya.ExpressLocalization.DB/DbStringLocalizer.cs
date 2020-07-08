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
    public class DbStringLocalizer<TResource, TTranslation> : IDbStringLocalizer<TResource, TTranslation>
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly IEFGenericDataManager _dataManager;
        private readonly IExpressTranslator _translator;
        private readonly ExpressMemoryCache _cache;
        private readonly ExpressLocalizationOptions _options;

        /// <summary>
        /// Initialize a new instance of DbStringLocalizer
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataManager"></param>
        /// <param name="translator"></param>
        /// <param name="cache"></param>
        public DbStringLocalizer(IEFGenericDataManager dataManager,
                                 IExpressTranslator translator,
                                 ExpressMemoryCache cache,
                                 IOptions<ExpressLocalizationOptions> options)
        {
            _options = options.Value;
            _dataManager = dataManager;
            _translator = translator;
            _cache = cache;
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
            var availableInTranslate = false;

            // Option 1: Look in the cache
            bool availableInCache = _cache.TryGetValue(name, out string value);

            if (!availableInCache)
            {
                // Option 2: Look in DB
                bool availableInDb = TryGetValueFromDb(name, out value);

                if (!availableInDb && _options.AutoTranslate)
                {
                    // Option 3: Online translate
                    availableInTranslate = _translator.TryTranslate(name, "text", out value);
                }

                if (!availableInDb && _options.AutoAddKeys)
                {
                    // Save value to XML resource regardless the value has been translated or not
                    // If the value is not translated, the default "name" will be assigned to the "value"
                    // Anyhow, the saved values needs to be checked and confirmed one by one
                    bool savedToResource = AddValueToDb(name, value ?? name);
                }

                if (availableInDb || availableInTranslate)
                {
                    // Save to cache
                    _cache.Set(name, value);

                    // Set availability to true
                    availableInCache = true;
                }
            }

            var val = string.Format(value, arguments);

            return new LocalizedString(name, val, resourceNotFound: !availableInCache, searchedLocation: nameof(TTranslation));
        }


        private bool TryGetValueFromDb(string name, out string value)
        {
            // set initial value 
            value = name;

            // get current culture
            var cultureId = CultureInfo.CurrentCulture.Name;

            // get value from db
            var val = _dataManager.GetAsync<TTranslation>(x => x.Resource.Key == name && x.CultureID == cultureId).Result;

            var success = false;
            if (val != null)
            {
                success = true;
                value = val.Value;
            }

            return success;
        }

        private bool AddValueToDb(string name, string value)
        {
            // Check if the resource entity exists
            var resId = _dataManager.GetAsync<TResource, int>(x => x.Key == name, x => x.ID).Result;

            if (resId == 0)
            {
                var res = DynamicObjectCreator.DbResource<TResource>(name);
                resId = _dataManager.AddAsync<TResource, int>(res).Result;
            }

            var trans = DynamicObjectCreator.DbTranslation<TTranslation>(resId, value);
            var success = _dataManager.AddAsync<TTranslation>(trans).Result;

            return success;
        }
    }
}
