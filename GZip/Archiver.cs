using System.IO;
using System.IO.Compression;
using PluginsSupport;

namespace GZip
{
    [PluginExtension("Gnu zipped file (plugin) | *.gz")]
    public class Archiver : IPlugin
    {
        public void Compress(Stream input, Stream output)
        {
            using (GZipStream stream = new GZipStream(output, CompressionMode.Compress))
            {
                input.CopyTo(stream);
            }
        }

        public void Decompress(Stream input, Stream output)
        {
            using (GZipStream stream = new GZipStream(input, CompressionMode.Decompress))
            {
                stream.CopyTo(output);
            }
        }
    }
}
