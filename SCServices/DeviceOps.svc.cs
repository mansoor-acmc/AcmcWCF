using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{    
    public class DeviceOps : IDeviceOps
    {
        public bool Ping(DeviceMessage msg)
        {
            return new DBClass(SyncServices.DBClass.DbName.DeviceMsg).SavePing(msg);
        }

        public bool SaveMessage(DeviceMessage msg)
        {
            return new DBClass(SyncServices.DBClass.DbName.DeviceMsg).MessageDevice(msg);
        }

        public List<DeviceMessage> SaveMessages(List<DeviceMessage> msgs)
        {
            return new DBClass(SyncServices.DBClass.DbName.DeviceMsg).MessagesDevice(msgs);
        }
    }
}
