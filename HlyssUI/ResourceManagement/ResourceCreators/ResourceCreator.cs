using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.ResourceManagement.ResourceCreators
{
    public abstract class ResourceCreator
    {
        public readonly Type ResourceType;

        public ResourceCreator(Type type)
        {
            ResourceType = type;
        }

        public abstract object CreateResource(byte[] bytes);
    }
}
