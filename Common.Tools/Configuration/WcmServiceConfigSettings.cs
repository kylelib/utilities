using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Configuration
{
    public static class WcmServiceConfigSettings
    {

        public static int GlobalCacheDurationMinutes
        {
            get
            {
                return ConfigHelper.GetInt("g_GlobalCacheDurationMinutes");
            }
        }        
        
        public static String FolderNotificicationAllowedEmailAddressPattern
        {
            get
            {
                return ConfigHelper.GetString("g_FolderNotificicationAllowedEmailAddressPattern");
            }
        }

        public static String Domain
        {
            get
            {
                return ConfigHelper.GetString("g_Domain");
            }
        }

        public static string[] FAST_SearchHosts
        {
            get
            {
                return ConfigHelper.GetString("FAST_SearchHosts").Split(',');
            }
        }

        public static string[] FAST_IndexHosts
        {
            get
            {
                return ConfigHelper.GetString("FAST_IndexHosts").Split(',');
            }
        }

        public static string FAST_DefaultCollection
        {
            get
            {
                return ConfigHelper.GetString("FAST_DefaultCollection");
            }
        }

        public static string FAST_QTPipeline
        {
            get
            {
                return ConfigHelper.GetString("FAST_QTPipeline");
            }
        }

        public static string FAST_DefaultView
        {
            get
            {
                return ConfigHelper.GetString("FAST_DefaultView");
            }
        }

        public static string FAST_SpellCheckSetting
        {
            get
            {
                return ConfigHelper.GetString("FAST_SpellCheckSetting");
            }
        }

        public static bool FAST_LemmatizationEnabled
        {
            get
            {
                return ConfigHelper.GetBool("FAST_LemmatizationEnabled");
            }
        }

        public static string FAST_SpecialCharacterRegex
        {
            get
            {
                return ConfigHelper.GetString("FAST_SpecialCharacterRegex");
            }
        }

        public static Dictionary<string, string> FAST_AdditionalParameters
        {
            get
            {
                Dictionary<string, string> additionalParameters = new Dictionary<string, string>();

                foreach (var parameter in ConfigHelper.GetString("FAST_AdditionalParameters").Split('&'))
                {
                    string[] keyValue = parameter.Split('=');

                    if (keyValue.Count() == 2)
                    {
                        additionalParameters.Add(keyValue[0], keyValue[1]);
                    }
                }

                return additionalParameters;
            }
        }

        public static int FAST_Timeout
        {
            get
            {
                return ConfigHelper.GetInt("FAST_Timeout");
            }
        }

        public static bool FAST_LogSearchRequestss
        {
            get
            {
                return ConfigHelper.GetBool("FAST_LogSearchRequests");
            }
        }


        public static bool ContactUs_EmailEnabled
        {
            get
            {
                return ConfigHelper.GetBool("IconoEmail_ContactUs_EmailEnabled");
            }
        }

        public static String ContactUs_EmailToAddress
        {
            get
            {
                return ConfigHelper.GetString("IconoEmail_ContactUs_ToAddress");
            }
        }

        public static String ContactUs_EmailBodyTemplate
        {
            get
            {
                return ConfigHelper.GetString("IconoEmail_ContactUs_BodyTemplate");
            }
        }

        public static String ContactUs_EmailFromAddress
        {
            get
            {
                return ConfigHelper.GetString("IconoEmail_ContactUs_FromAddress");
            }
        }

        public static String ContactUs_EmailSubject
        {
            get
            {
                return ConfigHelper.GetString("IconoEmail_ContactUs_Subject");
            }
        }

        public static string Solr_Search
        {
            get
            {
                return ConfigHelper.GetString("SearchSolr");
            }
        }

        public static string LogSearch
        {
            get
            {
                return ConfigHelper.GetString("LogSearch");
            }
        }
    }
}
