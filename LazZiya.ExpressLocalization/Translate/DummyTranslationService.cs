using LazZiya.TranslationServices;
using System;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// This class has no any trnaslation functionality.
    /// It is provided just to avoid the exceptions where having 
    /// a registered translation service is a must.
    /// </summary>
    public class DummyTranslationService : ITranslationService
    {
        /// <summary>
        /// Translation service name
        /// </summary>
        public string ServiceName => "No service";

        /// <summary>
        /// This will return the same string with status code 306 (Unused).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public Task<TranslationResult> TranslateAsync(string source, string target, string text, string format)
        {
            throw new NotImplementedException("No translation service is defined! Please register and define a translation service.");
        }
    }
}
