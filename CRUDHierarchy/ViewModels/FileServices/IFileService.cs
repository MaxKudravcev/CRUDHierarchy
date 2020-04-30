using System.Collections.Generic;
using System.IO;

namespace CRUDHierarchy
{
    /// <summary>
    /// Provides basic methods for working with files
    /// </summary>
    interface IFileService
    {
        List<T> Open<T> (Stream fs);
        void Save<T>(Stream fs, List<T> objects);
    }
}
