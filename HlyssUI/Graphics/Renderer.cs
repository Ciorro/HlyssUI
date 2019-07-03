using HlyssUI.Components;

namespace HlyssUI.Graphics
{
    internal class Renderer
    {
        public void Render(Component component)
        {
            renderCommonComponents(component, false);
            renderOverlapingComponents(component);
        }

        private void renderCommonComponents(Component component, bool renderOverlayingComponents)
        {
            if (!component.Visible || (component.IsOverlay && !renderOverlayingComponents))
                return;

            if (component.Parent != null && !component.Parent.DisableClipping)
                component.Gui.Window.SetView(component.Parent.ClipArea.Area);

            if(component.Parent != null && component.Parent.Parent != null && component.Parent.DisableClipping)
                component.Gui.Window.SetView(component.Parent.Parent.ClipArea.Area);

            component.Draw();
            component.DrawDebug();

            component.Gui.Window.SetView(component.Gui.DefaultView);

            foreach (var child in component.Children)
            {
                renderCommonComponents(child, renderOverlayingComponents);
            }
        }

        private void renderOverlapingComponents(Component component)
        {
            if (!component.Visible)
                return;

            if (component.IsOverlay)
                renderCommonComponents(component, true);

            foreach (var child in component.Children)
            {
                renderOverlapingComponents(child);
            }
        }
    }
}
