using SFML.Window;

namespace HlyssUI.Components
{
    public sealed class RootComponent : Component
    {
        public RootComponent(HlyssApp gui)
        {
            App = gui;

            App.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";

                UpdateLocalTransform();
            };

            Width = $"{App.Window.Size.X}px";
            Height = $"{App.Window.Size.Y}px";
            Layout = HlyssUI.Layout.LayoutType.Absolute;
            Name = "root";

            UpdateLocalTransform();
        }
    }
}
