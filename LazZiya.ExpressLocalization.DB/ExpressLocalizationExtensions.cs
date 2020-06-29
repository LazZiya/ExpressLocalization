using LazZiya.ExpressLocalization.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LazZiya.ExpressLocalization.ModelBinding;
using Microsoft.Extensions.Localization;
using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB extensions
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with database support using the built-in entity models
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <param name="builder">builder</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext>(this IMvcBuilder builder)
            where TContext : DbContext
        {
            return builder
                .AddExpressLocalizationDb<TContext, XLResource, XLTranslation, XLCulture>();
        }

        /// <summary>
        /// Add ExpressLocalization support using the built-in entity models
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TContext : DbContext
        {
            return builder
                .AddExpressLocalizationDb<TContext, XLResource, XLTranslation, XLCulture>(options);
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TResource">Type of resource DbEntity</typeparam>
        /// <typeparam name="TTranslation">Type of translation entityy</typeparam>
        /// <typeparam name="TCulture">Type of culture DbEntity</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TResource, TTranslation, TCulture>(this IMvcBuilder builder)
            where TContext : DbContext
            where TResource : class, IXLDbResource
            where TTranslation : class, IXLDbTranslation
            where TCulture : class, IXLCulture
        {
            return builder
                .AddExpressLocalizationDb<TContext, TResource, TTranslation, TCulture>();
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TResource">Type of localization DbEntity</typeparam>
        /// <typeparam name="TTranslation">Type of translation entity</typeparam>
        /// <typeparam name="TCulture">Type of culture DbEntity</typeparam>
        /// <param name="options">XLDbOptions</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TResource, TTranslation, TCulture>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TContext : DbContext
            where TResource : class, IXLDbResource
            where TTranslation : class, IXLDbTranslation
            where TCulture : class, IXLCulture
        {
            builder.Services.Configure<ExpressLocalizationOptions>(options);

            builder.Services.AddSingleton<IEFGenericDataManager, EFGenericDataManager<TContext>>();

            builder.Services.AddSingleton<IStringLocalizer, DbStringLocalizer<TResource, TTranslation>>();
            //builder.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(DbStringLocalizer<,>));
            builder.Services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory<TResource, TTranslation>>();
            builder.Services.AddSingleton<IStringExpressLocalizerFactory, DbStringLocalizerFactory<TResource, TTranslation>>();
            
            builder.Services.AddSingleton<IHtmlLocalizer, DbHtmlLocalizer<TResource, TTranslation>>();
            //builder.Services.TryAddTransient(typeof(IHtmlLocalizer<>), typeof(DbHtmlLocalizer<,>));
            builder.Services.AddSingleton<IHtmlLocalizerFactory, DbHtmlLocalizerFactory<TResource, TTranslation>>();
            builder.Services.AddSingleton<IHtmlExpressLocalizerFactory, DbHtmlLocalizerFactory<TResource, TTranslation>>();
            

            // Configure route culture provide
            return builder.AddDbDataAnnotationsLocalization()
                          .AddModelBindingLocalization()
                          .AddIdentityErrorsLocalization();
        }

        /// <summary>
        /// Add DataAnnotations localization with specified resource type.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddDbDataAnnotationsLocalization(this IMvcBuilder builder)
        {
            // Add ExpressValdiationAttributes to provide error messages by default without using ErrorMessage="..."
            builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider>();

            // Add data annotations locailzation
            builder.AddDataAnnotationsLocalization(ops =>
            {
                // This will look for localization resource with type of T (shared resource)
                ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create("", "");
            });

            return builder;
        }
    }
}
