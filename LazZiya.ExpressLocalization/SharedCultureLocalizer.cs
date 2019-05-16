using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Reflection;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Access shared localization resources under folder
    /// </summary>
    public class SharedCultureLocalizer
    {
        private readonly IHtmlLocalizer _localizer;

        public SharedCultureLocalizer(IHtmlLocalizerFactory factory, Type type = null)
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
