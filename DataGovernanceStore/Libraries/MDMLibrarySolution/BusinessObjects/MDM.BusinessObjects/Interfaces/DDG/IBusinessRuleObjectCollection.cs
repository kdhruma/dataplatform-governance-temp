using System;
using System.Collections;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for Business Rule Object Collection
    /// </summary>
    public interface IBusinessRuleObjectCollection : IEnumerable
    {
        /// <summary>
        /// Represents no. of business rule object present into the collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Removes the BusinessRule object from Collection by reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id of BusinessRule object</param>
        /// <returns>True - If BusinessRule object is removed successfully, False - If BusinessRule object is not removed successfully</returns>
        Boolean RemoveByReferenceId(Int64 referenceId);
    }
}
