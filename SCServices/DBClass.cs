using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SyncServices
{
    public class DBClass
    {
        SqlConnection conn=null;

        public DBClass()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            conn = new SqlConnection(connString);
        }

        public DBClass(DbName msgDB)
        {
            string connString = string.Empty;

            if (msgDB == DbName.Counting)
            {
                connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                conn = new SqlConnection(connString);
            }
            else if (msgDB == DbName.DeviceMsg)
            {
                connString = ConfigurationManager.ConnectionStrings["ConnErrorDB"].ConnectionString;
                conn = new SqlConnection(connString);
            }
            else if (msgDB == DbName.Modula)
            {
                connString = ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString;
                conn = new SqlConnection(connString);
            }
            else if (msgDB == DbName.ImportExportDB)
            {
                connString = ConfigurationManager.ConnectionStrings["ConnImpExpDB"].ConnectionString;
                conn = new SqlConnection(connString);
            }
            else if (msgDB == DbName.DynamicsAX)
            {
                connString = ConfigurationManager.ConnectionStrings["dynamicsConString"].ConnectionString;
                conn = new SqlConnection(connString);
            }
        }

        public enum DbName
        {
            DeviceMsg,
            Modula,
            Counting,
            ImportExportDB,
            DynamicsAX
        }

        public long GetLastSyncId(bool isFgData)
        {
            long lastSyncId = 0;
            string sqlString = "select max(SyncId) from CountingSC";
            if (isFgData)
                sqlString = "select max(SyncId) from ItemsFG";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlString;

            if (this.conn.State != ConnectionState.Open)
                this.conn.Open();

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                long.TryParse(result.ToString(), out lastSyncId);
            }

            return lastSyncId;
        }

        public long UpdateFGCountingDesktop(List<PalletEntity> dt)
        {
            long rowsEffected = 0;
            long lastSyncId = GetLastSyncId(true);

            try
            {
                foreach (PalletEntity row in dt)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateItemFG";


                    cmd.Parameters.Add("@pItem", SqlDbType.NVarChar, 20);
                    cmd.Parameters.Add("@pIsMatched", SqlDbType.Bit);
                    cmd.Parameters.Add("@pProcessByIT", SqlDbType.Bit);
                    cmd.Parameters.Add("@pDateUpdated", SqlDbType.DateTime);
                    cmd.Parameters.Add("@pUpdatedBy", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@pDeviceName", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@pIsManual", SqlDbType.Bit);
                    cmd.Parameters.Add("@pSyncID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@pAvailPhysical", SqlDbType.Decimal);
                    cmd.Parameters.Add("@pItemNumber", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@pProductName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@pConfig", SqlDbType.VarChar, 2);
                    cmd.Parameters.Add("@pSize", SqlDbType.VarChar, 11);
                    cmd.Parameters.Add("@pColor", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@pWarehouse", SqlDbType.VarChar, 25);
                    cmd.Parameters.Add("@pBatchNumber", SqlDbType.VarChar, 15);
                    cmd.Parameters.Add("@pLocation", SqlDbType.VarChar, 12);
                    cmd.Parameters.Add("@pLocationNew", SqlDbType.VarChar, 12);
                    cmd.Parameters.Add("@pHasTransferLocation", SqlDbType.Bit);

                    cmd.Parameters.Add("@return", SqlDbType.Int);

                    cmd.Parameters["@pItem"].Value = row.Pallet;
                    cmd.Parameters["@pIsMatched"].Value = row.IsMatched;
                    cmd.Parameters["@pProcessByIT"].Value = row.ProcessByIT;
                    cmd.Parameters["@pDateUpdated"].Value = row.DateUpdated;
                    if (row.DateUpdated.Year <= 1753)
                        cmd.Parameters["@pDateUpdated"].Value = DBNull.Value;
                    cmd.Parameters["@pUpdatedBy"].Value = row.UpdatedBy;
                    cmd.Parameters["@pDeviceName"].Value = row.DeviceName;
                    cmd.Parameters["@pIsManual"].Value = row.IsManual;
                    cmd.Parameters["@pSyncID"].Value = row.SyncId;
                    cmd.Parameters["@pAvailPhysical"].Value = row.Available;
                    cmd.Parameters["@pItemNumber"].Value = row.ItemNumber;
                    cmd.Parameters["@pProductName"].Value = row.ProductName;
                    cmd.Parameters["@pConfig"].Value = row.Config;
                    cmd.Parameters["@pSize"].Value = row.Size;
                    cmd.Parameters["@pColor"].Value = row.Color;
                    cmd.Parameters["@pWarehouse"].Value = row.Warehouse;
                    cmd.Parameters["@pBatchNumber"].Value = row.BatchNumber;
                    cmd.Parameters["@pLocation"].Value = row.Location;
                    cmd.Parameters["@pLocationNew"].Value = row.LocationNew;
                    cmd.Parameters["@pHasTransferLocation"].Value = row.HasTransferLocation;

                    cmd.Parameters["@return"].Direction = ParameterDirection.ReturnValue;

                    if (this.conn.State != ConnectionState.Open)
                        this.conn.Open();

                    rowsEffected = cmd.ExecuteNonQuery();

                    //rowsEffected = (int)cmd.Parameters["@return"].Value;
                    rowsEffected = (int)cmd.Parameters["@return"].Value;
                    if (rowsEffected > 0)
                    {
                        //numOfRec++;
                        lastSyncId = row.SyncId;
                    }

                }
            }
            finally
            {
                if (this.conn.State != ConnectionState.Closed)
                    this.conn.Close();
            }

            return lastSyncId;
        }
              

        public long UpdateSupplyChainDesktop(List<ItemEntity> dt)
        {
            //DataTable dt = GetSCData();
            //int numOfRec = 0;
            long lastSyncId = GetLastSyncId(false);

            try
            {
                foreach (ItemEntity row in dt)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateItemSC";

                    cmd.Parameters.Add("@pItem", SqlDbType.NVarChar, 20);
                    cmd.Parameters.Add("@pDescription", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@pLocation", SqlDbType.NVarChar, 10);
                    cmd.Parameters.Add("@pUoM", SqlDbType.NVarChar, 10);
                    cmd.Parameters.Add("@pIsMatched", SqlDbType.Bit);
                    cmd.Parameters.Add("@pProcessByIT", SqlDbType.Bit);
                    cmd.Parameters.Add("@pSyncID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@pPhysicalCount", SqlDbType.Decimal);
                    cmd.Parameters.Add("@pAvailableCount", SqlDbType.Decimal);
                    cmd.Parameters.Add("@pDateUpdated", SqlDbType.DateTime);
                    cmd.Parameters.Add("@pUpdatedBy", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@pDeviceName", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@pIsManual", SqlDbType.Bit);
                    cmd.Parameters.Add("@return", SqlDbType.Int);

                    cmd.Parameters["@pItem"].Value = row.Item;
                    cmd.Parameters["@pDescription"].Value = row.Description;
                    cmd.Parameters["@pLocation"].Value = row.Location;
                    cmd.Parameters["@pUoM"].Value = row.UoM;
                    cmd.Parameters["@pIsMatched"].Value = row.IsMatched;
                    cmd.Parameters["@pProcessByIT"].Value = row.ProcessByIT;
                    cmd.Parameters["@pSyncID"].Value = row.SyncId;
                    cmd.Parameters["@pPhysicalCount"].Value = row.PhysicalCount;
                    cmd.Parameters["@pAvailableCount"].Value = row.AvailableCount;
                    cmd.Parameters["@pDateUpdated"].Value = row.DateUpdated;
                    if (row.DateUpdated.Year <= 1753)
                        cmd.Parameters["@pDateUpdated"].Value = DBNull.Value;
                    cmd.Parameters["@pUpdatedBy"].Value = row.UpdatedBy;
                    cmd.Parameters["@pDeviceName"].Value = row.DeviceName;
                    cmd.Parameters["@pIsManual"].Value = row.IsManual;
                    cmd.Parameters["@return"].Direction = ParameterDirection.ReturnValue;

                    if (this.conn.State != ConnectionState.Open)
                        this.conn.Open();

                    int rowsEffected = cmd.ExecuteNonQuery();

                    rowsEffected = (int)cmd.Parameters["@return"].Value;
                    if (rowsEffected > 0)
                    {
                        //numOfRec++;
                        lastSyncId = row.SyncId;
                    }
                }
            }
            finally
            {
                if (this.conn.State != ConnectionState.Closed)
                    this.conn.Close();
            }

            return lastSyncId;
        }

        public ItemEntity GetItem(string itemId)
        {
            ItemEntity oneItem = new ItemEntity();

            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Parameters.Add("@pItemID", SqlDbType.NVarChar, 50);
                cmd.Parameters["@pItemID"].Value = itemId;

                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetItemInfo";

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if(dr.Read())
                {
                    oneItem = new ItemEntity()
                    {
                        Item = dr["Item"].ToString(),
                        Description = dr["ItemName"].ToString(),
                        UoM = dr["UoM"].ToString(),
                        AvailableCount = 0,
                        Location = dr["Location"].ToString(),
                        DateUpdated = new DateTime(1901, 1, 1),
                        LockDate = new DateTime(1901, 1, 1),
                        SyncId = 0
                    };
                    if (dr["Stock"] != DBNull.Value)
                    {
                        oneItem.AvailableCount = Convert.ToInt64(decimal.Parse(dr["Stock"].ToString()));
                    }
                }
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }

            return oneItem;
        }

        public List<ItemEntity> GetItems(string search)
        {
            List<ItemEntity> manyItem = new List<ItemEntity>();

            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Parameters.Add("@pFindItem", SqlDbType.NVarChar, 100);
                cmd.Parameters["@pFindItem"].Value = search;

                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetItems";

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        ItemEntity oneItem = new ItemEntity()
                        {
                            Item = dr["Item"].ToString(),
                            Description = dr["ItemName"].ToString(),
                            UoM = dr["UoM"].ToString(),
                            AvailableCount = 0,
                            Location = dr["Location"].ToString(),
                            DateUpdated = new DateTime(1901, 1, 1),
                            LockDate = new DateTime(1901, 1, 1),
                            SyncId = 0
                        };
                        if (dr["Stock"] != DBNull.Value)
                        {
                            oneItem.AvailableCount = Convert.ToInt64(decimal.Parse(dr["Stock"].ToString()));
                        }

                        manyItem.Add(oneItem);
                    }
                }
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }

            return manyItem;
        }

        public List<ItemEntity> ResetDataSCCount()
        {
            List<ItemEntity> items = new List<ItemEntity>();

            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM InventorySC;";

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                long iCount=0;
                while(dr.Read())
                {
                    ItemEntity oneItem = new ItemEntity()
                    {
                        Item = dr["Item"].ToString(),
                        Description = dr["ItemName"].ToString(),
                        UoM = dr["UoM"].ToString(),
                        AvailableCount = 0,
                        Location = dr["Location"].ToString(),
                        DateUpdated = new DateTime(1901, 1, 1),
                        LockDate = new DateTime(1901, 1, 1),
                        SyncId = iCount
                    };
                    if (dr["Stock"] != DBNull.Value)
                    {
                        oneItem.AvailableCount = Convert.ToInt64(decimal.Parse(dr["Stock"].ToString()));
                    }

                    items.Add(oneItem);
                    iCount++;
                }
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }

            return items;
        }

        public List<ItemEntity> GetSCUnitOfMeasure()
        {
            List<ItemEntity> items = new List<ItemEntity>();

            SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ModulaConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Connection = sConnMod;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetUoMs";

                if (sConnMod.State != ConnectionState.Open)
                    sConnMod.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                long iCount = 0;
                while (dr.Read())
                {
                    ItemEntity oneItem = new ItemEntity()
                    {
                        Item = dr["Item"].ToString(),                        
                        UoM = dr["UoM"].ToString(),
                        SyncId = iCount
                    };

                    items.Add(oneItem);
                    iCount++;
                }
            }
            finally
            {
                if (sConnMod.State != ConnectionState.Closed)
                    sConnMod.Close();
            }

            return items;
        }

        public List<PalletEntity> ResetDataFGCount()
        {
            List<PalletEntity> items = new List<PalletEntity>();


            string sqlString = "SELECT  [Serial number], [Item number], [Product name], Configuration, Size, Color, Warehouse, Location, [Batch number], [Available physical] Qty,  DateUpdated, SyncId " +
                "FROM ItemsFG";


            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                long iCount=0;
                while (dr.Read())
                {
                    PalletEntity oneItem = new PalletEntity()
                    {
                        ItemNumber = dr["Item number"].ToString(),
                        ProductName = dr["Product name"].ToString(),
                        Config = dr["Configuration"].ToString(),
                        Size = dr["Size"].ToString(),
                        Color = dr["Color"].ToString(),
                        Warehouse = dr["Warehouse"].ToString(),
                        Location = dr["Location"].ToString(),
                        BatchNumber = dr["Batch number"].ToString(),
                        Pallet = dr["Serial number"].ToString(),
                        Available =decimal.Parse( dr["Qty"].ToString()),
                        SyncId = long.Parse(dr["SyncId"].ToString())
                    };

                    items.Add(oneItem);
                    iCount++;
                }

                
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                this.conn.Close();
            }

            

            return items;
        }


        public void ErrorInsert(string salesId, string pickingId, string errorString, string fullTrace, DateTime errorDate, string projectName,
            string username, string deviceName, string methodName, string parameters)
        {
            //SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnErrorDB"].ConnectionString);

            try
            {
                string sqlString = "INSERT INTO ErrorInfo (ProjectName, SalesId, PickingListID, ErrorString, Username, DeviceName, FullTrace, MethodName, Parameters) " +
                    "VALUES(@p1, @p2, @p3, @p5, @p6, @p7, @p8, @p9, @p10)";

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p3", SqlDbType.NVarChar);
                //cmd.Parameters.Add("@p4", SqlDbType.DateTime);
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p6", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p7", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p8", SqlDbType.NVarChar,4000);
                cmd.Parameters.Add("@p9", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p10", SqlDbType.NVarChar,4000);

                cmd.Parameters["@p1"].Value = projectName;
                cmd.Parameters["@p2"].Value = salesId;
                cmd.Parameters["@p3"].Value = pickingId;
                //cmd.Parameters["@p4"].Value = errorDate;
                cmd.Parameters["@p5"].Value = errorString;
                cmd.Parameters["@p6"].Value = username;
                cmd.Parameters["@p7"].Value = deviceName;
                cmd.Parameters["@p8"].Value = fullTrace;
                cmd.Parameters["@p9"].Value = methodName;
                cmd.Parameters["@p10"].Value = parameters;

                

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                object result = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public bool SavePing(DeviceMessage pingMsg)
        {
            
            try
            {
                string sqlString = "INSERT INTO DevicePing (Username, DeviceName, PingDate, DeviceIP, ProjectName) " +
                    "VALUES(@p1, @p2, @p3, @p4, @p5);";

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p3", SqlDbType.DateTime);
                cmd.Parameters.Add("@p4", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar);

                cmd.Parameters["@p1"].Value = pingMsg.Username;
                cmd.Parameters["@p2"].Value = pingMsg.DeviceName;
                cmd.Parameters["@p3"].Value = pingMsg.DateOccur;
                cmd.Parameters["@p4"].Value = pingMsg.DeviceIP;
                cmd.Parameters["@p5"].Value = string.IsNullOrEmpty(pingMsg.ProjectName)?"SaleService":pingMsg.ProjectName;

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return false;
        }

        public bool MessageDevice(DeviceMessage message, bool closeConn=true)
        {
            
            try
            {
                string sqlString = "INSERT INTO DeviceMessage (Username, DeviceName, MsgDate, DeviceIP, Message, MethodName, Parameter, ProjectName) " +
                    "VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8);";

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p3", SqlDbType.DateTime);
                cmd.Parameters.Add("@p4", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p6", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 4000);
                cmd.Parameters.Add("@p8", SqlDbType.NVarChar);

                cmd.Parameters["@p1"].Value = message.Username;
                cmd.Parameters["@p2"].Value = message.DeviceName;
                cmd.Parameters["@p3"].Value = message.DateOccur;
                cmd.Parameters["@p4"].Value = message.DeviceIP;
                cmd.Parameters["@p5"].Value = message.Message;
                cmd.Parameters["@p6"].Value = message.MethodName;
                cmd.Parameters["@p7"].Value = message.Parameters;
                cmd.Parameters["@p8"].Value = string.IsNullOrEmpty(message.ProjectName) ? "SaleService" : message.ProjectName;

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
            }
            finally
            {
                if (closeConn)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return false;
        }

        public List<DeviceMessage> MessagesDevice(List<DeviceMessage> messages)
        {
            foreach (DeviceMessage message in messages)
            {
                if (this.MessageDevice(message, false))
                    message.IsSaved = true;
            }

            if (conn.State != ConnectionState.Closed)
                conn.Close();

            return messages;
        }

        public string CheckPalletApprovedBulk(string pallets)
        {            
            string strReturn = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@pPallets", SqlDbType.VarChar);                
                cmd.Parameters.Add("@pReturn", SqlDbType.VarChar, 500);
                cmd.Parameters["@pReturn"].Direction = ParameterDirection.Output;

                cmd.Parameters["@pPallets"].Value = pallets;
                
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CheckPalletApproved";

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (cmd.Parameters["@pReturn"].Value != DBNull.Value)
                    strReturn = cmd.Parameters["@pReturn"].Value.ToString();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return strReturn;
        }

        /// <summary>
        /// Updates or Transfers the location of pallets in Bulk
        /// </summary>
        /// <param name="pallets">Pallets with Locations, comma seperated</param>
        /// <returns>Those pallets whose are not transferred because of IsPosted=1</returns>
        public string UpdatePalletLocationBulk(string pallets, string userName, string deviceName)
        {
            /*dbo.PalletLocTransfer
	            @pPallets VARCHAR(MAX),
	            @pReturn VARCHAR(MAX) output*/
            string strReturn = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@pPallets", SqlDbType.VarChar);
                cmd.Parameters.Add("@pDeviceName", SqlDbType.VarChar,50);
                cmd.Parameters.Add("@pUserName", SqlDbType.VarChar,10);
                cmd.Parameters.Add("@pReturn", SqlDbType.VarChar,500);
                cmd.Parameters["@pReturn"].Direction = ParameterDirection.Output;

                cmd.Parameters["@pPallets"].Value = pallets;
                cmd.Parameters["@pDeviceName"].Value = deviceName;
                cmd.Parameters["@pUserName"].Value = userName;

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PalletLocTransferNew";

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (cmd.Parameters["@pReturn"].Value != DBNull.Value)
                    strReturn = cmd.Parameters["@pReturn"].Value.ToString();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return strReturn;
        }

        /// <summary>
        /// Save Location transfer history time
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public bool SaveLocationTime(LocationHistory history, string message)
        {
            try
            {
                string sqlString = "INSERT INTO LocationHistory (PalletNum, DeviceName, CreateDateTime, Location, Message, IsManual, Username) " +
                    "VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7);";

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p3", SqlDbType.DateTime);
                cmd.Parameters.Add("@p4", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar);
                cmd.Parameters.Add("@p6", SqlDbType.Bit);
                cmd.Parameters.Add("@p7", SqlDbType.NVarChar);


                cmd.Parameters["@p1"].Value = history.PalletNum;
                cmd.Parameters["@p2"].Value = history.DeviceName;
                cmd.Parameters["@p3"].Value = history.CreateDateTime;
                cmd.Parameters["@p4"].Value = history.Location;
                cmd.Parameters["@p5"].Value = message;
                cmd.Parameters["@p6"].Value = history.IsManual;
                cmd.Parameters["@p7"].Value = history.UserName;


                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return false;
        }

        public bool PrintAgainPallet(string palletNum, long recordId, string deviceName, string deviceUser)
        {
            long rowsEffected = 0;
            
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertPrintAgain";

                cmd.Parameters.Add("@pPalletNum", SqlDbType.NVarChar, 50);
                cmd.Parameters.Add("@pRecordId", SqlDbType.BigInt);
                cmd.Parameters.Add("@pDeviceName", SqlDbType.NVarChar, 20);
                cmd.Parameters.Add("@pUsername", SqlDbType.NVarChar, 25);

                cmd.Parameters["@pPalletNum"].Value = palletNum;
                cmd.Parameters["@pRecordId"].Value = recordId;
                cmd.Parameters["@pDeviceName"].Value = deviceName;
                cmd.Parameters["@pUsername"].Value = deviceUser;

                if (this.conn.State != ConnectionState.Open)
                    this.conn.Open();

                rowsEffected = cmd.ExecuteNonQuery();

                
                //rowsEffected = (int)cmd.Parameters["@return"].Value;
                if (rowsEffected > 0)
                {
                    return true;
                }

            }
            finally
            {
                if (this.conn.State != ConnectionState.Closed)
                    this.conn.Close();
            }

            return false;
        }

        public List<ItemForChart> GetProductionByLines(DateTime date)
        {
            List<ItemForChart> items = new List<ItemForChart>();

            //SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Parameters.Add("@pDate", SqlDbType.DateTime);
                cmd.Parameters["@pDate"].Value = date;

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProdByLine";

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    ItemForChart oneItem = new ItemForChart()
                    {
                        LineOfOrigin = int.Parse(dr["LineOfOrigin"].ToString()),
                        LineName = dr["LineName"].ToString(),
                        DescGrade = dr["DescGrade"].ToString(),
                        BoxesOnPallet = int.Parse(dr["BoxesOnPallet"].ToString()),
                        SQM = decimal.Parse(dr["SQM"].ToString())
                        
                        
                    };
                    items.Add(oneItem);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return items;
        }

        public List<ItemForChart> GetProductionByItems(DateTime date)
        {
            List<ItemForChart> items = new List<ItemForChart>();

            //SqlConnection sConnMod = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cmd.Parameters.Add("@pDate", SqlDbType.DateTime);
                cmd.Parameters["@pDate"].Value = date;

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProdByItem";

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    ItemForChart oneItem = new ItemForChart()
                    {
                        ItemArticle = dr["ItemArticle"].ToString(),
                        ItemDesc = dr["DescItemArticle"].ToString(),
                        DescGrade = dr["DescGrade"].ToString(),
                        SQM = decimal.Parse(dr["SQM"].ToString()),
                        BoxesOnPallet = int.Parse(dr["BoxesOnPallet"].ToString())
                    };
                    items.Add(oneItem);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return items;
        }

        public List<DuplicatePallet> GetDuplicatePallets()
        {
            List<DuplicatePallet> pallets = new List<DuplicatePallet>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDuplicatePallets";

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    DuplicatePallet oneItem = new DuplicatePallet()
                    {
                        ItemArticle = dr["ItemArticle"].ToString(),
                        PalletNum = dr["PalletNum"].ToString(),
                        RecordId = int.Parse(dr["RecId"].ToString()),                        
                        WhichMarpak = int.Parse(dr["WhichMarpak"].ToString())
                    };
                    DateTime created =new DateTime();
                    if(DateTime.TryParse(dr["ExpTimestamp"].ToString(), out created))
                    {
                        oneItem.CreatedDateTime = created;
                    }
                    
                    pallets.Add(oneItem);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return pallets;
        }

        public bool ClearDuplicatePallet(DuplicatePallet pallet)
        {
            try
            {                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ClearDuplicatePallet";

                cmd.Parameters.Add("@pPalletNum", SqlDbType.NVarChar);
                cmd.Parameters.Add("@pRecordId", SqlDbType.BigInt);
                cmd.Parameters.Add("@pHasSeen", SqlDbType.Bit);

                cmd.Parameters["@pPalletNum"].Value = pallet.PalletNum;
                cmd.Parameters["@pRecordId"].Value = pallet.RecordId;
                cmd.Parameters["@pHasSeen"].Value = true;
                                

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return false;
        }

        public bool ClearDuplicatePalletsAll(List<DuplicatePallet> pallets)
        {
            SqlCommand cmd = null;
            int result = 0;

            try
            {
                foreach (DuplicatePallet pallet in pallets)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ClearDuplicatePallet";

                    cmd.Parameters.Add("@pPalletNum", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@pRecordId", SqlDbType.BigInt);
                    cmd.Parameters.Add("@pHasSeen", SqlDbType.Bit);

                    cmd.Parameters["@pPalletNum"].Value = pallet.PalletNum;
                    cmd.Parameters["@pRecordId"].Value = pallet.RecordId;
                    cmd.Parameters["@pHasSeen"].Value = true;


                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    result += cmd.ExecuteNonQuery();
                }
                if (result > 0)
                    return true;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return false;
        }
    }
}
