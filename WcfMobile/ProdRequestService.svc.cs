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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProdRequestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProdRequestService.svc or ProdRequestService.svc.cs at the Solution Explorer and start debugging.
    public class ProdRequestService : IProdRequestService
    {
        #region Production Request
        public ProdRequestContract GetProdRequest(string requestId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProductionRequest(context, requestId);
        }

        public ProdRequestContract[] GetProdRequestList(string customerId, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProductionRequestList(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdRequestContract[] GetProdRequestListBySize(string customerId, string size, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdRequestListBySize(context, customerId, size, int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdRequestContract[] GetProdRequestListBySizeAndMonth(string customerId, string size, string _month)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdRequestByMonthAndSize(context, customerId, size, int.Parse(_month));
        }

        public ProdRequestContract[] GetProdRequestListByItem(string customerId, string itemId, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdRequestListByItem(context, customerId, itemId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdRequestContract[] GetProdRequestListByItemNotFinished(string customerId, string itemId, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdRequestListByItemNotFinished(context, customerId, itemId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public string CreateProdRequest(ProdRequestContract row)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.CreateProdRequest(context, row);
        }

        public Boolean UpdateProdRequest(ProdRequestContract row, string requestId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.UpdateProdRequest(context, row, requestId);
        }

        public Boolean DeleteProdRequest(string requestId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();

            //var userCred=new System.ServiceModel.Security.UserNamePasswordClientCredential();
            client.ClientCredentials.UserName.UserName = "ax2";
            client.ClientCredentials.UserName.Password = "Dyn@n1c5Ax";
            
            return client.DeleteProdRequest(context, requestId);
        }

        public LookupContract[] GetProdRequestStatusLookup()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();

            return client.GetProdReqStatusLookup(context);
        }

        public ProdRequestContract[] GetProdRequestByStatusList(string customerId, string _status, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();

            return client.GetProdRequestByStatusList(context, customerId, int.Parse(pageNum), int.Parse(pageSize), int.Parse(_status));
        }

        public ProdRequestContract[] GetProdRequestByDateList(string customerId, string _date, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();

            return client.GetProdRequestByDateList(context, customerId, int.Parse(pageNum), int.Parse(pageSize), Convert.ToDateTime(_date));
        }

        #endregion



        #region Production Schedules Methods

        public ProdScheduleContract[] GetProdSchedules(string customerId, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdSchedules(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdScheduleContract[] GetProdSchedulesByItem(string customerId, string itemId, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdSchedulesByItem(context, customerId, itemId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdScheduleContract GetProdScheduleByRequest(string requestId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdSchedulesByRequest(context, requestId);
        }

        public ProdScheduleContract[] GetProdSchedulesByDate(string customerId, string _date, string pageNum, string pageSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            return client.GetProdSchedulesByDate(context, customerId, Convert.ToDateTime(_date), int.Parse(pageNum), int.Parse(pageSize));
        }

        public ProdScheduleDragDropContract[] GetProdSchedulesRearrange(string lineName, string startDate, string endDate)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdRequestServiceClient client = new ProdRequestServiceClient();
            var items = client.GetProdSchedulesRearrange(context, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), lineName);
            if (items != null && items.Count() > 0)
                items = items.OrderBy(t => t.ProductionLine).ThenBy(t => t.StartTimeProduction).ToArray();

            return items;
        }
        #endregion
    }
}
