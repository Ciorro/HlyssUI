using HlyssUI.Layout;

namespace HlyssUI.Components
{
    public class ScrollArea : LayoutComponent
    {
        private HScrollBar _hBar;
        private VScrollBar _vBar;
        private bool _disableHScroll;
        private bool _disableVScroll;

        public bool DisableHorizontalScroll
        {
            get { return _disableHScroll; }
            set
            {
                _hBar.Visible = !value;
                _hBar.Enabled = !value;
                _disableHScroll = value;
            }
        }

        public bool DisableVerticalScroll
        {
            get { return _disableVScroll; }
            set
            {
                _vBar.Visible = !value;
                _vBar.Enabled = !value;
                _disableVScroll = value;
            }
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);
            
            _hBar = new HScrollBar(400);
            _vBar = new VScrollBar(400);
            AddChild(_hBar);
            AddChild(_vBar);

            _hBar.Target = this;
            _vBar.Target = this;

            //ClipArea.OutlineThickness = -1;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _hBar.MarginTop = $"{TargetSize.Y - _hBar.TargetSize.Y}px";
            _vBar.MarginLeft = $"{TargetSize.X - _vBar.TargetSize.X}px";
        }

        public override void RefreshLayout()
        {
            if (Children.Count < 3)
                return;

            Children[2].UpdateLocalTransform();

            int maxX = Children[2].TargetSize.X;
            int maxY = Children[2].TargetSize.Y;
            
            _hBar.Visible = maxX > TargetSize.X && !_disableHScroll;
            _vBar.Visible = maxY > TargetSize.Y && !_disableVScroll;
            _hBar.ContentWidth = maxX;
            _vBar.ContentHeight = maxY;

            int x = (int)((maxX - TargetSize.X) * _hBar.Percentage) * -1;
            int y = (int)((maxY - TargetSize.Y) * _vBar.Percentage) * -1;

            Children[2].Left = $"{x}px";
            Children[2].Top = $"{y}px";
        }
    }
}
