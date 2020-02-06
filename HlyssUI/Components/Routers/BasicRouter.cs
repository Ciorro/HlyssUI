namespace HlyssUI.Components.Routers
{
    public class BasicRouter : Router
    {
        public override void Navigate(Component component)
        {
            if (Children.Count == 0)
            {
                component.Visible = true;
                Children.Add(component);
            }
            else
            {
                Children[0].Visible = false;
                Children[0] = component;
            }

            System.GC.Collect();
        }
    }
}
