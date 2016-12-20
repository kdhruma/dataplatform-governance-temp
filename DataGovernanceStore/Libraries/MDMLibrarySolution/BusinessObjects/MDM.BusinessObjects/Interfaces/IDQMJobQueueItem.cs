using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM job queue item.
    /// </summary>
    public interface IDQMJobQueueItem : IMDMObject
    {
        /// <summary>
        /// Property indicates unique Id representing table item
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Unique Id of the parent Job
        /// </summary>
        Int64? ParentJobId { get; set; }

        /// <summary>
        /// Property indicates Job type
        /// </summary>
        DQMJobType JobType { get; set; }

        /// <summary>
        /// Property indicates whether job is in initialization progress
        /// </summary>
        Boolean IsInitializationInProgress { get; set; }

        /// <summary>
        /// Property indicates whether job is initialized
        /// </summary>
        Boolean IsInitialized { get; set; }

        /// <summary>
        /// Property indicates whether job finalization is in progress
        /// </summary>
        Boolean IsFinalizationInProgress { get; set; }

        /// <summary>
        /// Property indicates whether Job has processed
        /// </summary>
        Boolean IsProcessed { get; set; }

        /// <summary>
        /// Property indicates whether Job is canceled
        /// </summary>
        Boolean IsCanceled { get; set; }

        /// <summary>
        /// Property indicates the Created Date time of Job
        /// </summary>
        DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Property indicates the time when initialization started
        /// </summary>
        DateTime? InitializationStartTime { get; set; }

        /// <summary>
        /// Property indicates the time when initialization ended
        /// </summary>
        DateTime? InitializationEndTime { get; set; }

        /// <summary>
        /// Property indicates the job finalization process start time
        /// </summary>
        DateTime? FinalizationStartTime { get; set; }

        /// <summary>
        /// Property indicates the processing start time of the Job
        /// </summary>
        DateTime? ProcessStartTime { get; set; }

        /// <summary>
        /// Property indicates the processing end time of the Job
        /// </summary>
        DateTime? ProcessEndTime { get; set; }

        /// <summary>
        /// Property indicates number of impacted entities effected by the Job
        /// </summary>
        Int64? ImpactedCount { get; set; }

        /// <summary>
        /// Property indicates server Id who has created this Job
        /// </summary>
        Int32? ServerId { get; set; }

        /// <summary>
        /// Field indicates server name who has created this Job
        /// </summary>
        String ServerName { get; set; }

        /// <summary>
        /// Job Context
        /// </summary>
        String Context { get; set; }

        /// <summary>
        /// Property indicates profile Id
        /// </summary>
        Int32? ProfileId { get; set; }

        /// <summary>
        /// Property indicates IsSystem job status
        /// </summary>
        Boolean IsSystem { get; set; }
    }
}