using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get range rule related information.
    /// </summary>
    public interface IRangeRule : ICloneable
    {
        /// <summary>
        /// Property denoting Order
        /// </summary>
        Int32 Order { get; set; }

        /// <summary>
        /// Property denoting lower limit (more or equal to this value)
        /// </summary>
        Double From { get; set; }

        /// <summary>
        /// Property denoting upper limit (less or equal than this value)
        /// </summary>
        Double To { get; set; }

        /// <summary>
        /// Property denoting Merge Action
        /// </summary>
        MergeAction Action { get; set; }

        /// <summary>
        /// Field for Realtionship type Id
        /// </summary>
        Int32? RelationshipTypeId { get; set; }

        /// <summary>
        /// Field for target Container Id
        /// </summary>
        Int32? TargetContainerId { get; set; }

        /// <summary>
        /// Field for Workflow Name
        /// </summary>
        String WorkflowName{ get; set; }

        /// <summary>
        /// Property denoting MessageCode to be used for button text while generating the action button using given MergePlanningProfile
        /// </summary>
        String ActionLabelMessageCode { get; set; }

        /// <summary>
        /// Property denoting merging profile id
        /// </summary>
        Int32? MergingProfileId { get; set; }
    }
}