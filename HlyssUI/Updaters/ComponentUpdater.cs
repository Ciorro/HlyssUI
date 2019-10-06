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
                if (!child.Initialized)
                    InitializeComponent(component, child);

                Update(child);
            }
        }

        private static void InitializeComponent(Component component, Component child)
        {
            child.Gui = component.Gui;
            child.Parent = component;

            child.OnInitialized();
            child.ScheduleRefresh();
        }
    }
}
