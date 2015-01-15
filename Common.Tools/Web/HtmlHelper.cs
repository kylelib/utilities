using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using Common.Tools.IO;
using System.Net;

namespace Common.Tools.Web
{
    public partial class HtmlHelper
    {
        #region "Constants"

        private const string UTF8_TO_ASCII_PATH = "/SMART/EmailTemplates/UTF8_to_ASCII.txt";
        private const string UTF8_TO_TEXT_PATH = "/SMART/EmailTemplates/UTF8_to_TEXT.txt";
        private const string HTML_STRIP_REGULAR_EXPRESSION = @"<(.|\n)*?>";

        #endregion

        #region "Public Methods"

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

        /// <summary>
        /// Convert HTML Body into HTML for email.  This function will translate characters to ASCII and will qualify all links
        /// </summary>
        /// <param name="HTMLBody">HTML base from webpage</param>
        /// <returns>HTML code for email</returns>
        public static string CreateHTMLEmailSource(string HTMLBody)
        {
            string strHTMLEmailSource = HTMLBody;

            // translate UTF-8 characters to ASCII equivalent
            strHTMLEmailSource = TranslateUTF8ToASCII(strHTMLEmailSource);

            // fully qualify all paths
            strHTMLEmailSource = FullyQualifyURLs(strHTMLEmailSource);

            return strHTMLEmailSource;
        }

        /// <summary>
        /// Convert HTML Body into Text for email.  This function will translate characters to Text based characters and will qualify all links
        /// </summary>
        /// <param name="TextBody">Text base from webpage</param>
        /// <returns>Text code for email</returns>
        public static string CreateTextEmailSource(string HTMLBody)
        {
            string strHTMLEmailSource = HTMLBody;

            // translate UTF-8 characters to ASCII equivalent
            strHTMLEmailSource = TranslateUTF8ToText(strHTMLEmailSource);

            // fully qualify all paths
            strHTMLEmailSource = FullyQualifyURLs(strHTMLEmailSource);

            //Now translate HTML to Text
            string strTextEmailSource = ConvertHTMLToText(strHTMLEmailSource);

            return strTextEmailSource;
        }

        /// <summary>
        /// Replace all relative URLs with fully qualified equivalents
        /// </summary>
        /// <param name="pHTML"></param>
        /// <returns>string</returns>
        public static string FullyQualifyURLs(string HTMLSource)
        {
            string strDomain = ConfigurationManager.AppSettings["g_Domain"];

            string strAbsoluteCIPURLIndicatorUC = "\"/MEDIA/";
            string strAbsoluteCIPURLIndicator2UC = "'/MEDIA/";

            string strAbsoluteURLIndicatorUC = "\"/SMART/";
            string strAbsoluteURLIndicator2UC = "'/SMART/";

            string strRelativeURLIndicator = "\"../";

            string strReplacementURL = "\"" + strDomain + "/SMART/";
            string strReplacement2URL = "'" + strDomain + "/SMART/";

            string strReplacementCIPURL = "\"" + strDomain + "/Media/";
            string strReplacementCIP2URL = "'" + strDomain + "/Media/";

            string strOutputHTML = HTMLSource;
            strOutputHTML = Regex.Replace(strOutputHTML, strAbsoluteCIPURLIndicatorUC, strReplacementCIPURL, RegexOptions.IgnoreCase);

            strOutputHTML = Regex.Replace(strOutputHTML, strAbsoluteCIPURLIndicator2UC, strReplacementCIP2URL, RegexOptions.IgnoreCase);

            strOutputHTML = Regex.Replace(strOutputHTML, strAbsoluteURLIndicatorUC, strReplacementURL, RegexOptions.IgnoreCase);

            strOutputHTML = Regex.Replace(strOutputHTML, strAbsoluteURLIndicator2UC, strReplacement2URL, RegexOptions.IgnoreCase);

            strOutputHTML = strOutputHTML.Replace(strRelativeURLIndicator, strReplacementURL);

            //The following is needed for relative URLs from Text based emails
            strOutputHTML = strOutputHTML.Replace("(/SMART/", "(" + strDomain + "/SMART/");
            strOutputHTML = strOutputHTML.Replace(" /SMART/", " " + strDomain + "/SMART/");

            return strOutputHTML;
        }

        /// <summary>
        /// This pattern Matches everything found inside html tags;
        /// (.|\n) - > Look or any character or a new line
        /// *?  -> 0 or more occurences, and make a non-greedy search meaning
        /// That the match will stop at the first available '>' it sees, and not at the last one
        /// (if it stopped at the last one we could have overlooked 
        /// nested HTML tags inside a bigger HTML tag..)
        /// </summary>
        /// <param name="pHTML">HTML to be stripped</param>
        /// <returns>string</returns>
        public static string ConvertHTMLToText(string HTMLSource)
        {
            string strTextSource = HTMLSource;

            strTextSource = strTextSource.Replace("Iconoculture - Email To A Friend", string.Empty);
            strTextSource = strTextSource.Replace("  ", string.Empty);
            strTextSource = strTextSource.Replace("\r", string.Empty);
            strTextSource = strTextSource.Replace("\n", string.Empty);
            strTextSource = strTextSource.Replace("\t", string.Empty);
            strTextSource = strTextSource.Replace("&#xD;&#xA;", string.Empty);
            strTextSource = strTextSource.Replace("&#8217;", "'");
            strTextSource = strTextSource.Replace("&#8220;", "\"");
            strTextSource = strTextSource.Replace("&#8221;", "\"");
            strTextSource = strTextSource.Replace("&nbsp;", " ");
            strTextSource = strTextSource.Replace("&amp;", "&");
            strTextSource = strTextSource.Replace("<li>", Environment.NewLine + "- ");
            strTextSource = strTextSource.Replace("<LI>", Environment.NewLine + "- ");
            strTextSource = strTextSource.Replace("<br />", Environment.NewLine);
            strTextSource = strTextSource.Replace("<br/>", Environment.NewLine);
            strTextSource = strTextSource.Replace("<br>", Environment.NewLine);
            strTextSource = strTextSource.Replace("<BR />", Environment.NewLine);
            strTextSource = strTextSource.Replace("<BR/>", Environment.NewLine);
            strTextSource = strTextSource.Replace("<BR>", Environment.NewLine);
            strTextSource = strTextSource.Replace("<DIV", Environment.NewLine + "<DIV");
            strTextSource = strTextSource.Replace("<div", Environment.NewLine + "<div");
            strTextSource = strTextSource.Replace("</DIV>", Environment.NewLine);
            strTextSource = strTextSource.Replace("</div>", Environment.NewLine);

            //Remove the rest of the HTML elements
            strTextSource = Regex.Replace(strTextSource, HTML_STRIP_REGULAR_EXPRESSION, string.Empty);

            //Convert 3 breaks to only 2 breaks (to avoid large spacing)
            strTextSource = strTextSource.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);

            return strTextSource;
        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// Retrieves an array of all UTF-8 to ASCII mapping values (or UTF-8 to Text Mapping values - depending on Template)
        /// </summary>
        /// <param name="pTranslateHTML"></param>
        /// <returns>string[]</returns>
        private static string[] GetTranslationKeys(string RelativeTemplatePath)
        {
            string strTranslationTemplatePath = HttpContext.Current.Server.MapPath(RelativeTemplatePath);

            string strTranslations = FileHelper.ReadFileText(strTranslationTemplatePath);
            strTranslations = strTranslations.Replace(Environment.NewLine, "|");

            string[] aTranslationKeys = strTranslations.Split(new char[] { '|' });

            return aTranslationKeys;
        }


        /// <summary>
        /// Translate all UTF-8 specific characters to their ASCII equivalent
        /// </summary>
        /// <param name="pUTF8Source">UTF-8 Text</param>
        /// <returns>string</returns>
        private static string TranslateUTF8ToASCII(string UTF8SourceText)
        {
            string[] aTranslationKeys = GetTranslationKeys(UTF8_TO_ASCII_PATH);
            string strASCIISourceText = UTF8SourceText;
            string[] aKeyValue = null;

            foreach (string strTranslation in aTranslationKeys)
            {
                aKeyValue = strTranslation.Split(',');
                strASCIISourceText = strASCIISourceText.Replace(aKeyValue[0], aKeyValue[1]);
            }

            return strASCIISourceText;
        }

        /// <summary>
        /// Translate all UTF-8 specific characters to their Text equivalent
        /// </summary>
        /// <param name="pUTF8Source">UTF-8 Text</param>
        /// <returns>string</returns>
        private static string TranslateUTF8ToText(string UTF8SourceText)
        {
            string[] aTranslationKeys = GetTranslationKeys(UTF8_TO_TEXT_PATH);
            string strTextSourceText = UTF8SourceText;
            string[] aKeyValue = null;

            foreach (string strTranslation in aTranslationKeys)
            {
                aKeyValue = strTranslation.Split(',');
                strTextSourceText = strTextSourceText.Replace(aKeyValue[0], aKeyValue[1]);
            }

            return strTextSourceText;
        }
        #endregion


    }
}
