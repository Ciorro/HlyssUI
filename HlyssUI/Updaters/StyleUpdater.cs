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
            component.OnStyleChanged();

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }
        }
    }
}
