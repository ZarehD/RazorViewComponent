﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RazorViewComponentLib.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RazorViewComponentLib.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A slot component ({0}) cannot be used without a parent component..
        /// </summary>
        internal static string Err_CannotUseSlotWithoutParent_Fmt {
            get {
                return ResourceManager.GetString("Err_CannotUseSlotWithoutParent_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to add content of slot ({0}) to parent component..
        /// </summary>
        internal static string Err_FailedToAddSlot_Fmt {
            get {
                return ResourceManager.GetString("Err_FailedToAddSlot_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The partial view pathname is null, empty, or whitespace..
        /// </summary>
        internal static string Err_InvalidPartialViewPathname {
            get {
                return ResourceManager.GetString("Err_InvalidPartialViewPathname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Slot class ({0}) is not decorated with the[HtmlTargetElementAttribute attribute..
        /// </summary>
        internal static string Err_MissingHtmlTargetElement_Fmt {
            get {
                return ResourceManager.GetString("Err_MissingHtmlTargetElement_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing target element tag name in HtmlTargetElementAttribute attribute applied to slot class ({0})..
        /// </summary>
        internal static string Err_MissingHtmlTargetElementName_Fmt {
            get {
                return ResourceManager.GetString("Err_MissingHtmlTargetElementName_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value not specified for required attribute, &apos;{0}&apos;..
        /// </summary>
        internal static string Err_MissingRequiredAttribute_Fmt {
            get {
                return ResourceManager.GetString("Err_MissingRequiredAttribute_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A ViewContext was not available for this operation..
        /// </summary>
        internal static string Err_NoViewContext {
            get {
                return ResourceManager.GetString("Err_NoViewContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This slot component ({0}) has already been added to the parent component. Slots must be unique within the scope of a parent component..
        /// </summary>
        internal static string Err_SlotAlreadyExists_Fmt {
            get {
                return ResourceManager.GetString("Err_SlotAlreadyExists_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Slot ({0}) has no content..
        /// </summary>
        internal static string Err_SlotHasNoContent_Fmt {
            get {
                return ResourceManager.GetString("Err_SlotHasNoContent_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified slot is undefined: {0}..
        /// </summary>
        internal static string Msg_SlotNotDefined_Fmt {
            get {
                return ResourceManager.GetString("Msg_SlotNotDefined_Fmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to acquire an instance of type {0}..
        /// </summary>
        internal static string Msg_UnableToAcquireInstance_Fmt {
            get {
                return ResourceManager.GetString("Msg_UnableToAcquireInstance_Fmt", resourceCulture);
            }
        }
    }
}
