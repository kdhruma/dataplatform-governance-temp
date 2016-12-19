using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Jobs;

    /// <summary>
    /// Exposes methods or properties to set or get job scheduled collection.
    /// </summary>
    public interface IJobScheduleCollection : IEnumerable<JobSchedule>
    {
        /// <summary>
        ///  no. of Job Schedule
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Add item to current jobSchedule collection
        /// </summary>
        /// <param name="item">Job Schedule to add</param>
        void Add(IJobSchedule item);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="Id">Id of Job Schedule</param>
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
        /// Get JobSchedule
        /// </summary>
        /// <param name="Id">Id of JobSchedule</param>
        /// <returns>JobSchedule</returns>
        IJobSchedule GetJobSchedule(int Id);

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="jobScheduleId">jobScheduleId</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 jobScheduleId);

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
