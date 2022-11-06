using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SoapUtlUserMgt = SoapUtility.UserMgtServices;
using SoapUtility.ProcurementGroup;
using AuthenticationUtility;
using System.Configuration;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PurchaseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PurchaseService.svc or PurchaseService.svc.cs at the Solution Explorer and start debugging.
    public class PurchaseService : IPurchaseService
    {
        public const string D365ServiceName = "ProcurementGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;
        string companyName = string.Empty;

        public PurchaseService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new PurchServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            companyName = ConfigurationManager.AppSettings["DynamicsCompany"];
            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = companyName
            };
        }
        

        public void SetCompanyName(string _company)
        {
            companyName = _company;
            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = _company
            };
        }

        public POHeader GetPurchOrderForGRN(string _purchId)
        {
            POHeader header = new POHeader();

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                header = ((PurchService)channel).GetPurchOrderForGRN(new GetPurchOrderForGRN(context, _purchId)).result;                
            }

            return header;
        }

        public POHeader[] GetAvailablePOs(DateTime selectDate)
        {
            POHeader[] headers ;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                headers = ((PurchService)channel).GetAvailablePOs(new GetAvailablePOs(context, selectDate)).result;                
            }

            return headers;
        }

        public string GenerateGRN(POHeader _header, string device, string updatedBy)
        {
            string result = string.Empty;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((PurchService)channel).GenerateGRN(new GenerateGRN(context, _header, device, updatedBy)).result;
            }
            return result;
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

        public List<UserData> GetUserData()
        {
            return new UserMgtService(companyName).GetUserData("PurchaseGRN");

        }

        public void SaveLoginHistory(string userName, string device, string deviceIp, string projectName)
        {
            new SalesService(companyName).SaveLoginHistory(deviceIp, device, projectName, userName);
        }
    }
}
