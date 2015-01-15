using Common.Tools.Logging.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Common.Tools.Logging
{
    public class WebAppLogger : IWebAppLogger
    {
        #region IWebAppLogger methods

        public PerformanceEvent LogPerformanceRequest(HttpRequest request, int contactId, string sessionId, long totalMilliseconds)
        {
            PerformanceEvent performanceEvent = CreatePerformanceEvent(request, contactId, sessionId, totalMilliseconds);
            PerformanceEventDbUtil.Write(performanceEvent);

            return performanceEvent;
        }

        public ErrorEvent LogException(HttpRequest request, int contactID, Exception exception, string comments)
        {
            ErrorEvent errorEvent = null;
            String message = null;
            String stackTrace = null;
            try
            {
                //Consolidate message and stackTraces of the exception and any/all of its inner exceptions.
                Util.ExceptionUtil.BuildStrings(exception, out message, out stackTrace);

                if (exception != null)
                    errorEvent = CreateErrorEvent(request, contactID, message, stackTrace, comments, true);
                else
                    errorEvent = CreateErrorEvent(request, contactID, null, null, comments, true);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {
                String ip = null;
                String source = "web";
                try
                {
                    ip = GetHostName();
                    source = GetSource(request);
                }
                catch { }
                ExceptionUtil.HandleLoggingException(ex, contactID, source, ip, true);
            }
            return errorEvent;
        }

        public ErrorEvent LogMessage(HttpRequest request, int contactID, string message, string comments)
        {
            ErrorEvent errorEvent = null;
            try
            {
                errorEvent = CreateErrorEvent(request, contactID, message, null, comments, false);
                ErrorEventDbUtil.Write(errorEvent);
            }
            catch (Exception ex)
            {
                String ip = null;
                String source = "web";
                try
                {
                    ip = GetHostName();
                    source = GetSource(request);
                }
                catch { }
                ExceptionUtil.HandleLoggingException(ex, contactID, source, ip, true);
            }
            return errorEvent;
        }

        #endregion

        #region Private methods

        private ErrorEvent CreateErrorEvent(HttpRequest request, int contactID, String message, String stackTrace, String comments, bool includeFormValues)
        {
            String referrerPath = null;
            if (request.UrlReferrer != null)
                referrerPath = request.UrlReferrer.AbsoluteUri;
            if (includeFormValues)
            {
                String formValues = GetFormValues(request);
                if (formValues.Length > 0)
                    message += " form values: [" + formValues + "]";
            }
            String cookieValues = GetCookieValues(request);
            String ipAddress = GetHostName();
            String source = GetSource(request);

            return new ErrorEvent(0, contactID, comments, message, request.Url.AbsoluteUri, referrerPath, source, stackTrace, cookieValues, ipAddress);
        }

        private PerformanceEvent CreatePerformanceEvent(HttpRequest request, int contactId, string sessionId, long totalMilliseconds)
        {
            PerformanceEvent performanceEvent = new PerformanceEvent()
            {
                RequestUrl = request.Path,
                RequestParameters = request.Url.Query,
                RequestDuration = totalMilliseconds,
                IsPostRequest = (request.RequestType.ToLower() == "post"),
                ContactID = contactId,
                SessionID = sessionId
            };

            return performanceEvent;
        }

        #endregion

        #region Static helper methods

        private static String GetCookieValues(HttpRequest request)
        {
            var sb = new StringBuilder();
            foreach (string key in request.Cookies.AllKeys)
                sb.Append(key + "=" + request.Cookies[key].Value + ", ");
            return sb.ToString().TrimEnd().TrimEnd(',');
        }

        private static String GetFormValues(HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in request.Form.AllKeys)
                sb.Append(key + "=" + request.Form[key] + ", ");
            return sb.ToString().TrimEnd().TrimEnd(',');
        }

        private static String GetHostName()
        {
            return Dns.GetHostName();
        }

        private static String GetSource(HttpRequest request)
        {
            return request.Url.Segments[1].Trim(new char[1] { '/' }) + "_web";
        }

        #endregion
    }
}
