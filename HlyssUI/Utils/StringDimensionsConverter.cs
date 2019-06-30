﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HlyssUI.Utils
{
    static class StringDimensionsConverter
    {
        static Regex regex = new Regex(@"^(\d+)(%|px|vw|vh)$");

        public static int Convert(string inputValue, int parentValue)
        {
            Match match = regex.Match(inputValue);
            int value = 0;

            if(match.Success)
            {
                if(match.Groups[2].Value == "%")
                    value = (int)(parentValue * (int.Parse(match.Groups[1].Value) / 100f));
                else if(match.Groups[2].Value == "px")
                    value = int.Parse(match.Groups[1].Value);
            }

            return value;
        }
    }
}