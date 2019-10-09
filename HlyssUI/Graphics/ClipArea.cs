using HlyssUI.Components;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Graphics
{
    public class ClipArea
    {
        public View Area { get; private set; } = new View();
        public int OutlineThickness = 0;

        private Component _component;
        private IntRect _currentBounds;

        public IntRect Bounds
        {
            get { return _currentBounds; }
        }

        public ClipArea(Component component)
        {
            _component = component;
        }

        public void Update()
        {
            Vector2f winSize = (Vector2f)_component.App.Window.Size;
            IntRect bounds = getShrinkedComponentBounds(_component.Bounds);

            fitInParent(ref bounds);
            makeEven(ref winSize, ref bounds);

            _currentBounds = bounds;

            Area.Center = new Vector2f((int)(bounds.Left + bounds.Width / 2), (int)(bounds.Top + bounds.Height / 2));
            Area.Size = new Vector2f(bounds.Width, bounds.Height);
            Area.Viewport = new FloatRect(bounds.Left / winSize.X, bounds.Top / winSize.Y, bounds.Width / winSize.X, bounds.Height / winSize.Y);
        }

        private IntRect getShrinkedComponentBounds(IntRect bounds)
        {
            bounds.Left += OutlineThickness;
            bounds.Top += OutlineThickness;
            bounds.Width -= OutlineThickness * 2;
            bounds.Height -= OutlineThickness * 2;

            return bounds;
        }

        private void fitInParent(ref IntRect bounds)
        {
            if (_component.Parent != null)
            {
                if (bounds.Left < _component.Parent.ClipArea.Bounds.Left)
                {
                    bounds.Left = _component.Parent.ClipArea.Bounds.Left;
                    bounds.Width = _component.Bounds.Left + _component.Bounds.Width - bounds.Left;
                }

                if (bounds.Top < _component.Parent.ClipArea.Bounds.Top)
                {
                    bounds.Top = _component.Parent.ClipArea.Bounds.Top;
                    bounds.Height = _component.Bounds.Top + _component.Bounds.Height - bounds.Top;
                }

                if (bounds.Left + bounds.Width > _component.Parent.ClipArea.Bounds.Left + _component.Parent.ClipArea.Bounds.Width)
                    bounds.Width = _component.Parent.ClipArea.Bounds.Left + _component.Parent.ClipArea.Bounds.Width - bounds.Left;
                if (bounds.Top + bounds.Height > _component.Parent.ClipArea.Bounds.Top + _component.Parent.ClipArea.Bounds.Height)
                    bounds.Height = _component.Parent.ClipArea.Bounds.Top + _component.Parent.ClipArea.Bounds.Height - bounds.Top;

                if (bounds.Width < 0)
                    bounds.Width = 0;
                if (bounds.Height < 0)
                    bounds.Height = 0;
            }
        }

        private void makeEven(ref Vector2f winSize, ref IntRect bounds)
        {
            if (winSize.X % 2 != 0)
                winSize.X++;
            if (winSize.Y % 2 != 0)
                winSize.Y++;

            if (bounds.Left % 2 != 0)
                bounds.Left--;
            if (bounds.Top % 2 != 0)
                bounds.Top--;
            if (bounds.Width % 2 != 0)
                bounds.Width++;
            if (bounds.Height % 2 != 0)
                bounds.Height++;
        }
    }
}
