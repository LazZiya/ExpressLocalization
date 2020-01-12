using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Specifies the numeric range constraints for the value of a data field. And produces localized error message.
    /// </summary>
    public sealed class ExRangeAttribute : RangeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalizatin.DataAnnotations.ExRangeAttribute class 
        /// by using the specified minimum and maximum values.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public ExRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.RangeAttribute_ValidationError;
        }

        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalizatin.DataAnnotations.ExRangeAttribute class 
        /// by using the specified minimum and maximum values.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public ExRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.RangeAttribute_ValidationError;
        }

        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalization.DataAnnotations.ExRangeAttribute
        ///     class by using the specified minimum and maximum values and the specific type.
        /// </summary>
        /// <param name="type">Specifies the type of the object to test.</param>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Type is null
        /// </exception>
        public ExRangeAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.RangeAttribute_ValidationError;
        }
    }
}
