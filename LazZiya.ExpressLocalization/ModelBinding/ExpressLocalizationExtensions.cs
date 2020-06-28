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
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddModelBindingLocalization(this IMvcBuilder builder)
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
