using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// DbStringLocalizer factory
    /// </summary>
    public class DbStringLocalizerFactory<TResource, TTranslation> : IExpressStringLocalizerFactory
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly IEFGenericDataManager _dataManager;
        private readonly IExpressTranslator _translator;
        private readonly ExpressMemoryCache _cache;
        private readonly IOptions<ExpressLocalizationOptions> _options;

        /// <summary>
        /// Initialize a new instance of DbStringLocalizerFactory
        /// </summary>
        public DbStringLocalizerFactory(IEFGenericDataManager dataManager,
                                        IExpressTranslator translator,
                                        ExpressMemoryCache cache,
                                        IOptions<ExpressLocalizationOptions> options)
        {
            _dataManager = dataManager;
            _translator = translator;
            _cache = cache;
            _options = options;
        }

        /// <summary>
        /// Create a new IStringLocalizer using the default settings
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new DbStringLocalizer<TResource, TTranslation>(_dataManager, _translator, _cache, _options);
        }

        /// <summary>
        /// Create a new IStringLocalizer for the given type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return Create();
        }

        /// <summary>
        /// Create a new IStringLocalizer using the given type name
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return Create();
        }
    }
}
