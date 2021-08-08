using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
//using SyncServices.SalesOrderAX;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using SoapUtility.SOPickServiceGroup;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesService" in code, svc and config file together.
    public class SalesService : ISalesService
    {
        public const string D365ServiceName = "SOPickServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public SalesService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new SOPickServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }


        public SalesTable FindSalesOrder(string salesId)
        {
            SalesTableContract contract = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contract = ((SOPickService)channel).findSalesOrder(new findSalesOrder(context,salesId)).result;
            }
                         
            
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
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                try
                {
                    contract = ((SOPickService)channel).findPickingList(new findPickingList(context, pickingId)).result;
                }
                catch (System.ServiceModel.FaultException aifExp)
                {
                    string allMsgs = string.Empty;
                    //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                    //foreach (InfologMessage msg in msgs)
                    //{
                    //    allMsgs += msg.Message + Environment.NewLine;
                    //}
                    allMsgs = aifExp.Message;
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
                        //UpdatedDateTimeSpecified = true,
                        UpdateStatus = PalletStatus.PickingList,
                        //UpdateStatusSpecified = true,
                        DeviceName = device,
                        Remarks = "Searching Picking List for: " + pickingId,
                        PalletNo = string.Empty
                    };
                    ((SOPickService)channel).SaveHistory(new SaveHistory(context, history));
                    
                }
            }

                        
            SalesTable salesTable = new SalesTable()
            {
                SalesId = contract.SalesId,
                SalesName = contract.SalesName,
                PickingId = contract.PickingId,
                PackingSlip = contract.PackingSlip,
                DriverName = contract.DriverName,
                TruckPlate = contract.TruckPlate,
                TruckTicketNum = contract.TruckTicket,
                LoadingLine = contract.TruckLoadLine,
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
            SalesLineContract[] result = null;


            try
            {
                List<SalesLineContract> rows = new List<SalesLineContract>();
                foreach (string pallet in pallets)
                {
                    SalesLineContract row = new SalesLineContract() { Serial = pallet };
                    rows.Add(row);
                }
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    result = ((SOPickService)channel).PalletsReserving(new PalletsReserving(context, configId, device, itemId, lineRecId, rows.ToArray(), pickingId, salesId, userName)).result;
                }
                
               

                if (result != null && result.Length > 0)
                    returnValue = new SalesLine().ToListConvert(result.ToList());
            }
            catch (System.ServiceModel.FaultException aifExp)
            {
                string allMsgs = string.Empty;
                //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                //foreach (InfologMessage msg in msgs)
                //{
                //    allMsgs += msg.Message + Environment.NewLine;
                //}
                allMsgs = aifExp.Message;
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
            
            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    result = ((SOPickService)channel).CheckPalletAvailable(new CheckPalletAvailable(context, configId, itemId, lineRecId, pickingId, salesId, pallet)).result;
                }
                
            }
            catch (System.ServiceModel.FaultException aifExp)
            {
                string allMsgs = string.Empty;
                //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                //foreach (InfologMessage msg in msgs)
                //{
                //    allMsgs += msg.Message + Environment.NewLine;
                //}
                allMsgs = aifExp.Message ;
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
                    //UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.Search,
                    //UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Search"
                };
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    ((SOPickService)channel).SaveHistory(new SaveHistory(context, history));
                }
                
            }
            

            return new SalesLine().ToConvert(result);
        }

        public List<SalesLine> CheckPalletAvailableMulti(string salesId, string itemId, string configId, string pickingId, List<PalletItemContract> pallets,
            string userName, string device, long lineRecId)
        {
            SalesLineContract[] items=null;
            List<SalesLine> returnValue = new List<SalesLine>();
            //CustomersDeliveryByQtyClient client = new CustomersDeliveryByQtyClient();
            

            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    items = ((SOPickService)channel).CheckPalletAvailableMulti(new CheckPalletAvailableMulti(context, configId, device, itemId, lineRecId, pickingId, salesId, pallets.ToArray(), userName)).result;
                }
               
               //string allParameters = "SalesId: " + salesId + ", ItemId:" + itemId + ", PickingId:" + pickingId + ", Username:" + userName + ", Device:" + device;
               // new DBClass(SyncServices.DBClass.DbName.DeviceMsg).ErrorInsert("", "", "Error", "", DateTime.Now, "SaleService", userName, device, "CheckPalletAvailable", allParameters);

            }
            catch (System.ServiceModel.FaultException aifExp)
            {
                string allMsgs = string.Empty;
                //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                //foreach (InfologMessage msg in msgs)
                //{
                //    allMsgs += msg.Message + Environment.NewLine;
                //}
                allMsgs = aifExp.Message;
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
            

            if (items != null && items.Count() > 0)
                returnValue = new SalesLine().ToListConvert(items.ToList());

            return returnValue;
        }

        public string SalesDeliveryNote(string _salesId)
        {
            string contractResult = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                contractResult = ((SOPickService)channel).SalesDeliveryNote(new SalesDeliveryNote(context, _salesId)).result;
            }

            return contractResult;
        }

        public string UnreservePallet(string pallet, string pickingId, string userName, string device)
        {
            bool result = false;
            
            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    result = ((SOPickService)channel).UnreservePallet(new UnreservePallet(context, pallet)).result;
                }                
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
                    //UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.UnReserve,
                    //UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Un-Reserve"
                };
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    ((SOPickService)channel).SaveHistory(new SaveHistory(context, history));
                }
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
            SalesLineContract[] items = null;
            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    items =((SOPickService)channel).GetLatestPallets(new GetLatestPallets(context, itemId, pickingId)).result;
                }
                
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
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                 ((SOPickService)channel).SavePickingLoad(new SavePickingLoad(context, pickingId, startLoad, stopLoad));
            }

            
        }

        public CustomerDeliveryContract[] CustomersDeliveryByQty(DateTime startDate, DateTime endDate)
        {
            CustomerDeliveryContract[] result = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((SOPickService)channel).CustomersDeliveryByQty(new CustomersDeliveryByQty(context, endDate, startDate)).result;
            }

            return result;

            
        }


        public CustomerDeliveryContract[] CustomersDeliveryByTrucks(DateTime startDate, DateTime endDate)
        {
            CustomerDeliveryContract[] result = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((SOPickService)channel).CustomersDeliveryByTrucks(new CustomersDeliveryByTrucks(context, endDate, startDate)).result;
            }

            return result;
        }

        public SalesTable ReceivePickingList(string userName, string device)
        {
            string pickingId=string.Empty;
            SalesTableContract contract = null;
            

            try
            {
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    contract =((SOPickService)channel).ReceivePickingList(new ReceivePickingList(context, device, userName)).result;
                }
                
                if (contract != null && contract.SalesLines != null && contract.SalesLines.Count() > 0)
                {  
                    pickingId = contract.SalesLines[0].PickingId;
                }
            }
            catch (System.ServiceModel.FaultException aifExp)
            {
                string allMsgs = string.Empty;
                //InfologMessage[] msgs = aifExp.Detail.InfologMessageList;
                //foreach (InfologMessage msg in msgs)
                //{
                //    allMsgs += msg.Message + Environment.NewLine;
                //}
                allMsgs = aifExp.Message;
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
                    //UpdatedDateTimeSpecified = true,
                    UpdateStatus = PalletStatus.PickingList,
                    //UpdateStatusSpecified = true,
                    DeviceName = device,
                    Remarks = "Searching Picking List for: " + pickingId,
                    PalletNo = string.Empty
                };
                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    ((SOPickService)channel).SaveHistory(new SaveHistory(context, history));
                }
            }
            SalesTable salesTable = new SalesTable()
            {
                SalesId = contract.SalesId,
                SalesName = contract.SalesName,
                PickingId = contract.PickingId,
                PackingSlip = contract.PackingSlip,
                DriverName = contract.DriverName,
                TruckPlate = contract.TruckPlate,
                DeliveryDate = contract.DeliveryDate,
                DeliveryMode = contract.DeliveryMode,
                DeliveryName = contract.DeliveryName,
                HalfPallet = contract.HalfPallet == NoYes.No ? false : true,
                PickSameDimension = contract.SameConfiguration == NoYes.No ? false : true,
                StartLoading = contract.StartLoad,
                StopLoading = contract.StopLoad,
                TruckTicketNum = contract.TruckTicket,
                LoadingLine = contract.TruckLoadLine
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
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                ((SOPickService)channel).LoginDevice(new LoginDevice(context, deviceIp, device, projectName, userName));
            }
                        
        }

        public FGLineContract[] GetFGLines()
        {
            FGLineContract[] result=null; 
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((SOPickService)channel).GetFGLines(new GetFGLines(context)).result;
            }

            return result;
        }

        public FGDeliveryContract[] GetDeliveries(DateTime dateSearch)
        {
            FGDeliveryContract[] result = null;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((SOPickService)channel).GetDeliveries(new GetDeliveries(context, dateSearch)).result;
            }

            return result;
        }

        public List<UserData> GetUserData()
        {
            return new UserMgtService().GetUserData("SaleService");            
            
        }

        public string ChangeLoadingLine(string pickingId, int loadingLineNum)
        {
            string result = string.Empty;
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                result = ((SOPickService)channel).ChangeTruckLoadingLine(new ChangeTruckLoadingLine(context, loadingLineNum, pickingId)).result;
            }

            return result;
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
