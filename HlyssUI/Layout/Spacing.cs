using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Layout
{
    public class Spacing
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public int Horizontal
        {
            get { return Left + Right; }
        }

        public int Vertical
        {
            get { return Top + Bottom; }
        }

        public Vector2i TopLeft
        {
            get { return new Vector2i(Left, Top); }
        }

        public Vector2i TopRight
        {
            get { return new Vector2i(Right, Top); }
        }

        public Vector2i BottomLeft
        {
            get { return new Vector2i(Left, Bottom); }
        }

        public Vector2i BottomRight
        {
            get { return new Vector2i(Right, Bottom); }
        }

        public Spacing() { }

        public Spacing(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static bool operator ==(Spacing s1, Spacing s2)
        {
            return s1.Left == s2.Left &&
                   s1.Right == s2.Right &&
                   s1.Top == s2.Top &&
                   s1.Bottom == s2.Bottom;
        }

        public static bool operator !=(Spacing s1, Spacing s2)
        {
            return s1.Left != s2.Left ||
                   s1.Right != s2.Right ||
                   s1.Top != s2.Top ||
                   s1.Bottom != s2.Bottom;
        }

        //TODO: Move Intersects method from Spacing class
        public static bool Intersects(IntRect r1, IntRect r2)
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

        public static string[] SplitShorthand(string shorthandStr)
        {
            string[] outputParts = new string[4]
            {
                "0px", "0px", "0px", "0px"
            };

            string[] inputParts = shorthandStr.Trim(' ').Split(' ');

            switch (inputParts.Length)
            {
                case 1: outputParts[0] = outputParts[1] = outputParts[2] = outputParts[3] = inputParts[0]; break;
                case 2: 
                    outputParts[0] = outputParts[2] = inputParts[0]; 
                    outputParts[1] = outputParts[3] = inputParts[1];
                    break;
                case 3:
                    outputParts[0] = inputParts[0];
                    outputParts[1] = outputParts[2] = inputParts[1];
                    outputParts[3] = inputParts[2];
                    break;
                case 4:
                    outputParts[0] = inputParts[0];
                    outputParts[1] = inputParts[1];
                    outputParts[2] = inputParts[2];
                    outputParts[3] = inputParts[3];
                    break;
                default:
                    break;
            }

            return outputParts;
        }

        public override string ToString()
        {
            return $"[Spacing] Top({Top}) Right({Right}) Bottom({Bottom}) Letf({Left})";
        }
    }
}
