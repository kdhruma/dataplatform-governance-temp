using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DynamicTableSchema;

    /// <summary>
    ///  Exposes methods or properties used for setting the database table collection.
    /// </summary>
    public interface IDBTableCollection : IEnumerable<DBTable>
    {
        #region Properties
        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of DBTableCollection object
        /// </summary>
        /// <returns>Xml string representing the DBTableCollection</returns>
        String ToXml();

        #endregion
    }
}
