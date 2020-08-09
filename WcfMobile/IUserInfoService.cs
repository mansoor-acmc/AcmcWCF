using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;
using WcfMobile.UserServices;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserInfoService" in both code and config file together.
    [ServiceContract]
    public interface IUserInfoService
    {
        [OperationContract]
        [WebGet(UriTemplate = "LoginUser/{userId}/{password}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool LoginUser(string userId, string password);

        [OperationContract]
        [WebGet(UriTemplate = "GetUserInfo/{userId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        UserInfo GetUserInfo(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "GetUserInfoOnLogin/{userId}/{password}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        UserInfo GetUserInfoOnLogin(string userId, string password);
    }
}
