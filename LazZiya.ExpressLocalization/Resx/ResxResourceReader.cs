using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
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
        /// Initialize a new instance of <see cref="ResxResourceReader"/> based on the default resource type
        /// </summary>
        public ResxResourceReader(Type type, string location, ILoggerFactory loggerFactory)
        {
            if (type == null)
                throw new NotImplementedException(nameof(type));

            var resourceName = GetResourceName(type, location);
            
            _logger = loggerFactory.CreateLogger<ResxResourceReader>();
            
            _manager = new ResourceManager($"{resourceName}", type.Assembly);
        }

        /// <summary>
        /// ResourceManager looks for .resources files in the project output directory.
        /// So if our resource file is .\LocalizationResources\LocSource.tr.resx
        /// Then ResourceManager will look for: SampleProject.LocalizationResources.LocSource.tr.resources
        ///
        /// The resource name must be in the following format:
        /// {assembly-name}.{resources-folder}.{resource-namespace}.{resource-name}
        /// e.g. SampleProject.LocalizationResources.LocSource
        /// e.g. SampleProject.LocalizationResources.Pages.Products.IndexModel
        ///
        /// The challenge is when we have a type name for a PageModel out of resources-folder,
        /// such type has no class inside the resources folder, so we have to reconstruct the
        /// type full name to fulfill above format.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="location">Resources folder path</param>
        /// <returns></returns>
        private string GetResourceName(Type type, string location)
        {
            // Get {assembly-name}
            // e.g.: SampleProject
            var assemblyName = type.Assembly.GetName().Name;

            var locationAsNamespace = location.Replace(Path.DirectorySeparatorChar, '.').Replace(Path.AltDirectorySeparatorChar, '.');

            // If we have a type resource already inside the resources folder
            // take the full type name
            // Otherwise, the type is out of resources folder, so we need to add the resources folder name
            // to the type full name
            return type.FullName.StartsWith($"{assemblyName}.{locationAsNamespace}.")
                ? type.FullName
                : type.FullName.Replace($"{assemblyName}.", $"{assemblyName}.{locationAsNamespace}.");
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
