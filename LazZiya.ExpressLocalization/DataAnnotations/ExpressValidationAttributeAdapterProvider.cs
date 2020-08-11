using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Registeres express valdiation attributes
    /// </summary>
    public class ExpressValidationAttributeAdapterProvider<T> : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
        where T : class
    {
        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            return ExAttributeSwitch.GetAttributeAdapter<T>(attribute, stringLocalizer) ?? base.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
