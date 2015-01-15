using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Numbers
{
    public static class NumberExtensions
    {
        public static int LimitSizeToInt32(this long number)
        {
            return number > Int32.MaxValue ? Int32.MaxValue : (int)number;
        }

        public static List<int> ToList(this int i)
        {
            List<int> ids = new List<int>();
            ids.Add(i);

            return ids;
        }

    }
}
