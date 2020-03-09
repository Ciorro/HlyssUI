﻿using HlyssUI.Extensions;
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

        public static uint BorderThickness = 1;
        public static uint BorderRadius = 3;
        public static uint CharacterSize = 14;

        private static bool isDarkTheme = false;

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
                return _colors[color].GetModified(isDarkTheme ? modifier * -1 : modifier);
            else
                return stringToColor(color).GetModified(isDarkTheme ? modifier * -1 : modifier);
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

            bool.TryParse(data[theme]["IsDarkTheme"], out isDarkTheme);

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
                {"black-transparent", new Color(0, 0, 0, 0) },
                {"transparent", new Color(0, 0, 0, 0) },
                //HTML KNOWN COLORS
                {"maroon", new Color(128,0,0) },
                {"dark-red", new Color(139,0,0) },
                {"brown", new Color(165,42,42) },
                {"firebrick", new Color(178,34,34) },
                {"crimson", new Color(220,20,60) },
                {"red", new Color(255,0,0) },
                {"tomato", new Color(255,99,71) },
                {"coral", new Color(255,127,80) },
                {"indian-red", new Color(205,92,92) },
                {"light-coral", new Color(240,128,128) },
                {"dark-salmon", new Color(233,150,122) },
                {"salmon", new Color(250,128,114) },
                {"light-salmon", new Color(255,160,122) },
                {"orange-red", new Color(255,69,0) },
                {"dark-orange", new Color(255,140,0) },
                {"orange", new Color(255,165,0) },
                {"gold", new Color(255,215,0) },
                {"dark-golden-rod", new Color(184,134,11) },
                {"golden-rod", new Color(218,165,32) },
                {"pale-golden-rod", new Color(238,232,170) },
                {"dark-khaki", new Color(189,183,107) },
                {"khaki", new Color(240,230,140) },
                {"olive", new Color(128,128,0) },
                {"yellow", new Color(255,255,0) },
                {"yellow-green", new Color(154,205,50) },
                {"dark-olive-green", new Color(85,107,47) },
                {"olive-drab", new Color(107,142,35) },
                {"lawn-green", new Color(124,252,0) },
                {"chart-reuse", new Color(127,255,0) },
                {"green-yellow", new Color(173,255,47) },
                {"dark-green", new Color(0,100,0) },
                {"green", new Color(0,128,0) },
                {"forest-green", new Color(34,139,34) },
                {"lime", new Color(0,255,0) },
                {"lime-green", new Color(50,205,50) },
                {"light-green", new Color(144,238,144) },
                {"pale-green", new Color(152,251,152) },
                {"dark-sea-green", new Color(143,188,143) },
                {"medium-spring-green", new Color(0,250,154) },
                {"spring-green", new Color(0,255,127) },
                {"sea-green", new Color(46,139,87) },
                {"medium-aqua-marine", new Color(102,205,170) },
                {"medium-sea-green", new Color(60,179,113) },
                {"light-sea-green", new Color(32,178,170) },
                {"dark-slate-gray", new Color(47,79,79) },
                {"teal", new Color(0,128,128) },
                {"dark-cyan", new Color(0,139,139) },
                {"aqua", new Color(0,255,255) },
                {"cyan", new Color(0,255,255) },
                {"light-cyan", new Color(224,255,255) },
                {"dark-turquoise", new Color(0,206,209) },
                {"turquoise", new Color(64,224,208) },
                {"medium-turquoise", new Color(72,209,204) },
                {"pale-turquoise", new Color(175,238,238) },
                {"aqua-marine", new Color(127,255,212) },
                {"powder-blue", new Color(176,224,230) },
                {"cadet-blue", new Color(95,158,160) },
                {"steel-blue", new Color(70,130,180) },
                {"corn-flower-blue", new Color(100,149,237) },
                {"deep-sky-blue", new Color(0,191,255) },
                {"dodger-blue", new Color(30,144,255) },
                {"light-blue", new Color(173,216,230) },
                {"sky-blue", new Color(135,206,235) },
                {"light-sky-blue", new Color(135,206,250) },
                {"midnight-blue", new Color(25,25,112) },
                {"navy", new Color(0,0,128) },
                {"dark-blue", new Color(0,0,139) },
                {"medium-blue", new Color(0,0,205) },
                {"blue", new Color(0,0,255) },
                {"royal-blue", new Color(65,105,225) },
                {"blue-violet", new Color(138,43,226) },
                {"indigo", new Color(75,0,130) },
                {"dark-slate-blue", new Color(72,61,139) },
                {"slate-blue", new Color(106,90,205) },
                {"medium-slate-blue", new Color(123,104,238) },
                {"medium-purple", new Color(147,112,219) },
                {"dark-magenta", new Color(139,0,139) },
                {"dark-violet", new Color(148,0,211) },
                {"dark-orchid", new Color(153,50,204) },
                {"medium-orchid", new Color(186,85,211) },
                {"purple", new Color(128,0,128) },
                {"thistle", new Color(216,191,216) },
                {"plum", new Color(221,160,221) },
                {"violet", new Color(238,130,238) },
                {"magenta", new Color(255,0,255) },
                {"orchid", new Color(218,112,214) },
                {"medium-violet-red", new Color(199,21,133) },
                {"pale-violet-red", new Color(219,112,147) },
                {"deep-pink", new Color(255,20,147) },
                {"hot-pink", new Color(255,105,180) },
                {"light-pink", new Color(255,182,193) },
                {"pink", new Color(255,192,203) },
                {"antique-white", new Color(250,235,215) },
                {"beige", new Color(245,245,220) },
                {"bisque", new Color(255,228,196) },
                {"blanched-almond", new Color(255,235,205) },
                {"wheat", new Color(245,222,179) },
                {"corn-silk", new Color(255,248,220) },
                {"lemon-chiffon", new Color(255,250,205) },
                {"light-golden-rod-yellow", new Color(250,250,210) },
                {"light-yellow", new Color(255,255,224) },
                {"saddle-brown", new Color(139,69,19) },
                {"sienna", new Color(160,82,45) },
                {"chocolate", new Color(210,105,30) },
                {"peru", new Color(205,133,63) },
                {"sandy-brown", new Color(244,164,96) },
                {"burly-wood", new Color(222,184,135) },
                {"tan", new Color(210,180,140) },
                {"rosy-brown", new Color(188,143,143) },
                {"moccasin", new Color(255,228,181) },
                {"navajo-white", new Color(255,222,173) },
                {"peach-puff", new Color(255,218,185) },
                {"misty-rose", new Color(255,228,225) },
                {"lavender-blush", new Color(255,240,245) },
                {"linen", new Color(250,240,230) },
                {"old-lace", new Color(253,245,230) },
                {"papaya-whip", new Color(255,239,213) },
                {"sea-shell", new Color(255,245,238) },
                {"mint-cream", new Color(245,255,250) },
                {"slate-gray", new Color(112,128,144) },
                {"light-slate-gray", new Color(119,136,153) },
                {"light-steel-blue", new Color(176,196,222) },
                {"lavender", new Color(230,230,250) },
                {"floral-white", new Color(255,250,240) },
                {"alice-blue", new Color(240,248,255) },
                {"ghost-white", new Color(248,248,255) },
                {"honeydew", new Color(240,255,240) },
                {"ivory", new Color(255,255,240) },
                {"azure", new Color(240,255,255) },
                {"snow", new Color(255,250,250) },
                {"black", new Color(0,0,0) },
                {"dim-gray", new Color(105,105,105) },
                {"dim-grey", new Color(105,105,105) },
                {"gray", new Color(128,128,128) },
                {"grey", new Color(128,128,128) },
                {"dark-gray", new Color(169,169,169) },
                {"dark-grey", new Color(169,169,169) },
                {"silver", new Color(192,192,192) },
                {"light-gray", new Color(211,211,211) },
                {"light-grey", new Color(211,211,211) },
                {"gainsboro", new Color(220,220,220) },
                {"white-smoke", new Color(245,245,245) },
                {"white", new Color(255,255,255) },
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
                Logger.Log($"Invalid color ({color}) [StringToColor]");
                return Color.White;
            }
        }
    }
}
