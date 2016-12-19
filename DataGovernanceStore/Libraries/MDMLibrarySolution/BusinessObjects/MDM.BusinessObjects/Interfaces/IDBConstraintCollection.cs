using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;

    /// <summary>
    /// Exposes methods or properties to set or get the database constraint collection related information.
    /// </summary>
    public interface IDBConstraintCollection : IEnumerable<DBConstraint>
    {
        #region Properties
        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of DBConstraintCollection object
        /// </summary>
        /// <returns>Xml string representing the DBConstraintCollection</returns>
        String ToXml();

        #endregion
    }
}
