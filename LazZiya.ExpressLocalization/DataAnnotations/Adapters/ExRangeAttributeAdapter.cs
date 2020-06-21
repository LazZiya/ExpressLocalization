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

        private readonly IStringLocalizer Localizer;
        private readonly bool _supportResx;
        public ExRangeAttributeAdapter(ExRangeAttribute attribute, IStringLocalizer stringLocalizer, bool supportResx) : base(attribute, stringLocalizer)
        {
            Min = attribute.Minimum;
            Max = attribute.Maximum;
            Localizer = stringLocalizer;
            _supportResx = supportResx;
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

            var msg = _supportResx
                ? GenericResourceReader.GetString(typeof(T), CultureInfo.CurrentCulture.Name,
                    DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName())
                : Localizer[DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName()].Value;

            return msg;
        }
    }
}
