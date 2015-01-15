using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging
{

    public class PerformanceEvent
    {
        public PerformanceEvent()
        {
            ID = 0;
            RequestUrl = string.Empty;
            RequestParameters = string.Empty;
            RequestDuration = 0;
            IsPostRequest = false;
            ContactID = 0;
            SessionID = string.Empty;
        }

        public int ID { get; set; }
        public string RequestUrl { get; set; }
        public string RequestParameters { get; set; }
        public long RequestDuration { get; set; }
        public bool IsPostRequest { get; set; }
        public int ContactID { get; set; }
        public string SessionID { get; set; }

    }
}
