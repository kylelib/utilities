using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Common.Tools.Strings
{
    public static partial class StringHelper
    {
        public static int GetWordCount(string TextBlock)
        {
            int intWordCount = 0;

            if (!string.IsNullOrEmpty(TextBlock))
            {
                string strStrippedTextBlock = StringHelper.RemoveHtmlTags(TextBlock);

                //Replace all special characters that should count as a break between words
                strStrippedTextBlock = strStrippedTextBlock.Replace("&nbsp;", " ");
                strStrippedTextBlock = strStrippedTextBlock.Replace(';', ' ');
                strStrippedTextBlock = strStrippedTextBlock.Replace(':', ' ');
                strStrippedTextBlock = strStrippedTextBlock.Replace('-', ' ');
                strStrippedTextBlock = strStrippedTextBlock.Replace('.', ' ');
                strStrippedTextBlock = strStrippedTextBlock.Replace('?', ' ');
                strStrippedTextBlock = strStrippedTextBlock.Replace('!', ' ');

                //then replace all double (or triple) spaces with single space
                while (strStrippedTextBlock.Contains("  "))
                {
                    strStrippedTextBlock = strStrippedTextBlock.Replace("  ", " ");
                }
                strStrippedTextBlock = strStrippedTextBlock.Trim();

                string[] aWords = strStrippedTextBlock.Split(' ');
                intWordCount = aWords.Length;
            }

            return intWordCount;
        }

        public static string RemoveHtmlTags(string TextWithHtml)
        {
            string pattern = @"<(.|\n)*?>";

            string strTextWithoutHtml = Regex.Replace(TextWithHtml, pattern, " ");

            return strTextWithoutHtml;
        }

        public static bool IsValidDate(string PotentialDate)
        {
            bool bIsValidDate = false;

            DateTime dtValidDate = DateTime.MinValue;

            bIsValidDate = DateTime.TryParse(PotentialDate, out dtValidDate);

            return bIsValidDate;
        }

        public static bool IsValidInteger(string PotentialInteger)
        {
            bool bIsValidInteger = true;

            int intValidInteger = 0;

            bIsValidInteger = int.TryParse(PotentialInteger, out intValidInteger);

            return bIsValidInteger;
        }


        public static ArrayList ConvertToArrayListOfIntegers(string CSVString)
        {

            ArrayList aIDs = new ArrayList();

            foreach (string strID in CSVString.Split(','))
            {
                int intID = 0;

                Int32.TryParse(strID, out intID);

                if (intID != 0)
                {
                    aIDs.Add(intID);
                }
                else
                {
                    //Clear array list as it is not valid and jump out of for loop.
                    aIDs = null;
                    break;
                }
            }

            return aIDs;
        }

        public static ArrayList ConvertToArrayListOfTagIDs(string CSVString)
        {

            ArrayList aIDs = new ArrayList();

            foreach (string strID in CSVString.Split(','))
            {
                int intID = 0;

                Int32.TryParse(strID, out intID);

                if (intID != 0)
                {
                    aIDs.Add(intID);
                }
                else
                {
                    //Clear array list as it is not valid and jump out of for loop.
                    aIDs = null;
                    break;
                }
            }

            return aIDs;
        }

        public static int[] StringToInts(string myString)
        {
            List<int> ints = new List<int>();
            string[] strings = myString.Split(',');

            foreach (string s in strings)
            {
                int i;
                if (int.TryParse(s.Trim(), out i))
                {
                    ints.Add(i);
                }
            }
            return ints.ToArray();
        }


        public static string StripNonAlphaNumericCharacters(string Phrase)
        {
            string strReturnValue = string.Empty;

            string strFormattedString = string.Empty;

            if (!string.IsNullOrEmpty(Phrase))
            {
                string strAlphaNumeric = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                for (int i = 0; i < Phrase.Length; i++)
                {
                    string strChar = Phrase.Substring(i, 1);
                    if (strAlphaNumeric.Contains(strChar))
                    {
                        strFormattedString = strFormattedString + strChar;
                    }
                }

            }
            return strFormattedString;

        }

        public static int CountOccurencesOfChar(string Instance, char CharacterToFind)
        {
            int intResult = 0;
            foreach (char cCurrent in Instance)
            {
                if (cCurrent == CharacterToFind)
                {
                    intResult++;
                }
            }
            return intResult;
        }

        public static string EncodeBase64(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public static string DecodeBase64(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        public static string GetTeaser(string strTeaserBase, int intTeaserLength)
        {
            string strTeaserContent;
            string[] aTeaser;
            string[] aSeparator;
            string strTeaser;

            strTeaser = string.Empty;

            if (!string.IsNullOrEmpty(strTeaserBase))
            {
                //clean up the HTML to remove elements not included in the web.config "g_HTMLTagsToPreserveTeaserOnly" setting
                strTeaserContent = CleanUpHTMLFormattingForTeaser(strTeaserBase);
                //set unique place holders so the teaser can be split into each
                //word and stored in a character array
                if ((strTeaserContent.Length > intTeaserLength) ||
                    (strTeaserContent.Length > Convert.ToInt32(ConfigurationManager.AppSettings["g_TeaserLength"])))
                {
                    strTeaserContent = strTeaserContent.Replace("<", "~#<");
                    strTeaserContent = strTeaserContent.Replace(">", ">~#");
                    strTeaserContent = strTeaserContent.Replace(" ", "~# ");
                    aSeparator = new string[1] { "~#" };
                    aTeaser = strTeaserContent.Split(aSeparator, StringSplitOptions.None);

                    if (intTeaserLength > 0)
                    {
                        strTeaser = BuildTeaser(aTeaser, Convert.ToInt32(intTeaserLength));
                    }
                    else
                    {
                        strTeaser = BuildTeaser(aTeaser, Convert.ToInt32(ConfigurationManager.AppSettings["g_TeaserLength"]));
                    }
                }
                else
                {
                    strTeaser = strTeaserContent;
                }
            }


            return strTeaser;

        }

        public static string BuildTeaser(string[] aTeaser, int iTeaserLength)
        {
            bool bInHTML = false;
            StringBuilder strTeaser= new StringBuilder();
            int iIndex= 0;
            bool bTeaserTruncated = false;

            foreach (string strToken in aTeaser)
            {
                //check to see if this character starts a HTML element
                if (strToken.StartsWith("<"))
                {
                    bInHTML = true;
                }

                if (bInHTML)
                {
                    strTeaser.Append(strToken);  //but do not count against size
                }
                else
                {
                    //We are not in HTML so we can just process the token as part of the teaser
                    if (iIndex < iTeaserLength) //add to teaser if not at max teaser length
                    {
                        strTeaser.Append(strToken);
                        iIndex += strToken.Length; // Add to index to keep track of size
                    }
                    else
                    {
                        bTeaserTruncated = true;  // If we get here, we have passed the max size for teaser
                    }
                }
                
                if (strToken.EndsWith(">"))
                {
                    bInHTML = false;
                }
            }

            string strCompleteTeaser = string.Empty;

            strCompleteTeaser = strTeaser.ToString();

            if (bTeaserTruncated)
            {
                strCompleteTeaser = strCompleteTeaser.Trim();
                char[] arrStripChars = { ',', ';', ':', '.', '-', '&' };
                strCompleteTeaser = strCompleteTeaser.TrimEnd(arrStripChars);
                strCompleteTeaser = strCompleteTeaser.Trim();
                strCompleteTeaser += "...";
            }

            return strCompleteTeaser;
        }

        public static string CleanUpHTMLFormatting(string strValue)
        {
            string strFormattedValue;
            string[] aHTMLTagsToPreserve;
            string[] aHTMLTagsToPreserveWithAttributes;

            //We always remove leading and trailing spaces...
            strFormattedValue = strValue.Trim();

            //Only clean up HTML if the html opening tags match the closing tags!  Otherwise, we tend to lose data
            //Also, only clean up HTML if there are HTML tags
            if (strFormattedValue.Split('<').Length == strFormattedValue.Split('>').Length && strFormattedValue.IndexOf("<") > -1)
            {
                //we need to process some html
                //Replace ending paragraphs with 2 break lines, this is to maintain some spacing...
                strFormattedValue = Regex.Replace(strFormattedValue, "</p>", "<br /><br />");

                //Remove all paragraph tags as they just screw up spacing
                strFormattedValue = Regex.Replace(strFormattedValue, "<p>", String.Empty);
                strFormattedValue = Regex.Replace(strFormattedValue, "<p\\s.*?>", String.Empty);
                //Trim any extra spaces that may remain
                strFormattedValue = strFormattedValue.Trim();

                //remove all strings with break tags
                while (strFormattedValue.EndsWith("<br />"))
                {
                    strFormattedValue = strFormattedValue.Substring(0, strFormattedValue.Length - "<br />".Length).Trim();
                }

                //Replace ending paragraphs with a break line, this is to maintain some spacing...
                strFormattedValue = Regex.Replace(strFormattedValue, "</p>", "<br />");

                //set tags that need to be maintained, just add to the list for other tags you wish to maintain
                aHTMLTagsToPreserve = ConfigurationManager.AppSettings["g_HTMLTagsToPreserve"].Split(',');

                //Create place holders for those values we wish to save
                foreach (string strHTMLTag in aHTMLTagsToPreserve)
                {
                    strFormattedValue = ReplaceTagsWithPlaceHolders(strFormattedValue, strHTMLTag);
                }

                //Replace all HTML tags (tags that needed to be preserved were modified to not be affected by the following expression.
                strFormattedValue = Regex.Replace(strFormattedValue, "<.*?>", String.Empty);

                //replace placeholder for tags 
                strFormattedValue = ReplacePlaceHoldersWithTags(strFormattedValue);

                aHTMLTagsToPreserveWithAttributes = ConfigurationManager.AppSettings["g_HTMLTagsToPreserveWithAttributes"].Split(',');
                //Most tags should not maintain attributes (otherwise styles can greatly change), therefore let's remove attributes of all tags we preserved.
                foreach (string strHTMLTag in aHTMLTagsToPreserve)
                {
                    //anchor should maintain attributes
                    if (!IsHTMLTagInList(strHTMLTag, aHTMLTagsToPreserveWithAttributes))
                    {
                        strFormattedValue = RemoveTagAttributes(strFormattedValue, strHTMLTag);
                    }
                }

                //we must qualify all www links
                strFormattedValue = Regex.Replace(strFormattedValue, "href=\"www", "href=\"http://www");
                strFormattedValue = Regex.Replace(strFormattedValue, "href='www", "href='http://www");
            }

            return strFormattedValue;
        }

        public static string CleanUpHTMLFormattingForTeaser(string strValue)
        {
            string strFormattedValue;
            string[] aHTMLTagsToPreserve;
            string[] aHTMLTagsToPreserveWithAttributes;

            //We always remove leading and trailing spaces...
            strFormattedValue = strValue.Trim();

            //replace bullets with space
            strFormattedValue = Regex.Replace(strFormattedValue, "<li>", " ");
            strFormattedValue = Regex.Replace(strFormattedValue, "</li>", "");
            strFormattedValue = Regex.Replace(strFormattedValue, "&lt;li&gt;", " ");
            strFormattedValue = Regex.Replace(strFormattedValue, "&lt;/li&gt;", "");

            if (strFormattedValue.IndexOf("<") > -1)
            {
                //we need to process some html 
                //Replace ending paragraphs with 2 break lines, this is to maintain some spacing...
                strFormattedValue = Regex.Replace(strFormattedValue, "</p>", "<br /><br />");

                //Remove all paragraph tags as they just screw up spacing
                strFormattedValue = Regex.Replace(strFormattedValue, "<p>", String.Empty);
                strFormattedValue = Regex.Replace(strFormattedValue, "<p\\s.*?>", String.Empty);
                //Trim any extra spaces that may remain
                strFormattedValue = strFormattedValue.Trim();

                //remove all strings with break tags
                while (strFormattedValue.EndsWith("<br />"))
                {
                    strFormattedValue = strFormattedValue.Substring(0, strFormattedValue.Length - "<br />".Length).Trim();
                }

                //Replace ending paragraphs with a break line, this is to maintain some spacing...
                strFormattedValue = Regex.Replace(strFormattedValue, "</p>", "<br />");

                //set tags that need to be maintained, just add to the list for other tags you wish to maintain
                aHTMLTagsToPreserve = ConfigurationManager.AppSettings["g_HTMLTagsToPreserveTeaserOnly"].Split(',');

                //Create place holders for those values we wish to save
                foreach (string strHTMLTag in aHTMLTagsToPreserve)
                {
                    strFormattedValue = ReplaceTagsWithPlaceHolders(strFormattedValue, strHTMLTag);
                }

                //Replace all HTML tags (tags that needed to be preserved were modified to not be affected by the following expression.
                strFormattedValue = Regex.Replace(strFormattedValue, "<.*?>", String.Empty);

                //replace placeholder for tags 
                strFormattedValue = ReplacePlaceHoldersWithTags(strFormattedValue);

                aHTMLTagsToPreserveWithAttributes = ConfigurationManager.AppSettings["g_HTMLTagsToPreserveWithAttributes"].Split(',');
                //Most tags should not maintain attributes (otherwise styles can greatly change), therefore let's remove attributes of all tags we preserved.
                foreach (string strHTMLTag in aHTMLTagsToPreserve)
                {
                    //anchor should maintain attributes
                    if (!IsHTMLTagInList(strHTMLTag, aHTMLTagsToPreserveWithAttributes))
                    {
                        strFormattedValue = RemoveTagAttributes(strFormattedValue, strHTMLTag);
                    }
                }

                //we must qualify all www links
                strFormattedValue = Regex.Replace(strFormattedValue, "href=\"www", "href=\"http://www");
                strFormattedValue = Regex.Replace(strFormattedValue, "href='www", "href='http://www");
            }

            return strFormattedValue;
        }

        private static string ReplaceTagsWithPlaceHolders(string strValue, string strHTMLTagName)
        {
            const string cPlaceHolderOpenTag = "%[%";
            const string cPlaceHolderCloseTag = "%]%";
            string strFormattedValue;
            MatchCollection colMatches;
            string strPattern;

            strFormattedValue = strValue;
            //replace complete tags (e.g. <a href="www.yahoo.com" > or <img source="/asdf.gif" />)
            strPattern = "<(" + strHTMLTagName + "/)>";
            colMatches = Regex.Matches(strFormattedValue, strPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match objMatch in colMatches)
            {
                strFormattedValue = strFormattedValue.Replace(objMatch.Value, cPlaceHolderOpenTag + objMatch.Groups[1].ToString() + cPlaceHolderCloseTag);
            }

            strPattern = "<(" + strHTMLTagName + "\\s.*?/)>";
            colMatches = Regex.Matches(strFormattedValue, strPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match objMatch in colMatches)
            {
                strFormattedValue = strFormattedValue.Replace(objMatch.Value, cPlaceHolderOpenTag + objMatch.Groups[1].ToString() + cPlaceHolderCloseTag);
            }

            strPattern = "<(" + strHTMLTagName + ")>";
            colMatches = Regex.Matches(strFormattedValue, strPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match objMatch in colMatches)
            {
                strFormattedValue = strFormattedValue.Replace(objMatch.Value, cPlaceHolderOpenTag + objMatch.Groups[1].ToString() + cPlaceHolderCloseTag);
            }

            strPattern = "<(" + strHTMLTagName + "\\s.*?)>";
            colMatches = Regex.Matches(strFormattedValue, strPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match objMatch in colMatches)
            {
                strFormattedValue = strFormattedValue.Replace(objMatch.Value, cPlaceHolderOpenTag + objMatch.Groups[1].ToString() + cPlaceHolderCloseTag);
            }

            strPattern = "<(/" + strHTMLTagName + ")>";
            colMatches = Regex.Matches(strFormattedValue, strPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match objMatch in colMatches)
            {
                strFormattedValue = strFormattedValue.Replace(objMatch.Value, cPlaceHolderOpenTag + objMatch.Groups[1].ToString() + cPlaceHolderCloseTag);
            }

            return strFormattedValue;
        }

        //The following method will replace tags that need to be preserved for formatting
        //Example:  strValue = "%%%u%%%happy dog%%%u%%%", strTagName="<u>"
        //Return: strFormattedValue = "<u style="red">happy dog</u>",
        private static string ReplacePlaceHoldersWithTags(string strValue)
        {
            const string cPlaceHolderOpenTag = "%[%";
            const string cPlaceHolderCloseTag = "%]%";
            string strFormattedValue;

            strFormattedValue = strValue;

            //Replace opening tags
            strFormattedValue = strFormattedValue.Replace(cPlaceHolderOpenTag, "<");

            //Replace closing tags
            strFormattedValue = strFormattedValue.Replace(cPlaceHolderCloseTag, ">");

            return strFormattedValue;
        }

        private static string RemoveTagAttributes(string strValue, string strHTMLTagName)
        {
            string strFormattedValue;

            strFormattedValue = strValue;

            //Remove attributes from opening tags (e.g. <ul style="display:block">)
            strFormattedValue = Regex.Replace(strFormattedValue, "<" + strHTMLTagName + "\\s[^/]*?>", "<" + strHTMLTagName + ">");

            //remove attributes from complete tags (e.g. <hr style="height:20px" />)
            strFormattedValue = Regex.Replace(strFormattedValue, "<" + strHTMLTagName + "\\s[^>]*?/>", "<" + strHTMLTagName + " />");

            return strFormattedValue;
        }

        private static bool IsHTMLTagInList(string strHTMLTag, string[] aHTMLTagsToPreserveWithAttributes)
        {
            bool bHTMLTagInList;
            int iIndex;

            iIndex = Array.IndexOf(aHTMLTagsToPreserveWithAttributes, strHTMLTag);

            if (iIndex > -1)
            {
                bHTMLTagInList = true;
            }
            else
            {
                bHTMLTagInList = false;
            }

            return bHTMLTagInList;
        }

        private enum ColumnParsingState
        {
            DoNotBreak,
            BreakOnPunctuation,
            MustBreakOnNextSpace
        }


        public static string[] TextToColumns(string text, int numberOfColumns)
        {
            string[] aColumns = new String[numberOfColumns];

            //There are several valid formats for a break tag, so let's replace them all with the XHTML version before doing any more work:
            string xHTMLBreakTag = "<br />";
            text = text.Replace("<br>", xHTMLBreakTag);
            text = text.Replace("<br/>", xHTMLBreakTag);
            text = text.Replace("<BR>", xHTMLBreakTag);
            text = text.Replace("<BR/>", xHTMLBreakTag);

            //The first half of this method will simply break the columns by using the <br /> tag as an indicator of where to break
            int[] aBreakTagIndexes = FindAllOccurencesOfSubstring(xHTMLBreakTag, text, 0);

            //NOTE: If there are 3 columns, there should only be 2 break tags
            if (aBreakTagIndexes.Length == numberOfColumns - 1)
            {
                int intPreviousBreakTagIndex = 0;

                for (int i = 0; i < numberOfColumns; i++)
                {
                    if (i < numberOfColumns - 1)
                    {
                        aColumns[i] = text.Substring(intPreviousBreakTagIndex, aBreakTagIndexes[i] - intPreviousBreakTagIndex);
                        intPreviousBreakTagIndex = aBreakTagIndexes[i] + xHTMLBreakTag.Length;
                    }
                    else
                    {
                        //We need special processing for the last column (as there is always 1 less BreakTags than columns)
                        aColumns[i] = text.Substring(intPreviousBreakTagIndex);
                        break;
                    }
                }
            }
            else
            {
                // (if break tags are not specified, or the correct number is not specified) The default logic will split the columns based on size
                int intCharactersPerColumn = Convert.ToInt32(text.Length / numberOfColumns);

                int intCurrentIndex = 0;
                int intCurrentColumn = 0;
                int intColumnSizeIndexMargin = 30;  //This value represents that instead of breaking at 
                //exactly the mid point for each column, the column can break at up-to or down-to 
                // (+ or -) 20 characters.  This allows the system to break at more logical locations.

                ColumnParsingState eParsingState = ColumnParsingState.DoNotBreak;

                foreach (char c in text.ToCharArray())
                {
                    //The following logic determines what "state" the current parsing is in
                    if (intCurrentIndex <= intCharactersPerColumn - intColumnSizeIndexMargin)
                    {
                        eParsingState = ColumnParsingState.DoNotBreak;
                    }
                    else if (intCurrentIndex > intCharactersPerColumn - intColumnSizeIndexMargin
                        && intCurrentIndex <= intCharactersPerColumn + intColumnSizeIndexMargin)
                    {
                        eParsingState = ColumnParsingState.BreakOnPunctuation;
                    }
                    else
                    {
                        eParsingState = ColumnParsingState.MustBreakOnNextSpace;
                    }

                    switch (eParsingState)
                    {
                        case ColumnParsingState.DoNotBreak:

                            //Add character to current column, and move on
                            aColumns[intCurrentColumn] += c;
                            intCurrentIndex++;
                            break;

                        case ColumnParsingState.BreakOnPunctuation:

                            if (c == '.' || c == ';')
                            {
                                //Add character, but start new column
                                aColumns[intCurrentColumn] += c;
                                intCurrentIndex = 0;
                                intCurrentColumn++;
                            }
                            else
                            {
                                //just add character and move on
                                aColumns[intCurrentColumn] += c;
                                intCurrentIndex++;
                            }
                            break;

                        case ColumnParsingState.MustBreakOnNextSpace:

                            if (c == ' ')
                            {
                                //Ignore character, and start new column
                                intCurrentIndex = 0;
                                intCurrentColumn++;
                            }
                            else
                            {
                                //just add character and move on
                                aColumns[intCurrentColumn] += c;
                                intCurrentIndex++;
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < aColumns.Length; i++)
            {
                //Change null column to empty column (if necessary)
                if (aColumns[i] == null) aColumns[i] = string.Empty;
                //Now Trim column to prevent leading and trailing spaces
                aColumns[i] = aColumns[i].Trim();
            }

            return aColumns;
        }

        public static int[] FindAllOccurencesOfSubstring(string SubString, string StringToSearch, int startPos)
        {
            int foundPos = -1; // -1 represents not found.
            int count = 0;
            List<int> foundItems = new List<int>();

            do
            {
                foundPos = StringToSearch.IndexOf(SubString, startPos, StringComparison.Ordinal);

                if (foundPos > -1)
                {
                    startPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }

            } while (foundPos > -1 && startPos < StringToSearch.Length);

            return ((int[])foundItems.ToArray());
        }

        public static String Encrypt2MD5Password(String String2MD5Encrypt, int MaxPasswordLength)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] hashcode = MD5.ComputeHash(Encoding.Default.GetBytes(String2MD5Encrypt));

            StringBuilder hexifiedHashcode = new StringBuilder(32);

            // format each byte in hexifiedHashCode into a hex string
            for (int i = 0; i < hashcode.Length; i++)
            {
                hexifiedHashcode.Append(hashcode[i].ToString("X2"));
            }

            string strPassword = hexifiedHashcode.ToString();

            if (strPassword.Length > MaxPasswordLength)
            {
                strPassword = strPassword.Substring(0, MaxPasswordLength);
            }

            return strPassword;
        }

        public static bool VerifyEncryptedMD5Password(String String2MD5Encrypt, String EncryptedMD5String, int MaxPasswordLength)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            String EncryptedString2MD5Encrypt = Encrypt2MD5Password(String2MD5Encrypt, MaxPasswordLength);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return (0 == comparer.Compare(EncryptedString2MD5Encrypt, EncryptedMD5String)) ? true : false;
        }

        public static int ToIntStoppingAtFirstNonNumericCharacter(this string s)
        {
            int returnValue = 0;

            if (!string.IsNullOrEmpty(s) && !int.TryParse(s, out returnValue))
            {
                //The string is not a valid int, but let's grab all numberic characters starting the string, and use that as our int.
                char[] stringAsChars = s.ToCharArray();

                StringBuilder sb = new StringBuilder();
                foreach (char c in stringAsChars)
                {
                    if (Char.IsNumber(c))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        //as soon as you find a non-number, let's break and assume the int is finished building
                        break;
                    }
                }

                returnValue = int.Parse(sb.ToString());
            }

            return returnValue;
        }
    }
}
