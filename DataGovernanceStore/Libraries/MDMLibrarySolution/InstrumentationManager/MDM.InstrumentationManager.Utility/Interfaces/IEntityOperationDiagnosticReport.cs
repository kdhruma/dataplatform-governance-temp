using System;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Interfaces.Diagnostics;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get diagnostic report related information.
    /// </summary>
    public interface IEntityOperationDiagnosticReport : IDiagnosticReportBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        String InputDataXml { get; set; }

        #endregion
    }
}
