using SoapUtility.UserMgtServices;
using System.Runtime.Serialization;

namespace WcfMobile
{
    public class UserInfo
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NetworkAliasName { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string ErrorLog { get; set; }

        public UserInfoContract FromConvert()
        {
            return new UserInfoContract()
            {
                UserId = this.UserId,
                UserFullName = this.FullName,
                Company = this.Company,
                NetworkAlias = this.NetworkAliasName,
                UserPassword = this.Password
            };
        }

        public UserInfo ToConvert(UserInfoContract axdline)
        {
            if (axdline != null)
            {
                return new UserInfo()
                {
                    UserId = axdline.UserId,
                    FullName = axdline.UserFullName,
                    Company = axdline.Company,
                    NetworkAliasName = axdline.NetworkAlias,
                    ErrorLog = axdline.ErrorLog
                };
            }
            else
                return new UserInfo();
        }
    }
}