using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity activity log item status.
    /// </summary>
    public interface IEntityActivityLogItemStatus
    {
        /// <summary>
        /// Property indicates EntityActivityLog record Id
        /// </summary>
        Int64 EntityActivityLogId { get; set; }

        /// <summary>
        /// Indicates the activity action
        /// </summary>
        EntityActivityList PerformedAction { get; set; }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        Boolean? IsLoadingInProgress { get; set; }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        Boolean? IsLoaded { get; set; }

        /// <summary>
        ///  Property indicates whether all the loaded impacted entites in the impacted Entity table has processed
        /// </summary>
        Boolean? IsProcessed { get; set; }

        /// <summary>
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        DateTime? LoadStartTime { get; set; }

        /// <summary>
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        DateTime? LoadEndTime { get; set; }

        /// <summary>
        /// Property indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        DateTime? ProcessStartTime { get; set; }

        /// <summary>
        ///  Property indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        DateTime? ProcessEndTime { get; set; }

        /// <summary>
        /// Property indicates the Created Date time of Activity log table record
        /// </summary>
        DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Property indicates the number of processed entities related to EntityActivityLog item
        /// </summary>
        Int64 ProcessedEntitiesCount { get; set; }

        /// <summary>
        /// Property indicates the total number of entities related to EntityActivityLog item
        /// </summary>
        Int64 TotalEntitiesCount { get; set; }

        /// <summary>
        /// Property indicates the EntityActivityLog item State
        /// </summary>
        EntityActivityLogItemState State { get; }

        /// <summary>
        /// Property indicates the launcher of job 
        /// </summary>
        String UserName { get; set; }
    }
}
