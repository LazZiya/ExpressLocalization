using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxHtmlLocalizerFactory
    /// </summary>
    public class ResxHtmlLocalizerFactory<TResource> : IExpressHtmlLocalizerFactory
        where TResource : IExpressResource
    {
        private readonly IStringLocalizerFactory _factory;

        /// <summary>
        /// Initialize a new instance of ResxHtmlLocalizerFactory
        /// </summary>
        /// <param name="factory"></param>
        public ResxHtmlLocalizerFactory(IStringLocalizerFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the default shared resource
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new ResxHtmlLocalizer<TResource>(_factory);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
                throw new NotImplementedException(nameof(resourceSource));

            var localizer = _factory.Create(resourceSource);

            return new ResxHtmlLocalizer(localizer);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given base name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(string baseName, string location)
        {
            throw new NotSupportedException($"Creating a localizer using 'baseName' and 'location' is not supported! Use .Create() or .Create(Type resourceSource) instead.");
        }
    }
}
