using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Graphics
{
    internal class RoundedRectangle : Shape
    {
        private Vector2f _size = new Vector2f(100, 100);
        private bool _needsUpdate = false;
        private BorderRadius _corners = new BorderRadius();

        public BorderRadius BorderRadius
        {
            get { return _corners; }
            set
            {
                if (_corners != value)
                {
                    _corners = value;
                    _needsUpdate = true;
                }
            }
        }

        public Vector2f Size
        {
            get { return _size; }
            set
            {
                _size = value;
                _needsUpdate = true;
            }
        }

        public override Vector2f GetPoint(uint index)
        {
            //Author: Overdrivr
            //Source: https://github.com/SFML/SFML/wiki/Source%3A-Draw-Rounded-Rectangle

            int myCornerPointCount = (int)GetPointCount() / 4;

            float deltaAngle = 90.0f / (myCornerPointCount - 1);
            Vector2f center = new Vector2f();
            int centerIndex = (int)index / myCornerPointCount;

            uint radius = centerIndex switch
            {
                0 => BorderRadius.TopLeft,
                1 => BorderRadius.TopRight,
                2 => BorderRadius.BottomRight,
                3 => BorderRadius.BottomLeft,
                _ => BorderRadius.TopLeft
            };

            radius = (uint)Math.Min(Math.Min(_size.X, _size.Y) / 2, radius);

            switch (centerIndex)
            {
                case 0: center.X = _size.X - radius; center.Y = radius; break;
                case 1: center.X = radius; center.Y = radius; break;
                case 2: center.X = radius; center.Y = _size.Y - radius; break;
                case 3: center.X = _size.X - radius; center.Y = _size.Y - radius; break;
            }

            return new Vector2f(radius * (float)Math.Cos(deltaAngle * (index - centerIndex) * Math.PI / 180) + center.X,
                                -radius * (float)Math.Sin(deltaAngle * (index - centerIndex) * Math.PI / 180) + center.Y);
        }

        public override uint GetPointCount()
        {
            if (BorderRadius == BorderRadius.Zero)
                return 8;

            return 32;
        }

        public void UpdateGeometry()
        {
            if (_needsUpdate)
            {
                Update();
                _needsUpdate = false;
            }
        }
    }
}
