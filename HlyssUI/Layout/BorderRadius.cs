namespace HlyssUI.Layout
{
    class BorderRadius
    {
        internal const int DefaultRadius = 4;

        public static BorderRadius Zero
        {
            get { return new BorderRadius(0); }
        }

        public uint TopLeft { get; set; } = DefaultRadius;
        public uint TopRight { get; set; } = DefaultRadius;
        public uint BottomRight { get; set; } = DefaultRadius;
        public uint BottomLeft { get; set; } = DefaultRadius;

        public BorderRadius() { }

        public BorderRadius(uint topLeft, uint topRight, uint bottomRight, uint bottomLeft)
        {
            SetValues(topLeft, topRight, bottomRight, bottomLeft);
        }

        public BorderRadius(uint topLeft, uint topRightBottomLeft, uint bottomRight)
        {
            SetValues(topLeft, topRightBottomLeft, bottomRight, topRightBottomLeft);
        }

        public BorderRadius(uint topLeftBottomRight, uint topRightBottomLeft)
        {
            SetValues(topLeftBottomRight, topRightBottomLeft, topLeftBottomRight, topRightBottomLeft);
        }

        public BorderRadius(uint radius)
        {
            SetValues(radius, radius, radius, radius);
        }

        public BorderRadius(string shorthand)
        {
            string[] corners = SplitShorthand(shorthand);
            uint[] rad = new uint[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
                uint.TryParse(corners[i], out rad[i]);

            SetValues(rad[0], rad[1], rad[2], rad[3]);
        }

        private void SetValues(uint topLeft, uint topRight, uint bottomRight, uint bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }

        public static bool operator ==(BorderRadius br1, BorderRadius br2)
        {
            return br1.TopLeft == br2.TopLeft &&
                   br1.TopRight == br2.TopRight &&
                   br1.BottomRight == br2.BottomRight &&
                   br1.BottomLeft == br2.BottomLeft;
        }

        public static bool operator !=(BorderRadius br1, BorderRadius br2)
        {
            return br1.TopLeft != br2.TopLeft ||
                   br1.TopRight != br2.TopRight ||
                   br1.BottomRight != br2.BottomRight ||
                   br1.BottomLeft != br2.BottomLeft;
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
                    outputParts[1] = outputParts[3] = inputParts[1];
                    outputParts[2] = inputParts[2];
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
    }
}
