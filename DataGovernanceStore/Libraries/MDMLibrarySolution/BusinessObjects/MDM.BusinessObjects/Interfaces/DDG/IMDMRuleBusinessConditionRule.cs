using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains BusinessConditionRule information
    /// </summary>
    public interface IMDMRuleBusinessConditionRule
    {
        /// <summary>
        /// Property denoting the businesscondition id
        /// </summary>
        Int32 BusinessConditionId { get; set; }

        /// <summary>
        /// Property denoting the businesscondition name
        /// </summary>
        String BusinessConditionName { get; set; }

        /// <summary>
        /// Property denoting the businesscondition status
        /// </summary>
        RuleStatus BusinessConditionStatus { get; set; }

        /// <summary>
        /// Property denoting the businessconditionrule id
        /// </summary>
        Int32 BusinessConditionRuleId { get; set; }

        /// <summary>
        /// Property denoting the businessconditionrule name
        /// </summary>
        String BusinessConditionRuleName { get; set; }

        /// <summary>
        /// Property denoting the businessconditionrule status
        /// </summary>
        RuleStatus BusinessConditionRuleStatus { get; set; }

        /// <summary>
        /// Property denoting the businessconditionrule sequence
        /// </summary>
        Int32 Sequence { get; set; }
    }
}
