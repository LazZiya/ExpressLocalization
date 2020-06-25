using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// Add ExpressLocalization with .resx resource files based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            builder.Services.TryAddTransient<IStringLocalizer, ResxStringLocalizer<TResource>>();
            builder.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ResxStringLocalizer<>));
            builder.Services.TryAddSingleton<IStringLocalizerFactory, ResxStringLocalizerFactory<TResource>>();
            builder.Services.TryAddSingleton<IStringExpressLocalizerFactory, ResxStringLocalizerFactory<TResource>>();

            builder.Services.TryAddTransient<IHtmlLocalizer, ResxHtmlLocalizer<TResource>>();
            builder.Services.TryAddTransient(typeof(IHtmlLocalizer<>), typeof(ResxHtmlLocalizer<>));
            builder.Services.TryAddSingleton<IHtmlLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();
            builder.Services.TryAddSingleton<IHtmlExpressLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();

            return builder.AddDataAnnotationsLocalization<TResource>()
                          .AddModelBindingLocalization<TResource>()
                          .AddIdentityErrorsLocalization<TResource>();
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : class
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationResx<TResource>();
        }
    }
}
