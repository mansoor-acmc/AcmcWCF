using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//using WcfMobile.CustomerCases;
using WcfMobile.SalesServicesGroup;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerCaseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerCaseService.svc or CustomerCaseService.svc.cs at the Solution Explorer and start debugging.
    public class CustomerCaseService : ICustomerCaseService
    {
        public CategoryTypeLookup[] GetCategory()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetCategoryTypeLookup(context);
            }
        }

        public LookupContract[] GetStatusLookup()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetStatusLookup(context);
            }
        }

        public LookupContract[] GetResolutionLookup()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetResolutionLookup(context);
            }
        }

        public LookupContract[] GetPriorityLookup()
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetPriorityLookup(context);
            }
        }

        public CustomerCaseContract GetCustomerCase(string caseId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            
            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetCase(context, caseId);
                
            }
        }

        public CustomerCaseContract[] FindCustomerCases(string custAccount)
        {
            WcfMobile.SalesServicesGroup.CallContext context = new WcfMobile.SalesServicesGroup.CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

                      
            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.GetCustomerCases(context, custAccount);                
            }

        }

        public Boolean DeleteCustomerCase(string caseId)
        {
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            
            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.DeleteCustomerCase(context, caseId);
               
            }
        }

        public string CreateCustomerCase(CustomerCaseContract caseDetail)
        {
            if (caseDetail is null)
                throw new Exception("Cannot create case. CaseDetail is null.");


            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

           
            using (CaseServiceClient client = new CaseServiceClient())
            {
                var result = client.CreateCustomerCase(context, caseDetail);
                return result;
            }
        }

        public Boolean UpdateCustomerCase( CustomerCaseContract caseDetail, string caseId)
        {
            if (caseDetail is null)
                throw new Exception("Cannot update case. CaseDetail is null.");

            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            
            using (CaseServiceClient client = new CaseServiceClient())
            {
                return client.UpdateCustomerCase(context, caseDetail, caseId);
            }
        }


    }
}
