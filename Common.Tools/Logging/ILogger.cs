using System;
using System.Collections.Generic;
using System.Linq; 

 namespace Common.Tools.Logging
{
    public interface ILogger
    {
        ErrorEvent LogMessage(int contactID, String source, String message, String comments);

        ErrorEvent LogException(int contactID, String source, Exception excecption, String comments);
    }
}
