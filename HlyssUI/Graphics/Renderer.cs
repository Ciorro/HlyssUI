using HlyssUI.Components;

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

                if (component.Parent != null && !component.Parent.DisableClipping)
                    component.Gui.Window.SetView(component.Parent.ClipArea.Area);

                component.Draw(component.Gui.Window);
                component.DrawDebug();

                component.Gui.Window.SetView(component.Gui.DefaultView);

                foreach (var child in component.Children)
                {
                    renderComponents(child);
                }
            }
        }
    }
}
