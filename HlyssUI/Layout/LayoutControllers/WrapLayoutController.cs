using HlyssUI.Components;

namespace HlyssUI.Layout.LayoutControllers
{
    class WrapLayoutController : LayoutController
    {
        public WrapLayoutController() : base(LayoutType.Wrap) { }

        public override void ApplyLayout(Component component)
        {
            int x = 0, y = 0;
            int maxY = 0;

            foreach (var child in component.Children)
            {
                if (x + child.TargetSize.X + child.TargetMargins.Horizontal > component.TargetSize.X - component.TargetPaddings.Horizontal)
                {
                    y += maxY;
                    x = 0;
                    maxY = 0;
                }

                child.Left = $"{x}px";
                child.Top = $"{y}px";
                x += child.TargetMargins.Horizontal + child.TargetSize.X;

                if (child.TargetMargins.Vertical + child.TargetSize.Y > maxY)
                {
                    maxY = child.TargetMargins.Vertical + child.TargetSize.Y;
                }

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new WrapLayoutController();
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                //child.Top = $"{(component.TargetSize.Y - component.TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2}px";
                //child.UpdateLocalPosition();
            }
        }
    }
}
