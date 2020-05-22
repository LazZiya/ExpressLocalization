using LazZiya.ExpressLocalization.DB.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    internal interface ICulturesProvider<TCultureResource>
        where TCultureResource : class, IExpressLocalizationCulture
    {
        IList<CultureInfo> ActiveCultures { get; }
        string DefaultCulture { get; }
    }
}
