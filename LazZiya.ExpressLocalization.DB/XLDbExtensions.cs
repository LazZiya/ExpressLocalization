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
using LazZiya.TranslationServices;
using LazZiya.EFGenericDataManager;
using System.Linq;
using LazZiya.ExpressLocalization.ResxTools;
using LazZiya.ExpressLocalization.Identity;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB extensions
    /// </summary>
    public static class XLDbExtensions
    {
        /// <summary>
        /// Add ExpressLocalization with database support using the built-in entity models
        /// </summary>
        /// <typeparam name="TContext">Application db context</typeparam>
        /// <param name="builder">builder</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext>(this IMvcBuilder builder)
            where TContext : DbContext
        {
            return builder
                .AddExpressLocalizationDB<TContext, XLResource, XLTranslation, XLCulture>();
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
                .AddExpressLocalizationDB<TContext, TResourceEntity, TTranslationEntity, TCultureEntity>();
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
            builder.Services.AddScoped<IEFGenericDataManager, EFGenericDataManager<TContext>>();
            builder.Services.AddScoped<ISharedCultureLocalizer, XLDbLocalizer<TResourceEntity,TTranslationEntity, TCultureEntity>>();
            builder.Services.AddScoped<ICulturesProvider<TCultureEntity>, XLDbLocalizer<TResourceEntity, TTranslationEntity, TCultureEntity>>();

            var xlDbOps = new XLDbOptions();
            options.Invoke(xlDbOps);
            builder.Services.Configure<XLDbOptions>(options);            

            var sp = builder.Services.BuildServiceProvider();
            var culturesService = sp.GetService<ICulturesProvider<TCultureEntity>>();
            var dbLocalizer = sp.GetService<ISharedCultureLocalizer>();

            // Configure Request Localization
            builder.Services.Configure<RequestLocalizationOptions>(ops =>
            {
                ops.SupportedCultures = culturesService.ActiveCultures.ToList();
                ops.SupportedUICultures = culturesService.ActiveCultures.ToList();
                ops.DefaultRequestCulture = new RequestCulture(culturesService.DefaultCulture ?? "en");
            });
            
            // Configure identity errors localization
            builder.Services.AddTransient<IdentityErrorDescriber, IdentityErrorsLocalizer>();
            builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<DatabaseType>>();

            // Configure model binding errors localization
            builder.AddMvcOptions(ops =>
            {
                //ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(dbLocalizer);
            });

            // Configure data annotations errors localization
            builder.AddDataAnnotationsLocalization(ops =>
            {
                ops.DataAnnotationLocalizerProvider = (t, f) => dbLocalizer;
            });

            // Configure route culture provide
            return builder.ExAddRouteValueRequestCultureProvider(culturesService.ActiveCultures.ToList(), culturesService.DefaultCulture, true); ;
        }
    }
}
