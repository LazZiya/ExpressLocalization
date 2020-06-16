using LazZiya.ExpressLocalization.Common;
using LazZiya.ExpressLocalization.Translate;
using LazZiya.TranslationServices;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization.TagHelpers
{
    /// <summary>
    /// base class for localize tag helpers
    /// </summary>
    public class LocalizationTagHelperBase : TagHelper
    {
        private readonly IHtmlExpressLocalizerFactory _localizerFactory;
        private readonly IHtmlTranslatorFactory _translatorFactory;
        private readonly ExpressLocalizationOptions _options;

        /// <summary>
        /// pass array of objects for arguments
        /// </summary>
        [HtmlAttributeName("localize-args")]
        public object[] Args { get; set; }

        /// <summary>
        /// Localize or translate contents to the specified target culture. Default is current culture.
        /// </summary>
        [HtmlAttributeName("localize-culture")]
        public string Culture { get; set; } = string.Empty;

        /// <summary>
        /// Manually specify the content culture. Default is request default culture.
        /// </summary>
        [HtmlAttributeName("localize-translation-from")]
        public string TranslationFromCulture { get; set; } = string.Empty;

        /// <summary>
        /// Type of translation service.
        /// </summary>
        [HtmlAttributeName("localize-translation-service")]
        public Type TranslationServiceType { get; set; }
        
        /// <summary>
        /// Type of the localized resource
        /// </summary>
        [HtmlAttributeName("localize-source")]
        public Type ResourceSource { get; set; }

        /// <summary>
        /// Initialize a new instance of LocaizationTagHelperBase
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        public LocalizationTagHelperBase(IServiceProvider provider, IOptions<ExpressLocalizationOptions> options)
        {
            _localizerFactory = provider.GetRequiredService<IHtmlExpressLocalizerFactory>();

            _options = options.Value;
            
            if(_options.OnlineTranslation)
                _translatorFactory = provider.GetRequiredService<IHtmlTranslatorFactory>();
        }

        /// <summary>
        /// process localize tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();

            if (!string.IsNullOrWhiteSpace(content.GetContent()))
            {
                var str = content.GetContent().Trim();

                LocalizedHtmlString _localStr;

                if (string.IsNullOrWhiteSpace(Culture))
                {
                    var _loc = ResourceSource == null
                        ? _localizerFactory.Create()
                        : _localizerFactory.Create(ResourceSource);

                    _localStr = Args == null
                        ? _loc[str]
                        : _loc[str, Args];
                }
                else
                {
                    using (var cs = new CultureSwitcher(Culture))
                    {
                        var _loc = ResourceSource == null
                            ? _localizerFactory.Create()
                            : _localizerFactory.Create(ResourceSource);

                        _localStr = Args == null
                            ? _loc[str]
                            : _loc[str, Args];
                    }
                }

                if (_localStr.IsResourceNotFound && _options.OnlineTranslation)
                {
                    var _loc = TranslationServiceType != null && TranslationServiceType.GetInterface(typeof(ITranslationService).FullName) != null
                        ? _translatorFactory.Create(TranslationServiceType)
                        : _translatorFactory.Create();

                    _localStr = Args == null
                        ? _loc[str, TranslationFromCulture, Culture]
                        : _loc[str, TranslationFromCulture, Culture];
                }

                output.Content.SetHtmlContent(_localStr.Value);
            }
        }
    }
}
