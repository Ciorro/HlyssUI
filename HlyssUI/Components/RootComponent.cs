using SFML.Window;

namespace HlyssUI.Components
{
    class RootComponent : Component
    {
        private const int DEFAULT_APP_MARGIN = 0;

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Gui.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";

                UpdateLocalTransform();
            };

            Width = $"{Gui.Window.Size.X - DEFAULT_APP_MARGIN * 2}px";
            Height = $"{Gui.Window.Size.Y - DEFAULT_APP_MARGIN * 2}px";

            Margin = $"{DEFAULT_APP_MARGIN}px";

            UpdateLocalTransform();
            DisableClipping = true;
        }
    }
}
