using HlyssUI.Components;
using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout
{
    public class Box : LayoutComponent
    {
        private LayoutType _layout = LayoutType.Relative;

        public LayoutType Layout
        {
            get
            {
                return _layout;
            }
            set
            {
                _layout = value;
                ScheduleRefresh();
            }
        }

        public Box()
        {
            DisableClipping = true;
        }

        public override void RefreshLayout()
        {
            switch (Layout)
            {
                case LayoutType.Column:
                    column();
                    break;
                case LayoutType.Row:
                    row();
                    break;
                case LayoutType.ReversedColumn:
                    reversedColumn();
                    break;
                case LayoutType.ReversedRow:
                    reversedRow();
                    break;
                case LayoutType.Wrap:
                    Logger.Log("Box component does not support wrap layout type.");
                    break;
            }

            int maxX = 0, maxY = 0;

            foreach (var child in Children)
            {
                if (child.TransformChanged)
                    child.UpdateLocalTransform();

                if (child.TargetPosition.X + child.TargetSize.X + child.TargetMargins.Horizontal > maxX)
                    maxX = child.TargetPosition.X + child.TargetSize.X + child.TargetMargins.Horizontal;
                if (child.TargetPosition.Y + child.TargetSize.Y + child.TargetMargins.Vertical > maxY)
                    maxY = child.TargetPosition.Y + child.TargetSize.Y + child.TargetMargins.Vertical;
            }

            Width = $"{maxX + Pl + Pr}px";
            Height = $"{maxY + Pt + Pb}px";
        }

        private void row()
        {
            int x = 0;

            foreach (var child in Children)
            {
                child.Left = $"{x}px";
                child.Top = "0px";
                x += child.TargetMargins.Horizontal + child.TargetSize.X;
            }
        }

        private void column()
        {
            int y = 0;

            foreach (var child in Children)
            {
                child.Left = "0px";
                child.Top = $"{y}px";
                y += child.TargetMargins.Vertical + child.TargetSize.Y;
            }
        }

        private void reversedRow()
        {
            int x = 0;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Left = $"{x}px";
                Children[i].Top = "0px";
                x += Children[i].TargetMargins.Horizontal + Children[i].TargetSize.X;
            }
        }

        private void reversedColumn()
        {
            int y = 0;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Left = "0px";
                Children[i].Top = $"{y}px";
                y += Children[i].TargetMargins.Vertical + Children[i].TargetSize.Y;
            }
        }
    }
}
