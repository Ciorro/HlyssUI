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

        public void Update(HlyssForm form, Vector2i mousePosition)
        {
            _mPos = mousePosition;

            Component[] prevComponents = new Component[HoveredComponents.Count];
            HoveredComponents.CopyTo(prevComponents);
            HoveredComponents.Clear();

            FindHovered(form);
            CompareHover(prevComponents.ToList(), HoveredComponents);
        }

        private void FindHovered(HlyssForm form)
        {
            Component firstHovered = null;

            for (int i = form.FlatComponentTree.Count - 1; i >= 0; i--)
            {
                Component component = form.FlatComponentTree[i];

                if (component.Bounds.Contains(_mPos.X, _mPos.Y) && (component.Parent == null || component.PositionType == Layout.PositionType.Fixed || (component.Parent != null && component.Parent.ClipArea.Bounds.Contains(_mPos.X, _mPos.Y))) && component.Hoverable && component.Visible)
                {
                    firstHovered = component;
                    HoveredComponents.Add(component);
                    break;
                }
            }

            if (firstHovered != null)
                AddPredecessors(firstHovered);
        }

        private void AddPredecessors(Component component)
        {
            while (true)
            {
                if (component.Parent != null)
                {
                    HoveredComponents.Add(component.Parent);
                    component = component.Parent;
                    continue;
                }

                break;
            }
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
