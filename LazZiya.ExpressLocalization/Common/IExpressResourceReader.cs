namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Generic interface to create resource reader 
    /// that will read .resx or .xml resource files 
    /// for the specified resource of type <see cref="IXLResource"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExpressResourceReader<T> : IExpressResourceReader
        where T : IXLResource
    {

    }

    /// <summary>
    /// Generic interface to create resource reader 
    /// that will read .resx or .xml resource files 
    /// for the default resource type
    /// to get locaized valued from .resx files
    /// </summary>
    public interface IExpressResourceReader
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
