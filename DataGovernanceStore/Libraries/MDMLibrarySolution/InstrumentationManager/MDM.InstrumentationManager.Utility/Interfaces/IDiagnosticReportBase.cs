using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using MDM.BusinessObjects.Diagnostics;    
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// Exposes methods or properties to set or get diagnostic report base.
    /// </summary>
    public interface IDiagnosticReportBase
    {
        #region Properties

        /// <summary>
        /// Property denoting execution date and time
        /// </summary>
        DateTime ExecutionDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DiagnosticRecordCollection DiagnosticRecords { get; set; }

        /// <summary>
        /// Root Activity
        /// </summary>
        DiagnosticActivity Activity { get; }

        /// <summary>
        /// Property denoting the main operation that this report is capturing diagnostic report for
        /// </summary>
        String MainOperation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// AddDiagnosticRecord
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message);
        
        /// <summary>
        /// AddDiagnosticRecord
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="durationInMilliseconds"></param>
        void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Double durationInMilliseconds);

        /// <summary>
        /// AddDiagnosticRecord
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="durationInMilliseconds"></param>
        void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds);

        /// <summary>
        /// AddDiagnosticRecord
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="dataXml"></param>
        void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, String dataXml);

        /// <summary>
        /// Adds given DiagnoticReport to current report.
        /// </summary>
        /// <param name="report">DiagnosticReport</param>
        void AddDiagnosticReport(DiagnosticReportBase report);

        /// <summary>
        /// Gets Xml representation of Diagnostic Report Base
        /// </summary>
        /// <returns>Xml representation of Diagnostic Report Base</returns>
        String ToXml();

        #endregion
    }
}
