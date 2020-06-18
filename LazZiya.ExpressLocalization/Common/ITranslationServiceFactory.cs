using LazZiya.TranslationServices;

namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Interface to create ITranslationServices
    /// </summary>
    public interface ITranslationServiceFactory
    {
        /// <summary>
        /// Create new ITranslationService based on the default ITranslationService type defined in startup
        /// </summary>
        /// <returns></returns>
        ITranslationService Create();

        /// <summary>
        /// Create new ITranslationService
        /// </summary>
        /// <returns></returns>
        ITranslationService Create<TService>()
            where TService : ITranslationService;
    }
}
