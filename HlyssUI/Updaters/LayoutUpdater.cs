using HlyssUI.Components;
using HlyssUI.Layout;
using HlyssUI.Layout.LayoutControllers;
using System;

namespace HlyssUI.Updaters
{
    class LayoutUpdater
    {
        private bool _anyTransformChanged;

        public void Update(Component component)
        {
            CheckChanges(component);

            if (_anyTransformChanged)
            {
                Compose(component);
                Refresh(component);

                _anyTransformChanged = false;
            }
        }

        private void CheckChanges(Component component)
        {
            if (component.TransformChanged)
            {
                _anyTransformChanged = true;
            }

            foreach (var child in component.Children)
            {
                CheckChanges(child);
            }
        }

        private void Compose(Component component)
        {
            component.UpdateLocalTransform();

            ApplyExpand(component);

            foreach (var child in component.Children)
            {
                Compose(child);
            }
            /* TODO: SpaceSplitters
             * - commonsplitter: normal splitter including expand behaviour
             * - equalsplitter: every component has the same width/height
             */
            ApplyExpand(component);

            LayoutController layout = LayoutResolver.GetLayout(component.Layout);
            layout.ApplyLayout(component);
            layout.ApplyAutosize(component);
            layout.ApplyMaxSize(component);

            if (component.CenterContent)
                layout.ApplyContentCentering(component);
        }

        private void Refresh(Component component)
        {
            component.ApplyTransform();

            foreach (var child in component.Children)
            {
                Refresh(child);
            }
        }

        private void ApplyExpand(Component component)
        {
            int expandedCompoentnsCount = 0;
            int totalWidth = component.TargetPaddings.Horizontal;
            int totalHeight = component.TargetPaddings.Vertical;

            foreach (var child in component.Children)
            {
                if (child.Expand)
                {
                    expandedCompoentnsCount++;
                }
                else if (child.PositionType != PositionType.Fixed && child.PositionType != PositionType.Absolute && child.Visible)
                {
                    child.UpdateLocalTransform();

                    totalWidth += child.TargetSize.X + child.TargetMargins.Horizontal;
                    totalHeight += child.TargetSize.Y + child.TargetMargins.Vertical;
                }
            }

            foreach (var child in component.Children)
            {
                if (child.Expand)
                {
                    if (component.Layout == LayoutType.Row || component.Layout == LayoutType.Absolute)
                    {
                        int width = (component.TargetSize.X - totalWidth) / expandedCompoentnsCount - child.TargetMargins.Horizontal;
                        child.Width = $"{Math.Max(width, 0)}px";
                    }

                    if (component.Layout == LayoutType.Column || component.Layout == LayoutType.Absolute)
                    {
                        int height = (component.TargetSize.Y - totalHeight) / expandedCompoentnsCount - child.TargetMargins.Vertical;
                        child.Height = $"{Math.Max(height, 0)}px";
                    }

                    child.UpdateLocalSize();
                }
            }
        }
    }
}
