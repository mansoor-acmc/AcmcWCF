using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    public class ModulaMovement : IModulaMovement
    {
        SqlConnection conn = new SqlConnection();
       
        void StartConn(bool isDynamics = false)
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString;
            if (isDynamics)
                conString = System.Configuration.ConfigurationManager.ConnectionStrings["dynamicsConString"].ConnectionString;

            conn = new SqlConnection(conString);
            conn.Open();
        }
        void StopConn()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool OpenItemCode(int itemId, string time)
        {
            StartConn(true);

            string sqlText = "SELECT IsPosted FROM EamWorkItems WHERE Sto_ID=@p1;";
            
            SqlCommand comm = new SqlCommand(sqlText, conn);
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add("@p1", SqlDbType.Int);
            comm.Parameters["@p1"].Value = itemId;
            var isPosted = comm.ExecuteScalar();
            if (isPosted != null && isPosted.ToString().Equals("1"))
            {
                StopConn();
            }
            else
            {
                sqlText = "DELETE FROM EamWorkItems WHERE Sto_ID=@p1;";
                StartConn(true);

                comm = new SqlCommand(sqlText, conn);
                comm.CommandType = CommandType.Text;
                comm.Parameters.Add("@p1", SqlDbType.Int);
                comm.Parameters["@p1"].Value = itemId;
                int rowsEffected = comm.ExecuteNonQuery();
                StopConn();

                if (rowsEffected > 0)
                {
                    StartConn(false);
                    sqlText = "UPDATE EXP_MOVIMENTI set Mov_Marked=0 where Mov_id=@p1";
                    comm = new SqlCommand(sqlText, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@p1", SqlDbType.Int);
                    comm.Parameters["@p1"].Value = itemId;
                    rowsEffected = comm.ExecuteNonQuery();
                    //STO_TIME='" + dr["MOV_TIME1"].ToString() + "'"
                    sqlText = "UPDATE EXP_STORICO set STO_Type=0 where STO_TIME=@p1;";
                    comm = new SqlCommand(sqlText, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@p1", SqlDbType.NVarChar);
                    comm.Parameters["@p1"].Value = time;
                    rowsEffected = comm.ExecuteNonQuery();

                    StopConn();
                    if (rowsEffected > 0)
                        return true;
                }


            }
            return false;
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsNotUsed()
        {
            DataTable dtReturn = new DataTable("WorkOrders");
            StartConn(true);

            string sqlText = "SELECT STO_ID,STO_TIME 'TransDate',STO_ITEM 'Item',STO_QUANTITY 'Quantity',STO_WMSLOCID 'WMS Loc ID',"+
                "STO_COSTCENTER 'CostCenter',STO_HOSTRIF,STO_OPERATOR 'Operator' FROM EamWorkItems wi WHERE wi.IsUsed=0 AND wi.isposted=0;";

            SqlCommand comm = new SqlCommand(sqlText, conn);
            comm.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(comm);

            da.Fill(dtReturn);

            return dtReturn;
            //return new DBClass(DBClass.DbName.DynamicsAX).ItemsNotUsed();
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsNotUsedWithWOId()
        {
            DataTable dtReturn = new DataTable("WorkOrders");
            StartConn(true);

            string sqlText = "SELECT wi.STO_ID,wi.STO_TIME 'TransDate',wi.STO_ITEM 'Item',wi.STO_QUANTITY 'Quantity',wi.STO_WMSLOCID 'WMS Loc ID',"+
                "wi.STO_COSTCENTER 'CostCenter',wi.STO_HOSTRIF,wi.STO_OPERATOR 'Operator',wo.WORKORDERID 'WorkOrderId',wo.WorkOderPoolCode 'WO Pool Code',wo.MODIFIEDBY 'Modified By' " +
                "FROM EamWorkItems wi INNER JOIN PMWorkOrders wo ON wi.STO_HOSTRIF = wo.WORKORDERID " +
                "WHERE wi.IsUsed=0 AND wi.isposted=0;";

            SqlCommand comm = new SqlCommand(sqlText, conn);
            comm.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(comm);

            da.Fill(dtReturn);

            return dtReturn;
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsUsedButNotPosted()
        {
            DataTable dtReturn = new DataTable("WorkOrders");
            StartConn(true);

            string sqlText = "SELECT wi.STO_ID,wi.STO_TIME 'TransDate',wi.STO_ITEM 'Item',wi.STO_QUANTITY 'Quantity',wi.STO_WMSLOCID 'WMS Loc ID',"+
                "wi.STO_COSTCENTER 'CostCenter',wi.STO_HOSTRIF,wi.STO_OPERATOR 'Operator',wo.WORKORDERID 'WorkOrderId',wo.WorkOderPoolCode 'WO Pool Code',wo.MODIFIEDBY 'Modified By' " +
                "FROM PMWORKORDERMATERIAL ma INNER JOIN EamWorkItems wi ON ma.STO_ID = wi.STO_ID inner join PMWorkOrders wo on ma.WORKORDERID = wo.WORKORDERID " +
                "WHERE wi.IsUsed=1 AND wi.isposted=0 " +
                //"and STO_TIME<'2017-01-01' "+
                "ORDER BY wi.sto_costcenter;";

            SqlCommand comm = new SqlCommand(sqlText, conn);
            comm.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(comm);

            da.Fill(dtReturn);

            return dtReturn;
        }
    }
}
