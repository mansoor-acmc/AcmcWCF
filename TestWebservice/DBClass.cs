using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TestWebservice
{
    public class DBClass
    {
        public void CheckDMExport()
        {
            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnImpExpDB"].ConnectionString);

            try
            {
                string sqlString = "Select top 100 * from DMExport order by ExpTimestamp desc";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    string pallet = dr["PalletNum"].ToString();
                }
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }
        }

        public void ErrorInsert(string salesId, string pickingId, string errorString, string fullTrace, DateTime errorDate, string projectName,
            string username, string deviceName, string methodName)
        {
            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnErrorDB"].ConnectionString);

            try
            {
                string sqlString = "INSERT INTO ErrorInfo (ProjectName, SalesId, PickingListID, DateOfError, ErrorString, Username, DeviceName, FullTrace, MethodName) " +
                    "VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p3", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p4", SqlDbType.DateTime);
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p6", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p7", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p8", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p9", SqlDbType.NVarChar);

                cmd.Parameters["@p1"].Value = projectName;
                cmd.Parameters["@p2"].Value = salesId;
                cmd.Parameters["@p3"].Value = pickingId;
                cmd.Parameters["@p4"].Value = errorDate;
                cmd.Parameters["@p5"].Value = errorString;
                cmd.Parameters["@p6"].Value = username;
                cmd.Parameters["@p7"].Value = deviceName;
                cmd.Parameters["@p8"].Value = fullTrace;
                cmd.Parameters["@p9"].Value = methodName;

                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                object result = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }
        }
    }
}
