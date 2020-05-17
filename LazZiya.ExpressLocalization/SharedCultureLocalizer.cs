using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    public class SharedCultureLocalizer : ISharedCultureLocalizer
    {
        private readonly IHtmlLocalizer _localizer;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, GetLocalizedString(name, arguments));

        LocalizedString IStringLocalizer.this[string name] => new LocalizedString(name, GetLocalizedString(name));

        /// <summary>
        /// Shared culture localizer for razor pages views
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="type"></param>
        public SharedCultureLocalizer(IHtmlLocalizerFactory factory, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type.Name, assemblyName.Name);
        }

        /// <summary>
        /// Get localized formatted string for the provided text
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return GetLocalizedString(key);
            }
        }

        /// <summary>
        /// Get localized formatted string for the provided text with args
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <param name="args">List of object arguments for formatted texts</param>
        /// <returns></returns>
        public string this[string key, params object[] args]
        {
            get
            {
                return GetLocalizedString(key, args);
            }
        }


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
                _localizer[key].WriteTo(sw, HtmlEncoder.Default);
            else
                _localizer[key, args].WriteTo(sw, HtmlEncoder.Default);

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
                    _localizer[key].WriteTo(sw, HtmlEncoder.Default);
                else
                    _localizer[key, args].WriteTo(sw, HtmlEncoder.Default);
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
                ? _localizer[key]
                : _localizer[key, args];
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

                    ? _localizer[key]
                    : _localizer[key, args];
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
