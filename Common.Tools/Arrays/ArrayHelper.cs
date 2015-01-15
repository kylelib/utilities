using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using System.ComponentModel;

namespace Common.Tools.Arrays
{
    public static class ArrayHelper
    {
        public static List<string> ToListOfStrings<InputType>(this List<InputType> inputList)
        {
            List<string> outputList = new List<string>();

            foreach (var item in inputList)
            {
                outputList.Add(item.ToString());
            }
            return outputList;
        }

        public static SortedList<int, int> ToSortedList(this List<int> unsortedListOfInts)
        {
            SortedList<int, int> sortedInts = new SortedList<int, int>();

            foreach (int i in unsortedListOfInts)
            {
                if (!sortedInts.ContainsKey(i))
                    sortedInts.Add(i, i);
            }

            return sortedInts;
        }

        public static SortedList<int, int> ConvertToSortedList(string StringOfIntegers, char ParentSeparator, char ChildSeparator)
        {
            SortedList<int, int> slKeysAndValues = new SortedList<int, int>();

            string[] aKeysAndValues = StringOfIntegers.Trim(ParentSeparator).Split(ParentSeparator);

            foreach (string strKeyAndValue in aKeysAndValues)
            {
                string[] aKeyAndValue = strKeyAndValue.Split(ChildSeparator);
                int intKey = Convert.ToInt32(aKeyAndValue[0]);
                int intValue = Convert.ToInt32(aKeyAndValue[1]);
                slKeysAndValues.Add(intKey, intValue);
            }

            return slKeysAndValues;
        }

        public static List<Int32> ConvertToIntegerArray(ArrayList WeakArray)
        {
            List<Int32> aStrongTypedArray = new List<Int32>();

            foreach (Int32 intCurrent in WeakArray)
            {
                aStrongTypedArray.Add(intCurrent);
            }

            return aStrongTypedArray;
        }

        /// <summary>
        /// [Deprecated] Please try to avoid...Use strongly typed arrays
        /// </summary>
        /// <param name="StringOfIntegers"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static ArrayList ConvertToArrayList<T>(List<T> TypedArray)
        {
            ArrayList aWeakTypedArray = new ArrayList();

            foreach (T obj in TypedArray)
            {
                aWeakTypedArray.Add(obj);
            }

            return aWeakTypedArray;
        }


        public static List<Int32> ConvertToIntegerArray(string StringOfIntegers, char Separator)
        {
            List<Int32> aIntegers = new List<Int32>();

            //let's trim the separator off the beginning and end of the string
            if (!string.IsNullOrEmpty(StringOfIntegers))
            {
                string strPossibleIntegerList = StringOfIntegers.Trim(Separator);
                string[] aPossibleIntegers = strPossibleIntegerList.Split(Separator);

                foreach (string strPossibleInteger in aPossibleIntegers)
                {
                    int intTemp = 0;

                    bool bConversionSuccess = Int32.TryParse(strPossibleInteger, out intTemp);

                    if (bConversionSuccess)
                    {
                        aIntegers.Add(intTemp);
                    }
                    else
                    {
                        //Clear array list as it is not valid and jump out of for loop.
                        aIntegers = null;
                        break;
                    }
                }
            }

            return aIntegers;
        }

        /// <summary>
        /// [Deprecated] Please try to avoid...Use strongly typed arrays
        /// </summary>
        /// <param name="StringOfIntegers"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static ArrayList ConvertToArrayListOfIntegers(string StringOfIntegers, char Separator)
        {
            ArrayList aIntegers = new ArrayList();

            foreach (string strPossibleInteger in StringOfIntegers.Split(Separator))
            {
                int intTemp = 0;

                bool bConversionSuccess = Int32.TryParse(strPossibleInteger, out intTemp);

                if (bConversionSuccess)
                {
                    aIntegers.Add(intTemp);
                }
                else
                {
                    //Clear array list as it is not valid and jump out of for loop.
                    aIntegers = null;
                    break;
                }
            }

            return aIntegers;
        }

        public static string ConvertListToDelimitedString<T>(List<T> list, String delimiter)
        {
            string strDelimitedString = string.Empty;

            foreach (T obj in list)
            {
                strDelimitedString += obj.ToString() + delimiter;
            }

            if (!string.IsNullOrEmpty(strDelimitedString))
            {
                //Trim trailing delimiter
                strDelimitedString = strDelimitedString.Substring(0, strDelimitedString.Length - delimiter.Length);
            }

            return strDelimitedString;
        }

        public static List<int> MergeListsByIntersection(List<int> SourceList, List<int> ComparisonList)
        {
            IEnumerable<int> intersection = SourceList.Intersect(ComparisonList);
            List<int> IntersectionList = intersection.ToList();

            return IntersectionList;
        }

    }
}
