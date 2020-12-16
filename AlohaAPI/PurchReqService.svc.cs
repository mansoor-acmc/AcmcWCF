using AlohaAPI.AlohaServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlohaAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PurchReqService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PurchReqService.svc or PurchReqService.svc.cs at the Solution Explorer and start debugging.
    public class PurchReqService : IPurchReqService
    {
        public string Create(PurchReqServiceContract entity)
        {
            string companyName = ConfigurationManager.AppSettings["DynamicsCompany"];
            CallContext context = new CallContext()
            {
                MessageId = new Guid().ToString(),
                Company = companyName
            };
            EcoResProductQueryServiceClient client = new EcoResProductQueryServiceClient();
            return client.CreatePurchaseReq(context, entity, companyName);
        }
    }
}
