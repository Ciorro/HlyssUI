namespace HlyssUI.Components.Routers
{
    public class BasicRouter : Router
    {
        public override void Navigate(Component component)
        {
            if (Children.Count == 0)
            {
                Children.Add(component);
            }
            else
            {
                Children[0] = component;
            }

            System.GC.Collect();
        }
    }
}
