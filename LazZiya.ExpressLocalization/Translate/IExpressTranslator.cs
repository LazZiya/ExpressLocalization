using LazZiya.TranslationServices;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// Generic interface to implement ExpressTranslator service
    /// that will connext to external translation services
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IExpressTranslator<TService> : IExpressTranslator
        where TService : ITranslationService
    {

    }

    /// <summary>
    /// Interface to implement ExpressTranslator service
    /// that will connext to external translation services
    /// </summary>
    public interface IExpressTranslator
    {
        /// <summary>
        /// Try get translation
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format">text ot html</param>
        /// <param name="translation"></param>
        /// <returns></returns>
        bool TryTranslate(string text, string format, out string translation);

        /// <summary>
        /// Try get translation
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="format">text or html</param>
        /// <param name="translation"></param>
        /// <returns></returns>
        bool TryTranslate(string text, string from, string to, string format, out string translation);
    }
}