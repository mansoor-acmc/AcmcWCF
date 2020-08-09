using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfMobile.EAMServices;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEAMService" in both code and config file together.
    [ServiceContract]
    public interface IEAMService
    {
        [OperationContract]
        [WebGet(UriTemplate = "WorkOrderStatusLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] WorkOrderStatusLookup();

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrderTypes", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<WOTypeContract> GetWorkOrderTypes();

        [OperationContract]
        [WebGet(UriTemplate = "GetEquipLocations", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<WOLocationContract> GetEquipLocations();

        [OperationContract]
        [WebGet(UriTemplate = "GetWOPools", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<WOPool> GetWOPools();

        [OperationContract]
        [WebGet(UriTemplate = "GetAllEquipments", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMEquipment> GetEquipments();

        [OperationContract]
        [WebGet(UriTemplate = "SearchEquipments/{search}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMEquipment> SearchEquipments(string search);

        [OperationContract]
        [WebGet(UriTemplate = "GetEquipmentsBySection/{section}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMEquipment> GetEquipmentsBySection(string section);



        [OperationContract]
        [WebGet(UriTemplate = "GetFailureCodes", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMFailureCode> GetFailureCodes();

        [OperationContract]
        [WebGet(UriTemplate = "GetRepairCodes", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMRepairCode> GetRepairCodes();

        [OperationContract]
        [WebGet(UriTemplate = "GetEquipment/{equipId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PMEquipment GetEquipment(string equipId);

        [OperationContract]
        [WebGet(UriTemplate = "GetItemMultiSource/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ItemEntity GetItemMultiSource(string itemId);

        [OperationContract]
        [WebGet(UriTemplate = "GetItem/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ItemEntity GetItem(string itemId);

        [OperationContract]
        [WebGet(UriTemplate = "SearchItems/{search}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<ItemEntity> GetItems(string search);

        [OperationContract]
        [WebGet(UriTemplate = "GetInlineWorkItems/{workOrderId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMWorkItem> GetInlineWorkItems(string workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "GetOtherWorkItems/{workOrderId}/{search}/{topRecords}/{isItemCode}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMWorkItem> GetOtherWorkItems(string workOrderId, string search, string topRecords, string isItemCode);

        

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrderList/{equipmentId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMWorkOrder> GetWorkOrderList(string equipmentId);

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrderListByDates/{startDate}/{endDate}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<PMWorkOrder> GetWorkOrderListByDates(string startDate, string endDate);

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrder/{workOrderId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PMWorkOrder GetWorkOrder(string workOrderId);

        [OperationContract]
        [WebInvoke(UriTemplate = "SaveWorkOrder", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PMWorkOrder SaveWorkOrder(PMWorkOrder entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetWorkOrderStatus/{workOrderId}/{statusId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        int SetWorkOrderStatus(string workOrderId, string statusId);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteWorkItem/{workOrderId}/{workItemId}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        int DeleteWorkItem(string workOrderId, string workItemId);
              

        [OperationContract]
        [WebGet(UriTemplate = "MainCostByProdLine/{startDate}/{endDate}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PMLineCostContract[] MainCostByProdLine(string startDate, string endDate);
    }
}
