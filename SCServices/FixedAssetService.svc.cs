using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SoapUtility.InventCountingService;
using System.Configuration;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FixedAssetService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FixedAssetService.svc or FixedAssetService.svc.cs at the Solution Explorer and start debugging.
    public class FixedAssetService : IFixedAssetService
    {
        public const string D365ServiceName = "InventCountingServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public FixedAssetService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new JournalCountingServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            //context = new CallContext()
            //{
            //    MessageId = Guid.NewGuid().ToString(),
            //    Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            //};
        }

        public List<FAEntity> ResetData(string _company)
        {           
            //List<FAEntity> lst = new List<FAEntity>();
            //lst.Add(new FAEntity() { AssetId = "AC/HA00007", AssetGroupId = "A/C&/H/A", AssetName = "Samsung window A /c", BookId = "A/C&/H/A", AcquireDate = new DateTime(2013, 9, 30), AcquirePrice = 950, AvailableCount = 1, StatusFA = "Open" });
            //lst.Add(new FAEntity() { AssetId = "AC/HA00053", AssetGroupId = "A/C&/H/A", AssetName = "L.G Refrigerator 12 ft", BookId = "A/C&/H/A", AcquireDate = new DateTime(2013, 9, 30), AcquirePrice = 1360, AvailableCount = 1, StatusFA = "Open" });
            //lst.Add(new FAEntity() { AssetId = "AC/HA00335", AssetGroupId = "A/C&/H/A", AssetName = "AWT window A / c 2400", BookId = "A/C&/H/A", AcquireDate = new DateTime(2013, 9, 30), AcquirePrice = 550, AvailableCount = 1, StatusFA = "Open" });
            //lst.Add(new FAEntity() { AssetId = "CSC-00625", AssetGroupId = "C&S&C", AssetName = "LG & Class TV 65\" GM mr.Amro", BookId = "C&S&C", AcquireDate = new DateTime(2020, 12, 1), AcquirePrice = 5727, AvailableCount = 2, StatusFA = "Open" });
           
           

            return new DBClass().ResetDataFACount(_company);
            
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

        public List<UserData> GetUserData(string companyName) //Login username/passwords
        {
            return new UserMgtService(companyName).GetUserData("FACounting");
        }

        public long UpdateFixedAssetDesktop(List<FAEntity> dt)
        {
            return new DBClass().UpdateFixedAssetDesktop(dt);
        }

    }
}
