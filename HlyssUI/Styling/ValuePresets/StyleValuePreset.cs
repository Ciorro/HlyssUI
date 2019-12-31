using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Styling.ValuePresets
{
    public abstract class StyleValuePreset
    {
        public readonly string Name;
        public readonly string DefaultValue;
        public readonly bool Inheritable;

        protected StyleValuePreset(string name, string defaultValue, bool inheritable)
        {
            Name = name;
            DefaultValue = defaultValue;
            Inheritable = inheritable;
        }

        public StyleValue Get(string value = "")
        {
            return new StyleValue(Name, string.IsNullOrEmpty(value) ? DefaultValue : value, Inheritable);
        }
    }
}
