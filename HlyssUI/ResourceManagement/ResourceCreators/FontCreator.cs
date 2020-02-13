using SFML.Graphics;

namespace HlyssUI.ResourceManagement.ResourceCreators
{
    public class FontCreator : ResourceCreator
    {
        public FontCreator() : base(typeof(Font)) { }

        public override object CreateResource(byte[] bytes)
        {
            return new Font(bytes);
        }
    }
}
