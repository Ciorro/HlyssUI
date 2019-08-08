using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Updaters
{
    class GlobalTransformUpdater
    {
        public void Update(Component baseComponent)
        {
            if (baseComponent.TransformChanged)
            {
                RefreshComponents(baseComponent);
            }

            foreach (var child in baseComponent.Children)
            {
                Update(child);
            }
        }

        private void RefreshComponents(Component component)
        {
            refresh(component);

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }
        }

        private void refresh(Component component)
        {
            if (component.IsOnScreen)
                component.ApplyTransform();
            else
                component.TransformChanged = false;
        }
    }
}
