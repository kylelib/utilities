using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common.Tools.Logging
{
    public interface IWcfServiceLogger
    {
        ErrorEvent LogException(OperationContext context, int contactID, Exception exception, String comments);

        ErrorEvent LogMessage(OperationContext context, int contactID, String message, String comments);
    }
}
