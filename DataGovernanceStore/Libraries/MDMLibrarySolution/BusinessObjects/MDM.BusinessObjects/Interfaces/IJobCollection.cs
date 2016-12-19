using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;

    /// <summary>
    /// Exposes methods or properties to set or get job collection.
    /// </summary>
    public interface IJobCollection : ICollection<Job>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Add item to the collection
        /// </summary>
        /// <param name="iJob">Object which implements interface <see cref="IJob"/></param>
        void Add(IJob iJob);

        /// <summary>
        /// Clone Job collection.
        /// </summary>
        /// <returns>cloned Job collection object.</returns>
        IJobCollection Clone();

        /// <summary>
        /// Check by Id if Job exists in collection
        /// </summary>
        /// <param name="id">Id of job</param>
        /// <returns>Presence of Job in collection</returns>
        Boolean Contains(Int32 id);

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="jobId">Item Id</param>
        /// <returns>Cloned item</returns>
        IJob Get(Int32 jobId);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Remove Job from collection
        /// </summary>
        /// <param name="jobId">Id of Job which should be removed</param>
        /// <returns>Success of removing</returns>
        Boolean Remove(Int32 jobId);

        /// <summary>
        /// Get Xml representation of JobCollection object
        /// </summary>
        /// <returns>Xml string representing the JobCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
