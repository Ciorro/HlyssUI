using HlyssUI.Components;
using HlyssUI.Updaters;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Utils
{
    internal class InputManager
    {
        private HlyssForm _form;
        private HoverController _hoverController = new HoverController();

        public void RegisterEvents(HlyssForm form)
        {
            _form = form;

            _form.Window.MouseButtonPressed += Window_MouseButtonPressed;
            _form.Window.MouseButtonReleased += Window_MouseButtonReleased;
            _form.Window.MouseMoved += Window_MouseMoved;
            _form.Window.MouseWheelScrolled += Window_MouseWheelScrolled;
            _form.Window.KeyPressed += Window_KeyPressed;
            _form.Window.KeyReleased += Window_KeyReleased;
            _form.Window.TextEntered += Window_TextEntered;
        }

        public void UnregisterEvents()
        {
            _form.Window.MouseButtonPressed -= Window_MouseButtonPressed;
            _form.Window.MouseButtonReleased -= Window_MouseButtonReleased;
            _form.Window.MouseMoved -= Window_MouseMoved;
            _form.Window.MouseWheelScrolled -= Window_MouseWheelScrolled;
            _form.Window.KeyPressed -= Window_KeyPressed;
            _form.Window.KeyReleased -= Window_KeyReleased;
            _form.Window.TextEntered -= Window_TextEntered;

            _form = null;
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            _hoverController.Update(_form, Mouse.GetPosition(_form.Window));
            sendMouseMoveInfoToAllChildren(_form.Root, new Vector2i(e.X, e.Y));
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _hoverController.HoveredComponents.Count; i++)
            {
                Component component = _hoverController.HoveredComponents[i];

                if (component != null && component.Visible && component.Enabled)
                {
                    component.OnReleased(e.Button);
                }
            }

            sendMouseReleaseInfoToAllChildren(_form.Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (_hoverController.HoveredComponents.Count > 0 && !_hoverController.HoveredComponents[0].Clickable)
                return;

            for (int i = 0; i < _hoverController.HoveredComponents.Count; i++)
            {
                Component component = _hoverController.HoveredComponents[i];

                if (component != null && component.Visible && component.Enabled)
                {
                    component.OnPressed(e.Button);
                }
            }

            sendMousePressInfoToAllChildren(_form.Root, new Vector2i(e.X, e.Y), e.Button);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            _hoverController.Update(_form, Mouse.GetPosition(_form.Window));
            sendScrollInfoToAllChildren(_form.Root, e.Delta);
        }

        private void Window_TextEntered(object sender, TextEventArgs e)
        {
            sendTextInputInfoToAllChildren(_form.Root, e.Unicode);
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            sendKeyReleaseInfoToAllChildren(_form.Root, e.Code);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            sendKeyPressInfoToAllChildren(_form.Root, e.Code);
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
