﻿using HlyssUI.Components;

namespace HlyssUI.Updaters
{
    class StyleUpdater
    {
        public void Update(Component baseComponent)
        {
            Scan(baseComponent);
        }

        private void Scan(Component baseComponent)
        {
            if (baseComponent.Style.NeedsRefresh)
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
            refresh(component);

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }
        }

        private void refresh(Component component)
        {
            if (component.Parent != null && component.Parent.CascadeStyle)
                component.Style = component.Parent.Style;

            component.OnStyleChanged();
        }
    }
}
