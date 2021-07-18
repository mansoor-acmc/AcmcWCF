using AuthenticationUtility;
using SoapUtility.SalesServicesGroup;
using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProdRequestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProdRequestService.svc or ProdRequestService.svc.cs at the Solution Explorer and start debugging.
    public class ProdRequestService : IProdRequestService
    {
        public const string D365ServiceName = "SalesServicesGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public ProdRequestService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;
            
            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new ProdRequestServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public string[] GetAllProductionLines()
        {
            string[] allProdLines = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                allProdLines = ((SoapUtility.SalesServicesGroup.ProdRequestService)channel).GetAllProductionLines(new GetAllProductionLines(context)).result;
            }

            return allProdLines;
        }

        public SLCapacityContract[] GetLineCapacities(string _prodLineId)
        {
            SLCapacityContract[] aResult = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;               

                aResult = ((SoapUtility.SalesServicesGroup.ProdRequestService)channel).GetLineCapacities(new GetLineCapacities(context,_prodLineId)).result;
            }

            return aResult;
        }


        public ProdScheduleDragDropContract[] GetNewProdSchedules(DateTime dateStart)
        {
            ProdScheduleDragDropContract[] aResult = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                aResult = ((SoapUtility.SalesServicesGroup.ProdRequestService)channel).GetNewProdSchedules(new GetNewProdSchedules(context, dateStart)).result;
            }
                        
            return aResult;
        }

        public ProdScheduleDragDropContract[] GetProdSchedulesRearrange(string lineName, DateTime startDate, DateTime endDate)
        {
            ProdScheduleDragDropContract[] aResult = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                aResult = ((SoapUtility.SalesServicesGroup.ProdRequestService)channel).GetProdSchedulesRearrange(new GetProdSchedulesRearrange(context, lineName, endDate, startDate)).result;
            }
            
            //if (items != null && items.Count() > 0)
            //  items = items.Where(t=>t.RecordId>0).OrderBy(t=>t.ProductionLine).ThenBy(t => t.StartTimeProduction).ToArray();

            return aResult;
        }

        public bool SaveProdSchedule(ProdScheduleDragDropContract[] lines)
        {
            bool bResult = false;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                bResult = ((SoapUtility.SalesServicesGroup.ProdRequestService)channel).SaveProdSchedules(new SaveProdSchedules(context, lines)).result;
            }
            
            return bResult;
        }
    }
}
