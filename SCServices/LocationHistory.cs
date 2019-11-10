using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SyncServices
{
    [DataContract]
    public class LocationHistory
    {
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string PalletNum { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime CreateDateTime { get; set; }
    }
}