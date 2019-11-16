﻿using HlyssUI.Components;
using HlyssUI.Layout.Positioning;
using SFML.System;

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
                if (!child.Visible)
                    continue;

                if(child.PositionType == PositionType.Fixed)
                {
                    child.TargetRelativePosition = child.TargetPosition;
                    continue;
                }

                if (x + child.TargetSize.X + child.TargetMargins.Horizontal > component.TargetSize.X - component.TargetPaddings.Horizontal)
                {
                    y += maxY;
                    x = 0;
                    maxY = 0;
                }

                child.TargetRelativePosition = new Vector2i(x, y);
                x += child.TargetMargins.Horizontal + child.TargetSize.X;

                if (child.TargetMargins.Vertical + child.TargetSize.Y > maxY)
                {
                    maxY = child.TargetMargins.Vertical + child.TargetSize.Y;
                }

                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new WrapLayoutController();
        }

        public override void ApplyContentCentering(Component component)
        {
            return;
        }
    }
}
