using LazZiya.ExpressLocalization.Common;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Generic .resx resource reader
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxResourceReader<TResource> : IExpressResourceReader<TResource>
        where TResource : IXLResource
    {
        private readonly ResourceManager _manager;

        /// <summary>
        /// Initialize a new instance of <see cref="ResxResourceReader{TResource}"/>
        /// </summary>
        public ResxResourceReader()
        {
            _manager = new ResourceManager(typeof(TResource));
        }

        /// <summary>
        /// Try get a localized value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out string value)
        {
            try
            {
                value = _manager.GetString(name, CultureInfo.CurrentCulture);
            }
            catch
            {
                value = null;
            }

            return value != null;
        }
    }
}
