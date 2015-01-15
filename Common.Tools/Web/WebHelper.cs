using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace Common.Tools.Web
{
    public class WebHelper
    {

        #region "Public Methods"

        public static void DownloadFile(System.Web.HttpResponse objResponse, string strFileName, byte[] FileBytes, string strExtension, bool bPromptForDownload)
        {
            string strType = "";

            //Set Content Type for Response
            if (strExtension.LastIndexOf(".") > -1)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf(".") + 1);
            }
            switch (strExtension)
            {
                case "Xml":
                    strType = "text/Xml";
                    break;
                case "htm":
                    strType = "text/HTML";
                    break;
                case "html":
                    strType = "text/HTML";
                    break;
                case "txt":
                    strType = "text/plain";
                    break;
                case "json":
                    strType = "application/json";
                    break;
                case "pdf":
                    strType = "application/pdf";
                    break;
                case "xls":
                    strType = "application/vnd.ms-excel";
                    break;
                case "rtf":
                    strType = "application/rtf";
                    break;
                case "xhtml":
                    strType = "text/xhtml";
                    break;
                case "asp":
                    strType = "text/asp";
                    break;
                //audio file types
                case "wav":
                    strType = "audio/x-wav";
                    break;
                case "mpg":
                    strType = "audio/mpeg";
                    break;
                case "mp3":
                    strType = "audio/mpeg3";
                    break;
                case "wmv":
                    strType = "audio/x-ms-wmv";
                    break;
                //image file types
                case "gif":
                    strType = "image/gif";
                    break;
                case "jpg":
                    strType = "image/jpeg";
                    break;
                case "jpeg":
                    strType = "image/jpeg";
                    break;
                case "png":
                    strType = "image/png";
                    break;
                //video file types
                case "avi":
                    strType = "video/avi";
                    break;
                case "dat":
                    strType = "video/mpeg";
                    break;
                case "mov":
                    strType = "video/quicktime";
                    break;
                case "mpeg":
                    strType = "video/mpeg";
                    break;
                default:
                    //Handle All Other Files
                    strType = "application/octet-stream";
                    break;
            }

            if ((strExtension != null))
            {
                objResponse.Clear();

                objResponse.Buffer = false;

                objResponse.ContentType = strType;

                //ensure when building the file name that all forbidden

                //file name characters are replaced with
                if (bPromptForDownload)
                {
                    objResponse.AddHeader("content-disposition", "attachment; filename=" + strFileName + "." + strExtension);
                }
                else
                {
                    objResponse.AddHeader("content-disposition", "filename=" + strFileName + "." + strExtension);
                }

                objResponse.AddHeader("Content-Length", Convert.ToString(FileBytes.Length));

                objResponse.BinaryWrite(FileBytes);

                objResponse.Flush();

                objResponse.End();
            }
        }

        public static RepeaterItem GetParentRepeaterItem(Control CurrentControl)
        {
            Control objParentControl = CurrentControl.Parent;

            RepeaterItem objParentRepeater = null;

            while (objParentControl as RepeaterItem == null)
            {
                if (objParentControl.Parent != null)
                {
                    objParentControl = objParentControl.Parent;
                }
                else
                {
                    //If we reach here, there is no parent repeater Item.
                    objParentControl = null;
                    break;
                }
            }

            //If we found a parent repeater item, let's return that, otherwise return null
            if (objParentControl as RepeaterItem != null)
            {
                objParentRepeater = (RepeaterItem)objParentControl;
            }

            return objParentRepeater;
        }

        public static RepeaterItem GetGrandParentRepeaterItem(Control CurrentControl)
        {
            Control objParentControl = CurrentControl.Parent;
            return GetParentRepeaterItem(objParentControl);
        }

        public static String GetCookieValue(String cookieName, String valueKey, HttpRequest Request)
        {
            String value = null;
            HttpCookie cookie = Request.Cookies[cookieName];
            if (cookie != null)
            {
                if (valueKey == null)
                {
                    value = cookie.Value;
                }
                else if (cookie.HasKeys)
                {
                    value = cookie.Values[valueKey];
                }

                if (value != null)
                {
                    value = HttpUtility.UrlDecode(value);
                }
            }

            return value;
        }

        public static void SetCookieValue(String cookieName, String key, String value, HttpRequest Request, HttpResponse Response)
        {
            HttpCookie cookie = Request.Cookies[cookieName];
            //If request object doesn't have the cookie either, create a new cookie
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }
            if (String.IsNullOrEmpty(value))
            {
                cookie.Values.Remove(key);
            }
            else
            {
                cookie.Values[key] = HttpUtility.UrlEncode(value);
            }
            Response.Cookies.Add(cookie);
        }

        public static void CheckTreeNodesByValues(TreeNodeCollection ChildNodes, List<Int32> Values)
        {
            foreach (TreeNode objNode in ChildNodes)
            {
                Int32 intValue = 0;
                Int32.TryParse(objNode.Value, out intValue);

                if (Values.Contains(intValue))
                {
                    objNode.Checked = true;
                }

                if (objNode.ChildNodes != null && objNode.ChildNodes.Count > 0)
                {
                    CheckTreeNodesByValues(objNode.ChildNodes, Values);
                }
            }
        }

        public static void SelectCheckListItemsByValues(ListItemCollection ListItems, List<Int32> Values)
        {
            foreach (ListItem objNode in ListItems)
            {
                Int32 intValue = 0;
                Int32.TryParse(objNode.Value, out intValue);

                if (Values.Contains(intValue))
                {
                    objNode.Selected = true;
                }
            }
        }

        public static TreeNode FindTreeNodeByValue(TreeNodeCollection ChildNodes, string Value)
        {
            TreeNode objFoundNode = null;

            foreach (TreeNode objNode in ChildNodes)
            {
                //Check Current Node
                if (objNode.Value == Value)
                {
                    objFoundNode = objNode;
                    break;
                }

                //Check Children
                if (objNode.ChildNodes != null && objNode.ChildNodes.Count > 0)
                {
                    objFoundNode = FindTreeNodeByValue(objNode.ChildNodes, Value);
                    if (objFoundNode != null)
                    {
                        break;
                    }
                }
            }

            return objFoundNode;
        }

        #endregion

        #region "Private Methods"


        #endregion

    }
}
