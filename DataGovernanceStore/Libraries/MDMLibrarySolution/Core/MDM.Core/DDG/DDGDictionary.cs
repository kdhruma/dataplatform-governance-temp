using System;
using System.Collections.Generic;

namespace MDM.BusinessRuleManagement.Business
{
    using MDM.Core;

    /// <summary>
    /// Class specifying the DDG import/export dictionary
    /// </summary>
    public sealed class DDGDictionary
    {
        #region Public Dictionaries

        /// <summary>
        /// Indicates the mapping of DDG object type name in the DDG template with the respective DDG object type name available in the system
        /// </summary>
        public static Dictionary<ObjectType, String> ObjectsDictionary = new Dictionary<ObjectType, String>();

        /// <summary>
        /// Indicates the mapping of sheet name in the DDG template with the respective DDG object type name available in the system
        /// </summary>
        public static Dictionary<String, ObjectType> SheetNameToObjectTypeMap = new Dictionary<String, ObjectType>();

        /// <summary>
        /// Indicates the mapping of DDG metadata name with the respective metadata name in the DDG template
        /// </summary>
        public static Dictionary<Metadata, String> MetadataDictionary = new Dictionary<Metadata, String>();

        /// <summary>
        /// Stores config values from meta data sheet
        /// </summary>
        public static Dictionary<ObjectType, Boolean> MetadataItemsDictionary = new Dictionary<ObjectType, Boolean>();

        /// <summary>
        /// Indicates the mapping of DDG configuration name with the respective configuration name in the DDG template
        /// </summary>
        public static Dictionary<Configuration, String> ConfigurationDictionary = new Dictionary<Configuration, String>();

        /// <summary>
        /// Indicates the mapping of DDG configuration item name with the respective configuration item name in the DDG template
        /// </summary>
        public static Dictionary<String, ConfigurationItem> ConfigurationItemMapDictionary = new Dictionary<String, ConfigurationItem>();

        /// <summary>
        /// Stores config values from configuration data sheet
        /// </summary>
        public static Dictionary<ConfigurationItem, String> ConfigurationItemsDictionary = new Dictionary<ConfigurationItem, String>();

        /// <summary>
        /// Indicates the mapping of common headers in the DDG template with the respective common headers available in the system
        /// </summary>
        public static Dictionary<ExportImportCommonHeaders, String> ExportImportCommonHeadersDictionary = new Dictionary<ExportImportCommonHeaders, String>();

        /// <summary>
        /// Indicates the mapping of columns in Business Rules sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<BusinessRuleEnum, String> BusinessRuleDictionary = new Dictionary<BusinessRuleEnum, String>();

        /// <summary>
        /// Indicates the mapping of columns in Business Conditions sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<BusinessConditionEnum, String> BusinessConditionsDictionary = new Dictionary<BusinessConditionEnum, String>();

        /// <summary>
        /// Indicates the mapping of columns in Business Condition Sorting sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<BusinessConditionSortingEnum, String> BusinessConditionSortingDictionary = new Dictionary<BusinessConditionSortingEnum, String>();

        /// <summary>
        /// Indicates the mapping of columns in Dynamic Governance sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<DynamicGovernance, String> DynamicGovernanceDictionary = new Dictionary<DynamicGovernance, String>();

        /// <summary>
        /// Indicates the mapping of columns in Dynamic Governance Sorting sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<DynamicGovernanceSortingEnum, String> DynamicGovernanceSortingDictionary = new Dictionary<DynamicGovernanceSortingEnum, String>();

        /// <summary>
        /// Indicates the mapping of columns in System Message sheet with the respective column name in the DDG template
        /// </summary>
        public static Dictionary<DDGLocaleMessageEnum, String> DDGLocaleMessageDictionary = new Dictionary<DDGLocaleMessageEnum, String>();

        #endregion Public Dictionaries

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        static DDGDictionary()
        {
            BuildObjectTypeDictionary();
            BuildMetadataDictionary();

            BuildSheetNameToObjectTypeMap();

            BuildConfigurationDictionary();
            BuildConfigurationItemsMap();

            BuildExportImportCommonHeadersDictionary();

            BuildBusinessRulesDictionary();
            BuildBusinessConditionsDictionary();
            BuildBusinessConditionSortingDictionary();
            BuildDynamicGovernanceDictionary();
            BuildDynamicGovernanceSortingDictionary();
            BuildDDGLocaleMessageDictionary();
        }

        #endregion Constructor

        #region Methods

        #region Private Methods

        /// <summary>
        /// Builds the DDG objects dictionary
        /// </summary>
        private static void BuildObjectTypeDictionary()
        {
            ObjectsDictionary.Add(ObjectType.Metadata, "Metadata");
            ObjectsDictionary.Add(ObjectType.Configuration, "Configuration");
            ObjectsDictionary.Add(ObjectType.BusinessRules, "S1 - Business Rules");
            ObjectsDictionary.Add(ObjectType.BusinessConditions, "S2 - Business Conditions");
            ObjectsDictionary.Add(ObjectType.BusinessConditionSorting, "S2.1 Business Condition Sorting");
            ObjectsDictionary.Add(ObjectType.DynamicGovernance, "S3 - Dynamic Governance");
            ObjectsDictionary.Add(ObjectType.DynamicGovernanceSorting, "S3.1 Dynamic Governance Sorting");
            ObjectsDictionary.Add(ObjectType.SystemMessages, "S4 - System Messages");
        }

        /// <summary>
        /// Builds the DDG metadata dictionary
        /// </summary>
        private static void BuildMetadataDictionary()
        {
            MetadataDictionary.Add(Metadata.SheetNo, "Sheet No");
            MetadataDictionary.Add(Metadata.PhysicalSheetName, "Physical Sheet Name");
            MetadataDictionary.Add(Metadata.Description, "Description");
            MetadataDictionary.Add(Metadata.Processdatasheet, "Process data sheet?");
        }

        /// <summary>
        /// Builds the DDG sheet name to object type map dictionary
        /// </summary>
        private static void BuildSheetNameToObjectTypeMap()
        {
            SheetNameToObjectTypeMap.Add("S1 - Business Rules", ObjectType.BusinessRules);
            SheetNameToObjectTypeMap.Add("S2 - Business Conditions", ObjectType.BusinessConditions);
            SheetNameToObjectTypeMap.Add("S2.1 Business Condition Sorting", ObjectType.BusinessConditionSorting);
            SheetNameToObjectTypeMap.Add("S3 - Dynamic Governance", ObjectType.DynamicGovernance);
            SheetNameToObjectTypeMap.Add("S3.1 Dynamic Governance Sorting", ObjectType.DynamicGovernanceSorting);
            SheetNameToObjectTypeMap.Add("S4 - System Messages", ObjectType.SystemMessages);
        }

        /// <summary>
        /// Builds the DDG configuration dictionary
        /// </summary>
        private static void BuildConfigurationDictionary()
        {
            ConfigurationDictionary.Add(Configuration.ConfigurationKey, "Configuration Key");
            ConfigurationDictionary.Add(Configuration.ConfigurationValue, "Configuration Value");
        }

        /// <summary>
        /// Builds the DDG configuration Item Map dictionary
        /// </summary>
        private static void BuildConfigurationItemsMap()
        {
            ConfigurationItemMapDictionary.Add("Template Version", ConfigurationItem.TemplateVersion);
            ConfigurationItemMapDictionary.Add("Default Separator", ConfigurationItem.DefaultSeperator);
        }

        /// <summary>
        /// Builds the DDG export common headers dictionary
        /// </summary>
        private static void BuildExportImportCommonHeadersDictionary()
        {
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.Id, "ID");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.Action, "Action");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.Name, "Name");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.LastModifiedDateTime, "Last Modified Time");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.LastModifiedUser, "Last Modified User");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.LastPublishedDateTime, "Last Published Time");
            ExportImportCommonHeadersDictionary.Add(ExportImportCommonHeaders.LastPublishedUser, "Last Published User");
        }

        /// <summary>
        /// Builds the Business Rules dictionary
        /// </summary>
        private static void BuildBusinessRulesDictionary()
        {
            BusinessRuleDictionary.Add(BusinessRuleEnum.RuleType, "Type");
            BusinessRuleDictionary.Add(BusinessRuleEnum.IsEnabled, "IsEnabled");
            BusinessRuleDictionary.Add(BusinessRuleEnum.Status, "Status");
            BusinessRuleDictionary.Add(BusinessRuleEnum.Description, "Description");
            BusinessRuleDictionary.Add(BusinessRuleEnum.RuleDefinition, "Definition");
            BusinessRuleDictionary.Add(BusinessRuleEnum.MappedContexts, "Mapped Context(s)");
            BusinessRuleDictionary.Add(BusinessRuleEnum.DisplayType, "Display Type");
            BusinessRuleDictionary.Add(BusinessRuleEnum.DisplayList, "Display List");
            BusinessRuleDictionary.Add(BusinessRuleEnum.TargetAttributeName, "Target Attribute Name");
            BusinessRuleDictionary.Add(BusinessRuleEnum.TargetLocale, "Target Locale");
        }

        /// <summary>
        /// Builds the Business Conditions dictionary
        /// </summary>
        private static void BuildBusinessConditionsDictionary()
        {
            BusinessConditionsDictionary.Add(BusinessConditionEnum.IsEnabled, "IsEnabled");
            BusinessConditionsDictionary.Add(BusinessConditionEnum.Status, "Status");
            BusinessConditionsDictionary.Add(BusinessConditionEnum.Description, "Description");
            BusinessConditionsDictionary.Add(BusinessConditionEnum.PublishedBusinessConditionRulesNames, "Published Rule(s)");
            BusinessConditionsDictionary.Add(BusinessConditionEnum.DraftBusinessConditionRulesNames, "Draft Rule(s)");
            BusinessConditionsDictionary.Add(BusinessConditionEnum.MappedContexts, "Mapped Context(s)");
        }

        /// <summary>
        /// Builds the Business Condition Sorting dictionary
        /// </summary>
        private static void BuildBusinessConditionSortingDictionary()
        {
            BusinessConditionSortingDictionary.Add(BusinessConditionSortingEnum.BusinessConditionName, "Business Condition Name");
            BusinessConditionSortingDictionary.Add(BusinessConditionSortingEnum.BusinessConditionStatus, "Business Condition Status");
            BusinessConditionSortingDictionary.Add(BusinessConditionSortingEnum.BusinessRuleName, "Business Rule Name");
            BusinessConditionSortingDictionary.Add(BusinessConditionSortingEnum.BusinessRuleStatus, "Business Rule Status");
            BusinessConditionSortingDictionary.Add(BusinessConditionSortingEnum.Sequence, "Sort Order");
        }

        /// <summary>
        /// Builds the Dynamic Governance dictionary
        /// </summary>
        private static void BuildDynamicGovernanceDictionary()
        {
            DynamicGovernanceDictionary.Add(DynamicGovernance.IsEnabled, "IsEnabled");

            DynamicGovernanceDictionary.Add(DynamicGovernance.Organization, "Organization");
            DynamicGovernanceDictionary.Add(DynamicGovernance.Container, "Container");
            DynamicGovernanceDictionary.Add(DynamicGovernance.Category, "Category Path");
            DynamicGovernanceDictionary.Add(DynamicGovernance.EntityType, "Entity Type");
            DynamicGovernanceDictionary.Add(DynamicGovernance.RelationshipType, "Relationship Type");
            DynamicGovernanceDictionary.Add(DynamicGovernance.SecurityRole, "Role");

            DynamicGovernanceDictionary.Add(DynamicGovernance.EventSource, "Event Source");
            DynamicGovernanceDictionary.Add(DynamicGovernance.EventName, "Event Name");

            DynamicGovernanceDictionary.Add(DynamicGovernance.WorkflowName, "Workflow Name");
            DynamicGovernanceDictionary.Add(DynamicGovernance.WorkflowActivityName, "Workflow Activity");
            DynamicGovernanceDictionary.Add(DynamicGovernance.WorkflowAction, "Workflow Action");

            DynamicGovernanceDictionary.Add(DynamicGovernance.IsAsync, "IsAsync");

            DynamicGovernanceDictionary.Add(DynamicGovernance.PublishedBusinessRules, "Published Rule(s)");
            DynamicGovernanceDictionary.Add(DynamicGovernance.PublishedBusinessConditions, "Published Business Condition(s)");
            DynamicGovernanceDictionary.Add(DynamicGovernance.DraftBusinessRules, "Draft Rule(s)");
            DynamicGovernanceDictionary.Add(DynamicGovernance.DraftBusinessConditions, "Draft Business Condition(s)");
        }

        /// <summary>
        /// Builds the Dynamic Governance Sorting dictionary
        /// </summary>
        private static void BuildDynamicGovernanceSortingDictionary()
        {
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.Name, "Name");
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.MDMRule, "Business Rule / Business Condition");
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.RuleType, "Type");
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.Status, "Status");
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.IgnoreChangeContext, "IgnoreChangeContext");
            DynamicGovernanceSortingDictionary.Add(DynamicGovernanceSortingEnum.Sequence, "Sort Order");
        }

        /// <summary>
        /// Builds the DDG Locale message dictionary
        /// </summary>
        private static void BuildDDGLocaleMessageDictionary()
        {
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.MessageCode, "Message Code");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.MessageType, "Message Type");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.Message, "Message");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.Description, "Description");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.Locale, "Locale");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.WhereUsed, "Where Used");
            DDGLocaleMessageDictionary.Add(DDGLocaleMessageEnum.HelpfulLinks, "Helpful Links");
        }

        #endregion Private Methods

        #endregion Methods
    }
}
