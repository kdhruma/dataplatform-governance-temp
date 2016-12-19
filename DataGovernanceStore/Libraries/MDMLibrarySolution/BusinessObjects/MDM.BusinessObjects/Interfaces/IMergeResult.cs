using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get merge results related information.
    /// </summary>
    public interface IMergeResult : IMDMObject, ICloneable
    {
        /// <summary>
        /// Indicates the MergeResult id
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates the MergeJob id
        /// </summary>
        Int64 MergeJobId { get; set; }

        /// <summary>
        /// Indicates the SourceEntity id
        /// </summary>
        Int64 SourceEntityId { get; set; }

        /// <summary>
        /// Indicates the TargetEntity id
        /// </summary>
        Int64? TargetEntityId { get; set; }

        /// <summary>
        /// Indicates the Default MergeAction
        /// </summary>
        MergeAction MergeAction { get; set; }

        /// <summary>
        /// Indicates the DateTime of Merge
        /// </summary>
        DateTime? MergeDateTime { get; set; }

        /// <summary>
        /// Indicates the merge result status
        /// </summary>
        MergeResultStatus MergeResultStatus { get; set; }
    }
}