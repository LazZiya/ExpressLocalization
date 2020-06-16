using LazZiya.ExpressLocalization.Xml;
using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// HtmlTranslator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HtmlTranslator<T> : HtmlTranslator
        where T : ITranslationService
    {
        /// <summary>
        /// Initialize new intance of HtmlTranslator
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        /// <param name="logger"></param>
        public HtmlTranslator(IOptions<ExpressLocalizationOptions> options, IEnumerable<ITranslationService> translationServices, ILogger<HtmlTranslator> logger)
            : base(typeof(T), options, translationServices, logger)
        {

        }
    }

    /// <summary>
    /// HtmlTranslator
    /// </summary>
    public class HtmlTranslator : IHtmlTranslator
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly ITranslationService _translationService;
        private readonly ILogger _logger;
        /// <summary>
        /// Initialize new intance of HtmlTranslator
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        /// <param name="logger"></param>
        public HtmlTranslator(Type type, IOptions<ExpressLocalizationOptions> options, IEnumerable<ITranslationService> translationServices, ILogger<HtmlTranslator> logger)
        {
            _options = options;
            _translationService = translationServices.FirstOrDefault(x => x.GetType() == type);
            _logger = logger;
        }

        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] => GetTranslatedHtmlString(name, null, null);

        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] => GetTranslatedHtmlString(name, null, null, arguments);

        /// <summary>
        /// Get translated html string to the target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, string toLanguage, params object[] arguments] => GetTranslatedHtmlString(name, null, toLanguage, null);

        /// <summary>
        /// Get translated html string for specified source-target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, string fromLanguage, string toLanguage, params object[] arguments] => GetTranslatedHtmlString(name, fromLanguage, toLanguage, null);

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
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name)
        {
            return GetLocalizedString(name, null, null, null);
        }

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, string toLanguage)
        {
            return GetLocalizedString(name, null, toLanguage, null);
        }

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, string fromLanguage, string toLanguage)
        {
            return GetLocalizedString(name, fromLanguage, toLanguage, null);
        }

        /// <summary>
        /// Get translated string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return GetLocalizedString(name, null, null, arguments);
        }

        /// <summary>
        /// Get translated string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, string toLanguage, params object[] arguments)
        {
            return GetLocalizedString(name, null, toLanguage, arguments);
        }

        /// <summary>
        /// Get translated string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, string fromLanguage, string toLanguage, params object[] arguments)
        {
            return GetLocalizedString(name, fromLanguage, toLanguage, arguments);
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a localized string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private LocalizedHtmlString GetTranslatedHtmlString(string key, string fromLanguage, string toLanguage, params object[] arguments)
        {
            var trans = Translate(key, fromLanguage, toLanguage, "html");

            var value = arguments == null
                ? trans.Text
                : string.Format(trans.Text, arguments);

            return trans.StatusCode == HttpStatusCode.OK
                ? new LocalizedHtmlString(key, value, false)
                : new LocalizedHtmlString(key, key, true);
        }

        private LocalizedString GetLocalizedString(string key, string fromLanguage, string toLanguage, params object[] arguments)
        {
            var trans = Translate(key, fromLanguage, toLanguage, "text");

            var value = arguments == null
                ? trans.Text
                : string.Format(trans.Text, arguments);

            return trans.StatusCode == HttpStatusCode.OK
                ? new LocalizedString(key, value, false)
                : new LocalizedString(key, key, true);
        }

        private TranslationResult Translate(string key, string from, string to, string format)
        {
            var _source = string.IsNullOrWhiteSpace(from)
                ? _options.Value.DefaultCultureName
                : from;

            var _target = string.IsNullOrWhiteSpace(to)
                ? CultureInfo.CurrentCulture.Name
                : to;

            _logger.LogInformation($"TO : {to} - {_target} - {CultureInfo.CurrentCulture.Name}");
            _logger.LogInformation($"FROM : {from} - {_source} - {_options.Value.DefaultCultureName} - {key}");

            var trans = _translationService.TranslateAsync(_source, _target, key, format);

            return trans.Result;
        }
    }
}
