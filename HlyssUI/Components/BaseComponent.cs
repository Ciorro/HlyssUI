using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    class BaseComponent : Component
    {
        private const int DEFAULT_APP_MARGIN = 0;

        public BaseComponent(GuiScene scene) : base(scene)
        {
            scene.Gui.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";
            };

            Width = $"{scene.Gui.Window.Size.X - DEFAULT_APP_MARGIN * 2}px";
            Height = $"{scene.Gui.Window.Size.Y - DEFAULT_APP_MARGIN * 2}px";

            Margin = $"{DEFAULT_APP_MARGIN}px";

            DisableClipping = true;
        }
    }
}
