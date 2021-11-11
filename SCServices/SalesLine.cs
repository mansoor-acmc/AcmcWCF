﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using misc = SoapUtility.MiscServiceGroup;
//using SyncServices.SalesOrderAX;
using SoapUtility.SOPickServiceGroup;

namespace SyncServices
{
    public class SalesLine
    {
        public string SalesId { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public decimal? SalesQty { get; set; }
        public decimal? SalesQtySQM { get; set; }
        public decimal? SalesQtySQMReserved { get; set; }
        public decimal? SalesQtySQMRemaining { get; set; }
        public decimal? SalesQtyPallet { get; set; }
        public decimal? SalesQtyBox { get; set; }
        public string SalesUnit { get; set; }
        public string Grade { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Site { get; set; }
        public string Warehouse { get; set; }
        public string SerialNumber { get; set; }
        public string PickingId { get; set; }
        public string Remarks { get; set; }
        public bool IsHalfPallet { get; set; }
        public bool ExclusiveHalfPallet { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdateType { get; set; } //1=Find, 2=Delete, 3=Reserve, 4=Un-Reserve
        public long LineRecId { get; set; }
        public string Location { get; set; }
        public InventByGrLocContract[] Locations { get; set; }

        public EVSSalesLineContract FromConvert(SalesLine axdline)
        {
            return new EVSSalesLineContract()
            {
                SalesId = axdline.SalesId,
                ItemId = axdline.ItemId,
                Name = axdline.Name,
                SalesQty = axdline.SalesQty.GetValueOrDefault(),
                //SalesQtySpecified = true,
                SalesQtyBox = axdline.SalesQtyBox.GetValueOrDefault(),
                //SalesQtyBoxSpecified = true,
                SalesQtyPallet = axdline.SalesQtyPallet.GetValueOrDefault(),
                //SalesQtyPalletSpecified = true,
                SalesQtySQM = axdline.SalesQtySQM.GetValueOrDefault(),
                //SalesQtySQMSpecified = true,
                SalesQtySQMReserved = axdline.SalesQtySQMReserved.GetValueOrDefault(),
                //SalesQtySQMReservedSpecified = true,
                SalesQtySQMRemaining = axdline.SalesQtySQMRemaining.GetValueOrDefault(),
                //SalesQtySQMRemainingSpecified = true,
                SalesUnit = axdline.SalesUnit,
                Grade = axdline.Grade,
                Shade = axdline.Color,
                Size = axdline.Size,
                Site = axdline.Site,
                Warehouse = axdline.Warehouse,
                Serial = axdline.SerialNumber,
                PickingId = axdline.PickingId,
                wLocationId = axdline.Location,
                Remarks = axdline.Remarks,
                IsHalfPallet = axdline.IsHalfPallet,
                //IsHalfPalletSpecified = true,
                ExclusiveHalfPallet = axdline.ExclusiveHalfPallet,
                //ExclusiveHalfPalletSpecified = true,
                LineRecId=axdline.LineRecId,
                //LineRecIdSpecified=true,
                Locations = axdline.Locations
            };
        }
        public SalesLine ToConvert(EVSSalesLineContract axdline)
        {
            string locs = string.Empty;
            if (axdline.Locations != null && axdline.Locations.Count() > 0)
            {
                locs = string.Join(",", (axdline.Locations.Select(t => t.LocationId)));
            }
            return new SalesLine()
            {
                SalesId = axdline.SalesId,
                ItemId = axdline.ItemId,
                Name = axdline.Name,
                SalesQty = axdline.SalesQty,
                SalesQtyBox = axdline.SalesQtyBox,
                SalesQtyPallet = axdline.SalesQtyPallet,
                SalesQtySQM = axdline.SalesQtySQM,
                SalesQtySQMReserved = axdline.SalesQtySQMReserved,
                SalesQtySQMRemaining = axdline.SalesQtySQMRemaining,
                SalesUnit = axdline.SalesUnit,
                Grade = axdline.Grade,
                Color = axdline.Shade,
                Size = axdline.Size,
                Site = axdline.Site,
                Warehouse = axdline.Warehouse,
                SerialNumber = axdline.Serial,
                PickingId = axdline.PickingId,
                Location = locs,
                Remarks = axdline.Remarks,
                IsHalfPallet = axdline.IsHalfPallet,
                ExclusiveHalfPallet = axdline.ExclusiveHalfPallet,
                LineRecId = axdline.LineRecId,
                Locations = axdline.Locations
            };
            
        }
        public List<SalesLine> ToListConvert(List<EVSSalesLineContract> lines)
        {
            List<SalesLine> result = new List<SalesLine>();
            foreach (EVSSalesLineContract line in lines)
                result.Add(new SalesLine().ToConvert(line));
            
            return result;
        }



        public SalesLine ToConvert(misc.AcmcSalesLineContract axdline)
        {
            string locs = string.Empty;
            if (axdline.Locations != null && axdline.Locations.Count() > 0)
            {
                locs = string.Join(",", (axdline.Locations.Select(t => t.LocationId)));
            }
            return new SalesLine()
            {
                SalesId = axdline.SalesId,
                ItemId = axdline.ItemId,
                Name = axdline.Name,
                SalesQty = axdline.SalesQty,
                SalesQtyBox = axdline.SalesQtyBox,
                SalesQtyPallet = axdline.SalesQtyPallet,
                SalesQtySQM = axdline.SalesQtySQM,
                SalesQtySQMReserved = axdline.SalesQtySQMReserved,
                SalesQtySQMRemaining = axdline.SalesQtySQMRemaining,
                SalesUnit = axdline.SalesUnit,
                Grade = axdline.Grade,
                Color = axdline.Shade,
                Size = axdline.Size,
                Site = axdline.Site,
                Warehouse = axdline.Warehouse,
                SerialNumber = axdline.Serial,
                PickingId = axdline.PickingId,
                Location = locs,
                Remarks = axdline.Remarks,
                IsHalfPallet = axdline.IsHalfPallet,
                ExclusiveHalfPallet = axdline.ExclusiveHalfPallet,
                LineRecId = axdline.LineRecId,
                //Locations = axdline.Locations
            };

        }

        public List<SalesLine> ToAcmcListConvert(List<misc.AcmcSalesLineContract> lines)
        {
            List<SalesLine> result = new List<SalesLine>();
            foreach (misc.AcmcSalesLineContract line in lines)
                result.Add(new SalesLine().ToConvert(line));

            return result;
        }
    }
}