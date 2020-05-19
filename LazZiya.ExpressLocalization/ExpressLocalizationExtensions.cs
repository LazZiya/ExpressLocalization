﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using LazZiya.ExpressLocalization.DataAnnotations;

#if NETCOREAPP3_0 || NETCOREAPP3_1
#else
using Microsoft.AspNetCore.Routing;
#endif

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Add localization support for dot net core web apps with one simple step.
    /// <para>download necessary resource files from: https://github.com/LazZiya/ExpressLocalization.Resources</para>
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        private static IList<IRequestCultureProvider> _providers;
        private static IList<CultureInfo> _cultures;

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TLocalizationResource">Type of localization resource, this resource should contain all localization messages for Identity, ModelBinding, DataAnnotations localized validiation messages and view localizations as well.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            if (_options.ConfigureRedirectPaths)
                builder.ExConfigureApplicationCookie(_options.LoginPath, _options.LogoutPath, _options.AccessDeniedPath, _ops.DefaultRequestCulture.Culture.Name);

            return builder
                .AddViewLocalization(ops => { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TLocalizationResource>(_options.UseExpressValidationAttributes)
                .ExAddModelBindingLocalization<TLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name, _options.UseAllCultureProviders);
                //.ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TExpressLocalizationResource">Type of ExpressLocalizationResource, this resource should contain Identity, ModelBinding and DataAnnotations localized validiation messages.</typeparam>
        /// <typeparam name="TViewLocalizationResource">Type of ViewLocalizationResource, this resource is a shared view localization resource.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TExpressLocalizationResource, TViewLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TExpressLocalizationResource : class
            where TViewLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            if (_options.ConfigureRedirectPaths)
                builder.ExConfigureApplicationCookie(_options.LoginPath, _options.LogoutPath, _options.AccessDeniedPath, _ops.DefaultRequestCulture.Culture.Name);

            return builder
                .AddViewLocalization(ops => { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TViewLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TExpressLocalizationResource>(_options.UseExpressValidationAttributes)
                .ExAddModelBindingLocalization<TExpressLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TExpressLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name, _options.UseAllCultureProviders);
                //.ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TIdentityErrorsLocalizationResource">Type of localization resource for Identity error messages.</typeparam>
        /// <typeparam name="TModelBindingLocalizationResource">Type of localization resource for model binding error messages.</typeparam>
        /// <typeparam name="TDataAnnotationsLocalizationResource">Type of localization resource for data annotations.</typeparam>
        /// <typeparam name="TViewLocalizationResource">Type of localization shared resource for razor views.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TViewLocalizationResource, TDataAnnotationsLocalizationResource, TModelBindingLocalizationResource, TIdentityErrorsLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TViewLocalizationResource : class
            where TDataAnnotationsLocalizationResource : class
            where TModelBindingLocalizationResource : class
            where TIdentityErrorsLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            if (_options.ConfigureRedirectPaths)
                builder.ExConfigureApplicationCookie(_options.LoginPath, _options.LogoutPath, _options.AccessDeniedPath, _ops.DefaultRequestCulture.Culture.Name);

            return builder
                .AddViewLocalization(ops => { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TViewLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TDataAnnotationsLocalizationResource>(_options.UseExpressValidationAttributes)
                .ExAddModelBindingLocalization<TModelBindingLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TIdentityErrorsLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name, _options.UseAllCultureProviders);
                //.ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add DataAnnotatons localization to the project.
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TDataAnnotationsLocalizationResource">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <param name="useExpressValidationAttributes">Express validiation attributes provides already localized eror messages</param>
        /// <returns></returns>
        public static IMvcBuilder ExAddDataAnnotationsLocalization<TDataAnnotationsLocalizationResource>(this IMvcBuilder builder, bool useExpressValidationAttributes) where TDataAnnotationsLocalizationResource : class
        {
            builder.AddDataAnnotationsLocalization(ops =>
            {
                var t = typeof(TDataAnnotationsLocalizationResource);
                ops.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(t.Name, t.Assembly.GetName().Name);
            });

            if (useExpressValidationAttributes)
                builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<TDataAnnotationsLocalizationResource>>();

            return builder;
        }

        /// <summary>
        /// Add ModelBinding localization to the project.
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TModelBindingLocalizationResource">Type of ModelBinding localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddModelBindingLocalization<TModelBindingLocalizationResource>(this IMvcBuilder builder) where TModelBindingLocalizationResource : class
        {
            builder.AddMvcOptions(ops =>
            {
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages<TModelBindingLocalizationResource>();
            });

            return builder;
        }

        /// <summary>
        /// Add shared locaization settings for views
        /// <para>Sample resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TViewLocalizationResource">Type of shared localization resource for razor pages</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddSharedCultureLocalizer<TViewLocalizationResource>(this IMvcBuilder builder) where TViewLocalizationResource : class
        {
            builder.Services.AddSingleton<ISharedCultureLocalizer, SharedCultureLocalizer>((x) => new SharedCultureLocalizer(x.GetRequiredService<IHtmlLocalizerFactory>(), typeof(TViewLocalizationResource)));

            return builder;
        }

        /// <summary>
        /// Add IdentityErrors localization to the project
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TIdentityErrorsLocalizationResource">Type of IdentityErro messages localizaton resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddIdentityErrorMessagesLocalization<TIdentityErrorsLocalizationResource>(this IMvcBuilder builder) where TIdentityErrorsLocalizationResource : class
        {
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer<TIdentityErrorsLocalizationResource>>(ops =>
                new IdentityErrorsLocalizer<TIdentityErrorsLocalizationResource>());

            return builder;
        }

        /// <summary>
        /// Add client side libraries for valdiating localized inputs like decimal numbers
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        [Obsolete("This method is not in use. Register client side scripts manually")]
        public static IMvcBuilder ExAddClientSideLocalizationValidationScripts(this IMvcBuilder builder)
        {
            // builder.Services.AddTransient<ITagHelperComponent, LocValScrTagHelperComponent>();

            return builder;
        }

        /// <summary>
        /// Add RouteValueRequestCultureProvider, so localization will be based on route value {culture}
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cultures">List of supported cultures</param>
        /// <param name="defaultCulture">default culture name</param>
        /// <param name="useAllProviders">true to register all culture providers (Route, QueryString, Cookie, AcceptedLanguageHeader), false to use only Route culture provider.</param>
        /// <returns></returns>
        public static IMvcBuilder ExAddRouteValueRequestCultureProvider(this IMvcBuilder builder, IList<CultureInfo> cultures, string defaultCulture, bool useAllProviders)
        {
            builder.Services.Configure<RequestLocalizationOptions>(ops =>
            {
                if (useAllProviders)
                    ops.RequestCultureProviders.Insert(0, new RouteSegmentCultureProvider(cultures, defaultCulture));
                else
                {
                    ops.RequestCultureProviders.Clear();
                    ops.RequestCultureProviders.Add(new RouteSegmentCultureProvider(cultures, defaultCulture));
                }

                _providers = ops.RequestCultureProviders;
                _cultures = cultures;
            });

            builder.AddRazorPagesOptions(x =>
            {
                x.Conventions?.Add(new RouteTemplateModelConvention());
            });

            return builder;
        }

        /// <summary>
        /// Configure application cookie and add culture value to redirect path
        /// so unauthorized users will be redirected to login page and the culture will not be lost
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loginPath">Login path</param>
        /// <param name="logoutPath">Logout path</param>
        /// <param name="accessDeniedPath">Access denied path</param>
        /// <param name="defCulture">default culture name to add to the path when redirect to login </param>
        /// <returns></returns>
        public static IMvcBuilder ExConfigureApplicationCookie(this IMvcBuilder builder, string loginPath, string logoutPath, string accessDeniedPath, string defCulture)
        {
            // add culture value to route when user is redirected to login page
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Improvment : do we need to check for existing cookie authentication events before?
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
#if NETCOREAPP3_0 || NETCOREAPP3_1
                        var culture = ctx.Request.RouteValues["culture"];
#else
                        var culture = ctx.HttpContext.GetRouteValue("culture");
#endif
                        var requestPath = ctx.Request.Path;

                        var detectedCulture = ProviderCultureDetector.DetectCurrentCulture(_providers, ctx.HttpContext, _cultures, defCulture).Result;

                        if (culture == null)
                        {
                            culture = detectedCulture.ToString();
                            requestPath = $"/{culture}{requestPath}";
                        }

                        ctx.Response.Redirect($"/{culture}{loginPath}?ReturnUrl={requestPath}{ctx.Request.QueryString}");
                        return Task.CompletedTask;
                    },
                    OnRedirectToLogout = ctx =>
                    {
#if NETCOREAPP3_0 || NETCOREAPP3_1
                        var culture = ctx.Request.RouteValues["culture"] ?? defCulture;
#else
                        var culture = ctx.HttpContext.GetRouteValue("culture") ?? defCulture;
#endif
                        var detectedCulture = ProviderCultureDetector.DetectCurrentCulture(_providers, ctx.HttpContext, _cultures, defCulture).Result;

                        if (culture == null)
                        {
                            culture = detectedCulture.ToString();
                            logoutPath = $"/{culture}{logoutPath}";
                        }

                        ctx.Response.Redirect($"/{culture}{logoutPath}");
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
#if NETCOREAPP3_0 || NETCOREAPP3_1
                        var culture = ctx.Request.RouteValues["culture"] ?? defCulture;
#else
                        var culture = ctx.HttpContext.GetRouteValue("culture") ?? defCulture;
#endif
                        var detectedCulture = ProviderCultureDetector.DetectCurrentCulture(_providers, ctx.HttpContext, _cultures, defCulture).Result;

                        if (culture == null)
                        {
                            culture = detectedCulture.ToString();
                            accessDeniedPath = $"/{culture}{accessDeniedPath}";
                        }

                        ctx.Response.Redirect($"/{culture}{accessDeniedPath}");
                        return Task.CompletedTask;
                    }
                };
            });
            return builder;
        }
    }
}
