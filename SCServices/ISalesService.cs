using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncServices.SalesOrderAX;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalesService" in both code and config file together.
    [ServiceContract]
    public interface ISalesService
    {
        [OperationContract]
        SalesTable FindSalesOrder(string salesId);

        [OperationContract]
        SalesTable FindPickingList(string pickingId, string userName, string device);

        [OperationContract]
        List<SalesLine> ValidatePallets(string salesId, string itemId, string configId, string pickingId,
            List<string> pallets, string userName, string device, long lineRecId);

        [OperationContract]
        SalesLine CheckPalletAvailable(string salesId, string itemId, string configId, string pickingId, 
            string pallet, string userName, string device, DateTime dtSave, long lineRecId);

        [OperationContract]
        List<SalesLine> CheckPalletAvailableMulti(string salesId, string itemId, string configId, string pickingId, List<PalletItemContract> pallets,
            string userName, string device, long lineRecId);

        [OperationContract]
        string UnreservePallet(string pallet, string pickingId, string userName, string device);

        [OperationContract]
        List<SalesLine> GetLatestPallets(string pickingId, string itemId);

        [OperationContract]
        void SavePickingLoad(string pickingId, DateTime startLoad, DateTime stopLoad);

        [OperationContract]
        CustomerDeliveryContract[] CustomersDeliveryByQty(DateTime startDate, DateTime endDate);

        [OperationContract]
        CustomerDeliveryContract[] CustomersDeliveryByTrucks(DateTime startDate, DateTime endDate);

        [OperationContract]
        string GetPing();

        [OperationContract]
        SalesTable ReceivePickingList(string userName, string device);

        [OperationContract]
        void SaveLoginHistory(string userName, string device, string deviceIp, string projectName);

        [OperationContract]
        FGLineContract[] GetFGLines();

        [OperationContract]
        FGDeliveryContract[] GetDeliveries(DateTime dateSearch);
    }
}
