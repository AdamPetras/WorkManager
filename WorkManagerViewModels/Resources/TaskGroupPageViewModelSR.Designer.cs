﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkManager.ViewModels.Resources {
    using System;
    
    
    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, formatting them, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilderEx class via the ResXFileCodeGeneratorEx custom tool.
    // To add or remove a member, edit your .ResX file then rerun the ResXFileCodeGeneratorEx custom tool or rebuild your VS.NET project.
    // Copyright (c) Dmytro Kryvko 2006-2021 (http://dmytro.kryvko.googlepages.com/)
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DMKSoftware.CodeGenerators.Tools.StronglyTypedResourceBuilderEx", "3.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
#if !SILVERLIGHT
    [global::System.Reflection.ObfuscationAttribute(Exclude=true, ApplyToMembers=true)]
#endif
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class TaskGroupPageViewModelSR {
        
        private static global::System.Resources.ResourceManager _resourceManager;
        
        private static object _internalSyncObject;
        
        private static global::System.Globalization.CultureInfo _resourceCulture;
        
        /// <summary>
        /// Initializes a TaskGroupPageViewModelSR object.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public TaskGroupPageViewModelSR() {
        }
        
        /// <summary>
        /// Thread safe lock object used by this class.
        /// </summary>
        public static object InternalSyncObject {
            get {
                if (object.ReferenceEquals(_internalSyncObject, null)) {
                    global::System.Threading.Interlocked.CompareExchange(ref _internalSyncObject, new object(), null);
                }
                return _internalSyncObject;
            }
        }
        
        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resourceManager, null)) {
                    global::System.Threading.Monitor.Enter(InternalSyncObject);
                    try {
                        if (object.ReferenceEquals(_resourceManager, null)) {
                            global::System.Threading.Interlocked.Exchange(ref _resourceManager, new global::System.Resources.ResourceManager("WorkManager.ViewModels.Resources.TaskGroupPageViewModelSR", typeof(TaskGroupPageViewModelSR).Assembly));
                        }
                    }
                    finally {
                        global::System.Threading.Monitor.Exit(InternalSyncObject);
                    }
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return _resourceCulture;
            }
            set {
                _resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Opravdu chcete smazat všechny úkoly?'.
        /// </summary>
        public static string ClearDialogMessage {
            get {
                return ResourceManager.GetString(ResourceNames.ClearDialogMessage, _resourceCulture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Smazání všech skupin úkolů'.
        /// </summary>
        public static string ClearDialogTitle {
            get {
                return ResourceManager.GetString(ResourceNames.ClearDialogTitle, _resourceCulture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Ne'.
        /// </summary>
        public static string DialogNo {
            get {
                return ResourceManager.GetString(ResourceNames.DialogNo, _resourceCulture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Ano'.
        /// </summary>
        public static string DialogYes {
            get {
                return ResourceManager.GetString(ResourceNames.DialogYes, _resourceCulture);
            }
        }
        
        /// <summary>
        /// Lists all the resource names as constant string fields.
        /// </summary>
        public class ResourceNames {
            
            /// <summary>
            /// Stores the resource name 'ClearDialogMessage'.
            /// </summary>
            public const string ClearDialogMessage = "ClearDialogMessage";
            
            /// <summary>
            /// Stores the resource name 'ClearDialogTitle'.
            /// </summary>
            public const string ClearDialogTitle = "ClearDialogTitle";
            
            /// <summary>
            /// Stores the resource name 'DialogNo'.
            /// </summary>
            public const string DialogNo = "DialogNo";
            
            /// <summary>
            /// Stores the resource name 'DialogYes'.
            /// </summary>
            public const string DialogYes = "DialogYes";
        }
    }
}
