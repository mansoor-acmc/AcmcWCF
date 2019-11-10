using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncServices.InventCountingService;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFGSyncService" in both code and config file together.
    [ServiceContract]
    public interface IFGSyncService
    {
        [OperationContract]
        List<PalletEntity> ResetData();

        [OperationContract]
        List<InventAvailContract> GetFGYearInventory(int startId);

        [OperationContract]
        List<UserData> GetUserData();

        [OperationContract]
        long UpdateFGDesktop(List<PalletEntity> dt);

        [OperationContract]
        string GetPing();

    }

    [DataContract]
    public class PalletEntity
    {
        [DataMember]
        public string Pallet { get; set; }
        [DataMember]
        public bool IsMatched { get; set; }
        [DataMember]
        public bool ProcessByIT { get; set; }
        [DataMember]
        public bool PresentInStock { get; set; }
        [DataMember]
        public bool IsManual { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public long SyncId { get; set; }

        [DataMember]
        public string ItemNumber { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Config { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
        [DataMember]
        public decimal Available { get; set; }
    }

    [DataContract]
    public class UserData
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string UserType { get; set; }
    }
}
