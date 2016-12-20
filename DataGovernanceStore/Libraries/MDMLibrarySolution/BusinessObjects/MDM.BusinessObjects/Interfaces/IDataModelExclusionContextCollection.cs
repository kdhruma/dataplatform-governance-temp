using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Interface for data model exclusion context instances
    /// </summary>
    public interface IDataModelExclusionContextCollection : IEnumerable<DataModelExclusionContext>
    {
        /// <summary>
        /// Get Xml representation of DataModelExclusionContext Collection
        /// </summary>
        /// <returns>Xml representation of DataModelExclusionContext Collection</returns>
        String ToXml();

        /// <summary>
        /// Add DataModelExclusionContext in collection
        /// </summary>
        /// <param name="iDataModelExclusionContext">DataModelExclusionContext to add in collection</param>
        void Add(IDataModelExclusionContext iDataModelExclusionContext);

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataModelExclusionContext collection
        /// </summary>
        /// <param name="iDataModelExclusionContext">The object to remove from the DataModelExclusionContext collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IDataModelExclusionContext iDataModelExclusionContext);
    }
}
