using HlyssUI.Components;

namespace HlyssUI.Updaters
{
    class ComponentUpdater
    {
        public void Update(Component component)
        {
            component.Update();
            
            foreach (var child in component.Children)
            {
                if (child.Gui == null)
                    child.Gui = component.Gui;

                Update(child);
            }
        }
    }
}
