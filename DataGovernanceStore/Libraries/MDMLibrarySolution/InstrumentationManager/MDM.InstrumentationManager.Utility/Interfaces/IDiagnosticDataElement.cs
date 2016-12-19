using System;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties to set or get the diagnostic record related information.
    /// </summary>
    public interface IDiagnosticDataElement
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        DateTime TimeStamp { get;}

        /// <summary>
        ///
        /// </summary>
        Int64 ReferenceId { get; }

        /// <summary>
        ///
        /// </summary>
        Guid ActivityId { get; }

        /// <summary>
        ///
        /// </summary>
        Guid OperationId { get;}

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of DiagnosticRecord
        /// </summary>
        /// <returns>Xml representation of DiagnosticRecord</returns>
        String ToXml();

        /// <summary>
        /// GetExecutionContext
        /// </summary>
        /// <returns></returns>
        IExecutionContext GetExecutionContext();

        #endregion
    }
}
