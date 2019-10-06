using System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class RootComponent : Component
    {
        public RootComponent(HlyssApp gui)
        {
            Gui = gui;

            CreateStyle();

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

        private void CreateStyle()
        {
            DefaultStyle = Themes.Style.DefaultStyle;
            DisabledStyle = Themes.Style.DisabledStyle;
        }
    }
}
