using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI
{
    public class HlyssApp
    {
        public static bool Debug = false;
        public RenderWindow Window { get; private set; }

        public RootComponent Root;

        private Renderer _renderer = new Renderer();
        private StyleUpdater _styleUpdater = new StyleUpdater();
        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private LayoutUpdater _layoutUpdater = new LayoutUpdater();

        private InputManager _input;

        public HlyssApp(RenderWindow window)
        {
            Window = window;

            Root = new RootComponent(this);
            _input = new InputManager(this);
            _input.RegisterEvents();

            Theme.OnThemeLoaded += Theme_OnThemeLoaded;
        }

        private void Theme_OnThemeLoaded()
        {
            Root.StyleChanged = true;
        }

        public void Update()
        {
            DeltaTime.Update();

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

            if (Keyboard.IsKeyPressed(Keyboard.Key.P))
                Gauge.PrintSummary();
        }
    }
}
