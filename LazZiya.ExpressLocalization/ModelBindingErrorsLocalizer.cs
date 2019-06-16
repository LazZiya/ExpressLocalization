using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Globalization;

namespace LazZiya.ExpressLocalization
{
    internal static class ModelBindingErrorsLocalizer
    {
        internal static void SetLocalizedModelBindingErrorMessages<T>(this DefaultModelBindingMessageProvider provider) where T : class
        {
            var culture = CultureInfo.CurrentCulture.Name;

            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GenericResourceReader.GetValue<T>(culture, ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }
    }
}
