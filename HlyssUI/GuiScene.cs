using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using HlyssUI.Utils;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

namespace HlyssUI
{
    public class GuiScene
    {
        public Component Root;
        public Gui Gui;

        private Component _hoveredComponent;
        private Renderer _renderer = new Renderer();
        private HoverUpdater _hoverUpdater = new HoverUpdater();
        private StyleUpdater _styleUpdater = new StyleUpdater();
        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private LayoutUpdater _layoutUpdater = new LayoutUpdater();

        public GuiScene(Gui gui)
        {
            Gui = gui;

            Root = new RootComponent();
            Root.Style["Primary"] = Theme.GetColor("Primary");
            Root.Gui = Gui;
            Root.Scene = this;

            Root.OnAdded(Root);
        }

        public void Start()
        {
            registerEvents();
        }

        public void Stop()
        {
            unregisterEvents();
        }

        public void Update()
        {
            _componentUpdater.Update(Root);
            _layoutUpdater.Update(Root);
            _styleUpdater.Update(Root);
        }

        public void Draw()
        {
            //Stopwatch s = Stopwatch.StartNew();
            _renderer.Render(Root);
            //System.Console.WriteLine(s.ElapsedMilliseconds);
        }

        public void AddChild(Component component)
        {
            Root.AddChild(component);
        }

        public void RemoveChild(Component component)
        {
            Root.RemoveChild(component);
        }

        public void RemoveChild(string name)
        {
            Root.RemoveChild(name);
        }

        public void UpdateTheme()
        {
            updateComponentTheme(Root);
            Root.Style["Primary"] = Theme.GetColor("Primary");
        }

        private void updateComponentTheme(Component component)
        {
            component.Style = new Style();

            foreach (var child in component.Children)
            {
                updateComponentTheme(child);
            }
        }

        private void registerEvents()
        {
            Gui.Window.MouseButtonPressed += Window_MouseButtonPressed;
            Gui.Window.MouseButtonReleased += Window_MouseButtonReleased;
            Gui.Window.MouseMoved += Window_MouseMoved;
            Gui.Window.MouseWheelScrolled += Window_MouseWheelScrolled;
        }

        private void unregisterEvents()
        {
            Gui.Window.MouseButtonPressed -= Window_MouseButtonPressed;
            Gui.Window.MouseButtonReleased -= Window_MouseButtonReleased;
            Gui.Window.MouseMoved -= Window_MouseMoved;
            Gui.Window.MouseWheelScrolled -= Window_MouseWheelScrolled;
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            _hoveredComponent = _hoverUpdater.UpdateHover(Root, Mouse.GetPosition(Gui.Window));
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (_hoveredComponent != null && e.Button == Mouse.Button.Left)
            {
                _hoveredComponent.OnReleased();
            }

            sendReleaseInfoToAllChildren(Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (_hoveredComponent != null && e.Button == Mouse.Button.Left)
            {
                _hoveredComponent.OnPressed();
            }

            sendPressInfoToAllChildren(Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            _hoveredComponent = _hoverUpdater.UpdateHover(Root, Mouse.GetPosition(Gui.Window));
            sendScrollInfoToAllChildren(Root, e.Delta);
        }

        private void sendPressInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled)
                return;

            component.OnMousePressedAnywhere(location, button);

            foreach (var child in component.Children)
            {
                sendPressInfoToAllChildren(child, location, button);
            }
        }

        private void sendReleaseInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled)
                return;

            component.OnMouseReleasedAnywhere(location, button);

            foreach (var child in component.Children)
            {
                sendReleaseInfoToAllChildren(child, location, button);
            }
        }

        private void sendScrollInfoToAllChildren(Component component, float scroll)
        {
            if (!component.Enabled)
                return;

            component.OnScrolledAnywhere(scroll);

            foreach (var child in component.Children)
            {
                sendScrollInfoToAllChildren(child, scroll);
            }
        }
    }
}
