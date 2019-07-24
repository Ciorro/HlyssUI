using HlyssUI.Components;
using HlyssUI.Utils;

namespace HlyssUI.Layout
{
    public class Box : Component
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
                NeedsRefresh = true;
            }
        }

        public Box(Gui gui) : base(gui)
        {
            DisableClipping = true;
        }

        public override void Update()
        {
            base.Update();

            foreach (var child in Children)
            {
                if (child.NeedsRefresh)
                {
                    this.ForceRefresh();
                    Parent.ScheduleRefresh();
                    break;
                }
            }
        }

        public override void OnRefresh()
        {
            refreshLayout();

            Width = $"0px";
            Height = $"0px";

            int maxX = 0, maxY = 0;

            foreach (var child in Children)
            {
                if (child.X + child.W + child.Mr + child.Ml > maxX)
                    maxX = child.X + child.W + child.Mr + child.Ml;
                if (child.Y + child.H + child.Mb + child.Mt > maxY)
                    maxY = child.Y + child.H + child.Mb + child.Mt;
            }

            Width = $"{maxX + Pl + Pr}px";
            Height = $"{maxY + Pt + Pb}px";

            base.OnRefresh();
        }

        private void refreshLayout()
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
        }

        private void row()
        {
            int x = 0;

            foreach (var child in Children)
            {
                child.Left = $"{x}px";
                child.Top = "0px";
                x += child.MarginSize.X;
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
            }
        }

        private void reversedRow()
        {
            int x = 0;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Left = $"{x}px";
                Children[i].Top = "0px";
                x += Children[i].MarginSize.X;
            }
        }

        private void reversedColumn()
        {
            int y = 0;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Left = "0px";
                Children[i].Top = $"{y}px";
                y += Children[i].MarginSize.Y;
            }
        }
    }
}
