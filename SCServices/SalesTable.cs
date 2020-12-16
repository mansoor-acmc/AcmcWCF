using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncServices
{
    public class SalesTable
    {
        public string PickingId { get; set; }
        public string SalesId { get; set; }
        public string SalesName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryName { get; set; }
        public string DriverName { get; set; }
        public string TruckPlate { get; set; }
        public string DeliveryMode { get; set; }
        public List<SalesLine> Lines { get; set; }
        public bool HalfPallet { get; set; }
        public bool PickSameDimension { get; set; }
        public DateTime StartLoading { get; set; }
        public DateTime StopLoading { get; set; }

        public string TruckTicketNum { get; set; }
        public int LoadingLine { get; set; }
    }
}