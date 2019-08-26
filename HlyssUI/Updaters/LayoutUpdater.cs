using HlyssUI.Components;
using HlyssUI.Layout.LayoutControllers;
using System;
using System.Diagnostics;

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
                //int composeTicks = 0;
                //int refreshTicks = 0;
                //float all = 0;

                //Stopwatch s = Stopwatch.StartNew();
                Compose(component);
                //composeTicks = (int)s.ElapsedTicks;
                //s.Restart();
                Refresh(component);
                //refreshTicks = (int)s.ElapsedTicks;

                //all = composeTicks + refreshTicks;

                //Console.WriteLine($"Layout update: Compose time: {(int)((composeTicks / all) * 100)}%, Refresh time: {(int)((refreshTicks / all) * 100)}%");

                _anyTransformChanged = false;
            }
        }

        private void CheckChanges(Component component)
        {
            if (component.TransformChanged)
                _anyTransformChanged = true;

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

            LayoutController layout = LayoutResolver.GetLayout(component.Layout);
            layout.ApplyLayout(component);
            layout.ApplyAutosize(component);
        }

        private void Refresh(Component component)
        {
            component.ApplyTransform();

            foreach (var child in component.Children)
            {
                Refresh(child);
            }
        }
    }
}
