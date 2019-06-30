using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    class BaseComponent : Component
    {
        private const int DEFAULT_APP_MARGIN = 0;

        public BaseComponent(Gui gui) : base(gui)
        {
            gui.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";
            };

            Width = $"{gui.Window.Size.X - DEFAULT_APP_MARGIN * 2}px";
            Height = $"{gui.Window.Size.Y - DEFAULT_APP_MARGIN * 2}px";

            Margin = $"{DEFAULT_APP_MARGIN}px";

            DisableClipping = true;
        }
    }
}
