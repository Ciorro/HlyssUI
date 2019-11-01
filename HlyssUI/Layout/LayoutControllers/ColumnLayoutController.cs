using HlyssUI.Components;

namespace HlyssUI.Layout.LayoutControllers
{
    class ColumnLayoutController : LayoutController
    {
        public ColumnLayoutController() : base(LayoutType.Column) { }

        public override void ApplyLayout(Component component)
        {
            if (!component.ReversedVertical)
                Apply(component);
            else
                ApplyReversed(component);
        }

        public override LayoutController Get()
        {
            return new ColumnLayoutController();
        }

        private void Apply(Component component)
        {
            int y = 0;

            foreach (var child in component.Children)
            {
                if (!child.Visible)
                    continue;

                if (child.OnTop)
                {
                    child.UpdateLocalPosition();
                    continue;
                }

                child.Left = "0px";
                child.Top = $"{y}px";
                y += child.TargetMargins.Vertical + child.TargetSize.Y;

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        private void ApplyReversed(Component component)
        {
            int y = component.TargetSize.Y - component.TargetPaddings.Vertical;

            foreach (var child in component.Children)
            {
                if (!child.Visible)
                    continue;

                if (child.OnTop)
                {
                    child.UpdateLocalPosition();
                    continue;
                }

                y -= child.TargetMargins.Vertical + child.TargetSize.Y;
                child.Left = "0px";
                child.Top = $"{y}px";

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                if (!child.OnTop)
                {
                    child.Left = $"{(component.TargetSize.X - component.TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2}px";
                    child.UpdateLocalPosition();
                }
            }
        }
    }
}
