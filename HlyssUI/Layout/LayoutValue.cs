using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout
{
    enum ValueType
    {
        Pixel, Percent
    }

    struct LayoutValue
    {
        public static LayoutValue Default
        {
            get { return new LayoutValue(0, ValueType.Pixel); }
        }

        public int Value;
        public ValueType Type;

        public LayoutValue(int value, ValueType type)
        {
            Value = value;
            Type = type;
        }

        public static int GetPixelSize(LayoutValue inputValue, int parentValue)
        {
            float value = inputValue.Value;

            if (inputValue.Type == ValueType.Percent)
            {
                value *= parentValue / 100f;
            }

            return (int)value;
        }

        public static LayoutValue FromString(string str)
        {
            LayoutValue dimension = new LayoutValue();
            int value = 0;

            if (str.EndsWith("px"))
            {
                str = str.TrimEnd('p', 'x', ' ');
                int.TryParse(str, out value);
                dimension.Type = ValueType.Pixel;
            }
            else if (str.EndsWith("%"))
            {
                str = str.TrimEnd('%', ' ');
                int.TryParse(str, out value);
                dimension.Type = ValueType.Percent;
            }

            dimension.Value = value;
            return dimension;
        }
    }
}
