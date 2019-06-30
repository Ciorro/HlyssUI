using HlyssUI.Components;
using HlyssUI.Utils;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout
{
    public class Container : Component
    {
        public enum LayoutOrientation
        {
            Column, Row
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

                if(Fill)
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
    }
}
