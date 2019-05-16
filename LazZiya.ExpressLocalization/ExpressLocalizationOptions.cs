using Microsoft.AspNetCore.Builder;
using System;

namespace LazZiya.ExpressLocalization
{
    public class ExpressLocalizationOptions
    {
        public Action<RequestLocalizationOptions> RequestLocalizationOptions { get; set; }
    }
}
