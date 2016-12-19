using System;
using System.Collections.Generic;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Exposes methods or properties to set or get diagnostic record collection.
    /// </summary>
    public interface IDiagnosticRecordCollection : IEnumerable<DiagnosticRecord>, ICollection<DiagnosticRecord>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of diagnosticRecord Collection
        /// </summary>
        /// <returns>Xml representation of diagnosticRecord Collection</returns>
        String ToXml();

        #endregion
    }
}
