using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HlyssUI.ResourceManagement.ResourceLoaders
{
    class FileLoader : ResourceLoader
    {
        public FileLoader() : base("file") { }

        public override byte[] Load(string uri)
        {
            if (!Path.IsPathFullyQualified(uri))
                uri = Path.Combine(ApplicationPath, uri);

            return File.ReadAllBytes(uri);
        }

        public override async Task<byte[]> LoadAsync(string uri)
        {
            if (!Path.IsPathFullyQualified(uri))
                uri = Path.Combine(ApplicationPath, uri);

            return await File.ReadAllBytesAsync(uri);
        }
    }
}
