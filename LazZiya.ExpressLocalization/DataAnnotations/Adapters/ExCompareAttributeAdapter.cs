using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DataAnnotations.Adapters
{
    internal class ExCompareAttributeAdapter<T> : AttributeAdapterBase<ExCompareAttribute>
        where T : class
    {
        // name of the other attribute
        private string _att { get; set; }
        private readonly IStringLocalizer Localizer;
        private readonly bool _supportResx;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        /// <param name="supportResx">Support for old behaviour. This is temporary and to be removed in a feature release.</param>
        public ExCompareAttributeAdapter(ExCompareAttribute attribute, IStringLocalizer stringLocalizer, bool supportResx) : base(attribute, stringLocalizer)
        {
            _att = attribute.OtherProperty;
            Localizer = stringLocalizer;
            _supportResx = supportResx;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new NullReferenceException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-equalto", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-equalto-other", $"*.{_att}");
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new NullReferenceException(nameof(validationContext));

            var _locAtt = _supportResx
                 ? GenericResourceReader.GetString(typeof(T), CultureInfo.CurrentCulture.Name, _att)
                 : Localizer[_att].Value;

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), _locAtt);
        }
    }
}
