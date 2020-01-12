﻿using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LazZiya.ExpressLocalization.DataAnnotations
{
    /// <summary>
    /// Provides an attribute that compares two properties, and produces localized error message.
    /// </summary>
    public sealed class ExCompareAttribute : CompareAttribute
    {
        /// <summary>
        /// Initializes a new instance of the LazZiya.ExpressLocalization.DataAnnotations.ExCompareAttribute class.
        /// <paramref name="otherProperty">The property to compare with the current property.</paramref>
        /// </summary>
        public ExCompareAttribute(string otherProperty) : base(otherProperty)
        {
            this.ErrorMessage = ErrorMessage ?? DataAnnotationsErrorMessages.CompareAttribute_MustMatch;
        }
    }
}
