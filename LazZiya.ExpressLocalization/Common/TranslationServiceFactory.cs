using LazZiya.TranslationServices;
using System;

namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// TranslationServiceFactory
    /// </summary>
    public class TranslationServiceFactory<T> : ITranslationServiceFactory
        where T : ITranslationService
    {
        /// <summary>
        /// Create new ITranslationService based on the default ITranslationService type defined in startup
        /// </summary>
        /// <returns></returns>
        public ITranslationService Create()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    
        /// <summary>
        /// Create new ITranslationService based on the T type
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public ITranslationService Create<TService>()
            where TService : ITranslationService
        {
            return (TService)Activator.CreateInstance(typeof(TService));
        }
    }
}
