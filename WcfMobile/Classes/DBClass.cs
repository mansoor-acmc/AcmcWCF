using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WcfMobile
{
    public class DBClass
    {
        public ItemEntity GetItem(string itemId)
        {
            ItemEntity oneItem = new ItemEntity()
            {
                Item = itemId
            };

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
                if (dr.Read())
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
    }
}