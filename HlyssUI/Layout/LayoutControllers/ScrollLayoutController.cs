using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Layout.LayoutControllers
{
    class ScrollLayoutController : LayoutController
    {
        public ScrollLayoutController() : base(LayoutType.Scroll) { }

        public override void ApplyLayout(Component component)
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

                child.TargetRelativePosition = new Vector2i(-component.ScrollOffset.X, -component.ScrollOffset.Y); ;

                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new ScrollLayoutController();
        }

        public override void ApplyContentCentering(Component component)
        {
            return;
        }
    }
}
