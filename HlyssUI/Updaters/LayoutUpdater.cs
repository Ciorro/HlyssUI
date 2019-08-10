using HlyssUI.Components;
using HlyssUI.Layout;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        private bool _branchNeedsRefresh = false;

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

                _branchNeedsRefresh = true;
            }

            foreach (var child in component.Children)
            {
                updateBranch(child, child is LayoutComponent);
            }

            if (isLayout && (component.TransformChanged || _branchNeedsRefresh))
            {
                _branchNeedsRefresh = false;

                if (component is LayoutComponent)
                    (component as LayoutComponent).RefreshLayout();
                component.UpdateLocalTransform();
            }
        }
    }
}
