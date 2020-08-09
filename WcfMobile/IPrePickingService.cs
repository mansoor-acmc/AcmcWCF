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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPrePickingService" in both code and config file together.
    [ServiceContract]
    public interface IPrePickingService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetOnhandStock/{custAccount}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnhandByGradeContract[] GetOnhandStock(string custAccount);

        [OperationContract]
        [WebGet(UriTemplate = "GetOnhandStockByItem/{custAccount}/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnhandByGradeSizeColorContract[] GetOnhandStockByItem(string custAccount, string itemId);

        [OperationContract]
        [WebGet(UriTemplate = "GetOnhandStockByItemWithOrdered/{custAccount}/{itemId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        OnhandByGradeSizeColorContract[] GetOnhandStockByItemWithOrdered(string custAccount, string itemId);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllPrePickings/{custAccount}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PrePickingContract[] GetAllPrePickings(string custAccount);

        [OperationContract]
        [WebGet(UriTemplate = "GetPrePicking/{pickingId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PrePickingContract GetPrePicking(string pickingId);

        [OperationContract]
        [WebGet(UriTemplate = "GetStatusLookup", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        LookupContract[] GetStatusLookup();




        [OperationContract]
        [WebInvoke(UriTemplate = "CreatePrePicking", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CreatePrePicking(PrePickingContract record);

        [OperationContract]
        [WebInvoke(UriTemplate = "UpdatePrePicking/{pickingId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool UpdatePrePicking(PrePickingContract record, string pickingId);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeletePrePicking/{pickingId}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool DeletePrePicking(string pickingId);

        [OperationContract]
        [WebInvoke(UriTemplate = "SubmitStatusPrePicking/{pickingId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool SubmitStatusPrePicking(string pickingId);              

        [OperationContract]
        [WebInvoke(UriTemplate = "ApproveStatusPrePicking/{pickingId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool ApproveStatusPrePicking(string pickingId);

        [OperationContract]
        [WebInvoke(UriTemplate = "CancelStatusPrePicking/{pickingId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool CancelStatusPrePicking(string pickingId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GenerateStatusPrePicking/{pickingId}", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool GenerateStatusPrePicking(string pickingId);
    }
}
