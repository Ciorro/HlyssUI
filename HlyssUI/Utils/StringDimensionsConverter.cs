using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HlyssUI.Utils
{
    static class StringDimensionsConverter
    {
        public static Regex DimRegex = new Regex(@"^([+-]?\d+)(%|px|vw|vh)$", RegexOptions.Compiled);

        public static int Convert(string inputValue, int parentValue)
        {
            Match match = DimRegex.Match(inputValue);
            int value = 0;

            if (match.Success)
            {
                if (match.Groups[2].Value == "%")
                    value = (int)(parentValue * (int.Parse(match.Groups[1].Value) / 100f));
                else if (match.Groups[2].Value == "px")
                    value = int.Parse(match.Groups[1].Value);
            }

            return value;
        }
    }
}
