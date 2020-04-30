using System;

namespace PluginsSupport
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    /// <summary>
    ///Stores format and description of archiver plugin
    /// </summary>
    public class PluginExtensionAttribute : Attribute
    {
        public string FilterString { get; set; }
        public PluginExtensionAttribute(string dialogFilter)
        {
            this.FilterString = dialogFilter;
        }
    }
}
