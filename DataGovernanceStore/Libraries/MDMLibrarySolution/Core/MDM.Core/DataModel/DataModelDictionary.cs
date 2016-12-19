using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Core.DataModel
{
    /// <summary>
    /// Specifies the data model dictionary
    /// </summary>
    public sealed class DataModelDictionary
    {
        #region Public Dictionaries

        /// <summary>
        /// Indicates the mapping of object type name in the data model template with the respective object type name
        /// </summary>
        public static Dictionary<String, ObjectType> ConfigToObjectTypeMap = new Dictionary<string, ObjectType>();

        /// <summary>
        /// Indicates the mapping of object type name with the respective object type name in the data model template
        /// </summary>
        public static Dictionary<ObjectType, String> ObjectsDictionary = new Dictionary<ObjectType, String>();

        /// <summary>
        /// Indicates the mapping of data model metadata name with the respective metadata name in the data model template
        /// </summary>
        public static Dictionary<DataModelMetadata, String> MetadataDictionary = new Dictionary<DataModelMetadata, String>();

        /// <summary>
        /// Indicates the mapping of data model configuration name with the respective configuration name in the data model template
        /// </summary>
        public static Dictionary<DataModelConfiguration, String> ConfigurationDictionary = new Dictionary<DataModelConfiguration, String>();

        /// <summary>
        /// Indicates the mapping of data model configuration name with the respective configuration name in the data model template
        /// </summary>
        public static Dictionary<String, DataModelConfigurationItem> ConfigurationItemsMap = new Dictionary<String, DataModelConfigurationItem>();

        /// <summary>
        /// Indicates the mapping of data model common header name with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelCommonHeaders, String> CommonHeadersDictionary = new Dictionary<DataModelCommonHeaders, String>();

        /// <summary>
        /// Indicates the mapping of data model organization column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelOrganization, String> OrganizationDictionary = new Dictionary<DataModelOrganization, String>();

        /// <summary>
        /// Indicates the mapping of data model container localization column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerLocalization, String> ContainerLocalizationDictionary = new Dictionary<DataModelContainerLocalization, String>();

        /// <summary>
        /// Indicates the mapping of data model hierarchy column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelHierarchy, String> HierarchyDictionary = new Dictionary<DataModelHierarchy, String>();

        /// <summary>
        /// Indicates the mapping of data model container column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainer, String> ContainerDictionary = new Dictionary<DataModelContainer, String>();

        /// <summary>
        /// Indicates the mapping of data model entity type column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelEntityType, String> EntityTypeDictionary = new Dictionary<DataModelEntityType, String>();

        /// <summary>
        /// Indicates the mapping of data model relationship column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelRelationshipType, String> RelationshipTypeDictionary = new Dictionary<DataModelRelationshipType, String>();

        /// <summary>
        /// Indicates the mapping of data model attribute model column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelAttributeModel, String> AttributeModelDictionary = new Dictionary<DataModelAttributeModel, String>();

        /// <summary>
        /// Indicates the mapping of data model attribute model localization column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelAttributeModelLocalization, String> AttributeModelLocalizationDictionary = new Dictionary<DataModelAttributeModelLocalization, String>();

        /// <summary>
        /// Indicates the mapping of data model category column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelCategory, String> CategoryDictionary = new Dictionary<DataModelCategory, String>();

        /// <summary>
        /// Indicates the mapping of data model category localization column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelCategoryLocalization, String> CategoryLocalizationDictionary = new Dictionary<DataModelCategoryLocalization, String>();
       
        /// <summary>
        /// Indicates the mapping of data model container entity type mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerEntityTypeMapping, String> ContainerEntityTypeMappingDictionary = new Dictionary<DataModelContainerEntityTypeMapping, String>();

        /// <summary>
        /// Indicates the mapping of entity type attribute mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelEntityTypeAttributeMapping, String> EntityTypeAttributeMappingDictionary = new Dictionary<DataModelEntityTypeAttributeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model container entity type attribute mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerEntityTypeAttributeMapping, String> ContainerEntityTypeAttributeMappingDictionary = new Dictionary<DataModelContainerEntityTypeAttributeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model category attribute mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelCategoryAttributeMapping, String> CategoryAttributeMappingDictionary = new Dictionary<DataModelCategoryAttributeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model relationship type entity type mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelRelationshipTypeEntityTypeMapping, String> RelationshipTypeEntityTypeMappingDictionary = new Dictionary<DataModelRelationshipTypeEntityTypeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model relationship type entity type mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelRelationshipTypeEntityTypeMappingCardinality, String> RelationshipTypeEntityTypeMappingCardinalityDictionary = new Dictionary<DataModelRelationshipTypeEntityTypeMappingCardinality, String>();

        /// <summary>
        /// Indicates the mapping of data model container relationship type entity type mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerRelationshipTypeEntityTypeMapping, String> ContainerRelationshipTypeEntityTypeMappingDictionary = new Dictionary<DataModelContainerRelationshipTypeEntityTypeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model container relationship type entity type mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerRelationshipTypeEntityTypeMappingCardinality, String> ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary = new Dictionary<DataModelContainerRelationshipTypeEntityTypeMappingCardinality, String>();

        /// <summary>
        /// Indicates the mapping of data model relationship type attribute mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelRelationshipTypeAttributeMapping, String> RelationshipTypeAttributeMappingDictionary = new Dictionary<DataModelRelationshipTypeAttributeMapping, String>();

        /// <summary>
        /// Indicates the mapping of data model container relationship type attribute mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelContainerRelationshipTypeAttributeMapping, String> ContainerRelationshipTypeAttributeMappingDictionary = new Dictionary<DataModelContainerRelationshipTypeAttributeMapping, String>();
        
        /// <summary>
        /// Indicates the mapping of data model security role column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelSecurityRole, String> SecurityRoleDictionary = new Dictionary<DataModelSecurityRole, String>();

        /// <summary>
        /// Indicates the mapping of data model security user column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelSecurityUser, String> SecurityUserDictionary = new Dictionary<DataModelSecurityUser, String>();

        /// <summary>
        /// Indicates the mapping of data model lookup model column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelLookupModel, String> LookupModelDictionary = new Dictionary<DataModelLookupModel, String>();

        /// <summary>
        /// Indicates the mapping of data model entity hierarchy configuration column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelEntityVariantDefinition, String> EntityVariantDefinitionDictionary = new Dictionary<DataModelEntityVariantDefinition, String>();

        /// <summary>
        /// Indicates the mapping of data model word list column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelImportWordList, String> WordListModelDictionary = new Dictionary<DataModelImportWordList, String>();

        /// <summary>
        /// Indicates the mapping of data model word element column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelWordElement, String> WordElementModelDictionary = new Dictionary<DataModelWordElement, String>();

        /// <summary>
        /// Indicates the mapping of data model entity variant definition mapping column with the respective column name in the data model template
        /// </summary>
        public static Dictionary<DataModelEntityVariantDefinitionMapping, String> EntityVariantDefinitionMappingDictionary = new Dictionary<DataModelEntityVariantDefinitionMapping, String>();

        #endregion

        #region Constructor

        /// <summary>
        /// Specifies the data model dictionary constructor
        /// </summary>
        static DataModelDictionary()
        {
            BuildConfigToObjectTypeMap();
            BuildObjectsDictionary();
            BuildMetadataDictionary();
            BuildConfigurationDictionary();
            BuildConfigurationItemsMap();
            BuildCommonHeadersDictionary();
            BuildOrganizationDictionary();
            BuildHierarchyDictionary();
            BuildContainerDictionary();
            BuildContainerLocalizationDictionary();
            BuildEntityTypeDictionary();
            BuildRelationshipTypeDictionary();
            BuildAttributeModelDictionary();
            BuildAttributeModelLocalizationDictionary();
            BuildCategoryDictionary();
            BuildCategoryLocalizationDictionary();

            BuildContainerEntityTypeMappingDictionary();
            BuildEntityTypeAttributeMappingDictionary();
            BuildContainerEntityTypeAttributeMappingDictionary();
            BuildCategoryAttributeMappingDictionary();
            BuildRelationshipTypeEntityTypeMappingDictionary();
            BuildRelationshipTypeEntityTypeMappingCardinalityDictionary();
            BuildContainerRelationshipTypeEntityTypeMappingDictionary();
            BuildContainerRelationshipTypeEntityTypeMappingCardinalityDictionary();
            BuildRelationshipTypeAttributeMappingDictionary();
            BuildContainerRelationshipTypeAttributeMappingDictionary();

            BuildSecurityRoleDictionary();
            BuildSecurityUserDictionary();
            BuildLookupModelDictionary();

            BuildWordListDictionary();
            BuildWordElementDictionary();

            BuildEntityVariantDefinitionDictionary();
            BuildEntityVariantDefinitionMappingDictionary();

        }

        #endregion

        #region Methods

        /// <summary>
        /// Build config to ObjectType map
        /// </summary>
        private static void BuildConfigToObjectTypeMap()
        {
            ConfigToObjectTypeMap.Add("S1 - Organization", ObjectType.Organization);
            ConfigToObjectTypeMap.Add("S2 - Hierarchy", ObjectType.Taxonomy);
            ConfigToObjectTypeMap.Add("S3 - Container", ObjectType.Catalog);
            ConfigToObjectTypeMap.Add("S4 - Container - Locale", ObjectType.ContainerLocalization);
            ConfigToObjectTypeMap.Add("S5 - Entity Type", ObjectType.EntityType);
            ConfigToObjectTypeMap.Add("S6 - Relationship Type", ObjectType.RelationshipType);
            ConfigToObjectTypeMap.Add("S7 - Attribute", ObjectType.AttributeModel);
            ConfigToObjectTypeMap.Add("S8 - Attribute - Locale", ObjectType.AttributeModelLocalization);
            ConfigToObjectTypeMap.Add("S9 - Category", ObjectType.Category);
            ConfigToObjectTypeMap.Add("S10 - Category - Locale", ObjectType.CategoryLocalization);

            ConfigToObjectTypeMap.Add("S11 - CON - ET", ObjectType.ContainerEntityTypeMapping);
            ConfigToObjectTypeMap.Add("S12 - ET - ATTR", ObjectType.EntityTypeAttributeMapping);
            ConfigToObjectTypeMap.Add("S13 - CON - ET - ATTR", ObjectType.ContainerEntityTypeAttributeMapping);
            ConfigToObjectTypeMap.Add("S14 - CAT - ATTR", ObjectType.CategoryAttributeMapping);
            ConfigToObjectTypeMap.Add("S15 - RT - ET", ObjectType.RelationshipTypeEntityTypeMapping);
            ConfigToObjectTypeMap.Add("S16 - RT - ET - CARD", ObjectType.RelationshipTypeEntityTypeMappingCardinality);
            ConfigToObjectTypeMap.Add("S17 - CON - RT - ET", ObjectType.ContainerRelationshipTypeEntityTypeMapping);
            ConfigToObjectTypeMap.Add("S18 - CON - RT - ET - CARD", ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality);
            ConfigToObjectTypeMap.Add("S19 - RT - ATTR", ObjectType.RelationshipTypeAttributeMapping);
            ConfigToObjectTypeMap.Add("S20 - CON - RT - ATTR", ObjectType.ContainerRelationshipTypeAttributeMapping);
            ConfigToObjectTypeMap.Add("S21 - Security Role", ObjectType.Role);
            ConfigToObjectTypeMap.Add("S22 - Security User", ObjectType.User);
            ConfigToObjectTypeMap.Add("S23 - Lookup Model", ObjectType.LookupModel);
            ConfigToObjectTypeMap.Add("S24 - Word List", ObjectType.WordList);
            ConfigToObjectTypeMap.Add("S25 - Word Element", ObjectType.WordElement);
            ConfigToObjectTypeMap.Add("S26 - Entity Variant Definition", ObjectType.EntityVariantDefinition);
            ConfigToObjectTypeMap.Add("S27 - EVD Mapping", ObjectType.EntityVariantDefinitionMapping);
        }

        /// <summary>
        /// Build DataModel Object Dictionary
        /// </summary>
        private static void BuildObjectsDictionary()
        {
            ObjectsDictionary.Add(ObjectType.DataModelMetadata, "Metadata");
            ObjectsDictionary.Add(ObjectType.Configuration, "Configuration");
            ObjectsDictionary.Add(ObjectType.Organization, "S1 - Organization");
            ObjectsDictionary.Add(ObjectType.Taxonomy, "S2 - Hierarchy");
            ObjectsDictionary.Add(ObjectType.Catalog, "S3 - Container");
            ObjectsDictionary.Add(ObjectType.ContainerLocalization, "S4 - Container - Locale");
            ObjectsDictionary.Add(ObjectType.EntityType, "S5 - Entity Type");
            ObjectsDictionary.Add(ObjectType.RelationshipType, "S6 - Relationship Type");
            ObjectsDictionary.Add(ObjectType.AttributeModel, "S7 - Attribute");
            ObjectsDictionary.Add(ObjectType.AttributeModelLocalization, "S8 - Attribute - Locale");
            ObjectsDictionary.Add(ObjectType.Category, "S9 - Category");
            ObjectsDictionary.Add(ObjectType.CategoryLocalization, "S10 - Category - Locale");

            ObjectsDictionary.Add(ObjectType.ContainerEntityTypeMapping, "S11 - CON - ET");
            ObjectsDictionary.Add(ObjectType.EntityTypeAttributeMapping, "S12 - ET - ATTR");
            ObjectsDictionary.Add(ObjectType.ContainerEntityTypeAttributeMapping, "S13 - CON - ET - ATTR");
            ObjectsDictionary.Add(ObjectType.CategoryAttributeMapping, "S14 - CAT - ATTR");
            ObjectsDictionary.Add(ObjectType.RelationshipTypeEntityTypeMapping, "S15 - RT - ET");
            ObjectsDictionary.Add(ObjectType.RelationshipTypeEntityTypeMappingCardinality, "S16 - RT - ET - CARD");
            ObjectsDictionary.Add(ObjectType.ContainerRelationshipTypeEntityTypeMapping, "S17 - CON - RT - ET");
            ObjectsDictionary.Add(ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality, "S18 - CON - RT - ET - CARD");
            ObjectsDictionary.Add(ObjectType.RelationshipTypeAttributeMapping, "S19 - RT - ATTR");
            ObjectsDictionary.Add(ObjectType.ContainerRelationshipTypeAttributeMapping, "S20 - CON - RT - ATTR");

            ObjectsDictionary.Add(ObjectType.Role, "S21 - Security Role");
            ObjectsDictionary.Add(ObjectType.User, "S22 - Security User");
            ObjectsDictionary.Add(ObjectType.UserPreferences, "S22 - Security User");
            ObjectsDictionary.Add(ObjectType.LookupModel, "S23 - Lookup Model");
            ObjectsDictionary.Add(ObjectType.WordList, "S24 - Word List");
            ObjectsDictionary.Add(ObjectType.WordElement, "S25 - Word Element");

            ObjectsDictionary.Add(ObjectType.EntityVariantDefinition, "S26 - Entity Variant Definition");
            ObjectsDictionary.Add(ObjectType.EntityVariantDefinitionMapping, "S27 - EVD Mapping");
        }

        /// <summary>
        /// Build Metadata Dictionary
        /// </summary>
        private static void BuildMetadataDictionary()
        {
            MetadataDictionary.Add(DataModelMetadata.SheetNo, "Sheet No");
            MetadataDictionary.Add(DataModelMetadata.DataModelTypeName, "DataModel Type Name");
            MetadataDictionary.Add(DataModelMetadata.SheetName, "Physical Sheet Name");
            MetadataDictionary.Add(DataModelMetadata.Processdatasheet, "Process data sheet?");
        }

        /// <summary>
        /// Build Configuration Dictionary
        /// </summary>
        private static void BuildConfigurationDictionary()
        {
            ConfigurationDictionary.Add(DataModelConfiguration.ConfigurationKey, "Configuration Key");
            ConfigurationDictionary.Add(DataModelConfiguration.ConfigurationValue, "Configuration Value");
        }

        /// <summary>
        /// Build Configuration Items Map
        /// </summary>
        private static void BuildConfigurationItemsMap()
        {
            ConfigurationItemsMap.Add("Template Version", DataModelConfigurationItem.TemplateVersion);
            ConfigurationItemsMap.Add("Default Separator", DataModelConfigurationItem.DefaultSeparator);
            ConfigurationItemsMap.Add("Validate Lookup Dependencies", DataModelConfigurationItem.ValidateLookupDependencies);
            ConfigurationItemsMap.Add("Validate UOM Dependencies", DataModelConfigurationItem.ValidateUomDependencies);
        }

        /// <summary>
        /// Build CommonHeaders Dictionary
        /// </summary>
        private static void BuildCommonHeadersDictionary()
        {
            CommonHeadersDictionary.Add(DataModelCommonHeaders.Id, "ID");
            CommonHeadersDictionary.Add(DataModelCommonHeaders.Action, "Action");
            CommonHeadersDictionary.Add(DataModelCommonHeaders.UniqueIdentifier, "Unique Identifier");
        }

        /// <summary>
        /// Build Organization Dictionary
        /// </summary>
        private static void BuildOrganizationDictionary()
        {
            OrganizationDictionary.Add(DataModelOrganization.OrganizationName, "Organization Name");
            OrganizationDictionary.Add(DataModelOrganization.OrganizationLongName, "Organization Long Name");
            OrganizationDictionary.Add(DataModelOrganization.OrganizationParentName, "Organization Parent Name");
        }

        /// <summary>
        /// Build Hierarchy Dictionary
        /// </summary>
        private static void BuildHierarchyDictionary()
        {
            HierarchyDictionary.Add(DataModelHierarchy.HierarchyName, "Hierarchy Name");
            HierarchyDictionary.Add(DataModelHierarchy.HierarchyLongName, "Hierarchy Long Name");
            HierarchyDictionary.Add(DataModelHierarchy.LeafNodeOnly, "Leaf Node Only");
        }

        /// <summary>
        /// Build Containers Dictionary
        /// </summary>
        private static void BuildContainerDictionary()
        {
            ContainerDictionary.Add(DataModelContainer.ContainerName, "Container Name");
            ContainerDictionary.Add(DataModelContainer.ContainerLongName, "Container Long Name");
            ContainerDictionary.Add(DataModelContainer.OrganizationName, "Organization Name");
            ContainerDictionary.Add(DataModelContainer.HierarchyName, "Hierarchy Name");
            ContainerDictionary.Add(DataModelContainer.ContainerType, "Container Type");
            ContainerDictionary.Add(DataModelContainer.ContainerQualifier, "Container Qualifier");
            ContainerDictionary.Add(DataModelContainer.ContainerSecondaryQualifiers, "Container Secondary Qualifiers");
            ContainerDictionary.Add(DataModelContainer.ParentContainerName, "Parent Container Name");
            ContainerDictionary.Add(DataModelContainer.NeedsApprovedCopy, "Needs Approved Copy");
            ContainerDictionary.Add(DataModelContainer.WorkflowType, "Workflow Type");
            ContainerDictionary.Add(DataModelContainer.AutoExtensionEnabled, "Auto Extension Enabled?");
        }

        /// <summary>
        /// Build ContainerLocalization Dictionary
        /// </summary>
        private static void BuildContainerLocalizationDictionary()
        {
            ContainerLocalizationDictionary.Add(DataModelContainerLocalization.ContainerName, "Container Name");
            ContainerLocalizationDictionary.Add(DataModelContainerLocalization.Locale, "Locale");
        }
        
        /// <summary>
        /// Build EntityType Dictionary
        /// </summary>
        private static void BuildEntityTypeDictionary()
        {
            EntityTypeDictionary.Add(DataModelEntityType.EntityTypeName, "Entity Type Name");
            EntityTypeDictionary.Add(DataModelEntityType.EntityTypeLongName, "Entity Type Long Name");
        }

        /// <summary>
        /// Build RelationshipType Dictionary
        /// </summary>
        private static void BuildRelationshipTypeDictionary()
        {
            RelationshipTypeDictionary.Add(DataModelRelationshipType.RelationshipTypeName, "Relationship Type Name");
            RelationshipTypeDictionary.Add(DataModelRelationshipType.RelationshipTypeLongName, "Relationship Type Long Name");
            RelationshipTypeDictionary.Add(DataModelRelationshipType.EnforceRelatedEntityStateOnSourceEntity, "Enforce Related Entity State On Source Entity");
            RelationshipTypeDictionary.Add(DataModelRelationshipType.CheckRelatedEntityPromoteStatusOnPromote, "Check Related Entity Promote Status On Promote");
        }

        /// <summary>
        /// Build AttributeModel Dictionary
        /// </summary>
        private static void BuildAttributeModelDictionary()
        {
            AttributeModelDictionary.Add(DataModelAttributeModel.AttributeType, "Attribute Type");
            AttributeModelDictionary.Add(DataModelAttributeModel.AttributeName, "Attribute Name");
            AttributeModelDictionary.Add(DataModelAttributeModel.AttributeLongName, "Attribute Long Name");
            AttributeModelDictionary.Add(DataModelAttributeModel.AttributeParentName, "Attribute Parent Name");
            AttributeModelDictionary.Add(DataModelAttributeModel.DataType, "Data Type");
            AttributeModelDictionary.Add(DataModelAttributeModel.DisplayType, "Display Type");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsCollection, "Is Collection");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsInheritable, "Is Inheritable");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsLocalizable, "Is Localizable");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsComplex, "Is Complex");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsLookup, "Is Lookup");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsRequired, "Is Required");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsReadOnly, "Is ReadOnly");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsHidden, "Is Hidden");
            AttributeModelDictionary.Add(DataModelAttributeModel.ShowAtEntityCreation, "Show At Entity Creation?");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsSearchable, "Is Searchable");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsNullValueSearchRequired, "Is Null Value Search Required");
            AttributeModelDictionary.Add(DataModelAttributeModel.GenerateReportTableColumn, "Generate Report Table Column?");
            AttributeModelDictionary.Add(DataModelAttributeModel.DefaultValue, "Default Value"); 
            AttributeModelDictionary.Add(DataModelAttributeModel.MinimumLength, "Minimum Length");
            AttributeModelDictionary.Add(DataModelAttributeModel.MaximumLength, "Maximum Length");
            AttributeModelDictionary.Add(DataModelAttributeModel.RangeFrom, "Range From");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsRangeFromInclusive, "Is Range From Inclusive");
            AttributeModelDictionary.Add(DataModelAttributeModel.RangeTo, "Range To");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsRangeToInclusive, "Is Range To Inclusive");
            AttributeModelDictionary.Add(DataModelAttributeModel.Precision, "Precision");
            AttributeModelDictionary.Add(DataModelAttributeModel.IsPrecisionArbitrary, "Use Arbitrary Precision?");
            AttributeModelDictionary.Add(DataModelAttributeModel.UOMType, "UOM Type");
            AttributeModelDictionary.Add(DataModelAttributeModel.AllowedUOMs, "Allowed UOMs");
            AttributeModelDictionary.Add(DataModelAttributeModel.DefaultUOM, "Default UOM");
            AttributeModelDictionary.Add(DataModelAttributeModel.AllowableValues, "Allowable Values");
            AttributeModelDictionary.Add(DataModelAttributeModel.LookUpTableName, "LookUp Table Name");
            AttributeModelDictionary.Add(DataModelAttributeModel.LookupDisplayColumns, "Lookup Display Columns");
            AttributeModelDictionary.Add(DataModelAttributeModel.LookupSearchColumns, "Lookup Search Columns");
            AttributeModelDictionary.Add(DataModelAttributeModel.LookupDisplayFormat, "Lookup Display Format");
            AttributeModelDictionary.Add(DataModelAttributeModel.LookupSortOrder, "Lookup Sort Order");
            AttributeModelDictionary.Add(DataModelAttributeModel.ExportFormat, "Export Format");
            AttributeModelDictionary.Add(DataModelAttributeModel.SortOrder, "Sort Order");
            AttributeModelDictionary.Add(DataModelAttributeModel.Definition, "Definition");
            AttributeModelDictionary.Add(DataModelAttributeModel.Example, "Example");
            AttributeModelDictionary.Add(DataModelAttributeModel.BusinessRule, "Business Rule");
            AttributeModelDictionary.Add(DataModelAttributeModel.Label, "Label");
            AttributeModelDictionary.Add(DataModelAttributeModel.Extension, "Extension");
            AttributeModelDictionary.Add(DataModelAttributeModel.WebURI, "Web URI");
            AttributeModelDictionary.Add(DataModelAttributeModel.EnableHistory, "Enable History");
            AttributeModelDictionary.Add(DataModelAttributeModel.ApplyTimeZoneConversion, "Apply Time Zone Conversion");
            AttributeModelDictionary.Add(DataModelAttributeModel.AttributeRegExp, "Attribute Regular Expression");
        }

        /// <summary>
        /// Build dictionary for attribute model localization.
        /// </summary>
        private static void BuildAttributeModelLocalizationDictionary()
        {
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.AttributePath, "Attribute Path");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.LocaleName, "Locale");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.AttributeLongName, "Attribute Long Name");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.MinimumLength, "Min Length");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.MaximumLength, "Max Length");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.Definition, "Definition");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.AttributeExample, "Example");
            AttributeModelLocalizationDictionary.Add(DataModelAttributeModelLocalization.BusinessRule, "Business Rule");
        }

        /// <summary>
        /// Build dictionary for category.
        /// </summary>
        private static void BuildCategoryDictionary()
        {
            CategoryDictionary.Add(DataModelCategory.CategoryName, "Category Name");
            CategoryDictionary.Add(DataModelCategory.CategoryLongName, "Category Long Name");
            CategoryDictionary.Add(DataModelCategory.ParentCategoryPath, "Parent Category Path");
            CategoryDictionary.Add(DataModelCategory.HierarchyName, "Hierarchy Name");
        }

        /// <summary>
        /// Build dictionary for category.
        /// </summary>
        private static void BuildCategoryLocalizationDictionary()
        {
            CategoryLocalizationDictionary.Add(DataModelCategoryLocalization.CategoryName, "Category Name");
            CategoryLocalizationDictionary.Add(DataModelCategoryLocalization.ParentCategoryPath, "Parent Category Path");
            CategoryLocalizationDictionary.Add(DataModelCategoryLocalization.HierarchyName, "Hierarchy Name");
            CategoryLocalizationDictionary.Add(DataModelCategoryLocalization.Locale, "Locale");
            CategoryLocalizationDictionary.Add(DataModelCategoryLocalization.CategoryLongName, "Category Long Name");
        }

        /// <summary>
        /// Build ContainerEntityTypeMapping Dictionary
        /// </summary>
        private static void BuildContainerEntityTypeMappingDictionary()
        {
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.OrganizationName, "Organization Name");
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.ContainerName, "Container Name");
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.EntityTypeName, "Entity Type Name");
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.ShowAtCreation, "Show at Creation");
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.MinimumExtensions, "Minimum Extensions Required");
            ContainerEntityTypeMappingDictionary.Add(DataModelContainerEntityTypeMapping.MaximumExtensions, "Maximum Extensions Allowed");
        }

        /// <summary>
        /// Build EntityType Attribute Mapping Dictionary
        /// </summary>
        private static void BuildEntityTypeAttributeMappingDictionary()
        {
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.EntityTypeName, "Entity Type Name");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.AttributePath, "Attribute Path");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.ShowAtCreation, "Show at Creation");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.Required, "Is Required");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.ReadOnly, "Is ReadOnly");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.Extension, "Extension");
            EntityTypeAttributeMappingDictionary.Add(DataModelEntityTypeAttributeMapping.SortOrder, "Sort Order");
        }

        /// <summary>
        /// Build Container EntityType Attribute Mapping Dictionary
        /// </summary>
        private static void BuildContainerEntityTypeAttributeMappingDictionary()
        {
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.OrganizationName, "Organization Name");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.ContainerName, "Container Name");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.EntityTypeName, "Entity Type Name");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.AttributePath, "Attribute Path");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.ShowAtCreation, "Show at Creation");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.Required, "Is Required");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.ReadOnly, "Is ReadOnly");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.Extension, "Extension");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.SortOrder, "Sort Order");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.InheritableOnly, "Inheritable Only");
            ContainerEntityTypeAttributeMappingDictionary.Add(DataModelContainerEntityTypeAttributeMapping.AutoPromotable, "Auto Promotable");
        }

        /// <summary>
        /// Build Category Attribute Mapping Dictionary
        /// </summary>
        private static void BuildCategoryAttributeMappingDictionary()
        {
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.HierarchyName, "Hierarchy Name");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.CategoryName, "Category Name");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.ParentCategoryPath, "Parent Category Path");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.AttributePath, "Attribute Path");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.IsRequired, "Is Required");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.IsReadOnly, "Is ReadOnly");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.DefaultValue, "Default Value");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.MinimumLength, "Minimum Length");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.MaximumLength, "Maximum Length");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.RangeFrom, "Range From");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.IsRangeFromInclusive, "Is Range From Inclusive");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.RangeTo, "Range To");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.IsRangeToInclusive, "Is Range To Inclusive");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.Precision, "Precision");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.AllowedUOMs, "Allowed UOMs");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.DefaultUOM, "Default UOM");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.AllowableValues, "Allowable Values");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.SortOrder, "Sort Order");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.Definition, "Definition");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.Example, "Example");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.BusinessRule, "Business Rule");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.InheritableOnly, "Inheritable Only");
            CategoryAttributeMappingDictionary.Add(DataModelCategoryAttributeMapping.AutoPromotable, "Auto Promotable");
        }

        /// <summary>
        /// Build RelationshipType EntityType Mapping Dictionary
        /// </summary>
        private static void BuildRelationshipTypeEntityTypeMappingDictionary()
        {
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.EntityTypeName, "Entity Type Name");
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.RelationshipTypeName, "Relationship Type Name");
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.DrillDown, "Drill Down?");
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.IsDefaultRelation, "Is Default");
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.ReadOnly, "Is ReadOnly");
            RelationshipTypeEntityTypeMappingDictionary.Add(DataModelRelationshipTypeEntityTypeMapping.Excludable, "Excludable?");
        }

        /// <summary>
        /// Build RelationshipType EntityType Mapping Cardinality Dictionary
        /// </summary>
        private static void BuildRelationshipTypeEntityTypeMappingCardinalityDictionary()
        {
            RelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, "Entity Type Name");
            RelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, "Relationship Type Name");
            RelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName, "To Entity Type Name");
            RelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelRelationshipTypeEntityTypeMappingCardinality.MinRequired, "Min Required");
            RelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelRelationshipTypeEntityTypeMappingCardinality.MaxAllowed, "Max Allowed");
        }

        /// <summary>
        /// Build Container RelationshipType EntityType Mapping Dictionary
        /// </summary>
        private static void BuildContainerRelationshipTypeEntityTypeMappingDictionary()
        {
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.OrganizationName, "Organization Name");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.ContainerName, "Container Name");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.EntityTypeName, "Entity Type Name");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName, "Relationship Type Name");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.DrillDown, "Drill Down?");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.IsDefaultRelation, "Is Default");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.ReadOnly, "Is ReadOnly");
            ContainerRelationshipTypeEntityTypeMappingDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMapping.Excludable, "Excludable?");
        }

        /// <summary>
        /// Build Container RelationshipType EntityTypeMapping Cardinality Dictionary
        /// </summary>
        private static void BuildContainerRelationshipTypeEntityTypeMappingCardinalityDictionary()
        {
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, "Organization Name");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName, "Container Name");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, "Entity Type Name");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, "Relationship Type Name");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName, "To Entity Type Name");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MinRequired, "Min Required");
            ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary.Add(DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MaxAllowed, "Max Allowed");
        }

        /// <summary>
        /// Build RelationshipType Attribute Mapping Dictionary
        /// </summary>
        private static void BuildRelationshipTypeAttributeMappingDictionary()
        {
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.RelationshipTypeName, "Relationship Type Name");
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.AttributePath, "Attribute Path");
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.Required, "Is Required");
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.ReadOnly, "Is ReadOnly");
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.ShowInline, "Show On Display Table?");
            RelationshipTypeAttributeMappingDictionary.Add(DataModelRelationshipTypeAttributeMapping.SortOrder, "Sort Order");
        }

        /// <summary>
        /// Build Container RelationshipType Attribute Mapping Dictionary
        /// </summary>
        private static void BuildContainerRelationshipTypeAttributeMappingDictionary()
        {
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.OrganizationName, "Organization Name");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.ContainerName, "Container Name");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.RelationshipTypeName, "Relationship Type Name");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.AttributePath, "Attribute Path");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.Required, "Is Required");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.ReadOnly, "Is ReadOnly");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.ShowInline, "Show On Display Table?");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.SortOrder, "Sort Order");
            ContainerRelationshipTypeAttributeMappingDictionary.Add(DataModelContainerRelationshipTypeAttributeMapping.AutoPromotable, "Auto Promotable");
        }
       
        /// <summary>
        /// Build Security Role Dictionary
        /// </summary>
        private static void BuildSecurityRoleDictionary()
        {
            SecurityRoleDictionary.Add(DataModelSecurityRole.ShortName, "Role Name");
            SecurityRoleDictionary.Add(DataModelSecurityRole.LongName, "Role Long Name");
            SecurityRoleDictionary.Add(DataModelSecurityRole.IsPrivateRole, "Is Private Role");
        }

        /// <summary>
        /// Build Security User Dictionary
        /// </summary>
        private static void BuildSecurityUserDictionary()
        {
            SecurityUserDictionary.Add(DataModelSecurityUser.Login, "Login");
            SecurityUserDictionary.Add(DataModelSecurityUser.Password, "Password");
            SecurityUserDictionary.Add(DataModelSecurityUser.AuthenticationType, "Authentication Type");
            SecurityUserDictionary.Add(DataModelSecurityUser.Roles, "Roles");
            SecurityUserDictionary.Add(DataModelSecurityUser.ManagerName, "Manager Name");
            SecurityUserDictionary.Add(DataModelSecurityUser.Initials, "Initials");
            SecurityUserDictionary.Add(DataModelSecurityUser.FirstName, "First Name");
            SecurityUserDictionary.Add(DataModelSecurityUser.LastName, "Last Name");
            SecurityUserDictionary.Add(DataModelSecurityUser.Email, "Email");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultUILocale, "Default UI Locale");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultDataLocale, "Default Data Locale");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultOrganization, "Default Organization");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultHierarchy, "Default Hierarchy");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultContainer, "Default Container");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultRole, "Default Role");
            SecurityUserDictionary.Add(DataModelSecurityUser.DefaultTimeZone, "Default Time Zone");
            SecurityUserDictionary.Add(DataModelSecurityUser.NoOfRecordsToShowPerPageInDisplayTable, "No of Records to show Per Page in Display Table");
            SecurityUserDictionary.Add(DataModelSecurityUser.NoOfPagesToShowForDisplayTable, "No of Pages to show For Display Table");
        }

        /// <summary>
        /// Build Lookup Model Dictionary
        /// </summary>
        private static void BuildLookupModelDictionary()
        {
            LookupModelDictionary.Add(DataModelLookupModel.TableName, "Table Name");
            LookupModelDictionary.Add(DataModelLookupModel.Sequence, "Sequence");
            LookupModelDictionary.Add(DataModelLookupModel.ColumnName, "Column Name");
            LookupModelDictionary.Add(DataModelLookupModel.DataType, "Data Type");
            LookupModelDictionary.Add(DataModelLookupModel.Width, "Width");
            LookupModelDictionary.Add(DataModelLookupModel.Precision, "Precision");
            LookupModelDictionary.Add(DataModelLookupModel.Nullable, "Nullable?");
            LookupModelDictionary.Add(DataModelLookupModel.IsUnique, "Is Unique");
            LookupModelDictionary.Add(DataModelLookupModel.DefaultValue, "Default Value");
            LookupModelDictionary.Add(DataModelLookupModel.CheckConstraint, "Check Constraint");
        }

        /// <summary>
        /// Build Word List Dictionary
        /// </summary>
        private static void BuildWordListDictionary()
        {
            WordListModelDictionary.Add(DataModelImportWordList.ImportWordListShortName, "WordList Short Name");
            WordListModelDictionary.Add(DataModelImportWordList.ImportWordListLongName, "WordList Long Name");
            WordListModelDictionary.Add(DataModelImportWordList.ImportWordListIsFlushAndFillMode, "Flush and Fill");
        }

        /// <summary>
        /// Build Word Element Dictionary
        /// </summary>
        private static void BuildWordElementDictionary()
        {
            WordElementModelDictionary.Add(DataModelWordElement.Word, "Word");
            WordElementModelDictionary.Add(DataModelWordElement.Substitute, "Substitute");
            WordElementModelDictionary.Add(DataModelWordElement.WordListName, "WordList Short Name");
        }

        /// <summary>
        /// Builds the entity variant definition dictionary.
        /// </summary>
        private static void BuildEntityVariantDefinitionDictionary()
        {
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.EntityVariantDefinitionName, "Entity Variant Definition Name");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.HasDimensionAttributes, "Has Dimension Attributes?");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.RootEntityType, "Root Entity Type");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.ChildLevel, "Child Level");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.ChildEntityType, "Child Entity Type");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.SourceAttribute, "Source Attribute");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.IsOptional, "Is Optional?");
            EntityVariantDefinitionDictionary.Add(DataModelEntityVariantDefinition.TargetAttribute, "Target Attribute");
        }

        /// <summary>
        /// Builds the entity variant definition mapping dictionary.
        /// </summary>
        private static void BuildEntityVariantDefinitionMappingDictionary()
        {
            EntityVariantDefinitionMappingDictionary.Add(DataModelEntityVariantDefinitionMapping.EntityVariantDefinitionName, "Entity Variant Definition Name");
            EntityVariantDefinitionMappingDictionary.Add(DataModelEntityVariantDefinitionMapping.ContainerName, "Container Name");
            EntityVariantDefinitionMappingDictionary.Add(DataModelEntityVariantDefinitionMapping.CategoryPath, "Category Path");
        }

        #endregion
    }

    /// <summary>
    /// Specifies the Internal Object Collection
    /// </summary>
    public static class InternalObjectCollection
    {
        #region Public InternalObjectTypeCollection
        /// <summary>
        /// Indicates the collection of Internal Entity Types which are part of system
        /// </summary>
        public static Collection<String> EntityTypeNames = new Collection<String> { "category", "entity", "child entity" };

        /// <summary>
        /// Indicates the collection of Internal security user names which are part of system
        /// </summary>
        public static Collection<String> SecurityUserNames = new Collection<String> { "system", "cfadmin" };

        /// <summary>
        /// Indicates the collection of Internal Security roles which are part of system
        /// </summary>
        public static Collection<String> SecurityRoleNames = new Collection<String> { "systemadmin", "system admin", "all data access" };

        /// <summary>
        /// Indicates the collection of lookup internal column names which are part of system
        /// </summary>
        public static Collection<String> LookupInternalColumnNames = new Collection<String>() { "id", "createdatetime", "moddatetime", "createuser", "moduser", "createprogram", "modprogram" };

        #endregion
    }
}