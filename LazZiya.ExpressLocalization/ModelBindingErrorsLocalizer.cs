using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace LazZiya.ExpressLocalization
{
    internal static class ModelBindingErrorsLocalizer
    {
        /// <summary>
        /// Use DB for localization
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="dbLocalizer">DB localizer service</param>
        internal static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, ISharedCultureLocalizer dbLocalizer)
        {
            SetLocalizedModelBindingErrorMessages(provider, dbLocalizer, null);
        }
        
        /// <summary>
        /// Use resx file for locaization
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="resxType"></param>
        internal static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, Type resxType)
        {
            SetLocalizedModelBindingErrorMessages(provider, null, resxType);
        }

        private static void SetLocalizedModelBindingErrorMessages(this DefaultModelBindingMessageProvider provider, ISharedCultureLocalizer dbLocalizer, Type resxType)
        {
            provider.SetAttemptedValueIsInvalidAccessor((x, y)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelState_AttemptedValueIsInvalid, x, y));

            provider.SetMissingBindRequiredValueAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelBinding_MissingBindRequiredMember, x));

            provider.SetMissingKeyOrValueAccessor(()
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.KeyValuePair_BothKeyAndValueMustBePresent));

            provider.SetMissingRequestBodyRequiredValueAccessor(()
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelBinding_MissingRequestBodyRequiredMember));

            provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelState_NonPropertyAttemptedValueIsInvalid, x));

            provider.SetNonPropertyUnknownValueIsInvalidAccessor(()
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelState_NonPropertyUnknownValueIsInvalid));

            provider.SetNonPropertyValueMustBeANumberAccessor(()
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.HtmlGeneration_NonPropertyValueMustBeNumber));

            provider.SetUnknownValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelState_UnknownValueIsInvalid, x));

            provider.SetValueIsInvalidAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.HtmlGeneration_ValueIsInvalid, x));

            provider.SetValueMustBeANumberAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.HtmlGeneration_ValueMustBeNumber, x));

            provider.SetValueMustNotBeNullAccessor((x)
                => GetLoclizedModelBindingError(dbLocalizer, resxType, ModelBindingMessages.ModelBinding_NullValueNotValid, x));
        }

        private static string GetLoclizedModelBindingError(ISharedCultureLocalizer dbLocalizer, Type resxType, string code, params object[] args)
        {
            if (dbLocalizer != null)
                return dbLocalizer[code, args];

            var msg = GenericResourceReader.GetString(resxType, string.Empty, code, args);
            return msg;
        }
    }
}
