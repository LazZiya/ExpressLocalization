using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ExpressLocalization extensions for .resx resource files based resources
    /// </summary>
    public static class ExpressLocalizationExtensions
    {        
        /// <summary>
        /// Add ExpressLocalization based on ".resx" resource files
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder)
            where TResource : IExpressResource
        {
            var ops = new ExpressLocalizationOptions();

            return builder.AddExpressLocalizationResx<TResource>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization based on ".resx" resource files
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TResource : IExpressResource
        {
            builder.Services.Configure<ExpressLocalizationOptions>(options);

            // ExpressMemoryCache for caching localized values
            builder.Services.AddSingleton<ExpressMemoryCache>();

            // Register IStringLocalizer for the default shared resource type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IStringLocalizer, ResxStringLocalizer<TResource>>();
            builder.Services.AddSingleton<IStringLocalizerFactory, ResxStringLocalizerFactory<TResource>>();

            // Register IHtmlLocalizer for the default shared resource type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IHtmlLocalizer, ResxHtmlLocalizer<TResource>>();
            builder.Services.AddSingleton<IHtmlLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();

            // Register generic localizers for user defined resource entities
            // e.g. IStringLocalizer<ProductArea>
            // e.g. IStringLocalizer<UserArea>
            builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(ResxStringLocalizer<>));
            builder.Services.AddTransient(typeof(IHtmlLocalizer<>), typeof(ResxHtmlLocalizer<>));

            // Express localizer factories for creating localizers with the default shared resource type
            // Use .Create() method for creating localizers.
            builder.Services.AddSingleton<IExpressStringLocalizerFactory, ResxStringLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IExpressHtmlLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();

            return builder.AddDataAnnotationsLocalization<TResource>()
                          .AddModelBindingLocalization()
                          .AddIdentityErrorsLocalization();
        }
    }
}
