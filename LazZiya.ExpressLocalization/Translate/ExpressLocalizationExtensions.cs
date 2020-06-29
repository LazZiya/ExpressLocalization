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
        /// <typeparam name="TTransService">ITranslationService</typeparam>
        /// <returns></returns>
        public static IMvcBuilder WithTranslationService<TTransService>(this IMvcBuilder builder)
            where TTransService : ITranslationService
        {
            builder.Services.AddSingleton<IStringTranslator, StringTranslator<TTransService>>();
            builder.Services.AddSingleton<IHtmlTranslator, HtmlTranslator<TTransService>>();
            builder.Services.AddSingleton<IHtmlTranslatorFactory, HtmlTranslatorFactory>();
            builder.Services.AddSingleton<IStringTranslatorFactory, StringTranslatorFactory>();

            builder.Services.AddSingleton<ITranslationServiceFactory, TranslationServiceFactory<TTransService>>();

            return builder;
        }
    }
}
