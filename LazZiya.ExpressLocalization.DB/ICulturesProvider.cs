using LazZiya.ExpressLocalization.DB.Models;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    internal interface ICulturesProvider<TCultureResource>
        where TCultureResource : class, IXLCulture
    {
        ICollection<CultureInfo> ActiveCultures { get; }
        string DefaultCulture { get; }
    }
}
