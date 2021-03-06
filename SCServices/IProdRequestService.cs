﻿using SyncServices.SalesServicesGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProdRequestService" in both code and config file together.
    [ServiceContract]
    public interface IProdRequestService
    {
        [OperationContract]
        string[] GetAllProductionLines();

        [OperationContract]
        SLCapacityContract[] GetLineCapacities(string _prodLineId);

        [OperationContract]
        ProdScheduleDragDropContract[] GetNewProdSchedules(DateTime dateStart);

        [OperationContract]        
        ProdScheduleDragDropContract[] GetProdSchedulesRearrange(string lineName, DateTime startDate, DateTime endDate);

        [OperationContract]
        bool SaveProdSchedule(ProdScheduleDragDropContract[] lines);
    }
}
