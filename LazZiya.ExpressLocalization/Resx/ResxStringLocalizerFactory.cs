using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Localization;
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
        private readonly ExpressMemoryCache _cache;
        private readonly IExpressResourceReader<TResource> _reader;

        /// <summary>
        /// Initialize a new instance of ResxStringLocalizerFactory
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="reader"></param>
        public ResxStringLocalizerFactory(ExpressMemoryCache cache, IExpressResourceReader<TResource> reader)
        {
            _cache = cache;
            _reader = reader;
        }

        /// <summary>
        /// Create new IStringLocalizer with the default shared resource
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new ResxStringLocalizer<TResource>(_cache, _reader);
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return Create();
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the specified name and location
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