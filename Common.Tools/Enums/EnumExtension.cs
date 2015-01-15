using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Enums
{
    public static class EnumExtension
    {
        public static IEnumerable<Enum> GetFlags(this Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value) && value.GetHashCode() != 0) //exclude "none" option from enum return (as technically every flag enum contains 0)
                    yield return value;
        }
    }
}
