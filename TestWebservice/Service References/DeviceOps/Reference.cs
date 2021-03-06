﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebservice.DeviceOps {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeviceMessage", Namespace="http://schemas.datacontract.org/2004/07/SyncServices")]
    [System.SerializableAttribute()]
    public partial class DeviceMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateOccurField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeviceIPField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeviceNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsSavedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MethodNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParametersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateOccur {
            get {
                return this.DateOccurField;
            }
            set {
                if ((this.DateOccurField.Equals(value) != true)) {
                    this.DateOccurField = value;
                    this.RaisePropertyChanged("DateOccur");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DeviceIP {
            get {
                return this.DeviceIPField;
            }
            set {
                if ((object.ReferenceEquals(this.DeviceIPField, value) != true)) {
                    this.DeviceIPField = value;
                    this.RaisePropertyChanged("DeviceIP");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DeviceName {
            get {
                return this.DeviceNameField;
            }
            set {
                if ((object.ReferenceEquals(this.DeviceNameField, value) != true)) {
                    this.DeviceNameField = value;
                    this.RaisePropertyChanged("DeviceName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSaved {
            get {
                return this.IsSavedField;
            }
            set {
                if ((this.IsSavedField.Equals(value) != true)) {
                    this.IsSavedField = value;
                    this.RaisePropertyChanged("IsSaved");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MethodName {
            get {
                return this.MethodNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MethodNameField, value) != true)) {
                    this.MethodNameField = value;
                    this.RaisePropertyChanged("MethodName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameters {
            get {
                return this.ParametersField;
            }
            set {
                if ((object.ReferenceEquals(this.ParametersField, value) != true)) {
                    this.ParametersField = value;
                    this.RaisePropertyChanged("Parameters");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProjectName {
            get {
                return this.ProjectNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectNameField, value) != true)) {
                    this.ProjectNameField = value;
                    this.RaisePropertyChanged("ProjectName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DeviceOps.IDeviceOps")]
    public interface IDeviceOps {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDeviceOps/Ping", ReplyAction="http://tempuri.org/IDeviceOps/PingResponse")]
        bool Ping(TestWebservice.DeviceOps.DeviceMessage msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDeviceOps/SaveMessage", ReplyAction="http://tempuri.org/IDeviceOps/SaveMessageResponse")]
        bool SaveMessage(TestWebservice.DeviceOps.DeviceMessage msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDeviceOps/SaveMessages", ReplyAction="http://tempuri.org/IDeviceOps/SaveMessagesResponse")]
        TestWebservice.DeviceOps.DeviceMessage[] SaveMessages(TestWebservice.DeviceOps.DeviceMessage[] msgs);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDeviceOpsChannel : TestWebservice.DeviceOps.IDeviceOps, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DeviceOpsClient : System.ServiceModel.ClientBase<TestWebservice.DeviceOps.IDeviceOps>, TestWebservice.DeviceOps.IDeviceOps {
        
        public DeviceOpsClient() {
        }
        
        public DeviceOpsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DeviceOpsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DeviceOpsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DeviceOpsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Ping(TestWebservice.DeviceOps.DeviceMessage msg) {
            return base.Channel.Ping(msg);
        }
        
        public bool SaveMessage(TestWebservice.DeviceOps.DeviceMessage msg) {
            return base.Channel.SaveMessage(msg);
        }
        
        public TestWebservice.DeviceOps.DeviceMessage[] SaveMessages(TestWebservice.DeviceOps.DeviceMessage[] msgs) {
            return base.Channel.SaveMessages(msgs);
        }
    }
}
