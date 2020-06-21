using Microsoft.Extensions.Localization;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// IStringTranslator interface
    /// </summary>
    public interface IStringTranslator : IStringLocalizer
    {
        /// <summary>
        /// Get translated string to the target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedString this[string name, string toLanguage, params object[] arguments] { get; }

        /// <summary>
        /// Get translated html string for specified source-target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        LocalizedString this[string name, string fromLanguage, string toLanguage, params object[] arguments] { get; }

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
    }
}