using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HlyssUI.ResourceManagement.ResourceLoaders
{
    public abstract class ResourceLoader
    {
        protected static string ApplicationPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public readonly string Protocol;

        public ResourceLoader(string protocol)
        {
            Protocol = protocol;
        }

        public abstract byte[] Load(string uri);

        public virtual async Task<byte[]> LoadAsync(string uri) 
        {
            throw new NotImplementedException();
        }
    }
}
