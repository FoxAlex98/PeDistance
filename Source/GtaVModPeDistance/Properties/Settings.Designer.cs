﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GtaVModPeDistance.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int PedMinSpawningDistance {
            get {
                return ((int)(this["PedMinSpawningDistance"]));
            }
            set {
                this["PedMinSpawningDistance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int PedMaxSpawningDistance {
            get {
                return ((int)(this["PedMaxSpawningDistance"]));
            }
            set {
                this["PedMaxSpawningDistance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int CameraMinSpawningHeight {
            get {
                return ((int)(this["CameraMinSpawningHeight"]));
            }
            set {
                this["CameraMinSpawningHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int CameraMaxSpawningHeight {
            get {
                return ((int)(this["CameraMaxSpawningHeight"]));
            }
            set {
                this["CameraMaxSpawningHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int TeleportingDelay {
            get {
                return ((int)(this["TeleportingDelay"]));
            }
            set {
                this["TeleportingDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int RenderingDelay {
            get {
                return ((int)(this["RenderingDelay"]));
            }
            set {
                this["RenderingDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int PedSpawningDelay {
            get {
                return ((int)(this["PedSpawningDelay"]));
            }
            set {
                this["PedSpawningDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int CollectingDataDelay {
            get {
                return ((int)(this["CollectingDataDelay"]));
            }
            set {
                this["CollectingDataDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ClearCollectingDataDelay {
            get {
                return ((int)(this["ClearCollectingDataDelay"]));
            }
            set {
                this["ClearCollectingDataDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.8")]
        public float CameraFixedHeight {
            get {
                return ((float)(this["CameraFixedHeight"]));
            }
            set {
                this["CameraFixedHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int MaxCollectedData {
            get {
                return ((int)(this["MaxCollectedData"]));
            }
            set {
                this["MaxCollectedData"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int MaxCollectedSelectionable {
            get {
                return ((int)(this["MaxCollectedSelectionable"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AdvancedMode {
            get {
                return ((bool)(this["AdvancedMode"]));
            }
            set {
                this["AdvancedMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("No")]
        public string SaveScreenShotLocally {
            get {
                return ((string)(this["SaveScreenShotLocally"]));
            }
            set {
                this["SaveScreenShotLocally"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MachineLearningProject")]
        public string DirectoryName {
            get {
                return ((string)(this["DirectoryName"]));
            }
        }
    }
}
