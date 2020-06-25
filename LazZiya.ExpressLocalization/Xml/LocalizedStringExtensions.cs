using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Xml.Linq;

namespace LazZiya.ExpressLocalization.Xml
{
    internal static class XmlLocalizedStringExtensions
    {
        internal static XElement ToXElement(this LocalizedHtmlString str, bool isActive = false)
        {
            return new XElement("data", new XAttribute("isActive", isActive),
                                        new XElement("key", str.Name),
                                        new XElement("value", str.Value),
                                        new XElement("comment", "AUTO"));
        }

        internal static XElement ToXElement(this LocalizedString str, bool isActive = false)
        {
            return new XElement("data", new XAttribute("isActive", isActive),
                                        new XElement("key", str.Name),
                                        new XElement("value", str.Value),
                                        new XElement("comment", "AUTO"));
        }

        internal static bool WriteTo(this LocalizedHtmlString str, XDocument xDocument, string path)
        {
            try
            {
                lock (xDocument)
                {
                    xDocument.Root.Add(str.ToXElement());
                    xDocument.Save(path);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        internal static bool WriteTo(this LocalizedString str, XDocument xDocument, string path)
        {
            try
            {
                lock (xDocument)
                {
                    xDocument.Root.Add(str.ToXElement());
                    xDocument.Save(path);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
    }
}
