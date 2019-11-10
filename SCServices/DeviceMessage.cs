using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SyncServices
{
    [DataContract]
    public class DeviceMessage
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public DateTime DateOccur { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string DeviceName { get; set; }

        [DataMember]
        public string  ProjectName { get; set; }

        [DataMember]
        public string DeviceIP { get; set; }

        [DataMember]
        public bool IsSaved { get; set; }

        [DataMember]
        public string Parameters { get; set; }

        [DataMember]
        public Int64 ID { get; set; }
    }
}