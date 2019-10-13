using HlyssUI.Controllers;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HlyssUI.Components
{
    public class Component
    {
        #region Events
        public delegate void InitializedHandler(object sender);
        public event InitializedHandler Initialized;

        public delegate void ClickedHandler(object sender);
        public event ClickedHandler Clicked;

        public delegate void DoubleClickedHandler(object sender);
        public event DoubleClickedHandler DoubleClicked;

        public delegate void PressedHandler(object sender);
        public event PressedHandler Pressed;

        public delegate void ReleasedHandler(object sender);
        public event ReleasedHandler Released;

        public delegate void MouseEnteredHandler(object sender);
        public event MouseEnteredHandler MouseEntered;

        public delegate void MouseLeftHandler(object sender);
        public event MouseLeftHandler MouseLeft;
        #endregion

        private Stopwatch _doubleClickTimer = Stopwatch.StartNew();
        private Vector2i _firstClickPos = new Vector2i();
        private bool _doubleClick = false;
        private bool _focused = false;
        private bool _enabled = true;
        private bool _visible = true;
        private DebugRect _debugRect = new DebugRect();
        private Controller[] _controllers;

        private Style _defaultStyle = new Style();
        private Style _hoverStyle = new Style();
        private Style _pressedStyle = new Style();
        private Style _disabledStyle = new Style();

        //TODO: Max/Min width and height

        private LayoutValue _positionX = LayoutValue.Default;
        private LayoutValue _positionY = LayoutValue.Default;
        private LayoutValue _width = LayoutValue.Default;
        private LayoutValue _height = LayoutValue.Default;
        private LayoutValue _marginLeft = LayoutValue.Default;
        private LayoutValue _marginRight = LayoutValue.Default;
        private LayoutValue _marginTop = LayoutValue.Default;
        private LayoutValue _marginBottom = LayoutValue.Default;
        private LayoutValue _paddingLeft = LayoutValue.Default;
        private LayoutValue _paddingRight = LayoutValue.Default;
        private LayoutValue _paddingTop = LayoutValue.Default;
        private LayoutValue _paddingBottom = LayoutValue.Default;

        public List<Component> Children { get; set; } = new List<Component>();
        public Component Parent = null;

        internal Vector2i GlobalPosition
        {
            get
            {
                Vector2i parentPad = (Parent != null) ? Parent.Paddings.TopLeft : new Vector2i();
                return ((Parent != null) ? Parent.GlobalPosition + Position + parentPad + Margins.TopLeft : Position) + ScrollOffset;
            }
        }

        public Vector2i TargetPosition { get; internal set; } = new Vector2i();
        public Vector2i Position { get; internal set; } = new Vector2i();
        public Vector2i TargetSize { get; internal set; } = new Vector2i();
        public Vector2i Size { get; internal set; } = new Vector2i();

        public Spacing TargetMargins { get; internal set; } = new Spacing();
        public Spacing Margins { get; internal set; } = new Spacing();
        public Spacing TargetPaddings { get; internal set; } = new Spacing();
        public Spacing Paddings { get; internal set; } = new Spacing();

        public Vector2i ScrollOffset { get; set; }

        public IntRect Bounds => new IntRect(GlobalPosition, Size);

        #region Transform getters

        //Position
        internal int X => LayoutValue.GetPixelSize(_positionX, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Y => LayoutValue.GetPixelSize(_positionY, (Parent != null) ? Parent.TargetSize.Y : 0);

        //Size
        internal int W => LayoutValue.GetPixelSize(_width, (Parent != null) ? Parent.TargetSize.X - Parent.TargetPaddings.Horizontal : 0);
        internal int H => LayoutValue.GetPixelSize(_height, (Parent != null) ? Parent.TargetSize.Y - Parent.TargetPaddings.Vertical : 0);

        //Margin
        internal int Ml => LayoutValue.GetPixelSize(_marginLeft, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Mr => LayoutValue.GetPixelSize(_marginRight, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Mt => LayoutValue.GetPixelSize(_marginTop, (Parent != null) ? Parent.TargetSize.Y : 0);
        internal int Mb => LayoutValue.GetPixelSize(_marginBottom, (Parent != null) ? Parent.TargetSize.Y : 0);

        //Padding
        internal int Pl => LayoutValue.GetPixelSize(_paddingLeft, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Pr => LayoutValue.GetPixelSize(_paddingRight, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Pt => LayoutValue.GetPixelSize(_paddingTop, (Parent != null) ? Parent.TargetSize.Y : 0);
        internal int Pb => LayoutValue.GetPixelSize(_paddingBottom, (Parent != null) ? Parent.TargetSize.Y : 0);
        #endregion

        #region Transform setters

        public string Left
        {
            set
            {
                _positionX = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string Top
        {
            set
            {
                _positionY = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string Width
        {
            set
            {
                _width = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string Height
        {
            set
            {
                _height = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string MarginLeft
        {
            set
            {
                _marginLeft = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string MarginRight
        {
            set
            {
                _marginRight = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string MarginTop
        {
            set
            {
                _marginTop = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string MarginBottom
        {
            set
            {
                _marginBottom = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string Margin
        {
            set
            {
                _marginLeft = LayoutValue.FromString(value);
                _marginRight = LayoutValue.FromString(value);
                _marginTop = LayoutValue.FromString(value);
                _marginBottom = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string PaddingLeft
        {
            set
            {
                _paddingLeft = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string PaddingRight
        {
            set
            {
                _paddingRight = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string PaddingTop
        {
            set
            {
                _paddingTop = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string PaddingBottom
        {
            set
            {
                _paddingBottom = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string Padding
        {
            set
            {
                _paddingLeft = LayoutValue.FromString(value);
                _paddingRight = LayoutValue.FromString(value);
                _paddingTop = LayoutValue.FromString(value);
                _paddingBottom = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }
        #endregion

        #region Styles

        public Style Style
        {
            get
            {
                if (!Enabled) return DefaultStyle.Combine(DisabledStyle);
                else if (IsPressed) return DefaultStyle.Combine(PressedStyle);
                else if (Hovered) return DefaultStyle.Combine(HoverStyle);

                return DefaultStyle;
            }
        }

        public Style DefaultStyle
        {
            get
            {
                return (Parent != null) ? Parent.DefaultStyle.Combine(_defaultStyle) : _defaultStyle;
            }
            set
            {
                _defaultStyle = value;
                StyleChanged = true;
            }
        }

        public Style HoverStyle
        {
            get
            {
                return (Parent != null) ? Parent.HoverStyle.Combine(_hoverStyle) : _hoverStyle;
            }
            set
            {
                _hoverStyle = value;
                StyleChanged = true;
            }
        }

        public Style PressedStyle
        {
            get
            {
                return (Parent != null) ? Parent.PressedStyle.Combine(_pressedStyle) : _pressedStyle;
            }
            set
            {
                _pressedStyle = value;
                StyleChanged = true;
            }
        }

        public Style DisabledStyle
        {
            get
            {
                return (Parent != null) ? Parent.DisabledStyle.Combine(_disabledStyle) : _disabledStyle;
            }
            set
            {
                _disabledStyle = value;
                StyleChanged = true;
            }
        }
        #endregion

        public ClipArea ClipArea { get; private set; }

        public string Transition
        {
            set
            {
                foreach (var controller in _controllers)
                {
                    controller.TweenType = value;
                }
            }
        }

        public bool IsOnScreen
        {
            get
            {
                return Spacing.Intersects(Bounds, (Parent != null) ? Parent.Bounds : App.Root.Bounds);
            }
        }

        public bool Enabled
        {
            get
            {
                return (Parent != null && !Parent.Enabled) ? false : _enabled;
            }
            set
            {
                _enabled = value;

                if (!Style.IsNullOrEmpty(DisabledStyle))
                    StyleChanged = true;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {

                if (_visible != value)
                {
                ScheduleRefresh();
                StyleChanged = true;
                    OnVisibilityChanged(value);
                }

                _visible = value;
            }
        }

        public bool Focused
        {
            get
            {
                return _focused;
            }
            set
            {
                if (!_focused && value)
                    OnFocusGained();
                else if (_focused && !value)
                    OnFocusLost();

                _focused = value;
            }
        }

        public bool IsInitialized
        {
            get { return App != null && (Parent != null || this is RootComponent); }
        }

        public bool TransformChanged { get; private set; } = true;
        public bool StyleChanged { get; set; } = true;

        public bool IsOverlay { get; protected set; }
        public bool Hovered { get; set; }
        public bool Hoverable { get; set; } = true;
        public bool IsPressed { get; private set; }
        public bool DisableClipping { get; set; } = true;
        public bool CenterContent { get; set; }
        public bool AutosizeX { get; set; }
        public bool AutosizeY { get; set; }
        public bool ReversedHorizontal { get; set; }
        public bool ReversedVertical { get; set; }

        public bool Reversed
        {
            set
            {
                ReversedHorizontal = value;
                ReversedVertical = value;
            }
        }

        public bool Autosize
        {
            set
            {
                AutosizeX = value;
                AutosizeY = value;
            }
        }

        public LayoutType Layout = LayoutType.Row;

        public string Name { get; set; } = Guid.NewGuid().ToString();
        public HlyssApp App { get; set; }

        public Component()
        {
            ClipArea = new ClipArea(this);

            _controllers = new Controller[] {
                new PositionController(this),
                new SizeController(this),
                new MarginController(this),
                new PaddingController(this)
            };

            TransformChanged = true;
            StyleChanged = true;
        }

        #region Children management

        public void AddChild(Component component)
        {
            InsertChild(Children.Count, component);
        }

        public void InsertChild(int index, Component component)
        {
            TransformChanged = true;

            component.Parent = this;
            //component.OnAdded(this);
            Children.Insert(index, component);
            OnChildAdded(component);
        }

        public void RemoveChild(string name)
        {
            RemoveChild(GetChild(name));
        }

        public void RemoveChild(Component component)
        {
            TransformChanged = true;

            component.Parent = null;
            //component.OnRemoved(this);
            Children.Remove(component);
            OnChildRemoved(component);
        }

        public Component GetChild(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name == name)
                    return child;
            }

            return null;
        }

        public void ReorderChild(Component component, int index)
        {
            if (Children.Contains(component))
                Children.Remove(component);

            if (index > Children.Count)
                index = Children.Count;

            Children.Insert(index, component);
        }

        public Component FindChild(string name, Component root = null)
        {
            if (root == null)
                root = this;

            foreach (var child in root.Children)
            {
                if (child.Name == name)
                    return child;

                Component component = FindChild(name, child);

                if (component != null)
                    return component;
            }

            return null;
        }

        #endregion

        #region Theming
        public void ResetColors()
        {
            //Style.Reset();
        }
        #endregion

        #region Updating
        public virtual void Update()
        {
            bool anyTransitionApplied = false;

            foreach (var controller in _controllers)
            {
                if (controller.Update())
                    anyTransitionApplied = true;
            }

            if (anyTransitionApplied && IsOnScreen)
            {
                OnRefresh();
                ClipArea.Update();
            }
        }

        public void ScheduleRefresh()
        {
            TransformChanged = true;
        }

        public void ForceRefresh()
        {
            OnRefresh();
        }
        #endregion

        #region Updating Transform

        internal void UpdateLocalTransform()
        {
            UpdateLocalPosition();
            UpdateLocalSize();
            UpdateLocalSpacing();
        }

        internal void UpdateLocalPosition()
        {
            TargetPosition = new Vector2i(X, Y);
        }

        internal void UpdateLocalSize()
        {
            TargetSize = new Vector2i(W, H);
        }

        internal void UpdateLocalSpacing()
        {
            TargetMargins = new Spacing(Ml, Mr, Mt, Mb);
            TargetPaddings = new Spacing(Pl, Pr, Pt, Pb);
        }

        public void ApplyTransform()
        {
            foreach (var controller in _controllers)
            {
                controller.Start();
            }

            TransformChanged = false;
        }
        #endregion

        #region Drawing
        public virtual void Draw(RenderTarget target) { }
        #endregion

        #region Event handling

        public virtual void OnInitialized() 
        {
            Initialized?.Invoke(this);
        }

        public virtual void OnVisibilityChanged(bool visible) { }

        public virtual void OnChildAdded(Component child) { }

        public virtual void OnChildRemoved(Component child) { }

        public virtual void OnPressed()
        {
            if (!Style.IsNullOrEmpty(PressedStyle))
                StyleChanged = true;

            IsPressed = true;
            Pressed?.Invoke(this);

            if (_doubleClick)
            {
                _doubleClick = false;
                return;
            }

            Vector2i currentMPos = Mouse.GetPosition(App.Window);

            if (_doubleClickTimer.ElapsedMilliseconds <= 500 && _firstClickPos == currentMPos)
            {
                _doubleClick = true;
                DoubleClicked?.Invoke(this);
            }

            _doubleClickTimer.Restart();
            _firstClickPos = currentMPos;
        }

        public virtual void OnReleased()
        {
            if (IsPressed)
            {
                Focused = true;
                Clicked?.Invoke(this);
                OnClicked();
            }

            IsPressed = false;
            Released?.Invoke(this);

            if (!Style.IsNullOrEmpty(PressedStyle))
                StyleChanged = true;
        }

        public virtual void OnFocusGained() { }

        public virtual void OnFocusLost() { }

        public virtual void OnClicked() { }

        public virtual void OnKeyPressed(Keyboard.Key key) { }

        public virtual void OnKeyReleased(Keyboard.Key key) { }

        public virtual void OnTextInput(string text) { }

        public virtual void OnMouseEntered()
        {
            MouseEntered?.Invoke(this);

            //Console.WriteLine($"Mouse entered {this}");

            if (!Style.IsNullOrEmpty(HoverStyle))
                StyleChanged = true;
        }

        public virtual void OnMouseLeft()
        {
            IsPressed = false;
            MouseLeft?.Invoke(this);

            if (!Style.IsNullOrEmpty(HoverStyle))
                StyleChanged = true;
        }

        public virtual void OnMouseMovedAnywhere(Vector2i location) { }

        public virtual void OnRefresh() {/* Logger.Log($"{this} refreshed");*/ }

        public virtual void OnStyleChanged()
        {
            StyleChanged = false;
        }

        public virtual void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            if (!Bounds.Contains(location.X, location.Y))
            {
                Focused = false;
            }
        }

        public virtual void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button) { }

        public virtual void OnScrolledAnywhere(float scroll) { }
        #endregion

        #region Debugging

        public void DrawDebug()
        {
            if (HlyssApp.Debug)
            {
                _debugRect.Update(this);
                _debugRect.Draw(this);
            }
        }
        #endregion

        public override string ToString()
        {
            return $"[name: {((Name.Length > 10) ? Name.Substring(0, 10) + "..." : Name)}, type: {GetType()}]";
        }

        public string ToString(bool showFullNames)
        {
            return $"[name: {((Name.Length > 10 && !showFullNames) ? Name.Substring(0, 10) + "..." : Name)}, type: {GetType()}]";
        }
    }
}
