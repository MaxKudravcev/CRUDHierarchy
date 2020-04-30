using System.IO;

namespace PluginsSupport
{
    public interface IPlugin
    {
        void Compress(Stream input, Stream output);
        void Decompress(Stream input, Stream output);
    }
}
