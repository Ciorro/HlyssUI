using HlyssUI.Components;

namespace HlyssUI.Updaters
{
    class ComponentUpdater
    {
        public void Update(Component component)
        {
            if (!component.Visible)
                return;

            component.Update();

            foreach (var child in component.Children)
            {
                if (!child.IsInitialized)
                    InitializeComponent(component, child);

                Update(child);
            }
        }

        private static void InitializeComponent(Component component, Component child)
        {
            child.App = component.App;
            child.Parent = component;

            child.OnInitialized();
            child.ScheduleRefresh();
        }
    }
}
