using SFML.Graphics;
using System;

namespace HlyssUI.Extensions
{
    public static class RectExtension
    {
        public static bool Intersects(this IntRect r1, IntRect r2)
        {
            int r1Left = Math.Min(r1.Left, r1.Left + r1.Width);
            int r1Right = Math.Max(r1.Left, r1.Left + r1.Width);
            int r1Top = Math.Min(r1.Top, r1.Top + r1.Height);
            int r1Bottom = Math.Max(r1.Top, r1.Top + r1.Height);

            int r2Left = Math.Min(r2.Left, r2.Left + r2.Width);
            int r2Right = Math.Max(r2.Left, r2.Left + r2.Width);
            int r2Top = Math.Min(r2.Top, r2.Top + r2.Height);
            int r2Bottom = Math.Max(r2.Top, r2.Top + r2.Height);

            return !(r2Left > r1Right ||
               r2Right < r1Left ||
               r2Top > r1Bottom ||
               r2Bottom < r1Top);
        }
    }
}
