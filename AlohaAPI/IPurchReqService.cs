using AlohaAPI.AlohaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AlohaAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPurchReqService" in both code and config file together.
    [ServiceContract]
    public interface IPurchReqService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Create", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string Create(PurchReqServiceContract entity);
    }
}
