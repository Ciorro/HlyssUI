using HlyssUI.Layout;
using SFML.System;

namespace HlyssUI.Components
{
    public class ScrollArea : Component
    {
        private Component _contentBox;
        private Component _scrollBox;
        private VScrollBar _vScroll;
        private HScrollBar _hScroll;

        public Component Content
        {
            get
            {
                if (_contentBox.Children.Count > 0)
                    return _contentBox.Children[0];

                return null;
            }
            set
            {
                if (_contentBox.Children.Count > 0)
                    _contentBox.Children[0] = value;
                else
                    _contentBox.AddChild(value);
            }
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Layout = LayoutType.Relative;

            _contentBox = new Component();
            _contentBox.Layout = LayoutType.Scroll;
            _contentBox.Width = "100%";
            _contentBox.Height = "100%";
            _contentBox.DisableClipping = false;
            _contentBox.Padding = "15px";
            AddChild(_contentBox);

            _scrollBox = new Component();
            _scrollBox.Width = "100%";
            _scrollBox.Height = "100%";
            _scrollBox.Layout = LayoutType.Relative;
            _scrollBox.Reversed = true;
            AddChild(_scrollBox);

            _vScroll = new VScrollBar(400);
            _scrollBox.AddChild(_vScroll);

            _hScroll = new HScrollBar(400);
            _scrollBox.AddChild(_hScroll);
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            if (Content == null)
                return;

            int maxX = Content.TargetSize.X + _contentBox.Paddings.Horizontal;
            int maxY = Content.TargetSize.Y + _contentBox.Paddings.Vertical;

            int x = (int)((maxX - TargetSize.X) * _hScroll.Percentage) * -1;
            int y = (int)((maxY - TargetSize.Y) * _vScroll.Percentage) * -1;

            _contentBox.ScrollOffset = new Vector2i(-x, -y);
        }
    }
}
