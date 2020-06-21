using System.Text.RegularExpressions;

namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Trim all whitespaces in a string (start, end and between)
    /// </summary>
    internal static class Trimmer
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+"); 
        internal static string ReplaceWhitespace(this string input, string replacement) 
        { 
            return sWhitespace.Replace(input.Trim(), replacement); 
        }
    }
}
