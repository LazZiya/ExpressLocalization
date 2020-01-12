using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    internal class ExRangeAttributeAdapter<T> : AttributeAdapterBase<ExRangeAttribute>
        where T : class
    {
        private object Min { get; set; }
        private object Max { get; set; }
        public ExRangeAttributeAdapter(ExRangeAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            Min = attribute.Minimum;
            Max = attribute.Maximum;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-range", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-range-max", $"{Max}");
            MergeAttribute(context.Attributes, "data-val-range-min", $"{Min}");
            MergeAttribute(context.Attributes, "data-val-required", GetRequiredErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), Min, Max);
        }

        private string GetRequiredErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            var msg = GenericResourceReader.GetValue<T>(CultureInfo.CurrentCulture.Name, DataAnnotationsErrorMessages.RequiredAttribute_ValidationError);
            return string.Format(msg, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
