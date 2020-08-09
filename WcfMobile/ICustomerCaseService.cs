using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
//using WcfMobile.CustomerCases;
using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerCaseService" in both code and config file together.
    [ServiceContract]
    public interface ICustomerCaseService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetCategoryLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CategoryTypeLookup[] GetCategory();

        [OperationContract]
        [WebGet(UriTemplate = "GetStatusLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] GetStatusLookup();

        [OperationContract]
        [WebGet(UriTemplate = "GetResolutionLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] GetResolutionLookup();

        [OperationContract]
        [WebGet(UriTemplate = "GetPriorityLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] GetPriorityLookup();

        [OperationContract]
        [WebGet(UriTemplate = "GetCase/{caseId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerCaseContract GetCustomerCase(string caseId);

        [OperationContract]
        [WebGet(UriTemplate = "FindCases/{custAccount}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        CustomerCaseContract[] FindCustomerCases(string custAccount);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteCase/{caseId}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean DeleteCustomerCase(string caseId);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateCase", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CreateCustomerCase(CustomerCaseContract caseDetail);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateCase/{caseId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean UpdateCustomerCase(CustomerCaseContract caseDetail,string caseId);
    }
}
