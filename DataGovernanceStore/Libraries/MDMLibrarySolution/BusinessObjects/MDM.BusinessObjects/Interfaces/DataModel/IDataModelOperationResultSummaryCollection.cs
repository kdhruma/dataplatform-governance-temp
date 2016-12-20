using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data model operation result summary collection. 
    /// </summary>
    public interface IDataModelOperationResultSummaryCollection : ICollection<DataModelOperationResultSummary>
    {
        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iDataModelOperationResultSummary">Object which implements interface <see cref="IDataModelOperationResultSummary"/></param>
        void Add(IDataModelOperationResultSummary iDataModelOperationResultSummary);

        /// <summary>
        /// Compare collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        String ToXml();
    }
}
