using System;

namespace CRUDHierarchy
{ 
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    /// <summary>
    ///Stores format and description of ISerializator implementation class
    /// </summary>
    class SerializationFormatAttribute : Attribute
    {
        public string FilterString { get; set; }
        public SerializationFormatAttribute(string dialogFilter)
        {
            this.FilterString = dialogFilter;
        }
    }
}
