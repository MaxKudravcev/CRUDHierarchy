using System.Collections.Generic;

namespace CRUDHierarchy
{
    /// <summary>
    /// Provides basic methods for working with files
    /// </summary>
    interface IFileService
    {
        List<T> Open<T> (string filename);
        void Save<T>(string filename, List<T> objects);
    }
}
