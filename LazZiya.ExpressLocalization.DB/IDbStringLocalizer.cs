using LazZiya.ExpressLocalization.DB.Models;
using Microsoft.Extensions.Localization;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Interface to implement IStringLocalizer for a specific resource and tranlsation types 
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public interface IDbStringLocalizer<TResource, TTranslation> : IStringLocalizer
        where TResource : class, IXLDbResource
        where TTranslation : class, IXLDbTranslation
    {
    }
}
