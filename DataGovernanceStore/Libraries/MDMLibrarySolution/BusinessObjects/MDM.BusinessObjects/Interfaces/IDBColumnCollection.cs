using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DynamicTableSchema;

    /// <summary>
    ///  Exposes methods or properties used for defining database column collection.
    /// </summary>
    public interface IDBColumnCollection : IEnumerable<DBColumn>
    {
        #region Properties
        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of DBColumnCollection object
        /// </summary>
        /// <returns>Xml string representing the DBColumnCollection</returns>
        String ToXml();

        #endregion
    }
}
