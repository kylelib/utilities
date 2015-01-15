using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Common.Tools.Logging
{
    public class PerformanceEventDbUtil
    {
       public static PerformanceEvent Write(PerformanceEvent performanceEvent)
       {
          String _dbConnection = ConfigurationManager.ConnectionStrings["IconoDB"].ConnectionString;

          using (SqlConnection connection = new SqlConnection(_dbConnection))
          {
              SqlCommand command = connection.CreateCommand();
              command.CommandType = CommandType.StoredProcedure;
              command.CommandText = "[icoProd].[InsertUserPageRequest]";

              command.Parameters.AddWithValue("@pageUrl", EnsureFit(performanceEvent.RequestUrl, 4000));
              command.Parameters.AddWithValue("@urlParameters", EnsureFit(performanceEvent.RequestParameters, 4000));
              command.Parameters.AddWithValue("@duration", EnsureFit(performanceEvent.RequestDuration, int.MaxValue));
              command.Parameters.AddWithValue("@contactID", performanceEvent.ContactID);
              command.Parameters.AddWithValue("@aspnetSessionID", EnsureFit(performanceEvent.SessionID, 50));
              command.Parameters.AddWithValue("@isPostRequest", performanceEvent.IsPostRequest);          
              var outParam = command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
              outParam.Direction = ParameterDirection.Output;
              
              connection.Open();
              command.ExecuteNonQuery();

              int PerformanceEventID  = (int)command.Parameters["@ID"].Value;
              performanceEvent.ID = PerformanceEventID;             
          }
          return performanceEvent;
       }

       private static String EnsureFit(String val, int size)
       {
           if (val != null && val.Length > size)
               return val.Substring(0, size - 1);
           return val;
       }

       private static int EnsureFit(long val, int maxSize)
       {
           if (val > maxSize)
               return maxSize ;
           return (int)val;
       }
    }
}
