using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get merge plan item properties.
    /// </summary>
    public interface IMergePlanItem : IMDMObject
    {
        /// <summary>
        /// Indicates the Merge Plan Item Id
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates the Merge Planning Job Id
        /// </summary>
        Int64? MergePlanningJobId { get; set; }

        /// <summary>
        /// Indicates the Matching Job Id
        /// </summary>
        Int64? MatchingJobId { get; set; }

        /// <summary>
        /// Indicates the Source Entity Id
        /// </summary>
        Int64 SourceEntityId { get; set; }

        /// <summary>
        /// Indicates the Chosen Suspect Entity Id
        /// </summary>
        Int64 ChosenSuspectEntityId { get; set; }
        
        /// <summary>
        /// Indicates the ChosenSuspectScore
        /// </summary>
        Double ChosenSuspectScore { get; set; }

        /// <summary>
        /// Indicates the ChosenSuspectUserName
        /// </summary>
        String ChosenSuspectUserName { get; set; }

        /// <summary>
        /// Indicates the Merge Actions
        /// </summary>
        Collection<MergeAction> MergeActions { get; set; }

        /// <summary>
        /// Indicates the User Review Status
        /// </summary>
        MergePlanUserReviewStatus UserReviewStatus { get; set; }

        /// <summary>
        /// Indicates the Reviewing User
        /// </summary>
        String ReviewingUser { get; set; }

        /// <summary>
        /// Indicates the Merge Status
        /// </summary>
        MergeStatus? MergeStatus { get; set; }

        /// <summary>
        /// Indicates the Final Entity Id
        /// </summary>
        Int64? FinalTargetEntityId { get; set; }

        /// <summary>
        /// Indicates the Last Modification
        /// </summary>
        DateTime? LastModification { get; set; }

        /// <summary>
        /// Indicates the Status Message
        /// </summary>
        String StatusMessage { get; set; }
        
        /// <summary>
        /// Indicates the Workflow Name
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Indicates the target container id
        /// </summary>
        Int32? TargetContainerId { get; set; }

        /// <summary>
        /// Indicates the relationship type id
        /// </summary>
        Int32? RelationshipTypeId { get; set; }
    }
}