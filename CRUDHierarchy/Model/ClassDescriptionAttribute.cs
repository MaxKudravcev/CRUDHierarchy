namespace CRUDHierarchy
{
    /// <summary>
    /// Stores a human-readable name of a class or enum
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Enum, AllowMultiple = false)]
    
    public class ClassDescriptionAttribute : System.Attribute
    {
        public string Name { get; set; }

        public ClassDescriptionAttribute(string name)
        {
            this.Name = name;
        }
    }
}
