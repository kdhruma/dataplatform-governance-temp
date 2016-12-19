using System;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using MDM.BusinessObjects.Diagnostics;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties to set or get the diagnostic activity related information.
    /// </summary>
    public interface IDiagnosticActivity : IActivityBase, IDiagnosticDataElement
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        DateTime StartDateTime { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime EndDateTime { get; }

        /// <summary>
        /// 
        /// </summary>
        Double DurationInMilliSeconds { get; }

        /// <summary>
        /// 
        /// </summary>
        DiagnosticRecordCollection DiagnosticRecords { get; }

        /// <summary>
        /// 
        /// </summary>
        DiagnosticActivityCollection DiagnosticActivities { get; }

        #endregion

        #region Methods

        #endregion
    }
}
