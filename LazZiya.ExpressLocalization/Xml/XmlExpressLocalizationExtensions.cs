using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// ExpressLocalization extensions for Xml based resources
    /// </summary>
    public static class XmlExpressLocalizationExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            builder.Services.TryAddTransient<IStringLocalizer, XmlStringLocalizer<TResource>>();
            builder.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(XmlStringLocalizer<>));
            builder.Services.TryAddSingleton<IStringLocalizerFactory, XmlStringLocalizerFactory<TResource>>();
            builder.Services.TryAddSingleton<IStringExpressLocalizerFactory, XmlStringLocalizerFactory<TResource>>();

            builder.Services.TryAddTransient<IHtmlLocalizer, XmlHtmlLocalizer<TResource>>();
            builder.Services.TryAddTransient(typeof(IHtmlLocalizer<>), typeof(XmlHtmlLocalizer<>));
            builder.Services.TryAddSingleton<IHtmlLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();
            builder.Services.TryAddSingleton<IHtmlExpressLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();

            builder.ExAddDataAnnotationsLocalizationXml<TResource>();

            return builder;
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : class
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationXml<TResource>();
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <typeparam name="TTransService">ITranslationService</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource, TTransService>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : class
            where TTransService : ITranslationService
        {
            builder.Services.TryAddTransient<IStringTranslator, StringTranslator<TTransService>>();
            builder.Services.TryAddTransient<IHtmlTranslator, HtmlTranslator<TTransService>>();
            builder.Services.TryAddSingleton<IHtmlTranslatorFactory, HtmlTranslatorFactory>();
            builder.Services.TryAddSingleton<IStringTranslatorFactory, StringTranslatorFactory>();

            builder.Services.TryAddSingleton<ITranslationServiceFactory, TranslationServiceFactory<TTransService>>();

            return builder.AddExpressLocalizationXml<TResource>(xOps);
        }

        /// <summary>
        /// Add DataAnnotations, ModelBinding and IdentityErrors localization to the project.
        /// </summary>
        /// <typeparam name="TResource">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IMvcBuilder ExAddDataAnnotationsLocalizationXml<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            // Add ExpressValdiationAttributes to provide error messages by default without using ErrorMessage="..."
            // After removing support for old behaviour use below implementation
            // builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<T>>();
            
            // This is a temporary solution to provide support for the old behaviour with resx files
            builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<TResource>>((x) => new ExpressValidationAttributeAdapterProvider<TResource>(false));

            // Add data annotations locailzation
            builder.AddDataAnnotationsLocalization(ops =>
            {
                // This will look for localization resource with type of T (shared resource)
                ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(TResource));

                // This will look for localization resources depending on specific type, e.g. LoginModel.en.xml
                //ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(t);
            });

            // Add ModelBinding errors localization
            builder.AddMvcOptions(ops =>
            {
                var factory = builder.Services.BuildServiceProvider().GetService(typeof(IStringExpressLocalizerFactory)) as IStringExpressLocalizerFactory;
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(factory);
            });

            // Add Identity Erros localization
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer>();

            return builder;
        }
    }
}
