using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CRUDHierarchy
{
    [SerializationFormat("Binary (*.bin)", "bin")]
    class BinarySerializer : ISerializer
    {
        private BinaryFormatter binaryFormatter = new BinaryFormatter();

        public T Deserialize<T>(Stream serializationStream)
        {
            return (T)binaryFormatter.Deserialize(serializationStream);
        }

        public void Serialize<T>(Stream serializationStream, T obj)
        {
            binaryFormatter.Serialize(serializationStream, obj);
        }
    }
}
