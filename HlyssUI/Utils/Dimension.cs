using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Utils
{
    enum DimensionType
    {
        Pixel, Percent
    }

    struct Dimension
    {
        public static Dimension Default
        {
            get { return new Dimension(0, DimensionType.Pixel); }
        }

        public int Value;
        public DimensionType Type;

        public Dimension(int value, DimensionType type)
        {
            Value = value;
            Type = type;
        }

        public static int GetPixelSize(Dimension inputValue, int parentValue)
        {
            float value = inputValue.Value;

            if (inputValue.Type == DimensionType.Percent)
            {
                value *= parentValue / 100f;
            }

            return (int)value;
        }

        public static Dimension FromString(string str)
        {
            Dimension dimension = new Dimension();
            int value = 0;

            if (str.EndsWith("px"))
            {
                str = str.TrimEnd('p', 'x', ' ');
                int.TryParse(str, out value);
                dimension.Type = DimensionType.Pixel;
            }
            else if (str.EndsWith("%"))
            {
                str = str.TrimEnd('%', ' ');
                int.TryParse(str, out value);
                dimension.Type = DimensionType.Percent;
            }

            dimension.Value = value;
            return dimension;
        }
    }
}
