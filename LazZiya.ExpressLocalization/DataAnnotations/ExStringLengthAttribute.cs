using LazZiya.ExpressLocalization.Messages;
using System.ComponentModel.DataAnnotations;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Specifies the minimum and maximum length of characters that are allowed in a data field. 
    /// And provides a localized error message
    /// </summary>
    public sealed class ExStringLengthAttribute : StringLengthAttribute
    {
        private bool IsCustomError;

        /// <summary>
        /// Gets or sets the minimum length of a string.
        /// </summary>
        /// <returns>The minimum length of a string.</returns>
        public new int MinimumLength
        {
            get => base.MinimumLength; 
            set
            {
                base.MinimumLength = value;
                if (!IsCustomError)
                    this.ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum;
            }
         }

        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalization.DataAnnotations.ExStringLengthAttribute 
        /// class by using a specified maximum length.
        /// </summary>
        /// <param name="maximumLength">The maximum length of a string.</param>
        public ExStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            this.IsCustomError = !(ErrorMessage is null);
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.StringLengthAttribute_ValidationError;
        }
    }
}
