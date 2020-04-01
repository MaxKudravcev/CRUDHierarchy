using System.Reflection;

namespace CRUDHierarchy
{
    /// <summary>
    /// A class for storing a field and its value
    /// </summary>
    public class Field
    {
        public FieldInfo FieldInfo { get; set; }
        public object Value { get; set; }

        public Field(FieldInfo value)
        {
            FieldInfo = value;
            Value = FieldInfo.FieldType == typeof(bool) ? false as object : null; //For correct work of CheckBoxes
        }
    }
}
