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
                .AddExpressLocalizationDB<TContext, XLResource, XLCulture>();
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TLocalizationEntity">Type of localization DbEntity</typeparam>
        /// <typeparam name="TCultureEntity">Type of culture DbEntity</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext, TLocalizationEntity, TCultureEntity>(this IMvcBuilder builder)
            where TContext : DbContext
            where TLocalizationEntity : class, IXLResource
            where TCultureEntity : class, IXLCulture
        {
            builder.Services.AddTransient<ISharedCultureLocalizer, XLDbLocalizer<TContext, TLocalizationEntity, TCultureEntity>>();
            builder.Services.AddTransient<ICulturesProvider<TCultureEntity>, XLDbLocalizer<TContext, TLocalizationEntity, TCultureEntity>>();

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
