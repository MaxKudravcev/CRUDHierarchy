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
     * EXAMPLE: <class>Person<field>Name:string:Vasya</field><field>Age:int:228</field></class>
     * 
     * IEnumerable: <array>Type<element> element </element> ... </array>
     * 
     * EXAMPLE: <array>Person<class>Person<field>Name:string:Vasya</field><field>Age:int:228</field></class><class>Person<field>Name:string:Vasya</field><field>Age:int:228</field></class></array>
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
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(result);
            sw.Flush();
        }

        public object Deserialize(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);            
            result = sr.ReadLine();    
            
            return Deserialize(ref result);
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
            result += "<class>" + obj.GetType().FullName;
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                result += $"<field>{field.Name}:{field.FieldType.FullName}:";
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
            result += "<array>" + type.GetGenericArguments()[0].FullName;
            while(enumerator.MoveNext())
            {
                result += "<element>";
                Serialize(enumerator.Current);
                result += "</element>";
            }
            result += "</array>";
        }

        #endregion

        private object Deserialize(ref string str)
        {
            string tag = CutNextTag(ref str);

            return tag == "<array>" ? DeserializeCollection(ref str) : DeserializeObject(ref str);            
        }

        private object DeserializeCollection(ref string str)
        {
            string typeStr = str.Substring(0, str.IndexOf('<'));
            str = str.Remove(0, typeStr.Length);

            Type collectionElementsType = Assembly.GetExecutingAssembly().GetType(typeStr);
            IList objects = createList(collectionElementsType);

            string tag = CutNextTag(ref str);
            while (tag != "</array>")
            {
                string listElement = str.Substring(0, str.IndexOf("</element>"));
                str = str.Remove(0, listElement.Length + "</element>".Length);
                objects.Add(Deserialize(ref listElement));
                tag = CutNextTag(ref str);
            }
            return objects;
        }

        private object DeserializeObject(ref string str)
        {
            string typeStr = str.Substring(0, str.IndexOf('<'));
            str = str.Remove(0, typeStr.Length);

            Type objectType = Assembly.GetExecutingAssembly().GetType(typeStr);
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); 
            object obj = Activator.CreateInstance(objectType);

            string tag = CutNextTag(ref str);
            while (tag != "</class>")
            {
                string name = str.Substring(0, str.IndexOf(':'));
                str = str.Remove(0, name.Length + 1);
                string fieldTypeStr = str.Substring(0, str.IndexOf(':'));
                str = str.Remove(0, fieldTypeStr.Length + 1);

                //todo: Maybe come up with faster way or get rid of serializing field types at all 
                Type fieldType = Assembly.GetExecutingAssembly().GetType(fieldTypeStr) ?? fields.Single(field => field.Name == name).FieldType;

                if(fieldType.IsEnum || fieldType.IsPrimitive || fieldType == typeof(string))
                {
                    object fieldValue = fieldType.IsEnum ? Enum.Parse(fieldType, str.Substring(0, str.IndexOf('<'))) : Convert.ChangeType(str.Substring(0, str.IndexOf('<')), fieldType);
                    str = str.Remove(0, str.IndexOf('<'));

                    fields.Single(field => field.Name == name).SetValue(obj, fieldValue);
                }
                else
                {
                    fields.Single(field => field.Name == name).SetValue(obj, Deserialize(ref str));
                }
                CutNextTag(ref str); //Cut a </field> tag
                tag = CutNextTag(ref str);
            }
            return obj;
        }

        private IList createList(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private string CutNextTag(ref string str)
        {
            string tag = str.Substring(0, str.IndexOf('>') + 1);
            str = str.Remove(0, tag.Length);
            return tag;
        }
    }
}
