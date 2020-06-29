using LazZiya.EFGenericDataManager;
using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Database html localizer factory
    /// </summary>
    public class DbHtmlLocalizerFactory<TResource, TTranslation> : IHtmlExpressLocalizerFactory
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IEFGenericDataManager _dataManager;
        private readonly IStringTranslator _stringTranslator;
        private readonly IHtmlTranslator _htmlTranslator;

        /// <summary>
        /// Initialize a new instance of DbHtmlLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataManager"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="htmlTranslator"></param>
        public DbHtmlLocalizerFactory(IOptions<ExpressLocalizationOptions> options, 
                               IEFGenericDataManager dataManager, 
                               IStringTranslator stringTranslator,
                               IHtmlTranslator htmlTranslator)
        {
            _options = options;
            _dataManager = dataManager;
            _stringTranslator = stringTranslator;
            _htmlTranslator = htmlTranslator;
        }

        /// <summary>
        /// Create a new instance of DbHtmlLocalizer
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new DbHtmlLocalizer<TResource, TTranslation>(_options, _dataManager, _stringTranslator, _htmlTranslator);
        }

        /// <summary>
        /// Create a new instance of DbHtmlLocalizer
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return Create();
        }

        /// <summary>
        /// Create a new instance of DbHtmlLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            return Create();
        }
    }
}
