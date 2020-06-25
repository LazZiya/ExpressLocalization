using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Extesnions for adding DataAnnotation Localization
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add DataAnnotations, ModelBinding and IdentityErrors localization to the project.
        /// </summary>
        /// <typeparam name="TResource">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddDataAnnotationsLocalization<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            // Add ExpressValdiationAttributes to provide error messages by default without using ErrorMessage="..."
            // After removing support for old behaviour use below implementation
            // builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<T>>();

            // This is a temporary solution to provide support for the old behaviour with resx files
            builder.Services.AddTransient<IValidationAttributeAdapterProvider, ExpressValidationAttributeAdapterProvider<TResource>>((x) => new ExpressValidationAttributeAdapterProvider<TResource>(false));

            // Add data annotations locailzation
            builder.AddDataAnnotationsLocalization(ops =>
            {
                // This will look for localization resource with type of T (shared resource)
                ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(TResource));

                // This will look for localization resources depending on specific type, e.g. LoginModel.en.xml
                //ops.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(t);
            });

            return builder;
        }
    }
}
