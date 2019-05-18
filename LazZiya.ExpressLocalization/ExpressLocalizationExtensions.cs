using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace LazZiya.ExpressLocalization
{
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// </summary>
        /// <typeparam name="T1">Type of ViewLocalizationResource</typeparam>
        /// <typeparam name="T2">Type of DataAnnotationsLocalizationResource</typeparam>
        /// <typeparam name="T3">Type of ModelBindingLocalizationResource</typeparam>
        /// <typeparam name="T4">Type of IdentityErrorsLocalizationResource</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">RequestLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<T1,T2,T3,T4>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where T1 : class 
            where T2 : class
            where T3 : class
            where T4 : class 
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            return builder
                .AddViewLocalization()
                .ExAddSharedCultureLocalizer<T1>()
                .ExAddDataAnnotationsLocalization<T2>()
                .ExAddModelBindingLocalization<T3>()
                .ExAddIdentityErrorMessagesLocalization<T4>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name);
        }

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// </summary>
        /// <typeparam name="T1">Type of ViewLocalizationResource</typeparam>
        /// <typeparam name="T2">Type of DataAnnotationsLocalizationResource</typeparam>
        /// <typeparam name="T3">Type of ModelBindingLocalizationResource</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">RequestLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<T1, T2, T3>(this IMvcBuilder builder, Action<RequestLocalizationOptions> options)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var _options = new RequestLocalizationOptions();
            options.Invoke(_options);

            builder.Services.Configure<RequestLocalizationOptions>(options);

            return builder
                .AddViewLocalization()
                .ExAddSharedCultureLocalizer<T1>()
                .ExAddDataAnnotationsLocalization<T2>()
                .ExAddModelBindingLocalization<T3>()
                .ExAddRouteValueRequestCultureProvider(_options.SupportedCultures, _options.DefaultRequestCulture.Culture.Name);
        }

        public static IMvcBuilder ExAddDataAnnotationsLocalization<T>(this IMvcBuilder builder) where T : class
        {
            builder.AddDataAnnotationsLocalization(x =>
            {
                var type = typeof(T);
                var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var localizer = factory.Create(type.Name, assemblyName.Name);

                x.DataAnnotationLocalizerProvider = (t, f) => localizer;
            });

            return builder;
        }

        public static IMvcBuilder ExAddModelBindingLocalization<T>(this IMvcBuilder builder) where T : class
        {
            builder.AddMvcOptions(ops =>
            {
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages<T>();
            });

            return builder;
        }

        public static IMvcBuilder ExAddSharedCultureLocalizer<T>(this IMvcBuilder builder) where T : class
        {
            builder.Services.AddSingleton<SharedCultureLocalizer>((x) => new SharedCultureLocalizer(x.GetRequiredService<IHtmlLocalizerFactory>(), typeof(T)));

            return builder;
        }

        public static IMvcBuilder ExAddIdentityErrorMessagesLocalization<T>(this IMvcBuilder builder) where T : class
        {
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer<T>>(ops =>
                new IdentityErrorsLocalizer<T>());

            return builder;
        }

        public static IMvcBuilder ExAddRouteValueRequestCultureProvider(this IMvcBuilder builder, IList<CultureInfo> cultures, string defaultCulture)
        {
            builder.Services.Configure<RequestLocalizationOptions>(ops=> {
                ops.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(cultures, defaultCulture));
            });

            builder.AddRazorPagesOptions(x =>
            {
                x.Conventions.Add(new RouteTemplateModelConvention());
            });

            return builder;
        }
    }
}
