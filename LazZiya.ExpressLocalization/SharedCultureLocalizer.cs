using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    public class SharedCultureLocalizer
    {
        private readonly IHtmlLocalizer _localizer;

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
            var sw = new StringWriter();

            GenericResourceReader.GetValue(resourceSource, culture, key, args).WriteTo(sw, HtmlEncoder.Default);

            return sw.ToString();
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
            var sw = new StringWriter();
            GenericResourceReader.GetValue(resourceSource, CultureInfo.CurrentCulture.Name, key, args).WriteTo(sw, HtmlEncoder.Default);

            return sw.ToString();
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
            return GenericResourceReader.GetValue(resourceSource, culture, key, args);
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
            return GenericResourceReader.GetValue(resourceSource, CultureInfo.CurrentCulture.Name, key, args);
        }

        #region Obsolete methods to be removed in next release

        /// <summary>
        /// Get localized string for the provided text.
        /// <para>Use in UI side, for backend text localization use FormattedText instead</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        [Obsolete("The method Text is obsolete and will be removed in a feature release! use GetLocalizedString instead.")]
        public LocalizedHtmlString Text(string key, params object[] args)
        {
            return args == null
                ? _localizer[key]
                : _localizer[key, args];
        }

        /// <summary>
        /// Localize a string according to specified culture
        /// <para>Use in UI side, for backend text localization use FormattedText instead</para>
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        [Obsolete("The method Text is obsolete and will be removed in a feature release! use GetLocalizedString instead.")]
        public LocalizedHtmlString Text(string culture, string key, params object[] args)
        {
            return args == null
                ? _localizer.WithCulture(CultureInfo.GetCultureInfo(culture))[key]
                : _localizer.WithCulture(CultureInfo.GetCultureInfo(culture))[key, args];
        }

        /// <summary>
        /// Localize a string according to a specific culture and specified resource type
        /// <para>Use in UI side, for backend text localization use FormattedText instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        [Obsolete("The method Text is obsolete and will be removed in a feature release! use GetLocalizedString instead.")]
        public LocalizedHtmlString Text(Type resourceSource, string culture, string key, params object[] args)
        {
            return GenericResourceReader.GetValue(resourceSource, culture, key, args);
        }

        /// <summary>
        /// Localize a string value from specified culture resource
        /// <para>Use in UI side, for backend text localization use FormattedText instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [Obsolete("The method Text is obsolete and will be removed in a feature release! use GetLocalizedString instead.")]
        public LocalizedHtmlString Text(Type resourceSource, string key, params object[] args)
        {
            return GenericResourceReader.GetValue(resourceSource, CultureInfo.CurrentCulture.Name, key, args);
        }
        #endregion
    }
}
