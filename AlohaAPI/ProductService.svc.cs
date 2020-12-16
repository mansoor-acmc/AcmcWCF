using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AlohaAPI.AlohaServices;

namespace AlohaAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductService.svc or ProductService.svc.cs at the Solution Explorer and start debugging.
    public class ProductService : IProductService
    {
        public EcoResProductContract GetProduct(string id)
        {
            CallContext context = new CallContext()
            {
                MessageId = new Guid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            EcoResProductQueryServiceClient client = new EcoResProductQueryServiceClient();
            return client.GetProductInfo(context, id);
        }

        public EcoResProductContract[] GetProducts(string search)
        {
            CallContext context = new CallContext()
            {
                MessageId = new Guid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            EcoResProductQueryServiceClient client = new EcoResProductQueryServiceClient();
            return client.GetProducts(context, search);
        }

        public string CreateProduct(EcoResProductContract entity)
        {
            string companyName = ConfigurationManager.AppSettings["DynamicsCompany"];
            CallContext context = new CallContext()
            {
                MessageId = new Guid().ToString(),
                Company = companyName
            };
            EcoResProductQueryServiceClient client = new EcoResProductQueryServiceClient();
            return client.CreateProductMaster(context, entity, companyName);
        }
    }
}
