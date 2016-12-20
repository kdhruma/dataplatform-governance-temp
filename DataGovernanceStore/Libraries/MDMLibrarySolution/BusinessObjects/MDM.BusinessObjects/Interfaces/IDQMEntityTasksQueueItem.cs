using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get DQM entity tasks queue item.
    /// </summary>
    public interface IDQMEntityTasksQueueItem : IMDMObject
    {
        /// <summary>
        /// Property indicates queue item Id
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property indicates parent Job Id
        /// </summary>
        Int64 ParentJobId { get; set; }

        /// <summary>
        /// Property indicates the Id of Entity
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Indicates the catalog Id of entity
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property indicates Job type
        /// </summary>
        DQMJobType JobType { get; set; }

        /// <summary>
        /// Property indicates whether item is in processing state
        /// </summary>
        Boolean IsInProgress { get; set; }

        /// <summary>
        /// Property indicates weightage of an queued entity
        /// </summary>
        Int16? Weightage { get; set; }

        /// <summary>
        ///  Property indicates the last modified time of this queued entity
        /// </summary>
        DateTime? LastModifiedDateTime { get; set; }

        /// <summary>
        /// Property indicates server Id who has created this entity task
        /// </summary>
        Int32? ServerId { get; set; }

        /// <summary>
        /// Property indicates server name who has created this entity task
        /// </summary>
        String ServerName { get; set; }

        /// <summary>
        /// Property indicating entity Short name
        /// </summary>
        String EntityName { get; set; }

        /// <summary>
        /// Property indicating entity Long name
        /// </summary>
        String EntityLongName { get; set; }

        /// <summary>
        /// Property indicates task Context
        /// </summary>
        String Context { get; set; }

        /// <summary>
        /// Property indicates Profile Id
        /// </summary>
        Int32? ProfileId { get; set; }

        /// <summary>
        /// Property indicates parent job IsSystem status
        /// </summary>
        Boolean ParentJobIsSystem { get; set; }
    }
}