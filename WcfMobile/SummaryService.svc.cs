using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

//using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SummaryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SummaryService.svc or SummaryService.svc.cs at the Solution Explorer and start debugging.
    public class SummaryService : ISummaryService
    {
        public SalesServicesGroup.SummaryContract[] SummaryCases(string custAccount)
        {
            SalesServicesGroup.CallContext context = new SalesServicesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (SalesServicesGroup.CaseServiceClient client = new SalesServicesGroup.CaseServiceClient())
            {
                return client.GetCasesSummaryByCust(context, custAccount);

            }
        }

        public SalesServicesGroup.SummaryContract[] SummaryProdReq(string custAccount)
        {
            SalesServicesGroup.CallContext context = new SalesServicesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (SalesServicesGroup.ProdRequestServiceClient client = new SalesServicesGroup.ProdRequestServiceClient())
            {
                return client.GetProdRequestSummaryByCust(context, custAccount);

            }
        }

        public SalesOrderServices.CustomerDeliveryContract[] GetDeliverySummary(string customerId,string dateSearch)
        {
            SalesOrderServices.SOPickServiceClient client = new SalesOrderServices.SOPickServiceClient();
            SalesOrderServices.CallContext context = new SalesOrderServices.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetDailyDeliverySummaryByCust(context, customerId, Convert.ToDateTime(dateSearch));
        }

        public SalesOrderServices.CustomerDeliveryContract[] GetDeliveryInDatesSummary(string customerId, string dateStart, string dateEnd)
        {
            SalesOrderServices.SOPickServiceClient client = new SalesOrderServices.SOPickServiceClient();
            SalesOrderServices.CallContext context = new SalesOrderServices.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetDeliverySummaryByCust(context, customerId, Convert.ToDateTime(dateStart), Convert.ToDateTime(dateEnd));
        }

        public decimal CustomerBalance(string customerId)
        {
            SalesServicesGroup.CallContext context = new SalesServicesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (SalesServicesGroup.InventOnHandServiceClient client = new SalesServicesGroup.InventOnHandServiceClient())
            {
                return client.OutstandingBalance(context, customerId);
            }
        }
    }
}
