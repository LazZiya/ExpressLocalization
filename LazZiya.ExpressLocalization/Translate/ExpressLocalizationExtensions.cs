using LazZiya.TranslationServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            builder.Services.TryAddTransient<IStringTranslator, StringTranslator<TTransService>>();
            builder.Services.TryAddTransient<IHtmlTranslator, HtmlTranslator<TTransService>>();
            builder.Services.TryAddSingleton<IHtmlTranslatorFactory, HtmlTranslatorFactory>();
            builder.Services.TryAddSingleton<IStringTranslatorFactory, StringTranslatorFactory>();

            builder.Services.TryAddSingleton<ITranslationServiceFactory, TranslationServiceFactory<TTransService>>();

            return builder;
        }
    }
}
