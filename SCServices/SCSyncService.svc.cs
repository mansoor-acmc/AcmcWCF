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
using SoapUtlUserMgt = SoapUtility.UserMgtServices;
using AuthenticationUtility;
using System.ServiceModel.Channels;

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
            return new UserMgtService().GetUserData("SCCounting");            

            //allUser.Add(new UserData() { UserName = "mansoor", Password = "12345aa", UserType = "Admin" });
            //allUser.Add(new UserData() { UserName = "usman", Password = "us12345", UserType = "Supervisor" });
            //allUser.Add(new UserData() { UserName = "emran", Password = "em2019", UserType = "Supervisor" });
            //allUser.Add(new UserData() { UserName = "abdullah", Password = "ab2019", UserType = "Employee" });
            //allUser.Add(new UserData() { UserName = "zaid", Password = "za2019", UserType = "Employee" });
            //allUser.Add(new UserData() { UserName = "basem", Password = "ba2019", UserType = "Supervisor" });
            //allUser.Add(new UserData() { UserName = "oliver", Password = "ol2019", UserType = "Employee" });
                        
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


        /*-------------------------------------18/10/2020------------------------------------------------------*/

        
        public List<WmsLocationContract> GetWHLocations()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            JournalCountingServiceClient client = new JournalCountingServiceClient();
            return client.GetWHLocations(context).ToList();
        }

        public List<SCForTransfer> TransferItemsToNewLocation(List<SCForTransfer> lines)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            JournalCountingServiceClient client = new JournalCountingServiceClient();
            var linesFromAX = client.UpdateTransferItems(context, lines.ToArray());

            return linesFromAX.ToList();
        }
    }
}
