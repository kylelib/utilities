using System;
using System.Collections;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Common.Tools.Strings
{

    #region Class StringEnum

    /// <summary>
    /// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
    /// </summary>
    public class StringEnum
    {
        public static string GetStringValue<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (StringValueAttribute[])fi.GetCustomAttributes(typeof(StringValueAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Value : value.ToString();
        }

        public static string GetDisplayName<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DisplayNameAttribute[])fi.GetCustomAttributes(typeof(DisplayNameAttribute), false);

            return (attributes.Length > 0) ? attributes[0].DisplayName : value.ToString();
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return Parse(enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return Parse(enumType, stringValue, ignoreCase) != null;
        }


        public static ListItemCollection BuildItemsFromEnum(Enum objEnum, bool Sort = true)
        {
            ListItemCollection colListItems = new ListItemCollection();
            ListItem liItem;

            //System.Collections.Generic.Dictionary<String, String> colUnsortedListItems = new System.Collections.Generic.Dictionary<string, string>();
            SortedList colSortedListItems = new SortedList();

            foreach (int value in Enum.GetValues(objEnum.GetType()))
            {
                liItem = new ListItem();

                liItem.Value = value.ToString();
                liItem.Text = StringEnum.GetStringValue((Enum)Enum.Parse(objEnum.GetType(), value.ToString(), true));

                if (liItem.Text != string.Empty)
                {
                    if (Sort == true) colSortedListItems.Add(liItem.Text, liItem);
                    else colSortedListItems.Add(value, liItem);
                }
                liItem = null;
            }

            if (colSortedListItems.Count > 0)
            {
                foreach (ListItem liListItem in colSortedListItems.GetValueList())
                {
                    colListItems.Add(liListItem);
                }
            }

            return colListItems;
        }

        public static ListItem ConvertIntBasedEnumToListItem(Enum EnumItem)
        {
            string strText = GetStringValue(EnumItem);
            int intValue = Convert.ToInt32(EnumItem);

            ListItem objListItem = new ListItem(strText, intValue.ToString());

            return objListItem;
        }

    }
    #endregion


    #region "Custom Enum Attribute Classes"

    /// <summary>
    /// Simple attribute class for storing String Values
    /// </summary>
    public class DisplayNameAttribute : Attribute
    {
        private string _displayName;

        /// <summary>
        /// Creates a new <see cref="DisplayNameAttribute"/> instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public DisplayNameAttribute(string displayName)
        {
            _displayName = displayName;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value></value>
        public string DisplayName
        {
            get { return _displayName; }
        }
    }

    /// <summary>
    /// Simple attribute class for storing String Values
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        private string _value;

        /// <summary>
        /// Creates a new <see cref="StringValueAttribute"/> instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value></value>
        public string Value
        {
            get { return _value; }
        }
    }

    #endregion

}