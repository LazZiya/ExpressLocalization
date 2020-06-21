using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.Xml
{
    internal class XmlLocalizerHelper
    {
        /// <summary>
        /// Helper method to create full path for current culture xml resource file
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        internal static string XmlDocumentFullPath(string baseName, string location)
        {
            return Path.Combine(location, $"{baseName}.{CultureInfo.CurrentCulture.Name}.xml");
        }

        /// <summary>
        /// Helper method to create Xml resource file
        /// </summary>
        /// <param name="fPath"></param>
        /// <returns></returns>
        internal static XDocument GetXmlDocument(string fPath)
        {
            if (!File.Exists(fPath))
            {
                try
                {
                    // Create a copy of the template xml resource
                    var path = typeof(XmlTemplate).Assembly.Location;
                    var folder = path.Substring(0, path.LastIndexOf('\\'));
                    File.Copy($"{folder}\\Xml\\XmlTemplate.xml", fPath);
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
