using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace LazZiya.ExpressLocalization.ModelBinding
{
    /// <summary>
    /// ModelBindingErrors Localizer
    /// </summary>
    public static class ModelBindingErrorsExtensions
    {
        /// <summary>
        /// Use DB for localization
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="factory">localizer factory</param>
        public static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, IExpressStringLocalizerFactory factory)
        {
            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GetLoclizedModelBindingError(factory, ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }

        private static string GetLoclizedModelBindingError(IExpressStringLocalizerFactory factory, string code, params object[] args)
        {
            var localizer = factory.Create();
            return localizer[code, args].Value;
        }
    }
}
