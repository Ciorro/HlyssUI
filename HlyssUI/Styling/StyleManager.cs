﻿using HlyssUI.Components;
using HlyssUI.Styling.ValuePresets;
using HlyssUI.Themes;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HlyssUI.Styling
{
    internal class StyleManager
    {
        private Component _component;

        public StyleClass Overrides = new StyleClass();

        public StyleManager(Component owner)
        {
            _component = owner;
        }

        public string GetValue(string name)
        {

            StyleState state = ResolveState();
            Console.Write($"{_component.Name} [{state}]: {name}");

            string value = null;
            Component component = _component;

            while(value == null)
            {
                value = StyleBank.GetClass(_component.Style).GetValue(name, state);

                if (component.Parent != null)
                    component = component.Parent;
                else
                    break;
            }

            if(value == null)
            {
                StyleValue styleValue = StyleValueResolver.Get(name);

                if (styleValue != null)
                    value = styleValue.Value;
                else value = string.Empty;
            }

            Console.WriteLine(" " + value);
            return value;
        }

        public StyleState ResolveState()
        {
            if (!_component.Enabled) return StyleState.Disabled;
            if (_component.IsPressed) return StyleState.Pressed;
            if (_component.Hovered) return StyleState.Hovered;

            return StyleState.Default;
        }

        #region Getters

        public uint GetUint(string key)
        {
            uint val = 0;
            val = uint.TryParse(GetValue(key), out val) ? val : 0;
            return val;
        }

        public int GetInt(string key)
        {
            int val = 0;
            val = int.TryParse(GetValue(key), out val) ? val : 0;
            return val;
        }

        public float GetFloat(string key)
        {
            float val = 0;
            val = float.TryParse(GetValue(key), NumberStyles.Float, CultureInfo.InvariantCulture, out val) ? val : 0;
            return val;
        }

        public bool GetBool(string key)
        {
            bool val = false;
            val = bool.TryParse(GetValue(key), out val) ? val : false;
            return val;
        }

        public Color GetColor(string key)
        {
            Color color = Theme.GetColor(GetValue(key));

            float opacity = GetFloat("opacity");
            if (opacity < 0) opacity = 0;
            if (opacity > 1) opacity = 1;

            color.A = (byte)(color.A * opacity);

            return color;
        }
        #endregion
        //TODO: Getting values from style set in the component
        //TODO: Setting value overrides
    }
}
