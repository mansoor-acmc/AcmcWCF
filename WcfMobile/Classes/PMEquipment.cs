using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfMobile
{
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
}