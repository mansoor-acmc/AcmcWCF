using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WcfMobile.EAMServices;

namespace WcfMobile
{
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
                Sto_ID = item.Sto_ID,
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
                QuantityConsumedSpecified = true,
                ItemRecId = item.ItemRecId,
                ItemRecIdSpecified = true,
                Sto_ID = item.Sto_ID,
                Sto_IDSpecified = true,
                Sto_Requester = item.Sto_Requester,
                Sto_HostRIF = item.Sto_HostRIF,
                ItemPKId = item.ID,
                ItemPKIdSpecified = true
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