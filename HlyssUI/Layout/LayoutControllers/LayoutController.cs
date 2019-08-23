using HlyssUI.Components;
using HlyssUI.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout.LayoutControllers
{
    abstract class LayoutController
    {
        private int _maxX = 0;
        private int _maxY = 0;

        public LayoutType Type { get; private set; }

        public LayoutController(LayoutType type)
        {
            Type = type;
        }

        public abstract LayoutController Get();
        public abstract void ApplyLayout(Component component);

        public void ApplyAutosize(Component component)
        {
            if (!component.Autosize)
                return;

            component.Width = $"{_maxX + component.TargetPaddings.Horizontal}px";
            component.Height = $"{_maxY + component.TargetPaddings.Vertical}px";

            component.UpdateLocalSize();

            _maxX = 0;
            _maxY = 0;
        }

        protected void CompareSize(Component component)
        {
            int componentRight = component.TargetPosition.X + component.TargetMargins.Horizontal + component.TargetSize.X;
            int componentBottom = component.TargetPosition.Y + component.TargetMargins.Vertical + component.TargetSize.Y;

            _maxX = Math.Max(_maxX, componentRight);
            _maxY = Math.Max(_maxY, componentBottom);
        }
    }
}
