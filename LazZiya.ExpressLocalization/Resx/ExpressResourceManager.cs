using LazZiya.ExpressLocalization.Common;
using System;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Generic resource manager
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ExpressResourceManager<TResource> : ExpressResourceManager, IExpressResourceManager<TResource>
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ExpressResourceManager{TResource}"/>
        /// </summary>
        public ExpressResourceManager()
            : base(typeof(TResource))
        {

        }
    }

    /// <summary>
    /// For reading localized values from .resx resources
    /// </summary>
    public class ExpressResourceManager : IExpressResourceManager
    {
        private readonly ResourceManager _manager;

        /// <summary>
        /// Initialzie a new instance of <see cref="ExpressResourceManager"/>
        /// </summary>
        /// <param name="type"></param>
        public ExpressResourceManager(Type type)
        {
            _manager = new ResourceManager(type);
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
