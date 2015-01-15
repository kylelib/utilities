using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging.Util
{
    public class ExceptionUtil 
    {
        /// <summary>
        /// Consolidate message and stackTraces for given exception and all of its inner exception.  
        /// </summary>
        /// <param name="exception">Any exception</param>
        /// <param name="message">Output param.  Consolidated exception messages.</param>
        /// <param name="stackTrace">Output param.  Consolidated stack traces.</param>
        public static void BuildStrings(Exception exception, out String message, out String stackTrace)
        {
            message = exception.Message ?? "[ ]";
            stackTrace = exception.StackTrace ?? "[ ]";
            
            if (exception.InnerException != null)
                ParseInner(exception.InnerException, ref message, ref stackTrace);
        } 
        
        private static void ParseInner(Exception exception, ref String message, ref String stackTrace)
        {
            message += "\nINNER EXCEPTION:" + exception.Message ?? "[ ]";
            stackTrace += "\nINNER EXCEPTION:" + exception.StackTrace ?? "[ ]";

            if (exception.InnerException != null)
                ParseInner(exception.InnerException, ref message , ref stackTrace);
        }
        
        public static void HandleLoggingException(Exception ex, int contactID, string source, String ip, bool sendEmail)
        {
            string message = null;
            string stackTrace = null;
            try
            {
                var loggingEx = new ApplicationException("Unable to log exception to the database", ex);               
                BuildStrings(loggingEx, out message, out stackTrace);
            }
            catch
            {
                message = "Unable to log exception to the database: " + ex.Message;
                stackTrace = ex.Source;
            }
            finally
            {   
                if (sendEmail)
                {
                    ErrorEvent errorEvent = new ErrorEvent(0, contactID, null, message, null, null, source, stackTrace, null, ip);
                    //ErrorEventEmailer.SendEmail(errorEvent, "V2 Logging Exception");
                }
                SystemApplictionEventLoggingUtil.WriteError(source, message + "\n\nStackTrace:\n" + stackTrace);            
            }
        }


   }
}
