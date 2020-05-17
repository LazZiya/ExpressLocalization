using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    public interface ISharedCultureLocalizer : IStringLocalizer
    {
        /// <summary>
        /// Get localized formatted string for the provided text
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <returns></returns>
        string this[string key] { get; }

        /// <summary>
        /// Get localized formatted string for the provided text with args
        /// </summary>
        /// <param name="key">The text to be localized</param>
        /// <param name="args">List of object arguments for formatted texts</param>
        /// <returns></returns>
        string this[string key, params object[] args] { get; }

        /// <summary>
        /// Get localized html string for the provided text.
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        LocalizedHtmlString GetLocalizedHtmlString(string key, params object[] args);

        /// <summary>
        /// Localize a string according to specified culture
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        LocalizedHtmlString GetLocalizedHtmlString(string culture, string key, params object[] args);

        /// <summary>
        /// Localize a string value from specified culture resource
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string key, params object[] args);

        /// <summary>
        /// Localize a string according to a specific culture and specified resource type
        /// <para>Use in UI side, for backend text localization use GetLocalizedString instead</para>
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>LocalizedHtmlString</returns>
        LocalizedHtmlString GetLocalizedHtmlString(Type resourceSource, string culture, string key, params object[] args);

        /// <summary>
        /// Get localized formatted string for the provided text with args
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        string GetLocalizedString(string key, params object[] args);

        /// <summary>
        /// Localize a string according to specified culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        string GetLocalizedString(string culture, string key, params object[] args);

        /// <summary>
        /// Localize a string value from specified culture resource
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        string GetLocalizedString(Type resourceSource, string key, params object[] args);

        /// <summary>
        /// Localize a string according to a specific culture and specified resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns>localized string</returns>
        string GetLocalizedString(Type resourceSource, string culture, string key, params object[] args);
    }
}
