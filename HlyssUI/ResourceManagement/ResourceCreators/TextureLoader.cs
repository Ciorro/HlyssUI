using SFML.Graphics;

namespace HlyssUI.ResourceManagement.ResourceCreators
{
    public class TextureCreator : ResourceCreator
    {
        public TextureCreator() : base(typeof(Texture)) { }

        public override object CreateResource(byte[] bytes)
        {
            if (bytes != null)
                return new Texture(bytes);
            else
                return null;
        }
    }
}
