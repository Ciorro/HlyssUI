using HlyssUI.Layout;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class ScrollArea : Component
    {
        private Box _box;
        private Panel _panel;
        private HScrollBar _hScroll;
        private VScrollBar _vScroll;
        private bool _disableHScroll;
        private bool _disableVScroll;

        public int Padding = 10;

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

        public ScrollArea(GuiScene scene) : base(scene)
        {
            ClipArea.OutlineThickness = 0;

            _panel = new Panel(scene);
            base.AddChild(_panel);

            _hScroll = new HScrollBar(scene, 1000);
            _hScroll.Target = _panel;
            base.AddChild(_hScroll);

            _vScroll = new VScrollBar(scene, 1000);
            _vScroll.Target = _panel;
            base.AddChild(_vScroll);

            _box = new Box(scene);
            _box.ClipArea.OutlineThickness = 0;
            _panel.AddChild(_box);

            Width = "200px";
            Height = "200px";
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _panel.Size = Size;
            updateAnchor();

            _hScroll.Position = new Vector2i(1, _panel.Size.Y - 16);
            _hScroll.Size = new Vector2i(_panel.Size.X - 2, 15);

            _vScroll.Position = new Vector2i(_panel.Size.X - 16, 1);
            _vScroll.Size = new Vector2i(15, _panel.Size.Y - 2);
        }

        private void updateAnchor()
        {
            int maxX = _box.Size.X + Padding * 2;
            int maxY = _box.Size.Y + Padding * 2;

            _hScroll.Visible = maxX > Size.X && !_disableHScroll;
            _vScroll.Visible = maxY > Size.Y && !_disableVScroll;
            _hScroll.ContentWidth = maxX;
            _vScroll.ContentHeight = maxY;

            _box.Left = $"{(int)((maxX - Size.X) * _hScroll.Percentage) * -1}px";
            _box.Top = $"{(int)((maxY - Size.Y) * _vScroll.Percentage) * -1}px";
            //_box.Position = new Vector2i((int)((maxX - Size.X) * _hScroll.Percentage) * -1, (int)((maxY - Size.Y) * _vScroll.Percentage) * -1) + new Vector2i(Padding, Padding);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _panel.Style["primary"] = Style["primary"];
        }

        #region Add/RemoveChild overrides

        public new void AddChild(Component component)
        {
            _box.AddChild(component);
        }

        public new void RemoveChild(Component component)
        {
            _box.RemoveChild(component);
        }

        public new void RemoveChild(string name)
        {
            _box.RemoveChild(name);
        }
        #endregion
    }
}
