using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LazZiya.ExpressLocalization.DB.Models;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB Localizer
    /// </summary>
    public class XLDbLocalizer<TContext, TXLResource, TXLCulture> : ISharedCultureLocalizer, ICulturesProvider<TXLCulture>
        where TContext : DbContext
        where TXLResource : class, IXLResource
        where TXLCulture : class, IXLCulture
    {
        private readonly TContext Context;

        private readonly ExpressLocalizationOptions Options;

        /// <summary>
        /// Get list of currently active cultures
        /// </summary>
        public IList<CultureInfo> ActiveCultures => Context.Set<TXLCulture>().AsNoTracking().Where(x => x.IsActive == true).Select(x => new CultureInfo(x.ID)).ToList();

        /// <summary>
        /// Get default culture
        /// </summary>
        public string DefaultCulture => Context.Set<TXLCulture>().AsNoTracking().SingleOrDefault(x => x.IsDefault == true)?.ID;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, GetLocalizedString(name, arguments));

        LocalizedString IStringLocalizer.this[string name] => new LocalizedString(name, GetLocalizedString(name));

        /// <summary>
        /// Initialize ExpressLocalizationDataManager
        /// </summary>
        /// <param name="context"></param>
        public XLDbLocalizer(TContext context)
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
        public XLDbLocalizer(TContext context, ExpressLocalizationOptions options)
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
            var trans = Context.Set<TXLResource>().AsNoTracking()
                .Include(x => x.Translations)
                .Where(x => x.Key != null && x.Key == key)
                .Select(x => new
                {
                    Translation = x.Translations.Where(t => t.CultureName == culture).FirstOrDefault().Value
                }).FirstOrDefault()?.Translation;

            return args == null
                ? new LocalizedHtmlString(name: key, trans ?? key, true)
                : new LocalizedHtmlString(name: key, trans ?? key, false, args);
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
