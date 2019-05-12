using System;
using System.Linq;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// use this class to read properties from resource files with public properties
    /// </summary>
    internal static class GenericPropertyReader
    {
        public static string GetPropertyValue(string propertyName, Type type)
        {
            //if (type.GetProperties().Any(p => p.Name == propertyName))
            {
                return type.GetProperty(propertyName).GetValue(null).ToString();
            }

            //throw new NotImplementedException("The requested localization resource key is not implemented!");
        }

    }
}
