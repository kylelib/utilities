using Common.Tools.Logging.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace Common.Tools.Logging
{
    public class Logger: ILogger
    {
        #region ILogger methods

        public ErrorEvent LogException(int contactID, string source, Exception exception, string comments)
        {
            String message = null;
            String stackTrace = null;
            ErrorEvent errorEvent = null;
            try
            {
                //Consolidate message and stackTraces of the exception and any/all of its inner exceptions.
                Util.ExceptionUtil.BuildStrings(exception, out message, out stackTrace);
                
                if (exception != null)
                    errorEvent = CreateErrorEvent(contactID, source, message, stackTrace, comments);
                else
                    errorEvent = CreateErrorEvent(contactID, source, null, null, comments);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {               
                ExceptionUtil.HandleLoggingException(ex, contactID, source, null, false);
            }  
            return errorEvent;
        }
   
        public ErrorEvent LogMessage(int contactID, string source, string message, string comments)
        {
            ErrorEvent errorEvent = null;
            try 
            {
                errorEvent = CreateErrorEvent(contactID, source, message, null, comments);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {
                ExceptionUtil.HandleLoggingException(ex, contactID, source, null, false);
            }  
            return errorEvent;
        }

        #endregion 

        #region Private methods

        private ErrorEvent CreateErrorEvent(int contactID, string source, String message, String stackTrace, string comments)
        {
            return new ErrorEvent(0, contactID, comments, message, null, null, source, null, null, null);           
        }

        #endregion 
    }
}

