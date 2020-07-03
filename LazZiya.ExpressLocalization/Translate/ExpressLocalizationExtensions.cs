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
            // ExpressTranslator, the service that will connect to the default translation provider
            builder.Services.AddSingleton<IExpressTranslator, ExpressTranslator<TService>>();
            builder.Services.AddTransient(typeof(IExpressTranslator<>), typeof(ExpressTranslator<>));

            return builder;
        }
    }
}
