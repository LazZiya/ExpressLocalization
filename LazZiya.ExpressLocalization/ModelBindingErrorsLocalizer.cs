using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Globalization;

namespace LazZiya.ExpressLocalization
{
    internal static class ModelBindingErrorsLocalizer
    {
        internal static void SetLocalizedModelBindingErrorMessages<T>(this DefaultModelBindingMessageProvider provider) where T : class
        {
            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GetLoclizedModelBindingError<T>(ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }

        private static string GetLoclizedModelBindingError<T>(string code, params object[] args) where T : class
        {
            var msg = GenericResourceReader.GetValue<T>(string.Empty, code, args);

            return string.Format(msg, args);
        }
    }
}
