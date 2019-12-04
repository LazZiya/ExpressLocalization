using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Switches the current culture to a specific culture then return to the original culture and dispose.
    /// Since Localizer.WithCultre has became obsolete, 
    /// this is a solution to retrive a string from specific culture resource file
    /// <see ref="https://github.com/aspnet/AspNetCore/issues/7756"/>
    /// <see ref="https://github.com/jcemoller/culture-switcher/blob/master/src/CultureSwitcher.cs"/>
    /// </summary>
    public class CultureSwitcher : IDisposable
    {
        private readonly CultureInfo _originalCulture;

        /// <summary>
        /// Since Localizer.WithCultre has became obsolete, 
        /// this is a solution to retrive a string from specific culture resource file
        /// </summary>
        public CultureSwitcher(string culture)
        {
            if (!CultureInfo.Equals(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture))
            {
                throw new InvalidOperationException(
                    "Different CurrentCulture and CurrentUICulture culture is not supported."
                    );
            }

            _originalCulture = CultureInfo.CurrentCulture;
            var cultureInfo = String.IsNullOrEmpty(culture) ? CultureInfo.CurrentCulture : new CultureInfo(culture);

            SetCulture(cultureInfo);
        }

        private void SetCulture(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        /// <summary>
        /// dispose...
        /// </summary>
        public void Dispose()
        {
            SetCulture(_originalCulture);
        }
    }
}
