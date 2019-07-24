using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Transitions;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        private TransitionEngine _transitions = new TransitionEngine();
        private Stopwatch _doubleClickTimer = Stopwatch.StartNew();
        private Vector2i _firstClickPos = new Vector2i();
        private bool _doubleClick = false;
        private DebugRect _debugRect = new DebugRect();

        //TODO: Max/Min width and height

        private string _positionX = "0px";
        private string _positionY = "0px";
        private string _sizeX = "0px";
        private string _sizeY = "0px";
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

        internal Vector2i GlobalPosition { get; set; }

        internal Vector2i MarginSize
        {
            get
            {
                return new Vector2i(Ml + Mr + Size.X, Mt + Size.Y + Mb);
            }
        }

        internal Vector2i PaddingSize
        {
            get
            {
                return new Vector2i(Size.X - Pl - Pr, Size.Y - Pt - Pb);
            }
        }

        public IntRect Bounds => new IntRect(GlobalPosition, Size);

        #region Transform getters

        public Vector2i Position
        {
            get
            {
                return new Vector2i(X, Y);
            }
        }

        public Vector2i Size
        {
            get
            {
                return new Vector2i(W, H);
            }
        }

        //Position
        internal int X => StringDimensionsConverter.Convert(_positionX, (Parent != null) ? Parent.Size.X : 0);
        internal int Y => StringDimensionsConverter.Convert(_positionY, (Parent != null) ? Parent.Size.Y : 0);

        //Size
        internal int W => StringDimensionsConverter.Convert(_sizeX, (Parent != null) ? Parent.Size.X - Parent.Pl - Parent.Pr : 0);
        internal int H => StringDimensionsConverter.Convert(_sizeY, (Parent != null) ? Parent.Size.Y - Parent.Pt - Parent.Pb : 0);

        //Margin
        internal int Ml => StringDimensionsConverter.Convert(_marginLeft, (Parent != null) ? Parent.Size.X : 0);
        internal int Mr => StringDimensionsConverter.Convert(_marginRight, (Parent != null) ? Parent.Size.X : 0);
        internal int Mt => StringDimensionsConverter.Convert(_marginTop, (Parent != null) ? Parent.Size.Y : 0);
        internal int Mb => StringDimensionsConverter.Convert(_marginBottom, (Parent != null) ? Parent.Size.Y : 0);

        //Padding
        internal int Pl => StringDimensionsConverter.Convert(_paddingLeft, (Parent != null) ? Parent.Size.X : 0);
        internal int Pr => StringDimensionsConverter.Convert(_paddingRight, (Parent != null) ? Parent.Size.X : 0);
        internal int Pt => StringDimensionsConverter.Convert(_paddingTop, (Parent != null) ? Parent.Size.Y : 0);
        internal int Pb => StringDimensionsConverter.Convert(_paddingBottom, (Parent != null) ? Parent.Size.Y : 0);
        #endregion

        #region Transform setters

        public string Left
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _positionX = value;
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
                }
            }
        }

        public string Width
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _sizeX = value;
                    NeedsRefresh = true;
                }
            }
        }

        public string Height
        {
            set
            {
                if (StringDimensionsConverter.DimRegex.IsMatch(value))
                {
                    _sizeY = value;
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
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
                    NeedsRefresh = true;
                }
            }
        }
        #endregion

        public Style Style = new Style();
        public ClipArea ClipArea { get; private set; }

        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public bool Active { get; set; } = true;
        public bool CoverParent { get; set; } = true;
        public bool IsOverlay { get; protected set; }
        public bool Hovered { get; set; }
        public bool IsPressed { get; private set; }
        public bool NeedsRefresh { get; protected set; }
        public bool DisableClipping { get; set; }
        public bool CascadeColor { get; set; }

        public string Name { get; set; } = Guid.NewGuid().ToString();
        public Gui Gui { get; private set; }

        public Component(Gui gui)
        {
            Gui = gui;
            ClipArea = new ClipArea(this);
            _transitions.Component = this;
        }

        #region Children management

        public void AddChild(Component component)
        {
            InsertChild(Children.Count, component);
        }

        public void InsertChild(int index, Component component)
        {
            NeedsRefresh = true;

            component.Parent = this;
            component.OnAdded();
            Children.Insert(index, component);
            OnChildAdded(component);
        }

        public void RemoveChild(Component component)
        {
            component.Parent = null;
            component.OnRemoved();
            Children.Remove(component);
            OnChildRemoved(component);
        }

        public void RemoveChild(string name)
        {
            RemoveChild(GetChild(name));
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
            _transitions.Update();
            ClipArea.Update();
        }

        public void ScheduleRefresh()
        {
            NeedsRefresh = true;
        }

        public void ForceRefresh()
        {
            OnRefresh();
        }
        //update positiona jezeli wywolal parent lub ustawic childom nowy pos
        //ale jest Transformed position
        #endregion

        #region Drawing
        public virtual void Draw(RenderTarget target) { }
        #endregion

        #region Event handling

        public virtual void OnAdded()
        {
            Logger.Log($"{this} added to {this.Parent}", Gui.Debug);
            Added?.Invoke(this);
        }

        public virtual void OnRemoved()
        {
            Logger.Log($"{this} removed from parent", Gui.Debug);
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
            }

            IsPressed = false;
            Logger.Log($"{this} released", Gui.Debug);
            Released?.Invoke(this);
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
            NeedsRefresh = false;
        }

        public virtual void OnStyleChanged()
        {

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

        #region Transitions

        public void Resize(int w, int h, string unit)
        {
            Match matchFromW = StringDimensionsConverter.DimRegex.Match(_sizeX);
            Match matchFromH = StringDimensionsConverter.DimRegex.Match(_sizeY);

            _transitions.RunAll(new SizeTransition(int.Parse(matchFromW.Groups[1].Value), int.Parse(matchFromH.Groups[1].Value), w, h, unit, this));
        }

        public void ChangeColor(string color, Color to)
        {
            _transitions.RunAll(new ColorTransition(to, color));
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
            return $"[name: {Name}, type: {GetType()}]";
        }
    }
}
