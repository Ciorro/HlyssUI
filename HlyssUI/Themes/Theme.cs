using HlyssUI.Extensions;
using HlyssUI.Utils;
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
        private static Dictionary<string, Style> _styles = new Dictionary<string, Style>()
        {
            {"default", new Style() }
        };

        public static Style DefaultStyle
        {
            get { return _styles["default"]; }
        }

        public static uint BorderThickness = 1;
        public static uint BorderRadius = 3;
        public static uint CharacterSize = 14;

        public static string Name { get; private set; }

        public static Color GetColor(string color)
        {
            color = color.ToLower();

            int modifier = 0;

            if (color.Split(' ').Length > 1)
            {
                int.TryParse(color.Split(' ')[1], out modifier);
                color = color.Split(' ')[0];
            }

            if (_colors.ContainsKey(color))
                return _colors[color].GetModified(modifier);
            else
                return stringToColor(color).GetModified(modifier);
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

        public static Style GetStyle(string name)
        {
            if (_styles.ContainsKey(name))
                return _styles[name];

            return _styles["default"];
        }

        public static void Load(string file, string theme)
        {
            var parser = new IniParser.FileIniDataParser();
            var data = parser.ReadFile(file);

            _colors = new Dictionary<string, Color>()
            {
                {"text", stringToColor(data[theme]["TextColor"]) },
                {"primary", stringToColor(data[theme]["PrimaryColor"]) },
                {"secondary", stringToColor(data[theme]["SecondaryColor"]) },
                {"accent", stringToColor(data[theme]["AccentColor"]) },
                {"success", stringToColor(data[theme]["SuccessColor"]) },
                {"error", stringToColor(data[theme]["ErrorColor"]) },
                {"warning", stringToColor(data[theme]["WarningColor"]) },
                {"info", stringToColor(data[theme]["InformationColor"]) },
                {"white-transparent", new Color(255, 255, 255, 0) },
                {"black-transparent", new Color(0, 0, 0, 0) }
            };

            Name = theme;
            OnThemeLoaded?.Invoke();
        }

        private static Color stringToColor(string color)
        {
            try
            {
                int currentIndex = 0;
                color = color.TrimStart('#');

                string a = $"0xFF";

                if(color.Length == 8)
                {
                    a = $"0x{color[currentIndex++]}{color[currentIndex++]}";
                }

                string r = $"0x{color[currentIndex++]}{color[currentIndex++]}";
                string g = $"0x{color[currentIndex++]}{color[currentIndex++]}";
                string b = $"0x{color[currentIndex++]}{color[currentIndex++]}";

                byte rb = Convert.ToByte(r, 16);
                byte gb = Convert.ToByte(g, 16);
                byte bb = Convert.ToByte(b, 16);
                byte ab = Convert.ToByte(a, 16);

                return new Color(rb, gb, bb, ab);
            }
            catch
            {
                Logger.Log("Invalid color [StringToColor]");
                return Color.White;
            }
        }
    }
}
