using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.DependencyInjection;

namespace LazZiya.ExpressLocalization.ModelBinding
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
        public static IMvcBuilder AddModelBindingLocalization<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            // Add ModelBinding errors localization
            builder.AddMvcOptions(ops =>
            {
                var factory = builder.Services.BuildServiceProvider().GetService(typeof(IStringExpressLocalizerFactory)) as IStringExpressLocalizerFactory;
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(factory);
            });

            return builder;
        }
    }
}
