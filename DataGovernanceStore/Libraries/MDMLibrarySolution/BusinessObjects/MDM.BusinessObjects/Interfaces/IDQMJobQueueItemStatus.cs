using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM job queue items status.
    /// </summary>
    public interface IDQMJobQueueItemStatus
    {
        /// <summary>
        /// Property indicates unique Id representing table item
        /// </summary>
        Int64 JobQueueId { get; set; }

        /// <summary>
        /// Property indicates job type
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
        /// Property indicates whether entity has processed
        /// </summary>
        Boolean IsProcessed { get; set; }

        /// <summary>
        /// Property indicates whether Job is canceled
        /// </summary>
        Boolean IsCanceled { get; set; }

        /// <summary>
        /// Property indicates the created date time of job
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
        /// Property indicates the processing start time of the job
        /// </summary>
        DateTime? ProcessStartTime { get; set; }

        /// <summary>
        /// Property indicates the processing end time of the job
        /// </summary>
        DateTime? ProcessEndTime { get; set; }

        /// <summary>
        /// Property indicates the number of processed jobs related to JobQueue item
        /// </summary>
        Int64 ProcessedEntitiesCount { get; set; }

        /// <summary>
        /// Property indicates the total number of jobs related to JobQueue item
        /// </summary>
        Int64 TotalEntitiesCount { get; set; }

        /// <summary>
        /// Property indicates the JobQueue item State
        /// </summary>
        JobQueueItemState State { get; }

        /// <summary>
        /// Property indicates the launcher of job 
        /// </summary>
        String UserName { get; set; }

        /// <summary>
        /// Property indicates profile Id
        /// </summary>
        Int32? ProfileId { get; set; }

		/// <summary>
		/// Property indicates profile name
		/// </summary>
		String ProfileName { get; set; }

        /// <summary>
        /// Property indicates LastActionUserName (Audit Ref data)
        /// </summary>
        String LastActionUserName { get; set; }

        /// <summary>
        /// Property indicates LastActionDateTime (Audit Ref data)
        /// </summary>
        DateTime? LastActionDateTime { get; set; }

        /// <summary>
        /// Property indicates LastActionProgramName (Audit Ref data)
        /// </summary>
        String LastActionProgramName { get; set; }

        /// <summary>
        /// Property indicates IsSystem job status
        /// </summary>
        Boolean IsSystem { get; set; }
    }
}