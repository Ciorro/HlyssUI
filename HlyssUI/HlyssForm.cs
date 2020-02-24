using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI
{
    public class HlyssForm
    {
        public delegate void CloseHandler(object sender);
        public event CloseHandler Closed;

        public delegate void ShowHandler(object sender);
        public event ShowHandler Shown;

        public static bool Debug = false;

        public static ContextSettings WindowSettings = new ContextSettings()
        {
            AntialiasingLevel = 4
        };

        public static uint Framerate { get; set; } = 120;

        public RenderWindow Window { get; private set; }
        public RootComponent Root { get; private set; } = new RootComponent();

        public Vector2i MousePosition
        {
            get { return Mouse.GetPosition(Window); }
            set { Mouse.SetPosition(value, Window); }
        }

        public bool IsOpen
        {
            get { return Window != null && Window.IsOpen; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title && IsOpen)
                    Window.SetTitle(value);
                _title = value;
            }
        }

        public Vector2u Size
        {
            get { return _size; }
            set
            {
                if (value != _size && IsOpen)
                    Window.Size = value;
                _size = value;
            }
        }

        public Image Icon
        {
            get { return _icon; }
            set
            {
                if (value == null)
                    value = new Image(Properties.Resources.Empty_Image);

                if (IsOpen)
                    Window.SetIcon(value.Size.X, value.Size.Y, value.Pixels);

                _icon = value;
            }
        }

        public Styles WindowStyle = Styles.Default;

        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private LayoutUpdater _layoutUpdater = new LayoutUpdater();
        private StyleUpdater _styleUpdater = new StyleUpdater();
        private Renderer _renderer = new Renderer();

        private InputManager _input = new InputManager();
        private TreeFlatter _treeFlatter = new TreeFlatter();

        private string _title = string.Empty;
        private Vector2u _size = new Vector2u();
        private bool _isExternalWindow = false;
        private bool _shouldClose = false;
        private Image _icon;

        private bool _isInitialized = false;

        internal List<Component> FlatComponentTree { get; private set; } = new List<Component>();

        public HlyssForm(RenderWindow window)
        {
            Window = window;
            Init();

            _isExternalWindow = true;
        }

        public HlyssForm() 
        {
            Icon = null;
        }

        public void Show()
        {
            if (!IsOpen)
            {
                Window = new RenderWindow(new VideoMode(Size.X, Size.Y), Title, WindowStyle, WindowSettings);

                Init();

                if (!_isExternalWindow)
                    _input.RegisterEvents(this);

                OnShown();
                Shown?.Invoke(this);
            }
        }

        public void Hide()
        {
            _shouldClose = true;
        }

        public void Update()
        {
            if (!_isExternalWindow)
                Window.DispatchEvents();

            FlatComponentTree = _treeFlatter.GetComponentList(Root);

            _componentUpdater.Update(Root);
            _styleUpdater.Update(Root);
            _layoutUpdater.Update(Root);

            if (_shouldClose)
            {
                Close();
                _shouldClose = false;
            }
            else
            {
                OnUpdate();
            }
        }

        public void Draw()
        {
            if (!IsOpen)
                return;

            if (!_isExternalWindow)
                Window.Clear(Theme.GetColor("primary"));

            _renderer.Render(Root);

            if (!_isExternalWindow)
                Window.Display();
        }

        protected virtual void OnShown() { }
        protected virtual void OnClosed() { }
        protected virtual void OnInitialized() { }
        protected virtual void OnUpdate() { }

        private void Init()
        {
            Root.Form = this;
            Root.OnInitialized();

            Theme.OnThemeLoaded += () => Root.StyleChanged = true;

            if (!_isExternalWindow)
            {
                Window.SetFramerateLimit(Framerate);
                Window.Closed += (_, __) => Hide();
                Window.SetIcon(Icon.Size.X, Icon.Size.Y, Icon.Pixels);
            }

            if (!_isInitialized)
            {
                _isInitialized = true;
                OnInitialized();
            }
        }

        private void Close()
        {
            if (IsOpen)
            {
                _input.UnregisterEvents();

                Window.Close();
                Window.Dispose();
                Window = null;

                OnClosed();
                Closed?.Invoke(this);
            }
        }
    }
}
