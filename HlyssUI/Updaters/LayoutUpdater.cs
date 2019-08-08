using HlyssUI.Components;
using HlyssUI.Layout;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        //private bool _branchNeedsRefresh = false;

        //public void Update(Component component)
        //{
        //    for (int i = component.Children.Count - 1; i >= 0; i--)
        //    {
        //        Update(component.Children[i]);
        //    }

        //    if(component is LayoutComponent && _branchNeedsRefresh)
        //    {
        //        _branchNeedsRefresh = false;
        //        (component as LayoutComponent).RefreshLayout();
        //        (component as LayoutComponent).UpdateLocalTransform();
        //        return;
        //    }

        //    if (component.TransformChanged)
        //    {
        //        _branchNeedsRefresh = true;

        //        if(component is LayoutComponent)
        //            (component as LayoutComponent).RefreshLayout();
        //        component.UpdateLocalTransform();
        //    }
        //}

        public void Update(Component component)
        {
            updateBranch(component, component is LayoutComponent);
        }

        private void updateBranch(Component component, bool isLayout)
        {
            if(!isLayout && component.TransformChanged)
            {
                if (component is LayoutComponent)
                    (component as LayoutComponent).RefreshLayout();
                component.UpdateLocalTransform();
            }

            foreach (var child in component.Children)
            {
                updateBranch(child, child is LayoutComponent);
            }

            if (isLayout && component.TransformChanged)
            {
                if (component is LayoutComponent)
                    (component as LayoutComponent).RefreshLayout();
                component.UpdateLocalTransform();
            }
        }
    }
}
