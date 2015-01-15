using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tools.Numbers
{
    public class MathHelper
    {

        public static bool IsEven(int intValue)
        {
            return ((intValue & 1) == 0);
        }

        public static bool IsOdd(int intValue)
        {
            return ((intValue & 1) == 1);
        }

    }
}
