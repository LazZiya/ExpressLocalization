using LazZiya.ExpressLocalization.Common;

namespace LazZiya.ExpressLocalization.Resx
{
    public interface IExpressResourceManager<T> : IExpressResourceManager
        where T : IXLResource
    {

    }

    public interface IExpressResourceManager
    {
        bool TryGetValue(string name, out string value);
    }
}
