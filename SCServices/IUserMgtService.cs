using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncServices.UserMgtServices;
using System.IO;


namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserMgtService" in both code and config file together.
    [ServiceContract]
    public interface IUserMgtService
    {
        [OperationContract]
        UserInfo LoginUser(string userId, string password);

        [OperationContract]
        UserInfo GetUserInfo(string userId, string password);

        [OperationContract]
        List<AttendanceContract> GetAttendances(string userId, string password, DateTime profileDate);

        [OperationContract]
        byte[] DownloadFile(ref string fileName,  string filePath);

        [OperationContract]
        string GetPing();
    }

    [DataContract]
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
