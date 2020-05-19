using LazZiya.ExpressLocalization.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// ExpressLocalization DB extensions
    /// </summary>
    public static partial class ExpressLocalizationExtensions
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
                .AddExpressLocalizationDB<TContext, ExpressLocalizationEntity<int>, ExpressLocalizationCulture<int>, int>();
        }

        /// <summary>
        /// Add ExpressLocalization with DB support using customized entity models
        /// </summary>
        /// <typeparam name="TContext">DbContext</typeparam>
        /// <typeparam name="TExpressLocalizationEntity">Type of localization DbEntity</typeparam>
        /// <typeparam name="TCulturesResource">Type of culture DbEntity</typeparam>
        /// <typeparam name="TKey">Type of entity key</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationDB<TContext, TExpressLocalizationEntity, TCulturesResource, TKey>(this IMvcBuilder builder)
            where TContext : DbContext
            where TExpressLocalizationEntity : class, IExpressLocalizationEntity<TKey>
            where TCulturesResource : class, IExpressLocalizationCulture<TKey>
            where TKey : IEquatable<TKey>
        {
            builder.Services.AddTransient<ISharedCultureLocalizer, ExpressLocalizationDbProvider<TContext, TExpressLocalizationEntity, TCulturesResource, TKey>>();
            builder.Services.AddTransient<ICulturesProvider<TCulturesResource, TKey>, ExpressLocalizationDbProvider<TContext, TExpressLocalizationEntity, TCulturesResource, TKey>>();

            var sp = builder.Services.BuildServiceProvider();
            var culturesService = sp.GetService<ICulturesProvider<TCulturesResource, TKey>>();
            var dbLocalizer = sp.GetService<ISharedCultureLocalizer>();

            // Configure Request Localization
            builder.Services.Configure<RequestLocalizationOptions>(ops =>
            {
                ops.SupportedCultures = culturesService.ActiveCultures;
                ops.SupportedUICultures = culturesService.ActiveCultures;
                ops.DefaultRequestCulture = new RequestCulture(culturesService.DefaultCulture);
            });

            // Configure model binding errors localization
            builder.AddMvcOptions(ops =>
            {
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(dbLocalizer);
            });

            // Configure identity errors localization
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer>();

            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<DbType>>();

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
