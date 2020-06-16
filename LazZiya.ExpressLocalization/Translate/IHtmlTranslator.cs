using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// IHtmlTranslator interface
    /// </summary>
    public interface IHtmlTranslator : IHtmlLocalizer
    {
        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        LocalizedHtmlString this[string name] { get; }

        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedHtmlString this[string name, params object[] arguments] { get; }

        /// <summary>
        /// Get translated html string to the target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedHtmlString this[string name, string toLanguage, params object[] arguments] { get; }

        /// <summary>
        /// Get translated html string for specified source-target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedHtmlString this[string name, string fromLanguage, string toLanguage, params object[] arguments] { get; }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        LocalizedString GetString(string name);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedString GetString(string name, params object[] arguments);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        LocalizedString GetString(string name, string toLanguage);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedString GetString(string name, string toLanguage, params object[] arguments);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        LocalizedString GetString(string name, string fromLanguage, string toLanguage);

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedString GetString(string name, string fromLanguage, string toLanguage, params object[] arguments);

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        IHtmlLocalizer WithCulture(CultureInfo culture);
    }
}