using LazZiya.TranslationServices;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// ExpressTranslator to get a translated text from the default translation service
    /// </summary>
    public class ExpressTranslator<TService> : IExpressTranslator<TService>
        where TService : ITranslationService
    {
        private readonly ITranslationService _service;
        private readonly ExpressLocalizationOptions _options;

        /// <summary>
        /// Initialize a new instance of ExpressTranslator
        /// </summary>
        /// <param name="translationServices"></param>
        /// <param name="options"></param>
        public ExpressTranslator(IEnumerable<ITranslationService> translationServices, IOptions<ExpressLocalizationOptions> options)
        {
            _service = translationServices.FirstOrDefault(x => x.GetType() == typeof(TService));
            _options = options.Value;
        }

        /// <summary>
        /// Try translate a text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="format">text ot html</param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public bool TryTranslate(string text, string from, string to, string format, out string translation)
        {
            var trans = _service.TranslateAsync(from, to, text, format).Result;

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                translation = trans.Text;
                return true;
            }

            translation = text;
            return false;
        }

        /// <summary>
        /// Try translate a text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format">text ot html</param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public bool TryTranslate(string text, string format, out string translation)
        {
            var from = _options.DefaultCultureName;

            var to = CultureInfo.CurrentCulture.Name;

            var trans = _service.TranslateAsync(from, to, text, format).Result;

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                translation = trans.Text;
                return true;
            }

            translation = text;
            return false;
        }
    }
}
