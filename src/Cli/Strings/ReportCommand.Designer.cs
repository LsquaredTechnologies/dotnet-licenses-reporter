﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lsquared.DotnetLicensesReporter.Strings {
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
    internal class ReportCommand {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ReportCommand() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Lsquared.DotnetLicensesReporter.Strings.ReportCommand", typeof(ReportCommand).Assembly);
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
        ///   Looks up a localized string similar to Collect and report all licenses used in a project or solution..
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Open the generated file with the default application..
        /// </summary>
        internal static string OpenFileOptionDescription {
            get {
                return ResourceManager.GetString("OpenFileOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The output directory write files to..
        /// </summary>
        internal static string OutputDirectoryOptionDescription {
            get {
                return ResourceManager.GetString("OutputDirectoryOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OUTPUT_PATH.
        /// </summary>
        internal static string OutputDirectoryOptionHelpName {
            get {
                return ResourceManager.GetString("OutputDirectoryOptionHelpName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The output formats to use to display package licenses..
        /// </summary>
        internal static string OutputFormatsOptionDescription {
            get {
                return ResourceManager.GetString("OutputFormatsOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FORMAT.
        /// </summary>
        internal static string OutputFormatsOptionHelpName {
            get {
                return ResourceManager.GetString("OutputFormatsOptionHelpName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project or solution to report licenses from..
        /// </summary>
        internal static string ProjectArgumentDescription {
            get {
                return ResourceManager.GetString("ProjectArgumentDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PROJECT | SOLUTION.
        /// </summary>
        internal static string ProjectArgumentHelpName {
            get {
                return ResourceManager.GetString("ProjectArgumentHelpName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Suppress output to console..
        /// </summary>
        internal static string SilentOptionDescription {
            get {
                return ResourceManager.GetString("SilentOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A liquid template file to use to display package licenses when output-formats contain &quot;template&quot;..
        /// </summary>
        internal static string TemplateOptionDescription {
            get {
                return ResourceManager.GetString("TemplateOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TEMPLATE_PATH.
        /// </summary>
        internal static string TemplateOptionHelpName {
            get {
                return ResourceManager.GetString("TemplateOptionHelpName", resourceCulture);
            }
        }
    }
}
