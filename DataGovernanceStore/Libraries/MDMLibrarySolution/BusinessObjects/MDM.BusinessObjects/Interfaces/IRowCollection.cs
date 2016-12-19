using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of rows.
    /// </summary>
    public interface IRowCollection : IEnumerable<Row>
    {
        #region ToXml methods

        /// <summary>
        /// Get Xml representation of CellCollection object
        /// </summary>
        /// <returns>Xml string representing the CellCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of CellCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml( ObjectSerialization serialization );

        #endregion ToXml methods
    }
}
