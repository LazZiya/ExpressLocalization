using LazZiya.ExpressLocalization.Xml;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization.TagHelpers
{
    /// <summary>
    /// Localization tag helper, localize text inside <![CDATA[<localize>Hellow</localize>]]>
    /// </summary>
    public class LocalizeTagHelper : LocalizationTagHelperBase
    {
        /// <summary>
        /// Initialize a new instance of LocalizeTagHelper
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        public LocalizeTagHelper(IServiceProvider provider, IOptions<ExpressLocalizationOptions> options) 
            : base(provider, options)
        {
        }

        /// <summary>
        /// process localize tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //replace <localize> tag with <span>
            output.TagName = "";
            await base.ProcessAsync(context, output);
        }
    }
}
