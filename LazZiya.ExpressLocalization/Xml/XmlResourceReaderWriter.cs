using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic resource manager
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlResourceReaderWriter<TResource> : XmlResourceReaderWriter
        where TResource : IExpressResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="XmlResourceReaderWriter{TResource}"/>
        /// </summary>
        public XmlResourceReaderWriter(IOptions<ExpressLocalizationOptions> options, ILoggerFactory loggerFactory)
            : base(typeof(TResource), options.Value.ResourcesPath, loggerFactory)
        {

        }
    }

    /// <summary>
    /// For reading localized values from .resx resources
    /// </summary>
    public class XmlResourceReaderWriter
    {
        private readonly string _path;
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly ILogger _logger;

        /// <summary>
        /// Searched location
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Initialzie a new instance of <see cref="XmlResourceReaderWriter"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="location">Resources folder path</param>
        /// <param name="loggerFactory"></param>
        public XmlResourceReaderWriter(Type type, string location, ILoggerFactory loggerFactory)
        {
            if (type == null)
                throw new NotImplementedException(nameof(type));

            _logger = loggerFactory.CreateLogger<XmlResourceReaderWriter>();

            TypeName = ResourceTypeHelper.CreateResourceName(type, location);

            _path = $".\\{location}\\{TypeName}.{{0}}.xml";
        }

        /// <summary>
        /// Try get a localized value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out string value)
        {
            var path = string.Format(_path, CultureInfo.CurrentCulture.Name);

            var _doc = GetXmlDocument(path);

            _lock.EnterReadLock();
            try
            {
                var elmnt = Find(name, _doc);
                value = elmnt?.Element("value").Value;
            }
            catch
            {
                value = null;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return value != null;
        }

        private XElement Find(string name, XDocument doc)
        {
            return doc.Root.Descendants("data").FirstOrDefault(x => x.Element("key").Value.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Try set a localized value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public bool TrySetValue(string name, string value, string comment, bool isApproved)
        {
            var path = string.Format(_path, CultureInfo.CurrentCulture.Name);

            var _doc = GetXmlDocument(path);

            var xElement = CreateXElement(name, value, comment, isApproved);

            var success = false;

            _lock.EnterUpgradeableReadLock();
            try
            {
                if (Find(name, _doc) == null)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        _doc.Root.Add(xElement);
                        _doc.Save(path);
                        success = true;
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                    _logger.LogInformation($"New key adding result: '{success}', key name: '{name}', path: '{path}'");
                }
            }
            catch
            {
                value = null;
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            return success;
        }

        private XElement CreateXElement(string name, string value, string comment, bool isApproved)
        {
            return new XElement("data", new XAttribute("isActive", isApproved),
                                        new XElement("key", name),
                                        new XElement("value", value),
                                        new XElement("comment", comment ?? "Created by ExpressLocalization"));
        }

        /// <summary>
        /// Helper method to create Xml resource file
        /// </summary>
        /// <param name="fPath"></param>
        /// <returns></returns>
        private XDocument GetXmlDocument(string fPath)
        {
            if (!File.Exists(fPath))
            {
                try
                {
                    // Create a copy of the template xml resource
                    var assemblyPath = typeof(XmlTemplate).Assembly.Location;
                    var path = assemblyPath.Substring(0, assemblyPath.LastIndexOf('\\'));
                    File.Copy($"{path}\\Xml\\XmlTemplate.xml", fPath);
                }
                catch (Exception e)
                {
                    throw new FileLoadException($"Can't load or create resource file. {e.Message}");
                }
            }

            return XDocument.Load(fPath);
        }

        /// <summary>
        /// Dispose _lock
        /// </summary>
        ~XmlResourceReaderWriter()
        {
            if (_lock != null) _lock.Dispose();
        }
    }
}
