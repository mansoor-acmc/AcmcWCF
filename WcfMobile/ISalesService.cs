using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfMobile.SalesOrderServices;
using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalesService" in both code and config file together.
    [ServiceContract]
    public interface ISalesService
    {
        /// <summary>
        /// Get Sales-orders list between 2 dates for a specific customer
        /// </summary>
        /// <param name="dateFrom">From Date</param>
        /// <param name="dateTo">To Date (End Date)</param>
        /// <param name="customrId">Customer account number</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetSalesOrders/{customerId}/{dateFrom}/{dateTo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId);

        /// <summary>
        /// Get single Sales-order data along with all Sales-lines
        /// </summary>
        /// <param name="salesId">Sales-order number</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "FindSalesOrder/{salesId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        //[WebInvoke(Method ="Get", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,UriTemplate = "FindSalesOrder")]
        SalesTableContract FindSalesOrder(string salesId);

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliveryStatusLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        WcfMobile.SalesOrderServices.LookupContract[] GetDeliveryStatus();

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliveries/{customerId}/{dateSearch}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        FGDeliveryContract[] GetDeliveries(string dateSearch, string customerId);

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliveriesByStatus/{customerId}/{dateSearch}/{statusId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        FGDeliveryContract[] GetDeliveriesByStatus(string dateSearch, string customerId, string statusId);

        [OperationContract]
        [WebGet(UriTemplate = "GetDeliverySummary/{customerId}/{dateSearch}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerDeliveryContract[] GetDeliverySummary(string dateSearch, string customerId);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerOnHand/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnHandContract[] GetCustomerOnHand(string customerId, string pageNum, string pageSize); //add pagination and more than 0 stock   

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerOnHandQuery/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Tuple<int, OnHandContract[]> GetCustomerOnHandQuery(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerOnHandNoStock/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnHandContract[] GetCustomerOnHandNoStock(string customerId, string pageNum, string pageSize); //add pagination and with 0 stock

        [OperationContract]
        [WebGet(UriTemplate = "GetOnHandByItem/{itemNumber}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnHandContract[] GetOnHandByItem(string itemNumber);

        [OperationContract]
        [WebGet(UriTemplate = "GetProductionRequests/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProdRequestContract[] GetProductionRequests(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetInvoices/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerInvoiceContract[] GetCustomerInvoices(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetInvoicesBySales/{salesId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerInvoiceContract[] GetInvoicesBySales(string salesId);

        [OperationContract]
        [WebGet(UriTemplate = "GetInvoicesByDate/{customerId}/{_date}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerInvoiceContract[] GetCustomerInvoicesByDate(string customerId, string _date, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetInvoice/{invoiceId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerInvoiceContract GetInvoice(string invoiceId);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerOlderStock/{customerId}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OlderStockContract[] GetCustomerOlderStock(string customerId, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerOlderStockOverall/{customerId}/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OlderStockContract[] GetCustomerOlderStockOverall(string customerId, string itemId );

        [OperationContract]
        [WebGet(UriTemplate = "GetProductsByModel/{customerId}/{model}/{sortField}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Tuple<int, ProductContract[]> GetProductsByModel(string customerId, string model,string sortField, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProductsBySize/{customerId}/{size}/{sortField}/{pageNum}/{pageSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Tuple<int, ProductContract[]> GetProductsBySize(string customerId, string size, string sortField, string pageNum, string pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "GetProductInfo/{itemNumber}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProductContract GetProductInfo(string itemNumber);

        [OperationContract]
        [WebGet(UriTemplate = "GetOpenPickingLists/{customerId}/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PLNotDeliveredContract[] GetOpenPickingLists(string customerId, string itemId);


        [OperationContract]
        [WebGet(UriTemplate = "GetProductsByCustomerAll/{customerId}/{sortField}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProductContract[] GetProductsByCustomerAll(string customerId, string sortField);


        [OperationContract]
        [WebGet(UriTemplate = "Ping", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetPing();

        
    }
}
