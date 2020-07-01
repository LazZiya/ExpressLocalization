using LazZiya.ExpressLocalization.Cache;
using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxStringLocalizerFactory
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxStringLocalizerFactory<TResource> : IExpressStringLocalizerFactory
        where TResource : IXLResource
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly ExpressMemoryCache _cache;

        /// <summary>
        /// Initialize a new instance of ResxStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cache"></param>
        public ResxStringLocalizerFactory(ExpressMemoryCache cache, IOptions<ExpressLocalizationOptions> options)
        {
            _options = options;
            _cache = cache;
        }

        /// <summary>
        /// Create new IStringLocalizer with the default shared resource
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new ResxStringLocalizer<TResource>(_cache, _options);
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return new ResxStringLocalizer(_cache, resourceSource, _options.Value.ResourcesPath, _options);
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the specified name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new ResxStringLocalizer(_cache, baseName, location, _options);
        }
    }
}
