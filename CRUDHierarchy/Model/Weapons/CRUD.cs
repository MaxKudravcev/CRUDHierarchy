using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [DataContract]
    [KnownType("GetDerivedTypes")]
    [Serializable]
    /// <summary>
    /// The main class in a Weapon Hierarchy.
    /// Contains methods for creating and updating instances
    /// </summary>
    public abstract class CRUD
    {
        /// <summary>
        /// Initializes all fields of the instance that was created via Activator
        /// </summary>
        /// <param name="values">A list of values to initialize fields of created instance</param>
        public void Create(List<object> values)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            { 
                object assignValue = (fields[i].FieldType.IsSubclassOf(typeof(CRUD)) || fields[i].FieldType.IsEnum) ? values[i] : Convert.ChangeType(values[i], fields[i].FieldType);

                fields[i].SetValue(this, assignValue);
            }
        }

        /// <summary>
        /// Returns an array with values of all instance's fields
        /// </summary>
        /// <returns>Array of field values as objects</returns>
        public object[] Read()
        {
            return this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Select(field => field.GetValue(this)).ToArray();
        }

        public abstract string GetName();

        private static Type[] GetDerivedTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(CRUD))).ToArray();
        }
    }
}
