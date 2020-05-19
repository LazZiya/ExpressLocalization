using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB Provider
    /// </summary>
    public class ExpressLocalizationDbProvider<TContext, TExpressLocalizationResource, TCulturesResource, TKey> : ISharedCultureLocalizer, ICulturesProvider<TCulturesResource, TKey>
        where TContext : DbContext
        where TExpressLocalizationResource : class, IExpressLocalizationEntity<TKey>
        where TCulturesResource : class, IExpressLocalizationCulture<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TContext Context;

        private readonly ExpressLocalizationOptions Options;

        /// <summary>
        /// Get list of currently active cultures
        /// </summary>
        public IList<CultureInfo> ActiveCultures => Context.Set<TCulturesResource>().AsNoTracking().Where(x => x.IsActive == true).Select(x => new CultureInfo(x.Name)).ToList();

        /// <summary>
        /// Get default culture
        /// </summary>
        public string DefaultCulture => Context.Set<TCulturesResource>().AsNoTracking().SingleOrDefault(x => x.IsDefault == true).Name;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, GetLocalizedString(name, arguments));

        LocalizedString IStringLocalizer.this[string name] => new LocalizedString(name, GetLocalizedString(name));

        /// <summary>
        /// Initialize ExpressLocalizationDataManager
        /// </summary>
        /// <param name="context"></param>
        public ExpressLocalizationDbProvider(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Options = new ExpressLocalizationOptions();

            Context = context;
        }

        /// <summary>
        /// Initialize ExpressLocalizationDataManager
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        public ExpressLocalizationDbProvider(TContext context, ExpressLocalizationOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Options = options;

            Context = context;
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
            var res = Context.Set<TExpressLocalizationResource>().AsNoTracking()
                .FirstOrDefault(x => x.Key != null && x.Key == key && x.CultureName == culture);

            if (res == null)
            {
                return args == null
                    ? new LocalizedHtmlString(key, key, true)
                    : new LocalizedHtmlString(key, key, false, args);
            }

            return args == null
             ? new LocalizedHtmlString(key, res.Value, true)
             : new LocalizedHtmlString(key, res.Value, false, args);
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
