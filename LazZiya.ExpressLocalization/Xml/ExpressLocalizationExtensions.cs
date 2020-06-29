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
            where TResource : IXLResource
        {
            builder.Services.AddSingleton<IStringLocalizer, XmlStringLocalizer<TResource>>();
            builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(XmlStringLocalizer<>));
            builder.Services.AddSingleton<IStringLocalizerFactory, XmlStringLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IStringExpressLocalizerFactory, XmlStringLocalizerFactory<TResource>>();

            builder.Services.AddSingleton<IHtmlLocalizer, XmlHtmlLocalizer<TResource>>();
            builder.Services.AddSingleton(typeof(IHtmlLocalizer<>), typeof(XmlHtmlLocalizer<>));
            builder.Services.AddSingleton<IHtmlLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();
            builder.Services.AddSingleton<IHtmlExpressLocalizerFactory, XmlHtmlLocalizerFactory<TResource>>();

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
        public static IMvcBuilder AddExpressLocalizationXml<TResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where TResource : IXLResource
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationXml<TResource>();
        }
    }
}
