using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of tables.
    /// </summary>
    public interface ITableCollection : IEnumerable<Table>
    {
        #region ToXml methods

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <returns>Xml string representing the TableCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml( ObjectSerialization serialization );

        #endregion ToXml methods

        /// <summary>
        /// Add table in ITableCollection
        /// </summary>
        /// <param name="table">Table to add in ITableCollection</param>
        /// <exception cref="ArgumentNullException">Thrown if table is null</exception>
        void AddTable( ITable table );

        /// <summary>
        /// Add tables in current table collection
        /// </summary>
        /// <param name="tables">ITableCollection to add in current collection</param>
        /// <exception cref="ArgumentNullException">Thrown if tableCollection is null</exception>
        void AddTables( ITableCollection tables );
    }
}
