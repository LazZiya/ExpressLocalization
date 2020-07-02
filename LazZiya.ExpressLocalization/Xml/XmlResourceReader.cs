using LazZiya.ExpressLocalization.Common;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.Xml
{
    /// <summary>
    /// Generic resource manager
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class XmlResourceReader<TResource> : XmlResourceReader, IExpressResourceReader<TResource>
        where TResource : IXLResource
    {
        /// <summary>
        /// Initialize a new instance of <see cref="XmlResourceReader{TResource}"/>
        /// </summary>
        public XmlResourceReader(IOptions<ExpressLocalizationOptions> options)
            : base(typeof(TResource), options.Value.ResourcesPath)
        {

        }
    }

    /// <summary>
    /// For reading localized values from .resx resources
    /// </summary>
    public class XmlResourceReader : IExpressResourceReader
    {
        private readonly string _path;

        /// <summary>
        /// Initialzie a new instance of <see cref="XmlResourceReader"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="location"></param>
        public XmlResourceReader(Type type, string location)
        {
            _path = $".\\{location}\\{type.Name}.{{0}}.xml";
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

            try
            {
                var elmnt = _doc.Root.Descendants("data").FirstOrDefault(x => x.Element("key").Value.Equals(name, StringComparison.OrdinalIgnoreCase));
                value = elmnt?.Element("value").Value;
            }
            catch
            {
                value = null;
            }
            finally
            {
                _doc = null;
            }

            return value != null;
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
                    File.Copy(".\\Xml\\XmlTemplate.xml", fPath);
                }
                catch (Exception e)
                {
                    throw new FileLoadException($"Can't load or create resource file. {e.Message}");
                }
            }

            return XDocument.Load(fPath);
        }
    }
}
