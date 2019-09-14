using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Updaters
{
    class HoverUpdater
    {
        private Component _currentHover;

        public Component UpdateHover(Component component, Vector2i mPos)
        {
            _currentHover = null;

            ResetHover(component, mPos);
            FindHoveredComponent(component, mPos);
            //unhoverExceptCurrent(component);

            return _currentHover;
        }

        private void ResetHover(Component component, Vector2i mPos)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                ResetHover(component.Children[i], mPos);
            }

            bool prevHoverState = component.Hovered;

            if (!component.Bounds.Contains(mPos.X, mPos.Y) && prevHoverState)
            {
                component.Hovered = false;
                component.OnMouseLeft();
            }
        }

        private void FindHoveredComponent(Component component, Vector2i mPos)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                FindHoveredComponent(component.Children[i], mPos);
            }

            if (_currentHover != null && _currentHover.CoverParent && _currentHover.Visible)
                return;

            bool prevHoverState = component.Hovered;

            if (component.Bounds.Contains(mPos.X, mPos.Y) && component.CoverParent && (component.Parent == null || component.Parent != null && component.Parent.ClipArea.Bounds.Contains(mPos.X, mPos.Y) && component.Enabled))
            {
                component.Hovered = true;

                if (!prevHoverState)
                {
                    component.OnMouseEntered();
                }

                _currentHover = component;
            }
        }

        private void unhoverExceptCurrent(Component component)
        {
            if (_currentHover == null || component.Name != _currentHover.Name)
            {
                if (component.Hovered)
                    component.OnMouseLeft();
                component.Hovered = false;
            }

            foreach (var child in component.Children)
            {
                unhoverExceptCurrent(child);
            }
        }
    }
}
