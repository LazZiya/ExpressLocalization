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
    public class HtmlTranslator<TService> : HtmlTranslator
        where TService : ITranslationService
    {
        /// <summary>
        /// Initialize new intance of HtmlTranslator
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        /// <param name="logger"></param>
        public HtmlTranslator(IEnumerable<ITranslationService> translationServices, IOptions<ExpressLocalizationOptions> options, ILogger<HtmlTranslator> logger)
            : base(typeof(TService), translationServices, options, logger)
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
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        /// <param name="logger"></param>
        /// <param name="tServiceType"></param>
        public HtmlTranslator(Type tServiceType, IEnumerable<ITranslationService> translationServices, IOptions<ExpressLocalizationOptions> options, ILogger<HtmlTranslator> logger)
        {
            _options = options;
            _translationService = translationServices.FirstOrDefault(x => x.GetType() == tServiceType);
            _logger = logger;
        }

        /// <summary>
        /// Initialize new intance of HtmlTranslator
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationService"></param>
        /// <param name="logger"></param>
        public HtmlTranslator(ITranslationService translationService, IOptions<ExpressLocalizationOptions> options, ILogger<HtmlTranslator> logger)
        {
            _options = options;
            _translationService = translationService;
            _logger = logger;
        }

        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name] =>
            GetTranslatedHtmlString(name, fromLanguage: null, toLanguage: null);

        /// <summary>
        /// Get translated html string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, params object[] arguments] =>
            GetTranslatedHtmlString(name, fromLanguage: null, toLanguage: null, arguments: arguments);

        /// <summary>
        /// Get translated html string to the target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, string toLanguage, params object[] arguments] =>
            GetTranslatedHtmlString(name, fromLanguage: null, toLanguage: toLanguage, arguments: arguments);

        /// <summary>
        /// Get translated html string for specified source-target language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedHtmlString this[string name, string fromLanguage, string toLanguage, params object[] arguments] =>
            GetTranslatedHtmlString(name, fromLanguage: fromLanguage, toLanguage: toLanguage, arguments: null);

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
            return GetLocalizedString(name, fromLanguage: null, toLanguage: null);
        }

        /// <summary>
        /// Get translated string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, string toLanguage)
        {
            return GetLocalizedString(name, fromLanguage: null, toLanguage: toLanguage);
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
            return GetLocalizedString(name, fromLanguage: fromLanguage, toLanguage: toLanguage);
        }

        /// <summary>
        /// Get translated string with arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return GetLocalizedString(name, fromLanguage: null, toLanguage: null, arguments: arguments);
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
            return GetLocalizedString(name, fromLanguage: null, toLanguage: toLanguage, arguments: arguments);
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
            return GetLocalizedString(name, fromLanguage: fromLanguage, toLanguage: toLanguage, arguments: arguments);
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

            var value = key;
            var isResourceNotFound = true;

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                value = trans.Text;
                isResourceNotFound = false;
            }

            if (arguments != null && arguments.Length > 0)
            {
                value = string.Format(value, arguments);
            }

            return new LocalizedHtmlString(key, value, isResourceNotFound);
        }

        private LocalizedString GetLocalizedString(string key, string fromLanguage, string toLanguage, params object[] arguments)
        {
            var trans = Translate(key, fromLanguage, toLanguage, "text");

            var value = key;
            var isResourceNotFound = true;

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                value = trans.Text;
                isResourceNotFound = false;
            }

            if (arguments != null && arguments.Length > 0)
            {
                value = string.Format(value, arguments);
            }

            return new LocalizedString(key, value, isResourceNotFound);
        }

        private TranslationResult Translate(string key, string from, string to, string format)
        {
            var _source = string.IsNullOrWhiteSpace(from)
                ? _options.Value.DefaultCultureName
                : from;

            var _target = string.IsNullOrWhiteSpace(to)
                ? CultureInfo.CurrentCulture.Name
                : to;

            var trans = _translationService.TranslateAsync(_source, _target, key, format);

            return trans.Result;
        }
    }
}
