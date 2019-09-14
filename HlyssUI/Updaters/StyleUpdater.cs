using HlyssUI.Components;

namespace HlyssUI.Updaters
{
    class StyleUpdater
    {
        public void Update(Component baseComponent)
        {
            if (baseComponent.StyleChanged)
            {
                RefreshComponents(baseComponent);
                return;
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
            component.OnStyleChanged();
        }
    }
}
