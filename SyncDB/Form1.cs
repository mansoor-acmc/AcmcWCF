using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.usersTableAdapter.Update(this.palletScanFGServerDataSet1.Users);

            // Call SyncAgent.Synchronize() to initiate the synchronization process.
            // Synchronization only updates the local database, not your project's data source.
            SCDataCacheSyncAgent syncAgent = new SCDataCacheSyncAgent();
            syncAgent.Users.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;

            Microsoft.Synchronization.Data.SyncStatistics syncStats = syncAgent.Synchronize();

            // TODO: Reload your project data source from the local database (for example, call the TableAdapter.Fill method).

            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'palletScanFGServerDataSet1.Users' table. You can move, or remove it, as needed.
          
            LoadData();
        }

        private void LoadData()
        {
        // TODO: This line of code loads data into the 'palletScanFGServerDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.palletScanFGServerDataSet1.Users);
        }
    }
}
