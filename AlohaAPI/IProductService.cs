using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AlohaAPI.AlohaServices;

namespace AlohaAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProductService" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetProduct/{id}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        EcoResProductContract GetProduct(string id);


        [OperationContract]
        [WebGet(UriTemplate = "GetProducts/{search}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        EcoResProductContract[] GetProducts(string search);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateProduct", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CreateProduct(EcoResProductContract entity);
    }
}
