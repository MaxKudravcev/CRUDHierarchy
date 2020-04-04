using System.IO;

namespace CRUDHierarchy
{
    /// <summary>
    /// An interface for all available serializators
    /// </summary>
    interface ISerializer
    {
        T Deserialize<T> (Stream serializationStream);
        void Serialize<T> (Stream serializationStream, T obj);
    }
}
