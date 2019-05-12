using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Reflection;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    public class CultureLocalizer
    {
        private readonly IHtmlLocalizer _localizer;

        public CultureLocalizer(IHtmlLocalizerFactory factory, Type type = null)
        {
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type.Name, assemblyName.Name);
        }

        public LocalizedHtmlString Text(string key, params string[] args)
        {
            return args == null
                ? _localizer[key]
                : _localizer[key, args];
        }
    }
}
