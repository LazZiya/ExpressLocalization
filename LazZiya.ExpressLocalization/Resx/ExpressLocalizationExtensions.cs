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
        /// Add ExpressLocalization with .resx resource files based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder)
            where TResource : IXLResource
        {
            builder.Services.AddSingleton<IStringLocalizer, ResxStringLocalizer<TResource>>();
            builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(ResxStringLocalizer<>));
            builder.Services.AddSingleton<IStringLocalizerFactory, ResxStringLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IStringExpressLocalizerFactory, ResxStringLocalizerFactory<TResource>>();

            builder.Services.AddSingleton<IHtmlLocalizer, ResxHtmlLocalizer<TResource>>();
            builder.Services.AddSingleton(typeof(IHtmlLocalizer<>), typeof(ResxHtmlLocalizer<>));
            builder.Services.AddSingleton<IHtmlLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IHtmlExpressLocalizerFactory, ResxHtmlLocalizerFactory<TResource>>();

            return builder.AddDataAnnotationsLocalization<TResource>()
                          .AddModelBindingLocalization()
                          .AddIdentityErrorsLocalization();
        }

        /// <summary>
        /// Add ExpressLocalization with Xml based resources
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="TResource">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationResx<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : IXLResource
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationResx<TResource>();
        }
    }
}
