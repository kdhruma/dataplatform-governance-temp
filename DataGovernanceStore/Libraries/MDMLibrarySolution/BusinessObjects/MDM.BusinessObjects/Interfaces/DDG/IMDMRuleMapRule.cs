using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains MDMRuleMapRule information
    /// </summary>
    public interface IMDMRuleMapRule
    {
        #region Properties

        /// <summary>
        /// Property denoting the rulemap id
        /// </summary>
        Int32 RuleMapId { get; set; }

        /// <summary>
        /// Property denoting the rulemap name
        /// </summary>
        String RuleMapName { get; set; }

        /// <summary>
        /// Property denoting the rule id
        /// </summary>
        Int32 RuleId { get; set; }

        /// <summary>
        /// Property denoting the rule name
        /// </summary>
        String RuleName { get; set; }

        /// <summary>
        /// Property denoting the rule type
        /// </summary>
        MDMRuleType RuleType { get; set; }

        /// <summary>
        /// Property denoting the rule status
        /// </summary>
        RuleStatus RuleStatus { get; set; }

        /// <summary>
        /// Property denoting the ignorechangecontext
        /// </summary>
        Boolean IgnoreChangeContext { get; set; }

        /// <summary>
        /// Property denoting the sequence
        /// </summary>
        Int32 Sequence { get; set; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
