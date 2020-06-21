using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DataAnnotations;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
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
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<T>(this IMvcBuilder builder)
            where T : class
        {

            builder.Services.AddTransient<IStringLocalizer, XmlStringLocalizer<T>>();
            builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(XmlStringLocalizer<>));
            builder.Services.AddTransient<IStringLocalizerFactory, XmlStringLocalizerFactory<T>>();
            builder.Services.AddTransient<IStringExpressLocalizerFactory, XmlStringLocalizerFactory<T>>();
            
            builder.Services.AddTransient<IHtmlLocalizer, XmlHtmlLocalizer<T>>();
            builder.Services.AddTransient(typeof(IHtmlLocalizer<>), typeof(XmlHtmlLocalizer<>));
            builder.Services.AddTransient<IHtmlLocalizerFactory, XmlHtmlLocalizerFactory<T>>();
            builder.Services.AddTransient<IHtmlExpressLocalizerFactory, XmlHtmlLocalizerFactory<T>>();

            builder.ExAddDataAnnotationsLocalization<T>(true);

            return builder;
        }
        
        /// <summary>
        /// Add ExpressLocalization with Xml based resources
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="T">Resource type</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<T>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where T : class
        {
            builder.Services.Configure<ExpressLocalizationOptions>(xOps);

            return builder.AddExpressLocalizationXml<T>();
        }
        
        /// <summary>
        /// Add ExpressLocalization with Xml based resources.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="xOps"></param>
        /// <typeparam name="T">Resource type</typeparam>
        /// <typeparam name="U">ITranslationService</typeparam>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationXml<T, U>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> xOps)
            where T : class
            where U : ITranslationService
        {
            builder.Services.AddTransient<IStringTranslator, StringTranslator<U>>();
            builder.Services.AddTransient<IHtmlTranslator, HtmlTranslator<U>>();
            builder.Services.AddTransient<IHtmlTranslatorFactory, HtmlTranslatorFactory>();
            builder.Services.AddTransient<IStringTranslatorFactory, StringTranslatorFactory>();

            builder.Services.AddTransient<ITranslationServiceFactory, TranslationServiceFactory<U>>();

            return builder.AddExpressLocalizationXml<T>(xOps);
        }


        /// <summary>
        /// Add DataAnnotatons localization to the project.
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="T">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <param name="useExpressValidationAttributes">Express validiation attributes provides already localized eror messages</param>
        /// <returns></returns>
        private static IMvcBuilder ExAddDataAnnotationsLocalization<T>(this IMvcBuilder builder, bool useExpressValidationAttributes)
            where T : class
        {
            builder.AddDataAnnotationsLocalization(ops =>
            {
                // This will look for localization resource with type of T
                ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(T));

                // This will look for locaization resources of caller page type 
                // e.g. LoginModel.en.xml
                //ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(t);
            });

            if (useExpressValidationAttributes)
                builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<T>>();

            return builder;
        }
    }
}
