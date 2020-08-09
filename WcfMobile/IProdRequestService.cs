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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProdRequestService" in both code and config file together.
    [ServiceContract]
    public interface IProdRequestService
    {
        /*
            Production Request Methods 
        */

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequest/{requestId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract GetProdRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestList/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestList(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestListBySize/{customerId}/{size}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestListBySize(string customerId, string size, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestListByItem/{customerId}/{itemId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestListByItem(string customerId, string itemId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestListByItemNotFinished/{customerId}/{itemId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestListByItemNotFinished(string customerId, string itemId, string pageNum, string pageSize);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateProdRequest", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CreateProdRequest(ProdRequestContract row);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateProdRequest/{requestId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean UpdateProdRequest(ProdRequestContract row, string requestId);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteProdRequest/{requestId}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean DeleteProdRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestStatusLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] GetProdRequestStatusLookup();

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestByStatusList/{customerId}/{_status}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestByStatusList(string customerId, string _status, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdRequestByDateList/{customerId}/{_date}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProdRequestByDateList(string customerId, string _date, string pageNum, string pageSize);


        /*
        Production Schedules Methods 
        */
        [OperationContract]
        [WebGet(UriTemplate = "GetProductionSchedules/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdScheduleContract[] GetProdSchedules(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProductionSchedulesByItem/{customerId}/{itemId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdScheduleContract[] GetProdSchedulesByItem(string customerId, string itemId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdScheduleByRequest/{requestId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdScheduleContract GetProdScheduleByRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = "GetProductionSchedulesByDate/{customerId}/{_date}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdScheduleContract[] GetProdSchedulesByDate(string customerId, string _date, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProdSchedulesRearrange/{lineName}/{startDate}/{endDate}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdScheduleDragDropContract[] GetProdSchedulesRearrange(string lineName, string startDate, string endDate);
    }
}
