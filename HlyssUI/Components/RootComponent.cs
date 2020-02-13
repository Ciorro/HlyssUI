using SFML.Window;

namespace HlyssUI.Components
{
    public sealed class RootComponent : Component
    {
        public RootComponent()
        {
            Layout = HlyssUI.Layout.LayoutType.Absolute;
            Name = "root";

            UpdateLocalTransform();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();

            Form.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";

                UpdateLocalTransform();
            };

            Width = $"{Form.Window.Size.X}px";
            Height = $"{Form.Window.Size.Y}px";
        }
    }
}
