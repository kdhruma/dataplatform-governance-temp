using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MDM.Core.DataModel
{
    /// <summary>
    /// Represents the data model metadata 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelMetadata
    {
        /// <summary>
        /// Represents the sheet number
        /// </summary>
        [EnumMember]
        SheetNo,

        /// <summary>
        /// Represents the data model type name
        /// </summary>
        [EnumMember]
        DataModelTypeName,

        /// <summary>
        /// Represents the sheet name
        /// </summary>
        [EnumMember]
        SheetName,

        /// <summary>
        /// Represents the process data sheet
        /// </summary>
        [EnumMember]
        Processdatasheet
    }

    /// <summary>
    /// Represents the data model metadata 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelConfiguration
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
    /// Represents the configuration items
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelConfigurationItem
    {
        /// <summary>
        /// Represents default version number
        /// </summary>
        [EnumMember]
        TemplateVersion = 0,

        /// <summary>
        /// Represents default separator
        /// </summary>
        [EnumMember]
        DefaultSeparator = 1,

        /// <summary>
        /// Represents flag for enabling or disabling lookup dependencies validation during Attribute import
        /// </summary>
        [EnumMember]
        ValidateLookupDependencies = 2,

        /// <summary>
        /// Represents flag for enabling or disabling UOM dependencies validation during Attribute import
        /// </summary>
        [EnumMember]
        ValidateUomDependencies = 3
    }

    /// <summary>
    /// Represents data model common headers
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelCommonHeaders
    {
        /// <summary>
        /// Represent the identifier of data model
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// Represents the action
        /// </summary>
        [EnumMember]
        Action,

        /// <summary>
        /// Represents the unique identifier
        /// </summary>
        [EnumMember]
        UniqueIdentifier
    }

    /// <summary>
    /// Represents data model organization
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelOrganization
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the organization long name
        /// </summary>
        [EnumMember]
        OrganizationLongName,

        /// <summary>
        /// Represents the organization parent name
        /// </summary>
        [EnumMember]
        OrganizationParentName
    }

    /// <summary>
    /// Represents data model container localization 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerLocalization
    {
        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the locale
        /// </summary>
        [EnumMember]
        Locale
    }

    /// <summary>
    /// Represents data model hierarchy
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelHierarchy
    {
        /// <summary>
        /// Represents the hierarchy name
        /// </summary>
        [EnumMember]
        HierarchyName,

        /// <summary>
        /// Represents the hierarchy long name
        /// </summary>
        [EnumMember]
        HierarchyLongName,

        /// <summary>
        /// Represents the hierarchy leaf node only flag
        /// </summary>
        [EnumMember]
        LeafNodeOnly
    }

    /// <summary>
    /// Represents data model container
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainer
    {
        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the container long name
        /// </summary>
        [EnumMember]
        ContainerLongName,

        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the hierarchy name
        /// </summary>
        [EnumMember]
        HierarchyName,

        /// <summary>
        /// Represents the container type
        /// </summary>
        [EnumMember]
        ContainerType,

        /// <summary>
        /// Represents the container qualifier
        /// </summary>
        [EnumMember]
        ContainerQualifier,

        /// <summary>
        /// Represents the container secondary qualifier
        /// </summary>
        [EnumMember]
        ContainerSecondaryQualifiers,

        /// <summary>
        /// Represents the parent container
        /// </summary>
        [EnumMember]
        ParentContainerName,

        /// <summary>
        /// Represents that approved copy is required or not for container
        /// </summary>
        [EnumMember]
        NeedsApprovedCopy,

        /// <summary>
        /// Represents the workflow type
        /// </summary>
        [EnumMember]
        WorkflowType,

        /// <summary>
        /// Represents that auto extension is enabled or not for container
        /// </summary>
        [EnumMember]
        AutoExtensionEnabled
    }

    /// <summary>
    /// Represents data model entity type 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelEntityType
    {
        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the entity type long name
        /// </summary>
        [EnumMember]
        EntityTypeLongName
    }

    /// <summary>
    /// Represents data model relationship type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelRelationshipType
    {
        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents the relationship type long name
        /// </summary>
        [EnumMember]
        RelationshipTypeLongName,

        /// <summary>
        /// Represents the relationship type Enforce Related Entity State On Source Entity flag
        /// </summary>
        [EnumMember]
        EnforceRelatedEntityStateOnSourceEntity,

        /// <summary>
        /// Represents the relationship type Check Related Entity Promote Status On Promote flag
        /// </summary>
        [EnumMember]
        CheckRelatedEntityPromoteStatusOnPromote
    }

    /// <summary>
    /// Represents data model attribute model 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelAttributeModel
    {
        /// <summary>
        /// Represents the attribute type
        /// </summary>
        [EnumMember]
        AttributeType,

        /// <summary>
        /// Represents the attribute name
        /// </summary>
        [EnumMember]
        AttributeName,

        /// <summary>
        /// Represents the attribute long name
        /// </summary>
        [EnumMember]
        AttributeLongName,

        /// <summary>
        /// Represents the attribute parent name
        /// </summary>
        [EnumMember]
        AttributeParentName,

        /// <summary>
        /// Represents the data type
        /// </summary>
        [EnumMember]
        DataType,

        /// <summary>
        /// Represents the display type
        /// </summary>
        [EnumMember]
        DisplayType,

        /// <summary>
        /// Represents IsCollection property
        /// </summary>
        [EnumMember]
        IsCollection,

        /// <summary>
        /// Represents IsInheritable property
        /// </summary>
        [EnumMember]
        IsInheritable,

        /// <summary>
        /// Represents IsLocalizable property of attribute model
        /// </summary>
        [EnumMember]
        IsLocalizable,

        /// <summary>
        /// Represents IsComplex property of attribute model
        /// </summary>
        [EnumMember]
        IsComplex,

        /// <summary>
        /// Represents IsLookup property of attribute model
        /// </summary>
        [EnumMember]
        IsLookup,

        /// <summary>
        /// Represents IsRequired property of attribute model
        /// </summary>
        [EnumMember]
        IsRequired,

        /// <summary>
        /// Represents IsReadOnly property of attribute model
        /// </summary>
        [EnumMember]
        IsReadOnly,

        /// <summary>
        /// Represents IsHidden property of attribute model
        /// </summary>
        [EnumMember]
        IsHidden,

        /// <summary>
        /// Represents ShowAtEntityCreation property of attribute model
        /// </summary>
        [EnumMember]
        ShowAtEntityCreation,

        /// <summary>
        /// Represents IsSearchable property of attribute model
        /// </summary>
        [EnumMember]
        IsSearchable,

        /// <summary>
        /// Represents the IsNullValueSearchRequired property of attribute model
        /// </summary>
        [EnumMember]
        IsNullValueSearchRequired,

        /// <summary>
        /// Represents the GenerateReportTableColumn property
        /// </summary>
        [EnumMember]
        GenerateReportTableColumn,

        /// <summary>
        /// Represents the default value 
        /// </summary>
        [EnumMember]
        DefaultValue,

        /// <summary>
        /// Represents the minimum length
        /// </summary>
        [EnumMember]
        MinimumLength,

        /// <summary>
        /// Represents the maximum length
        /// </summary>
        [EnumMember]
        MaximumLength,

        /// <summary>
        /// Represents RangeFrom property
        /// </summary>
        [EnumMember]
        RangeFrom,

        /// <summary>
        /// Represents IsRangeFromInclusive property of attribute model
        /// </summary>
        [EnumMember]
        IsRangeFromInclusive,

        /// <summary>
        /// Represents RangeTo property of attribute model
        /// </summary>
        [EnumMember]
        RangeTo,

        /// <summary>
        /// Represents IsRangeToInclusive property of attribute model
        /// </summary>
        [EnumMember]
        IsRangeToInclusive,

        /// <summary>
        /// Represents the precision
        /// </summary>
        [EnumMember]
        Precision,

        /// <summary>
        /// Represents the UOM type
        /// </summary>
        [EnumMember]
        UOMType,

        /// <summary>
        /// Represents the allowed UOMs
        /// </summary>
        [EnumMember]
        AllowedUOMs,

        /// <summary>
        /// Represents the default UOM
        /// </summary>
        [EnumMember]
        DefaultUOM,

        /// <summary>
        /// Represents the allowable values
        /// </summary>
        [EnumMember]
        AllowableValues,


        /// <summary>
        /// Represents the lookUp table name
        /// </summary>
        [EnumMember]
        LookUpTableName,

        /// <summary>
        /// Represents the lookup display columns
        /// </summary>
        [EnumMember]
        LookupDisplayColumns,

        /// <summary>
        /// Represents the lookup search columns
        /// </summary>
        [EnumMember]
        LookupSearchColumns,

        /// <summary>
        /// Represents the lookup display format
        /// </summary>
        [EnumMember]
        LookupDisplayFormat,

        /// <summary>
        /// Represents the lookup sort order
        /// </summary>
        [EnumMember]
        LookupSortOrder,

        /// <summary>
        /// Represents the export format
        /// </summary>
        [EnumMember]
        ExportFormat,

        /// <summary>
        /// Represents the sort order
        /// </summary>
        [EnumMember]
        SortOrder,

        /// <summary>
        /// Represents the definition property of attribute model
        /// </summary>
        [EnumMember]
        Definition,

        /// <summary>
        /// Represents the example property of attribute model
        /// </summary>
        [EnumMember]
        Example,

        /// <summary>
        /// Represents the business rule property of attribute model
        /// </summary>
        [EnumMember]
        BusinessRule,

        /// <summary>
        /// Represents the label property of attribute model
        /// </summary>
        [EnumMember]
        Label,

        /// <summary>
        /// Represents the extension property of attribute model
        /// </summary>
        [EnumMember]
        Extension,

        /// <summary>
        /// Represents the WebURI property of attribute model
        /// </summary>
        [EnumMember]
        WebURI,

        /// <summary>
        /// Represents the IsPrecisionArbitrary property of attribute model
        /// </summary>
        [EnumMember]
        IsPrecisionArbitrary,

        /// <summary>
        /// Represents the Enable History property of attribute model
        /// </summary>
        [EnumMember]
        EnableHistory,

        /// <summary>
        /// Represents the Apply Time Zone Conversion of attribute model
        /// </summary>
        [EnumMember]
        ApplyTimeZoneConversion,

        /// <summary>
        /// AttributeRegExp
        /// </summary>
        [EnumMember]
        AttributeRegExp
    }

    /// <summary>
    /// Represents data model attribute model localization
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelAttributeModelLocalization
    {
        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents the locale name
        /// </summary>
        LocaleName,

        /// <summary>
        /// Represents the attribute long name
        /// </summary>
        [EnumMember]
        AttributeLongName,

        /// <summary>
        /// Represents the maximum length
        /// </summary>
        [EnumMember]
        MaximumLength,

        /// <summary>
        /// Represents the minimum length
        /// </summary>
        [EnumMember]
        MinimumLength,

        /// <summary>
        /// Represents the definition
        /// </summary>
        [EnumMember]
        Definition,

        /// <summary>
        /// Represents the attribute example
        /// </summary>
        [EnumMember]
        AttributeExample,

        /// <summary>
        /// Represents the business rule
        /// </summary>
        [EnumMember]
        BusinessRule
    }

    /// <summary>
    /// Represents data model category 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelCategory
    {
        /// <summary>
        /// Represents the identifier
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// Represents the action
        /// </summary>
        [EnumMember]
        Action,

        /// <summary>
        /// Represents the unique identifier
        /// </summary>
        [EnumMember]
        UniqueIdentifier,

        /// <summary>
        /// Represents the category name
        /// </summary>
        [EnumMember]
        CategoryName,

        /// <summary>
        /// Represents the category long name
        /// </summary>
        [EnumMember]
        CategoryLongName,

        /// <summary>
        /// Represents the parent category path
        /// </summary>
        [EnumMember]
        ParentCategoryPath,

        /// <summary>
        /// Represents the hierarchy name
        /// </summary>
        [EnumMember]
        HierarchyName
    }

    /// <summary>
    /// Represents data model category localization
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelCategoryLocalization
    {
        /// <summary>
        /// Represents the identifier
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// Represents the action
        /// </summary>
        [EnumMember]
        Action,

        /// <summary>
        /// Represents the unique identifier
        /// </summary>
        [EnumMember]
        UniqueIdentifier,

        /// <summary>
        /// Represents the category name
        /// </summary>
        [EnumMember]
        CategoryName,

        /// <summary>
        /// Represents the parent category path
        /// </summary>
        [EnumMember]
        ParentCategoryPath,

        /// <summary>
        /// Represents the hierarchy name
        /// </summary>
        [EnumMember]
        HierarchyName,

        /// <summary>
        /// Represents the locale
        /// </summary>
        [EnumMember]
        Locale,

        /// <summary>
        /// Represents the category long name
        /// </summary>
        [EnumMember]
        CategoryLongName
    }

    /// <summary>
    /// Represents data model container entity type mapping 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerEntityTypeMapping
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents if the selected entity type can be shown at creation time
        /// </summary>
        [EnumMember]
        ShowAtCreation,

        /// <summary>
        /// Represents the minimum occurance of entity in different categories for a container
        /// </summary>
        [EnumMember]
        MinimumExtensions,

        /// <summary>
        /// Represents the maximum occurance of entity in different categories for a container
        /// </summary>
        [EnumMember]
        MaximumExtensions
    }

    /// <summary>
    /// Represents data model entity type attribute mapping 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelEntityTypeAttributeMapping
    {
        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents Required property of entity type attribute mapping
        /// </summary>
        [EnumMember]
        Required,

        /// <summary>
        /// Represents ReadOnly property of entity type attribute mapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents ShowAtCreation property of entity type attribute mapping
        /// </summary>
        [EnumMember]
        ShowAtCreation,

        /// <summary>
        /// Represents SortOrder property of entity type attribute mapping
        /// </summary>
        [EnumMember]
        SortOrder,

        /// <summary>
        /// Represents Extension property of entity type attribute mapping
        /// </summary>
        [EnumMember]
        Extension
    }

    /// <summary>
    /// Represents data model container entity type attribute mapping 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerEntityTypeAttributeMapping
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents the Required property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        Required,

        /// <summary>
        /// Represents the ReadOnly proeprty of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents the ShowAtCreation of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ShowAtCreation,

        /// <summary>
        /// Represents the SortOrder property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        SortOrder,

        /// <summary>
        /// Represents the extension property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        Extension,

        /// <summary>
        /// Represents the InheritableOnly property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        InheritableOnly,

        /// <summary>
        /// Represents the AutoPromotable property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        AutoPromotable
    }

    /// <summary>
    /// Represents data model category attribute mapping
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelCategoryAttributeMapping
    {
        /// <summary>
        /// Represents the hierarchy name
        /// </summary>
        [EnumMember]
        HierarchyName,

        /// <summary>
        /// Represents the category name
        /// </summary>
        [EnumMember]
        CategoryName,

        /// <summary>
        /// Represents the parent category path
        /// </summary>
        [EnumMember]
        ParentCategoryPath,

        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents the IsRequired property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        IsRequired,

        /// <summary>
        /// Represents the IsReadOnly property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        IsReadOnly,

        /// <summary>
        /// Represents the DefaultValue property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        DefaultValue,

        /// <summary>
        /// Represents the minimum length
        /// </summary>
        [EnumMember]
        MinimumLength,

        /// <summary>
        /// Represents the maximum length
        /// </summary>
        [EnumMember]
        MaximumLength,

        /// <summary>
        /// Represents the RangeFrom property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        RangeFrom,

        /// <summary>
        /// Represents the IsRangeFromInclusive property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        IsRangeFromInclusive,

        /// <summary>
        /// Represents the RangeTo property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        RangeTo,

        /// <summary>
        /// Represents the IsRangeToInclusive property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        IsRangeToInclusive,

        /// <summary>
        /// Represents the precision
        /// </summary>
        [EnumMember]
        Precision,

        /// <summary>
        /// Represents the allowed UOMs
        /// </summary>
        [EnumMember]
        AllowedUOMs,

        /// <summary>
        /// Represents the default UOM
        /// </summary>
        [EnumMember]
        DefaultUOM,

        /// <summary>
        /// Allowable Values
        /// </summary>
        [EnumMember]
        AllowableValues,

        /// <summary>
        /// Sort Order
        /// </summary>
        [EnumMember]
        SortOrder,

        /// <summary>
        /// Repesents the definition property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        Definition,

        /// <summary>
        /// Represents the example property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        Example,

        /// <summary>
        /// Represents the BusinessRule property of CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        BusinessRule,

        /// <summary>
        /// Represents the InheritableOnly property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        InheritableOnly,

        /// <summary>
        /// Represents the AutoPromotable property of ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        AutoPromotable
    }

    /// <summary>
    /// Represents data model relationship type entity type mapping
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelRelationshipTypeEntityTypeMapping
    {
        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents DrillDown property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        DrillDown,

        /// <summary>
        /// Represents IsDefaultRelationship property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        IsDefaultRelation,

        /// <summary>
        /// Represents ReadOnly property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents Excludable property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        Excludable
    }

    /// <summary>
    /// Represents data model relationship type entity type mapping cardinality 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelRelationshipTypeEntityTypeMappingCardinality
    {
        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents ToEntityType name
        /// </summary>
        [EnumMember]
        ToEntityTypeName,

        /// <summary>
        /// Represents MinRelationshipRequired property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        MinRequired,

        /// <summary>
        /// epresents MaxRelationshipRequired property of RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        MaxAllowed
    }

    /// <summary>
    /// Represents data model container relationship type entity type mapping 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerRelationshipTypeEntityTypeMapping
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents the DrillDown property of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        DrillDown,

        /// <summary>
        /// Represents IsDefaultRelationship property of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        IsDefaultRelation,

        /// <summary>
        /// Represents ReadOnly property of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents Excludable property of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        Excludable

    }

    /// <summary>
    /// Represents data model container relationship type entity type mapping cardinality
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerRelationshipTypeEntityTypeMappingCardinality
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the entity type name
        /// </summary>
        [EnumMember]
        EntityTypeName,

        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents ToEntityType name
        /// </summary>
        [EnumMember]
        ToEntityTypeName,

        /// <summary>
        /// Represents the minimum relationships allowed
        /// </summary>
        [EnumMember]
        MinRequired,

        /// <summary>
        /// Represents the max relationships allowed
        /// </summary>
        [EnumMember]
        MaxAllowed
    }

    /// <summary>
    /// Represents data model relatioship type attribute mapping
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelRelationshipTypeAttributeMapping
    {
        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents Required property of RelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        Required,

        /// <summary>
        /// Represents ReadOnly property of RelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents ShowInline property of RelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ShowInline,

        /// <summary>
        /// Represents SortOrder property of RelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        SortOrder
    }

    /// <summary>
    /// Represents data model container relationship type attribute mapping
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelContainerRelationshipTypeAttributeMapping
    {
        /// <summary>
        /// Represents the organization name
        /// </summary>
        [EnumMember]
        OrganizationName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the relationship type name
        /// </summary>
        [EnumMember]
        RelationshipTypeName,

        /// <summary>
        /// Represents the attribute path
        /// </summary>
        [EnumMember]
        AttributePath,

        /// <summary>
        /// Represents Required property of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        Required,

        /// <summary>
        /// Represents ReadOnly property of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ReadOnly,

        /// <summary>
        /// Represents ShowInline property of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ShowInline,

        /// <summary>
        /// Represents SortOrder property of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        SortOrder,

        /// <summary>
        /// Represents the AutoPromotable property of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        AutoPromotable
    }

    /// <summary>
    /// Represents data model security role
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelSecurityRole
    {
        /// <summary>
        /// Represents the short name of security role
        /// </summary>
        [EnumMember]
        ShortName,

        /// <summary>
        /// Represents the long name of security role
        /// </summary>
        [EnumMember]
        LongName,

        /// <summary>
        /// Indicates if the security role is a private role
        /// </summary>
        [EnumMember]
        IsPrivateRole
    }

    /// <summary>
    /// Represents data model security user
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelSecurityUser
    {
        /// <summary>
        /// Represents the login name of security user
        /// </summary>
        [EnumMember]
        Login,

        /// <summary>
        /// Represents the password of security user
        /// </summary>
        [EnumMember]
        Password,

        /// <summary>
        /// Represents the authentication type
        /// </summary>
        [EnumMember]
        AuthenticationType,

        /// <summary>
        /// Represents the roles associated with the security user
        /// </summary>
        [EnumMember]
        Roles,

        /// <summary>
        /// Represents the manager name
        /// </summary>
        [EnumMember]
        ManagerName,

        /// <summary>
        /// Represents the initials of security user
        /// </summary>
        [EnumMember]
        Initials,

        /// <summary>
        /// Represents the first name of security user
        /// </summary>
        [EnumMember]
        FirstName,

        /// <summary>
        /// Represents the last name of security user
        /// </summary>
        [EnumMember]
        LastName,

        /// <summary>
        /// Represents the email of security user
        /// </summary>
        [EnumMember]
        Email,

        /// <summary>
        /// Represents the default UI locale
        /// </summary>
        [EnumMember]
        DefaultUILocale,

        /// <summary>
        /// Represents the default data locale
        /// </summary>
        [EnumMember]
        DefaultDataLocale,

        /// <summary>
        /// Represents the default organization
        /// </summary>
        [EnumMember]
        DefaultOrganization,

        /// <summary>
        /// Represents the default hierarchy
        /// </summary>
        [EnumMember]
        DefaultHierarchy,

        /// <summary>
        /// Represents the default container
        /// </summary>
        [EnumMember]
        DefaultContainer,

        /// <summary>
        /// Represents the default role
        /// </summary>
        [EnumMember]
        DefaultRole,

        /// <summary>
        /// Represents the default time zone
        /// </summary>
        [EnumMember]
        DefaultTimeZone,

        /// <summary>
        /// Indicates the number of records to show per page in display table
        /// </summary>
        [EnumMember]
        NoOfRecordsToShowPerPageInDisplayTable,

        /// <summary>
        /// Indicates the number of pages to show for display table
        /// </summary>
        [EnumMember]
        NoOfPagesToShowForDisplayTable
    }

    /// <summary>
    /// Represents data model lookup model
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelLookupModel
    {
        /// <summary>
        /// Represents the lookup table name
        /// </summary>
        [EnumMember]
        TableName,

        /// <summary>
        /// Represents the Sequence property of lookup model
        /// </summary>
        [EnumMember]
        Sequence,

        /// <summary>
        /// Represents the Column Name of lookup model
        /// </summary>
        [EnumMember]
        ColumnName,

        /// <summary>
        /// Represents the DataType property of lookup model
        /// </summary>
        [EnumMember]
        DataType,

        /// <summary>
        /// Represents the Width property of lookup model
        /// </summary>
        [EnumMember]
        Width,

        /// <summary>
        /// Represents the Precision property of lookup model
        /// </summary>
        [EnumMember]
        Precision,

        /// <summary>
        /// Represents the Nullable property of lookup model
        /// </summary>
        [EnumMember]
        Nullable,

        /// <summary>
        /// Represents the IsUnique property of lookup model
        /// </summary>
        [EnumMember]
        IsUnique,

        /// <summary>
        /// Represents the DefaultValue property of lookup model
        /// </summary>
        [EnumMember]
        DefaultValue,

        /// <summary>
        /// Represents the CheckConstraint property of lookup model
        /// </summary>
        [EnumMember]
        CheckConstraint
    }

    /// <summary>
    /// Represents data model entity variant definition
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelEntityVariantDefinition
    {
        /// <summary>
        /// Represents the Entity variant definition name
        /// </summary>
        [EnumMember]
        EntityVariantDefinitionName,

        /// <summary>
        /// Field for Root entity type
        /// </summary>
        [EnumMember]
        RootEntityType,

        /// <summary>
        /// Represents the Child level
        /// </summary>
        [EnumMember]
        ChildLevel,

        /// <summary>
        /// Represents the Child entity type
        /// </summary>
        [EnumMember]
        ChildEntityType,

        /// <summary>
        /// Represents the Source attribute
        /// </summary>
        [EnumMember]
        SourceAttribute,

        /// <summary>
        /// Represents the Is optional
        /// </summary>
        [EnumMember]
        IsOptional,

        /// <summary>
        /// Represents the Target attribute
        /// </summary>
        [EnumMember]
        TargetAttribute,

        /// <summary>
        /// Represents the has dimension attributes
        /// </summary>
        [EnumMember]
        HasDimensionAttributes
    }


    /// <summary>
    /// Represents word list data model 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelImportWordList
    {
        /// <summary>
        /// Represents the short name of WordList
        /// </summary>
        [EnumMember]
        ImportWordListShortName,

        /// <summary>
        /// Represents the long name of WordList
        /// </summary>
        [EnumMember]
        ImportWordListLongName,

        /// <summary>
        /// Represents if Flush and Fill mode is used for WordList import
        /// </summary>
        [EnumMember]
        ImportWordListIsFlushAndFillMode
    }

    /// <summary>
    /// Represents word element data model 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelWordElement
    {
        /// <summary>
        /// Represents word of WordElement
        /// </summary>
        [EnumMember]
        Word,

        /// <summary>
        /// Represents the Substitute of WordElement
        /// </summary>
        [EnumMember]
        Substitute,

        /// <summary>
        /// Represents the WordListId of WordElement
        /// </summary>
        [EnumMember]
        WordListName
    }

    /// <summary>
    /// Represents sheets of data model 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelSheet
    {
        /// <summary>
        /// Represents the Organization sheet
        /// </summary>
        [EnumMember]
        [Description("S1 - Organization")]
        Organization,

        /// <summary>
        /// Represents the Hierarchy sheet
        /// </summary>
        [EnumMember]
        [Description("S2 - Hierarchy")]
        Hierarchy,

        /// <summary>
        /// Represents the Container sheet
        /// </summary>
        [EnumMember]
        [Description("S3 - Container")]
        Container,

        /// <summary>
        /// Represents the Container locale sheet
        /// </summary>
        [EnumMember]
        [Description("S4 - Container - Locale")]
        ContainerLocale,

        /// <summary>
        /// Represents the EntityType sheet
        /// </summary>
        [EnumMember]
        [Description("S5 - Entity Type")]
        EntityType,

        /// <summary>
        /// Represents the RelationshipType sheet
        /// </summary>
        [EnumMember]
        [Description("S6 - Relationship Type")]
        RelationshipType,

        /// <summary>
        /// Represents the Attribute sheet
        /// </summary>
        [EnumMember]
        [Description("S7 - Attribute")]
        Attribute,

        /// <summary>
        /// Represents the Attribute locale sheet
        /// </summary>
        [EnumMember]
        [Description("S8 - Attribute - Locale")]
        AttributeLocale,

        /// <summary>
        /// Represents the Category sheet
        /// </summary>
        [EnumMember]
        [Description("S9 - Category")]
        Category,

        /// <summary>
        /// Represents the Category locale sheet
        /// </summary>
        [EnumMember]
        [Description("S10 - Category - Locale")]
        CategoryLocale,

        /// <summary>
        /// Represents the Container Entity Type Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S11 - CON - ET")]
        ContainerEntityTypeMapping,

        /// <summary>
        /// Represents the Entity Type Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S12 - ET - ATTR")]
        EntityTypeAttributeMapping,

        /// <summary>
        /// Represents the Container Entity Type Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S13 - CON - ET - ATTR")]
        ContainerEntityTypeAttributeMapping,

        /// <summary>
        /// Represents the Category Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S14 - CAT - ATTR")]
        CategoryAttributeMapping,

        /// <summary>
        /// Represents the RelationshipType EntityType Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S15 - RT - ET")]
        RelationshipTypeEntityTypeMapping,

        /// <summary>
        /// Represents the RelationshipType EntityType Cardinality sheet
        /// </summary>
        [EnumMember]
        [Description("S16 - RT - ET - CARD")]
        RelationshipTypeEntityTypeCardinality,

        /// <summary>
        /// Represents the Container RelationshipType EntityType Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S17 - CON - RT - ET")]
        ContainerRelationshipTypeEntityTypeMapping,

        /// <summary>
        /// Represents the Container RelationshipType EntityType Cardinality sheet
        /// </summary>
        [EnumMember]
        [Description("S18 - CON - RT - ET - CARD")]
        ContainerRelationshipTypeEntityTypeCardinality,

        /// <summary>
        /// Represents the RelationshipType Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S19 - RT - ATTR")]
        RelationshipTypeAttributeMapping,

        /// <summary>
        /// Represents the Container RelationshipType Attribute Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S20 - CON - RT - ATTR")]
        ContainerRelationshipTypeAttributeMapping,

        /// <summary>
        /// Represents the Security Role sheet
        /// </summary>
        [EnumMember]
        [Description("S21 - Security Role")]
        SecurityRole,

        /// <summary>
        /// Represents the Security User sheet
        /// </summary>
        [EnumMember]
        [Description("S22 - Security User")]
        SecurityUser,

        /// <summary>
        /// Represents the LookupModel sheet
        /// </summary>
        [EnumMember]
        [Description("S23 - Lookup Model")]
        LookupModel,

        /// <summary>
        /// Represents the WordList sheet
        /// </summary>
        [EnumMember]
        [Description("S24 - Word List")]
        WordList,

        /// <summary>
        /// Represents the WordElement sheet
        /// </summary>
        [EnumMember]
        [Description("S25 - Word Element")]
        WordElement,

        /// <summary>
        /// Represents the Entity Variant Definition sheet
        /// </summary>
        [EnumMember]
        [Description("S26 - Entity Variant Definition")]
        EntityVariantDefinition,

        /// <summary>
        /// Represents the Entity Variant Definition Mapping sheet
        /// </summary>
        [EnumMember]
        [Description("S27 - EVD Mapping")]
        EntityVariantDefinitionMapping
    }

    /// <summary>
    /// Represents data model entity variant definition
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelEntityVariantDefinitionMapping
    {
        /// <summary>
        /// Represents the Entity variant definition name
        /// </summary>
        [EnumMember]
        EntityVariantDefinitionName,

        /// <summary>
        /// Represents the container name
        /// </summary>
        [EnumMember]
        ContainerName,

        /// <summary>
        /// Represents the category path
        /// </summary>
        [EnumMember]
        CategoryPath

    }

}