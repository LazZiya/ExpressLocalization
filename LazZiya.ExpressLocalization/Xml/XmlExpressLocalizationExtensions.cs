using LazZiya.ExpressLocalization.Common;
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
    public static class XmlExpressLocalizationExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with Xml based resources. XmlResource type must implemet <see cref="IXmlResource"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="T">XmlResource type, must implement IXmlResource</typeparam>
        /// <see cref="IXmlResource"/>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<T>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where T : IXmlResource
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);
            builder.Services.AddScoped<IStringExpressLocalizerFactory, XmlStringLocalizerFactory<T>>();
            builder.Services.AddScoped<IHtmlExpressLocalizerFactory, XmlHtmlLocalizerFactory<T>>();

            builder.Services.AddScoped<IHtmlLocalizer, XmlHtmlLocalizer<T>>();
            builder.Services.AddScoped<IStringLocalizer, XmlStringLocalizer<T>>();

            return builder;
        }
        
        /// <summary>
        /// Add ExpressLocalization with Xml based resources. XmlResource type must implemet <see cref="IXmlResource"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="T">IXmlResource</typeparam>
        /// <typeparam name="U">ITranslationService</typeparam>
        /// <see cref="IXmlResource"/>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<T, U>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where T : IXmlResource
            where U : ITranslationService
        {
            builder.Services.AddScoped<IHtmlTranslatorFactory, HtmlTranslatorFactory<U>>();
            builder.Services.AddScoped<IHtmlTranslator, HtmlTranslator<U>>();

            return builder.AddExpressLocalizationXml<T>(xOps);
        }
    }
}
