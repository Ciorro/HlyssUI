using System;
using SFML.Window;

namespace HlyssUI.Components
{
    class RootComponent : Component
    {
        public RootComponent()
        {
            CreateStyle();
        }

        private void CreateStyle()
        {
            DefaultStyle = Themes.Style.DefaultStyle;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Gui.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";

                UpdateLocalTransform();
            };

            Width = $"{Gui.Window.Size.X}px";
            Height = $"{Gui.Window.Size.Y}px";

            UpdateLocalTransform();
        }
    }
}
