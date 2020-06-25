using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LazZiya.ExpressLocalization.Identity
{
    /// <summary>
    /// Extesnions for adding Identity errors localization
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add DataAnnotations, ModelBinding and IdentityErrors localization to the project.
        /// </summary>
        /// <typeparam name="TResource">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddIdentityErrorsLocalization<TResource>(this IMvcBuilder builder)
            where TResource : class
        {
            // Add Identity Erros localization
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer>();

            return builder;
        }
    }
}
