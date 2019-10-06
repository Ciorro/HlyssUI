using HlyssUI.Components;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Graphics
{
    internal class Renderer
    {
        private IntRect _windowArea;

        public void Render(Component component)
        {
            _windowArea = new IntRect(0, 0, (int)component.Gui.Window.Size.X, (int)component.Gui.Window.Size.Y);

            component.Gui.Window.SetView(component.Gui.Window.DefaultView);
            renderComponents(component);
        }

        private void renderComponents(Component component)
        {
            if (!component.IsOnScreen || !component.Visible || !component.Bounds.Intersects(_windowArea))
                return;

            View area = getNearestClipArea(component);

            if (area != null)
                component.Gui.Window.SetView(area);

            component.Draw(component.Gui.Window);
            component.DrawDebug();

            component.Gui.Window.SetView(new View()
            {
                Center = (Vector2f)component.Gui.Window.Size / 2,
                Size = (Vector2f)component.Gui.Window.Size,
                Viewport = new FloatRect(0, 0, 1, 1)
            });

            foreach (var child in component.Children)
            {
                renderComponents(child);
            }

        }

        private View getNearestClipArea(Component component)
        {
            while (true)
            {
                if (component.Parent != null)
                {
                    if (!component.Parent.DisableClipping)
                        return component.Parent.ClipArea.Area;

                    component = component.Parent;
                    continue;
                }

                return null;
            }
        }
    }
}