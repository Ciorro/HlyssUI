using HlyssUI.Components;
using HlyssUI.Layout.Positioning;
using SFML.System;

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
                if (!child.Visible)
                    continue;

                if (child.PositionType == PositionType.Fixed)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                child.TargetRelativePosition = new Vector2i(x, 0);
                x += child.TargetMargins.Horizontal + child.TargetSize.X;

                CompareSize(child);
            }
        }

        private void ApplyReversed(Component component)
        {
            int x = component.TargetSize.X - component.TargetPaddings.Horizontal;

            foreach (var child in component.Children)
            {
                if (!child.Visible)
                    continue;

                if (child.PositionType == PositionType.Fixed)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                x -= child.TargetMargins.Horizontal + child.TargetSize.X;
                child.TargetRelativePosition = new Vector2i(x, 0);

                CompareSize(child);
            }
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                if (child.PositionType != PositionType.Fixed)
                {
                    child.TargetRelativePosition = new Vector2i(child.TargetRelativePosition.X, (component.TargetSize.Y - component.TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2);
                }
            }
        }
    }
}
