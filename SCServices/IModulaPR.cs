using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SoapUtility.EAMServices;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModulaPR" in both code and config file together.
    [ServiceContract]
    public interface IModulaPR
    {
        [OperationContract]
        ItemEntity GetItem(string itemId);

        [OperationContract]
        List<ItemEntity> GetItems(string search);

        [OperationContract]
        List<WOPool> GetWOPools();

        [OperationContract]
        List<PMEquipment> GetEquipments();

        [OperationContract]
        List<PMEquipment> SearchEquipments(string search);

        [OperationContract]
        List<PMFailureCode> GetFailureCodes();

        [OperationContract]
        List<PMRepairCode> GetRepairCodes();

        [OperationContract]
        PMEquipment GetEquipment(string equipId);

        [OperationContract]
        List<WOTypeContract> GetWorkOrderTypes();

        [OperationContract]
        List<WOLocationContract> GetEquipLocations();

        [OperationContract]
        List<PMWorkItem> GetInlineWorkItems(string workOrderId);

        [OperationContract]
        List<PMWorkItem> GetOtherWorkItems(string workOrderId, string search, int topRecords, bool isItemCode);



        [OperationContract]
        PMWorkOrder GetlatestWorkOrder(string workOrderId);

        [OperationContract]
        PMWorkOrder SaveWorkOrder(PMWorkOrder entity);

        [OperationContract]
        int SetWorkOrderStatus(string workOrderId, int statusId);

        [OperationContract]
        int DeleteWorkItem(string workOrderId, int sto_id);

        [OperationContract]
        byte[] DownloadFile(ref string fileName, string filePath);

        [OperationContract]
        PMLineCostContract[] MainCostByProdLine(DateTime startDate, DateTime endDate);

        [OperationContract]
        string GetPing();
    }

    

    [DataContract]
    public class WOPool
    {
        [DataMember]
        public string WorkOrderPoolCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class PMEquipment
    {
        [DataMember]
        public string EquipmentID { get; set; }
        [DataMember]
        public string EquipmentName { get; set; }
        [DataMember]
        public string EquipmentGroupCode { get; set; }
        [DataMember]
        public string LocationCode { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public List<EquipCatalog> Attachments { get; set; }
    }

    [DataContract]
    public class PMFailureCode
    {
        [DataMember]
        public string FailureCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class PMRepairCode
    {
        [DataMember]
        public string FailureCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string RepairCode { get; set; }
    }

    [DataContract]
    public class EquipCatalog
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FileExtension { get; set; }
        [DataMember]
        public string FileDescription { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public string FileSize { get; set; }

        public EquipCatalog ToConvert(EquipCatalogContract item)
        {
            return new EquipCatalog()
            {
                FileName = item.FileName,
                FileExtension = item.FileExtension,
                FileDescription = item.FileDescription,
                FilePath = item.FilePath
                //FileSize = item.FileSize
            };
        }
        public List<EquipCatalog> ToListConvert(List<EquipCatalogContract> lines)
        {
            List<EquipCatalog> result = new List<EquipCatalog>();
            foreach (EquipCatalogContract line in lines)
                result.Add(new EquipCatalog().ToConvert(line));

            return result;
        }
    }

    [DataContract]
    public class PMWorkOrder
    {
        [DataMember]
        public long TablePk { get; set; }
        [DataMember]
        public string WorkOrderID { get; set; }
        [DataMember]
        public string WOPoolCode { get; set; }
        [DataMember]
        public string WOType { get; set; }
        [DataMember]
        public string WOStatus { get; set; }
        [DataMember]
        public string WOEquipment { get; set; }
        [DataMember]
        public string FailureCode { get; set; }
        [DataMember]
        public string RepairCode { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public string RequestBy { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public List<PMWorkItem> WorkItems { get; set; }
        [DataMember]
        public string Description { get; set; }

        public PMWorkOrder ToConvert(PMWorkOrderContract wo)
        {
            var item = new PMWorkOrder()
            {
                WorkOrderID = wo.WorkOrderID,
                WOPoolCode = wo.WorkOrderPoolCode,
                WOType = wo.WorkOrderType.ToString(),
                WOEquipment = wo.EquipmentID,
                RepairCode = wo.RepairCode,
                StartDate = wo.WOStartDate,
                WOStatus=wo.WorkOrderStatus.ToString(),
                Description = wo.Description,
                WorkItems = new PMWorkItem().ToListConvert(wo.WorkOrderItems.ToList())
            };

            if (item.WOStatus.Equals("WorkInProgress"))
                item.WOStatus = "Started";
            return item;
        }

        public PMWorkOrderContract FromConvert()
        {
            if (this.WOStatus.Equals("Started"))
                this.WOStatus = "WorkInProgress";
            return new PMWorkOrderContract()
            {
                WorkOrderID = this.WorkOrderID,
                Description = this.Description,
                WorkOrderPoolCode = this.WOPoolCode,
                WorkOrderType =(PMWorkOrderType)Enum.Parse(typeof(PMWorkOrderType), this.WOType),
                //WorkOrderTypeSpecified=true,
                EquipmentID = this.WOEquipment,
                RepairCode = this.RepairCode,
                WOStartDate = this.StartDate.Value,
                //WOStartDateSpecified=true,
                WorkOrderStatus = (PMWorkOrderStatus)Enum.Parse(typeof(PMWorkOrderStatus), this.WOStatus),
                //WorkOrderStatusSpecified=true,
                WorkOrderItems = new PMWorkItem().FromListConvert(this.WorkItems).ToArray()
            };
        }
    }

    [DataContract]
    public class PMWorkItem
    {
        [DataMember]
        public long WOPkId { get; set; }
        [DataMember]
        public string WorkOrderID { get; set; }
        [DataMember]
        public string ItemID { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string UoM { get; set; }
        [DataMember]
        public decimal Qty { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public long ItemRecId { get; set; }
        [DataMember]
        public int Sto_ID { get; set; }
        [DataMember]
        public string Sto_Requester { get; set; }
        [DataMember]
        public string Sto_HostRIF { get; set; }
        [DataMember]
        public int ID { get; set; }

        public PMWorkItem ToConvert(PMWorkItemContract item)
        {
            return new PMWorkItem()
            {
                ItemID = item.ItemId,
                ItemName = item.ItemName,
                UoM = item.PMUnit,
                Location = item.Location,
                Qty = item.QuantityConsumed,
                ItemRecId = item.ItemRecId,
                Sto_ID  =   item.Sto_ID,
                Sto_Requester = item.Sto_Requester,
                Sto_HostRIF = item.Sto_HostRIF,
                ID = item.ItemPKId
            };
        }
        public List<PMWorkItem> ToListConvert(List<PMWorkItemContract> lines)
        {
            List<PMWorkItem> result = new List<PMWorkItem>();
            foreach (PMWorkItemContract line in lines)
                result.Add(new PMWorkItem().ToConvert(line));

            return result;
        }

        public PMWorkItemContract FromConvert(PMWorkItem item)
        {
            return new PMWorkItemContract()
            {
                ItemId = item.ItemID,
                ItemName = item.ItemName,
                PMUnit = item.UoM,
                Location = item.Location,
                QuantityConsumed = item.Qty,
                //QuantityConsumedSpecified = true,
                ItemRecId = item.ItemRecId,
                //ItemRecIdSpecified = true,
                Sto_ID = item.Sto_ID,
                //Sto_IDSpecified = true,
                Sto_Requester = item.Sto_Requester,
                Sto_HostRIF = item.Sto_HostRIF,
                ItemPKId = item.ID,
                //ItemPKIdSpecified = true
            };
        }
        public List<PMWorkItemContract> FromListConvert(List<PMWorkItem> lines)
        {
            List<PMWorkItemContract> result = new List<PMWorkItemContract>();
            foreach (PMWorkItem line in lines)
                result.Add(new PMWorkItem().FromConvert(line));

            return result;
        }
    }
}
