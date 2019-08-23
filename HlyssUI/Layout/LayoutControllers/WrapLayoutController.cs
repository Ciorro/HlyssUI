﻿using System;
using System.Collections.Generic;
using System.Text;
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
            }
        }

        public override LayoutController Get()
        {
            return new WrapLayoutController();
        }
    }
}