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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProdPlanService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProdPlanService.svc or ProdPlanService.svc.cs at the Solution Explorer and start debugging.
    public class ProdPlanService : IProdPlanService
    {
        public ItemSizeContract[] GetItemSizes()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetItemSizes(context);
        }

        public string[] GetProdLines()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            var lines =  client.GetSortingLines(context);

            return lines;
        }



        public CustForcastPlanContract[] GetCustPlan(string customerId, string yearId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetCustPlan(context, customerId, int.Parse(yearId));
        }

        public CustForcastPlanContract[] GetCustPlanByMonth(string customerId, string yearId, string monthId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetCustPlanByMonth(context, customerId, int.Parse(yearId), monthId);
        }

        public CustForcastPlanContract[] GetCustPlanBySize(string customerId, string yearId, string sizeId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetCustPlanBySize(context, customerId, int.Parse(yearId), sizeId);
        }

        public CustForcastPlanContract[] GetCustPlanByMonthSize(string customerId, string yearId, string monthId, string sizeId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetCustPlanByMonthSize(context, customerId, int.Parse(yearId), monthId, sizeId);
        }

        public long CreateCustomerPlan(CustForcastPlanContract row)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.CreateCustPlan(context, row);
        }

        public void UpdateCustomerPlan(CustForcastPlanContract row, string recordId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            client.UpdateCustPlan(context, row, Int64.Parse(recordId));
        }

        public Boolean DeleteCustomerPlan(string recordId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.DeleteCustPlan(context, Int64.Parse(recordId));
        }

        public ForcastSummaryContract[] GetMonthlyForcastBySize(string customerId, string itemSize, string yearId, string monthId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetMonthlyForcastBySize(context, customerId, itemSize, int.Parse(yearId), monthId);
        }

        public ForcastSummaryContract[] GetMonthlyForcastSummary(string customerId, string yearId, string monthId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetMonthlyForcastSummary(context, customerId, int.Parse(yearId), monthId);
        }

        public ForcastSummaryContract[] GetYearlyForcastBySize(string customerId, string yearId, string itemSize)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            ProdPlanServiceClient client = new ProdPlanServiceClient();
            return client.GetYearlyForcastBySize(context, customerId, int.Parse(yearId),  itemSize);
        }
    }
}
