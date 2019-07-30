using System;
using HlyssUI.Components;
using SFML.Graphics;

namespace HlyssUI.Graphics
{
    internal class Renderer
    {
        public void Render(Component component)
        {
            component.Gui.Window.SetView(component.Gui.Window.DefaultView);
            renderComponents(component);
        }

        private void renderComponents(Component component)
        {
            if (component.IsOnScreen)
            {
                if (!component.Visible)
                    return;

                View area = getNearestClipArea(component);

                if (area != null)
                    component.Gui.Window.SetView(area);

                component.Draw(component.Gui.Window);
                component.DrawDebug();

                component.Gui.Window.SetView(component.Gui.DefaultView);

                foreach (var child in component.Children)
                {
                    renderComponents(child);
                }
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
