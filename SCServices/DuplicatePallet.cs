using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SyncServices
{
    [DataContract]
    public class DuplicatePallet
    {
        [DataMember]
        public string PalletNum { get; set; }
        [DataMember]
        public DateTime? CreatedDateTime { get; set; }
        [DataMember]
        public int RecordId { get; set; }
        [DataMember]
        public int WhichMarpak { get; set; }
        [DataMember]
        public string ItemArticle { get; set; }
    }
}