using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    /// <summary>
    /// Adapter to provide localized error message for <see cref="ExStringLengthAttribute"/>
    /// </summary>
    public class ExStringLengthAttributeAdapter : AttributeAdapterBase<ExStringLengthAttribute>
  {
        private readonly int MaxLenght;
        private readonly int MinLenght;

        /// <summary>
        /// Initialize a new instance of <see cref="ExStringLengthAttributeAdapter"/>
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        public ExStringLengthAttributeAdapter(ExStringLengthAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            MaxLenght = attribute.MaximumLength;
            MinLenght = attribute.MinimumLength;
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
            MergeAttribute(context.Attributes, "data-val-length", GetErrorMessage(context));
            if (MaxLenght != int.MaxValue)
            {
              MergeAttribute(context.Attributes, "data-val-length-max", $"{MaxLenght}");
            }
            if (MinLenght > 0)
            {
              MergeAttribute(context.Attributes, "data-val-length-min", $"{MinLenght}");
            }
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

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), MaxLenght, MinLenght);
        }
    }
}
