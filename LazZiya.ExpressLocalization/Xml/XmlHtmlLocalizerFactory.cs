using LazZiya.ExpressLocalization.Common;
using Microsoft.AspNetCore.Mvc.Localization;
using System;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// XmlHtmlLocalizerFactory
    /// </summary>
    public class XmlHtmlLocalizerFactory<TResource> : IExpressHtmlLocalizerFactory
        where TResource : IExpressResource
    {
        private readonly IExpressStringLocalizerFactory _factory;
        /// <summary>
        /// Instantiate a new XmlStringLocalizerFactory
        /// </summary>
        /// <param name="factory"></param>
        public XmlHtmlLocalizerFactory(IExpressStringLocalizerFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Create new XmlStringLocalizer using the default type
        /// </summary>
        /// <returns></returns>
        public IHtmlLocalizer Create()
        {
            return new XmlHtmlLocalizer<TResource>(_factory);
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
            var localizer = _factory.Create(baseName, location);
            return new XmlHtmlLocalizer(localizer);
        }
    }
}
