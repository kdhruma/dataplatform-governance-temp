using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get data model operation result summary collection. 
    /// </summary>
    public interface IDDGOperationResultSummaryCollection : ICollection<DDGOperationResultSummary>
    {
        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iDdgOperationResultSummary">Object which implements interface <see cref="IDataModelOperationResultSummary"/></param>
        void Add(IDDGOperationResultSummary iDdgOperationResultSummary);

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
