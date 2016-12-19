using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get data processor entity.
    /// </summary>
    public interface IDataProcessorEntity
    {
        /// <summary>
        /// Property Denoting Entity Id
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property Denoting Container Id
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property Denoting Program Name
        /// </summary>
        String ProgramName { get; set; }

        /// <summary>
        /// Property Denoting EntityActivityLog Id
        /// </summary>
        Int64 EntityActivityLogId { get; set; }

        /// <summary>
        /// Property Denoting Weightage
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Property Denoting PerformedAction on EntityActivityList
        /// </summary>
        EntityActivityList PerformedAction { get; set; }
    }
}
