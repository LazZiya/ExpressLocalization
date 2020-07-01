using LazZiya.ExpressLocalization.Common;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization.Resx
{
    public class ExpressResourceManager<T>
        where T : IXLResource
    {
        private readonly ResourceManager _manager;
        public ExpressResourceManager()
        {
            _manager = new ResourceManager(typeof(T));
        }

        public bool TryGetValue(string name, out string value)
        {
            value = _manager.GetString(name, CultureInfo.CurrentCulture);

            return value != null;
        }
    }
}
