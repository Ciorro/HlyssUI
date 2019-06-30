using HlyssUI.Components;
using System;

namespace HlyssUI.Layout
{
    public class Container : Component
    {
        public enum LayoutType
        {
            Column, Row, ReversedColumn, ReversedRow, Wrap
        }

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
                NeedsRefresh = true;
            }
        }

        //TODO: Zostawic fill?
        public bool Fill = false;

        public Container(Gui gui) : base(gui)
        {
            Width = "100%";
            Height = "100%";
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

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
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
        }

        private void row()
        {
            int x = 0;

            foreach (var child in Children)
            {
                child.Left = $"{x}px";
                child.Top = "0px";
                x += child.MarginSize.X;

                if (Fill)
                {
                    child.Height = $"{Size.Y - child.Mt - child.Mb - Pt - Pb}px";
                }
            }
        }

        private void column()
        {
            int y = 0;

            foreach (var child in Children)
            {
                child.Left = "0px";
                child.Top = $"{y}px";
                y += child.MarginSize.Y;

                if (Fill)
                {
                    child.Width = $"{Size.X - child.Ml - child.Mr - Pl - Pr}px";
                }
            }
        }

        private void reversedRow()
        {
            int x = W - Pl - Pr;

            foreach (var child in Children)
            {
                x -= child.MarginSize.X;
                child.Left = $"{x}px";
                child.Top = "0px";

                if (Fill)
                {
                    child.Height = $"{Size.Y - child.Mt - child.Mb - Pt - Pb}px";
                }
            }
        }

        private void reversedColumn()
        {
            int y = H - Pt - Pb;

            foreach (var child in Children)
            {
                y -= child.MarginSize.Y;
                child.Left = "0px";
                child.Top = $"{y}px";

                if (Fill)
                {
                    child.Width = $"{Size.X - child.Ml - child.Mr - Pl - Pr}px";
                }
            }
        }

        private void wrap()
        {
            int x = 0, y = 0;
            int maxY = 0;

            foreach (var child in Children)
            {
                if(x + child.W > W - Pl - Pr)
                {
                    y += maxY;
                    x = 0;
                    maxY = 0;
                }

                child.Left = $"{x}px";
                child.Top = $"{y}px";
                x += child.MarginSize.X;

                if (child.MarginSize.Y > maxY)
                {
                    maxY = child.MarginSize.Y;
                }
            }
        }
    }
}
