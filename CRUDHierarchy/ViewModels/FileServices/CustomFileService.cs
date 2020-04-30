using System.Collections.Generic;
using System.IO;

namespace CRUDHierarchy
{
    [SerializationFormat("Custom serialization format (*.csf)| *.csf")]
    class CustomFileService : IFileService
    {
        public List<T> Open<T>(Stream fs)
        {
            CustomSerializer customSerializer = new CustomSerializer(typeof(List<T>));
            List<T> objects = new List<T>();
            objects = customSerializer.Deserialize(fs) as List<T>;
            
            return objects;
        }

        public void Save<T>(Stream fs, List<T> objects)
        {
            CustomSerializer customSerializer = new CustomSerializer(typeof(List<T>));
            customSerializer.Serialize(fs, objects);        
        }
    }
}
