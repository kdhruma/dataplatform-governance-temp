using System;
using System.Runtime.Serialization;

namespace MDM.BusinessRuleManagement.Business
{
    /// <summary>
    /// Represents the DDG metadata 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum Metadata
    {
        /// <summary>
        /// Represents the sheet number
        /// </summary>
        [EnumMember]
        SheetNo,

        /// <summary>
        /// Represents the physical sheet name
        /// </summary>
        [EnumMember]
        PhysicalSheetName,

        /// <summary>
        /// Represents the description
        /// </summary>
        [EnumMember]
        Description,

        /// <summary>
        /// Represents the process data sheet
        /// </summary>
        [EnumMember]
        Processdatasheet
    }

    /// <summary>
    /// Represents the DDG configuration
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum Configuration
    {
        /// <summary>
        /// Represents configuration key
        /// </summary>
        [EnumMember]
        ConfigurationKey = 0,

        /// <summary>
        /// Represents configuration value
        /// </summary>
        [EnumMember]
        ConfigurationValue = 1
    }

    /// <summary>
    /// Represents the DDG configuration items
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ConfigurationItem
    {
        /// <summary>
        /// Represents default version number
        /// </summary>
        [EnumMember]
        TemplateVersion = 0,

        /// <summary>
        /// Represents default seperator
        /// </summary>
        [EnumMember]
        DefaultSeperator = 1
    }

    /// <summary>
    /// Specifies the DDG Export/Import Comman Headers
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportImportCommonHeaders
    {
        /// <summary>
        /// Represents the ID of DDG Object
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// Represents the name of DDG Object
        /// </summary>
        [EnumMember]
        Name,

        /// <summary>
        /// Represents the Action
        /// </summary>
        [EnumMember]
        Action,

        /// <summary>
        /// Represents the Last Modified Time
        /// </summary>
        [EnumMember]
        LastModifiedDateTime,

        /// <summary>
        /// Represents the Last Modified User
        /// </summary>
        [EnumMember]
        LastModifiedUser,

        /// <summary>
        /// Represents the Last Published Time
        /// </summary>
        [EnumMember]
        LastPublishedDateTime,

        /// <summary>
        /// Represents the Last Published User
        /// </summary>
        [EnumMember]
        LastPublishedUser
    }

    /// <summary>
    /// Represents Business Rules
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum BusinessRuleEnum
    {
        /// <summary>
        /// Represents Rule Type
        /// </summary>
        [EnumMember]
        RuleType,

        /// <summary>
        /// Represents whether the BusinessRule is Enabled or Disabled
        /// </summary>
        [EnumMember]
        IsEnabled,

        /// <summary>
        /// Represents whether the BusinessRule is Published or Draft
        /// </summary>
        [EnumMember]
        Status,

        /// <summary>
        /// Represents the BusinessRule Description
        /// </summary>
        [EnumMember]
        Description,

        /// <summary>
        /// Represents the BusinessRule Definition
        /// </summary>
        [EnumMember]
        RuleDefinition,

        /// <summary>
        /// Represents the MappedContexts
        /// </summary>
        [EnumMember]
        MappedContexts,

        /// <summary>
        /// Represents the DisplayType for BusinessRules
        /// </summary>
        [EnumMember]
        DisplayType,

        /// <summary>
        /// Represents the DisplayList for BusinessRules
        /// </summary>
        [EnumMember]
        DisplayList,

        /// <summary>
        /// Represents the IsAsync
        /// </summary>
        [EnumMember]
        IsAsync,

        /// <summary>
        /// Represents the Target Attribute name
        /// </summary>
        [EnumMember]
        TargetAttributeName,

        /// <summary>
        /// Represents the Target locale
        /// </summary>
        [EnumMember]
        TargetLocale
    };

    /// <summary>
    /// Represents Business Conditions
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum BusinessConditionEnum
    {
        /// <summary>
        /// Represents whether the BusinessCondition is Enabled or Disabled
        /// </summary>
        [EnumMember]
        IsEnabled,

        /// <summary>
        /// Represents whether the BusinessCondition is Published or Draft
        /// </summary>
        [EnumMember]
        Status,

        /// <summary>
        /// Represents the BusinessCondition Description
        /// </summary>
        [EnumMember]
        Description,

        /// <summary>
        /// Represents Published BusinessCondition RuleNames
        /// </summary>
        [EnumMember]
        PublishedBusinessConditionRulesNames,

        /// <summary>
        /// Represents Draft BusinessCondition RuleNames
        /// </summary>
        [EnumMember]
        DraftBusinessConditionRulesNames,

        /// <summary>
        /// Represents the MappedContexts
        /// </summary>
        [EnumMember]
        MappedContexts,
    };

    /// <summary>
    /// Represents Business Condition Sorting
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum BusinessConditionSortingEnum
    {
        /// <summary>
        /// Represents the Business Condition Name
        /// </summary>
        [EnumMember]
        BusinessConditionName,

        /// <summary>
        /// Represents the Business Condition Status
        /// </summary>
        [EnumMember]
        BusinessConditionStatus,

        /// <summary>
        /// Represents he Business Rule Name
        /// </summary>
        [EnumMember]
        BusinessRuleName,

        /// <summary>
        /// Represents the Business Rule Status
        /// </summary>
        [EnumMember]
        BusinessRuleStatus,

        /// <summary>
        /// Represents the Sequence in which Business Rule Mapped to the Business Condition should be executed
        /// </summary>
        [EnumMember]
        Sequence
    };

    /// <summary>
    /// Represents DynamicGovernance
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DynamicGovernance
    {
        /// <summary>
        /// Represents whether the Rule is Enabled or Disabled
        /// </summary>
        [EnumMember]
        IsEnabled,

        /// <summary>
        /// Represents the Organization
        /// </summary>
        [EnumMember]
        Organization,

        /// <summary>
        /// Represents the Container
        /// </summary>
        [EnumMember]
        Container,

        /// <summary>
        /// Represents the Category
        /// </summary>
        [EnumMember]
        Category,

        /// <summary>
        /// Represents the Entity Type
        /// </summary>
        [EnumMember]
        EntityType,

        /// <summary>
        /// Represents the Relationship Type
        /// </summary>
        [EnumMember]
        RelationshipType,

        /// <summary>
        /// Represents the Security Role
        /// </summary>
        [EnumMember]
        SecurityRole,

        /// <summary>
        /// Represents the origin of Event
        /// </summary>
        [EnumMember]
        EventSource,

        /// <summary>
        /// Represents the Event Name
        /// </summary>
        [EnumMember]
        EventName,

        /// <summary>
        /// Represents the Workflow Name
        /// </summary>
        [EnumMember]
        WorkflowName,

        /// <summary>
        /// Represents the Workflow Activity Name
        /// </summary>
        [EnumMember]
        WorkflowActivityName,

        /// <summary>
        /// Represents the Workflow Activity Action
        /// </summary>
        [EnumMember]
        WorkflowAction,

        /// <summary>
        /// Represents the Published BusinessRules
        /// </summary>
        [EnumMember]
        PublishedBusinessRules,

        /// <summary>
        /// Represents the Published BusinessConditions
        /// </summary>
        [EnumMember]
        PublishedBusinessConditions,

        /// <summary>
        /// Represents the Draft BusinessRules
        /// </summary>
        [EnumMember]
        DraftBusinessRules,

        /// <summary>
        /// Represents the Draft BusinessConditions
        /// </summary>
        [EnumMember]
        DraftBusinessConditions,

        /// <summary>
        /// Represents the rule will be executed Sync or Async
        /// </summary>
        [EnumMember]
        IsAsync,
    }

    /// <summary>
    /// Represents Dynamic Governance Sorting
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DynamicGovernanceSortingEnum
    {
        /// <summary>
        /// Represents the name of Dynamic Governance
        /// </summary>
        [EnumMember]
        Name,

        /// <summary>
        /// Represents the Business Rule / Business Condition
        /// </summary>
        [EnumMember]
        MDMRule,

        /// <summary>
        /// Represents the Rule Type
        /// </summary>
        [EnumMember]
        RuleType,

        /// <summary>
        /// Represents the Rule Status
        /// </summary>
        [EnumMember]
        Status,

        /// <summary>
        /// Represents the Sort Order
        /// </summary>
        [EnumMember]
        Sequence,

        /// <summary>
        /// Represents the Ignore Change Context
        /// </summary>
        [EnumMember]
        IgnoreChangeContext
    }

    /// <summary>
    /// Represents enumeration for DDG locale messages
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DDGLocaleMessageEnum
    {
        /// <summary>
        /// Represents code of System Message
        /// </summary>
        [EnumMember]
        MessageCode,

        /// <summary>
        /// Represents type of System Message
        /// </summary>
        [EnumMember]
        MessageType,

        /// <summary>
        /// Represents the message
        /// </summary>
        [EnumMember]
        Message,

        /// <summary>
        /// Represents the description about the message
        /// </summary>
        [EnumMember]
        Description,

        /// <summary>
        /// Represents the locale
        /// </summary>
        [EnumMember]
        Locale,

        /// <summary>
        /// Represents the place where message is used
        /// </summary>
        [EnumMember]
        WhereUsed,

        /// <summary>
        /// Represents the helpful links about the message
        /// </summary>
        [EnumMember]
        HelpfulLinks
    };

}
