using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WcfMobile.EAMServices;

namespace WcfMobile
{
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
                WOStatus = wo.WorkOrderStatus.ToString(),
                Description = wo.Description
            };

            if (wo.WorkOrderItems != null && wo.WorkOrderItems.Count() > 0)
                item.WorkItems = new PMWorkItem().ToListConvert(wo.WorkOrderItems.ToList());

            if (this.WOStatus != null && item.WOStatus.Equals("WorkInProgress"))
                item.WOStatus = "Started";
            return item;
        }

        public PMWorkOrderContract FromConvert()
        {
            if (this.WOStatus != null && this.WOStatus.Equals("Started"))
                this.WOStatus = "WorkInProgress";
            var item = new PMWorkOrderContract()
            {
                WorkOrderID = this.WorkOrderID,
                Description = this.Description,
                WorkOrderPoolCode = this.WOPoolCode,
                WorkOrderType = (PMWorkOrderType)Enum.Parse(typeof(PMWorkOrderType), this.WOType),
                WorkOrderTypeSpecified = true,
                EquipmentID = this.WOEquipment,
                RepairCode = this.RepairCode,
                WOStartDate = this.StartDate.Value,
                WOStartDateSpecified = true
            };

            if (this.WOStatus != null && this.WOStatus.Length > 0)
            {
                item.WorkOrderStatus = (PMWorkOrderStatus)Enum.Parse(typeof(PMWorkOrderStatus), this.WOStatus);
                item.WorkOrderStatusSpecified = true;
            }

            if (this.WorkItems != null && this.WorkItems.Count() > 0)
                item.WorkOrderItems = new PMWorkItem().FromListConvert(this.WorkItems).ToArray();

            return item;
        }
    }
}