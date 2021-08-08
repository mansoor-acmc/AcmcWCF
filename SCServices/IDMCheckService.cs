using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SoapUtility.DataManagerServices;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDMCheckService" in both code and config file together.
    [ServiceContract]
    public interface IDMCheckService
    {
        [OperationContract]
        string GetPing();

        [OperationContract]
        DMExportContract GetPalletInfo(string palletNum);

        [OperationContract]
        DMExportContract GetPalletInfoByRecordId(long recordId);

        [OperationContract]
        bool ConfirmPalletReceive(string palletNum, long recordId, string deviceName, string deviceUser, bool isFromSL);

        [OperationContract]
        string ConfirmPalletAndLocationReceive(string palletNum, string locationId, long recordId, string deviceName, string deviceUser, bool isFromSL);

        [OperationContract]
        bool CancelPalletReceive(string palletNum, long recordId, string deviceName, string deviceUser);

        [OperationContract]
        bool UpdateAndConfirmPalletReceive(DMExportContract pallet);

        [OperationContract]
        bool PrintAgainPallet(string palletNum, long recordId, string deviceName, string deviceUser);

        [OperationContract]
        List<DMExportMiniContract> DMClearPrintAgain();

        [OperationContract]
        List<DMExportContract> UpdateOfflinePallets(List<DMExportOfflineContract> lines);

        [OperationContract]
        List<LocationHistory> TransferPalletsToNewLocation(List<LocationHistory> lines);

        [OperationContract]
        List<DMSummaryContract> SummaryPallets(string itemId);

        [OperationContract]
        List<DMExportContract> ItemGroupPallets(string itemId, string grade, string shade, string caliber);

        [OperationContract]
        bool CreateDowntimeForMarpak(int whichMarpak);

        [OperationContract]
        List<ItemForChart> GetProductionByLinesForChart(DateTime date);

        [OperationContract]
        List<ItemForChart> GetProductionByItemsForChart(DateTime date);

        [OperationContract]
        List<WmsLocationContract> GetWHLocations();

        [OperationContract]
        List<DuplicatePallet> GetDuplicatePallets();

        [OperationContract]
        bool ClearDuplicatePallet(DuplicatePallet pallet);

        [OperationContract]
        bool ClearDuplicatePalletsAll(List<DuplicatePallet> pallets);
    }

    [DataContract]
    public class ItemForChart
    {
        [DataMember]
        public string ItemArticle { get; set; }
        [DataMember]
        public string ItemDesc { get; set; }
        [DataMember]
        public string DescGrade { get; set; }

        [DataMember]
        public decimal SQM { get; set; }
        [DataMember]
        public int BoxesOnPallet { get; set; }
        [DataMember]
        public int LineOfOrigin { get; set; }
        [DataMember]
        public string LineName { get; set; }
    }
}
