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
    public abstract class Component
    {
        #region Events
        public delegate void AddedHandler(object sender);
        public event AddedHandler Added;

        public delegate void RemovedHandler(object sender);
        public event RemovedHandler Removed;

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
        private DebugRect _debugRect = new DebugRect();

        private bool _transformChanged = true;
        private bool _styleChanged = true;

        private Controller[] _controllers;

        //TODO: Max/Min width and height

        private string _positionX = "0px";
        private string _positionY = "0px";
        private string _width = "0px";
        private string _height = "0px";
        private string _marginLeft = "0px";
        private string _marginRight = "0px";
        private string _marginTop = "0px";
        private string _marginBottom = "0px";
        private string _paddingLeft = "0px";
        private string _paddingRight = "0px";
        private string _paddingTop = "0px";
        private string _paddingBottom = "0px";

        public List<Component> Children { get; private set; } = new List<Component>();
        public Component Parent = null;

        internal Vector2i GlobalPosition
        {
            get { return (Parent != null) ? Parent.GlobalPosition + Position : Position; }
        }

        public Vector2i TargetPosition { get; internal set; } = new Vector2i();
        public Vector2i Position { get; internal set; } = new Vector2i();
        public Vector2i TargetSize { get; internal set; } = new Vector2i();
        public Vector2i Size { get; internal set; } = new Vector2i();

        public Spacing Margins { get; internal set; } = new Spacing();
        public Spacing Paddings { get; internal set; } = new Spacing();

        public IntRect Bounds => new IntRect(GlobalPosition, Size);

        #region Transform getters

        //Position
        internal int X => StringDimensionsConverter.Convert(_positionX, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Y => StringDimensionsConverter.Convert(_positionY, (Parent != null) ? Parent.TargetSize.Y : 0);

        //Size
        internal int W => StringDimensionsConverter.Convert(_width, (Parent != null) ? Parent.TargetSize.X - Parent.Pl - Parent.Pr : 0);
        internal int H => StringDimensionsConverter.Convert(_height, (Parent != null) ? Parent.TargetSize.Y - Parent.Pt - Parent.Pb : 0);

        //Margin
        internal int Ml => StringDimensionsConverter.Convert(_marginLeft, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Mr => StringDimensionsConverter.Convert(_marginRight, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Mt => StringDimensionsConverter.Convert(_marginTop, (Parent != null) ? Parent.TargetSize.Y : 0);
        internal int Mb => StringDimensionsConverter.Convert(_marginBottom, (Parent != null) ? Parent.TargetSize.Y : 0);

        //Padding
        internal int Pl => StringDimensionsConverter.Convert(_paddingLeft, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Pr => StringDimensionsConverter.Convert(_paddingRight, (Parent != null) ? Parent.TargetSize.X : 0);
        internal int Pt => StringDimensionsConverter.Convert(_paddingTop, (Parent != null) ? Parent.TargetSize.Y : 0);
        internal int Pb => StringDimensionsConverter.Convert(_paddingBottom, (Parent != null) ? Parent.TargetSize.Y : 0);
        #endregion

        #region Transform setters

        public string Left
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _positionX = value;
                    TransformChanged = true;
                }
            }
        }

        public string Top
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _positionY = value;
                    TransformChanged = true;
                }
            }
        }

        public string Width
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _width = value;
                    TransformChanged = true;
                }
            }
        }

        public string Height
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _height = value;
                    TransformChanged = true;
                }
            }
        }

        public string MarginLeft
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _marginLeft = value;
                    TransformChanged = true;
                }
            }
        }

        public string MarginRight
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _marginRight = value;
                    TransformChanged = true;
                }
            }
        }

        public string MarginTop
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _marginTop = value;
                    TransformChanged = true;
                }
            }
        }

        public string MarginBottom
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _marginBottom = value;
                    TransformChanged = true;
                }
            }
        }

        public string Margin
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _marginLeft = value;
                    _marginRight = value;
                    _marginTop = value;
                    _marginBottom = value;
                    TransformChanged = true;
                }
            }
        }

        public string PaddingLeft
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _paddingLeft = value;
                    TransformChanged = true;
                }
            }
        }

        public string PaddingRight
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _paddingRight = value;
                    TransformChanged = true;
                }
            }
        }

        public string PaddingTop
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _paddingTop = value;
                    TransformChanged = true;
                }
            }
        }

        public string PaddingBottom
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _paddingBottom = value;
                    TransformChanged = true;
                }
            }
        }

        public string Padding
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _paddingLeft = value;
                    _paddingRight = value;
                    _paddingTop = value;
                    _paddingBottom = value;
                    TransformChanged = true;
                }
            }
        }
        #endregion

        public Style Style = new Style();
        public ClipArea ClipArea { get; private set; }

        public bool IsOnScreen
        {
            get
            {
                //TODO: Move Intersects method from Spacing class
                return Spacing.Intersects(Bounds, (Parent != null) ? Parent.Bounds : Scene.BaseNode.Bounds);
            }
        }

        public bool TransformChanged
        {
            get { return _transformChanged; }
            set
            {
                _transformChanged = value;

                foreach (var child in Children)
                {
                    if (value)
                        child.TransformChanged = value;
                }
            }
        }

        public bool StyleChanged
        {
            get { return _styleChanged; }
            set
            {
                _styleChanged = value;

                foreach (var child in Children)
                {
                    child.StyleChanged = value;
                }
            }
        }

        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool Active { get; set; } = true;
        public bool CoverParent { get; set; } = true;
        public bool IsOverlay { get; protected set; }
        public bool Hovered { get; set; }
        public bool IsPressed { get; private set; }
        public bool DisableClipping { get; set; }
        public bool CascadeStyle { get; set; }

        public string Name { get; set; } = Guid.NewGuid().ToString();
        public Gui Gui { get; set; }
        public GuiScene Scene { get; set; }

        public Component()
        {
            ClipArea = new ClipArea(this);

            _controllers = new Controller[] {
                new PositionController(this),
                new SizeController(this)
            };
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
            component.OnAdded(this);
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
            component.OnRemoved(this);
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

        #endregion

        #region Theming
        public void ResetColors()
        {
            Style.Reset();
        }
        #endregion

        #region Updating
        public virtual void Update()
        {
            foreach (var controller in _controllers)
            {
                controller.Update();
            }

            ClipArea.Update();
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

        #region Drawing
        public virtual void Draw(RenderTarget target) { }
        #endregion

        #region Event handling

        public virtual void OnAdded(Component parent)
        {
            Gui = parent.Gui;
            Scene = parent.Scene;

            Logger.Log($"{this} added to {parent}", Gui.Debug);
            Added?.Invoke(this);
        }

        public virtual void OnRemoved(Component parent)
        {
            Logger.Log($"{this} removed from parent", Gui.Debug);

            Gui = null;
            Scene = null;
            Removed?.Invoke(this);
        }

        public virtual void OnChildAdded(Component child)
        {
        }

        public virtual void OnChildRemoved(Component child)
        {
        }

        public virtual void OnPressed()
        {
            IsPressed = true;
            Logger.Log($"{this} pressed", Gui.Debug);
            Pressed?.Invoke(this);

            //double click
            if (_doubleClick)
            {
                _doubleClick = false;
                return;
            }

            Vector2i currentMPos = Mouse.GetPosition(Gui.Window);

            if (_doubleClickTimer.ElapsedMilliseconds <= 500 && _firstClickPos == currentMPos)
            {
                _doubleClick = true;
                Logger.Log($"{this} double clicked", Gui.Debug);
                DoubleClicked?.Invoke(this);
            }

            _doubleClickTimer.Restart();
            _firstClickPos = currentMPos;
        }

        public virtual void OnReleased()
        {
            if (IsPressed)
            {
                Logger.Log($"{this} clicked", Gui.Debug);
                Clicked?.Invoke(this);
                OnClicked();
            }

            IsPressed = false;
            Logger.Log($"{this} released", Gui.Debug);
            Released?.Invoke(this);
        }

        public virtual void OnClicked()
        {
        }

        public virtual void OnMouseEntered()
        {
            Logger.Log($"Mouse entered {this}", Gui.Debug);
            MouseEntered?.Invoke(this);
        }

        public virtual void OnMouseLeft()
        {
            IsPressed = false;
            Logger.Log($"Mouse left {this}", Gui.Debug);
            MouseLeft?.Invoke(this);
        }

        public virtual void OnRefresh()
        {
            Logger.Log($"{this} refreshed", Gui.Debug);
        }

        public virtual void OnStyleChanged()
        {
            StyleChanged = false;
        }

        public virtual void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
        }

        public virtual void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
        }

        public virtual void OnScrolledAnywhere(float scroll)
        {
        }
        #endregion

        #region Debugging

        public void DrawDebug()
        {
            if (Gui.Debug)
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

        internal void UpdateLocalTransform()
        {
            Logger.Log($"{this} updated transform.", Gui.Debug);

            TargetSize = new Vector2i(W, H);
            TargetPosition = new Vector2i(X, Y);
            Margins = new Spacing(Ml, Mr, Mt, Mb);
            Paddings = new Spacing(Pl, Pr, Pt, Pb);

            foreach (var controller in _controllers)
            {
                controller.OnValueChanged();
            }

            TransformChanged = false;
        }
    }
}
