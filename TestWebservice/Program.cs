using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;
using TestWebservice.DMCheckService;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Collections;
using System.Net;

namespace TestWebservice
{
    public class Credent
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ModulaMovement.ModulaMovementClient clientMov = new ModulaMovement.ModulaMovementClient();
            //var tbl = clientMov.ItemsNotUsed();
            Debug.Print("Start Program...");
            Console.Read();

            bool isOpened = clientMov.OpenItemCode(210089, "2023-10-22 08:32:22");
            /*
            ModulaService.ModulaPRClient prClient = new ModulaService.ModulaPRClient();
            var codes = prClient.GetWOPools();


            var pOHeader = new PurchaseService.PurchaseServiceClient().GetPurchOrderForGRN("22/PO-1991");
            //var FAItems = new FAService.FixedAssetServiceClient().ResetData("acmc").ToList();

            ArrayList abc = new ArrayList();
            bool result11 = false;
            string str = "";

            DMExportContract contract1 = new DMExportContract()
            {
                PalletNum = "N149017",
                ItemNumber = "SWE-2086012DG095/R",
                RecordId = 1733457,
                Grade = "G1",
                Shade = "D51",
                Caliber = "C5",
                Size = "600X600X10",
                LineOfOrigin = 4,
                WhichMarpak = 1,
                LGVOrForklift = PalletTransportBy.LGV,
                BoxesOnPallet = 48
            };
            DMCheckServiceClient dmClient = new DMCheckServiceClient();
            dmClient.UpdateAndConfirmPalletReceive(contract1);
            

           //var locs=dmClient.GetWHLocations();
           //int totRows = locs.Count();
           //Console.WriteLine(totRows.ToString());
            SCSyncService.SCSyncServiceClient scClient1 = new SCSyncService.SCSyncServiceClient();
            var scUomItems = scClient1.GetSCUnitOfMeasureFromAX("SM3M00091,SM3M00017,SM2M010149,DP2M010005");


            EAMService.EAMServiceClient eamClient = new EAMService.EAMServiceClient();
            var eamItems = eamClient.PostedWorkItems();

            string[] pallets = { "M031855"};
              
            

            SalesOrder.SalesServiceClient ssClient = new SalesServiceClient();
            //str = ssClient.SalesDeliveryNote("21SO-01327");
            var salesLines = ssClient.ValidatePallets("21SO-01330", "ORI-3088004DM140", "MS", "PKL21-002637", pallets, "azm", "S14", 0);
            foreach(SalesLine line in salesLines)
            {
                str += line.SerialNumber + "=" + line.Remarks;
            }
            var soPick = new SalesService.SalesServiceClient();
            var salesTable = soPick.FindSalesOrder("21SO-01820");
            Console.WriteLine(salesTable.PickingId);

            FGSyncService.FGSyncServiceClient fgClient1 = new FGSyncService.FGSyncServiceClient();
            var fgItems = fgClient1.GetFGYearInventory(0);
            int countFG = fgItems.Count();


            

           List<SyncServices.LocationHistory> allOne = new List<SyncServices.LocationHistory>();
            SyncServices.LocationHistory one = null;

           one = new SyncServices.LocationHistory() { PalletNum = "M200052", Location = "C5", IsManual = true, UserName = "fg", DeviceName = "S8-172.17.5.6" };
           allOne.Add(one);
           var result = dmClient.TransferPalletsToNewLocation(allOne.ToArray());
           if (result.Count() > 0)
               Console.WriteLine("Lines Transferred: " + result.Count().ToString());

           ProdRequestService.ProdRequestServiceClient prodService = new ProdRequestService.ProdRequestServiceClient();
           //ProdRequestService prodRequestService = new SyncServices.ProdRequestService();
           var allLines = prodService.GetAllProductionLines();

           foreach (string line in allLines)
           {
               Console.WriteLine(line);
           }
           Console.WriteLine();
           Console.Write("Select Line: ");
           string lineId = Console.ReadLine();

           var lineCaps = prodService.GetLineCapacities(lineId);
           foreach(var cap in lineCaps)
               Console.WriteLine(cap.ItemSizeThickness);

           //var isSuccess = new Program().CheckLoginAsync("am.bagedo@arabian-ceramics.com", "Amro*7894");

           Console.WriteLine("Sales Users---");
           SalesService.SalesServiceClient ssClient = new SalesService.SalesServiceClient();
           var userData = ssClient.GetUserData();


           foreach (var user in userData)
           {
               Console.WriteLine(user.UserName + "(" + user.UserType + "): " + user.Password);
           }

           Console.WriteLine("");
           Console.WriteLine("SC Users---");
           SCSyncService.SCSyncServiceClient scClient = new SCSyncService.SCSyncServiceClient();
           userData = scClient.GetUserData();
           foreach (var user in userData)
           {
               Console.WriteLine(user.UserName + "(" + user.UserType + "): " + user.Password);
           }

           Console.WriteLine("");
           Console.WriteLine("FG Users---");
           FGSyncService.FGSyncServiceClient fgClient = new FGSyncService.FGSyncServiceClient();
           userData = fgClient.GetUserData();
           foreach (var user in userData)
           {
               Console.WriteLine(user.UserName + "(" + user.UserType + "): " + user.Password);
           }

           DMCheckServiceClient dmClient = new DMCheckServiceClient();
           //var locs=dmClient.GetWHLocations();
           //int totRows = locs.Count();
           //Console.WriteLine(totRows.ToString());

           List<LocationHistory> allOne = new List<LocationHistory>();
           LocationHistory one = null;

           one = new LocationHistory() { PalletNum = "M085601", Location = "H3", IsManual = true, UserName = "fg", DeviceName = "S8-172.17.5.6" };
           allOne.Add(one);
           var result = dmClient.TransferPalletsToNewLocation(allOne.ToArray());
           if (result.Count() > 0)
               Console.WriteLine("Lines Transferred: " + result.Count().ToString());

           dmClient.Close();
           





            
            ModulaMovement.ModulaMovementClient clientMov = new ModulaMovement.ModulaMovementClient();
            var tbl = clientMov.ItemsNotUsed();

            bool isOpened=clientMov.OpenItemCode(167376, "2020-08-12 09:58:34");

            SalesOrderService.SalesOrderServiceClient soClient = new SalesOrderService.SalesOrderServiceClient();
            var salesid = soClient.FindSalesOrder("19SO-05804");
            

            //DateTime start = new DateTime(2019, 05, 15);
            //DateTime end = new DateTime(2019, 10, 15);
            var saleOrders = soClient.GetSalesOrders(start.ToShortDateString(), end.ToShortDateString(), "LC0004");


            ////var delv=soClient.GetDeliveries(start.ToShortDateString());

            //List<LocationHistory> allOne = new List<LocationHistory>();
            //LocationHistory one = null;

            ////one = new DMForTransfer() { palletNumField = "L012356", whLocationIdField = "E2" };
            ////allOne.Add(one);
            //one = new LocationHistory() { PalletNum = "L033737", Location = "E1", IsManual=false,UserName="fg",DeviceName="S8-172.17.5.6" };
            //allOne.Add(one);
            ////one = new LocationHistory() { PalletNum = "K700011", Location = "J2", IsManual = true, UserName = "fg", DeviceName = "S8-172.17.5.6" };
            ////allOne.Add(one);
            ////one = new LocationHistory() { PalletNum = "K215552", Location = "A3", IsManual = true, UserName = "mansoor", DeviceName = "S14-172.17.5.6" };
            ////allOne.Add(one);
            ////one = new LocationHistory() { PalletNum = "K215553", Location = "A4", IsManual = true, UserName = "mansoor", DeviceName = "S14-172.17.5.6" };
            ////allOne.Add(one);

            //DMCheckService.DMCheckServiceClient dmClient = new DMCheckService.DMCheckServiceClient();
            //var result = dmClient.TransferPalletsToNewLocation(allOne.ToArray());
            //if (result.Count() > 0)
            //    Console.WriteLine("Lines Transferred: " + result.Count().ToString());


            List<PalletItemContract> contract = new List<PalletItemContract>();
            contract.Add(new PalletItemContract
            {
                serialField = "L004649"
            });
           SalesOrder.SalesServiceClient client1 = new SalesServiceClient();
            var linesReturned = client1.CheckPalletAvailableMulti("20SO-00393", "GRA-3088010DM140", "G1", "PKL20-000984", contract.ToArray(),"KVN","S10",5637550335);
            

            var items1 = client1.ReceivePickingList("169.254.2.1", "169.254.2.1");
            foreach (SalesLine line in items1.Lines)
            {
                str += line.PickingId.ToString() + ": "+line.Location;
            }
            
            //FGService.FGSyncServiceClient client = new FGService.FGSyncServiceClient();

            //var dt = new DMCheckService.DMCheckServiceClient().GetProductionByLinesForChart(DateTime.Now.Date);


            
            //SalesOrder.SalesServiceClient client = new SalesServiceClient();
            //List<PalletItemContract> items = new List<PalletItemContract>();
            //items.Add(new PalletItemContract() { serialField = "K148334", updatedDateField = new DateTime(2020, 3, 1, 8, 9, 15), updatedDateFieldSpecified = true });
            //items.Add(new PalletItemContract() { serialField = "G206977", updatedDateField = new DateTime(2017, 5, 15, 8, 10, 12), updatedDateFieldSpecified = true });
            //items.Add(new PalletItemContract() { serialField = "G206978", updatedDateField = new DateTime(2017, 5, 15, 8, 10, 46), updatedDateFieldSpecified = true });
            //items.Add(new PalletItemContract() { serialField = "H172194", updatedDateField = new DateTime(2017, 5, 15, 8, 11, 52), updatedDateFieldSpecified = true });
            //items.Add(new PalletItemContract() { serialField = "H172195", updatedDateField = new DateTime(2017, 5, 15, 8, 12, 25), updatedDateFieldSpecified = true });
            //items.Add(new PalletItemContract() { serialField = "I035316", updatedDateField = new DateTime(2017, 5, 15, 8, 13, 53), updatedDateFieldSpecified = true });

            var result = client.CheckPalletAvailableMulti("20SO-01080", "CET-3049024AG090", "G1", "PKL20-002745", items.ToArray(), "Mansoor", "Computer", 0);
            if (result!=null)
            {
                new DBClass().CheckDMExport();

                new DBClass().ErrorInsert("16SO-06291", "ACMC-014127", "error string", "error trace", DateTime.Now, "SalesService", "Mansoor", "Computer", "CheckPalletAvailable");
            }
            ////SalesTableContract contract = client.findSalesOrder(context, "16SO-06192");
            ////Console.Write( contract.DeliveryName);

            //List<string> pallets = new List<string>();// { "I113646", "I113647", "I113648", "I113650" };
            //pallets.Add("I114272");
            //pallets.Add("I113647");
            //pallets.Add("I113648");
            //pallets.Add("I113650");

            
           string str="";
           var items = client.FindPickingList("ACMC-025199", "mansoor", "0.125");
           foreach (SalesLine line in items.Lines)
           {
              str += "Line# "+line.LineRecId.ToString()+Environment.NewLine;
           }

           Console.Write(str);
           Console.Write( items.SalesId);
           Console.Write(",  "+items.DeliveryDate.ToString());
           

            //var items = client.ValidatePallets("17SO-04439", "JOS-3049024SG090", "G1", "ACMC-016936", pallets.ToArray(), "Mansoor", "Computer 0.125", 5637162577);
            //if (items != null && items.Count() > 0)
            //  Console.Write(items[0].Name);
            //var items = client.ValidatePallets("16SO-06291", "CTO-3015008DM120", "G1", "ACMC-014108", pallets.ToArray(), "Mansoor", "Computer");

            List<string> pallets = new List<string>();
            pallets.Add("G145786");

            try
            {
                SalesServiceClient client = new SalesServiceClient();
                var items = client.ValidatePallets("17SO-02675", "LGN-3049010DM090", "ACMC-014670", pallets.ToArray(),"Mansoor","Computer 172.17.0.125");
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }

            //SCSyncServiceClient client = new SCSyncServiceClient();
            //List<ItemEntity> data = client.ResetData().ToList();

            //string count = data.Count().ToString();


            //**********ModulaPRClient**
            ModulaPRClient client = new ModulaPRClient();

            PMWorkOrder order = new PMWorkOrder()
            {                
                WOPoolCode = "Elec",
                WOType="Preventive",
                WOEquipment="EqpID-000000095",
                WorkOrderID = "W-0007070",
                StartDate=new DateTime(2017,2,10),
                WOStatus = "Created"
            };
            List<PMWorkItem> items = new List<PMWorkItem>();
            PMWorkItem item = new PMWorkItem()
            {
                ItemID="PP1E010016",
                Location="1005",
                Qty=5,
                UoM="Pcs",
                ItemRecId = 5637266099,
                Sto_ID=96103,
                Sto_Requester="Mansoor"
            };
            items.Add(item);
            item = new PMWorkItem()
            {
                ItemID = "SD1M030023",
                Location = "1003",
                Qty = 2,
                UoM = "PCS",
                ItemRecId = 5637266100,
                Sto_ID = 96104,
                Sto_Requester="Mansoor"
            };
            items.Add(item);
            item = new PMWorkItem()
            {
                ItemID = "CPM010422",
                Location = "1037",
                Qty = 2,
                UoM = "PCS",
                ItemRecId = 0,
                Sto_ID = 96105,
                Sto_Requester = "Mansoor"
            };
            items.Add(item);
            order.WorkItems = items.ToArray();


            var newOrder=client.SaveWorkOrder(order);
            if (newOrder != null)
            {
                string woId = newOrder.WorkOrderID;
            }

            //ModulaPRClient client = new ModulaPRClient();

            //int result = client.SetWorkOrderStatus("W-0007100", 6);
            //var items = client.GetOtherWorkItems("W-0007093", "", 10);
            //var items=client.GetInlineWorkItems("W-0007091");
            foreach (PMWorkItem item in items)
            {
                string name = item.ItemID;
                Console.WriteLine(name+", "+item.Sto_HostRIF+", "+item.ItemName);
            }

            UserMgtServices.UserMgtServiceClient client = new  UserMgtServices.UserMgtServiceClient();
            byte[] fileTo = client.DownloadFile("PH3200 HYDRAULIC PRESS WITH QUICK DIE-SET CHANGE-OVER DEVICE1_ACMC-02092_ACMC-01295.pdf", "\\\\172.17.0.150\\Dyn_Attach_Doc\\EamEqp\\");
            using (FileStream fs = new FileStream(@"C:\FilesToImport\HYDRAULIC PRESS.pdf", FileMode.OpenOrCreate))
            {
                fs.Write(fileTo, 0, fileTo.Length);
                fs.Close();
            }

            string count = fileTo.Length.ToString();

            Console.WriteLine("Bytes count: "+count);
            //Process.Start()
            var attendances = client.GetAttendances("h.bahakeem", "123",new DateTime(2016,10,20));
            foreach (UserMgtServices.AttendanceContract row in attendances)
            {
                Console.WriteLine(row.EmployeeId+", "+row.WorkStartTime+" -- "+row.WorkEndTime);
            }

            DMCheckService.DMExportContract line1 = new DMCheckService.DMExportContract()
            {
                palletNumField = "I000365",
                recordIdField = 54817,
                recordIdFieldSpecified=true,
                gradeField = "G7",
                shadeField = "D65",
                caliberField = "C7",
                shiftField = 2,
                lineOfOriginField = 8,
                whichMarpakField = 2,
                lGVOrForkliftField = DMCheckService.PalletTransportBy.LGV,
                totalSurfaceField = 34.990M,
                totalPiecesOnPalletField = 96,
                boxesOnPalletField = 24
            };

            bool result11 = false;
            DMCheckService.DMCheckServiceClient client = new DMCheckService.DMCheckServiceClient();
            //if (client.UpdateAndConfirmPalletReceive(line))
              //  result11 = true;
            DMCheckService.DMExportContract item = client.GetPalletInfo("J032910");
            
            Console.Write(item.itemDescField);
            
           DeviceMessage msg = new DeviceMessage()
           {
               DeviceName = "Mansoor",
               DateOccur = DateTime.Now,
               DeviceIP = "172.17.0.125",
               Message = "Testing from TestService",
               MethodName = "Main()",
               ProjectName = "SaleService",
               Parameters = "parameter name",
               Username = "Mansoor"
           };
           DeviceOpsClient opsClient = new DeviceOpsClient();
           bool isSaved = opsClient.SaveMessage(msg);
           Console.Write("Msg Saved");
             */

            Debug.Print("Ends Program...");
            Console.Read();
        }


        public async System.Threading.Tasks.Task<bool> CheckLoginAsync(string uName, string uPassword)
        {
            string message;
            bool isSuccess = false;

            var client = new HttpClient();

            
            string url = "http://productapi.arabian-ceramics.com/api/Account/Login";
            var item = new Credent
            {
                email = uName,
                password = uPassword
            };
            var jSonData = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var needToPost = new StringContent(jSonData, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(url, needToPost);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.                
                string result = await reader.ReadToEndAsync();

                var objects = JObject.Parse(result);

                foreach (JProperty p in objects.Properties())
                {
                    string name = p.Name;
                    string value = p.Value.ToString();
                    //Console.Write(name + ": " + value);

                    if (name.Equals("message"))
                        message = p.Value.ToString();
                    else if (name.Equals("isSuccess"))
                        isSuccess = bool.Parse(p.Value.ToString());
                }

            }

            return isSuccess;
        }
    }
}
