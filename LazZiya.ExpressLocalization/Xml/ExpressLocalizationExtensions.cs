using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// ExpressLocalization extensions for Xml based resources
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder)
            where TResource : IXLResource
        {
            var ops = new ExpressLocalizationOptions();

            // Register dummy translatio service to avoid startup exceptions
            builder.Services.AddTransient<ITranslationService, DummyTranslationService>();

            return builder
                .AddExpressLocalizationXml<TResource, DummyTranslationService>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : IXLResource
        {
            // Register dummy translatio service to avoid startup exceptions
            builder.Services.AddTransient<ITranslationService, DummyTranslationService>();

            return builder
                .AddExpressLocalizationXml<TResource, DummyTranslationService>(xOps);
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources,
        /// and use defined translation service type
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource, TService>(this IMvcBuilder builder)
            where TResource : IXLResource
            where TService : ITranslationService
        {
            var ops = new ExpressLocalizationOptions();

            return builder
                .AddExpressLocalizationXml<TResource, TService>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources,
        /// and use defined translation service type
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<TResource, TService>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : IXLResource
            where TService : ITranslationService
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            // ExpressMemoryCache for caching localized values
            builder.Services.AddSingleton<ExpressMemoryCache>();

            // Register IStringLocalizer for the default shared resource type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IStringLocalizer, XmlStringLocalizer<TResource>>();
            builder.Services.AddSingleton<IStringLocalizerFactory, XmlStringLocalizerFactory<TResource>>();

            // Register IHtmlLocalizer for the default shared resource type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IHtmlLocalizer, XmlHtmlLocalizer<TResource>>();
            builder.Services.AddSingleton<IHtmlLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();

            // Register generic localizers for user defined resource entities
            // e.g. IStringLocalizer<ProductArea>
            // e.g. IStringLocalizer<UserArea>
            builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(XmlStringLocalizer<>));
            builder.Services.AddTransient(typeof(IHtmlLocalizer<>), typeof(XmlHtmlLocalizer<>));
            
            // Express localizer factories for creating localizers with the default shared resource type
            // Use .Create() method for creating localizers.
            builder.Services.AddSingleton<IExpressStringLocalizerFactory, XmlStringLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IExpressHtmlLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();

            return builder.AddDataAnnotationsLocalization<TResource>()
                          .AddModelBindingLocalization()
                          .AddIdentityErrorsLocalization()
                          .WithTranslationService<TService>();
        }
    }
}
