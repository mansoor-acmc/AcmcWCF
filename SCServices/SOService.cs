using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using AuthenticationUtility;
using SoapUtility.MiscServiceGroup;

namespace SyncServices
{
    public class SOService
    {
        public const string D365ServiceName = "MiscServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public SOService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new SOReserveClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }


        public List<SalesLine> PalletsReserve(string salesId, string itemId, string configId, string pickingId,
                List<string> pallets, string userName, string device, long lineRecId)
        {
            /*
             Already moved the code to SalesService.svc.cs
             */
            List<SalesLine> returnValue = new List<SalesLine>();
            /*AcmcSalesLineContract[] result = null;

            try
            {
                List<AcmcSalesLineContract> rows = new List<AcmcSalesLineContract>();
                foreach (string pallet in pallets)
                {
                    AcmcSalesLineContract row = new AcmcSalesLineContract() { Serial = pallet };
                    rows.Add(row);
                }
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    result = ((SOReserve)channel).PalletsReserving(new PalletsReserving(context, configId, itemId, lineRecId, pickingId, salesId, device, rows.ToArray(), userName)).result;
                }

                if (result != null && result.Length > 0)
                    returnValue = new SalesLine().ToAcmcListConvert(result.ToList());
            }
            catch (System.ServiceModel.FaultException aifExp)
            {
                string allMsgs = string.Empty;
                //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                //foreach (InfologMessage msg in msgs)
                //{
                //    allMsgs += msg.Message + Environment.NewLine;
                //}
                allMsgs = aifExp.Message;
                if (!string.IsNullOrEmpty(allMsgs))
                {
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallets:[" + string.Join(";", pallets) + "], Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallets:[" + string.Join(";", pallets) + "], Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);
                }
                catch { }
                throw exp;
            }*/

            return returnValue;
        }

    }
}