using HlyssUI.Layout;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class ScrollArea : Box
    {
        private Box _box;
        private Panel _panel;
        private HScrollBar _hScroll;
        private VScrollBar _vScroll;
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
            _box.Padding = "10px";
            _panel.AddChild(_box);
            _box.Layout = LayoutType.Column;

            _panel.Width = "200px";
            _panel.Height = "200px";
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            updateAnchor();

            _hScroll.Position = new Vector2i(1, _panel.Size.Y - 16);
            _hScroll.Size = new Vector2i(_panel.Size.X - 2, 15);
            _hScroll.MarginTop = $"{Size.Y - _hScroll.Size.Y}px";

            _vScroll.Position = new Vector2i(_panel.Size.X - 16, 1);
            _vScroll.Size = new Vector2i(15, _panel.Size.Y - 2);
            _vScroll.MarginLeft = $"{Size.X - _vScroll.Size.X}px";
        }

        public override void Update()
        {
            base.Update();
            //updateAnchor();
        }

        private void updateAnchor()
        {
            int maxX = _box.Size.X;
            int maxY = _box.Size.Y;

            _hScroll.Visible = maxX > Size.X && !_disableHScroll;
            _vScroll.Visible = maxY > Size.Y && !_disableVScroll;
            _hScroll.ContentWidth = maxX;
            _vScroll.ContentHeight = maxY;

            _box.Left = $"{(int)((maxX - Size.X) * _hScroll.Percentage) * -1}px";
            _box.Top = $"{(int)((maxY - Size.Y) * _vScroll.Percentage) * -1}px";
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _panel.Style["primary"] = Style["primary"];
        }

        #region Add/RemoveChild overrides

        public new void AddChild(Component component)
        {
            InsertChild(_box.Children.Count, component);
        }

        public new void InsertChild(int index, Component component)
        {
            NeedsRefresh = true;

            component.Parent = this;
            component.OnAdded();
            _box.InsertChild(index, component);
            OnChildAdded(component);
        }

        public new void RemoveChild(Component component)
        {
            component.Parent = null;
            component.OnRemoved();
            _box.RemoveChild(component);
            OnChildRemoved(component);
        }

        public new void RemoveChild(string name)
        {
            RemoveChild(GetChild(name));
        }

        public new Component GetChild(string name)
        {
            foreach (var child in _box.Children)
            {
                if (child.Name == name)
                    return child;
            }

            return null;
        }
        #endregion
    }
}