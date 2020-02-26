using HlyssUI.Components.Routers;
using HlyssUI.Controllers;
using HlyssUI.Extensions;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Styling;
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

        public delegate void PressedHandler(object sender, Mouse.Button button);
        public event PressedHandler Pressed;

        public delegate void ReleasedHandler(object sender, Mouse.Button button);
        public event ReleasedHandler Released;

        public delegate void MouseEnteredHandler(object sender);
        public event MouseEnteredHandler MouseEntered;

        public delegate void MouseLeftHandler(object sender);
        public event MouseLeftHandler MouseLeft;

        public delegate void FocusLostHandler(object sender);
        public event FocusLostHandler FocusLost;

        public delegate void FocusGainedHandler(object sender);
        public event FocusGainedHandler FocusGained;

        public delegate void ScrolledOnHandler(object sender, float scroll);
        public event ScrolledOnHandler ScrolledOn;
        #endregion

        private Stopwatch _doubleClickTimer = Stopwatch.StartNew();
        private Vector2i _firstClickPos = new Vector2i();
        private bool _doubleClick = false;
        private bool _focused = false;
        private bool _enabled = true;
        private bool _visible = true;
        private string _style = string.Empty;
        private DebugRect _debugRect = new DebugRect();
        private Controller[] _controllers;
        private List<Component> _children = new List<Component>();

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

        private LayoutValue _maxWidth = LayoutValue.Max;
        private LayoutValue _maxHeight = LayoutValue.Max;

        private PositionType _positionType = PositionType.Static;
        private LayoutType _layout = LayoutType.Row;

        public List<Component> Children
        {
            get { return _children; }
            set
            {
                if (OverwriteChildren)
                    _children = value;
                else
                    _children.AddRange(value);
            }
        }

        public List<Component> ChildrenBefore
        {
            set
            {
                _children.InsertRange(0, value);
            }
        }

        public Component Slot
        {
            get
            {
                if (!string.IsNullOrEmpty(SlotName))
                    return FindChild(SlotName);
                else return this;
            }
        }

        public List<Component> SlotContent
        {
            get
            {
                return (string.IsNullOrEmpty(SlotName)) ? Children : FindChild(SlotName).Children;
            }
            set
            {
                if (string.IsNullOrEmpty(SlotName))
                    Children = value;
                else
                    FindChild(SlotName).Children = value;
            }
        }

        public Component Parent = null;

        public Vector2i GlobalPosition
        {
            get
            {
                if (PositionType != PositionType.Fixed)
                {
                    Vector2i parentPad = (Parent != null) ? Parent.Paddings.TopLeft : new Vector2i();
                    Vector2i globalPosition = ((Parent != null) ? Parent.GlobalPosition + RelativePosition + parentPad + Margins.TopLeft : RelativePosition);

                    if (Parent != null && Parent.Overflow == OverflowType.Scroll && PositionType != PositionType.Absolute)
                        globalPosition += Parent.ScrollOffset;

                    return globalPosition;
                }
                else
                {
                    return RelativePosition;
                }
            }
        }

        internal Vector2i TargetRelativePosition { get; set; } = new Vector2i();
        internal Vector2i RelativePosition { get; set; } = new Vector2i();

        public Vector2i TargetPosition { get; internal set; } = new Vector2i();
        public Vector2i Position { get; internal set; } = new Vector2i();
        public Vector2i TargetSize { get; internal set; } = new Vector2i();
        public Vector2i Size { get; internal set; } = new Vector2i();

        public Spacing TargetMargins { get; internal set; } = new Spacing();
        public Spacing Margins { get; internal set; } = new Spacing();
        public Spacing TargetPaddings { get; internal set; } = new Spacing();
        public Spacing Paddings { get; internal set; } = new Spacing();

        public Vector2i ScrollOffset { get; internal set; }
        public Vector2i TargetScrollOffset { get; internal set; }

        public IntRect Bounds => new IntRect(GlobalPosition, Size);

        public int ContentHeight
        {
            get
            {
                int maxY = 0;

                foreach (var child in Children)
                {
                    int y = child.TargetRelativePosition.Y + child.TargetMargins.Vertical + child.TargetSize.Y;

                    if (y > maxY)
                    {
                        maxY = y;
                    }
                }

                return maxY;
            }
        }

        public int ContentWidth
        {
            get
            {
                int maxX = 0;

                foreach (var child in Children)
                {
                    int x = child.TargetRelativePosition.X + child.TargetMargins.Horizontal + child.TargetSize.X;

                    if (x > maxX)
                    {
                        maxX = x;
                    }
                }

                return maxX;
            }
        }

        #region Transform getters

        //Position
        internal int X => LayoutValue.GetPixelSize(_positionX, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Y => LayoutValue.GetPixelSize(_positionY, (Parent != null) ? Parent.TargetSize.Y : 0);

        //Size
        internal int W => LayoutValue.GetPixelSize(_width, (Parent != null) ? Parent.TargetSize.X - Parent.TargetPaddings.Horizontal : 0);
        internal int H => LayoutValue.GetPixelSize(_height, (Parent != null) ? Parent.TargetSize.Y - Parent.TargetPaddings.Vertical : 0);
        internal int MaxW => LayoutValue.GetPixelSize(_maxWidth, (Parent != null) ? Parent.TargetSize.X - Parent.TargetPaddings.Horizontal : 0);
        internal int MaxH => LayoutValue.GetPixelSize(_maxHeight, (Parent != null) ? Parent.TargetSize.Y - Parent.TargetPaddings.Vertical : 0);

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

        public string MaxWidth
        {
            set
            {
                _maxWidth = LayoutValue.FromString(value);
                TransformChanged = true;
            }
        }

        public string MaxHeight
        {
            set
            {
                _maxHeight = LayoutValue.FromString(value);
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
                string[] values = Spacing.SplitShorthand(value);

                _marginTop = LayoutValue.FromString(values[0]);
                _marginRight = LayoutValue.FromString(values[1]);
                _marginBottom = LayoutValue.FromString(values[2]);
                _marginLeft = LayoutValue.FromString(values[3]);
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
                string[] values = Spacing.SplitShorthand(value);

                _paddingTop = LayoutValue.FromString(values[0]);
                _paddingRight = LayoutValue.FromString(values[1]);
                _paddingBottom = LayoutValue.FromString(values[2]);
                _paddingLeft = LayoutValue.FromString(values[3]);
                TransformChanged = true;
            }
        }
        #endregion

        #region Styles

        public StyleManager StyleManager;
        public string Style
        {
            get { return _style; }
            set
            {
                _style = value;
                StyleChanged = true;
            }
        }
        #endregion

        public ClipArea ClipArea { get; private set; }

        public bool IsOnScreen
        {
            get
            {
                if (PositionType != PositionType.Fixed)
                    return Bounds.Intersects((Parent != null) ? Parent.ClipArea.Bounds : Form.Root.Bounds);
                else
                    return Bounds.Intersects(Form.Root.Bounds);
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
            get { return Form != null && (Parent != null || this is RootComponent); }
        }

        public bool TransformChanged { get; private set; } = true;
        public bool StyleChanged { get; set; } = true;

        public bool Hovered { get; set; }
        public bool Hoverable { get; set; } = true;
        public bool Clickable { get; set; } = true;
        public bool IsPressed { get; private set; }
        public bool CenterContent { get; set; }
        public bool AutosizeX { get; set; }
        public bool AutosizeY { get; set; }
        public bool ReversedHorizontal { get; set; }
        public bool ReversedVertical { get; set; }
        public bool Expand { get; set; }
        public bool OverwriteChildren { get; set; } = true;

        public string SlotName { get; set; } = string.Empty;

        public OverflowType Overflow = OverflowType.Visible;

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

        public LayoutType Layout
        {
            get { return _layout; }
            set
            {
                if (value != _layout)
                {
                    _layout = value;
                    ScheduleRefresh();
                }
            }
        }

        public PositionType PositionType
        {
            get { return _positionType; }
            set
            {
                if (value != _positionType)
                {
                    _positionType = value;
                    ScheduleRefresh();
                }
            }
        }

        public string Name { get; set; } = string.Empty;
        public readonly string Id = Guid.NewGuid().ToString();

        public HlyssForm Form { get; set; }

        public Component()
        {
            ClipArea = new ClipArea(this);

            _controllers = new Controller[] {
                new PositionController(this),
                new SizeController(this),
                new MarginController(this),
                new PaddingController(this),
                new ScrollController(this)
            };

            StyleManager = new StyleManager(this);

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

        public Component FindParent(string name)
        {
            if (Parent != null && Parent.Name == name)
                return Parent;
            else if (Parent == null)
                return null;
            else
                return Parent.FindParent(name);
        }

        public Router GetClosestRouter()
        {
            if (Parent != null && Parent is Router)
                return Parent as Router;
            else if (Parent == null)
                return null;
            else
                return Parent.GetClosestRouter();
        }

        public void Reparent(Component newParent)
        {
            if (Parent != null)
                Parent.Children.Remove(this);

            newParent.Children.Add(this);
            Parent = newParent;
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
                ForceRefreshAllSubcomponents();
            }
        }

        public void ScheduleRefresh()
        {
            TransformChanged = true;
        }

        public void ForceRefresh()
        {
            if (IsInitialized)
            {
                OnRefresh();
                ClipArea.Update();
            }
        }

        public void ForceRefreshAllSubcomponents()
        {
            ForceRefresh();

            foreach (var child in Children)
            {
                child.ForceRefreshAllSubcomponents();
            }
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

        public virtual void OnPressed(Mouse.Button button)
        {
            StyleChanged = true;

            if (button == Mouse.Button.Left)
                IsPressed = true;

            Pressed?.Invoke(this, button);

            if (_doubleClick)
            {
                _doubleClick = false;
                return;
            }

            Vector2i currentMPos = Mouse.GetPosition(Form.Window);

            if (_doubleClickTimer.ElapsedMilliseconds <= 500 && _firstClickPos.Near(currentMPos, 0) && button == Mouse.Button.Left)
            {
                _doubleClick = true;
                DoubleClicked?.Invoke(this);
                OnDoubleClicked();
            }

            if (button == Mouse.Button.Left)
            {
                _doubleClickTimer.Restart();
                _firstClickPos = currentMPos;
            }
        }

        public virtual void OnReleased(Mouse.Button button)
        {
            if (IsPressed && button == Mouse.Button.Left)
            {
                Focused = true;
                Clicked?.Invoke(this);
                OnClicked();
            }

            IsPressed = false;
            Released?.Invoke(this, button);

            StyleChanged = true;
        }

        public virtual void OnFocusGained()
        {
            FocusGained?.Invoke(this);
        }

        public virtual void OnFocusLost()
        {
            FocusLost?.Invoke(this);
        }

        public virtual void OnClicked() { }

        public virtual void OnDoubleClicked() { }

        public virtual void OnKeyPressed(Keyboard.Key key) { }

        public virtual void OnKeyReleased(Keyboard.Key key) { }

        public virtual void OnTextInput(string text) { }

        public virtual void OnMouseEntered()
        {
            MouseEntered?.Invoke(this);
            StyleChanged = true;
        }

        public virtual void OnMouseLeft()
        {
            IsPressed = false;
            MouseLeft?.Invoke(this);

            StyleChanged = true;
        }

        public virtual void OnMouseMovedAnywhere(Vector2i location) { }

        public virtual void OnRefresh() { }

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

        public virtual void OnScrolledAnywhere(float scroll)
        {
            if (Hovered)
            {
                ScrolledOn?.Invoke(this, scroll);

                if (Overflow == OverflowType.Scroll && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                    ScrollByX((int)scroll * (int)(ContentWidth * 0.05f));
                else if (Overflow == OverflowType.Scroll)
                    ScrollByY((int)scroll * (int)(ContentHeight * 0.05f));
            }
        }
        #endregion

        #region Scrolling
        public void ScrollTo(Vector2i offset)
        {
            int x = offset.X;
            int y = offset.Y;

            if (x < TargetSize.X - ContentWidth - Paddings.Horizontal) x = TargetSize.X - ContentWidth - Paddings.Horizontal;
            if (y < TargetSize.Y - ContentHeight - Paddings.Vertical) y = TargetSize.Y - ContentHeight - Paddings.Vertical;
            if (x > 0) x = 0;
            if (y > 0) y = 0;

            Vector2i newOffset = new Vector2i(x, y);

            if (newOffset != TargetScrollOffset)
            {
                TargetScrollOffset = newOffset;
                ScheduleRefresh();
            }
        }

        public void ScrollToX(int xOffset)
        {
            ScrollTo(new Vector2i(xOffset, 0));
        }

        public void ScrollToY(int yOffset)
        {
            ScrollTo(new Vector2i(0, yOffset));
        }

        public void ScrollBy(Vector2i amount)
        {
            ScrollTo(TargetScrollOffset + amount);
        }

        public void ScrollByX(int xAmount)
        {
            ScrollBy(new Vector2i(xAmount, 0));
        }

        public void ScrollByY(int yAmount)
        {
            ScrollBy(new Vector2i(0, yAmount));
        }
        #endregion

        #region Debugging

        public void DrawDebug()
        {
            if (HlyssForm.Debug)
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
