using Common.Tools.Logging.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Common.Tools.Logging
{
    public class WcfServiceLogger: IWcfServiceLogger
    {
        #region IWcfLogger methods

        public ErrorEvent LogException(OperationContext context, int contactID, Exception exception, string comments)
        {
            String message = null;
            String stackTrace = null;
            ErrorEvent errorEvent = null; 
            try
            {
                //Consolidate message and stackTraces of the exception and any/all of its inner exceptions.
                Util.ExceptionUtil.BuildStrings(exception, out message, out stackTrace);

                if (exception != null)
                    errorEvent = CreateErrorEvent(context, contactID, message, stackTrace, comments, true);
                else
                    errorEvent = CreateErrorEvent(context, contactID, null, null, comments, true);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {
                String ip = null;
                String source = "svc";
                try
                {
                    ip = GetHostName();
                    source = GetSource(context);
                }
                catch { }
                ExceptionUtil.HandleLoggingException(ex, contactID, source, ip, false);
            }           
            return errorEvent;
        }

        public ErrorEvent LogMessage(OperationContext context, int contactID, string message, string comments)
        {
            ErrorEvent errorEvent = null; 
            try
            {
                errorEvent = CreateErrorEvent(context, contactID, message, null, comments, false);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {
                String ip = null;
                String source = "svc";
                try
                {
                    ip = GetHostName();
                    source = GetSource(context);
                }
                catch { }
                ExceptionUtil.HandleLoggingException(ex, contactID, source, ip, false);
            }  
            return errorEvent;
        }

        #endregion 
       
        #region Private methods

        private ErrorEvent CreateErrorEvent(OperationContext operationContext, int contactID, String message, String stackTrace, String comments, bool includeParamValues)
        {
            var uri = operationContext.Channel.LocalAddress.Uri;
            String source = GetSource(operationContext);
            String ipAddress = GetHostName();

            if (includeParamValues)
            {
                String inputValues = GetInputParameterValues(operationContext);
                if (inputValues.Length > 0)
                    message += ", INPUT PARAM VALUES: " + inputValues;
            }
            return new ErrorEvent(0, contactID, comments, message, uri.AbsoluteUri, null, source, stackTrace, null, ipAddress);
        }
 
        #endregion 

        #region Static helper methods

        private static String GetInputParameterValues(OperationContext context)
        {
            string ret = String.Empty;
            if (context.IncomingMessageProperties.Keys.Contains("inputs"))
            {
                object[] inputs = (object[])context.IncomingMessageProperties["Inputs"];
                StringBuilder sb = new StringBuilder();
                foreach (object i in inputs)
                    sb.Append(System.Web.HttpUtility.UrlEncode(i.ToString()) + "&");
                ret = sb.ToString().TrimEnd().TrimEnd('&');
            }
            return ret; 
        }

        private static String GetHostName()
        {
            return Dns.GetHostName();         
        }

        private static String GetRemoteIP(OperationContext conteext)
        {
            IPAddress ipAddress = Dns.GetHostAddresses(String.Empty).FirstOrDefault
                (ip => ip.AddressFamily == AddressFamily.InterNetwork);
            return ipAddress.ToString();
        }

        private static String GetSource(OperationContext context)
        {
            Uri uri = context.Channel.LocalAddress.Uri;
            return uri.Segments[1].Trim(new char[1] { '/' }) + "_svc";
        }

        #endregion
    }
}
