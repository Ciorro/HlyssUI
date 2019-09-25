namespace HlyssUI.Utils
{
    static class DimensionConverter
    {
        public static int Convert(Dimension inputValue, int parentValue)
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
