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
                if (!child.IsInitialized)
                    InitializeComponent(component, child);

                Update(child);
            }
        }

        private static void InitializeComponent(Component component, Component child)
        {
            child.Form = component.Form;
            child.Parent = component;

            child.OnInitialized();
            child.ScheduleRefresh();
        }
    }
}
