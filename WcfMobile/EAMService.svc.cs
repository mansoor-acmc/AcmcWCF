using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfMobile.EAMServices;

namespace WcfMobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EAMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EAMService.svc or EAMService.svc.cs at the Solution Explorer and start debugging.
    public class EAMService : IEAMService
    {
        public ItemEntity GetItem(string itemId)
        {
            return new DBClass().GetItem(itemId);
        }

        public List<ItemEntity> GetItems(string search)
        {
            return new DBClass().GetItems(search);
        }

        public ItemEntity GetItemMultiSource(string itemId)
        {
            ItemEntity entity = new DBClass().GetItem(itemId);
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            var itemResult = client.GetOnhand(context, itemId);
            if(itemResult != null)
                entity.PhysicalCount = itemResult.OnHandQty;

            return entity;
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

        public List<PMEquipment> GetEquipmentsBySection(string section)
        {
            List<PMEquipment> items = new List<PMEquipment>();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            List<EquipContract> ds = client.GetEquipmentsByCostCenter(context, section).ToList();

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

            var items = client.GetInlineWorkItems(context, workOrderId).ToList();
            return new PMWorkItem().ToListConvert(items);
        }

        public List<PMWorkItem> GetOtherWorkItems(string workOrderId, string search, string topRecords, string isItemCode)
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
            var items = client.GetOtherWorkItems(context, workOrderId, search, int.Parse(topRecords),Convert.ToBoolean(isItemCode)).ToList();
            return new PMWorkItem().ToListConvert(items);
        }

        public List<PMWorkOrder> GetWorkOrderList(string equipmentId)
        {
            List<PMWorkOrder> lines = new List<PMWorkOrder>();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var contractList = client.GetWorkordersByEquipment(context, equipmentId);
            foreach(PMWorkOrderContract row in contractList)
            {
                lines.Add(
                    new PMWorkOrder().ToConvert(row));                
            }

            return lines;
        }

        public List<PMWorkOrder> GetWorkOrderListByDates(string startDate, string endDate)
        {
            List<PMWorkOrder> lines = new List<PMWorkOrder>();
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            var contractList = client.GetWorkordersByDates(context,Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            foreach (PMWorkOrderContract row in contractList)
            {
                lines.Add(
                    new PMWorkOrder().ToConvert(row));
            }

            return lines;
        }


        public PMWorkOrder GetWorkOrder(string workOrderId)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            PMWorkOrderContract contract = client.GetSingleWorkorderWithItems(context, workOrderId);
            PMWorkOrder entity = new PMWorkOrder();
            return entity.ToConvert(contract);
        }

        public LookupContract[] WorkOrderStatusLookup()
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.GetWorkOrderStatusLookup(context);
        }


        public PMWorkOrder SaveWorkOrder(PMWorkOrder entity)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            if (entity.StartDate < DateTime.Now.Date)
                throw new Exception("StartDate should be today or in future date.");

            if (string.IsNullOrEmpty(entity.WOEquipment))
                throw new Exception("WOEquipment should not be empty.");

            PMWorkOrderContract contract = entity.FromConvert();
            var item = client.SaveWorkOrder(context, contract);

            return entity.ToConvert(item);
        }

        public int SetWorkOrderStatus(string workOrderId, string statusId)
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
                item = client.SetWOStatus(context, workOrderId, int.Parse(statusId));
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

        public int DeleteWorkItem(string workOrderId, string workItemId)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
            Boolean result = client.DeleteWorkItemByRecId(context, workOrderId, long.Parse(workItemId));
            if (result)
                return 1;

            return 0;
        }

        

        public PMLineCostContract[] MainCostByProdLine(string startDate, string endDate)
        {
            WorkItemsServiceClient client = new WorkItemsServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.MainCostByProdLine(context, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));


        }
    }
}
