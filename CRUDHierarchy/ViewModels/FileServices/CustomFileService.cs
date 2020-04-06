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
            throw new System.NotImplementedException();
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
