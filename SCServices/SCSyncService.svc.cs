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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SCSyncService : ISCSyncService
    {          
        public long UpdateSCDesktop(List<ItemEntity> dt)
        {
            return new DBClass().UpdateSupplyChainDesktop(dt);
        }

        

        public List<ItemEntity> ResetData()
        {
            return new DBClass().ResetDataSCCount();
        }

        public List<ItemEntity> GetSCUnitOfMeasure()
        {
            return new DBClass().GetSCUnitOfMeasure();
        }

        public List<InventForItemUnit> GetSCUnitOfMeasureFromAX(string allItems)
        {
            List<InventForItemUnit> allRows = new List<InventForItemUnit>();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            InventCountingService.JournalCountingServiceClient client = new  JournalCountingServiceClient();
            var items = client.GetItemUoM(context, allItems);
            if (items.Count() > 0)
                allRows = items.ToList();

            return allRows;
        }

        public List<UserData> GetUserData()
        {
            List<UserData> allUser = new List<UserData>();

            //UserType are: Admin, Supervisor, Employee

            allUser.Add(new UserData() { UserName = "mansoor", Password = "12345aa", UserType = "Admin" });
            allUser.Add(new UserData() { UserName = "mardini", Password = "mar12345", UserType = "Supervisor" });
            allUser.Add(new UserData() { UserName = "usman", Password = "usm12345", UserType = "Supervisor" });
            allUser.Add(new UserData() { UserName = "hamza", Password = "h2018", UserType = "Employee" });
            allUser.Add(new UserData() { UserName = "ayman", Password = "a2018", UserType = "Employee" });
            allUser.Add(new UserData() { UserName = "hassan", Password = "m2018", UserType = "Employee" });
            allUser.Add(new UserData() { UserName = "bakar", Password = "b2018", UserType = "Employee" });
            allUser.Add(new UserData() { UserName = "omer", Password = "o2018", UserType = "Employee" });

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
