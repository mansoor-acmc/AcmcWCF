using SoapUtility.MiscServiceGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEAMService" in both code and config file together.
    [ServiceContract]
    public interface IEAMService
    {
        [OperationContract]
        WorkitemContract[] PostedWorkItems();
    }
}
