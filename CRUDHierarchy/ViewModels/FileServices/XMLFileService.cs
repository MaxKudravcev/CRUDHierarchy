using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [SerializationFormat("XML (*.xml)| *.xml")]
    class XMLFileService : IFileService
    {
        public List<T> Open<T>(string filename)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(List<T>));
            List<T> objects = new List<T>();

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                objects = dataContractSerializer.ReadObject(fs) as List<T>;
            }
            return objects;
        }

        public void Save<T>(string filename, List<T> objects)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(List<T>));

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                dataContractSerializer.WriteObject(fs, objects);
            }
        }
    }
}
