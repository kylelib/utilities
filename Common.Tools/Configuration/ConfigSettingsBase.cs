using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools.Configuration
{
    public abstract class ConfigSettingsBase
    {
        protected static T GetValue<T>(string appConfigKey)
        {
            string rawValue = ConfigurationManager.AppSettings[appConfigKey];
            if (string.IsNullOrEmpty(rawValue))
            {
                throw new Exception(string.Format("AppConfig file value {0} is required.", appConfigKey));
            }

            return ConvertValue<T>(rawValue);
        }

        protected static T GetValue<T>(string appConfigKey, T defaultValue)
        {
            string rawValue = ConfigurationManager.AppSettings[appConfigKey];
            if (string.IsNullOrEmpty(rawValue))
            {
                return defaultValue;
            }

            return ConvertValue<T>(rawValue);
        }

        //private static T ConvertValue<T>(string rawValue)
        //{
        //    T toReturn = (T)Convert.ChangeType(rawValue, typeof(T));

        //    return toReturn;
        //}
        private static T ConvertValue<T>(string rawValue)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                return (T)converter.ConvertFromString(rawValue);
            }
            return default(T);
        }


        protected static Dictionary<string, string> GetCustomList(string sectionName)
        {
            var rawSection = (Hashtable)ConfigurationManager.GetSection(sectionName);
            var list = rawSection.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value);

            return list;
        }
    }
}
