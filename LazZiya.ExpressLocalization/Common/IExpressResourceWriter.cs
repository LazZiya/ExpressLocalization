namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Generic interface to create resource writer
    /// that will read .resx or .xml resource files 
    /// for the specified resource of type <see cref="IXLResource"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExpressResourceWriter<T> : IExpressResourceWriter
        where T : IXLResource
    {

    }

    /// <summary>
    /// Generic interface to create resource writer 
    /// that will read .resx or .xml resource files 
    /// for the default resource type
    /// to get locaized valued from .resx files
    /// </summary>
    public interface IExpressResourceWriter
    {
        /// <summary>
        /// Try get localized value for given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        bool TrySetValue(string name, string value, string comment, bool isApproved);
    }
}
