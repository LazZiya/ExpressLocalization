namespace LazZiya.ExpressLocalization.Common
{
    /// <summary>
    /// Generic interface to implement read/write on resource files
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public interface IExpressResourceReaderWriter<TResource> : IExpressResourceReaderWriter, IExpressResourceReader<TResource>, IExpressResourceWriter<TResource>
        where TResource : IXLResource
    {

    }

    /// <summary>
    /// interface to implement read/write on resource files
    /// </summary>
    public interface IExpressResourceReaderWriter : IExpressResourceReader, IExpressResourceWriter
    {
    }
}
