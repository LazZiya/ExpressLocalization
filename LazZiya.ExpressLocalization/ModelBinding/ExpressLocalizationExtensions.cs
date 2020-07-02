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
        /// Add ModelBinding localization to the project.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddModelBindingLocalization(this IMvcBuilder builder)
        {
            // Add ModelBinding errors localization
            builder.AddMvcOptions(ops =>
            {
                var factory = builder.Services.BuildServiceProvider().GetService(typeof(IExpressStringLocalizerFactory)) as IExpressStringLocalizerFactory;
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages(factory);
            });

            return builder;
        }
    }
}
