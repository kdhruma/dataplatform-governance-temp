using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM processor error log item.
    /// </summary>
    public interface IDQMProcessorErrorLogItem : IMDMObject
    {
        /// <summary>
        /// Property indicates error log item Id
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property indicates Job Id
        /// </summary>
        Int64? JobId { get; set; }

        /// <summary>
        /// Property indicates Job type
        /// </summary>
        DQMJobType? JobType { get; set; }

        /// <summary>
        /// Property indicates the Id of Entity
        /// </summary>
        Int64? EntityId { get; set; }

        /// <summary>
        /// Property indicates the catalog Id of entity
        /// </summary>
        Int32? ContainerId { get; set; }

        /// <summary>
        ///  Property indicates the last modified time of this queued entity. Please set to Null if you want to use SQL Server's current date and time.
        /// </summary>
        DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Property indicates error message
        /// </summary>
        String ErrorMessage { get; set; }

        /// <summary>
        /// Property indicates processor name
        /// </summary>
        String ProcessorName { get; set; }

        /// <summary>
        /// Property indicates user
        /// </summary>
        String ModifiedUser { get; set; }

        /// <summary>
        /// Property indicates program
        /// </summary>
        String ModifiedProgram { get; set; }
    }
}