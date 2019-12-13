using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Layout.LayoutControllers
{
    class AbsoluteLayoutController : LayoutController
    {
        public AbsoluteLayoutController() : base(LayoutType.Absolute) { }

        public override void ApplyLayout(Component component)
        {
            if (component.ReversedHorizontal || component.ReversedVertical)
                ApplyReversed(component);
            else
                Apply(component);
        }

        private void ApplyReversed(Component component)
        {
            foreach (var child in component.Children)
            {
                if (!child.Visible)
                    continue;

                if (child.PositionType == PositionType.Fixed)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                if (component.ReversedHorizontal)
                {
                    int x = component.TargetSize.X - component.TargetPaddings.Horizontal - child.TargetMargins.Horizontal - child.TargetSize.X;
                    child.TargetRelativePosition = new Vector2i(x, child.TargetRelativePosition.Y);
                }

                if (component.ReversedVertical)
                {
                    int y = component.TargetSize.Y - component.TargetPaddings.Vertical - child.TargetMargins.Vertical - child.TargetSize.Y;
                    child.TargetRelativePosition = new Vector2i(child.TargetRelativePosition.X, y);
                }

                CompareSize(child);
            }
        }

        private void Apply(Component component)
        {
            foreach (var child in component.Children)
            {
                if (!child.Visible)
                    continue;

                if (child.PositionType == PositionType.Fixed)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                child.TargetRelativePosition = child.TargetPosition;
                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new AbsoluteLayoutController();
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                if (child.PositionType != PositionType.Fixed)
                {
                    child.TargetRelativePosition = new Vector2i((component.TargetSize.X - component.TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2, (component.TargetSize.Y - component.TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2);
                }
            }
        }
    }
}
