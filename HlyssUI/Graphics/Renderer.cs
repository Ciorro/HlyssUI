using HlyssUI.Components;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Graphics
{
    internal class Renderer
    {
        private IntRect _windowArea;
        private View _defaultView;

        public void Render(Component component)
        {
            _windowArea = new IntRect(0, 0, (int)component.App.Window.Size.X, (int)component.App.Window.Size.Y);
            _defaultView = new View()
            {
                Center = (Vector2f)component.App.Window.Size / 2,
                Size = (Vector2f)component.App.Window.Size,
                Viewport = new FloatRect(0, 0, 1, 1)
            };

            component.App.Window.SetView(component.App.Window.DefaultView);
            RenderComponents(component);
        }

        private void RenderComponents(Component rootComponent)
        {
            foreach (var component in rootComponent.App.FlatComponentTree)
            {
                if (!component.IsOnScreen || !component.Visible || !component.Bounds.Intersects(_windowArea))
                    continue;

                View area = GetNearestClipArea(component);

                if (area != null && !component.OnTop)
                    component.App.Window.SetView(area);

                component.Draw(component.App.Window);
                component.DrawDebug();

                component.App.Window.SetView(_defaultView);
            }
        }

        private View GetNearestClipArea(Component component)
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