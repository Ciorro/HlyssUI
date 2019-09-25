using HlyssUI.Components;
using HlyssUI.Layout.LayoutControllers;
using HlyssUI.Utils;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        private bool _anyTransformChanged;

        public void Update(Component component)
        {
            CheckChanges(component);

            if (_anyTransformChanged)
            {
                Compose(component);
                Refresh(component);

                _anyTransformChanged = false;
            }
        }

        private void CheckChanges(Component component)
        {
            if (component.TransformChanged)
            {
                _anyTransformChanged = true;
            }

            foreach (var child in component.Children)
            {
                CheckChanges(child);
            }
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
