using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    /// <summary>
    /// Adapter to provide localized error message for <see cref="ExMinLengthAttribute"/>
    /// </summary>
    public class ExMinLengthAttributeAdapter : AttributeAdapterBase<ExMinLengthAttribute>
    {
        private int MinLength { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="ExMinLengthAttributeAdapter"/>
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        public ExMinLengthAttributeAdapter(ExMinLengthAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            MinLength = attribute.Length;
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
            MergeAttribute(context.Attributes, "data-val-minlength", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-minlength-min", $"{MinLength}");
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

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), MinLength);
        }
    }
}
