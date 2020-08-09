using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SyncServices.EAMServices;
using System.Configuration;
using System.Data;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModulaPR" in code, svc and config file together.
    public class ModulaPR : IModulaPR
    {
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

            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            List<WOPoolContract> ds = client.GetWOPoolCode(context).ToList();
            foreach (WOPoolContract dr in ds)
            {
                WOPool woEquipment = new WOPool()
                {
                    WorkOrderPoolCode = dr.WOPoolCode,
                    Description = dr.Description
                };
                items.Add(woEquipment);
            }
            client.Close();
            return items;
        }

        public List<PMFailureCode> GetFailureCodes()
        {
            List<PMFailureCode> items = new List<PMFailureCode>();

            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            List<FailureCodeContract> ds = client.GetFailureCodes(context).ToList();
            foreach (FailureCodeContract dr in ds)
            {
                PMFailureCode failureCode = new PMFailureCode()
                {
                    FailureCode = dr.FailureCode,
                    Description = dr.Description
                };

                items.Add(failureCode);
            }
            client.Close();

            return items;
        }

        public List<PMRepairCode> GetRepairCodes()
        {
            List<PMRepairCode> items = new List<PMRepairCode>();

            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            List<RepairCodeContract> ds = client.GetRepaireCodes(context).ToList();
            foreach (RepairCodeContract dr in ds)
            {
                PMRepairCode repairCode = new PMRepairCode()
                {
                    FailureCode = dr.FailureCode,
                    Description = dr.Description,
                    RepairCode = dr.RepairCode
                };

                items.Add(repairCode);
            }
            client.Close();

            return items;
        }

        public List<PMEquipment> GetEquipments()
        {
            List<PMEquipment> items = new List<PMEquipment>();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };


            List<EquipContract> ds = client.GetAllEquipments(context).ToList();

            foreach (EquipContract dr in ds)
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

            
            client.Close();

            return items;
        }

        public List<PMEquipment> SearchEquipments(string search)
        {
            List<PMEquipment> items = new List<PMEquipment>();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            if (search.ToUpper().StartsWith("EQPID"))
            {
                items.Add(GetEquipment(search));
            }
            else
            {
                //for bulk search use "*"
                if (!search.EndsWith("*"))
                    search = search + "*";
                else if (!search.StartsWith("*"))
                    search = "*" + search;

                List<EquipContract> ds = client.SearchEquipments(context, search).ToList();

                foreach (EquipContract dr in ds)
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

            client.Close();

            return items;
        }

        public PMEquipment GetEquipment(string equipId)
        {
            PMEquipment woEquipment = new PMEquipment();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            
            EquipContract equipment = client.GetEquipment(context, equipId);

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
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };


            return client.GetWorkOrderTypes(context).ToList();
        }

        public List<WOLocationContract> GetEquipLocations()
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };


            return client.GetEquipLocations(context).ToList();
        }

        public List<PMWorkItem> GetInlineWorkItems(string workOrderId)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var items=client.GetInlineWorkItems(context, workOrderId).ToList();
            return new PMWorkItem().ToListConvert(items);
        }

        public List<PMWorkItem> GetOtherWorkItems(string workOrderId, string search, int topRecords, bool isItemCode)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            
            if (string.IsNullOrEmpty(search))
                search = "*";
            else
                search = "*" + search + "*";
            var items = client.GetOtherWorkItems(context, workOrderId, search, topRecords, isItemCode).ToList();
            return new PMWorkItem().ToListConvert(items);
        }




        public PMWorkOrder GetlatestWorkOrder(string workOrderId)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PMWorkOrderContract contract = client.GetLatestWorkorder(context, workOrderId);
            PMWorkOrder entity = new PMWorkOrder();
            return entity.ToConvert(contract);
        }




        public PMWorkOrder SaveWorkOrder(PMWorkOrder entity)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PMWorkOrderContract contract = entity.FromConvert();
            var item = client.SaveWorkOrder(context, contract);

            return entity.ToConvert(item);
        }

        public int SetWorkOrderStatus(string workOrderId, int statusId)
        {
            bool item = false;
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                item = client.SetWOStatus(context, workOrderId, statusId);
            }
            catch (System.ServiceModel.FaultException<AifFault> aifExp)
            {
                string allMsgs = string.Empty;
                InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                foreach (InfologMessage msg in msgs)
                {
                    allMsgs += msg.Message + Environment.NewLine;
                }

                if (!string.IsNullOrEmpty(allMsgs))
                {
                    //string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallets:[" + string.Join(";", pallets) + "], Username:" + userName + ", Device:" + device;
                    //new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    //string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallets:[" + string.Join(";", pallets) + "], Username:" + userName + ", Device:" + device;
                    //new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);
                }
                catch { }
                throw exp;
            }
            if (item)
                return 1;
            else
                return 0;
        }

        public int DeleteWorkItem(string workOrderId, int sto_id)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            Boolean result = client.DeleteWorkItem(context, workOrderId, sto_id);
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
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.MainCostByProdLine(context, startDate, endDate);

            
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

