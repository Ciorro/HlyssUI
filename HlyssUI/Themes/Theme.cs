using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Xml;

namespace HlyssUI.Themes
{
    class Theme : Dictionary<string, Color>
    {
        public bool IsDarkTheme { get; private set; }

        public Theme(XmlNode themeXml)
        {
            Load(themeXml);
        }

        private void Load(XmlNode themeXml)
        {
            if (themeXml.Attributes["dark"] != null)
            {
                bool isDark = false;
                bool.TryParse(themeXml.Attributes["dark"].Value, out isDark);
                IsDarkTheme = isDark;
            }

            foreach (XmlNode themeProperty in themeXml.ChildNodes)
            {
                if (!ContainsKey(themeProperty.Name))
                    Add(themeProperty.Name, ThemeManager.StringToColor(themeProperty.InnerText));
            }
        }
    }
}
