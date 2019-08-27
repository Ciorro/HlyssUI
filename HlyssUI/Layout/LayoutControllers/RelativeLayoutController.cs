﻿using System;
using System.Collections.Generic;
using System.Text;
using HlyssUI.Components;

namespace HlyssUI.Layout.LayoutControllers
{
    class RelativeLayoutController : LayoutController
    {
        public RelativeLayoutController() : base(LayoutType.Relative) { }

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
                if (component.ReversedHorizontal)
                {
                    int x = component.TargetSize.X - component.TargetPaddings.Horizontal - child.TargetMargins.Horizontal - child.TargetSize.X;
                    child.Left = $"{x}px";
                }

                if (component.ReversedVertical)
                {
                    int y = component.TargetSize.Y - component.TargetPaddings.Vertical - child.TargetMargins.Vertical - child.TargetSize.Y;
                    child.Top = $"{y}px";
                }

                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        private void Apply(Component component)
        {
            foreach (var child in component.Children)
            {
                child.UpdateLocalPosition();
                CompareSize(child);
            }
        }

        public override LayoutController Get()
        {
            return new RelativeLayoutController();
        }
    }
}