using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization.Resx
{
    /// <summary>
    /// Generic .resx resource reader
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class ResxResourceReader<TResource> : ResxResourceReader
        where TResource : IExpressResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ResxResourceReader{TResource}"/> based on given type
        /// </summary>
        public ResxResourceReader(string location, ILoggerFactory loggerFactory)
            : base(typeof(TResource), location, loggerFactory)
        {

        }
    }

    /// <summary>
    /// A <see cref="ResxResourceReader"/> to read .resx files
    /// </summary>
    public class ResxResourceReader
    {
        private readonly ResourceManager _manager;
        private readonly ILogger _logger;

        /// <summary>
        /// The searched location
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Initialize a new instance of <see cref="ResxResourceReader"/> based on the default resource type
        /// </summary>
        public ResxResourceReader(Type type, string location, ILoggerFactory loggerFactory)
        {
            if (type == null)
                throw new NotImplementedException(nameof(type));

            // Create a fully qualified resource name
            TypeName = ResourceTypeHelper.CreateResourceName(type, location);
            
            _logger = loggerFactory.CreateLogger<ResxResourceReader>();
            
            _manager = new ResourceManager($"{TypeName}", type.Assembly);
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
            catch(Exception e)
            {
                value = null;
                _logger.LogError(e.Message);
            }

            return value != null;
        }
    }
}
