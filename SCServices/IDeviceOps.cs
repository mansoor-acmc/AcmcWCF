using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDeviceOps" in both code and config file together.
    [ServiceContract]
    public interface IDeviceOps
    {
        [OperationContract]
        bool Ping(DeviceMessage msg);

        [OperationContract]
        bool SaveMessage(DeviceMessage msg);

        [OperationContract]
        List<DeviceMessage> SaveMessages(List<DeviceMessage> msgs);
    }
}
