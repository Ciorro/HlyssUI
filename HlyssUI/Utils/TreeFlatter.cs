using HlyssUI.Components;
using SFML.Window;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Utils
{
    internal class TreeFlatter
    {
        private List<Component> components = new List<Component>();
        private List<Component> onTopComponents = new List<Component>();

        public List<Component> GetComponentList(Component component)
        {
            components.Clear();
            onTopComponents.Clear();

            addComponentSubtree(component, false);
            return components.Concat(onTopComponents).ToList();
        }

        private void addComponentSubtree(Component component, bool isFixed)
        {
            if (component.Visible)
            {
                if (!isFixed)
                {
                    components.Add(component);
                }
                else
                {
                    onTopComponents.Add(component);
                }

                foreach (var childComponent in component.Children)
                {
                    if (childComponent.PositionType == Layout.PositionType.Fixed && childComponent.Visible)
                        isFixed = true;

                    addComponentSubtree(childComponent, isFixed);

                    if (childComponent.PositionType == Layout.PositionType.Fixed && childComponent.Visible)
                        isFixed = false;
                }
            }
        }
    }
}
