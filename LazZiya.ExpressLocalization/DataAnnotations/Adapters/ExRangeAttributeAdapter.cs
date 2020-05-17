using LazZiya.ExpressLocalization.DB;
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
        public ExRangeAttributeAdapter(ExRangeAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            Min = attribute.Minimum;
            Max = attribute.Maximum;
            Localizer = stringLocalizer;
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

            var msg = typeof(T) == typeof(DbType)
                ? Localizer[DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName()]
                : GenericResourceReader.GetString(typeof(T), CultureInfo.CurrentCulture.Name, 
                    DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName());
            
            return msg;
        }
    }
}
