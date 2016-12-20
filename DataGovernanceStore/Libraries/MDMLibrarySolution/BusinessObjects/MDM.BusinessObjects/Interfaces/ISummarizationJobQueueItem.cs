using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for processing the summarization job queue item.
    /// </summary>
    public interface ISummarizationJobQueueItem : IMDMObject
    {
        /// <summary>
        /// Indicates identifier of summarization job queue item
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates container identifier of summarization job queue item
        /// </summary>
        Int32? ContainerId { get; set; }

        /// <summary>
        /// Indicates entity type identifier of summarization job queue item
        /// </summary>
        Int32? EntityTypeId { get; set; }

        /// <summary>
        /// Indicates category identifier of summarization job queue item
        /// </summary>
        Int64? CategoryId { get; set; }

        /// <summary>
        /// Indicates DQM job type of summarization job queue item
        /// </summary>
        DQMJobType JobType { get; set; }

        /// <summary>
        /// Indicates if the summarization job is in progress
        /// </summary>
        Boolean IsInProgress { get; set; }

        /// <summary>
        /// Indicates weightage of summarization job queue item
        /// </summary>
        Int32? Weightage { get; set; }

        /// <summary>
        /// Indicates parent job identifier of summarization job queue item
        /// </summary>
        Int64 ParentJobId { get; set; }

        /// <summary>
        /// Indicates server identifier of summarization job queue item
        /// </summary>
        Int32? ServerId { get; set; }

        /// <summary>
        /// Indicates server name of summarization job queue item
        /// </summary>
        String ServerName { get; set; }

        /// <summary>
        /// Indicates last modified date time of summarization job queue item
        /// </summary>
        DateTime? LastModifiedDateTime { get; set; }

    }
}