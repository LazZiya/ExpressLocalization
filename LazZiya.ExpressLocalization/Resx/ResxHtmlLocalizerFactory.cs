using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxHtmlLocalizerFactory
    /// </summary>
    public class ResxHtmlLocalizerFactory<TResource> : IExpressHtmlLocalizerFactory
        where TResource : IXLResource
    {
        private readonly ExpressMemoryCache _cache;
        private ExpressResourceManager _manager;

        /// <summary>
        /// Initialize a new instance of ResxHtmlLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cache"></param>
        public ResxHtmlLocalizerFactory(ExpressMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the default shared resource
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            _manager = new ExpressResourceManager(typeof(TResource));

            return new ResxHtmlLocalizer<TResource>(_cache, _manager);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            _manager = new ExpressResourceManager(resourceSource);

            return new ResxHtmlLocalizer(_cache, _manager);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given base name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            return Create();
        }
    }
}
