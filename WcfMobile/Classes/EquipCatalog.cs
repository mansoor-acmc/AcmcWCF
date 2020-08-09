using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WcfMobile.EAMServices;

namespace WcfMobile
{
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
}