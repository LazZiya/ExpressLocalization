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
        where TResource : IXLResource
    {
        private readonly IStringLocalizer<TResource> _localizer;

        /// <summary>
        /// Initialize a new instance of ResxHtmlLocalizerFactory
        /// </summary>
        /// <param name="localizer"></param>
        public ResxHtmlLocalizerFactory(IStringLocalizer<TResource> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the default shared resource
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new ResxHtmlLocalizer<TResource>(_localizer);
        }

        /// <summary>
        /// Create a new instance of IHtmlLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            return Create();
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
