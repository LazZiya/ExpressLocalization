using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.AspNetCore.Mvc.Localization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Interface to implement IHtmlLocalizer for a specific resource and tranlsation types 
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public interface IDbHtmlLocalizer<TResource, TTranslation> : IHtmlLocalizer
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
    }
}
