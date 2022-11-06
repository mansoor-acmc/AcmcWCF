using SoapUtility.ProcurementGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPurchaseService" in both code and config file together.
    [ServiceContract]
    public interface IPurchaseService
    {
        [OperationContract]
        void SetCompanyName(string company);

        [OperationContract]
        POHeader GetPurchOrderForGRN(string _purchId);

        [OperationContract]
        string GenerateGRN(POHeader _header, string device, string updatedBy);

        [OperationContract]
        POHeader[] GetAvailablePOs(DateTime selectDate);


        [OperationContract]
        string GetPing();

        [OperationContract]
        List<UserData> GetUserData();

        [OperationContract]
        void SaveLoginHistory(string userName, string device, string deviceIp, string projectName);
    }
}
