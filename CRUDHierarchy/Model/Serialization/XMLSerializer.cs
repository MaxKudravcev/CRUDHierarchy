using System.IO;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    class XMLSerializer : ISerializer
    {
        public T Deserialize<T>(Stream serializationStream)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)ser.ReadObject(serializationStream);
        }

        public void Serialize<T>(Stream serializationStream, T obj)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(serializationStream, obj);
        }
    }
}
