using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModulaMovement" in both code and config file together.
    [ServiceContract]
    public interface IModulaMovement
    {
        [OperationContract]
        bool OpenItemCode(int itemId, string time);

        [OperationContract]
        DataTable ItemsNotUsed();

        [OperationContract]
        DataTable ItemsNotUsedWithWOId();

        [OperationContract]
        DataTable ItemsUsedButNotPosted();
    }
}
