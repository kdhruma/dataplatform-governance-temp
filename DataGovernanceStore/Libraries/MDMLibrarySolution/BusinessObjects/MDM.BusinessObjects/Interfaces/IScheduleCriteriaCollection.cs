using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of schedule criteria.
    /// </summary>
    public interface IScheduleCriteriaCollection : IEnumerable<ScheduleCriteria>
    {
        /// <summary>
        /// No. of ScheduleCriteria under current collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Add new item into the collection
        /// </summary>
        /// <param name="item">IScheduleCriteria</param>
        void Add(IScheduleCriteria item);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="Id">Id of ScheduleCriteria</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 Id);

        /// <summary>
        /// Compare collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Gets hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Get Schedule Criteria
        /// </summary>
        /// <param name="Id">Id of Schedule criteria</param>
        /// <returns>IScheduleCriteria</returns>
        IScheduleCriteria GetScheduleCriteria(int Id);

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 Id);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>Convert item to XML</returns>
        String ToXml();

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>Convert item to XML</returns>
        String ToXml(MDM.Core.ObjectSerialization serialization);
    }
}
