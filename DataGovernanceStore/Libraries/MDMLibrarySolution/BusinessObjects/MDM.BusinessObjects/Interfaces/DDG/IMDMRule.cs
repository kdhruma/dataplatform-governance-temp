using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains MDMRule information
    /// </summary>
    public interface IMDMRule : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the rule type
        /// </summary>
        MDMRuleType RuleType { get; set; }

        /// <summary>
        /// Indicates the rule description
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Indicates the rule definition
        /// </summary>
        String RuleDefinition { get; set; }

        /// <summary>
        /// Indicates the ids of mapping to which rule are mapped
        /// </summary>
        Collection<Int32> RuleMapIds { get; set; }

        /// <summary>
        /// Indicates the names of mapping to which rule are mapped
        /// </summary>
        Collection<String> RuleMapNames { get; set; }

        /// <summary>
        /// Indicates the display type
        /// </summary>
        DisplayType DisplayType { get; set; }

        /// <summary>
        /// Indicates whether rule is a system rule
        /// </summary>
        Boolean IsSystemRule { get; set; }

        /// <summary>
        /// Indicates whether rule is enabled
        /// </summary>
        Boolean IsEnabled { get; set; }

        /// <summary>
        /// Indicates whether BusinessRule/BusinessCondition is published or draft
        /// </summary>
        RuleStatus RuleStatus { get; set; }

        /// <summary>
        /// Indicates the id of publish version of BusinessRule/BusinessCondition
        /// </summary>
        Int32 PublishedVersionId { get; set; }

        /// <summary>
        /// Indicates denotes the target attribute name
        /// </summary>
        String TargetAttributeName { get; set; }

        /// <summary>
        /// Indicates denotes the target locale
        /// </summary>
        LocaleEnum TargetLocale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 Sequence { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of MDMRule Object
        /// </summary>
        /// <returns>BusinessConditionRules</returns>
        String ToXml();

        /// <summary>
        /// Sets the rule context to MDMRule
        /// </summary>
        /// <param name="ruleContext">Indicates the Rule context</param>
        void SetRuleContext(IMDMRuleContext ruleContext);

        /// <summary>
        /// Returns list of Attributes or RelationshipTypes
        /// </summary>
        /// <returns>List of Attributes or RelationshipTypes</returns>
        IMDMRuleDisplayList GetDisplayList();

        /// <summary>
        /// Sets the list of Attributes or RelationshipTypes to be displayed
        /// </summary>
        /// <param name="displayList">Indicates the list of Attributes or RelationshipTypes to be displayed</param>
        void SetDisplayList(IMDMRuleDisplayList displayList);

        /// <summary>
        /// Gets the business condition rules
        /// </summary>
        /// <returns>BusinessConditionRules</returns>
        IMDMRuleCollection GetBusinessConditionRules();

        /// <summary>
        /// Get all BusinessConditionRule Ids based on Rule status
        /// </summary>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>List of BusinessConditionRule Ids</returns>
        ICollection<Int32> GetBusinessConditionRuleIds(RuleStatus ruleStatus);

        /// <summary>
        /// Gets all BusinessConditionRule Names
        /// </summary>
        /// <returns>List of BusinessConditionRule Names</returns>
        /// <param name="ruleStatus">Indicates the ruleStatus</param>
        ICollection<String> GetBusinessConditionRuleNames(RuleStatus ruleStatus);

        /// <summary>
        /// Set the Business ConditionRules in a BusinessCondition
        /// </summary>
        /// <param name="businessConditionRuleNames">Indicates the Business Rules mapped to BusinessCondition</param>
        /// <param name="ruleStatus">Indicates the RuleStatus</param>
        void SetBusinessConditionRules(ICollection<String> businessConditionRuleNames, RuleStatus ruleStatus);

        /// <summary>
        /// Add business condition rule name into MDMRule
        /// </summary>
        /// <param name="businessConditionRuleId">Indicates the business condition rule id</param>
        /// <param name="businessConditionRuleName">Indicates the business condition rule name</param>
        /// <param name="ruleStatus">Indicates the ruleStatus</param>
        /// <param name="sequence">Indicates the sequence in which the business condition rule has to be executed</param>
        void SetBusinessConditionRule(Int32 businessConditionRuleId, String businessConditionRuleName, RuleStatus ruleStatus, Int32 sequence = Constants.DDG_DEFAULT_SORTORDER);

        /// <summary>
        /// Indicates the last Modified AuditInfo
        /// </summary>
        /// <returns>Returns the last modified audit information</returns>
        IAuditInfo GetLastModifiedAuditInfo();

        /// <summary>
        /// Indicates the last Published AuditInfo
        /// </summary>
        /// <returns>Returns the last published audit information</returns>
        IAuditInfo GetPublishAuditInfo();

        #endregion Methods
    }
}
