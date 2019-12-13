using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Extensions
{
    public static class RectExtension
    {
        public static bool Intersects(this IntRect r1, IntRect r2)
        {
            int r1MinX = Math.Min(r1.Left, r1.Left + r1.Width);
            int r1MaxX = Math.Max(r1.Left, r1.Left + r1.Width);
            int r1MinY = Math.Min(r1.Top, r1.Top + r1.Height);
            int r1MaxY = Math.Max(r1.Top, r1.Top + r1.Height);

            int r2MinX = Math.Min(r2.Left, r2.Left + r2.Width);
            int r2MaxX = Math.Max(r2.Left, r2.Left + r2.Width);
            int r2MinY = Math.Min(r2.Top, r2.Top + r2.Height);
            int r2MaxY = Math.Max(r2.Top, r2.Top + r2.Height);

            int interLeft = Math.Max(r1MinX, r2MinX);
            int interTop = Math.Max(r1MinY, r2MinY);
            int interRight = Math.Min(r1MaxX, r2MaxX);
            int interBottom = Math.Min(r1MaxY, r2MaxY);

            if (interLeft <= interRight && interTop <= interBottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
