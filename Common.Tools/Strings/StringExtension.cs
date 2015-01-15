using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Common.Tools.Strings
{
    public static class StringExtension
    {

        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        public static T ToNonNullable<T>(this string s) where T : struct
        {
            T result = new T();

            //Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        public static bool HasValue(this string s)
        {
            bool stringHasValue = !string.IsNullOrEmpty(s);

            return stringHasValue;
        }

        public static bool HasNoValue(this string s)
        {
            return !HasValue(s);
        }


        public static string TruncateOnSpace(this string val, int charCount, string appendTrailingCharacters = "...")
        {
            if (val != null && val.LastIndexOf(" ") > 0 && val.Length > charCount)
                val = val.Substring(0, val.Substring(0, charCount).LastIndexOf(" ")) + appendTrailingCharacters;

            return val;
        }


        public static bool IsValidInteger(this string myString)
        {
            int intValidInteger = 0;
            return int.TryParse(myString, out intValidInteger);
        }

        public static string ToPlural(this string myString)
        {
            string pluralName = string.Empty;
            //Change plural to be "ies" for "y"
            if (myString.EndsWith("y"))
                pluralName = myString.Substring(0, myString.Length - 1) + "ies";
            else if (myString.EndsWith("ss") || myString.EndsWith("x") || myString.EndsWith("z") || myString.EndsWith("ch") || myString.EndsWith("sh"))
                pluralName = myString + "es";
            else if (!myString.EndsWith("s"))
                pluralName = myString + "s";
            else
                pluralName = myString; //we can assume it is already plural if we reach here.

            return pluralName;
        }

        public static string ToAlphaNumeric(this string myString, bool allowSpaces)
        {
            string replacementString = string.Empty;

            string pattern = "[^a-zA-Z0-9]";

            if (allowSpaces)
                pattern = "[^a-zA-Z0-9 ]";

            Regex rgx = new Regex(pattern);
            replacementString = rgx.Replace(myString, "");

            return replacementString;
        }

        public static bool EndsWith(this StringBuilder sb, string match, StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (sb.Length < match.Length)
                return false;

            string end = sb.ToString(sb.Length - match.Length, match.Length);
            return end.Equals(match, comparison);
        }
    }
}
