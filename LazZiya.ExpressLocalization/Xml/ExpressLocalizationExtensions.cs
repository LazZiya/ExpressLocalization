using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
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
    public static class ExpressLocalizationExtensions
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
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : class
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationXml<TResource>();
        }
    }
}
