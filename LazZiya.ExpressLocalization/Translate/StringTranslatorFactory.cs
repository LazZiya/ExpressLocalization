using LazZiya.TranslationServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// StringTranslatorFactory
    /// </summary>
    public class StringTranslatorFactory : IStringTranslatorFactory
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly ITranslationServiceFactory _translationServiceFactory;
        private readonly ILogger<StringTranslator> _logger;

        /// <summary>
        /// Initialize a new intance of StringTranslatorFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationServiceFactory"></param>
        /// <param name="logger"></param>
        public StringTranslatorFactory(IOptions<ExpressLocalizationOptions> options, ITranslationServiceFactory translationServiceFactory, ILogger<StringTranslator> logger)
        {
            _options = options;
            _translationServiceFactory = translationServiceFactory;
            _logger = logger;
        }

        /// <summary>
        /// Create a new instance of StringTranslator
        /// </summary>
        /// <returns></returns>
        public IStringTranslator Create()
        {
            var tService = _translationServiceFactory.Create();
            return new StringTranslator(tService, _options, _logger);
        }
        
        /// <summary>
        /// Create a new instance of HtmlTranslator
        /// </summary>
        /// <returns></returns>
        public IStringTranslator Create(Type type)
        {
            if (type == null)
                throw new NullReferenceException(nameof(ITranslationService));
            
            if (type.GetInterface(typeof(ITranslationService).FullName) == null)
                throw new Exception($"The provided type is of type {type.FullName}, but this service must implement {typeof(ITranslationService)}");

            var tService = _translationServiceFactory.Create(type);
            return new StringTranslator(tService, _options, _logger);
        }
        
        /// <summary>
        /// Create new instance of IHtmlTranslator
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public IStringTranslator Create<TService>()
            where TService : ITranslationService
        {
            var tService = _translationServiceFactory.Create<TService>();
            return new StringTranslator(tService, _options, _logger);
        }
    }
}
