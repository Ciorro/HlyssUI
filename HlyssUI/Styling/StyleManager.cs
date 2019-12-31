using HlyssUI.Components;
using SFML.Graphics;
using System;
using System.Collections.Generic;
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

            string value = null;
            Component component = _component;

            while(value == null)
            {
                value = StyleBank.GetClass(_component.Style).GetValue(name);

                if (component.Parent != null)
                    component = component.Parent;
                else
                    break;
            }

            return string.Empty; //TODO: value to return if not found
        }

        public StyleState ResolveState()
        {
            if (!_component.Enabled) return StyleState.Disabled;
            if (_component.IsPressed) return StyleState.Pressed;
            if (_component.Hovered) return StyleState.Hovered;

            return StyleState.Default;
        }

        #region Getters

        public uint GetUint(string key, uint @default = 0)
        {
            uint val = 0;
            val = uint.TryParse(GetValue(key), out val) ? val : @default;
            return val;
        }

        public int GetInt(string key, int @default = 0)
        {
            int val = 0;
            val = int.TryParse(GetValue(key), out val) ? val : @default;
            return val;
        }

        public float GetFloat(string key, float @default = 0)
        {
            float val = 0;
            val = float.TryParse(GetValue(key), NumberStyles.Float, CultureInfo.InvariantCulture, out val) ? val : @default;
            return val;
        }

        public bool GetBool(string key, bool @default = false)
        {
            bool val = false;
            val = bool.TryParse(GetValue(key), out val) ? val : @default;
            return val;
        }

        public Color GetColor(string key, Color @default = default)
        {
            Color color = ContainsKey(key) ? Theme.GetColor(this[key]) : @default;

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
