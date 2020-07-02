using LazZiya.ExpressLocalization.Common;
using System;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization.Resx
{
    public class ExpressResourceManager<T> : ExpressResourceManager, IExpressResourceManager<T>
        where T : IXLResource
    {
        public ExpressResourceManager()
            : base(typeof(T))
        {

        }
    }

    public class ExpressResourceManager : IExpressResourceManager
    {
        private readonly ResourceManager _manager;
        public ExpressResourceManager(Type type)
        {
            _manager = new ResourceManager(type);
        }

        public bool TryGetValue(string name, out string value)
        {
            value = _manager.GetString(name, CultureInfo.CurrentCulture);

            return value != null;
        }
    }
}
