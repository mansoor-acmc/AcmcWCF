using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SyncServices.InventCountingService;

namespace SyncServices
{
    [ServiceContract]
    public interface ISCSyncService
    {
        [OperationContract]
        List<ItemEntity> ResetData();

        [OperationContract]
        List<ItemEntity> GetSCUnitOfMeasure();

        [OperationContract]
        List<InventForItemUnit> GetSCUnitOfMeasureFromAX(string allItems);

        [OperationContract]
        List<UserData> GetUserData();

        [OperationContract]
        long UpdateSCDesktop(List<ItemEntity> dt);

        [OperationContract]
        string GetPing();

        // TODO: Add your service operations here
    }

    [DataContract]
    public class ItemEntity
    {
        [DataMember]
        public string Item { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string UoM { get; set; }
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
    }


    
}
