using System.Globalization;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Reads resource key and return relevant localized string value
    /// </summary>
    internal class GenericResourceReader
    {
        /// <summary>
        /// Reads resource key and return relevant localized string value
        /// </summary>
        /// <typeparam name="T">type of resource file that containes localized string values</typeparam>
        /// <param name="code">key name to look for</param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string GetValue<T>(string code, params object[] args) where T : class
        {
            var _res = new System.Resources.ResourceManager(typeof(T));

            string msg;
            try
            {
                msg = _res.GetString(code, CultureInfo.CurrentCulture);
            }
            catch 
            {
                msg = code;
            }

            return string.Format(msg, args);
        }
    }
}
