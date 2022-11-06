using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFixedAssetService" in both code and config file together.
    [ServiceContract]
    public interface IFixedAssetService
    {
        [OperationContract]
        List<FAEntity> ResetData(string company);

        [OperationContract]
        string GetPing();

        [OperationContract]
        List<UserData> GetUserData(string companyName);

        [OperationContract]
        long UpdateFixedAssetDesktop(List<FAEntity> dt);
    }

    [DataContract]
    public class FAEntity
    {
        [DataMember]
        public string AssetId { get; set; }
        [DataMember]
        public string AssetGroupId { get; set; }
        [DataMember]
        public string AssetName { get; set; }
        [DataMember]
        public string BookId { get; set; }
        [DataMember] 
        public string Location { get; set; }
        [DataMember]
        public DateTime AcquireDate { get; set; }
        [DataMember]
        public decimal AcquirePrice { get; set; }
        [DataMember]
        public decimal PhysicalCount { get; set; }
        [DataMember]
        public decimal AvailableCount { get; set; }
        [DataMember]
        public bool ProcessByIT { get; set; }
        [DataMember]
        public bool IsMatched { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public long SyncId { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public DateTime LockDate { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public bool IsManual { get; set; }
        [DataMember]
        public string StatusFA { get; set; }
        [DataMember]
        public string Company { get; set; }
    }


}
