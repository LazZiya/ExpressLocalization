using System;
using System.Collections.Generic;
using System.Globalization;

namespace LazZiya.ExpressLocalization.DB
{
    internal interface ICulturesProvider<TCultureResource, TKey>
        where TCultureResource : class, IExpressLocalizationCulture<TKey>
        where TKey : IEquatable<TKey>
    {
        IList<CultureInfo> ActiveCultures { get; }
        string DefaultCulture { get; }
    }
}
