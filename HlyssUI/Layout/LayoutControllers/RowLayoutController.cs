using HlyssUI.Components;

namespace HlyssUI.Layout.LayoutControllers
{
    class RowLayoutController : LayoutController
    {
        public RowLayoutController() : base(LayoutType.Row) { }

        public override void ApplyLayout(Component component)
        {
            if (!component.ReversedHorizontal)
                Apply(component);
            else
                ApplyReversed(component);
        }

        public override LayoutController Get()
        {
            return new RowLayoutController();
        }

        private void Apply(Component component)
        {
            int x = 0;

            foreach (var child in component.Children)
            {
                child.Left = $"{x}px";
                child.Top = "0px";
                x += child.TargetMargins.Horizontal + child.TargetSize.X;

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        private void ApplyReversed(Component component)
        {
            int x = component.TargetSize.X - component.TargetPaddings.Horizontal;

            foreach (var child in component.Children)
            {
                x -= child.TargetMargins.Horizontal + child.TargetSize.X;
                child.Left = $"{x}px";
                child.Top = "0px";

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                child.Top = $"{(component.TargetSize.Y - component.TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2}px";
                child.UpdateLocalPosition();
            }
        }
    }
}
