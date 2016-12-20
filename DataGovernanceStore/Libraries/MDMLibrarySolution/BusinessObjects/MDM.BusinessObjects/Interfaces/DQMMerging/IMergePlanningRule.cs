using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get merge planning rule.
    /// </summary>
    public interface IMergePlanningRule : ICloneable
    {
        /// <summary>
        /// Property denoting Default MergeAction
        /// </summary>
        MergeAction DefaultAction { get; set; }

        /// <summary>
        /// Property denoting RangeRules Collection
        /// </summary>
        RangeRuleCollection RangeRules { get; set; }

        /// <summary>
        /// Property denoting Realtionship type Id
        /// </summary>
        Int32? RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting target Container Id
        /// </summary>
        Int32? TargetContainerId { get; set; }

        /// <summary>
        /// Property denoting Workflow Name
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting MessageCode to be used for button text while generating the action button using given MergePlanningProfile
        /// </summary>
        String ActionLabelMessageCode { get; set; }
    }
}