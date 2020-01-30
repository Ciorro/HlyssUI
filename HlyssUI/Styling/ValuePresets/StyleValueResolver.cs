using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Styling.ValuePresets
{
    public static class StyleValueResolver
    {
        //Registering all available hss values
        private static List<StyleValuePreset> _presets = new List<StyleValuePreset>()
        {
            new PrimaryColorPreset(),
            new SecondaryColorPreset(),
            new AccentColorPreset(),
            new TextColorPreset(),
            new WarningColorPreset(),
            new InformationColorPreset(),
            new ErrorColorPreset(),
            new SuccessColorPreset(),
            new FontSizePreset(),
            new HoverablePreset(),
            new OpacityPreset(),
            new BorderRadiusPreset(),
            new BorderThicknessPreset(),
            new MarginEasePreset(),
            new MarginEaseDurationPreset(),
            new PaddingEasePreset(),
            new PaddingEaseDurationPreset(),
            new PositionEasePreset(),
            new PositionEaseDurationPreset(),
            new SizeEasePreset(),
            new SizeEaseDurationPreset(),
            new SmoothScrollPreset()
        };

        public static StyleValue Get(string name, string value = "")
        {
            foreach (var preset in _presets)
            {
                if (preset.Name == name)
                    return preset.Get(value);
            }

            return null;
        }

        public static bool IsValueInheritable(string name)
        {
            StyleValue value = Get(name);

            if (value != null)
                return value.Inheritable;

            return false;
        }

        public static void AddPreset(StyleValuePreset preset)
        {
            _presets.Add(preset);
        }

        public static void RemovePreset(string name)
        {
            for (int i = 0; i < _presets.Count; i++)
            {
                if (_presets[i].Name == name)
                {
                    _presets.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
