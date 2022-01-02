using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SoapUtility.MiscServiceGroup;
using System.Configuration;
using AuthenticationUtility;
using System.ServiceModel.Channels;
using System.Net;

namespace SyncServices
{
    public class ModulaMovement : IModulaMovement
    {
        public const string D365ServiceName = "MiscServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public ModulaMovement()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new ModulaMovementClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }


        SqlConnection conn = new SqlConnection();
       
        void StartConn()
        {
            //bool isDynamics = false
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString;
            //if (isDynamics)
            //    conString = System.Configuration.ConfigurationManager.ConnectionStrings["dynamicsConString"].ConnectionString;

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
            bool operEffected = false;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                operEffected = ((SoapUtility.MiscServiceGroup.ModulaMovement)channel).OpenWorkItem(new OpenWorkItem(context, itemId)).result;
            }
                
            //StartConn(true);

            //string sqlText = "SELECT IsPosted FROM EamWorkItems WHERE Sto_ID=@p1;";
            
            //SqlCommand comm = new SqlCommand(sqlText, conn);
            //comm.CommandType = CommandType.Text;
            //comm.Parameters.Add("@p1", SqlDbType.Int);
            //comm.Parameters["@p1"].Value = itemId;
            //var isPosted = comm.ExecuteScalar();
            //if (isPosted != null && isPosted.ToString().Equals("1"))
            //{
            //    StopConn();
            //}
            //else
            //{
            //    sqlText = "DELETE FROM EamWorkItems WHERE Sto_ID=@p1;";
            //    StartConn(true);

            //    comm = new SqlCommand(sqlText, conn);
            //    comm.CommandType = CommandType.Text;
            //    comm.Parameters.Add("@p1", SqlDbType.Int);
            //    comm.Parameters["@p1"].Value = itemId;
            //    int rowsEffected = comm.ExecuteNonQuery();
            //    StopConn();

                if (operEffected)
                {
                    StartConn();
                    string sqlText = "UPDATE EXP_MOVIMENTI set Mov_Marked=0 where Mov_id=@p1";
                   
                    SqlCommand comm = new SqlCommand(sqlText, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@p1", SqlDbType.Int);
                    comm.Parameters["@p1"].Value = itemId;
                    int rowsEffected = comm.ExecuteNonQuery();
                    //STO_TIME='" + dr["MOV_TIME1"].ToString() + "'"
                    
                    sqlText = "UPDATE EXP_STORICO set STO_Type=0 where STO_TIME=@p1;";
                    comm = new SqlCommand(sqlText, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@p1", SqlDbType.NVarChar);
                    comm.Parameters["@p1"].Value = time;
                    rowsEffected = comm.ExecuteNonQuery();

                    StopConn();
                    if (rowsEffected > 0)
                        operEffected = true;
                }


            
            return operEffected;
        }


        static DataTable ItemsNotUsedTable()
        {
            

            DataTable dt = new DataTable("ItemsNotUsed");

            dt.Columns.Add("STO_ID", typeof(int));
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("Item");
            dt.Columns.Add("Quantity", typeof(double));
            dt.Columns.Add("WMS Loc ID", typeof(Int64));
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("STO_HOSTRIF");
            dt.Columns.Add("Operator");
            return dt;
        }
        static DataTable ItemsNotUsedWithWOIdTable()
        {
            

            DataTable dt = new DataTable("ItemsNotUsedWithWOId");

            dt.Columns.Add("STO_ID", typeof(int));
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("Item");
            dt.Columns.Add("Quantity", typeof(double));
            dt.Columns.Add("WMS Loc ID", typeof(Int64));
            dt.Columns.Add("CostCenter");
            dt.Columns.Add("STO_HOSTRIF");
            dt.Columns.Add("Operator");
            dt.Columns.Add("WorkOrderId");
            dt.Columns.Add("WO Pool Code");
            dt.Columns.Add("Modified By");
            return dt;
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsNotUsed()
        {            
            DataTable itemNotUsed = ItemsNotUsedTable();

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                var items = ((SoapUtility.MiscServiceGroup.ModulaMovement)channel).ItemsNotUsed(new ItemsNotUsed(context)).result;
                foreach (var one in items)
                {
                    DataRow dr = itemNotUsed.NewRow();
                    dr["STO_ID"] = one.parmSto_ID;
                    dr["TransDate"] = one.parmSto_Time;
                    dr["Item"] = one.parmSto_Item;
                    dr["Quantity"] = one.parmSto_Quantity;
                    dr["WMS Loc ID"] = one.parmSto_WmsLocID;
                    dr["CostCenter"] = one.parmSto_CostCenter;
                    dr["STO_HOSTRIF"] = one.parmSto_HostRIF;
                    dr["Operator"] = one.parmSto_Operator;

                    itemNotUsed.Rows.Add(dr);
                }

                int count = itemNotUsed.Rows.Count;
            }

            //StartConn(true);

            //string sqlText = "SELECT STO_ID,STO_TIME 'TransDate',STO_ITEM 'Item',STO_QUANTITY 'Quantity',STO_WMSLOCID 'WMS Loc ID',"+
            //    "STO_COSTCENTER 'CostCenter',STO_HOSTRIF,STO_OPERATOR 'Operator' FROM EamWorkItems wi WHERE wi.IsUsed=0 AND wi.isposted=0;";

            //SqlCommand comm = new SqlCommand(sqlText, conn);
            //comm.CommandType = CommandType.Text;
            //SqlDataAdapter da = new SqlDataAdapter(comm);

            //da.Fill(dtReturn);

            return itemNotUsed;
            //return new DBClass(DBClass.DbName.DynamicsAX).ItemsNotUsed();
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsNotUsedWithWOId()
        {
            DataTable itemNotUsed = ItemsNotUsedWithWOIdTable();

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                var items = ((SoapUtility.MiscServiceGroup.ModulaMovement)channel).ItemsNotUsedWithWOId(new ItemsNotUsedWithWOId(context)).result;

                foreach (var one in items)
                {
                    DataRow dr = itemNotUsed.NewRow();
                    dr["STO_ID"] = one.parmSto_ID;
                    dr["TransDate"] = one.parmSto_Time;
                    dr["Item"] = one.parmSto_Item;
                    dr["Quantity"] = one.parmSto_Quantity;
                    dr["WMS Loc ID"] = one.parmSto_WmsLocID;
                    dr["CostCenter"] = one.parmSto_CostCenter;
                    dr["STO_HOSTRIF"] = one.parmSto_HostRIF;
                    dr["Operator"] = one.parmSto_Operator;
                    dr["WorkOrderId"] = one.parmSto_WorkOrderID;
                    dr["WO Pool Code"] = one.parmSto_WOPoolCode;
                    dr["Modified By"] = one.parmSto_ModifiedBy;

                    itemNotUsed.Rows.Add(dr);
                }

                int count = itemNotUsed.Rows.Count;
            }
            //StartConn(true);

            //string sqlText = "SELECT wi.STO_ID,wi.STO_TIME 'TransDate',wi.STO_ITEM 'Item',wi.STO_QUANTITY 'Quantity',wi.STO_WMSLOCID 'WMS Loc ID',"+
            //    "wi.STO_COSTCENTER 'CostCenter',wi.STO_HOSTRIF,wi.STO_OPERATOR 'Operator',wo.WORKORDERID 'WorkOrderId',wo.WorkOderPoolCode 'WO Pool Code',wo.MODIFIEDBY 'Modified By' " +
            //    "FROM EamWorkItems wi INNER JOIN PMWorkOrders wo ON wi.STO_HOSTRIF = wo.WORKORDERID " +
            //    "WHERE wi.IsUsed=0 AND wi.isposted=0;";

            //SqlCommand comm = new SqlCommand(sqlText, conn);
            //comm.CommandType = CommandType.Text;
            //SqlDataAdapter da = new SqlDataAdapter(comm);

            //da.Fill(dtReturn);

            return itemNotUsed;
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public DataTable ItemsUsedButNotPosted()
        {
            DataTable itemNotUsed = ItemsNotUsedWithWOIdTable();

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                ModulaItemContract[] items = ((SoapUtility.MiscServiceGroup.ModulaMovement)channel).ItemsUsedButNotPosted(new ItemsUsedButNotPosted(context)).result;

                foreach (var one in items)
                {
                    DataRow dr = itemNotUsed.NewRow();
                    dr["STO_ID"] = one.parmSto_ID;
                    dr["TransDate"] = one.parmSto_Time;
                    dr["Item"] = one.parmSto_Item;
                    dr["Quantity"] = one.parmSto_Quantity;
                    dr["WMS Loc ID"] = one.parmSto_WmsLocID;
                    dr["CostCenter"] = one.parmSto_CostCenter;
                    dr["STO_HOSTRIF"] = one.parmSto_HostRIF;
                    dr["Operator"] = one.parmSto_Operator;
                    dr["WorkOrderId"] = one.parmSto_WorkOrderID;
                    dr["WO Pool Code"] = one.parmSto_WOPoolCode;
                    dr["Modified By"] = one.parmSto_ModifiedBy;

                    itemNotUsed.Rows.Add(dr);
                }

                int count = itemNotUsed.Rows.Count;
            }
            //StartConn(true);

            //string sqlText = "SELECT wi.STO_ID,wi.STO_TIME 'TransDate',wi.STO_ITEM 'Item',wi.STO_QUANTITY 'Quantity',wi.STO_WMSLOCID 'WMS Loc ID',"+
            //    "wi.STO_COSTCENTER 'CostCenter',wi.STO_HOSTRIF,wi.STO_OPERATOR 'Operator',wo.WORKORDERID 'WorkOrderId',wo.WorkOderPoolCode 'WO Pool Code',wo.MODIFIEDBY 'Modified By' " +
            //    "FROM PMWORKORDERMATERIAL ma INNER JOIN EamWorkItems wi ON ma.STO_ID = wi.STO_ID inner join PMWorkOrders wo on ma.WORKORDERID = wo.WORKORDERID " +
            //    "WHERE wi.IsUsed=1 AND wi.isposted=0 " +
            //    //"and STO_TIME<'2017-01-01' "+
            //    "ORDER BY wi.sto_costcenter;";

            //SqlCommand comm = new SqlCommand(sqlText, conn);
            //comm.CommandType = CommandType.Text;
            //SqlDataAdapter da = new SqlDataAdapter(comm);

            //da.Fill(dtReturn);

            return itemNotUsed;
        }
    }
}
