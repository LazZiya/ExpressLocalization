using LazZiya.ExpressLocalization.DataAnnotations.Adapters;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
#endif

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Registeres express valdiation attributes
    /// </summary>
    internal class ExpressValidationAttributeAdapterProvider<T> : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
        where T : class
    {
        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            var type = attribute.GetType();

            if (type == typeof(ExRequiredAttribute))
                return new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
            if(type == typeof(ExMaxLengthAttribute))
                return new MaxLengthAttributeAdapter((MaxLengthAttribute)attribute, stringLocalizer);
            
            if(type == typeof(ExMinLengthAttribute))
                return new MinLengthAttributeAdapter((MinLengthAttribute)attribute, stringLocalizer);

            if (type == typeof(ExCompareAttribute))
            
            if (type == typeof(ExRangeAttribute))
                return new RangeAttributeAdapter((RangeAttribute)attribute, stringLocalizer);

            if (type == typeof(ExRegularExpressionAttribute))
                return new RegularExpressionAttributeAdapter((RegularExpressionAttribute)attribute, stringLocalizer);

#elif NETCOREAPP3_0 || NETCOREAPP3_1
            if (type == typeof(ExMaxLengthAttribute))
                return new ExMaxLengthAttributeAdapter((ExMaxLengthAttribute)attribute, stringLocalizer);

            if (type == typeof(ExMinLengthAttribute))
                return new ExMinLengthAttributeAdapter((ExMinLengthAttribute)attribute, stringLocalizer);

            if (type == typeof(ExCompareAttribute))
                return new ExCompareAttributeAdapter((ExCompareAttribute)attribute, stringLocalizer);
            
            if (type == typeof(ExRangeAttribute))
                return new ExRangeAttributeAdapter<T>((ExRangeAttribute)attribute, stringLocalizer);

            if (type == typeof(ExRegularExpressionAttribute))
                return new ExRegularExpressionAttributeAdapter<T>((ExRegularExpressionAttribute)attribute, stringLocalizer);
#endif

            return base.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
