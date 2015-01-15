using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools.Dates
{
    public class DateHelper
    {
        /// <summary>
        /// Takes a string and parse it into a date
        /// </summary>
        /// <param name="potentialDate">string representing a date</param>
        /// <param name="defaultDateOffsetIfNotValid">if string cannot be parsed, date will default to "today" +/- offset</param>
        /// <returns></returns>
        public static DateTime GetDateFromString(string potentialDate, int defaultDateOffsetIfNotValid = 0)
        {
            DateTime parsedDate = DateTime.MinValue;

            switch (potentialDate.ToLower())
            {
                case null:
                case "":
                case "tomorrow":
                    //If no parameter specified, then we assume we grab wireRequestDate equal to tomorrow
                    parsedDate = DateTime.Now.AddDays(1).Date;
                    break;

                case "today":
                    parsedDate = DateTime.Now.Date;
                    break;

                case "yesterday":
                    parsedDate = DateTime.Now.AddDays(-1).Date;
                    break;

                default:

                    //The following line will set wireRequestDate (if parameter is a valid date)
                    if (!DateTime.TryParse(potentialDate, out parsedDate))
                    {
                        parsedDate = DateTime.Now.Date.AddDays(defaultDateOffsetIfNotValid);
                    }
                    break;
            }

            return parsedDate;
        }
    }
}
