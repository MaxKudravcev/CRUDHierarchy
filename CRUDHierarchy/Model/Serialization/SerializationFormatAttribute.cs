using System;

namespace CRUDHierarchy
{ 
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    /// <summary>
    ///Stores format and description of ISerializator implementation class
    /// </summary>
    class SerializationFormatAttribute : Attribute
    {
        public string Description { get; set; }
        public string Extension { get; set; }

        public SerializationFormatAttribute(string description, string extension)
        {
            this.Description = description;
            this.Extension = extension;
        }
    }
}
