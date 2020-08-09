using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfMobile
{
    [DataContract]
    public class WOPool
    {
        [DataMember]
        public string WorkOrderPoolCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}