﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChainmailleDesigner.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Chainmaille Designer\\Weaves")]
        public string PatternDirectory {
            get {
                return ((string)(this["PatternDirectory"]));
            }
            set {
                this["PatternDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Chainmaille Designer\\Designs")]
        public string DesignDirectory {
            get {
                return ((string)(this["DesignDirectory"]));
            }
            set {
                this["DesignDirectory"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int HistoryLimit
        {
            get
            {
                return ((int)(this["HistoryLimit"]));
            }
            set
            {
                this["HistoryLimit"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Hex\\HexSmall")]
        public string CurrentPattern {
            get {
                return ((string)(this["CurrentPattern"]));
            }
            set {
                this["CurrentPattern"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Gray")]
        public global::System.Drawing.Color BackgroundColor {
            get {
                return ((global::System.Drawing.Color)(this["BackgroundColor"]));
            }
            set {
                this["BackgroundColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color OutlineColor {
            get {
                return ((global::System.Drawing.Color)(this["OutlineColor"]));
            }
            set {
                this["OutlineColor"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Transparent")]
        public global::System.Drawing.Color UnspecifiedElementColor {
            get {
                return ((global::System.Drawing.Color)(this["UnspecifiedElementColor"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RingLord\\AnodizedAluminum")]
        public string MaterialPalette {
            get {
                return ((string)(this["MaterialPalette"]));
            }
            set {
                this["MaterialPalette"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Pattern File|*.xml;*.ini|XML File|*.xml|IGP Pattern File|*.ini")]
        public string PatternFileFilter {
            get {
                return ((string)(this["PatternFileFilter"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Design File (*.xml)|*.xml|Image File|*.bmp;*.png;*.tif|Bitmap (*.bmp)|*.bmp|Porta" +
            "ble Network Graphic (*.png)|*.png|Tagged Image File (*.tif)|*.tif")]
        public string DesignFileFilter {
            get {
                return ((string)(this["DesignFileFilter"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Image File|*.bmp;*.gif;*.jpg;*.png;*.tif|Bitmap (*.bmp)|*.bmp|Graphics Interchang" +
            "e Format (*.gif)|*.gif|Joint Photographic Experts Group (*.jpg)|*.jpg|Portable N" +
            "etwork Graphic (*.png)|*.png|Tagged Image File (*.tif)|*.tif")]
        public string ImageFileFilter {
            get {
                return ((string)(this["ImageFileFilter"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Chainmaille Designer\\Colors")]
        public string PaletteDirectory {
            get {
                return ((string)(this["PaletteDirectory"]));
            }
            set {
                this["PaletteDirectory"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Palette File (*.xml)|*.xml")]
        public string PaletteFileFilter {
            get {
                return ((string)(this["PaletteFileFilter"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\ColorIndicator2Rings.bmp")]
        public string PaletteSelectedColorImageFile {
            get {
                return ((string)(this["PaletteSelectedColorImageFile"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Any File (*.*)|*.*|Text File (*.txt)|*.txt")]
        public string ReportFileFilter {
            get {
                return ((string)(this["ReportFileFilter"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Design File (*.xml)|*.xml")]
        public string DesignFileOutFilter {
            get {
                return ((string)(this["DesignFileOutFilter"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DesignerName {
            get {
                return ((string)(this["DesignerName"]));
            }
            set {
                this["DesignerName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Help/index.html")]
        public string HelpUrl {
            get {
                return ((string)(this["HelpUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LICENSE.rtf")]
        public string LicenseFileName {
            get {
                return ((string)(this["LicenseFileName"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Fuchsia")]
        public global::System.Drawing.Color GuidelineColor {
            get {
                return ((global::System.Drawing.Color)(this["GuidelineColor"]));
            }
            set {
                this["GuidelineColor"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Image File|*.bmp;*.png;*.tif|Bitmap (*.bmp)|*.bmp|Portable Network Graphic (*.png" +
            ")|*.png|Tagged Image File|*.bmp;*.png;*.tif|Bitmap (*.bmp)|*.bmp|Portable Networ" +
            "k Graphic (*.png)|*.png|Tagged Image File (*.tif)|*.tif")]
        public string ColorImageFileFilter {
            get {
                return ((string)(this["ColorImageFileFilter"]));
            }
        }
    }
}
