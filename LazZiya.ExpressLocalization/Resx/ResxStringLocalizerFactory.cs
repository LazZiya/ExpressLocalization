using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// ResxStringLocalizerFactory
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxStringLocalizerFactory<TResource> : IStringExpressLocalizerFactory
        where TResource : class
    {
        private readonly IOptions<ExpressLocalizationOptions> _options;
        private readonly IStringTranslator _stringTranslator;
        private readonly IStringLocalizerFactory _factory;

        /// <summary>
        /// Initialize a new instance of ResxStringLocalizerFactory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="stringTranslator"></param>
        /// <param name="factory"></param>
        public ResxStringLocalizerFactory(IOptions<ExpressLocalizationOptions> options,
                                          IStringTranslator stringTranslator,
                                          IStringLocalizerFactory factory)
        {
            _options = options;
            _stringTranslator = stringTranslator;
            _factory = factory;
        }

        /// <summary>
        /// Create new IStringLocalizer with the default shared resource
        /// </summary>
        /// <returns></returns>
        public IStringLocalizer Create()
        {
            return new ResxStringLocalizer<TResource>(_options, _stringTranslator, _factory);
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the given resource type
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            return new ResxStringLocalizer(resourceSource, _options, _stringTranslator, _factory);
        }

        /// <summary>
        /// Create a new IStringLocalizer based on the specified name and location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new ResxStringLocalizer(baseName, location, _options, _stringTranslator, _factory);
        }
    }
}
