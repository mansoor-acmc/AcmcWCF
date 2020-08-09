using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISummaryService" in both code and config file together.
    [ServiceContract]
    public interface ISummaryService
    {
        [OperationContract]
        [WebGet(UriTemplate = "SummaryCases/{custAccount}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SummaryContract[] SummaryCases(string custAccount);

        [OperationContract]
        [WebGet(UriTemplate = "SummaryProductionRequests/{custAccount}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SummaryContract[] SummaryProdReq(string custAccount);

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliveryDailySummary/{customerId}/{dateSearch}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SalesOrderServices.CustomerDeliveryContract[] GetDeliverySummary(string customerId, string dateSearch);

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliveryInDatesSummary/{customerId}/{dateStart}/{dateEnd}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SalesOrderServices.CustomerDeliveryContract[] GetDeliveryInDatesSummary(string customerId, string dateStart, string dateEnd);

        [OperationContract]
        [WebGet(UriTemplate = "CustOutstandingBalance/{customerId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        decimal CustomerBalance(string customerId);
    }
}
