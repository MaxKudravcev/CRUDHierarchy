using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CRUDHierarchy
{
    [SerializationFormat("Binary (*.bin)| *.bin")]
    class BinaryFileService : IFileService
    {

        private BinaryFormatter formatter = new BinaryFormatter();

        public List<T> Open<T>(Stream fs)
        {
            List<T> objects = new List<T>();           
            objects = formatter.Deserialize(fs) as List<T>;
            
            return objects;
        }

        public void Save<T>(Stream fs, List<T> objects)
        {
                formatter.Serialize(fs, objects);
        }
    }
}
