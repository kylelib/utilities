using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging.Util
{
    public static class SystemApplictionEventLoggingUtil
    {
        public static void WriteError(String source, String message)
        {
            Write(source, message, EventLogEntryType.Error);
        }
               
        public static void WriteWarning(String source, String message)
        {
            Write(source, message, EventLogEntryType.Warning);
        }

        public static void WriteInfo(String source, String message)
        {
            Write(source, message, EventLogEntryType.Information);
        }
        
        private static void Write(String source, String message, EventLogEntryType type)
        {
            source = source ?? "Icono App"; //if the source is missing, provide default value.         
            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, "Application");
                EventLog.WriteEntry(source, message, type);
            }
            catch { }
            
        }
    }
}
