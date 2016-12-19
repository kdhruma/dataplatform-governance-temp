using System;
using MDM.BusinessObjects.Imports;
using MDM.Core;
using MDM.BusinessObjects.Diagnostics;
using MDM.BusinessObjects.Exports;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the diagnostic report profile related information.
    /// </summary>
    public interface IDiagnosticReportProfile : IMDMObject
    {
        /// <summary>
        /// Property denoting reportType
        /// </summary>
        DiagnosticToolsReportType DiagnosticToolsReportType { get; set; }

        /// <summary>
        /// Property denoting reportType
        /// </summary>
        DiagnosticToolsReportSubType DiagnosticToolsReportSubType { get; set; }

        /// <summary>
        /// Property denoting input xml
        /// </summary>
        String InputXml { get; set; }

        /// <summary>
        /// Property denoting File is enabled or not
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting UserInterface Profile
        /// </summary>
        String UIProfile { get; set; }

    }
}
