using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Mvc.Localization;
using System;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Database html localizer factory
    /// </summary>
    public class DbHtmlLocalizerFactory<TResource, TTranslation> : IExpressHtmlLocalizerFactory
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
        private readonly IDbStringLocalizer<TResource, TTranslation> _localizer;

        /// <summary>
        /// Initialize a new instance of DbHtmlLocalizerFactory
        /// </summary>
        /// <param name="localizer"></param>
        public DbHtmlLocalizerFactory(IDbStringLocalizer<TResource, TTranslation> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Create a new instance of DbHtmlLocalizer
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new DbHtmlLocalizer<TResource, TTranslation>(_localizer);
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
