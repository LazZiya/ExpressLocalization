using LazZiya.ExpressLocalization.Xml;
using LazZiya.TranslationServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// HtmlTranslatorFactory
    /// </summary>
    public class HtmlTranslatorFactory<TService> : IHtmlTranslatorFactory
        where TService : ITranslationService
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IEnumerable<ITranslationService> _translationServices;
        private readonly ILogger<HtmlTranslator> _logger;

        /// <summary>
        /// Initialize a new intance of HtmlTranslatorFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationServices"></param>
        public HtmlTranslatorFactory(IOptions<ExpressLocalizationOptions> options, IEnumerable<ITranslationService> translationServices, ILogger<HtmlTranslator> logger)
        {
            _options = options;
            _translationServices = translationServices;
            _logger = logger;
        }

        /// <summary>
        /// Create a new instance of HtmlTranslator
        /// </summary>
        /// <returns></returns>
        public IHtmlTranslator Create()
        {
            return new HtmlTranslator<TService>(_options, _translationServices, _logger);
        }

        /// <summary>
        /// Create a new instance of HtmlTranslator
        /// </summary>
        /// <returns></returns>
        public IHtmlTranslator Create(Type type)
        {
            if (type == null)
                throw new NullReferenceException(nameof(ITranslationService));
            
            if (type.GetInterface(typeof(ITranslationService).FullName) == null)
                throw new Exception($"The provided type is of type {type.FullName}, but this service must implement {typeof(ITranslationService)}");

            return new HtmlTranslator(type, _options, _translationServices, _logger);
        }

        /// <summary>
        /// Create new instance of IHtmlTranslator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IHtmlTranslator Create<T>()
            where T : ITranslationService
        {
            return new HtmlTranslator<T>(_options, _translationServices, _logger);
        }
    }
}
