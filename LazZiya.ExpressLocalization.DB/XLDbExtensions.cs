using LazZiya.ExpressLocalization.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.Extensions.Options;
using LazZiya.ExpressLocalization.DB.TranslationTools;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB extensions
    /// </summary>
    public static class XLDbExtensions
    {
        /// <summary>
        /// Add ExpressLocalization support using the built-in entity models
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <param name="builder">builder</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext>(this IMvcBuilder builder)
            where TContext : DbContext
        {
            return builder
                .AddExpressLocalizationDB<TContext, XLResource, XLTranslation, XLCulture>(x => x.RecursiveMode = RecursiveMode.Full);
        }

        /// <summary>
        /// Add ExpressLocalization support using the built-in entity models
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <param name="builder">builder</param>
        /// <param name="options">XLDbOptions</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext>(this IMvcBuilder builder, Action<XLDbOptions> options)
            where TContext : DbContext
        {
            return builder
                .AddExpressLocalizationDB<TContext, XLResource, XLTranslation, XLCulture>(options);
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TResourceEntity">Type of resource DbEntity</typeparam>
        /// <typeparam name="TTranslationEntity">Type of translation entityy</typeparam>
        /// <typeparam name="TCultureEntity">Type of culture DbEntity</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext, TResourceEntity, TTranslationEntity, TCultureEntity>(this IMvcBuilder builder)
            where TContext : DbContext
            where TResourceEntity : class, IXLResource
            where TTranslationEntity : class, IXLTranslation
            where TCultureEntity : class, IXLCulture
        {
            return builder
                .AddExpressLocalizationDB<TContext, TResourceEntity, TTranslationEntity, TCultureEntity>(x => x.RecursiveMode = RecursiveMode.None);
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TResourceEntity">Type of localization DbEntity</typeparam>
        /// <typeparam name="TTranslationEntity">Type of translation entity</typeparam>
        /// <typeparam name="TCultureEntity">Type of culture DbEntity</typeparam>
        /// <param name="options">XLDbOptions</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext, TResourceEntity, TTranslationEntity, TCultureEntity>(this IMvcBuilder builder, Action<XLDbOptions> options)
            where TContext : DbContext
            where TResourceEntity : class, IXLResource
            where TTranslationEntity : class, IXLTranslation
            where TCultureEntity : class, IXLCulture
        {
            var xlDbOps = new XLDbOptions();
            options.Invoke(xlDbOps);

            builder.Services.AddTransient<ISharedCultureLocalizer, XLDbLocalizer<TContext, TResourceEntity,TTranslationEntity, TCultureEntity>>();
            builder.Services.AddTransient<ICulturesProvider<TCultureEntity>, XLDbLocalizer<TContext, TResourceEntity, TTranslationEntity, TCultureEntity>>();
            builder.Services.Configure<XLDbOptions>(options);
            
            if(xlDbOps.RecursiveMode == RecursiveMode.Full)
                builder.Services.AddTransient<IXLTranslateApiClient, RapidApiClient>();

            var sp = builder.Services.BuildServiceProvider();
            var culturesService = sp.GetService<ICulturesProvider<TCultureEntity>>();
            var dbLocalizer = sp.GetService<ISharedCultureLocalizer>();

            // Configure Request Localization
            builder.Services.Configure<RequestLocalizationOptions>(ops =>
            {
                ops.SupportedCultures = culturesService.ActiveCultures;
                ops.SupportedUICultures = culturesService.ActiveCultures;
                ops.DefaultRequestCulture = new RequestCulture(culturesService.DefaultCulture ?? "en");
            });

            // Configure model binding errors localization
            builder.AddMvcOptions(ops =>
            {
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(dbLocalizer);
            });

            // Configure identity errors localization
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer>();

            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<DatabaseType>>();

            // Configure data annotations errors localization
            builder.AddDataAnnotationsLocalization(ops =>
            {
                ops.DataAnnotationLocalizerProvider = (t, f) => dbLocalizer;
            });

            // Configure route culture provide
            return builder.ExAddRouteValueRequestCultureProvider(culturesService.ActiveCultures, culturesService.DefaultCulture, true); ;
        }
    }
}
