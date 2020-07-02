using LazZiya.ExpressLocalization.Common;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Generic interface to create <see cref="ExpressResourceManager{TResource}"/> 
    /// for the specified resource of type <see cref="IXLResource"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExpressResourceManager<T> : IExpressResourceManager
        where T : IXLResource
    {

    }

    /// <summary>
    /// Interface to create <see cref="ExpressResourceManager"/>
    /// to get locaized valued from .resx files
    /// </summary>
    public interface IExpressResourceManager
    {
        /// <summary>
        /// Try get localized value for given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(string name, out string value);
    }
}
