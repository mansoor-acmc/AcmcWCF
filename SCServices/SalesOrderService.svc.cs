using AuthenticationUtility;
using SoapUtility.SOPickServiceGroup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesOrderService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SalesOrderService.svc or SalesOrderService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SalesOrderService : ISalesOrderService
    {
        public const string D365ServiceName = "EVSSOPickServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public SalesOrderService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new EVSSOPickServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public EVSSalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId)
        {
            EVSSalesTableContract[] allContract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                allContract = ((EVSSOPickService)channel).findSalesOrdersList(new findSalesOrdersList(context, customerId, Convert.ToDateTime(dateTo), Convert.ToDateTime(dateFrom))).result;
            }
                         

            return allContract;
        }

        public EVSSalesTableContract FindSalesOrder(string salesId)
        {
            EVSSalesTableContract contract = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((EVSSOPickService)channel).findSalesOrder(new findSalesOrder(context, salesId)).result;
            }
                        
            return contract;
        }

        public FGDeliveryContract[] GetDeliveries(string dateSearch, string customerId)
        {
            FGDeliveryContract[] contract = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((EVSSOPickService)channel).GetCustomerDeliveries(new GetCustomerDeliveries(context, customerId, Convert.ToDateTime(dateSearch))).result;
            }

            return contract;
        }

        public string GetPing()
        {
            return "Service is working perfectly.";
        }
    }
}
