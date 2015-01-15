using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Tools.Logging
{ 
    public interface IWebAppLogger
    {
        PerformanceEvent LogPerformanceRequest(HttpRequest request, int contactId, string sessionId, long totalMilliseconds);

        ErrorEvent LogException(HttpRequest request, int contactID, Exception exception, String comments);

        ErrorEvent LogMessage(HttpRequest request, int contactID, String message, String comments);
    }  
}
