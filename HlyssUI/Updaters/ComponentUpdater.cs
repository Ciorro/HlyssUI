using HlyssUI.Components;

namespace HlyssUI.Updaters
{
    class ComponentUpdater
    {
        public void Update(Component component)
        {
            //if (component.Enabled)
            {
                component.Update();
            }

            foreach (var child in component.Children)
            {
                Update(child);
            }
        }
    }
}
