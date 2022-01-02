using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SoapUtility.MiscServiceGroup;
using System.Configuration;
using AuthenticationUtility;
using System.ServiceModel.Channels;
using System.Data;
using System.Data.SqlClient;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EAMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EAMService.svc or EAMService.svc.cs at the Solution Explorer and start debugging.
    public class EAMService : IEAMService
    {
        public const string D365ServiceName = "MiscServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public EAMService()
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

        public WorkitemContract[] PostedWorkItems()
        {
            string conString = ConfigurationManager.AppSettings["modulaImportConString"];
            SqlConnection conn = new SqlConnection(conString);
            WorkitemContract[] workItems = null;
            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    var items =((SOReserve)channel).WorkItemsPosted(new WorkItemsPosted(context));
                }

                //***Mansoor***use actual service WorkItemsService
                
                string mov_ids = string.Empty;
                foreach (SoapUtility.MiscServiceGroup.WorkitemContract dr in workItems)
                {
                    if (mov_ids.Length > 0) mov_ids += ",";
                    mov_ids += dr.StoID;
                }

                if (!string.IsNullOrEmpty(mov_ids))
                {
                    conn.Open();

                    string sqlText = "UPDATE EXP_MOVIMENTI SET Mov_Deleted=1 WHERE MOV_ID IN (" + mov_ids + ")";
                    SqlCommand comm = new SqlCommand(sqlText, conn);
                    comm.CommandType = CommandType.Text;
                    int rowsEffected = comm.ExecuteNonQuery();
                    
                }
            }
            catch (Exception exp)
            {
                string err = exp.Message;
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
    }
}
