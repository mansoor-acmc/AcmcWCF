using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PrePickingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PrePickingService.svc or PrePickingService.svc.cs at the Solution Explorer and start debugging.
    public class PrePickingService : IPrePickingService
    {
        public OnhandByGradeContract[] GetOnhandStock(string custAccount)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.GetOnhandByGrade(context, custAccount);
        }

        public OnhandByGradeSizeColorContract[] GetOnhandStockByItem(string custAccount, string itemId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.GetOnhandByGradeSize(context, custAccount, itemId);
        }

        public OnhandByGradeSizeColorContract[] GetOnhandStockByItemWithOrdered(string custAccount, string itemId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.GetOnhandByGradeSizeWithOnOrder(context, custAccount, itemId);
        }

        public PrePickingContract[] GetAllPrePickings(string custAccount)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.GetPrePickings(context, custAccount);
        }

        public PrePickingContract GetPrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.GetSinglePrePicking(context, pickingId);
        }

        public LookupContract[] GetStatusLookup()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (PrePickingServiceClient client = new PrePickingServiceClient())
            {
                return client.GetStatusLookup(context);
            }
        }

        public string CreatePrePicking(PrePickingContract record)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.CreatePrePicking(context, record);
        }

        public bool UpdatePrePicking(PrePickingContract record, string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.UpdatePrePicking(context, pickingId, record);
        }

        public bool DeletePrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.DeletePrePicking(context, pickingId);
        }

        public bool SubmitStatusPrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.StatusUpdatePrePicking(context, pickingId, 1);
        }

        public bool ApproveStatusPrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.StatusUpdatePrePicking(context, pickingId, 2);
        }

        public bool CancelStatusPrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.StatusUpdatePrePicking(context, pickingId, 3);
        }
                

        public bool GenerateStatusPrePicking(string pickingId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PrePickingServiceClient client = new PrePickingServiceClient();
            return client.StatusUpdatePrePicking(context, pickingId, 4);
        }
    }
}
