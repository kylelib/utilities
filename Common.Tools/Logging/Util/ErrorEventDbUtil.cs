using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging
{
    public class ErrorEventDbUtil
    {
       public static ErrorEvent Write(ErrorEvent errorEvent)
       {
          String _dbConnection = ConfigurationManager.ConnectionStrings["IconoDB"].ConnectionString;

          using (SqlConnection connection = new SqlConnection(_dbConnection))
          {
              SqlCommand command = connection.CreateCommand();
              command.CommandType = CommandType.StoredProcedure;
              command.CommandText = "[icoProd].[InsertErrorEvent]";
              
              command.Parameters.AddWithValue("@contactID", errorEvent.ContactID);             
              command.Parameters.AddWithValue("@comments", EnsureFit(errorEvent.Comments, 254));
              command.Parameters.AddWithValue("@message", EnsureFit(errorEvent.Message, 2000));
              command.Parameters.AddWithValue("@webPath", EnsureFit(errorEvent.WebPath,1000));
              command.Parameters.AddWithValue("@referrerPath", EnsureFit(errorEvent.ReferrerPath, 1000));
              command.Parameters.AddWithValue("@source", EnsureFit(errorEvent.Source, 1000));
              command.Parameters.AddWithValue("@stackTrace", EnsureFit(errorEvent.StackTrace, 5000));
              command.Parameters.AddWithValue("@cookieValues", EnsureFit(errorEvent.CookieValues, 5000));
              command.Parameters.AddWithValue("@IPAddress", EnsureFit(errorEvent.IPAddress, 300));              
              var outParam = command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
              outParam.Direction = ParameterDirection.Output;
              
              connection.Open();
              command.ExecuteNonQuery();

              int errorEventID  = (int)command.Parameters["@ID"].Value;
              errorEvent.ID = errorEventID;             
          }
          return errorEvent;
       }

       private static String EnsureFit(String val, int size)
       {
           if (val != null && val.Length > size)
               return val.Substring(0, size - 1);
           return val;
       }
    }
}
