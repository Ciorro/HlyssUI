using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HlyssUI.ResourceManagement.ResourceLoaders
{
    public class HttpLoader : ResourceLoader
    {
        public HttpLoader() : base("http") { }

        public override byte[] Load(string uri)
        {
            try
            {
                WebClient client = new WebClient();
                return client.DownloadData(uri.Insert(0, "http://"));
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public override async Task<byte[]> LoadAsync(string uri)
        {
            try
            {
                WebClient client = new WebClient();
                return await client.DownloadDataTaskAsync(uri.Insert(0, "http://"));
            }
            catch
            {
                return null;
            }
        }
    }
}
