using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [SerializationFormat("Custom serialization format (*.csf)| *.csf")]
    class CustomFileService : IFileService
    {
        public List<T> Open<T>(string filename)
        {
            CustomSerializer customSerializer = new CustomSerializer(typeof(List<T>));
            List<T> objects = new List<T>();

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                objects = customSerializer.Deserialize(fs) as List<T>;
            }

            return objects;
        }

        public void Save<T>(string filename, List<T> objects)
        {
            CustomSerializer customSerializer = new CustomSerializer(typeof(List<T>));

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                customSerializer.Serialize(fs, objects);
            }
        }
    }
}
