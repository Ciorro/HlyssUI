using HlyssUI.Components;

namespace HlyssUI.Layout.LayoutControllers
{
    class ScrollLayoutController : LayoutController
    {
        public ScrollLayoutController() : base(LayoutType.Scroll) { }

        public override void ApplyLayout(Component component)
        {
            foreach (var child in component.Children)
            {
                child.Left = $"{-component.ScrollOffset.X}px";
                child.Top = $"{-component.ScrollOffset.Y}px";

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new ScrollLayoutController();
        }
    }
}
