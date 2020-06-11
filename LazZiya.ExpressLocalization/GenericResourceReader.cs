using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Reads resource key and return relevant localized string value
    /// </summary>
    public class GenericResourceReader
    {
        /// <summary>
        /// Reads resource key and return relevant localized string value
        /// </summary>
        /// <param name="culture">Culture name e.g. ar-SY</param>
        /// <param name="code">key name to look for</param>
        /// <param name="args"></param>
        /// <param name="resourceSource">resx type to get values from</param>
        /// <returns></returns>
        public static string GetString(Type resourceSource, string culture, string code, params object[] args)
        {
            return string.Format(GetHtmlString(resourceSource, culture, code, args).Value, args);
        }

        /// <summary>
        /// Reads resource key and return relevant localized string value
        /// </summary>
        /// <param name="resourceSource">Type of the resource that contains the localized strings</param>
        /// <param name="culture">Culture name e.g. ar-SY</param>
        /// <param name="code">key name to look for</param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static LocalizedHtmlString GetHtmlString(Type resourceSource, string culture, string code, params object[] args)
        {
            var _res = new System.Resources.ResourceManager(resourceSource);

            var cultureInfo = string.IsNullOrWhiteSpace(culture)
                ? CultureInfo.CurrentCulture
                : CultureInfo.GetCultureInfo(culture);

            bool _resourceNotFound;
            string _value;

            try
            {
                _value = _res.GetString(code, cultureInfo);
                _resourceNotFound = false;
            }
            catch (MissingSatelliteAssemblyException)
            {
                _resourceNotFound = true;
                _value = code;
            }
            catch (MissingManifestResourceException)
            {
                _resourceNotFound = true;
                _value = code;
            }

            return args == null
                ? new LocalizedHtmlString(code, _value, _resourceNotFound)
                : new LocalizedHtmlString(code, _value, _resourceNotFound, args);
        }
    }
}
