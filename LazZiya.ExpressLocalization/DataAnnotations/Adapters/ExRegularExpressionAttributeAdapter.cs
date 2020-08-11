using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    /// <summary>
    /// Adapter to provide localized error message for <see cref="ExRegularExpressionAttribute"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExRegularExpressionAttributeAdapter<T> : AttributeAdapterBase<ExRegularExpressionAttribute>
        where T : class
    {
        private readonly string RegexPattern;

        /// <summary>
        /// Initialize a new instance of <see cref="ExRegularExpressionAttributeAdapter{T}"/>
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        public ExRegularExpressionAttributeAdapter(ExRegularExpressionAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            RegexPattern = attribute.Pattern;
        }

        /// <summary>
        /// Add valdiation context
        /// </summary>
        /// <param name="context"></param>
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-regex", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-regex-pattern", RegexPattern);
            MergeAttribute(context.Attributes, "data-val-required", GetRequiredErrorMessage(context));
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

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), RegexPattern);
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
