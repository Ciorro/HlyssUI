﻿using System;
using HlyssUI.Components;
using HlyssUI.Layout.LayoutControllers;
using HlyssUI.Utils;

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
            component.UpdateLocalSize();
            component.UpdateLocalSpacing();

            foreach (var child in component.Children)
            {
                Compose(child);
            }

            ApplyExpand(component);

            LayoutController layout = LayoutResolver.GetLayout(component.Layout);
            layout.ApplyLayout(component);
            layout.ApplyAutosize(component);

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
            int totalWidth = 0;
            int totalHeight = 0;

            foreach (var child in component.Children)
            {
                if (child.Expand)
                    expandedCompoentnsCount++;
                else
                {
                    totalWidth += child.TargetSize.X + child.TargetMargins.Horizontal;
                    totalHeight += child.TargetSize.Y + child.TargetMargins.Vertical;
                }
            }

            foreach (var child in component.Children)
            {
                if (child.Expand)
                {
                    if(component.Layout == Layout.LayoutType.Row || component.Layout == Layout.LayoutType.Relative)
                        child.Width = $"{(component.TargetSize.X - totalWidth) / expandedCompoentnsCount - child.TargetMargins.Horizontal}px";
                    if (component.Layout == Layout.LayoutType.Column || component.Layout == Layout.LayoutType.Relative)
                        child.Height = $"{(component.TargetSize.Y - totalHeight) / expandedCompoentnsCount - child.TargetMargins.Vertical}px";

                    child.UpdateLocalSize();
                }
            }
        }
    }
}
