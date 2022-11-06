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
using SoapUtility.InventCountingService;
using System.Configuration;
using SoapUtlUserMgt = SoapUtility.UserMgtServices;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FGSyncService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FGSyncService : IFGSyncService
    {
        public const string D365ServiceName = "InventCountingServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public FGSyncService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new JournalCountingServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }


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

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                var items = ((JournalCountingService)channel).GetYearFGInventory(new GetYearFGInventory(context, startId)).result;
                if (items.Count() > 0)
                    allRows = items.ToList();
            }

            return allRows;
        }

        public List<UserData> GetUserData()
        {
            return new UserMgtService(string.Empty).GetUserData("PalletScan");

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
