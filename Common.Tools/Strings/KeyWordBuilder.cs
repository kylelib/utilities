using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace Common.Tools.Strings
{
    public class KeywordBuilder
    {
        #region "Public Constants"

        public enum eKeywordOptionType
        {
            AllWords,
            ExactPhrase,
            AnyWords,
            WithoutWords
        }
        #endregion

        #region "Private Instance Variables"
        private string m_strKeyword;

        private ArrayList m_aPhrases = new ArrayList();
        #endregion

        #region "Constructors"
        public KeywordBuilder()
        {
            m_strKeyword = string.Empty;
        }
        #endregion

        #region "Public Properties"

        public string Keyword
        {
            get { return m_strKeyword; }
            set { m_strKeyword = value; }
        }

        #endregion

        #region "Factory Methods"

        public static string CreateKeyword(string strAllWords, string strExactPhrase,
        string strAnyWords, string strWithoutWords)
        {
            KeywordBuilder objKeywordBuilder = new KeywordBuilder();

            if (strAllWords == null)
                strAllWords = "";
            if (strAnyWords == null)
                strAnyWords = "";
            if (strWithoutWords == null)
                strWithoutWords = "";
            if (strExactPhrase == null)
                strExactPhrase = "";


            strAllWords = KeywordBuilder.CleanupKeywords(strAllWords);
            //strExactPhrase = KeywordBuilder.CleanupKeywords(strExactPhrase);
            strAnyWords = KeywordBuilder.CleanupKeywords(strAnyWords);
            strWithoutWords = KeywordBuilder.CleanupKeywords(strWithoutWords);

            objKeywordBuilder.BuildKeywordFromOptions(strAllWords, strExactPhrase, strAnyWords, strWithoutWords);

            return objKeywordBuilder.Keyword;
        }

        public static string CleanupKeywords(string strKeywords)
        {
            string strCleanKeywords = string.Empty;

            if (strKeywords == null)
                strKeywords = "";
            strKeywords = strKeywords.Trim();

            if (strKeywords.ToLower() == "and" || strKeywords.ToLower() == "or")
            {
                strCleanKeywords = string.Empty;
            }
            else
            {
                strCleanKeywords = strKeywords;
            }

            if (strCleanKeywords.ToLower().StartsWith("and "))
            {
                strCleanKeywords = strCleanKeywords.Substring(4);
            }

            if (strCleanKeywords.ToLower().StartsWith("or "))
            {
                strCleanKeywords = strCleanKeywords.Substring(3);
            }


            if (strCleanKeywords.ToLower().EndsWith(" and"))
            {
                strCleanKeywords = strCleanKeywords.Substring(0,strCleanKeywords.Length - 4);
            }

            if (strCleanKeywords.ToLower().EndsWith(" or"))
            {
                strCleanKeywords = strCleanKeywords.Substring(0, strCleanKeywords.Length - 3);
            }

            if (strCleanKeywords.Contains("\""))
            {

                string exp = "\""; //prepare the expression. suppose you want to find the number of 'c'
                int intQuoteOccurences = Regex.Matches(strCleanKeywords, exp).Count;

                if (IsOdd(intQuoteOccurences))
                {
                    //Add extra quote to string so it can be parsed
                    strCleanKeywords += "\"";
                }
            }


            return strCleanKeywords;
        }

        #endregion

        #region "Public Methods"

        public void BuildKeywordFromOptions(string strAllWords, string strExactPhrase,
            string strAnyWords, string strWithoutWords)
        {

            strAllWords = BuildAllWordsToken(strAllWords);
            strExactPhrase = BuildPhraseToken(strExactPhrase);
            strAnyWords = BuildAnyWordsToken(strAnyWords);
            strWithoutWords = BuildWithoutWordsToken(strWithoutWords);

            //We need to wrap OR in paranthesis whenever there are other tokens in the query (to maintain order of operation)
            //If there is nothing else going into the query, we will leave off the paranthesis
            if ((strAllWords != string.Empty || strExactPhrase != string.Empty || strWithoutWords != string.Empty) && strAnyWords.Contains(" OR "))
            {
                strAnyWords = "(" + strAnyWords + ")";
            }

            //Add all of the tokens to the keyword
            Keyword = AppendStringToBaseWithSeparator(Keyword, " AND ", strAllWords);
            Keyword = AppendStringToBaseWithSeparator(Keyword, " AND ", strExactPhrase);
            Keyword = AppendStringToBaseWithSeparator(Keyword, " AND ", strAnyWords);
            Keyword = AppendStringToBaseWithSeparator(Keyword, " ANDNOT ", strWithoutWords);
        }

        #endregion

        #region "Private Methods"

        private string ReplaceQuotes(string pKeywords)
        {
            string strKeywords = pKeywords;

            ArrayList aQuoteIndexes = new ArrayList();

            int intIndex = 100; //if we start at 100, it will allow us to have up 900 phrases (becuase each index will be 3 digits).
            int intPhraseBeginIndex = strKeywords.IndexOf('"', 0);
            int intPhraseEndIndex = strKeywords.IndexOf('"', intPhraseBeginIndex + 1);


            while (intPhraseBeginIndex > -1 && intPhraseEndIndex > -1)
            {
                int intPhraseLength = intPhraseEndIndex - intPhraseBeginIndex + 1;

                string strPhrase = strKeywords.Substring(intPhraseBeginIndex, intPhraseLength);

                //Add Phrase to Phrase Collection and set placeholder to put Phrase back after processing
                m_aPhrases.Add(strPhrase);

                strKeywords = strKeywords.Replace(strPhrase, "PHRASE_" + intIndex.ToString());

                //Check for next Phrase
                intPhraseBeginIndex = strKeywords.IndexOf('"', 0);
                intPhraseEndIndex = strKeywords.IndexOf('"', intPhraseBeginIndex + 1);
                intIndex++;
            }

            return strKeywords;
        }


        private string RestoreQuotes(string pKeywords)
        {
            string strKeywords = pKeywords;
            int intIndex = 100;  //Original Phrases started at 100, so let's start there

            foreach (string strPhrase in m_aPhrases)
            {
                strKeywords = strKeywords.Replace("PHRASE_" + intIndex.ToString(), strPhrase);
                intIndex++;
            }

            return strKeywords;
        }

        private string BuildPhraseToken(string strExactPhrase)
        {
            //Remove all current quotes
            strExactPhrase = strExactPhrase.Replace("\"", "");

            //put entire phrase in quotes
            if (strExactPhrase.Length > 0)
            {
                strExactPhrase = "\"" + strExactPhrase + "\"";
            }

            return strExactPhrase;
        }

        private string BuildAllWordsToken(string strAllWords)
        {
            string[] aKeywords;
            string strToken = string.Empty;

            strAllWords = ReplaceQuotes(strAllWords);

            aKeywords = strAllWords.Split(' ');

            foreach (string strKeyword in aKeywords)
            {
                if (!IsStopWord(strKeyword))
                {
                    strToken = AppendStringToBaseWithSeparator(strToken, " AND ", strKeyword);
                }
            }

            strToken = RestoreQuotes(strToken);

            return strToken;
        }

        private string BuildAnyWordsToken(string strAnyWords)
        {
            string[] aKeywords;
            string strToken = string.Empty;

            strAnyWords = ReplaceQuotes(strAnyWords);

            aKeywords = strAnyWords.Split(' ');

            foreach (string strKeyword in aKeywords)
            {
                if (!IsStopWord(strKeyword))
                {
                    strToken = AppendStringToBaseWithSeparator(strToken, " OR ", strKeyword);
                }
            }

            strToken = RestoreQuotes(strToken);

            return strToken;
        }


        private string BuildWithoutWordsToken(string strWithoutWords)
        {
            string[] aKeywords;
            string strToken = string.Empty;


            strWithoutWords = ReplaceQuotes(strWithoutWords);

            aKeywords = strWithoutWords.Split(' ');

            foreach (string strKeyword in aKeywords)
            {
                if (!IsStopWord(strKeyword))
                {
                    strToken = AppendStringToBaseWithSeparator(strToken, " ANDNOT ", strKeyword);
                }
            }

            strToken = RestoreQuotes(strToken);

            return strToken;
        }

        private bool IsStopWord(string Word)
        {
            bool bIsStopWord = false;

            ArrayList aStopWords = new ArrayList();
            aStopWords.Add("AND");
            aStopWords.Add("ANDNOT");
            aStopWords.Add("NOT");
            aStopWords.Add("OR");


            if (aStopWords.BinarySearch(Word.ToUpper()) > -1)
            {
                bIsStopWord = true;
            }

            return bIsStopWord;
        }
        /// <summary>
        /// The following method will append the separator and string to the base string
        /// This will manage not adding the separator if BaseString is currently empty.  
        /// It will also prevent adding a separator if there is already a separator in place.
        /// </summary>
        /// <param name="strBaseString"></param>
        /// <param name="strStringToAppend"></param>
        /// <param name="strSeparator"></param>
        /// <returns></returns>
        private string AppendStringToBaseWithSeparator(string strBaseString, string strSeparator, string strStringToAppend)
        {
            //Trim base and string to append before doing anything
            strBaseString = strBaseString.Trim();
            strStringToAppend = strStringToAppend.Trim();

            if (strStringToAppend != string.Empty)
            {
                //Only perform the following logic if there is something to append.
                if (!string.IsNullOrEmpty(strBaseString))
                {
                    //Base string contains characters

                    if (!strBaseString.ToLower().EndsWith(strSeparator.ToLower()))
                    {
                        //Base string does not contain separator, so we must append the separator and the stringToAppend
                        strBaseString += strSeparator + strStringToAppend;
                    }
                    else
                    {
                        //Base string already contains separator, so let's just append the string
                        strBaseString += strStringToAppend;
                    }
                }
                else
                {
                    //Base string is empty, so simply append the StringToAppend (no need for a separator because there is nothing to separate)
                    strBaseString = strStringToAppend;
                }
            }

            return strBaseString;
        }

        #endregion

        private static bool IsOdd(int intValue)
        {
            return ((intValue & 1) == 1);
        }

    }
}

