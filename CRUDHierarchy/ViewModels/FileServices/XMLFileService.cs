using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [SerializationFormat("XML (*.xml)| *.xml")]
    class XMLFileService : IFileService
    {
        public List<T> Open<T>(Stream fs)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(List<T>));
            List<T> objects = new List<T>();          
            objects = dataContractSerializer.ReadObject(fs) as List<T>;  
            
            return objects;
        }

        public void Save<T>(Stream fs, List<T> objects)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(List<T>));
            dataContractSerializer.WriteObject(fs, objects);
        }
    }
}
