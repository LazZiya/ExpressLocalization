﻿using LazZiya.ExpressLocalization.Messages;
using System.ComponentModel.DataAnnotations;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field is required, and provides a localized error message
    /// </summary>
    public sealed class ExRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalization.DataAnnotations.ExRequiredAttribute class.
        /// </summary>
        public ExRequiredAttribute() : base()
        {
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.RequiredAttribute_ValidationError;
        }
    }
}
