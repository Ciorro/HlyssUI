using HlyssUI.Components;
using HlyssUI.Layout;
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
            _windowArea = new IntRect(0, 0, (int)component.Form.Window.Size.X, (int)component.Form.Window.Size.Y);
            _defaultView = new View()
            {
                Center = (Vector2f)component.Form.Window.Size / 2,
                Size = (Vector2f)component.Form.Window.Size,
                Viewport = new FloatRect(0, 0, 1, 1)
            };

            component.Form.Window.SetView(component.Form.Window.DefaultView);
            RenderComponents(component);
        }

        private void RenderComponents(Component rootComponent)
        {
            foreach (var component in rootComponent.Form.FlatComponentTree)
            {
                if (!component.IsOnScreen || !component.Visible || !component.Bounds.Intersects(_windowArea))
                    continue;

                View area = GetNearestClipArea(component);

                if (area != null && component.PositionType != PositionType.Fixed)
                    component.Form.Window.SetView(area);

                component.Draw(component.Form.Window);
                component.DrawDebug();

                component.Form.Window.SetView(_defaultView);
            }
        }

        private View GetNearestClipArea(Component component)
        {
            while (true)
            {
                if (component.Parent != null)
                {
                    if (component.Parent.Overflow != OverflowType.Visible)
                        return component.Parent.ClipArea.Area;

                    component = component.Parent;
                    continue;
                }

                return null;
            }
        }
    }
}