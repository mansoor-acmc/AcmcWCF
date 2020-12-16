using SyncServices.SalesServicesGroup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProdRequestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProdRequestService.svc or ProdRequestService.svc.cs at the Solution Explorer and start debugging.
    public class ProdRequestService : IProdRequestService
    {

        public string[] GetAllProductionLines()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetAllProductionLines(context);
        }

        public SLCapacityContract[] GetLineCapacities(string _prodLineId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetLineCapacities(context, _prodLineId);
        }

        public ProdScheduleDragDropContract[] GetNewProdSchedules(DateTime dateStart)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetNewProdSchedules(context, dateStart);
        }

        public ProdScheduleDragDropContract[] GetProdSchedulesRearrange(string lineName, DateTime startDate, DateTime endDate)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            var items = client.GetProdSchedulesRearrange(context, startDate, endDate, lineName);
            //if (items != null && items.Count() > 0)
              //  items = items.Where(t=>t.RecordId>0).OrderBy(t=>t.ProductionLine).ThenBy(t => t.StartTimeProduction).ToArray();

            return items;
        }

        public bool SaveProdSchedule(ProdScheduleDragDropContract[] lines)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.SaveProdSchedules(context, lines);
        }
    }
}
