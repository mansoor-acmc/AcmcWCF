using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using SyncServices.InventCountingService;
using System.Configuration;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FGSyncService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FGSyncService : IFGSyncService
    {
        public List<PalletEntity> ResetData()
        {
            return new DBClass().ResetDataFGCount();
        }

        public long UpdateFGDesktop(List<PalletEntity> dt)
        {
            return new DBClass().UpdateFGCountingDesktop(dt);
        }

        public List<InventAvailContract> GetFGYearInventory(int startId) 
        {
            List<InventAvailContract> allRows = new List<InventAvailContract>();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            InventCountingService.JournalCountingServiceClient client = new JournalCountingServiceClient();
            var items = client.GetYearFGInventory(context, startId);
            if (items.Count() > 0)
                allRows = items.ToList();

            return allRows;
        }

        public List<UserData> GetUserData()
        {
            List<UserData> allUser = new List<UserData>();

            //UserType are: Admin, Supervisor, Employee
            UserMgtServices.CallContext context = new UserMgtServices.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };


            UserMgtServices.UserManagementServiceClient client = new UserMgtServices.UserManagementServiceClient();
            var userLogins = client.GetMobileUsers(context, "PalletScan");
            foreach(var oneUser in userLogins)
            {
                allUser.Add(new UserData()
                {
                    UserName = oneUser.UserLoginName,
                    Password = oneUser.UserPassword,
                    UserType = oneUser.UserRoleType
                });
            }
                        
            return allUser;
        }

        public string GetPing()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "Your Sync Web-Service IP Address is: " + ip.ToString();
                }
            }
            return "Sorry";
        }

    }
}
