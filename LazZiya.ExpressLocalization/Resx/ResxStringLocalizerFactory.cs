using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxStringLocalizerFactory
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxStringLocalizerFactory<TResource> : IExpressStringLocalizerFactory
        where TResource : IExpressResource
    {
        private readonly ExpressMemoryCache _cache;
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly ILoggerFactory _logger;
        private readonly ConcurrentDictionary<string, ResxStringLocalizer> _localizerCache = new ConcurrentDictionary<string, ResxStringLocalizer>();

        /// <summary>
        /// Initialize a new instance of ResxStringLocalizerFactory
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public ResxStringLocalizerFactory(ExpressMemoryCache cache, 
                                          IOptions<ExpressLocalizationOptions> options, 
                                          ILoggerFactory loggerFactory)
        {
            _cache = cache;
            _options = options;
            _logger = loggerFactory;
        }

        /// <summary>
        /// Create new IStringLocalizer with the default shared resource
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return Create(typeof(TResource));
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            return _localizerCache.GetOrAdd(resourceSource.FullName, _ => new ResxStringLocalizer(resourceSource, _cache, _options, _logger));
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the specified name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            throw new NotSupportedException($"Creating a localizer using 'baseName' and 'location' is not supported! Use .Create() or .Create(Type resourceSource) instead.");
        }
    }
}