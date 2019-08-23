using HlyssUI.Components;
using System;

namespace HlyssUI.Layout
{
    public class Container : LayoutComponent
    {
        private LayoutType _layout = LayoutType.Column;

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

        public bool CenterContent = false;

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
        }

        public override void RefreshLayout()
        {
            UpdateLocalTransform();

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
                    wrap();
                    break;
            }

            foreach (var child in Children)
            {
                if (child.TransformChanged)
                    child.UpdateLocalTransform();
            }
        }

        private void row()
        {
            int x = 0;

            foreach (var child in Children)
            {
                child.UpdateLocalTransform();

                child.Left = $"{x}px";
                child.Top = "0px";
                x += child.TargetMargins.Horizontal + child.TargetSize.X;

                if (CenterContent)
                {
                    child.Top = $"{(TargetSize.Y - TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2}px";
                }
            }
        }

        private void column()
        {
            int y = 0;

            foreach (var child in Children)
            {
                child.UpdateLocalTransform();

                child.Left = "0px";
                child.Top = $"{y}px";
                y += child.TargetMargins.Vertical + child.TargetSize.Y;

                if (CenterContent)
                {
                    child.Left = $"{(TargetSize.X - TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2}px";
                }
            }
        }

        private void reversedRow()
        {
            int x = TargetSize.X - TargetPaddings.Horizontal;

            foreach (var child in Children)
            {
                child.UpdateLocalTransform();

                x -= child.TargetMargins.Horizontal + child.TargetSize.X;
                child.Left = $"{x}px";
                child.Top = "0px";

                if (CenterContent)
                {
                    child.Top = $"{(TargetSize.Y - TargetPaddings.Vertical - child.H - child.Mt - child.Mb) / 2}px";
                }
            }
        }

        private void reversedColumn()
        {
            int y = TargetSize.Y - TargetPaddings.Vertical;

            foreach (var child in Children)
            {
                child.UpdateLocalTransform();

                y -= child.TargetMargins.Vertical + child.TargetSize.Y;
                child.Left = "0px";
                child.Top = $"{y}px";

                if (CenterContent)
                {
                    child.Left = $"{(TargetSize.X - TargetPaddings.Horizontal - child.W - child.Ml - child.Mr) / 2}px";
                }
            }
        }

        private void wrap()
        {
            int x = 0, y = 0;
            int maxY = 0;

            foreach (var child in Children)
            {
                child.UpdateLocalTransform();

                if(x + child.TargetSize.X + child.TargetMargins.Horizontal > TargetSize.X - TargetPaddings.Horizontal)
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
            }
        }
    }
}
