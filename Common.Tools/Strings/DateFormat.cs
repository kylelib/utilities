using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Common.Tools.Strings
{
    public static class DateFormat
    {
        //Little endian Date Format = '1 January 2010'

        public static string GetEndianDateFormat(string strFormat)
        {
            bool bMonthAbbr = false;
            DateTime dtValidSourceDate = new DateTime();
            string strNewFormat = string.Empty;
            string _month = string.Empty;
            string _day = string.Empty;
            string _year = string.Empty;

            //Matches MONTH YEAR ie "July 2010"
            string re_Month_yyyy = "^(?<Month>JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER)([\\s])(?<Year>\\d{4})$";
            Regex objDatePattern2 = new Regex(re_Month_yyyy, RegexOptions.IgnoreCase);
            if (!objDatePattern2.IsMatch(strFormat))
            {
                bool bDateIsValid = DateTime.TryParse(strFormat, out dtValidSourceDate);
                if (bDateIsValid)
                {
                    strNewFormat = dtValidSourceDate.ToString("yyyy-MM-dd");
                }
            }

            // Match YYYY-MM-DD,
            string pattern = "(?<Year>\\d{4}|\\d{2})([/-])(?<Month>\\d{1,2})([/-])(?<Day>(?:\\d{1,2}))";
            Regex objDatePattern = new Regex(pattern, RegexOptions.None);

            if (objDatePattern.IsMatch(strNewFormat))
            {
                MatchCollection MatchArray = objDatePattern.Matches(strNewFormat);
                Match FirstMatch = MatchArray[0];
                _year = FirstMatch.Groups["Year"].Value;
                _month = FirstMatch.Groups["Month"].Value;
                _day = FirstMatch.Groups["Day"].Value;

                if (bMonthAbbr == true)
                {
                    return _day.TrimStart('0') + " " + getMonthShort(_month.TrimStart('0')) + " " + _year;
                }
                else
                {
                    return _day.TrimStart('0') + " " + getMonthFull(_month.TrimStart('0')) + " " + _year;
                }
            }
            else
            {
                return strFormat;
            }

        }

        public static string GetEndianDateFormat(DateTime dtDate)
        {
            bool bMonthAbbr = false;
            DateTime dtValidSourceDate = new DateTime();
            string strNewFormat = string.Empty;
            string _month = string.Empty;
            string _day = string.Empty;
            string _year = string.Empty;

            bool bDateIsValid = DateTime.TryParse(dtDate.ToString("yyyy-MM-dd"), out dtValidSourceDate);
            if (bDateIsValid)
            {
                strNewFormat = dtValidSourceDate.ToString("yyyy-MM-dd");
            }

            // Match YYYY-MM-DD,
            string pattern = "(?<Year>\\d{4}|\\d{2})([/-])(?<Month>\\d{1,2})([/-])(?<Day>(?:\\d{1,2}))";
            Regex objDatePattern = new Regex(pattern, RegexOptions.None);

            if (objDatePattern.IsMatch(strNewFormat))
            {
                MatchCollection MatchArray = objDatePattern.Matches(strNewFormat);
                Match FirstMatch = MatchArray[0];
                _year = FirstMatch.Groups["Year"].Value;
                _month = FirstMatch.Groups["Month"].Value;
                _day = FirstMatch.Groups["Day"].Value;

                if (bMonthAbbr == true)
                {
                    return _day.TrimStart('0') + " " + getMonthShort(_month.TrimStart('0')) + " " + _year;
                }
                else
                {
                    return _day.TrimStart('0') + " " + getMonthFull(_month.TrimStart('0')) + " " + _year;
                }
            }
            else
            {
                return dtDate.ToString("yyyy-MM-dd");
            }

        }

        public static string GetEndianDateFormat(string strFormat,bool bMonthAbbr) 
        {
            DateTime dtValidSourceDate = new DateTime();
            string strNewFormat = string.Empty;
            string _month = string.Empty;
            string _day = string.Empty;
            string _year = string.Empty;

            bool bDateIsValid = DateTime.TryParse(strFormat, out dtValidSourceDate);
            if (bDateIsValid)
            {
                strNewFormat = dtValidSourceDate.ToString("yyyy-MM-dd");
            }

            // Match YYYY-MM-DD,
            string pattern = "(?<Year>\\d{4}|\\d{2})([/-])(?<Month>\\d{1,2})([/-])(?<Day>(?:\\d{1,2}))";
            Regex objDatePattern = new Regex(pattern, RegexOptions.None);

            if (objDatePattern.IsMatch(strNewFormat))
            {
                MatchCollection MatchArray = objDatePattern.Matches(strNewFormat);
                Match FirstMatch = MatchArray[0];
                _year = FirstMatch.Groups["Year"].Value;
                _month = FirstMatch.Groups["Month"].Value;
                _day = FirstMatch.Groups["Day"].Value;

                if (bMonthAbbr == true)
                {
                    return _day.TrimStart('0') + " " + getMonthShort(_month.TrimStart('0')) + " " + _year;
                }
                else
                {
                    return _day.TrimStart('0') + " " + getMonthFull(_month.TrimStart('0')) + " " + _year;
                }
            }
            else
            {
                return strFormat;
            }
            
        }

        public static string FormatMarketFactSourceDates(string strDate)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            string strFormatted = strDate;

            //REGULAR EXPRESSION MATCHES

            //Matches 3.24.02 => YYYY-MM-DD
            string re_mm_dd_yy = "^(?<Month>\\d{1,2})([./-])(?<Day>\\d{1,2})([./-])(?<Year>(?:\\d{1,2}))$";
            Regex objDatePattern1 = new Regex(re_mm_dd_yy, RegexOptions.None);
            if (objDatePattern1.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern1.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _year = FirstMatch.Groups["Year"].Value;
                string _month = FirstMatch.Groups["Month"].Value;
                string _day = FirstMatch.Groups["Day"].Value;

                Regex objIntPattern = new Regex("^\\d{1}$");
                if(objIntPattern.IsMatch(_month))
                {
                    _month = "0" + _month;
                };

                strFormatted = getYearFull(_year) + "-" + _month + "-" + _day;
            }

            //Matches SPRING 02 => Spring 2002
            string re_Season_yy = "^(?<Season>SPRING|SUMMER|FALL|WINTER)([\\s])(?<Year>\\d{1,2})$";
            Regex objDatePattern2 = new Regex(re_Season_yy, RegexOptions.None);
            if (objDatePattern2.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern2.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _season = myTI.ToLower(FirstMatch.Groups["Season"].Value);
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = myTI.ToTitleCase(_season) + " " + getYearFull(_year);
            }

            //Matches 4.02 => April 2002
            string re_m_yy = "^(?<Month>\\d{1,2})([.])(?<Year>\\d{1,2})$";
            Regex objDatePattern3 = new Regex(re_m_yy, RegexOptions.None);
            if (objDatePattern3.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern3.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _month = FirstMatch.Groups["Month"].Value;
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = getMonthFull(_month.TrimStart('0')) + " " + getYearFull(_year);
            }

            //Matches 1/2.02 => January/February 2002
            string re_M_M_yyyy = "^(?<Month>\\d{1,2})([/])(?<Month2>\\d{1,2})([.])(?<Year>\\d{1,2})$";
            Regex objDatePattern4 = new Regex(re_M_M_yyyy, RegexOptions.None);
            if (objDatePattern4.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern4.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _month = FirstMatch.Groups["Month"].Value;
                string _month2 = FirstMatch.Groups["Month2"].Value;
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = getMonthFull(_month.TrimStart('0')) + "/" + getMonthFull(_month2.TrimStart('0')) + " " + getYearFull(_year);
            }

            //Matches 2001 => 2001
            string re_yyyy = "^(?<Year>\\d{4})$";
            Regex objDatePattern5 = new Regex(re_yyyy, RegexOptions.None);
            if (objDatePattern5.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern5.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = _year;
            }

            //Matches 6.28-7.4.02 => 28 June-4 July 2002
            string re_m_d_m_d_y = "^(?<Month>\\d{1,2})([.])(?<Day>\\d{1,2})([-])(?<Month2>\\d{1,2})([.])(?<Day2>\\d{1,2})([.])(?<Year>\\d{1,2})$";
            Regex objDatePattern6 = new Regex(re_m_d_m_d_y, RegexOptions.None);
            if (objDatePattern6.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern6.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _month = FirstMatch.Groups["Month"].Value;
                string _month2 = FirstMatch.Groups["Month2"].Value;
                string _day = FirstMatch.Groups["Day"].Value;
                string _day2 = FirstMatch.Groups["Day2"].Value;
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = _day.TrimStart('0') + " " + getMonthFull(_month.TrimStart('0')) + "-" + _day2 + " " + getMonthFull(_day2.TrimStart('0')) + " " + getYearFull(_year);
            }


            //Matches 12.01/1.02 => December 2001/January 2002
            string re_mm_yy_mm_yy = "^(?<Month>\\d{1,2})([.])(?<Year>\\d{1,2})([/])(?<Month2>\\d{1,2})([.])(?<Year2>\\d{1,2})$";
            Regex objDatePattern7 = new Regex(re_mm_yy_mm_yy, RegexOptions.None);
            if (objDatePattern7.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern7.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _month = FirstMatch.Groups["Month"].Value;
                string _month2 = FirstMatch.Groups["Month2"].Value;
                string _year = FirstMatch.Groups["Year"].Value;
                string _year2 = FirstMatch.Groups["Year2"].Value;

                strFormatted = getMonthFull(_month.TrimStart('0')) + " " + getYearFull(_year) + "/" + getMonthFull(_month2.TrimStart('0')) + " " + getYearFull(_year);
            }

            //Matches WINTER/SPRING 2002 => Winter/Spring 2002
            string re_season_season_year = "^(?<Season1>SPRING|SUMMER|FALL|WINTER)([/])(?<Season2>SPRING|SUMMER|FALL|WINTER)([\\s])(?<Year>\\d{4})$";
            Regex objDatePattern8 = new Regex(re_season_season_year, RegexOptions.None);
            if (objDatePattern8.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern8.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _season = myTI.ToLower(FirstMatch.Groups["Season1"].Value);
                string _season2 = myTI.ToLower(FirstMatch.Groups["Season2"].Value);
                string _year = FirstMatch.Groups["Year"].Value;

                strFormatted = myTI.ToTitleCase(_season) + "/" + myTI.ToTitleCase(_season2) + " " + _year;
            }

            //Matches WINTER 01-02 => Winter 2001-2002
            string re_season_year_year = "^(?<Season>SPRING|SUMMER|FALL|WINTER)([\\s])(?<Year>\\d{1,2})([-])(?<Year2>\\d{1,2})$";
            Regex objDatePattern9 = new Regex(re_season_year_year, RegexOptions.None);
            if (objDatePattern9.IsMatch(strDate))
            {
                MatchCollection MatchArray = objDatePattern9.Matches(strDate);
                Match FirstMatch = MatchArray[0];
                string _season = myTI.ToLower(FirstMatch.Groups["Season"].Value);
                string _year = FirstMatch.Groups["Year"].Value;
                string _year2 = FirstMatch.Groups["Year2"].Value;

                strFormatted = myTI.ToTitleCase(_season) + " " + getYearFull(_year) + "-" + getYearFull(_year2);
            }
            
            return strFormatted;
        }

        private static string getMonthFull(string month)
        {
            string strFullMonth = string.Empty;

            switch (month)
            {
                case "1":
                    strFullMonth = "January";
                    break;
                case "2":
                    strFullMonth = "February";
                    break;
                case "3":
                    strFullMonth = "March";
                    break;
                case "4":
                    strFullMonth = "April";
                    break;
                case "5":
                    strFullMonth = "May";
                    break;
                case "6":
                    strFullMonth = "June";
                    break;
                case "7":
                    strFullMonth = "July";
                    break;
                case "8":
                    strFullMonth = "August";
                    break;
                case "9":
                    strFullMonth = "September";
                    break;
                case "10":
                    strFullMonth = "October";
                    break;
                case "11":
                    strFullMonth = "November";
                    break;
                case "12":
                    strFullMonth = "December";
                    break;
            }

            return strFullMonth;
        }

        private static string getYearFull(string year)
        {
            string strFullYear = string.Empty;
            string re_shortYear = "^(?<Year>[0-6][0-9]{1,2})$";
            Regex objDatePattern = new Regex(re_shortYear, RegexOptions.None);
            if (objDatePattern.IsMatch(year))
            {
                MatchCollection MatchArray = objDatePattern.Matches(year);
                Match FirstMatch = MatchArray[0];
                string _year = FirstMatch.Groups["Year"].Value;

                return strFullYear = "20" + _year;
            }
            else
            {
                return strFullYear = "19" + year;
            }
        }

        private static string getMonthShort(string month)
        {
            string strFullMonth = string.Empty;

            switch (month)
            {
                case "1":
                    strFullMonth = "Jan";
                    break;
                case "2":
                    strFullMonth = "Feb";
                    break;
                case "3":
                    strFullMonth = "Mar";
                    break;
                case "4":
                    strFullMonth = "Apr";
                    break;
                case "5":
                    strFullMonth = "May";
                    break;
                case "6":
                    strFullMonth = "Jun";
                    break;
                case "7":
                    strFullMonth = "Jul";
                    break;
                case "8":
                    strFullMonth = "Aug";
                    break;
                case "9":
                    strFullMonth = "Sep";
                    break;
                case "10":
                    strFullMonth = "Oct";
                    break;
                case "11":
                    strFullMonth = "Nov";
                    break;
                case "12":
                    strFullMonth = "Dec";
                    break;
            }

            return strFullMonth;
        }
    }
}
