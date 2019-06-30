using HlyssUI.Components;
using System;
using System.Collections.Generic;

namespace HlyssUI
{
    internal class TreeFlatter
    {
        List<Component> components = new List<Component>();

        public List<Component> GetComponentList(Component component)
        {
            components.Clear();
            addComponentSubtree(component, components);
            return components;
        }

        private void addComponentSubtree(Component component, List<Component> components)
        {
            components.Add(component);

            foreach (var childComponent in component.Children)
            {
                addComponentSubtree(childComponent, components);
            }
        }
    }
}
