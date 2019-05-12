using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LazZiya.ExpressLocalization
{
    public class LocalizationResourcesTypes
    {
        public Type DataAnnotations { get; set; }
        public Type Views { get; set; }
        public Type ModelBinding { get; set; }
        public Type IdentityDescriber { get; set; }
    }
    public class ExpressLocalizationOptions
    {
        public Action<LocalizationResourcesTypes> LocalizationResourcesTypes { get; set; }
        public Action<RequestLocalizationOptions> RequestLocalizationOptions { get; set; }
        public Action<MvcOptions> MvcOptions { get; set; }
        public Action<LocalizationOptions> LocalizationOptions { get; set; }
    }
}
