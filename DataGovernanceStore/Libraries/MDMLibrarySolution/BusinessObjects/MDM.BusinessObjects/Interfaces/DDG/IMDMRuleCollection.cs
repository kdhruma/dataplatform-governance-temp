using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains collection of MDMRules
    /// </summary>
    public interface IMDMRuleCollection : ICollection<MDMRule>
    {
        /// <summary>
        /// Gets Xml representation of MDMRule Collection
        /// </summary>
        /// <returns>Returns xml representation of MDMRuleCollection</returns>
        String ToXml();

        /// <summary>
        /// Compares MDMRuleCollection with current collection
        /// </summary>
        /// <param name="subSetMDMRuleCollection">Indicates MDMRuleCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        Boolean IsSuperSetOf(MDMRuleCollection subSetMDMRuleCollection);

        /// <summary>
        /// Gets MDMRule based on the ruleid from MDMRuleCollection
        /// </summary>
        /// <param name="mdmRuleId">Id of rule</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        MDMRule GetMDMRuleById(Int32 mdmRuleId);

        /// <summary>
        /// Gets MDMRule based on the rulename from MDMRuleCollection
        /// </summary>
        /// <param name="mdmRuleName">Name of rule</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        MDMRule GetMDMRuleByName(String mdmRuleName);

        /// <summary>
        /// Returns id of all mdmrules
        /// </summary>
        /// <param name="ruleStatus">Parameter to filter mdmrule id's based on rulestatus</param>
        /// <returns>Collection of mdmrule id's</returns>
        ICollection<Int32> GetMDMRuleIds(RuleStatus ruleStatus = RuleStatus.Unknown);

        /// <summary>
        /// Returns name of all mdmrules
        /// </summary>
        /// <param name="ruleStatus">Parameter to filter mdmrule names based on rulestatus</param>
        /// <returns>Collection of mdmrule names</returns>
        ICollection<String> GetMDMRuleNames(RuleStatus ruleStatus = RuleStatus.Unknown);

        /// <summary>
        /// Gets all Non-BusinessConditions Rules
        /// </summary>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>Returns all Non-BusinessCondition Rules</returns>
        IMDMRuleCollection GetAllBusinessRules(RuleStatus ruleStatus = RuleStatus.Unknown);

        /// <summary>
        /// Gets all BusinessConditions
        /// </summary>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>Returns all Non-BusinessCondition Rules</returns>
        IMDMRuleCollection GetAllBusinessConditions(RuleStatus ruleStatus = RuleStatus.Unknown);

        /// <summary>
        /// Gets the rules by status
        /// </summary>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>Returns the MDMRules by status</returns>
        IMDMRuleCollection GetMDMRulesByStatus(RuleStatus ruleStatus = RuleStatus.Unknown);
    }
}