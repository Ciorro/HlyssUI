using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Updaters;
using SFML.System;
using SFML.Window;
using System.Threading;

namespace HlyssUI
{
    public class GuiScene
    {
        public Component BaseNode;
        public Gui Gui;

        private Component _hoveredComponent;
        private Renderer _renderer = new Renderer();
        private HoverUpdater _hoverUpdater = new HoverUpdater();
        private StyleUpdater _colorUpdater = new StyleUpdater();
        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private PositionUpdater _positionUpdater;

        public GuiScene(Gui gui)
        {
            Gui = gui;

            BaseNode = new BaseComponent(this);
            BaseNode.Style["Primary"] = Theme.GetColor("Primary");

            _positionUpdater = new PositionUpdater(this);
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
            _componentUpdater.Update(BaseNode);
            _positionUpdater.Update(BaseNode);
            _colorUpdater.Update(BaseNode);
        }

        public void Draw()
        {
            _renderer.Render(BaseNode);
        }

        public void AddChild(Component component)
        {
            BaseNode.AddChild(component);
        }

        public void RemoveChild(Component component)
        {
            BaseNode.RemoveChild(component);
        }

        public void RemoveChild(string name)
        {
            BaseNode.RemoveChild(name);
        }

        public void UpdateTheme()
        {
            updateComponentTheme(BaseNode);
            BaseNode.Style["Primary"] = Theme.GetColor("Primary");
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
            _hoveredComponent = _hoverUpdater.UpdateHover(BaseNode, Mouse.GetPosition(Gui.Window));
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (_hoveredComponent != null && e.Button == Mouse.Button.Left)
            {
                _hoveredComponent.OnReleased();
            }

            sendReleaseInfoToAllChildren(BaseNode, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (_hoveredComponent != null && e.Button == Mouse.Button.Left)
            {
                _hoveredComponent.OnPressed();
            }

            sendPressInfoToAllChildren(BaseNode, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            _hoveredComponent = _hoverUpdater.UpdateHover(BaseNode, Mouse.GetPosition(Gui.Window));
            sendScrollInfoToAllChildren(BaseNode, e.Delta);
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
