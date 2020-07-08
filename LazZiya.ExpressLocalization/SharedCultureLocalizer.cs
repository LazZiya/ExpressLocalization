using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using LazZiya.ExpressLocalization.Common;
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    [Obsolete("ISharedCultureLocalizer is deprected. use IStringLocalizer or IHtmlLocalizer instead. See <a href=\"https://github.com/LazZiya/ExpressLocalization/wiki/Migration-4.x-to-5.0\">Migration 4.x to 5.0</a>")]
    public class SharedCultureLocalizer<T> : ISharedCultureLocalizer
        where T : class
    {
        private readonly IHtmlLocalizer _localizer;
        private readonly Type _resType;
        private readonly ILogger _logger;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, GetLocalizedString(name, arguments));

        LocalizedString IStringLocalizer.this[string name] => new LocalizedString(name, GetLocalizedString(name));

        /// <summary>
        /// Shared culture localizer for razor pages views
        /// </summary>
        /// <param name="htmlLocalizerFactory"></param>
        /// <param name="logger"></param>
        public SharedCultureLocalizer(IHtmlLocalizerFactory htmlLocalizerFactory,
                                      ILogger<SharedCultureLocalizer<T>> logger)
        {
            _resType = typeof(T);
            _logger = logger;

            var assemblyName = new AssemblyName(_resType.GetTypeInfo().Assembly.FullName);
            _localizer = htmlLocalizerFactory.Create(_resType.Name, assemblyName.Name);
        }

        private LocalizedHtmlString Localizer(string key, params object[] args)
        {
            return args == null ? _localizer[key] : _localizer[key, args];
        }
        /// <summary>
        /// Get localized formatted string for the provided text
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <returns></returns>
        public string this[string key] => GetLocalizedString(key);

        /// <summary>
        /// Get localized formatted string for the provided text with args
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <param name="args">List of object arguments for formatted texts</param>
        /// <returns></returns>
        public string this[string key, params object[] args] => GetLocalizedString(key, args);


        /// <summary>
        /// Get localized formatted string for the provided text with args
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        public string GetLocalizedString(string key, params object[] args)
        {
            var sw = new StringWriter();

            if (args == null)
                Localizer(key).WriteTo(sw, HtmlEncoder.Default);
            else
                Localizer(key, args).WriteTo(sw, HtmlEncoder.Default);

            return sw.ToString();
        }

        /// <summary>
        /// Localize a string according to specified culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        public string GetLocalizedString(string culture, string key, params object[] args)
        {
            var sw = new StringWriter();

            using (var csw = new CultureSwitcher(culture))
            {
                if (args == null)
                    Localizer(key).WriteTo(sw, HtmlEncoder.Default);
                else
                    Localizer(key, args).WriteTo(sw, HtmlEncoder.Default);
            }

            return sw.ToString();
        }

        /// <summary>
        /// Localize a string according to a specific culture and specified resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        public string GetLocalizedString(Type resourceSource, string culture, string key, params object[] args)
        {
            var str = GenericResourceReader.GetString(resourceSource, culture, key, args);

            return str;
        }

        /// <summary>
        /// Localize a string value from specified culture resource
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        public string GetLocalizedString(Type resourceSource, string key, params object[] args)
        {
            var str = GenericResourceReader.GetString(resourceSource, CultureInfo.CurrentCulture.Name, key, args);

            return str;
        }

        /// <summary>
        /// Get localized html string for the provided text.
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        public LocalizedHtmlString GetLocalizedHtmlString(string key, params object[] args)
        {
            return args == null
                ? Localizer(key)
                : Localizer(key, args);
        }

        /// <summary>
        /// Localize a string according to specified culture
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        public LocalizedHtmlString GetLocalizedHtmlString(string culture, string key, params object[] args)
        {
            LocalizedHtmlString htmlStr;

            using (var csw = new CultureSwitcher(culture))
            {
                htmlStr = args == null

                    ? Localizer(key)
                    : Localizer(key, args);
            }

            return htmlStr;
        }

        /// <summary>
        /// Localize a string according to a specific culture and specified resource type
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        public LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string culture, string key, params object[] args)
        {
            return GenericResourceReader.GetHtmlString(resourceSource, culture, key, args);
        }

        /// <summary>
        /// Localize a string value from specified culture resource
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        public LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string key, params object[] args)
        {
            return GenericResourceReader.GetHtmlString(resourceSource, CultureInfo.CurrentCulture.Name, key, args);
        }

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
