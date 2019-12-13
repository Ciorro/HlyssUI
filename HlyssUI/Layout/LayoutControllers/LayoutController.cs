using HlyssUI.Components;
using SFML.System;
using System;

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
        public abstract void ApplyContentCentering(Component component);

        public void ApplyAutosize(Component component)
        {
            if (!component.AutosizeX && !component.AutosizeY)
                return;

            if (component.AutosizeX)
                component.Width = $"{_maxX + component.TargetPaddings.Horizontal}px";
            if (component.AutosizeY)
                component.Height = $"{_maxY + component.TargetPaddings.Vertical}px";

            component.UpdateLocalSize();

            _maxX = 0;
            _maxY = 0;
        }

        public void ApplyMaxSize(Component component)
        {
            component.TargetSize = new Vector2i(Math.Min(component.TargetSize.X, component.MaxW), Math.Min(component.TargetSize.Y, component.MaxH));
        }

        protected void CompareSize(Component component)
        {
            int componentRight = component.TargetRelativePosition.X + component.TargetMargins.Horizontal + component.TargetSize.X;
            int componentBottom = component.TargetRelativePosition.Y + component.TargetMargins.Vertical + component.TargetSize.Y;

            _maxX = Math.Max(_maxX, componentRight);
            _maxY = Math.Max(_maxY, componentBottom);
        }
    }
}
