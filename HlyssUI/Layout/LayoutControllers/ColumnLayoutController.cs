using HlyssUI.Components;
using SFML.System;

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

                if (child.PositionType == PositionType.Fixed || child.PositionType == PositionType.Absolute)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                child.TargetRelativePosition = new Vector2i(0, y);
                y += child.TargetMargins.Vertical + child.TargetSize.Y;

                if (child.PositionType == PositionType.Relative)
                {
                    child.TargetRelativePosition += child.TargetPosition;
                }

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

                if (child.PositionType == PositionType.Fixed || child.PositionType == PositionType.Absolute)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                y -= child.TargetMargins.Vertical + child.TargetSize.Y;
                child.TargetRelativePosition = new Vector2i(0, y);

                if (child.PositionType == PositionType.Relative)
                {
                    child.TargetRelativePosition += child.TargetPosition;
                }

                CompareSize(child);
            }
        }

        public override void ApplyContentCentering(Component component)
        {
            foreach (var child in component.Children)
            {
                switch (child.PositionType)
                {
                    case PositionType.Static:
                        child.TargetRelativePosition = new Vector2i((component.TargetSize.X - component.TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2, child.TargetRelativePosition.Y);
                        break;
                    case PositionType.Relative:
                        child.TargetRelativePosition = new Vector2i((component.TargetSize.X - component.TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2 + child.TargetPosition.X, child.TargetRelativePosition.Y);
                        break;
                }
            }
        }
    }
}
