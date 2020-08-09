using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfMobile.SalesOrderServices;
using salesGroup=WcfMobile.SalesServicesGroup;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SalesService.svc or SalesService.svc.cs at the Solution Explorer and start debugging.
    public class SalesService : ISalesService
    {
        public SalesTableContract[] GetSalesOrders(string dateFrom, string dateTo, string customerId)
        {

            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesTableContract[] allContract = client.findSalesOrdersList(context, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), customerId);
            client.Close();

            return allContract;
        }

        public SalesTableContract FindSalesOrder(string salesId)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesTableContract contract = client.findSalesOrder(context, salesId);
            client.Close();

            return contract;
        }

        public LookupContract[] GetDeliveryStatus()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (SOPickServiceClient client = new SOPickServiceClient())
            {
                return client.GetDeliveryStatusLookup(context);
            }
        }

        public FGDeliveryContract[] GetDeliveries(string dateSearch, string customerId)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            
            return client.GetCustomerDeliveries(context, Convert.ToDateTime(dateSearch), customerId);
        }

        public FGDeliveryContract[] GetDeliveriesByStatus(string dateSearch, string customerId, string statusId)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            PickingListStatus status = (PickingListStatus)int.Parse(statusId);
            return client.GetCustomerDeliveriesByStatus(context, Convert.ToDateTime(dateSearch), customerId, status);
        }

        

        public CustomerDeliveryContract[] GetDeliverySummary(string dateSearch, string customerId)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            
            return client.CustomersDeliveryByTrucks(context, Convert.ToDateTime(dateSearch), Convert.ToDateTime(dateSearch).AddDays(7));
        }

        public salesGroup.OnHandContract[] GetCustomerOnHand(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetOnHandByCustomer(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public Tuple<int, salesGroup.OnHandContract[]> GetCustomerOnHandQuery(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var lines = client.GetOnhandQueryByCustomer(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
            int recordCount = int.Parse(lines[0].ItemNumber);

            if (recordCount > 0)
            {
                var list = lines.ToList();
                list.RemoveAt(0);
                lines = list.ToArray();
            }


            return Tuple.Create(recordCount, lines);


        }

        public salesGroup.OnHandContract[] GetCustomerOnHandNoStock(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetOnHandByCustomerZeroStock(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public salesGroup.OnHandContract[] GetOnHandByItem(string itemNumber)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetOnHandByItem(context, itemNumber);
        }

        public salesGroup.ProdRequestContract[] GetProductionRequests(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.getProductionRequests(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public salesGroup.CustomerInvoiceContract[] GetCustomerInvoices(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetCustomerInvoices(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
        }

        public salesGroup.CustomerInvoiceContract[] GetCustomerInvoicesByDate(string customerId, string _date, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetCustomerInvoicesByDate(context, customerId,Convert.ToDateTime(_date), int.Parse(pageNum), int.Parse(pageSize));
        }


        public salesGroup.CustomerInvoiceContract[] GetInvoicesBySales(string salesId)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetInvoicesBySalesId(context, salesId);
        }

        public salesGroup.CustomerInvoiceContract GetInvoice(string invoiceId)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetInvoice(context, invoiceId);
        }

        public  salesGroup.OlderStockContract[] GetCustomerOlderStock(string customerId, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var lines = client.GetCustomerOlderStock(context, customerId, int.Parse(pageNum), int.Parse(pageSize));
            //int recordCount = int.Parse(lines[0].ItemNumber);

            //if (recordCount > 0)
            //{
            //    var list = lines.ToList();
            //    list.RemoveAt(0);
            //    lines = list.ToArray();
            //}


            //return Tuple.Create(recordCount, lines);
            return lines;
        }

        public salesGroup.OlderStockContract[] GetCustomerOlderStockOverall(string customerId, string itemId)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            itemId = itemId.Replace("_", "") + "*";
            var lines = client.GetCustomerOlderStockOverall(context, customerId, itemId);

            return lines;
        }

        public Tuple<int, salesGroup.ProductContract[]> GetProductsByModel(string customerId, string model, string sortField, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var lines = client.GetProductsByModel(context, customerId, model, sortField, int.Parse(pageNum), int.Parse(pageSize));
            return Tuple.Create(lines.TotalRecords, lines.Records);
        }

        public Tuple<int, salesGroup.ProductContract[]> GetProductsBySize(string customerId, string size, string sortField, string pageNum, string pageSize)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            salesGroup.ProductContract rec = client.GetProductsBySize(context, customerId, size, sortField, int.Parse(pageNum), int.Parse(pageSize));

            return Tuple.Create(rec.TotalRecords, rec.Records);
            //JObject o = JObject.FromObject(new
            //{
            //    RecordsCount = rec.TotalRecords,
            //    AllRecord = from line in rec.Records
            //                select new
            //                {
            //                    ItemId = line.ItemNumber,
            //                    ItemName = line.ProductName,
            //                    ShortDesc = line.ShortDescription,
            //                    Size = line.ItemSize,
            //                    Classify = line.Classification,
            //                    CustomerID = line.CustomerAccount
            //                }
            //}
            //);

            //return o;

        }

        public salesGroup.ProductContract[] GetProductsByCustomerAll(string customerId, string sortField)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            return client.GetProductsByCustomerAll(context, customerId, sortField);
        }

        public salesGroup.ProductContract GetProductInfo(string itemNumber)
        {
            salesGroup.InventOnHandServiceClient client = new salesGroup.InventOnHandServiceClient();
            salesGroup.CallContext context = new salesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetProductInfo(context, itemNumber);
        }

        public PLNotDeliveredContract[] GetOpenPickingLists(string customerId, string itemId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesOrderServices.SOPickServiceClient client = new SOPickServiceClient();
            return client.GetOpenPickingListByCustByItem(context, customerId, itemId);
        }




        public string GetPing()
        {
            return "Service is working perfectly.";
        }
    }
}
