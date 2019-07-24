using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace HlyssUI.Themes
{
    static public class Theme
    {
        public delegate void ThemeLoadedHandler();
        public static event ThemeLoadedHandler OnThemeLoaded;

        private static Dictionary<string, Color> _colors = new Dictionary<string, Color>();

        public static uint BorderThickness = 1;
        public static uint BorderRadius = 2;

        public static string Name { get; private set; }

        public static Color GetColor(string name)
        {
            if (_colors.ContainsKey(name))
                return _colors[name];
            else
                return Color.White;
        }

        public static void SetColor(string name, Color color)
        {
            if (_colors.ContainsKey(name))
                _colors[name] = color;
        }

        public static void AddColor(string name, Color color)
        {
            if (!_colors.ContainsKey(name))
                _colors.Add(name, color);
        }

        public static void Load(string file, string theme)
        {
            var parser = new IniParser.FileIniDataParser();
            var data = parser.ReadFile(file);

            _colors = new Dictionary<string, Color>()
            {
                {"Text", stringToColor(data[theme]["TextColor"]) },
                {"Primary", stringToColor(data[theme]["PrimaryColor"]) },
                {"Secondary", stringToColor(data[theme]["SecondaryColor"]) },
                {"Accent", stringToColor(data[theme]["AccentColor"]) },
                {"Success", stringToColor(data[theme]["SuccessColor"]) },
                {"Error", stringToColor(data[theme]["ErrorColor"]) },
                {"Warning", stringToColor(data[theme]["WarningColor"]) },
                {"Info", stringToColor(data[theme]["InformationColor"]) }
            };

            Name = theme;
            OnThemeLoaded?.Invoke();
        }

        private static Color stringToColor(string color)
        {
            string r = $"0x{color[0]}{color[1]}";
            string g = $"0x{color[2]}{color[3]}";
            string b = $"0x{color[4]}{color[5]}";
            string a = $"0xFF";

            if (color.Length == 8)
                a = $"0x{color[6]}{color[7]}";

            byte rb = Convert.ToByte(r, 16);
            byte gb = Convert.ToByte(g, 16);
            byte bb = Convert.ToByte(b, 16);
            byte ab = Convert.ToByte(a, 16);

            return new Color(rb, gb, bb, ab);
        }
    }
}
