using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    /// <summary>
    /// Adapter to provide localized error message for <see cref="ExCompareAttribute"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExCompareAttributeAdapter<T> : AttributeAdapterBase<ExCompareAttribute>
        where T : class
    {
        // name of the other attribute
        private string _att { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="ExCompareAttributeAdapter{T}"/>
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        public ExCompareAttributeAdapter(ExCompareAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            _att = attribute.OtherProperty;
        }

        /// <summary>
        /// Add validation context
        /// </summary>
        /// <param name="context"></param>
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-equalto", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-equalto-other", $"*.{_att}");
        }

        /// <summary>
        /// Get localized error message
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            var attLocalizedName = GenericResourceReader.GetValue<T>(CultureInfo.CurrentCulture.Name, _att);
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), attLocalizedName);
        }
    }
}
