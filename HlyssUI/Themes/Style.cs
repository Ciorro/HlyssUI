using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Themes
{
    public class Style
    {
        private Dictionary<string, Color> _colors = new Dictionary<string, Color>();

        public uint BorderRadius { get; set; } = Theme.BorderRadius;
        public uint BorderThickness { get; set; } = Theme.BorderThickness;
        public uint CharacterSize { get; set; } = Theme.CharacterSize;

        public bool NeedsRefresh { get; set; } = true;

        public bool Round
        {
            get { return BorderRadius == uint.MaxValue; }
            set
            {
                if (value)
                    BorderRadius = uint.MaxValue;
                else
                    BorderRadius = Theme.BorderRadius;

                NeedsRefresh = true;
            }
        }

        public Color this[string name]
        {
            get
            {
                name = name.ToLower();

                if (_colors.ContainsKey(name))
                    return _colors[name];
                else
                    return Color.White;
            }
            set
            {
                name = name.ToLower();

                if (_colors.ContainsKey(name))
                    _colors[name] = value;

                NeedsRefresh = true;
            }
        }

        public Style()
        {
            Reset();
        }

        public void Reset()
        {
            _colors = new Dictionary<string, Color>()
            {
                {"text", Theme.GetColor("Text")},
                {"primary", Theme.GetColor("Primary")},
                {"secondary", Theme.GetColor("Secondary")},
                {"accent", Theme.GetColor("Accent")},
                {"success", Theme.GetColor("Success")},
                {"error", Theme.GetColor("Error")},
                {"warning", Theme.GetColor("Warning")},
                {"info",  Theme.GetColor("Info")}
            };

            NeedsRefresh = true;
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
