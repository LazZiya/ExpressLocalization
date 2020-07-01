using LazZiya.ExpressLocalization.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Identity;
using LazZiya.ExpressLocalization.ModelBinding;
using Microsoft.Extensions.Localization;
using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using LazZiya.TranslationServices;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.ExpressLocalization.Cache;

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
            var ops = new ExpressLocalizationOptions();

            // Register dummy translatio service to avoid startup exceptions
            builder.Services.AddTransient<ITranslationService, DummyTranslationService>();

            return builder
                .AddExpressLocalizationDb<TContext, DummyTranslationService, XLResource, XLTranslation, XLCulture>(o => o = ops);
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
            // Register dummy translatio service to avoid startup exceptions
            builder.Services.AddTransient<ITranslationService, DummyTranslationService>();

            return builder
                .AddExpressLocalizationDb<TContext, DummyTranslationService, XLResource, XLTranslation, XLCulture>(options);
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
            var ops = new ExpressLocalizationOptions();

            // Register dummy translatio service to avoid startup exceptions
            builder.Services.AddTransient<ITranslationService, DummyTranslationService>();

            return builder
                .AddExpressLocalizationDb<TContext, DummyTranslationService, TResource, TTranslation, TCulture>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization with database support using the built-in entity models,
        /// and use defined translation service type
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <param name="builder">builder</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TService>(this IMvcBuilder builder)
            where TContext : DbContext
            where TService : ITranslationService
        {
            var ops = new ExpressLocalizationOptions();

            return builder
                .AddExpressLocalizationDb<TContext, TService, XLResource, XLTranslation, XLCulture>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization with database support using the built-in entity models,
        /// and use defined translation service type
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <param name="builder">builder</param>
        /// <param name="options">builder</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TService>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TContext : DbContext
            where TService : ITranslationService
        {
            return builder
                .AddExpressLocalizationDb<TContext, TService, XLResource, XLTranslation, XLCulture>(options);
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models,
        /// and use defined translation service type
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <typeparam name="TResource">Type of resource DbEntity</typeparam>
        /// <typeparam name="TTranslation">Type of translation entityy</typeparam>
        /// <typeparam name="TCulture">Type of culture DbEntity</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TService, TResource, TTranslation, TCulture>(this IMvcBuilder builder)
            where TContext : DbContext
            where TService : ITranslationService
            where TResource : class, IXLDbResource
            where TTranslation : class, IXLDbTranslation
            where TCulture : class, IXLCulture
        {
            var ops = new ExpressLocalizationOptions();

            return builder
                .AddExpressLocalizationDb<TContext, TService, TResource, TTranslation, TCulture>(o => o = ops);
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models,
        /// and use defined translation service type
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TService">Translation service</typeparam>
        /// <typeparam name="TResource">Type of localization DbEntity</typeparam>
        /// <typeparam name="TTranslation">Type of translation entity</typeparam>
        /// <typeparam name="TCulture">Type of culture DbEntity</typeparam>
        /// <param name="options">XLDbOptions</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDb<TContext, TService, TResource, TTranslation, TCulture>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TContext : DbContext
            where TService : ITranslationService
            where TResource : class, IXLDbResource
            where TTranslation : class, IXLDbTranslation
            where TCulture : class, IXLCulture
        {
            builder.Services.Configure<ExpressLocalizationOptions>(options);

            // ExpressMemoryCache for caching localized values
            builder.Services.AddSingleton<ExpressMemoryCache>();

            // Data manager to perform CRUD operations on the resource and translation entities
            builder.Services.AddTransient<IEFGenericDataManager, EFGenericDataManager<TContext>>();

            // Register IStringLocalizer for the default shared resource and translation type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IStringLocalizer, DbStringLocalizer<TResource, TTranslation>>();
            builder.Services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory<TResource, TTranslation>>();

            // Register IHtmlLocalizer for the default shared resource and translation type
            // This is the default (shared) resource entity and translation
            builder.Services.AddSingleton<IHtmlLocalizer, DbHtmlLocalizer<TResource, TTranslation>>();
            builder.Services.AddSingleton<IHtmlLocalizerFactory, DbHtmlLocalizerFactory<TResource, TTranslation>>();
            
            // Register generic IDbStringLocalizer for user defined resource and translation entities
            // e.g. IDbStringLocalizer<ProductArea, ProductAreaTranslation>
            // e.g. IDbStringLocalizer<UserArea, UserAreaTranslation>
            builder.Services.AddTransient(typeof(IDbStringLocalizer<,>), typeof(DbStringLocalizer<,>));
            builder.Services.AddTransient(typeof(IDbHtmlLocalizer<,>), typeof(DbHtmlLocalizer<,>));

            // Express localizer factories for creating localizers with the default shared resource type
            // Use .Create() method for creating localizers.
            builder.Services.AddSingleton<IExpressStringLocalizerFactory, DbStringLocalizerFactory<TResource, TTranslation>>();
            builder.Services.AddSingleton<IExpressHtmlLocalizerFactory, DbHtmlLocalizerFactory<TResource, TTranslation>>();
            

            // Configure route culture provide
            return builder.AddDbDataAnnotationsLocalization()
                          .AddModelBindingLocalization()
                          .AddIdentityErrorsLocalization()
                          .WithTranslationService<TService>();
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
                // This will look for localization resource of default type T (shared resource)
                ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create("", "");
            });

            return builder;
        }
    }
}
