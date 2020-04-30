using System.IO;
using System.IO.Compression;
using PluginsSupport;

namespace Zip
{
    [PluginExtension("Zip archive (plugin) | *.zip")]
    public class Archiver : IPlugin
    {
        public void Compress(Stream input, Stream output)
        {
            using (ZipArchive archive = new ZipArchive(output, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entry = archive.CreateEntry("entry.bin");
                input.CopyTo(entry.Open());
            }
        }

        public void Decompress(Stream input, Stream output)
        {
            using (ZipArchive archive = new ZipArchive(input, ZipArchiveMode.Read))
            {
                ZipArchiveEntry entry = archive.GetEntry("entry.bin");
                entry.Open().CopyTo(output);
            }
        }
    }
}
