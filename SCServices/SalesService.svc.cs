using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SyncServices.SalesOrderAX;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesService" in code, svc and config file together.
    public class SalesService : ISalesService
    {
        public SalesTable FindSalesOrder(string salesId)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            SalesTableContract contract = client.findSalesOrder(context, salesId);
            client.Close();
            
            SalesTable salesTable = new SalesTable()
            {
                SalesId = contract.SalesId,
                SalesName = contract.SalesName,
                DeliveryDate = contract.DeliveryDate,
                DeliveryMode = contract.DeliveryMode,
                DeliveryName = contract.DeliveryName,
                HalfPallet = contract.HalfPallet == NoYes.No ? false : true,
                PickSameDimension = contract.SameConfiguration == NoYes.No ? false : true,
                StartLoading = contract.StartLoad,
                StopLoading = contract.StopLoad
            };
            
            salesTable.Lines = new List<SalesLine>();
            if (contract.SalesLines.Count() > 0)
            {
                foreach (SalesLineContract axdline in contract.SalesLines)
                {
                    salesTable.Lines.Add(new SalesLine().ToConvert(axdline));
                }
            }
            return salesTable;
        }

        public SalesTable FindPickingList(string pickingId, string userName, string device)
        {
            SalesTableContract contract = null;
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                contract = client.findPickingList(context, pickingId);
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
                    string allParameters = "PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "FindPickingList", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "FindPickingList", allParameters);
                }
                catch { }
                throw exp;
            }
            finally
            {
                PickHistoryContract history = new PickHistoryContract()
                {
                    PickingId = pickingId,
                    UpdatedByUser = userName,
                    UpdatedDateTime = DateTime.Now,
                    UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.PickingList,
                    UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Searching Picking List for: " + pickingId,
                    PalletNo = string.Empty
                };
                client.SaveHistory(new CallContext()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Company = ConfigurationManager.AppSettings["DynamicsCompany"]
                }, history);

                client.Close();
            }
            SalesTable salesTable = new SalesTable()
            {
                SalesId = contract.SalesId,
                SalesName = contract.SalesName,
                PickingId = contract.PickingId,
                DriverName = contract.DriverName,
                TruckPlate = contract.TruckPlate,
                DeliveryDate = contract.DeliveryDate,
                DeliveryMode = contract.DeliveryMode,
                DeliveryName = contract.DeliveryName,
                HalfPallet = contract.HalfPallet == NoYes.No ? false : true,
                PickSameDimension = contract.SameConfiguration == NoYes.No ? false : true,
                StartLoading = contract.StartLoad,
                StopLoading=contract.StopLoad
            };

            salesTable.Lines = new List<SalesLine>();
            if (contract.SalesLines.Count() > 0)
            {
                foreach (SalesLineContract axdline in contract.SalesLines)
                {
                    salesTable.Lines.Add(new SalesLine().ToConvert(axdline));
                }
            }

            return salesTable;
        }

        public List<SalesLine> ValidatePallets(string salesId, string itemId, string configId, string pickingId,
                List<string> pallets, string userName, string device, long lineRecId)
        {
            List<SalesLine> returnValue = new List<SalesLine>();

            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };


            try
            {
                List<SalesLineContract> rows = new List<SalesLineContract>();
                foreach (string pallet in pallets)
                {
                    SalesLineContract row = new SalesLineContract() { Serial = pallet };
                    rows.Add(row);
                }
                List<SalesLineContract> result = client.ReservePallets(context, salesId, itemId, configId, pickingId, userName, device, lineRecId, rows.ToArray()).ToList();
                client.Close();

                if (result.Count > 0)
                    returnValue = new SalesLine().ToListConvert(result);
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
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallets:[" + string.Join(";", pallets) + "], Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "SalesId: "+salesId+", ItemId:"+itemId+", PickingId:"+pickingId+", Pallets:["+string.Join(";",pallets)+"], Username:"+ userName+", Device:"+device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "ValidatePallets", allParameters);
                }
                catch { }
                throw exp;
            }

            return returnValue;
        }

        public SalesLine CheckPalletAvailable(string salesId, string itemId, string configId, string pickingId, 
            string pallet, string userName, string device, DateTime dtSave, long lineRecId)
        {
            //SalesLine returnResult = new SalesLine();
            SalesLineContract result = null;
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                result = client.CheckPalletAvailable(context, salesId, itemId, configId, pickingId, pallet, lineRecId);
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
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallet:" + pallet + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);
                    
                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Pallet:" + pallet + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);
                }
                catch { }
                throw exp;
            }
            finally
            {
                PickHistoryContract history = new PickHistoryContract()
                {
                    PalletNo = pallet,
                    PickingId = pickingId,
                    UpdatedByUser = userName,
                    UpdatedDateTime = dtSave,
                    UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.Search,
                    UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Search"
                };
                client.SaveHistory(new CallContext()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Company = ConfigurationManager.AppSettings["DynamicsCompany"]
                }, history);
            }
            client.Close();

            return new SalesLine().ToConvert(result);
        }

        public List<SalesLine> CheckPalletAvailableMulti(string salesId, string itemId, string configId, string pickingId, List<PalletItemContract> pallets,
            string userName, string device, long lineRecId)
        {
            SalesLineContract[] items=null;
            List<SalesLine> returnValue = new List<SalesLine>();
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                items = client.CheckPalletAvailableMulti(context, salesId, itemId, configId, pickingId, userName,
                    device, lineRecId, pallets.ToArray());

                //string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
               // new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", "Error", "", DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);

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
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);
                }
                catch { }
                throw exp;
            }
            client.Close();

            if (items != null && items.Count() > 0)
                returnValue = new SalesLine().ToListConvert(items.ToList());

            return returnValue;
        }

        public string UnreservePallet(string pallet, string pickingId, string userName, string device)
        {
            bool result = false;
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                result = client.UnreservePallet(context, pallet);
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = ", PickingId:" + pickingId + ", Pallet:" + pallet + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "UnreservePallet", allParameters);
                }
                catch { }
                throw exp;
            }
            finally
            {
                PickHistoryContract history = new PickHistoryContract()
                {
                    PalletNo = pallet,
                    PickingId = pickingId,
                    UpdatedByUser = userName,
                    UpdatedDateTime = DateTime.Now,
                    UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.UnReserve,
                    UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Un-Reserve"
                };
                client.SaveHistory(new CallContext()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Company = ConfigurationManager.AppSettings["DynamicsCompany"]
                }, history);
                client.Close();
            }
            if (result)
            {
                
                return "true";
            }
            else
            {
                
                return "false";
            }
        }

        public List<SalesLine> GetLatestPallets(string pickingId, string itemId)
        {
            List<SalesLine> returnValue=new List<SalesLine>();
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                var items = client.GetLatestPallets(context, pickingId, itemId);
                client.Close();

                if (items.Count() > 0)
                    returnValue = new SalesLine().ToListConvert(items.ToList());
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "PickingId:" + pickingId + ", ItemId:" + itemId;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", "", "", "GetLatestPallets", allParameters);
                }
                catch { }
                throw exp;
            }

            return returnValue;
        }

        public void SavePickingLoad(string pickingId, DateTime startLoad, DateTime stopLoad)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            client.SavePickingLoad(context, pickingId, startLoad, stopLoad);
        }

        public CustomerDeliveryContract[] CustomersDeliveryByQty(DateTime startDate, DateTime endDate)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.CustomersDeliveryByQty(context, startDate, endDate);
        }


        public CustomerDeliveryContract[] CustomersDeliveryByTrucks(DateTime startDate, DateTime endDate)
        {
            SOPickServiceClient client = new SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            return client.CustomersDeliveryByTrucks(context, startDate, endDate);
        }

        public SalesTable ReceivePickingList(string userName, string device)
        {
            string pickingId=string.Empty;
            SalesTableContract contract = null;
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                contract = client.ReceivePickingList(context, userName, device);
                if (contract != null && contract.SalesLines != null && contract.SalesLines.Count() > 0)
                {
                    pickingId = contract.SalesLines[0].PickingId;
                }
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
                    string allParameters = "PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", allMsgs, "", DateTime.Now, "SaleService", userName, device, "FindPickingList", allParameters);

                    throw new Exception(allMsgs);
                }
            }
            catch (Exception exp)
            {
                try
                {
                    string allParameters = "PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
                    new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", exp.Message, exp.StackTrace, DateTime.Now, "SaleService", userName, device, "FindPickingList", allParameters);
                }
                catch { }
                throw exp;
            }
            finally
            {
                PickHistoryContract history = new PickHistoryContract()
                {
                    PickingId = pickingId,
                    UpdatedByUser = userName,
                    UpdatedDateTime = DateTime.Now,
                    UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.PickingList,
                    UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Searching Picking List for: " + pickingId,
                    PalletNo = string.Empty
                };
                client.SaveHistory(new CallContext()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Company = ConfigurationManager.AppSettings["DynamicsCompany"]
                }, history);

                client.Close();
            }
            SalesTable salesTable = new SalesTable()
            {
                SalesId = contract.SalesId,
                SalesName = contract.SalesName,
                PickingId = contract.PickingId,
                DriverName = contract.DriverName,
                TruckPlate = contract.TruckPlate,
                DeliveryDate = contract.DeliveryDate,
                DeliveryMode = contract.DeliveryMode,
                DeliveryName = contract.DeliveryName,
                HalfPallet = contract.HalfPallet == NoYes.No ? false : true,
                PickSameDimension = contract.SameConfiguration == NoYes.No ? false : true,
                StartLoading = contract.StartLoad,
                StopLoading = contract.StopLoad
            };

            salesTable.Lines = new List<SalesLine>();
            if (contract.SalesLines.Count() > 0)
            {
                foreach (SalesLineContract axdline in contract.SalesLines)
                {
                    salesTable.Lines.Add(new SalesLine().ToConvert(axdline));
                }
            }

            return salesTable;
        }

        public void SaveLoginHistory(string userName, string device, string deviceIp, string projectName)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                client.LoginDevice(context, device, deviceIp, userName, projectName);
            }
            finally
            {
                client.Close();
            }
        }

        public FGLineContract[] GetFGLines()
        {
            
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                return client.GetFGLines(context);
            }
            finally
            {
                client.Close();
            }
        }

        public FGDeliveryContract[] GetDeliveries(DateTime dateSearch)
        {
            SOPickServiceClient client = new SalesOrderAX.SOPickServiceClient();
            CallContext context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };

            try
            {
                var deliveries = client.GetDeliveries(context, dateSearch);
                return deliveries;
            }
            finally
            {
                client.Close();
            }
        }


        public string GetPing()
        {
            //save the Ping into database. so that we know that network is available or not

            //then check the following.
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "Your Sales Web-Service IP Address is: " + ip.ToString();
                }
            }
            return "Sorry";
        }
    }
}
