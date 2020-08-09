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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProdPlanService" in both code and config file together.
    [ServiceContract]
    public interface IProdPlanService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetItemSizes", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ItemSizeContract[] GetItemSizes();

        [OperationContract]
        [WebGet(UriTemplate = "GetProductionLines", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string[] GetProdLines();



        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerPlan/{customerId}/{yearId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustForcastPlanContract[] GetCustPlan(string customerId, string yearId);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerPlanByMonth/{customerId}/{yearId}/{monthId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustForcastPlanContract[] GetCustPlanByMonth(string customerId, string yearId, string monthId);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerPlanBySize/{customerId}/{yearId}/{sizeId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustForcastPlanContract[] GetCustPlanBySize(string customerId, string yearId, string sizeId);

        [OperationContract]
        [WebGet(UriTemplate = "GetCustomerPlanByMonthSize/{customerId}/{yearId}/{monthId}/{sizeId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustForcastPlanContract[] GetCustPlanByMonthSize(string customerId, string yearId, string monthId, string sizeId);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateCustomerPlan", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        long CreateCustomerPlan(CustForcastPlanContract row);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateCustomerPlan/{recordId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void UpdateCustomerPlan(CustForcastPlanContract row, string recordId);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteCustomerPlan/{recordId}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean DeleteCustomerPlan(string recordId);


        [OperationContract]
        [WebGet(UriTemplate = "GetMonthlyForcastBySize/{customerId}/{yearId}/{monthId}/{itemSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ForcastSummaryContract[] GetMonthlyForcastBySize(string customerId, string itemSize, string yearId, string monthId);

        [OperationContract]
        [WebGet(UriTemplate = "GetMonthlyForcastSummary/{customerId}/{yearId}/{monthId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ForcastSummaryContract[] GetMonthlyForcastSummary(string customerId, string yearId, string monthId);

        [OperationContract]
        [WebGet(UriTemplate = "GetYearlyForcastBySize/{customerId}/{yearId}/{itemSize}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ForcastSummaryContract[] GetYearlyForcastBySize(string customerId, string yearId, string itemSize);
    }
}
