using System;
using System.Collections.Generic;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Exposes methods or properties to set or get diagnostic record collection.
    /// </summary>
    public interface IDiagnosticActivityCollection : IEnumerable<DiagnosticActivity>, ICollection<DiagnosticActivity>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of diagnosticActivity Collection
        /// </summary>
        /// <returns>Xml representation of diagnosticActivity Collection</returns>
        String ToXml();

        #endregion
    }
}
