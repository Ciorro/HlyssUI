using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Styling;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using HlyssUI.Utils;
using SFML.Graphics;
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
            DeltaTime.Update();

            Gauge.StartMeasurement("Flattening", true);
            FlatComponentTree = _treeFlatter.GetComponentList(Root);
            Gauge.PauseMeasurement("Flattening");

            if(Keyboard.IsKeyPressed(Keyboard.Key.D))
                System.Console.WriteLine(string.Join(Environment.NewLine, FlatComponentTree) + "\n----");

            Gauge.StartMeasurement("Updater", true);
            _componentUpdater.Update(Root);
            Gauge.PauseMeasurement("Updater");
            Gauge.StartMeasurement("Layout", true);
            _layoutUpdater.Update(Root);
            Gauge.PauseMeasurement("Layout");
            Gauge.StartMeasurement("Style", true);
            _styleUpdater.Update(Root);
            Gauge.PauseMeasurement("Style");
        }

        public void Draw()
        {
            Gauge.StartMeasurement("Render", true);
            _renderer.Render(Root);
            Gauge.PauseMeasurement("Render");

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                Gauge.PrintSummary("Flattening", "Updater", "Layout", "Style", "Render");
        }
    }
}
