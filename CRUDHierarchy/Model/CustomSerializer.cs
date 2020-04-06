using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CRUDHierarchy
{
    /* CUSTOM SERIALIZATION FORMAT (*.csf):
     * 
     * FIELD: <field>Name:Type:Value</field>
     * 
     * CLASS: <class>Type: fields... </class>
     * 
     * EXAMPLE: <class>Person:<field>Name:string:Vasya</field><field>Age:int:228</field></class>
     * 
     * IEnumerable: <array>Type:<element> element </element> ... </array>
     * 
     * EXAMPLE: <array>Person:<class>Person:<field>Name:string:Vasya</field><field>Age:int:228</field></class><class>Person:<field>Name:string:Vasya</field><field>Age:int:228</field></class></array>
     * 
     * 
     */
    class CustomSerializer
    {
        private Type type;
        private string result;

        public CustomSerializer(Type type)
        {
            this.type = type;
        }

        public void Serialize(Stream stream, object obj)
        {
            result = "";
            Serialize(obj);
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine(result);
            }
        }

        public object Deserialize(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadLine();
            }
            return Deserialize(result);
        }

        #region Serialization

        private void Serialize(object obj)
        {
            if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                SerializeCollection(obj);
            else Serializeobject(obj);
        }

        private void Serializeobject(object obj)
        {
            result += "<class>" + obj.GetType().Name;
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                result += $"<field>{field.Name}:{field.FieldType.AssemblyQualifiedName}:";
                if (field.FieldType.IsPrimitive || field.FieldType == typeof(string) || field.FieldType.IsEnum)
                    result += field.GetValue(obj).ToString();
                else
                    Serialize(field.GetValue(obj));
                result += "</field>";
            }
            result += "</class>";
        }

        private void SerializeCollection(object obj)
        {
            IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator();
            
            //todo: get underlying type from enumerator
            result += "<array>" + type.GetGenericArguments()[0].AssemblyQualifiedName;
            while(enumerator.MoveNext())
            {
                result += "<element>";
                Serialize(enumerator.Current);
                result += "</element>";
            }
            result += "</array>";
        }

        #endregion

        private object Deserialize(string str)
        {
            string tag = CutNextTag(ref str);
           

            switch (tag)
            {
                case "<array>":
                    {
                        return DeserializeCollection(ref str);
                    }
                default:
                    break;
            }
        }

        private object DeserializeCollection(ref string str)
        {
            string typeStr = result.Substring(0, result.IndexOf(':') - 1);
            result = result.Remove(0, typeStr.Length + 1);

            Type collectionElementsType = Assembly.GetExecutingAssembly().GetType(typeStr);
            IList objects = createList(collectionElementsType);

            string tag = CutNextTag(ref str);
            while (tag != "</array>")
            {
                string listElement = result.Substring(0, result.IndexOf("</element>") - 1);
                result = result.Remove(0, listElement.Length + "</element>".Length - 1);
                objects.Add(Deserialize(listElement));
            }
            return objects;
        }

        private IList createList(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private string CutNextTag(ref string str)
        {
            string tag = str.Substring(0, str.IndexOf('>'));
            str = str.Remove(0, tag.Length);
            return tag;
        }
    }
}
