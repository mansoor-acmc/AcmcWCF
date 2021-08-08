using SoapUtility.SalesServicesGroup;
using SoapUtility.SOPickServiceGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalesOrderService" in both code and config file together.
    [ServiceContract]
    public interface ISalesOrderService
    {
        /// <summary>
        /// Get Sales-orders list between 2 dates for a specific customer
        /// </summary>
        /// <param name="dateFrom">From Date</param>
        /// <param name="dateTo">To Date (End Date)</param>
        /// <param name="customrId">Customer account number</param>
        /// <returns></returns>
        [OperationContract]        
        SalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId);

        /// <summary>
        /// Get single Sales-order data along with all Sales-lines
        /// </summary>
        /// <param name="salesId">Sales-order number</param>
        /// <returns></returns>
        [OperationContract]        
        //[WebInvoke(Method ="Get", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,UriTemplate = "FindSalesOrder")]
        SalesTableContract FindSalesOrder(string salesId);

        [OperationContract]        
        //[WebGet(UriTemplate = "GetDeliveries/{customerId}/{dateSearch}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        FGDeliveryContract[] GetDeliveries(string dateSearch, string customerId);

        [OperationContract]
        [WebGet(UriTemplate = "Ping", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]        
        string GetPing();
    }
}
