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
        public const string D365ServiceName = "SOPickServiceGroup";
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

            var client = new SOPickServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public SalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId)
        {
            SalesTableContract[] allContract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                allContract = ((SOPickService)channel).findSalesOrdersList(new findSalesOrdersList(context, customerId, Convert.ToDateTime(dateTo), Convert.ToDateTime(dateFrom))).result;
            }
                         

            return allContract;
        }

        public SalesTableContract FindSalesOrder(string salesId)
        {
            SalesTableContract contract = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((SOPickService)channel).findSalesOrder(new findSalesOrder(context, salesId)).result;
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

                contract = ((SOPickService)channel).GetCustomerDeliveries(new GetCustomerDeliveries(context, customerId, Convert.ToDateTime(dateSearch))).result;
            }

            return contract;
        }

        public string GetPing()
        {
            return "Service is working perfectly.";
        }
    }
}
