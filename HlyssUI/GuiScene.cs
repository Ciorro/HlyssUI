using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Updaters;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI
{
    public class GuiScene
    {
        public RootComponent Root;
        public Gui Gui;

        private Renderer _renderer = new Renderer();
        private HoverController _hoverController = new HoverController();
        private StyleUpdater _styleUpdater = new StyleUpdater();
        private ComponentUpdater _componentUpdater = new ComponentUpdater();
        private LayoutUpdater _layoutUpdater = new LayoutUpdater();

        public GuiScene(Gui gui)
        {
            Gui = gui;

            Root = new RootComponent(gui, this);
            Root.Style.SetValue("primary-color", "primary");
            Root.Gui = Gui;
            Root.Scene = this;
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
            _renderer.Render(Root);
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
            Root.Style.SetValue("primary-color", "primary");
        }

        private void updateComponentTheme(Component component)
        {
            //component.Style.Reset();

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
            Gui.Window.KeyPressed += Window_KeyPressed;
            Gui.Window.KeyReleased += Window_KeyReleased;
            Gui.Window.TextEntered += Window_TextEntered;
        }

        private void unregisterEvents()
        {
            Gui.Window.MouseButtonPressed -= Window_MouseButtonPressed;
            Gui.Window.MouseButtonReleased -= Window_MouseButtonReleased;
            Gui.Window.MouseMoved -= Window_MouseMoved;
            Gui.Window.MouseWheelScrolled -= Window_MouseWheelScrolled;
            Gui.Window.KeyPressed -= Window_KeyPressed;
            Gui.Window.KeyReleased -= Window_KeyReleased;
            Gui.Window.TextEntered -= Window_TextEntered;
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            _hoverController.Update(Root, Mouse.GetPosition(Gui.Window));
            sendMouseMoveInfoToAllChildren(Root, new Vector2i(e.X, e.Y));
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            foreach (var component in _hoverController.HoveredComponents)
            {
                if (component != null && e.Button == Mouse.Button.Left && component.Enabled)
                {
                    component.OnReleased();
                }
            }

            sendMouseReleaseInfoToAllChildren(Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            foreach (var component in _hoverController.HoveredComponents)
            {
                if (component != null && e.Button == Mouse.Button.Left && component.Enabled)
                {
                    component.OnPressed();
                }
            }

            sendMousePressInfoToAllChildren(Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            _hoverController.Update(Root, Mouse.GetPosition(Gui.Window));
            sendScrollInfoToAllChildren(Root, e.Delta);
        }

        private void Window_TextEntered(object sender, TextEventArgs e)
        {
            sendTextInputInfoToAllChildren(Root, e.Unicode);
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            sendKeyReleaseInfoToAllChildren(Root, e.Code);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            sendKeyPressInfoToAllChildren(Root, e.Code);
        }

        private void sendMouseMoveInfoToAllChildren(Component component, Vector2i location)
        {
            if (!component.Enabled)
                return;

            component.OnMouseMoveAnywhere(location);

            foreach (var child in component.Children)
            {
                sendMouseMoveInfoToAllChildren(child, location);
            }
        }

        private void sendKeyPressInfoToAllChildren(Component component, Keyboard.Key key)
        {
            if (!component.Enabled)
                return;

            component.OnKeyPressed(key);

            foreach (var child in component.Children)
            {
                sendKeyPressInfoToAllChildren(child, key);
            }
        }

        private void sendKeyReleaseInfoToAllChildren(Component component, Keyboard.Key key)
        {
            if (!component.Enabled)
                return;

            component.OnKeyReleased(key);

            foreach (var child in component.Children)
            {
                sendKeyReleaseInfoToAllChildren(child, key);
            }
        }

        private void sendTextInputInfoToAllChildren(Component component, string text)
        {
            if (!component.Enabled)
                return;

            component.OnTextInput(text);

            foreach (var child in component.Children)
            {
                sendTextInputInfoToAllChildren(child, text);
            }
        }

        private void sendMousePressInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled)
                return;

            component.OnMousePressedAnywhere(location, button);

            foreach (var child in component.Children)
            {
                sendMousePressInfoToAllChildren(child, location, button);
            }
        }

        private void sendMouseReleaseInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled)
                return;

            component.OnMouseReleasedAnywhere(location, button);

            foreach (var child in component.Children)
            {
                sendMouseReleaseInfoToAllChildren(child, location, button);
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