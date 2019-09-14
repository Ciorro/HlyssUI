using HlyssUI.Components;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Updaters
{
    class HoverController
    {
        public List<Component> HoveredComponents { get; private set; } = new List<Component>();

        private Vector2i _mPos;

        public void Update(Component rootComponent, Vector2i mousePosition)
        {
            _mPos = mousePosition;

            Component[] prevComponents = new Component[HoveredComponents.Count];
            HoveredComponents.CopyTo(prevComponents);
            HoveredComponents.Clear();

            Component hoveredComponent = FindHovered(rootComponent);

            if (hoveredComponent != null)
            {
                CompareHover(prevComponents.ToList(), HoveredComponents);
            }
        }

        private Component FindHovered(Component component)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                Component hoveredComponent = FindHovered(component.Children[i]);

                if (hoveredComponent != null)
                {
                    HoveredComponents.Add(component);
                    return hoveredComponent;
                }
            }

            if (component.Bounds.Contains(_mPos.X, _mPos.Y) && (component.Parent == null || (component.Parent != null && component.Parent.ClipArea.Bounds.Contains(_mPos.X, _mPos.Y))))
            {
                HoveredComponents.Add(component);
                return component;
            }

            return null;
        }

        private void CompareHover(List<Component> prevComponents, List<Component> currentComponents)
        {
            foreach (var component in prevComponents)
            {
                if (!currentComponents.Contains(component))
                {
                    component.Hovered = false;
                    component.OnMouseLeft();
                }
            }

            foreach (var component in currentComponents)
            {
                if (!prevComponents.Contains(component))
                {
                    component.Hovered = true;
                    component.OnMouseEntered();
                }
            }
        }
    }
}
