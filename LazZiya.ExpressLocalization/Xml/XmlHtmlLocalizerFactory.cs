using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlHtmlLocalizerFactory
    /// </summary>
    public class XmlHtmlLocalizerFactory<TResource> : IExpressHtmlLocalizerFactory
        where TResource : IXLResource
    {
        private readonly IStringLocalizerFactory _factory;

        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="factory"></param>
        public XmlHtmlLocalizerFactory(IStringLocalizerFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return Create(typeof(TResource));
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IHtmlLocalizer Create(Type resourceSource)
        {
            var localizer = _factory.Create(resourceSource);
            
            return new XmlHtmlLocalizer(localizer);
        }

        /// <summary>
        /// Create new instance of IStringLocalizer
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
