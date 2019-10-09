namespace HlyssUI.Components.Routers
{
    public abstract class Router : Component
    {
        public Router()
        {
            Width = "100%";
            Height = "100%";
        }

        public abstract void Navigate(Component component);
    }
}
