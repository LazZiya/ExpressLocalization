using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Reflection;

namespace LazZiya.ExpressLocalization
{
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Configure express localization options
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalizationOptions(this IMvcBuilder builder, Action<ExpressLocalizationOptions> optionsAction)
        {
            var options = new ExpressLocalizationOptions();
            optionsAction.Invoke(options);
            
            builder.Services.Configure<RequestLocalizationOptions>(options.RequestLocalizationOptions);
            
            var rt = new LocalizationResourcesTypes();
            options.LocalizationResourcesTypes.Invoke(rt);

            var modelBindingResourceType = rt.ModelBinding;
            var mvcOps = new MvcOptions();
            options.MvcOptions.Invoke(mvcOps);
            mvcOps.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(modelBindingResourceType);

            builder
                //global route template /{culture}/xxxsx
                .AddRazorPagesOptions(ops => ops.Conventions.Add(new GlobalTemplatePageRouteModelConvention()))

                //resources folder path
                .AddViewLocalization(options.LocalizationOptions)

                //Model binding messages localization
                .AddMvcOptions(ops => ops = mvcOps)

                //data annotations
                .AddDataAnnotationsLocalization(o =>
                {
                    var type = rt.DataAnnotations;
                    var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                    var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create(type.Name, assemblyName.Name);
                    o.DataAnnotationLocalizerProvider = (t, f) => localizer;
                });

            builder.Services.AddSingleton<CultureLocalizer>((x) => new CultureLocalizer(x.GetRequiredService<IHtmlLocalizerFactory>(), rt.Views));

            return builder;
        }

        private static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, Type type)
        {
            var msg = string.Empty;

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.AttemptedValueIsInvalidAccessor), type);
            provider.SetAttemptedValueIsInvalidAccessor((x, y) => string.Format(msg, x, y));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.MissingBindRequiredValueAccessor), type);
            provider.SetMissingBindRequiredValueAccessor((x) => string.Format(msg, x));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.MissingKeyOrValueAccessor), type);
            provider.SetMissingKeyOrValueAccessor(() => msg);

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.MissingRequestBodyRequiredValueAccessor), type);
            provider.SetMissingRequestBodyRequiredValueAccessor(() => msg);

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.NonPropertyAttemptedValueIsInvalidAccessor), type);
            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => string.Format(msg, x));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.NonPropertyUnknownValueIsInvalidAccessor), type);
            provider.SetNonPropertyUnknownValueIsInvalidAccessor(() => msg);

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.NonPropertyValueMustBeANumberAccessor), type);
            provider.SetNonPropertyValueMustBeANumberAccessor(() => msg);

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.UnknownValueIsInvalidAccessor), type);
            provider.SetUnknownValueIsInvalidAccessor((x) => string.Format(msg, x));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.ValueIsInvalidAccessor), type);
            provider.SetValueIsInvalidAccessor((x) => string.Format(msg, x));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.ValueMustBeANumberAccessor), type);
            provider.SetValueMustBeANumberAccessor((x) => string.Format(msg, x));

            msg = GenericPropertyReader.GetPropertyValue(nameof(provider.ValueMustNotBeNullAccessor), type);
            provider.SetValueMustNotBeNullAccessor((x) => string.Format(msg, x));
        }
    }
}
