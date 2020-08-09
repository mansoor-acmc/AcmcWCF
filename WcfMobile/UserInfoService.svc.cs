using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfMobile.UserServices;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserInfoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserInfoService.svc or UserInfoService.svc.cs at the Solution Explorer and start debugging.
    public class UserInfoService : IUserInfoService
    {
        public bool LoginUser(string userId, string password)
        {
            bool bolReturn = false;
            UserManagementServiceClient client = new UserManagementServiceClient();

            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            //client.ChannelFactory.Credentials.UserName.UserName = @"kabholding.com\ax2";
            //client.ChannelFactory.Credentials.UserName.Password = "Dyn@n1c5Ax";

            bolReturn = client.LoginUser(context, new UserInfoContract()
            {
                UserId = userId,
                UserPassword = password,
                NetworkDomain = "kabholding.com"
            });

            client.Close();

            return bolReturn;

        }

        public UserInfo GetUserInfo(string userId)
        {
            UserManagementServiceClient client = new UserManagementServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            UserInfoContract user = client.GetUserInfo(context, new UserInfoContract()
            {
                UserId = userId,
                UserPassword = "",
                NetworkDomain = "kabholding.com"
            }, false);
            client.Close();

            if (user != null)
                return new UserInfo().ToConvert(user);
            else
                return null;

        }

        public UserInfo GetUserInfoOnLogin(string userId, string password)
        {
            UserManagementServiceClient client = new UserManagementServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            UserInfoContract user = client.GetUserInfoOnLogin(context, new UserInfoContract()
            {
                UserId = userId,
                UserPassword = password,
                NetworkDomain = "kabholding.com"
            }, false);
            client.Close();

            if (user != null)
                return new UserInfo().ToConvert(user);
            else
                return null;
        }
    }
}
