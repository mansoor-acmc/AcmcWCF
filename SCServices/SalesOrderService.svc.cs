using SyncServices.SalesOrderAX;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesOrderService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SalesOrderService.svc or SalesOrderService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SalesOrderService : ISalesOrderService
    {
        public SalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId)
        {
           
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesTableContract[] allContract = client.findSalesOrdersList(context,Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), customerId);
            client.Close();

            return allContract;
        }

        public SalesTableContract FindSalesOrder(string salesId)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesTableContract contract = client.findSalesOrder(context, salesId);
            client.Close();

            return contract;
        }

        public FGDeliveryContract[] GetDeliveries(string dateSearch, string customerId)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetCustomerDeliveries(context, Convert.ToDateTime(dateSearch), customerId);
        }

        public string GetPing()
        {
            return "Service is working perfectly.";
        }
    }
}
