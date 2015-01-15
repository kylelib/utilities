using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging
{
    public class ILoggerFactory
    {
        public static ILogger CreateLogger()
        {
            return new Logger();
        }

        public static IWebAppLogger CreateWebAppLoggerInstance()
        {
            return new WebAppLogger();
        }
        
        public static IWcfServiceLogger CreateWcfLoggerInstance()
        {
            return new WcfServiceLogger();
        }
    }
}
