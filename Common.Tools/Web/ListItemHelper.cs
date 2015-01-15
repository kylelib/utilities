using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web.UI.WebControls;
using Common.Tools.Strings;

namespace Common.Tools.Web
{
    public class ListItemHelper
    {

        #region "Public Methods"

        public static ListItemCollection CreateListCollectionOfNumbers(int Low, int High)
        {
            ListItemCollection colNumbersList = new ListItemCollection();

            for (int i = Low; i <= High; i++)
            {
                string strNumber = i.ToString();
                ListItem objNumberItem = new ListItem(strNumber, strNumber);
                colNumbersList.Add(objNumberItem);
            }

            return colNumbersList;
        }


        public static ListItemCollection BuildListFromEnum(Enum objEnum)
        {
            ListItemCollection colListItems = new ListItemCollection();
            ListItem liItem;

            SortedList colSortedListItems = new SortedList();

            foreach (int value in Enum.GetValues(objEnum.GetType()))
            {
                liItem = new ListItem();

                liItem.Value = value.ToString();
                liItem.Text = StringEnum.GetStringValue((Enum)Enum.Parse(objEnum.GetType(), value.ToString(), true));

                if (liItem.Text != string.Empty)
                {
                    colSortedListItems.Add(liItem.Text, liItem);
                }
                liItem = null;
            }

            foreach (ListItem liListItem in colSortedListItems.GetValueList())
            {
                colListItems.Add(liListItem);
            }

            return colListItems;
        }

        public static ListItemCollection BuildListFromEnumByName(Enum objEnum)
        {
            ListItemCollection colListItems = new ListItemCollection();
            ListItem liItem;

            SortedList colSortedListItems = new SortedList();

            foreach (string strName in Enum.GetNames(objEnum.GetType()))
            {
                liItem = new ListItem();

                liItem.Value = strName;
                liItem.Text = strName;

                colSortedListItems.Add(liItem.Text, liItem);
            }

            foreach (ListItem liListItem in colSortedListItems.GetValueList())
            {
                colListItems.Add(liListItem);
            }

            return colListItems;
        }

        #endregion

    }
}
