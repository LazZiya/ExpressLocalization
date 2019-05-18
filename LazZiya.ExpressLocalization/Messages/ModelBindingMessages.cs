namespace LazZiya.ExpressLocalization.Messages
{
    /// <summary>
    /// These string values must be presnet in the supplied resource file.
    /// <para>
    /// Original messages obtained from: <see cref="https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.Core/src/Resources.resx"/>
    /// </para>
    /// </summary>
    internal struct ModelBindingMessages
    {
        internal const string ModelState_AttemptedValueIsInvalid = "The value '{0}' is not valid for {1}.";

        internal const string ModelBinding_MissingBindRequiredMember = "A value for the '{0}' parameter or property was not provided.";

        internal const string KeyValuePair_BothKeyAndValueMustBePresent = "A value is required.";

        internal const string ModelBinding_MissingRequestBodyRequiredMember = "A non-empty request body is required.";

        internal const string ModelState_NonPropertyAttemptedValueIsInvalid = "The value '{0}' is not valid.";

        internal const string ModelState_NonPropertyUnknownValueIsInvalid = "The supplied value is invalid.";

        internal const string HtmlGeneration_NonPropertyValueMustBeNumber = "The field must be a number.";

        internal const string ModelState_UnknownValueIsInvalid = "The supplied value is invalid for {0}.";

        internal const string HtmlGeneration_ValueIsInvalid = "The value '{0}' is invalid.";

        internal const string HtmlGeneration_ValueMustBeNumber = "The field {0} must be a number.";

        internal const string ModelBinding_NullValueNotValid = "The value '{0}' is invalid.";
    }
}
