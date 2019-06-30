using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Themes
{
    public class Colors
    {
        public Color TextColor;
        public Color PrimaryColor;
        public Color SecondaryColor;
        public Color PrimaryLighter;
        public Color SecondaryLighter;
        public Color AccentColor;
        public Color AccentDarker;
        public Color HoverColor;
        public Color HoverLighter;
        public Color BackgroundColor;

        public Colors()
        {
            TextColor = Theme.TextColor;
            PrimaryColor = Theme.PrimaryColor;
            SecondaryColor = Theme.SecondaryColor;
            PrimaryLighter = Theme.PrimaryLighter;
            SecondaryLighter = Theme.SecondaryLighter;
            AccentColor = Theme.AccentColor;
            HoverColor = Theme.HoverColor;
            HoverLighter = Theme.HoverLighter;
            BackgroundColor = Theme.BackgroundColor;
        }

        public static Color GetLegibleColor(Color color)
        {
            float brightness = (0.299f * color.R + 0.587f * color.G + 0.114f * color.B) / 255f;

            if (brightness > 0.5f)
                return Color.Black;
            else
                return Color.White;
        }

        public static bool operator ==(Colors c1, Colors c2)
        {
            return c1.TextColor == c2.TextColor &&
                   c1.PrimaryColor == c2.PrimaryColor &&
                   c1.SecondaryColor == c2.SecondaryColor &&
                   c1.PrimaryLighter == c2.PrimaryLighter &&
                   c1.SecondaryLighter == c2.SecondaryLighter &&
                   c1.AccentColor == c2.AccentColor &&
                   c1.HoverColor == c2.HoverColor &&
                   c1.HoverLighter == c2.HoverLighter &&
                   c1.BackgroundColor == c2.BackgroundColor;
        }

        public static bool operator !=(Colors c1, Colors c2)
        {
            return c1.TextColor != c2.TextColor ||
                   c1.PrimaryColor != c2.PrimaryColor ||
                   c1.SecondaryColor != c2.SecondaryColor ||
                   c1.PrimaryLighter != c2.PrimaryLighter ||
                   c1.SecondaryLighter != c2.SecondaryLighter ||
                   c1.AccentColor != c2.AccentColor ||
                   c1.HoverColor != c2.HoverColor ||
                   c1.HoverLighter != c2.HoverLighter ||
                   c1.BackgroundColor != c2.BackgroundColor;
        }

        //TODO: Autogenerate darker and lighter variants of colors
    }
}
