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
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IEFGenericDataManager _dataManager;
        private readonly IStringTranslator _stringTranslator;

        /// <summary>
        /// Initialize a new instance of DbStringLocalizerFactory
        /// </summary>
        public DbStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                                        IEFGenericDataManager dataManager, 
                                        IStringTranslator stringTranslator)
        {
            _options = options;
            _dataManager = dataManager;
            _stringTranslator = stringTranslator;
        }

        /// <summary>
        /// Create a new IStringLocalizer using the default settings
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new DbStringLocalizer<TResource, TTranslation>(_options, _dataManager, _stringTranslator);
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
