using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfMobile
{
    [DataContract]
    public class PMFailureCode
    {
        [DataMember]
        public string FailureCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}