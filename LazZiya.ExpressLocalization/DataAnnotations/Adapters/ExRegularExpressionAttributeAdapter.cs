using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    internal class ExRegularExpressionAttributeAdapter<T> : AttributeAdapterBase<ExRegularExpressionAttribute>
        where T : class
    {
        private readonly string RegexPattern;
        private readonly IStringLocalizer Localizer;
        private readonly bool _spoortResx;
        public ExRegularExpressionAttributeAdapter(ExRegularExpressionAttribute attribute, IStringLocalizer stringLocalizer, bool supportResx) : base(attribute, stringLocalizer)
        {
            RegexPattern = attribute.Pattern;
            Localizer = stringLocalizer;
            _spoortResx = supportResx;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-regex", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-regex-pattern", RegexPattern);
            MergeAttribute(context.Attributes, "data-val-required", GetRequiredErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), RegexPattern);
        }

        private string GetRequiredErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            var msg = _spoortResx
                ? GenericResourceReader.GetString(typeof(T), CultureInfo.CurrentCulture.Name,
                    DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName())
                : Localizer[DataAnnotationsErrorMessages.RequiredAttribute_ValidationError, validationContext.ModelMetadata.GetDisplayName()].Value;

            return msg;
        }
    }
}
