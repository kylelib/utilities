using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common.Tools.Configuration
{
    public static class ConfigHelper
    {
        public static bool GetBool(string key)
        {
            bool b = false;

            bool.TryParse(ConfigurationManager.AppSettings[key], out b);

            return b;
        }

        public static int GetInt(string key)
        {
            int i = 0;

            int.TryParse(ConfigurationManager.AppSettings[key], out i);

            return i;
        }

        public static string GetString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
