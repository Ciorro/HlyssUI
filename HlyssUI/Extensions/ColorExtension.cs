using SFML.Graphics;
using System;

namespace HlyssUI.Extensions
{
    public static class ColorExtension
    {
        public static Color GetModified(this Color baseColor, int level)
        {
            if (level > 0 && level <= 255)
                return GetLighter(baseColor, (byte)level);
            else if (level < 0 && level >= -255)
                return GetDarker(baseColor, (byte)Math.Abs(level));
            else
                return baseColor;
        }

        public static Color GetDarker(Color baseColor, byte level)
        {
            if (baseColor.R >= level)
                baseColor.R -= level;
            else
                baseColor.R = 0;

            if (baseColor.G >= level)
                baseColor.G -= level;
            else
                baseColor.G = 0;

            if (baseColor.B >= level)
                baseColor.B -= level;
            else
                baseColor.B = 0;

            return baseColor;
        }

        public static Color GetLighter(Color baseColor, byte level)
        {
            if (baseColor.R <= byte.MaxValue - level)
                baseColor.R += level;
            else
                baseColor.R = byte.MaxValue;

            if (baseColor.G <= byte.MaxValue - level)
                baseColor.G += level;
            else
                baseColor.G = byte.MaxValue;

            if (baseColor.B <= byte.MaxValue - level)
                baseColor.B += level;
            else
                baseColor.B = byte.MaxValue;

            return baseColor;
        }

        public static Color GetLegibleColor(this Color backgroundColor)
        {
            float brightness = (0.299f * backgroundColor.R + 0.587f * backgroundColor.G + 0.114f * backgroundColor.B) / 255f;

            if (brightness > 0.5f)
                return Color.Black;
            else
                return Color.White;
        }

        public static Color GetOpposite(this Color color)
        {
            return new Color((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
        }

        public static Color FromHex(this Color color, string hexCode)
        {
            try
            {
                string r = $"0x{hexCode[0]}{hexCode[1]}";
                string g = $"0x{hexCode[2]}{hexCode[3]}";
                string b = $"0x{hexCode[4]}{hexCode[5]}";

                byte rb = Convert.ToByte(r, 16);
                byte gb = Convert.ToByte(g, 16);
                byte bb = Convert.ToByte(b, 16);

                return new Color(rb, gb, bb);
            }
            catch
            {
                return Color.White;
            }
        }

        public static string ToHex(this Color color)
        {
            return $"{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
        }
    }
}
