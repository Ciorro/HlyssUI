﻿using HlyssUI.Components;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI
{
    class GuiUpdater
    {
        private Component _currentHover;

        public void Update(Component component)
        {
            if (component.Enabled)
            {
                component.Update();
            }

            foreach (var child in component.Children)
            {
                Update(child);
            }
        }

        public Component UpdateHover(Component component, Vector2i mPos)
        {
            _currentHover = null;

            unhover(component, mPos);
            hover(component, mPos);
            unhoverExceptCurrent(component);

            return _currentHover;
        }

        private void unhover(Component component, Vector2i mPos)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                unhover(component.Children[i], mPos);
            }

            bool prevHoverState = component.Hovered;

            if (!component.Bounds.Contains(mPos.X, mPos.Y) && prevHoverState)
            {
                component.Hovered = false;
                component.OnMouseLeft();
            }
        }

        private void hover(Component component, Vector2i mPos)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                hover(component.Children[i], mPos);
            }

            if (_currentHover != null && _currentHover.CoverParent && _currentHover.Visible)
                return;

            bool prevHoverState = component.Hovered;

            if (component.Bounds.Contains(mPos.X, mPos.Y) && (component.Parent == null || (component.Parent != null && component.Parent.ClipArea.Bounds.Contains(mPos.X, mPos.Y))))
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