using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SoapUtility.EAMServices;
using System.Configuration;
using System.Data;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModulaPR" in code, svc and config file together.
    public class ModulaPR : IModulaPR
    {
        public const string D365ServiceName = "EAMServices";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public ModulaPR()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new WorkItemsServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public ItemEntity GetItem(string itemId)
        {
            return new DBClass().GetItem(itemId);
        }

        public List<ItemEntity> GetItems(string search)
        {
            return new DBClass().GetItems(search);
        }

        public List<WOPool> GetWOPools()
        {
            List<WOPool> items = new List<WOPool>();
            WOPoolContract[] contract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((WorkItemsService)channel).GetWOPoolCode(new GetWOPoolCode(context)).result;
            }
                        
            foreach (WOPoolContract dr in contract)
            {
                WOPool woEquipment = new WOPool()
                {
                    WorkOrderPoolCode = dr.WOPoolCode,
                    Description = dr.Description
                };
                items.Add(woEquipment);
            }
            
            return items;
        }

        public List<PMFailureCode> GetFailureCodes()
        {
            List<PMFailureCode> items = new List<PMFailureCode>();
            FailureCodeContract[] contract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((WorkItemsService)channel).GetFailureCodes(new GetFailureCodes(context)).result;
            }
                        
            foreach (FailureCodeContract dr in contract)
            {
                PMFailureCode failureCode = new PMFailureCode()
                {
                    FailureCode = dr.FailureCode,
                    Description = dr.Description
                };

                items.Add(failureCode);
            }
           
            return items;
        }

        public List<PMRepairCode> GetRepairCodes()
        {
            List<PMRepairCode> items = new List<PMRepairCode>();
            RepairCodeContract[] contract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((WorkItemsService)channel).GetRepaireCodes(new GetRepaireCodes(context)).result;
            }
                        
            foreach (RepairCodeContract dr in contract)
            {
                PMRepairCode repairCode = new PMRepairCode()
                {
                    FailureCode = dr.FailureCode,
                    Description = dr.Description,
                    RepairCode = dr.RepairCode
                };

                items.Add(repairCode);
            }
            

            return items;
        }

        public List<PMEquipment> GetEquipments()
        {
            List<PMEquipment> items = new List<PMEquipment>();
            EquipContract[] contract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((WorkItemsService)channel).GetAllEquipments(new GetAllEquipments(context)).result;
            }


            foreach (EquipContract dr in contract)
            {
                PMEquipment woEquipment = new PMEquipment()
                {
                    EquipmentID = dr.EquipmentID,
                    EquipmentName = dr.EquipmentName,
                    EquipmentGroupCode = dr.EquipmentGroupCode,
                    LocationName = dr.LocationName,
                    LocationCode = dr.LocationCode
                };

                items.Add(woEquipment);
            }


            return items;
        }

        public List<PMEquipment> SearchEquipments(string search)
        {
            List<PMEquipment> items = new List<PMEquipment>();
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                EquipContract[] contract = null;
                if (search.ToUpper().StartsWith("EQPID"))
                {
                    var equip = ((WorkItemsService)channel).GetEquipment(new GetEquipment(context, search)).result;
                    contract = new EquipContract[1] { equip };
                }
                else
                {
                    //for bulk search use "*"
                    if (!search.EndsWith("*"))
                        search = search + "*";
                    else if (!search.StartsWith("*"))
                        search = "*" + search;
                    
                    contract = ((WorkItemsService)channel).SearchEquipments(new SearchEquipments(context, search)).result;
                }

                foreach (EquipContract dr in contract)
                {
                    PMEquipment woEquipment = new PMEquipment()
                    {
                        EquipmentID = dr.EquipmentID,
                        EquipmentName = dr.EquipmentName,
                        EquipmentGroupCode = dr.EquipmentGroupCode,
                        LocationName = dr.LocationName,
                        LocationCode = dr.LocationCode,
                        Attachments = new EquipCatalog().ToListConvert(dr.Attachments.ToList())
                    };

                    items.Add(woEquipment);
                }
            }

            return items;
        }

        public PMEquipment GetEquipment(string equipId)
        {
            PMEquipment woEquipment = new PMEquipment();
            EquipContract equipment;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                equipment = ((WorkItemsService)channel).GetEquipment(new GetEquipment(context, equipId)).result;
            }

            if (!string.IsNullOrEmpty(equipment.EquipmentID))
            {
                woEquipment = new PMEquipment()
                {
                    EquipmentID = equipment.EquipmentID,
                    EquipmentName = equipment.EquipmentName,
                    EquipmentGroupCode = equipment.EquipmentGroupCode,
                    LocationName = equipment.LocationName,
                    LocationCode = equipment.LocationCode,
                    Attachments = new EquipCatalog().ToListConvert(equipment.Attachments.ToList())
                };
            }
            return woEquipment;
        }

        public List<WOTypeContract> GetWorkOrderTypes()
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((WorkItemsService)channel).GetWorkOrderTypes(new GetWorkOrderTypes(context)).result.ToList();
            }
        }

        public List<WOLocationContract> GetEquipLocations()
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((WorkItemsService)channel).GetEquipLocations(new GetEquipLocations(context)).result.ToList();
            }

        }

        public List<PMWorkItem> GetInlineWorkItems(string workOrderId)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                var items =((WorkItemsService)channel).GetInlineWorkItems(new GetInlineWorkItems(context, workOrderId)).result.ToList();
                return new PMWorkItem().ToListConvert(items);
            }            
            
        }

        public List<PMWorkItem> GetOtherWorkItems(string workOrderId, string search, int topRecords, bool isItemCode)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                if (string.IsNullOrEmpty(search))
                    search = "*";
                else
                    search = "*" + search + "*";

                var items = ((WorkItemsService)channel).GetOtherWorkItems(new GetOtherWorkItems(context, isItemCode, search, topRecords, workOrderId)).result.ToList();
                return new PMWorkItem().ToListConvert(items);
            }
        }




        public PMWorkOrder GetlatestWorkOrder(string workOrderId)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                PMWorkOrderContract contract = ((WorkItemsService)channel).GetLatestWorkorder(new GetLatestWorkorder(context, workOrderId)).result;
                PMWorkOrder entity = new PMWorkOrder();
                return entity.ToConvert(contract);
            }            
            
        }




        public PMWorkOrder SaveWorkOrder(PMWorkOrder entity)
        {
            PMWorkOrderContract contract = entity.FromConvert();
            
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                //***Mansoor*** Uncomment following lines
                //var item = ((WorkItemsService)channel).SaveWorkOrder(new SaveWorkOrder(context, contract)).result;
               
                return entity.ToConvert(contract);
            }            
        }

        public int SetWorkOrderStatus(string workOrderId, int statusId)
        {
            bool item = false;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                
                item = ((WorkItemsService)channel).SetWOStatus(new SetWOStatus(context, statusId, workOrderId)).result;
            }
            
            if (item)
                return 1;
            else
                return 0;
        }

        public int DeleteWorkItem(string workOrderId, int sto_id)
        {
            bool result = false;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((WorkItemsService)channel).DeleteWorkItem(new DeleteWorkItem(context, sto_id, workOrderId)).result;
            }
            
            if (result)
                return 1;

            return 0;
        }

        public byte[] DownloadFile(ref string fileName, string filePath)
        {
            byte[] fileBytes = SendToFileShare(fileName, filePath);

            return fileBytes;
        }

        private byte[] SendToFileShare(string fileName, string filePath)
        {
            byte[] fileGot = new byte[0];

            //string networkShareLocation = @"\\172.17.0.150\Dyn_Attach_Doc\EamEqp\";

            var fullPath = filePath + fileName;


            //Credentials for the account that has write-access. Probably best to store these in a web.config file.
            //var domain = "kabholding.com";
            //var userID = "ax2";
            //var password = "Dyn@n1c5Ax";


            //if (ImpersonateUser(domain, userID, password) == true)
            //{
            //write the PDF to the share:
            //System.IO.File.WriteAllBytes(fullPath, fileGot);
            fileGot = System.IO.File.ReadAllBytes(fullPath);
            //using (FileStream file = File.Open(fullPath, FileMode.Open))
            //{
            //    file.Write(content, 0, content.Length);

            //    fileGot = new byte[file.Length];
            //    file.Close();
            //}


            //undoImpersonation();
            //}
            //else
            //{
            //Could not authenticate account. Something is up.
            //Log or something.
            //}

            return fileGot;
        }


        public PMLineCostContract[] MainCostByProdLine(DateTime startDate, DateTime endDate)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((WorkItemsService)channel).MainCostByProdLine(new MainCostByProdLine(context, endDate, startDate)).result;
            }
        }
        
        public string GetPing()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "Your Web-Service IP Address is: " + ip.ToString();
                }
            }
            return "Sorry";
        }
        
    }
}

