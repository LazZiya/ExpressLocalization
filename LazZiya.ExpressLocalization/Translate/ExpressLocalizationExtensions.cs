using LazZiya.TranslationServices;
using Microsoft.Extensions.DependencyInjection;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// Extensions to use translation services with ExpressLocalization
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TService">ITranslationService</typeparam>
        /// <returns></returns>
        public static IMvcBuilder WithTranslationService<TService>(this IMvcBuilder builder)
            where TService : ITranslationService
        {
            // String and Html translators with the default translation service
            builder.Services.AddSingleton<IStringTranslator, StringTranslator<TService>>();
            builder.Services.AddSingleton<IHtmlTranslator, HtmlTranslator<TService>>();
            
            // Generic String and Html translators with user defined translation service
            builder.Services.AddSingleton(typeof(IStringTranslator<>), typeof(StringTranslator<>));
            builder.Services.AddSingleton(typeof(IHtmlTranslator<>), typeof(HtmlTranslator<>));

            // Translator factories
            builder.Services.AddSingleton<IHtmlTranslatorFactory, HtmlTranslatorFactory>();
            builder.Services.AddSingleton<IStringTranslatorFactory, StringTranslatorFactory>();

            // Translation service factory, used to provide translation services for Translator Factories
            builder.Services.AddSingleton<ITranslationServiceFactory, TranslationServiceFactory<TService>>();

            return builder;
        }
    }
}
