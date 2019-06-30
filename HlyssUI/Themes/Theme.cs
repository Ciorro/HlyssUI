using SFML.Graphics;
using System;

namespace HlyssUI.Themes
{
    static public class Theme
    {
        public delegate void ThemeLoadedHandler();
        public static event ThemeLoadedHandler OnThemeLoaded;

        public static Color TextColor = Color.Black;
        public static Color PrimaryColor = new Color(225, 225, 225);
        public static Color SecondaryColor = new Color(173, 173, 173);
        public static Color PrimaryLighter = new Color(242, 242, 242);
        public static Color SecondaryLighter = new Color(204, 204, 204);
        public static Color AccentColor = new Color(0, 120, 215);
        public static Color AccentDarker = new Color(0, 120, 215);
        public static Color HoverColor = new Color(201, 222, 245);
        public static Color HoverLighter = new Color(229, 241, 251);
        public static Color BackgroundColor = Color.White;
        public static int BorderThickness = -1;

        public static string Name { get; private set; }

        public static void Load(string file, string theme)
        {
            var parser = new IniParser.FileIniDataParser();
            var data = parser.ReadFile(file);

            TextColor = stringToColor(data[theme]["TextColor"]);
            PrimaryColor = stringToColor(data[theme]["PrimaryColor"]);
            SecondaryColor = stringToColor(data[theme]["SecondaryColor"]);
            PrimaryLighter = stringToColor(data[theme]["PrimaryLighter"]);
            SecondaryLighter = stringToColor(data[theme]["SecondaryLighter"]);
            AccentColor = stringToColor(data[theme]["AccentColor"]);
            AccentDarker = stringToColor(data[theme]["AccentDarker"]);
            HoverColor = stringToColor(data[theme]["HoverColor"]);
            HoverLighter = stringToColor(data[theme]["HoverLighter"]);
            BackgroundColor = stringToColor(data[theme]["DialogBackgroundColor"]);

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
