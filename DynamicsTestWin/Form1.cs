using DynamicsTestWin.SalesOrderAX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynamicsTestWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgTest.DataSource = GetLatestPallets("PKL20-005574");
        }

        private DataTable DbErrorCollection()
        {
            string connStr = "Data Source=FAC-PROD-VMHS;Initial Catalog=ErrorCollection;User Id=MobileDB;Password=123456;";
            SqlConnection conn = new SqlConnection(connStr);

            DataTable dt = new DataTable("errorinfo");
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            string sqlString = "select top 100 * from errorinfo order by DateOfError desc;";

            try
            {
                cmd.CommandText = sqlString;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                adapter.SelectCommand = cmd;

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        private DataTable DbDataManager()
        {
            string connStr = "Data Source=FAC-PROD-VMHS;Initial Catalog=DataManagerImExp;Persist Security Info=True;Integrated Security=SSPI;Trusted_Connection=True;";
            SqlConnection conn = new SqlConnection(connStr);

            DataTable dt = new DataTable("DMExport");
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            string sqlString = "SELECT top 20 * FROM DMExport order by ExpTimestamp desc;";

            try
            {
                cmd.CommandText = sqlString;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                adapter.SelectCommand = cmd;

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        private void checkMultiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgTest.DataSource = GetLatestPallets("PKL20-005574");
        }

        public List<SalesLineContract> GetLatestPallets(string pickingId)
        {
            SalesTableContract items = new SalesTableContract();
            //List<SalesLine> returnValue = new List<SalesLine>();
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = "ACMC"
            };



            items = client.findPickingList(context, pickingId);

            //string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
            // new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", "Error", "", DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);

            return items.SalesLines.ToList();
        }

        private List<SalesByWCF.FGDeliveryContract> GetDeliveries(DateTime dt)
        {
            SalesByWCF.SalesServiceClient client = new SalesByWCF.SalesServiceClient();
            return client.GetDeliveries(dt).ToList();
        }

        private void getFromWCFServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgTest.DataSource = GetDeliveries(DateTime.Now.Date);
        }
    }
}
