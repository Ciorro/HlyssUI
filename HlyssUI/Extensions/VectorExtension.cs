using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Extensions
{
    public static class VectorExtension
    {
        public static bool Near(this Vector2i v1, Vector2i v2, int maxDiff)
        {
            return Distance(v1, v2) <= maxDiff;
        }

        private static float Distance(Vector2i point1, Vector2i point2)
        {
            float xDistance = Math.Abs(point1.X - point2.X);
            float yDistance = Math.Abs(point1.Y - point2.Y);

            float distance = (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance);

            return distance;
        }
    }
}
