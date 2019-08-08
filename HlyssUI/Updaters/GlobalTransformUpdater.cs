using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Updaters
{
    class GlobalTransformUpdater
    {
        public void Update(Component baseComponent)
        {
            Scan(baseComponent);

            //if (_needsRefresh)
            //{
            //    _needsRefresh = false;
            //    RefreshComponents(_scene.BaseNode);
            //}
        }

        private void Scan(Component baseComponent)
        {
            if (baseComponent.TransformChanged)
            {
                RefreshComponents(baseComponent);
                return;
            }

            foreach (var child in baseComponent.Children)
            {
                Scan(child);
            }
        }

        private void RefreshComponents(Component component)
        {
            //if(!(component is Box))
            refresh(component);

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }

            //if ((component is Box))
                //refresh(component);
        }

        private void refresh(Component component)
        {
            Vector2i parentPos = component.Parent != null ? component.Parent.GlobalPosition : new Vector2i();
            Vector2i parentPad = component.Parent != null ? component.Parent.Paddings.TopLeft : new Vector2i();

            component.GlobalPosition = component.TargetPosition + component.Margins.TopLeft + parentPad + parentPos;

            if (component.IsOnScreen)
                component.OnRefresh();
            else
                component.TransformChanged = false;
        }
    }
}
