using HlyssUI.Components;
using HlyssUI.Updaters;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Utils
{
    internal class InputManager
    {
        private HlyssApp _app;
        private HoverController _hoverController = new HoverController();

        public InputManager(HlyssApp app)
        {
            _app = app;
        }

        public void RegisterEvents()
        {
            _app.Window.MouseButtonPressed += Window_MouseButtonPressed;
            _app.Window.MouseButtonReleased += Window_MouseButtonReleased;
            _app.Window.MouseMoved += Window_MouseMoved;
            _app.Window.MouseWheelScrolled += Window_MouseWheelScrolled;
            _app.Window.KeyPressed += Window_KeyPressed;
            _app.Window.KeyReleased += Window_KeyReleased;
            _app.Window.TextEntered += Window_TextEntered;
        }

        public void UnregisterEvents()
        {
            _app.Window.MouseButtonPressed -= Window_MouseButtonPressed;
            _app.Window.MouseButtonReleased -= Window_MouseButtonReleased;
            _app.Window.MouseMoved -= Window_MouseMoved;
            _app.Window.MouseWheelScrolled -= Window_MouseWheelScrolled;
            _app.Window.KeyPressed -= Window_KeyPressed;
            _app.Window.KeyReleased -= Window_KeyReleased;
            _app.Window.TextEntered -= Window_TextEntered;
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            _hoverController.Update(_app.Root, Mouse.GetPosition(_app.Window));
            sendMouseMoveInfoToAllChildren(_app.Root, new Vector2i(e.X, e.Y));
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _hoverController.HoveredComponents.Count; i++)
            {
                Component component = _hoverController.HoveredComponents[i];

                if (component != null && component.Visible && e.Button == Mouse.Button.Left && component.Enabled)
                {
                    component.OnReleased();
                }
            }

            sendMouseReleaseInfoToAllChildren(_app.Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _hoverController.HoveredComponents.Count; i++)
            {
                Component component = _hoverController.HoveredComponents[i];

                if (component != null && component.Visible && e.Button == Mouse.Button.Left && component.Enabled)
                {
                    component.OnPressed();
                }
            }

            sendMousePressInfoToAllChildren(_app.Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            _hoverController.Update(_app.Root, Mouse.GetPosition(_app.Window));
            sendScrollInfoToAllChildren(_app.Root, e.Delta);
        }

        private void Window_TextEntered(object sender, TextEventArgs e)
        {
            sendTextInputInfoToAllChildren(_app.Root, e.Unicode);
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            sendKeyReleaseInfoToAllChildren(_app.Root, e.Code);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            sendKeyPressInfoToAllChildren(_app.Root, e.Code);
        }

        private void sendMouseMoveInfoToAllChildren(Component component, Vector2i location)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnMouseMovedAnywhere(location);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendMouseMoveInfoToAllChildren(component.Children[i], location);
            }
        }

        private void sendKeyPressInfoToAllChildren(Component component, Keyboard.Key key)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnKeyPressed(key);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendKeyPressInfoToAllChildren(component.Children[i], key);
            }
        }

        private void sendKeyReleaseInfoToAllChildren(Component component, Keyboard.Key key)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnKeyReleased(key);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendKeyReleaseInfoToAllChildren(component.Children[i], key);
            }
        }

        private void sendTextInputInfoToAllChildren(Component component, string text)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnTextInput(text);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendTextInputInfoToAllChildren(component.Children[i], text);
            }
        }

        private void sendMousePressInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnMousePressedAnywhere(location, button);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendMousePressInfoToAllChildren(component.Children[i], location, button);
            }
        }

        private void sendMouseReleaseInfoToAllChildren(Component component, Vector2i location, Mouse.Button button)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnMouseReleasedAnywhere(location, button);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendMouseReleaseInfoToAllChildren(component.Children[i], location, button);
            }
        }

        private void sendScrollInfoToAllChildren(Component component, float scroll)
        {
            if (!component.Enabled || !component.Visible)
                return;

            component.OnScrolledAnywhere(scroll);

            for (int i = 0; i < component.Children.Count; i++)
            {
                sendScrollInfoToAllChildren(component.Children[i], scroll);
            }
        }
    }
}
