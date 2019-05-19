using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace LazZiya.ExpressLocalization
{
    internal static class ModelBindingErrorsLocalizer
    {
        internal static void SetLocalizedModelBindingErrorMessages<T>(this DefaultModelBindingMessageProvider provider) where T : class
        {
            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GenericResourceReader.GetValue<T>(ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }
    }
}
