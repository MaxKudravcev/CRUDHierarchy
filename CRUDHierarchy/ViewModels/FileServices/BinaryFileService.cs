using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CRUDHierarchy
{
    [SerializationFormat("Binary (*.bin)| *.bin")]
    class BinaryFileService : IFileService
    {

        private BinaryFormatter formatter = new BinaryFormatter();

        public List<T> Open<T>(string filename)
        {
            List<T> objects = new List<T>();
            
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                objects = formatter.Deserialize(fs) as List<T>;
            }
            return objects;
        }

        public void Save<T>(string filename, List<T> objects)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(fs, objects);
            }
        }
    }
}
