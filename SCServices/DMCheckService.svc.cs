using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SoapUtility.DataManagerServices;
using System.Configuration;
using AuthenticationUtility;
using System.ServiceModel.Channels;

namespace SyncServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DMCheckService" in code, svc and config file together.
    public class DMCheckService : IDMCheckService
    {
        public const string D365ServiceName = "DataManagerServiceGroup";
        IClientChannel channel;
        string oauthHeader = string.Empty;
        CallContext context = null;

        public DMCheckService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var aosUriString = ClientConfiguration.Default.UriString;

            oauthHeader = OAuthHelper.GetAuthenticationHeader(true);
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(D365ServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new DMDataToSaveServiceClient(binding, endpointAddress);
            channel = client.InnerChannel;

            context = new CallContext()
            {
                MessageId = Guid.NewGuid().ToString(),
                Company = ConfigurationManager.AppSettings["DynamicsCompany"]
            };
        }

        public DMExportContract GetPalletInfo(string palletNum)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).GetPalletInfo(new GetPalletInfo(context, palletNum)).result;
            }            
        }

        public DMExportContract GetPalletInfoByRecordId(long recordId)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).GetPalletInfoByRecordId(new GetPalletInfoByRecordId(context, recordId)).result;
            }            
        }

        public bool ConfirmPalletReceive(string palletNum, long recordId, string deviceName, string deviceUser, bool isFromSL)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).ConfirmPalletReceive(new ConfirmPalletReceive(context, deviceName, deviceUser, isFromSL, palletNum, recordId)).result;
            }
            
        }

        public string ConfirmPalletAndLocationReceive(string palletNum, string locationId, long recordId, string deviceName, string deviceUser, bool isFromSL)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).ConfirmPalletAndLocationReceive(new ConfirmPalletAndLocationReceive(context, deviceName, deviceUser, isFromSL, locationId, palletNum, recordId)).result;
            }            
        }

        public bool UpdateAndConfirmPalletReceive(DMExportContract pallet)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).UpdateAndConfirmPalletReceive(new UpdateAndConfirmPalletReceive(context, pallet)).result;
            }            
        }

        public bool PrintAgainPallet(string palletNum, long recordId, string deviceName, string deviceUser)
        {
            //DBClass db = new DBClass(DBClass.DbName.ImportExportDB);
            //return db.PrintAgainPallet(palletNum, recordId, deviceName, deviceUser);
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                ((DMDataToSaveService)channel).DMSetPrintAgain(new DMSetPrintAgain(context, deviceName, deviceUser, palletNum, recordId));
            }            

            return true;
        }

        public List<DMExportMiniContract> DMClearPrintAgain()
        {            
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).DMClearPrintAgain(new DMClearPrintAgain(context)).result.ToList();                
            }
            
        }

        public List<DMExportContract> UpdateOfflinePallets(List<DMExportOfflineContract> lines)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).UpdateOfflinePallets(new UpdateOfflinePallets(context, lines.ToArray())).result.ToList();
            }            
        }

        #region Location Transfer
        /// <summary>
        /// Transfer pallets to new location. ***Direct DB interaction***        
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<LocationHistory> TransferPalletsToNewLocation(List<LocationHistory> lines)
        {
            DBClass dbClass = new DBClass(DBClass.DbName.DynamicsAX);
            string deviceName, userName;
            deviceName = lines[0].DeviceName;
            userName = lines[0].UserName;

            LocationHistory objLocation = MergeDMLines(lines, deviceName, userName);

            string hasPalletsApproved = dbClass.CheckPalletApprovedBulk(objLocation.Message);
            
            if (hasPalletsApproved.Length > 0)
            {
                string palletsNotApproved = string.Empty;

                string[] palletsArray = hasPalletsApproved.Split(',');
                foreach (string pallet in palletsArray)
                {
                    if (!string.IsNullOrEmpty(pallet))
                    {
                        string[] palletAndLoc = pallet.Split(';');
                        if (palletsNotApproved.Length > 0) palletsNotApproved += ",";
                         palletsNotApproved += palletAndLoc[0];
                    }
                }
                throw new Exception("Approve these Pallets: \r\n" + palletsNotApproved);
            }

            SaveLocations(objLocation.PalletNum,objLocation.Location, "Before saving Locations",objLocation.DeviceName,objLocation.UserName);

            string remainingPalletsToTransfer = dbClass.UpdatePalletLocationBulk(objLocation.Message, objLocation.UserName, objLocation.DeviceName); //Stored Procedure
            var remainingLines = ConvertStringToList(remainingPalletsToTransfer);

            var savedInSP = SavedDirectlyInSP(remainingLines, lines);

            List<LocationHistory> returnedLines = new List<LocationHistory>();
            if (savedInSP != null && savedInSP.Count > 0)
                returnedLines.AddRange(savedInSP);

            objLocation = MergeDMLines(returnedLines, deviceName, userName);
            SaveLocations(objLocation.PalletNum, objLocation.Location, "After direct saving to DB, the remaining Pallets", objLocation.DeviceName, objLocation.UserName);

            if (returnedLines.Count > 0)
            {
                try
                {
                    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                    {
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                        var linesFromAX = ((DMDataToSaveService)channel).UpdateTransferPallets(new UpdateTransferPallets(context, ConvertToDMForTransfer(returnedLines).ToArray())).result;


                        if (linesFromAX.Count() > 0)
                            returnedLines = ConvertFromDMForTransfer(linesFromAX.ToList());
                    }
                    //if (linesFromAX != null && linesFromAX.Count() > 0)
                    //{
                    //    returnedLines.AddRange(linesFromAX.ToList());
                        
                    //    var history = MergeDMLines(returnedLines);

                    //    SaveLocations(history.PalletNum, history.Location, "After Saving Locations", history.DeviceName, history.UserName);
                    //}
                }
                catch (Exception exp)
                {
                    var locError = MergeDMLines(returnedLines, deviceName, userName);
                    SaveLocations(locError.PalletNum, locError.Location, "On Error: " + exp.Message, locError.DeviceName, locError.UserName);
                    throw exp;
                }
            }

            return returnedLines;
        }

        private List<DMForTransfer> ConvertToDMForTransfer(List<LocationHistory> lines)
        {
            List<DMForTransfer> results = new List<DMForTransfer>();

            foreach(LocationHistory one in lines)
            {
                results.Add(new DMForTransfer
                {
                    PalletNum = one.PalletNum,
                    whLocationId = one.Location
                });
            }

            return results;
        }

        private List<LocationHistory> ConvertFromDMForTransfer(List<DMForTransfer> lines)
        {
            List<LocationHistory> results = new List<LocationHistory>();
            foreach (DMForTransfer one in lines)
            {
                results.Add(new LocationHistory
                {
                    PalletNum = one.PalletNum,
                    Location = one.whLocationId
                });
            }
            return results;
        }


        private List<LocationHistory> SavedDirectlyInSP(List<LocationHistory> remainingPallets, List<LocationHistory> allPallets)
        {
            foreach (LocationHistory one in remainingPallets)
            {
                int index = allPallets.FindIndex(t => t.PalletNum == one.PalletNum);
                allPallets.RemoveAt(index);
            }

            return allPallets;
        }

        private List<LocationHistory> ConvertStringToList(string pallets)
        {
            List<LocationHistory> updatedLines = new List<LocationHistory>();
            string[] palletsArray = pallets.Split(',');
            foreach (string pallet in palletsArray)
            {
                if (!string.IsNullOrEmpty(pallet))
                {
                    string[] palletAndLoc = pallet.Split(';');
                    LocationHistory one = new LocationHistory();
                    one.PalletNum = palletAndLoc[0];
                    string[] locAndIsManual = palletAndLoc[1].Split('-');
                    one.Location = locAndIsManual[0];
                    if(locAndIsManual.Length>0)
                    {
                        one.IsManual = locAndIsManual[1].Equals("1") ? true : false;
                    }
                    updatedLines.Add(one);
                }
            }
            return updatedLines;
        }

        /// <summary>
        /// Save Pallets' Locations history time.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="message"></param>
        private bool SaveLocations(string pallets, string locations, string message,string deviceName, string userName)
        {
            LocationHistory objLocation = new LocationHistory()
            {
                PalletNum = pallets,
                Location = locations,
                Message = message,
                CreateDateTime = DateTime.Now,
                DeviceName= deviceName,
                UserName = userName                
            };
            DBClass objDB = new DBClass(DBClass.DbName.DeviceMsg);
            return objDB.SaveLocationTime(objLocation, message);
        }

        private LocationHistory MergeDMLines(List<LocationHistory> lines, string deviceName, string userName)
        {
            LocationHistory objLocation = new LocationHistory();

            objLocation.CreateDateTime = DateTime.Now;
            objLocation.Message = "";            
            objLocation.PalletNum = "";
            objLocation.DeviceName = deviceName;
            objLocation.UserName = userName;
            objLocation.Location = "";
            objLocation.IsManual = false;

            foreach (LocationHistory line in lines)
            {
                objLocation.PalletNum += line.PalletNum + ",";
                objLocation.Location += line.Location.Trim() + ",";
                objLocation.Message += line.PalletNum + ";" + line.Location.Trim()+"-"+(line.IsManual?"1":"0") + ",";
                //objLocation.DeviceName = line.DeviceName;
                //objLocation.UserName = line.UserName;
            }
            if (objLocation.PalletNum.EndsWith(","))
                objLocation.PalletNum = objLocation.PalletNum.Substring(0, objLocation.PalletNum.Length - 1);
            if (objLocation.Location.EndsWith(","))
                objLocation.Location = objLocation.Location.Substring(0, objLocation.Location.Length - 1);           

            return objLocation;
        }
        
        #endregion



        public List<DMSummaryContract> SummaryPallets(string itemId)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).SummaryNormalPallets(new SummaryNormalPallets(context, itemId)).result.ToList();
            }            
        }

        public bool CancelPalletReceive(string palletNum, long recordId, string deviceName, string deviceUser)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).CancelPalletReceive(new CancelPalletReceive(context, deviceName, deviceUser, palletNum, recordId)).result;
            }
        }

        public List<DMExportContract> ItemGroupPallets(string itemId, string grade, string shade, string caliber)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).ItemGroupPallets(new ItemGroupPallets(context, itemId, caliber, grade, shade)).result.ToList();
            }
        }

        public bool CreateDowntimeForMarpak(int whichMarpak)
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).CreateDowntimeForMarpak(new CreateDowntimeForMarpak(context, whichMarpak)).result;
            }
        }

        public List<ItemForChart> GetProductionByLinesForChart(DateTime date)
        {
            return new DBClass(DBClass.DbName.ImportExportDB).GetProductionByLines(date);
        }

        public List<ItemForChart> GetProductionByItemsForChart(DateTime date)
        {
            return new DBClass(DBClass.DbName.ImportExportDB).GetProductionByItems(date);
        }

        public List<WmsLocationContract> GetWHLocations()
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).GetWHLocations(new GetWHLocations(context)).result.ToList();
            }
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <returns></returns>
        public List<DuplicatePallet> GetDuplicatePallets()
        {
            return new DBClass(DBClass.DbName.ImportExportDB).GetDuplicatePallets();
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <param name="pallet"></param>
        /// <returns></returns>
        public bool ClearDuplicatePallet(DuplicatePallet pallet)
        {
            return new DBClass(DBClass.DbName.ImportExportDB).ClearDuplicatePallet(pallet);
        }

        /// <summary>
        /// ***Convert it to WCF service method***
        /// </summary>
        /// <param name="pallets"></param>
        /// <returns></returns>
        public bool ClearDuplicatePalletsAll(List<DuplicatePallet> pallets)
        {
            return new DBClass(DBClass.DbName.ImportExportDB).ClearDuplicatePalletsAll(pallets);
        }

        public List<ItemCodeContract> GetItemCodes()
        {
            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                return ((DMDataToSaveService)channel).GetItemCodes(new GetItemCodes(context)).result.ToList();
            }            
        }

        public string GetPing()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "Your Sync Web-Service IP Address is: " + ip.ToString();

                }
            }
            return "Sorry";
        }
    }
}
