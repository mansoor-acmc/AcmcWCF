using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SoapUtility.UserMgtServices;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserMgtService" in code, svc and config file together.
    public class UserMgtService : IUserMgtService
    {
        public const string D365ServiceName = "UserMgtServices";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public UserMgtService()
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


        public UserInfo LoginUser(string userId, string password)
        {
            UserInfoContract user = null;
            //client.ChannelFactory.Credentials.UserName.UserName = @"kabholding.com\ax2";
            //client.ChannelFactory.Credentials.UserName.Password = "";
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

                bool isLoggedin = ((UserManagementService)channel).LoginUser(new LoginUser(context, contract)).result;
                if (isLoggedin)
                    user = ((UserManagementService)channel).GetUserInfo(new GetUserInfo(context, false, contract)).result;
            }
                       
                        
            if (user != null)
                return new UserInfo().ToConvert(user);
            else            
                return null;            
            
        }

        public UserInfo GetUserInfo(string userId, string password)
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

                user = ((UserManagementService)channel).GetUserInfo(new GetUserInfo(context, false, contract)).result;
            }

            if (user != null)
                return new UserInfo().ToConvert(user);
            else
                return null;            

        }

        

        public List<AttendanceContract> GetAttendances(string userId, string password, DateTime profileDate)
        {
            AttendanceContract[] attendances = null;
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

                attendances = ((UserManagementService)channel).GetCCAttendance(new GetCCAttendance(context, contract, profileDate)).result;
            }
                       

            if (attendances != null)
                return attendances.ToList();

            return new List<AttendanceContract>();                
        }

        public List<UserData> GetUserData(string projectName)
        {
            MobileUserLogin[] userLogins = null;
            List<UserData> allUser = new List<UserData>();

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                userLogins = ((UserManagementService)channel).GetMobileUsers(new GetMobileUsers(context, projectName)).result;
            }

            foreach (var oneUser in userLogins)
            {
                allUser.Add(new UserData()
                {
                    UserName = oneUser.UserLoginName,
                    Password = oneUser.UserPassword,
                    UserType = oneUser.UserRoleType
                });
            }
            return allUser;
        }

        public byte[] DownloadFile(ref string fileName,  string filePath)
        {
            byte[] fileBytes = SendToFileShare(fileName, filePath);

            return fileBytes;
        }

        public string GetPing()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "Your Web-Service IP Address is: " + ip.ToString();
                }
            }
            return "Sorry";
        }

        private byte[] SendToFileShare(string fileName, string filePath)
        {
            byte[] fileGot=new byte[0];
            
            //string networkShareLocation = @"\\172.17.0.150\Dyn_Attach_Doc\EamEqp\";

            var fullPath = filePath + fileName;


            //Credentials for the account that has write-access. Probably best to store these in a web.config file.
            //var domain = "kabholding.com";
            //var userID = "ax2";
            //var password = "Dyn@n1c5Ax";


            //if (ImpersonateUser(domain, userID, password) == true)
            //{
                //write the PDF to the share:
                //System.IO.File.WriteAllBytes(fullPath, fileGot);
                fileGot = File.ReadAllBytes(fullPath);
                //using (FileStream file = File.Open(fullPath, FileMode.Open))
                //{
                //    file.Write(content, 0, content.Length);

                //    fileGot = new byte[file.Length];
                //    file.Close();
                //}
                

                //undoImpersonation();
            //}
            //else
            //{
                //Could not authenticate account. Something is up.
                //Log or something.
            //}

            return fileGot;
        }

        /// <summary>
        /// Impersonates the given user during the session.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private bool ImpersonateUser(string domain, string userName, string password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        /// <summary>
        /// Undoes the current impersonation.
        /// </summary>
        private void undoImpersonation()
        {
            impersonationContext.Undo();
        }


        #region Impersionation global variables
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        #endregion
    }
}
