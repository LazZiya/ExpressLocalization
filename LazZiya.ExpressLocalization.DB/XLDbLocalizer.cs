using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.Extensions.Options;
using LazZiya.TranslationServices;
using LazZiya.EFGenericDataManager;
using System.Linq.Expressions;
using LazZiya.EFGenericDataManager.Models;
using Microsoft.Extensions.Logging;
using System.Net;

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB Localizer
    /// </summary>
    public class XLDbLocalizer<TXLResource, TXLTranslation, TXLCulture> : ISharedCultureLocalizer, ICulturesProvider<TXLCulture>
        where TXLResource : class, IXLResource
        where TXLTranslation : class, IXLTranslation
        where TXLCulture : class, IXLCulture
    {
        private readonly XLDbOptions xlOptions;
        private readonly ITranslationService TranslationService;
        private readonly IEFGenericDataManager DataManager;
        private readonly ILogger Log;

        /// <summary>
        /// Get list of currently active cultures
        /// </summary>
        public ICollection<CultureInfo> ActiveCultures =>
            DataManager.ListAsync<TXLCulture, CultureInfo>(1, 100,
                new List<Expression<Func<TXLCulture, bool>>> { { x => x.IsActive == true } },
                new List<OrderByExpression<TXLCulture>> { new OrderByExpression<TXLCulture> { Expression = x => x.ID, OrderByDir = OrderByDir.ASC } },
                null,
                x => new CultureInfo(x.ID)).Result.Item1;

        /// <summary>
        /// Get default culture
        /// </summary>
        public string DefaultCulture => DataManager.GetAsync<TXLCulture>(x => x.IsDefault == true).Result?.ID;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, GetLocalizedString(name, arguments));

        LocalizedString IStringLocalizer.this[string name] => new LocalizedString(name, GetLocalizedString(name));

        /// <summary>
        /// Initialize ExpressLocalizationDataManager
        /// </summary>
        /// <param name="dataManager"></param>
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        /// <param name="logger"></param>
        public XLDbLocalizer(IEFGenericDataManager dataManager, IOptions<XLDbOptions> options, IEnumerable<ITranslationService> translationServices, ILogger<XLDbLocalizer<TXLResource, TXLTranslation, TXLCulture>> logger)
        {
            if (dataManager == null)
            {
                throw new ArgumentNullException(nameof(dataManager));
            }

            Log = logger;
            DataManager = dataManager;
            xlOptions = options.Value;
            TranslationService = translationServices.FirstOrDefault(x => x.GetType() == xlOptions.TranslationService);
        }

        /// <summary>
        /// Get localized string value for the relevant key with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string this[string key, params object[] args] => GetLocalizedString(key, args);

        /// <summary>
        /// Get localized string value for the relevant key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] => GetLocalizedString(key);

        /// <summary>
        /// Get culture specific localized html string for the given key with arguments
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public LocalizedHtmlString GetLocalizedHtmlString(string culture, string key, params object[] args)
        {
            var resource = DataManager.GetAsync<TXLResource>(x => x.Key == key).Result;

            if (resource == null)
            {
                // Add resource to db if recursive mode is enabled
                if (xlOptions.AutoAddKeys)
                {
                    // Create a json object for the resource then deserialzie it to create a new resource key
                    var dynResource = new { ID = 0, Key = key };
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
                    var resourceJson = JsonConvert.SerializeObject(dynResource);
                    resource = JsonConvert.DeserializeObject<TXLResource>(resourceJson);
#else
                    var resourceJson = JsonSerializer.Serialize(dynResource);
                    resource = JsonSerializer.Deserialize(resourceJson, typeof(TXLResource)) as TXLResource;
#endif
                    var saveResult = DataManager.AddAsync<TXLResource>(resource).Result;
                    Log.LogInformation($"ExpressLocalization.DB - New resource key 'ID={resource?.ID ?? 0}', Add result: {saveResult}");
                }
                else
                {
                    return args == null
                        ? new LocalizedHtmlString(name: key, key, true)
                        : new LocalizedHtmlString(name: key, key, true, args);
                }
            }

            var translation = DataManager.GetAsync<TXLTranslation>(x => x.CultureName == culture && x.ResourceID == resource.ID).Result;

            if (translation == null)
            {
                // Add translation to db if recursive mode is enabled
                if (xlOptions.OnlineLocalization)
                {
                    try
                    {
                        // Translate key
                        var transResponse = TranslationService.TranslateAsync(DefaultCulture, culture, key, "html").Result;
                        Log.LogInformation($"ExpressLocalization.DB - Recursive Translation - {TranslationService.ServiceName} - {transResponse.StatusCode}");

                        if (transResponse.StatusCode == HttpStatusCode.OK)
                        {
                            // Create dynamic translation object
                            var dynTrans = new { ID = 0, ResourceID = resource.ID, CultureName = culture, Value = transResponse.Text };
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
                            var transJson = JsonConvert.SerializeObject(dynTrans);
                            translation = JsonConvert.DeserializeObject<TXLTranslation>(transJson);
#else
                        var transJson = JsonSerializer.Serialize(dynTrans);
                        translation = JsonSerializer.Deserialize(transJson, typeof(TXLTranslation)) as TXLTranslation;
#endif
                            var saveResult = DataManager.AddAsync<TXLTranslation>(translation).Result;
                            Log.LogInformation($"ExpressLocalization.DB - New translation 'ID={translation.ID}', adding result {saveResult}");
                        }
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"ExpressLocalization.DB - Translation service error: " + e.Message);
                    }
                }
            }

            return args == null
                ? new LocalizedHtmlString(name: key, value: translation?.Value ?? key, false)
                : new LocalizedHtmlString(name: key, value: translation?.Value ?? key, false, args);
        }

        /// <summary>
        /// Get localized html string for the given key with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public LocalizedHtmlString GetLocalizedHtmlString(string key, params object[] args)
        {
            return GetLocalizedHtmlString(CultureInfo.CurrentCulture.Name, key, args);
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string key, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string culture, string key, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get culture specific localized string value for the given key with arguments
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetLocalizedString(string culture, string key, params object[] args)
        {
            return args == null
                ? GetLocalizedHtmlString(culture, key).Value
                : string.Format(GetLocalizedHtmlString(culture, key).Value, args);
        }

        /// <summary>
        /// Get localized string value for the given key with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetLocalizedString(string key, params object[] args)
        {
            return GetLocalizedString(CultureInfo.CurrentCulture.Name, key, args);
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetLocalizedString(Type resourceSource, string key, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetLocalizedString(Type resourceSource, string culture, string key, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMETNED
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
