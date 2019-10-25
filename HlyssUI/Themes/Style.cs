using SFML.Graphics;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HlyssUI.Themes
{
    public class Style : Dictionary<string, string>
    {
        public static Style DefaultStyle
        {
            get
            {
                Style style = new Style();

                style.SetValue("text-color", "text");
                style.SetValue("primary-color", "primary");
                style.SetValue("secondary-color", "secondary");
                style.SetValue("accent-color", "accent");
                style.SetValue("success-color", "success");
                style.SetValue("error-color", "error");
                style.SetValue("warning-color", "warning");
                style.SetValue("information-color", "info");
                style.SetValue("character-size", "14");
                style.SetValue("opacity", "1");
                style.SetValue("border-radius", "4");
                style.SetValue("border-thickness", "1");

                return style;
            }
        }

        public static Style DisabledStyle
        {
            get
            {
                Style style = new Style();

                style.SetValue("text-color", "888888");
                style.SetValue("primary-color", "cccccc");
                style.SetValue("secondary-color", "bbbbbb");
                style.SetValue("accent-color", "cccccc");
                style.SetValue("success-color", "cccccc");
                style.SetValue("error-color", "cccccc");
                style.SetValue("warning-color", "cccccc");
                style.SetValue("information-color", "cccccc");

                return style;
            }
        }

        public static Style EmptyStyle
        {
            get { return new Style(); }
        }

        #region Getters

        //TODO: Allow users to add custom getter methods

        public string GetString(string key)
        {
            if (ContainsKey(key))
                return this[key];

            return string.Empty;
        }

        public uint GetUint(string key)
        {
            uint val = 0;
            uint.TryParse(GetString(key), out val);
            return val;
        }

        public int GetInt(string key)
        {
            int val = 0;
            int.TryParse(GetString(key), out val);
            return val;
        }

        public float GetFloat(string key)
        {
            float val = 0;
            float.TryParse(GetString(key), NumberStyles.Float, CultureInfo.InvariantCulture, out val);
            return val;
        }

        public Color GetColor(string key)
        {
            Color color = ContainsKey(key) ? Theme.GetColor(this[key]) : Color.White;

            float opacity = GetFloat("opacity");
            if (opacity < 0) opacity = 0;
            if (opacity > 1) opacity = 1;

            color.A = (byte)(color.A * opacity);

            return color;
        }
        #endregion

        public void SetValue(string key, string value)
        {
            if (!ContainsKey(key))
                Add(key, value);
            else
                this[key] = value;
        }

        public void SetValue(string key, object value)
        {
            SetValue(key, value.ToString());
        }

        public List<string> GetKeys()
        {
            return Keys.ToList();
        }

        public Style Combine(Style style)
        {
            Style newStyle = new Style();

            List<string> keys = GetKeys();

            foreach (var key in keys)
            {
                newStyle.SetValue(key, GetString(key));
            }

            keys = style.GetKeys();

            foreach (var key in keys)
            {
                if (!style.GetString(key).Contains("final"))
                    newStyle.SetValue(key, style.GetString(key));
            }

            return newStyle;
        }

        public static bool IsNullOrEmpty(Style style)
        {
            return style == null || style.GetKeys().Count == 0;
        }

        public override string ToString()
        {
            string s = string.Empty;
            List<string> keys = GetKeys();

            foreach (var key in keys)
            {
                s += $"[{key}]->[{this[key]}]\n";
            }

            return s;
        }
    }
}
