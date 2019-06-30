using HlyssUI.Components;
using System;

namespace HlyssUI.Layout
{
    public class Container : Component
    {
        public enum LayoutOrientation
        {
            Column, Row, ReversedColumn, ReversedRow
        }

        private LayoutOrientation _layout = LayoutOrientation.Column;

        public LayoutOrientation Layout
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
                case LayoutOrientation.Column:
                    column();
                    break;
                case LayoutOrientation.Row:
                    row();
                    break;
                case LayoutOrientation.ReversedColumn:
                    reversedColumn();
                    break;
                case LayoutOrientation.ReversedRow:
                    reversedRow();
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

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                x -= Children[i].MarginSize.X;
                Children[i].Left = $"{x}px";
                Children[i].Top = "0px";

                if (Fill)
                {
                    Children[i].Height = $"{Size.Y - Children[i].Mt - Children[i].Mb - Pt - Pb}px";
                }

            }
        }

        private void reversedColumn()
        {
            int y = H - Pt - Pb;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                y -= Children[i].MarginSize.Y;
                Children[i].Left = "0px";
                Children[i].Top = $"{y}px";

                if (Fill)
                {
                    Children[i].Width = $"{Size.X - Children[i].Ml - Children[i].Mr - Pl - Pr}px";
                }
            }
        }
    }
}
