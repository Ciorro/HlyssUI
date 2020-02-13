using HlyssUI.ResourceManagement.ResourceCreators;
using HlyssUI.ResourceManagement.ResourceLoaders;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HlyssUI.ResourceManagement
{
    public static class ResourceManager
    {
        private static Dictionary<string, object> _resources = new Dictionary<string, object>();

        private static Dictionary<string, ResourceLoader> _loaders = new Dictionary<string, ResourceLoader>()
        {
            {"file", new FileLoader() },
            {"http", new HttpLoader() }
        };

        private static Dictionary<Type, ResourceCreator> _creators = new Dictionary<Type, ResourceCreator>()
        {
            { typeof(Texture), new TextureCreator()},
            { typeof(Font), new FontCreator()},
        };

        public static void SetResource(string name, object resource, bool overrideResource = false)
        {
            if (!_resources.ContainsKey(name))
                _resources.Add(name, resource);
            else if (overrideResource)
                _resources[name] = resource;
        }

        public static void AddLoader(ResourceLoader loader)
        {
            if (!_loaders.ContainsKey(loader.Protocol))
                _loaders.Add(loader.Protocol, loader);
        }

        public static void AddCreator(ResourceCreator creator)
        {
            if (!_creators.ContainsKey(creator.ResourceType))
                _creators.Add(creator.ResourceType, creator);
        }

        public static T Get<T>(string uri)
        {
            if (!_resources.ContainsKey(uri))
                Load<T>(uri);

            return (T)_resources[uri];
        }

        public static async Task<T> GetAsync<T>(string uri)
        {
            if (!_resources.ContainsKey(uri))
                await LoadAsync<T>(uri);

            return (T)_resources[uri];
        }

        private static void Load<T>(string uriStr)
        {
            Uri uri;

            if (Uri.TryCreate(uriStr, UriKind.Absolute, out uri))
            {
                Console.WriteLine($"Loading resource using {uri.Scheme} protocol.");
                _resources.Add(uriStr, _creators[typeof(T)].CreateResource(_loaders[uri.Scheme].Load(uriStr.Remove(0, uri.Scheme.Length + 3))));
            }
            else
            {
                Console.WriteLine($"Loading resource using file protocol.");
                _resources.Add(uriStr, _creators[typeof(T)].CreateResource(_loaders["file"].Load(uriStr)));
            }
        }

        private static async Task LoadAsync<T>(string uriStr)
        {
            Uri uri;

            if (Uri.TryCreate(uriStr, UriKind.Absolute, out uri))
            {
                Console.WriteLine($"Loading resource using {uri.Scheme} protocol.");
                _resources.Add(uriStr, _creators[typeof(T)].CreateResource(await _loaders[uri.Scheme].LoadAsync(uriStr.Remove(0, uri.Scheme.Length + 3))));
            }
            else
            {
                Console.WriteLine($"Loading resource using file protocol.");
                _resources.Add(uriStr, _creators[typeof(T)].CreateResource(await _loaders["file"].LoadAsync(uriStr)));
            }
        }
    }
}
