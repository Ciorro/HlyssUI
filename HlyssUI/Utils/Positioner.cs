using HlyssUI.Components;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Utils
{
    class Positioner
    {
        public void Scan(Component baseComponent)
        {
            if(baseComponent.NeedsRefresh)
            {
                RefreshComponents(baseComponent);
            }

            foreach (var child in baseComponent.Children)
            {
                Scan(child);
            }
        }

        private void RefreshComponents(Component component)
        {
            refreshPosition(component);
            component.OnRefresh(); //ostatnio przesuniete z dolu

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }
        }

        private static void refreshPosition(Component component)
        {
            Vector2i parentPos = (component.Parent != null) ? component.Parent.GlobalPosition : new Vector2i();
            Vector2i parentPad = (component.Parent != null) ? new Vector2i(component.Parent.Pl, component.Parent.Pt) : new Vector2i();
            component.GlobalPosition = new Vector2i(component.X + component.Ml, component.Y + component.Mt) + parentPad + parentPos;
        }
    }
}
