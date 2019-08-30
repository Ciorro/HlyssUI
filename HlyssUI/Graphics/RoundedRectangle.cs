using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Graphics
{
    internal class RoundedRectangle : Shape
    {
        private uint _radius = Theme.BorderRadius;
        private Vector2f _size = new Vector2f(100, 100);

        public uint Radius
        {
            get { return (uint)Math.Min(Math.Min(_size.X, _size.Y) / 2, _radius); }
            set
            {
                _radius = value;
            }
        }

        public Vector2f Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Radius = _radius;
            }
        }

        public override Vector2f GetPoint(uint index)
        {
            float angle = index * 2 * (float)Math.PI / GetPointCount() - (float)Math.PI / 2;
            float x = (float)Math.Cos(angle) * Radius;
            float y = (float)Math.Sin(angle) * Radius;

            if (index < GetPointCount() / 4)
                return new Vector2f(_size.X - Radius + x, Radius + y);
            else if (index < GetPointCount() / 2)
                return new Vector2f(_size.X - Radius + x, _size.Y - Radius + y);
            else if (index < GetPointCount() / 4 * 3)
                return new Vector2f(Radius + x, _size.Y - Radius + y);
            else
                return new Vector2f(Radius + x, Radius + y);
        }

        public override uint GetPointCount()
        {
            return 16;
        }

        public new void UpdateGeometry()
        {
            base.Update();
        }
    }
}
