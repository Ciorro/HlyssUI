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
        private bool _disableHScroll;
        private bool _disableVScroll;

        public bool DisableHorizontalScroll
        {
            get { return _disableHScroll; }
            set
            {
                _hScroll.Visible = !value;
                _hScroll.Enabled = !value;
                _disableHScroll = value;
            }
        }

        public bool DisableVerticalScroll
        {
            get { return _disableVScroll; }
            set
            {
                _vScroll.Visible = !value;
                _vScroll.Enabled = !value;
                _disableVScroll = value;
            }
        }

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

        public ScrollArea()
        {
            Layout = LayoutType.Absolute;

            _contentBox = new Component();

            _contentBox.Layout = LayoutType.Scroll;
            _contentBox.Width = "100%";
            _contentBox.Height = "100%";
            _contentBox.Name = "contentbox";
            _contentBox.DisableClipping = false;
            _contentBox.ClipArea.OutlineThickness = -2;
            ClipArea.OutlineThickness = -2;
            AddChild(_contentBox);

            Content = new Component();

            _scrollBox = new Component();
            _scrollBox.Width = "100%";
            _scrollBox.Height = "100%";
            _scrollBox.Layout = LayoutType.Absolute;
            _scrollBox.Reversed = true;
            _scrollBox.Hoverable = false;
            AddChild(_scrollBox);

            _vScroll = new VScrollBar(400);
            _vScroll.Target = this;
            _scrollBox.AddChild(_vScroll);

            _hScroll = new HScrollBar(400);
            _hScroll.Target = this;
            _scrollBox.AddChild(_hScroll);
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            if (Content == null)
                return;

            int maxX = Content.TargetSize.X + _contentBox.Paddings.Horizontal;
            int maxY = Content.TargetSize.Y + _contentBox.Paddings.Vertical;

            _hScroll.Visible = maxX > TargetSize.X && !_disableHScroll;
            _vScroll.Visible = maxY > TargetSize.Y && !_disableVScroll;
            _hScroll.ContentWidth = maxX;
            _vScroll.ContentHeight = maxY;

            if (!_hScroll.Visible)
                _hScroll.Percentage = 0;

            if (!_vScroll.Visible)
                _vScroll.Percentage = 0;

            int x = (int)((maxX - TargetSize.X) * _hScroll.Percentage) * -1;
            int y = (int)((maxY - TargetSize.Y) * _vScroll.Percentage) * -1;

            foreach (var child in Content.Children)
            {
                child.ScrollOffset = new Vector2i(x, y);
            }
        }
    }
}
