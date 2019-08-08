using HlyssUI.Components;
using HlyssUI.Layout;
using HlyssUI.Utils;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        private bool _branchNeedsRefresh = false;

        public void Update(Component component)
        {
            for (int i = component.Children.Count - 1; i >= 0; i--)
            {
                Update(component.Children[i]);
            }
                
            if(component is LayoutComponent && _branchNeedsRefresh)
            {
                _branchNeedsRefresh = false;
                (component as LayoutComponent).RefreshLayout();
                (component as LayoutComponent).UpdateLocalTransform();
                return;
            }

            if (component.TransformChanged)
            {
                _branchNeedsRefresh = true;

                if(component is LayoutComponent)
                    (component as LayoutComponent).RefreshLayout();
                component.UpdateLocalTransform();
            }
        }
    }
}
