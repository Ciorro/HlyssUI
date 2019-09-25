namespace HlyssUI.Components.Overlays
{
    class Overlay : GuiScene
    {
        public Overlay(Gui gui) : base(gui)
        {
        }

        public void Show()
        {
            Gui.Navigator.PushOverlay(this);
            OnShown();
        }

        public void Hide()
        {
            Gui.Navigator.PopOverlay();
            OnHidden();
        }

        public virtual void OnShown() { }
        public virtual void OnHidden() { }
    }
}
