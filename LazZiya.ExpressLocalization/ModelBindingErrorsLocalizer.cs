using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// ModelBindingErrors Localizer
    /// </summary>
    public static class ModelBindingErrorsLocalizer
    {
        /// <summary>
        /// Use DB for localization
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="factory">localizer factory</param>
        public static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, IStringExpressLocalizerFactory factory)
        {
            SetLocalizedModelBindingErrorMessages(provider, factory, null);
        }
        
        /// <summary>
        /// Use resx file for locaization
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="resxType"></param>
        public static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, Type resxType)
        {
            SetLocalizedModelBindingErrorMessages(provider, null, resxType);
        }

        private static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, IStringExpressLocalizerFactory factory, Type resxType)
        {
            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GetLoclizedModelBindingError(factory, resxType, ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }

        private static string GetLoclizedModelBindingError(IStringExpressLocalizerFactory factory, Type resxType, string code, params object[] args)
        {
            if (factory != null)
            {
                var localizer = factory.Create();
                return localizer[code, args].Value;
            }

            var msg = GenericResourceReader.GetString(resxType, string.Empty, code, args);
            return msg;
        }
    }
}
