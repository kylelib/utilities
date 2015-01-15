using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools.Dates
{
    public static class DateExtensions
    {
        public static bool IsWeekend(this DateTime dateToCheck)
        {
            return (dateToCheck.DayOfWeek == DayOfWeek.Sunday || dateToCheck.DayOfWeek == DayOfWeek.Saturday);
        }


        public static bool IsWeekDay(this DateTime dateToCheck)
        {
            return !(dateToCheck.DayOfWeek == DayOfWeek.Sunday || dateToCheck.DayOfWeek == DayOfWeek.Saturday);
        }
    }
}
