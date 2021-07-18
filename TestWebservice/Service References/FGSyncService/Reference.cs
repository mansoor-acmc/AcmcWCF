﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebservice.FGSyncService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FGSyncService.IFGSyncService")]
    public interface IFGSyncService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFGSyncService/ResetData", ReplyAction="http://tempuri.org/IFGSyncService/ResetDataResponse")]
        SyncServices.PalletEntity[] ResetData();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFGSyncService/GetFGYearInventory", ReplyAction="http://tempuri.org/IFGSyncService/GetFGYearInventoryResponse")]
        SyncServices.InventCountingService.InventAvailContract[] GetFGYearInventory(int startId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFGSyncService/GetUserData", ReplyAction="http://tempuri.org/IFGSyncService/GetUserDataResponse")]
        SyncServices.UserData[] GetUserData();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFGSyncService/UpdateFGDesktop", ReplyAction="http://tempuri.org/IFGSyncService/UpdateFGDesktopResponse")]
        long UpdateFGDesktop(SyncServices.PalletEntity[] dt);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFGSyncService/GetPing", ReplyAction="http://tempuri.org/IFGSyncService/GetPingResponse")]
        string GetPing();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFGSyncServiceChannel : TestWebservice.FGSyncService.IFGSyncService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FGSyncServiceClient : System.ServiceModel.ClientBase<TestWebservice.FGSyncService.IFGSyncService>, TestWebservice.FGSyncService.IFGSyncService {
        
        public FGSyncServiceClient() {
        }
        
        public FGSyncServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FGSyncServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FGSyncServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FGSyncServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SyncServices.PalletEntity[] ResetData() {
            return base.Channel.ResetData();
        }
        
        public SyncServices.InventCountingService.InventAvailContract[] GetFGYearInventory(int startId) {
            return base.Channel.GetFGYearInventory(startId);
        }
        
        public SyncServices.UserData[] GetUserData() {
            return base.Channel.GetUserData();
        }
        
        public long UpdateFGDesktop(SyncServices.PalletEntity[] dt) {
            return base.Channel.UpdateFGDesktop(dt);
        }
        
        public string GetPing() {
            return base.Channel.GetPing();
        }
    }
}