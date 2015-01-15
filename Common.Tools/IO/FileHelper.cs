using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;

namespace Common.Tools.IO
{
    public partial class FileHelper
    {
        /// <summary>
        /// Method will save file to file system.  It will overwrite existing files at the same path.
        /// </summary>
        /// <param name="FilePath">Fully qualified File path including name and extension</param>
        /// <param name="FileBytes">Bytes which represent the entire file</param>
        public static FileInfo SaveToFileSystem(string FilePath, byte[] FileBytes)
        {
            FileStream objFileStream;
            ImpersonateUser iu = new ImpersonateUser();
            FileInfo objFileInfo;

            bool bOverrideCredentials = false;
            if (ConfigurationManager.AppSettings["OverrideCredentialsForSaveToFileSystem"] == "1")
            {
                bOverrideCredentials = true;
            }

            try
            {
                if (bOverrideCredentials)
                {
                    //Impersonate user so we can write to file system (without giving write access to the website's user)
                    iu.Impersonate(ConfigurationManager.AppSettings["g_PublishDomain"], ConfigurationManager.AppSettings["g_PublishUser"], ConfigurationManager.AppSettings["g_PublishPwd"]);
                }

                objFileStream = new FileStream(FilePath, FileMode.Create);

                objFileStream.Write(FileBytes, 0, FileBytes.Length);

                objFileStream.Close();

                objFileInfo = new FileInfo(FilePath);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Error Saving File to File System <br />" + ex.Message + "<br />" + ex.InnerException);
            }
            finally
            {
                if (bOverrideCredentials)
                {
                    iu.Undo();
                }
            }

            return objFileInfo;
        }

        public static byte[] GetBytesFromURL(string FileURL)
        {
            WebClient objWebClient = new WebClient();
            byte[] aFileBytes = objWebClient.DownloadData(FileURL);

            return aFileBytes;
        }

        public static string GetHTMLFromURL(string FileURL)
        {
            WebClient objWebClient = new WebClient();
            byte[] aFileBytes = objWebClient.DownloadData(FileURL);

            string strHtml = Encoding.ASCII.GetString(aFileBytes);

            return strHtml;
        }

        public static byte[] GetBytesFromUNC(string FileLocation)
        {
            ImpersonateUser iu = new ImpersonateUser();
            byte[] aFileBytes;

            bool bOverrideCredentials = false;
            if (ConfigurationManager.AppSettings["OverrideCredentialsForGetBytesFromUNC"] == "1")
            {
                bOverrideCredentials = true;
            }

            try
            {
                if (bOverrideCredentials)
                {
                    //Impersonate user so we can write to file system (without giving write access to the website's user)
                    iu.Impersonate(ConfigurationManager.AppSettings["g_PublishDomain"], ConfigurationManager.AppSettings["g_PublishUser"], ConfigurationManager.AppSettings["g_PublishPwd"]);
                }

                FileInfo objFileInfo = new FileInfo(FileLocation);
                FileStream objFileStream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read);

                //Set buffer size
                aFileBytes = new byte[objFileStream.Length];

                //read data into buffer (i.e. aFileBytes)
                using (BinaryReader objBinaryReader = new BinaryReader(objFileStream))
                {
                    aFileBytes = objBinaryReader.ReadBytes((int)objFileInfo.Length);
                    objBinaryReader.Close();
                }

                objFileStream.Close();
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Error Reading File from File System <br />" + ex.Message + "<br />" + ex.InnerException);
            }
            finally
            {
                if (bOverrideCredentials)
                {
                    iu.Undo();
                }
            }

            return aFileBytes;
        }

        public static string GetFileExtension(string FilePath)
        {
            string strFileExtension = string.Empty;

            int intExtensionBeginIndex = FilePath.LastIndexOf(".") + 1;

            if (intExtensionBeginIndex > 0)
            {
                strFileExtension = FilePath.Substring(intExtensionBeginIndex).ToLower();
            }

            return strFileExtension;
        }

        /// <summary>
        /// Read the contents of a file
        /// </summary>
        /// <param name="FilePath">Path to file in which to pull out the data</param>
        /// <returns>string</returns>
        public static string ReadFileText(string FilePath)
        {
            StreamReader objStreamReader = null;
            string strFileText = string.Empty;

            try
            {
                objStreamReader = new StreamReader(FilePath, Encoding.UTF8);
                strFileText = objStreamReader.ReadToEnd();
            }
            finally
            {
                objStreamReader.Close();
            }

            return strFileText;
        }

        public static void ForceDownload(string FilePath, HttpResponse objResponse)
        {
            string strType = null;

            string strName = Path.GetFileName(FilePath);
            string strExt = Path.GetExtension(FilePath).ToLower();

            //Set Content Type for Response
            switch (strExt)
            {
                case ".htm":
                case ".html":
                    strType = "text/HTML";
                    break;
                case ".txt":
                    strType = "text/plain";
                    break;
                case ".rtf":
                    strType = "application/rtf";
                    break;
                case ".csv":
                    strType = "Application/x-msexcel";
                    break;
                case ".pdf":
                    strType = "application/pdf";
                    break;
                case ".asf":
                    strType = "video/x-ms-asf";
                    break;
                case ".avi":
                    strType = "video/avi";
                    break;
                case ".doc":
                    strType = "application/msword";
                    break;
                case ".zip":
                    strType = "application/zip";
                    break;
                case ".xls":
                    strType = "application/vnd.ms-excel";
                    break;
                case ".gif":
                    strType = "image/gif";
                    break;
                case ".jpg":
                case "jpeg":
                    strType = "image/jpeg";
                    break;
                case ".wav":
                    strType = "audio/wav";
                    break;
                case ".mp3":
                    strType = "audio/mpeg3";
                    break;
                case ".mpg":
                case "mpeg":
                    strType = "video/mpeg";
                    break;
                case ".asp":
                    strType = "text/asp";
                    break;
                default:
                    //Handle All Other Files
                    strType = "application/octet-stream";
                    break;
            }

            objResponse.ContentType = strType;
            objResponse.AppendHeader("content-disposition", "attachment; filename=" + strName);

            objResponse.TransmitFile(FilePath);
            objResponse.End();
        }

    }
}
