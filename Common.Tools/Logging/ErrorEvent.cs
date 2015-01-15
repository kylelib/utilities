using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging
{
    public class ErrorEvent
    {
        public ErrorEvent(int errorEventID, int contactID, String comments, String message, String webPath, String referrerPath, String source,
                           String stackTrace, String cookieValues, String ipAddress)
        {
            ID = errorEventID;
            ContactID = contactID;
            Comments = comments;
            Message = message;
            WebPath = webPath;
            ReferrerPath = referrerPath;
            Source = source;
            StackTrace = stackTrace;
            CookieValues = cookieValues;
            IPAddress = ipAddress;
        }
 
        public int ID { get; set; }
        public int ContactID { get; private set; }
        public String Comments{ get; private set; }
        public String Message { get; private set; }
        public String WebPath { get; private set; }
        public String ReferrerPath { get; private set; }
        public String Source { get; private set; }
        public String StackTrace { get; private set; }
        public String CookieValues { get; private set; }
        public String IPAddress { get; private set; }
    }
}
