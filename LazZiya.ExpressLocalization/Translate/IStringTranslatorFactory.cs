using LazZiya.TranslationServices;
using System;

namespace LazZiya.ExpressLocalization.Translate
{
    /// <summary>
    /// IHtmlTranslatorFactory
    /// </summary>
    public interface IStringTranslatorFactory
    {
        /// <summary>
        /// Create new instance of IStringTranslator using the default translation service
        /// </summary>
        /// <returns></returns>
        IStringTranslator Create();
        
        /// <summary>
        /// Create new instance of IStringTranslator using the specified type
        /// </summary>
        /// <returns></returns>
        IStringTranslator Create(Type type);
        
        /// <summary>
        /// Create new instance of IStringTranslator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IStringTranslator Create<T>()
            where T : ITranslationService;
    }
}
