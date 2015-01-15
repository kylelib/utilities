using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Configuration
{
    public static class IqWebConfigSettings
    {
        private const string c_GlobalCacheDurationMinutes = "g_GlobalCacheDurationMinutes";

        public static int GlobalCacheDurationMinutes
        {
            get
            {
                return ConfigHelper.GetInt(c_GlobalCacheDurationMinutes);
            }
        }

        public static int MaxFeedsToDisplay
        {
            get
            {
                return ConfigHelper.GetInt("MaxFeedsToDisplay");
            }
        }
    }
}
