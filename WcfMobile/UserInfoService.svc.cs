using AuthenticationUtility;
using SoapUtility.UserMgtServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WcfMobile
{
    public class UserInfoService : IUserInfoService
    {
        public const string D365ServiceName = "UserMgtServices";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public UserInfoService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new UserManagementServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public bool LoginUser(string userId, string password)
        {
            bool bolReturn = false;            
            
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                var contract = new UserInfoContract()
                {
                    UserId = userId,
                    UserPassword = password,
                    NetworkDomain = "kabholding.com"
                };

                bolReturn = ((UserManagementService)channel).LoginUser(new LoginUser(context, contract)).result;                
            }

            return bolReturn;
        }

        public UserInfo GetUserInfo(string userId)
        {
            UserInfoContract user = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                var contract = new UserInfoContract()
                {
                    UserId = userId,
                    UserPassword = "",
                    NetworkDomain = "kabholding.com"
                };

                user = ((UserManagementService)channel).GetUserInfo(new GetUserInfo(context, false, contract)).result;
            }

            if (user != null)
                return new UserInfo().ToConvert(user);
            else
                return null;
        }

        public UserInfo GetUserInfoOnLogin(string userId, string password)
        {            
            UserInfoContract user = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                var contract = new UserInfoContract()
                {
                    UserId = userId,
                    UserPassword = password,
                    NetworkDomain = "kabholding.com"
                };

                user = ((UserManagementService)channel).GetUserInfoOnLogin(new GetUserInfoOnLogin(context, false, contract)).result;
            }

            if (user != null)
                return new UserInfo().ToConvert(user);
            else
                return null;
        }
    }
}
