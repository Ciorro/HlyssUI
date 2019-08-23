using HlyssUI.Components;
using HlyssUI.Layout;
using HlyssUI.Layout.LayoutControllers;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        public void Update(Component component)
        {
            Compose(component);
            Refresh(component);
        }

        private void Compose(Component component)
        {
            component.UpdateLocalSize();
            component.UpdateLocalSpacing();

            foreach (var child in component.Children)
            {
                Compose(child);
            }

            LayoutController layout = LayoutResolver.GetLayout(component.Layout);

            System.Console.WriteLine(layout);

            layout.ApplyLayout(component);
            layout.ApplyAutosize(component);
        }

        private void Refresh(Component component)
        {
            component.ApplyTransform();

            foreach (var child in component.Children)
            {
                Refresh(child);
            }
        }
    }
}
