using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Reflection;

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
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
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
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ResxStringLocalizerFactory<TResource>>();
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

            return _localizerCache.GetOrAdd(resourceSource.FullName, _ => new ResxStringLocalizer(resourceSource, _cache, _options, _loggerFactory));
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the specified name and location
        /// </summary>
        /// <param name="baseName">Type full name</param>
        /// <param name="location">Assembly name</param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            return _localizerCache.GetOrAdd($"B={baseName},L={location}", _ =>
            {
                var type = ResourceTypeHelper.GetResourceType(baseName, location);

                return new ResxStringLocalizer(type, _cache, _options, _loggerFactory);
            });
        }
    }
}