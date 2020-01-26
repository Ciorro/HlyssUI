using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Styling;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI
{
    public class HlyssApp
    {
        public static bool Debug = false;
        public RenderWindow Window { get; private set; }

        public RootComponent Root { get; private set; }

        public Vector2i MousePosition
        {
            get { return Mouse.GetPosition(Window); }
            set { Mouse.SetPosition(value, Window); }
        }

        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private LayoutUpdater _layoutUpdater = new LayoutUpdater();
        private StyleUpdater _styleUpdater = new StyleUpdater();
        private Renderer _renderer = new Renderer();

        private InputManager _input;
        private TreeFlatter _treeFlatter = new TreeFlatter();

        internal List<Component> FlatComponentTree { get; private set; } = new List<Component>();

        public HlyssApp(RenderWindow window)
        {
            Window = window;
            
            StyleBank.LoadFromString(Encoding.UTF8.GetString(HlyssUI.Properties.Resources.DefaultStyle));

            Root = new RootComponent(this);
            _input = new InputManager(this);
            _input.RegisterEvents();

            Theme.OnThemeLoaded += () => Root.StyleChanged = true;
        }

        public void Update()
        {
            FlatComponentTree = _treeFlatter.GetComponentList(Root);

            _componentUpdater.Update(Root);
            _layoutUpdater.Update(Root);
            _styleUpdater.Update(Root);
        }

        public void Draw()
        {
            _renderer.Render(Root);
        }
    }
}
