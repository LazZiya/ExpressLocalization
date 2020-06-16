using LazZiya.TranslationServices;
using System;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// IHtmlTranslatorFactory
    /// </summary>
    public interface IHtmlTranslatorFactory
    {
        /// <summary>
        /// Create new instance of IHtmlTranslator using the default translation service
        /// </summary>
        /// <returns></returns>
        IHtmlTranslator Create();
        
        /// <summary>
        /// Create new instance of IHtmlTranslator using the specified type
        /// </summary>
        /// <returns></returns>
        IHtmlTranslator Create(Type type);
        
        /// <summary>
        /// Create new instance of IHtmlTranslator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IHtmlTranslator Create<T>()
            where T : ITranslationService;
    }
}
