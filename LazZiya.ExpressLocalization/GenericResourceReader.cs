using System.Resources;
using System.Globalization;

namespace LazZiya.ExpressLocalization
{
    internal class GenericResourceReader
    {
        internal static string GetValue<T>(string code, params object[] args) where T : class
        {
            var _res = new ResourceManager(typeof(T));

            var msg = _res.GetString(code, CultureInfo.CurrentCulture);

            return string.Format(msg, args);
        }
    }
}
