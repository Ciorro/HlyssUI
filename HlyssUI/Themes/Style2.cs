using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Themes
{
    public class Style2
    {
        public Color TextColor;
        public Color PrimaryColor;
        public Color SecondaryColor;
        public Color AccentColor;
        public Color SuccessColor;
        public Color ErrorColor;
        public Color WarningColor;
        public Color InformationColor;

        public uint BorderRadius { get; set; } = Theme.BorderRadius;
        public uint BorderThickness { get; set; } = Theme.BorderThickness;

        public Style2()
        {
            Reset();
        }

        public void Reset()
        {
            TextColor = Theme.GetColor("Text");
            PrimaryColor = Theme.GetColor("Primary");
            SecondaryColor = Theme.GetColor("Secondary");
            AccentColor = Theme.GetColor("Accent");
            SuccessColor = Theme.GetColor("Success");
            ErrorColor = Theme.GetColor("Error");
            WarningColor = Theme.GetColor("Warning");
            InformationColor = Theme.GetColor("Information");
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
                baseColor.G = 0;

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

        public static Color GetLegibleColor(Color backgroundColor)
        {
            float brightness = (0.299f * backgroundColor.R + 0.587f * backgroundColor.G + 0.114f * backgroundColor.B) / 255f;

            if (brightness > 0.5f)
                return Color.Black;
            else
                return Color.White;
        }
    }
}
