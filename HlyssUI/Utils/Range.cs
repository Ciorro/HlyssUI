using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Utils
{
    public struct Range
    {
        public int Min;
        public int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"[Range] Min({Min}) Max({Max})";
        }
    }
}
