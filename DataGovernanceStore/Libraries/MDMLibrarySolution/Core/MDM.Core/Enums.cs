using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MDM.Core
{
    #region Assorted

    /// <summary>
    /// Defines type of the object 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ObjectType
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Organization
        /// </summary>
        [EnumMember]
        Organization = 2,

        /// <summary>
        /// Catalog
        /// </summary>
        [EnumMember]
        Catalog = 3,

        /// <summary>
        /// Node
        /// </summary>
        [EnumMember]
        Node = 33,

        /// <summary>
        /// Component
        /// </summary>
        [EnumMember]
        Component = 1,

        /// <summary>
        /// Commercial Attribute
        /// </summary>
        [EnumMember]
        CommercialAttribute = 34,

        /// <summary>
        /// Technical Attribute
        /// </summary>
        [EnumMember]
        TechnicalAttribute = 35,

        /// <summary>
        /// Taxonomy
        /// </summary>
        [EnumMember]
        Taxonomy = 38,

        /// <summary>
        /// View
        /// </summary>
        [EnumMember]
        View = 39,

        /// <summary>
        /// Relationships
        /// </summary>
        [EnumMember]
        Relationships = 40,

        /// <summary>
        /// Profile
        /// </summary>
        [EnumMember]
        Profile = 43,

        /// <summary>
        /// Syndication
        /// </summary>
        [EnumMember]
        Syndication = 44,

        /// <summary>
        /// Relationship Attribute
        /// </summary>
        [EnumMember]
        RelationshipAttribute = 101,

        /// <summary>
        /// Category
        /// </summary>
        [EnumMember]
        Category = 102,

        /// <summary>
        /// Entity
        /// </summary>
        [EnumMember]
        Entity = 103,

        /// <summary>
        /// Category Attribute
        /// </summary>
        [EnumMember]
        CategoryAttribute = 104,

        /// <summary>
        /// System Attribute
        /// </summary>
        [EnumMember]
        SystemAttribute = 105,

        /// <summary>
        /// Workflow Attribute
        /// </summary>
        [EnumMember]
        WorkflowAttribute = 106,

        /// <summary>
        /// Entity Type
        /// </summary>
        [EnumMember]
        EntityType = 107,

        /// <summary>
        /// Relationship Type
        /// </summary>
        [EnumMember]
        RelationshipType = 108,

        /// <summary>
        /// Locale
        /// </summary>
        [EnumMember]
        Locale = 109,

        /// <summary>
        /// Entity Metadata 
        /// </summary>
        [EnumMember]
        EntityMetadata = 110,

        /// <summary>
        /// CommmonAttribute
        /// </summary>
        [EnumMember]
        CommonAttribute = 111,

        /// <summary>
        /// Lookup
        /// </summary>
        [EnumMember]
        Lookup = 112,

        /// <summary>
        /// CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        DataModelMetadata = 113,

        /// <summary>
        /// ContainerLocaleMap
        /// </summary>
        [EnumMember]
        ContainerLocalization = 114,

        /// <summary>
        /// CategoryLocalization
        /// </summary>
        [EnumMember]
        CategoryLocalization = 115,

        /// <summary>
        /// ContainerEntityTypeMapping
        /// </summary>
        [EnumMember]
        ContainerEntityTypeMapping = 116,

        /// <summary>
        /// EntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        AttributeModel = 117,

        /// <summary>
        /// AttributesLocalization
        /// </summary>
        [EnumMember]
        AttributeModelLocalization = 118,

        /// <summary>
        /// EntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        EntityTypeAttributeMapping = 119,

        /// <summary>
        /// ContainerEntityTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ContainerEntityTypeAttributeMapping = 120,

        /// <summary>
        /// CategoryAttributeMapping
        /// </summary>
        [EnumMember]
        CategoryAttributeMapping = 121,

        /// <summary>
        /// RelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        RelationshipTypeEntityTypeMapping = 122,

        /// <summary>
        /// RelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        [EnumMember]
        RelationshipTypeEntityTypeMappingCardinality = 123,

        /// <summary>
        /// ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeEntityTypeMapping = 124,

        /// <summary>
        /// ContainerRelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeEntityTypeMappingCardinality = 125,

        /// <summary>
        /// RelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        RelationshipTypeAttributeMapping = 126,

        /// <summary>
        /// ContainerRelationshipTypeAttributeMapping
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeAttributeMapping = 127,

        /// <summary>
        /// Security Role
        /// </summary>
        [EnumMember]
        Role = 129,

        /// <summary>
        /// Security User
        /// </summary>
        [EnumMember]
        User = 130,

        /// <summary>
        /// UserPreferences
        /// </summary>
        [EnumMember]
        UserPreferences = 131,

        /// <summary>
        /// Lookup Model
        /// </summary>
        [EnumMember]
        LookupModel = 132,

        /// <summary>
        /// WordList Model
        /// </summary>
        [EnumMember]
        WordList = 133,

        /// <summary>
        /// WordElement Model
        /// </summary>
        [EnumMember]
        WordElement = 134,

        /// <summary>
        /// Configuration
        /// </summary>
        [EnumMember]
        Configuration = 135,

        /// <summary>
        /// Entity Variant Definition
        /// </summary>
        [EnumMember]
        EntityVariantDefinition = 136,

        /// <summary>
        /// Entity Variant Definition Mapping
        /// </summary>
        [EnumMember]
        EntityVariantDefinitionMapping = 137,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        BusinessCondition = 138,

        /// <summary>
        /// Indicates the metadata in the DDG import/export
        /// </summary>
        [EnumMember]
        Metadata = 139,

        /// <summary>
        /// Indicates the business rules in the DDG import/export
        /// </summary>
        [EnumMember]
        BusinessRules = 140,

        /// <summary>
        /// Indicates the business conditions in the DDG import/export
        /// </summary>
        [EnumMember]
        BusinessConditions = 141,

        /// <summary>
        /// Indicates the business conditions sorting in the DDG import/export
        /// </summary>
        [EnumMember]
        BusinessConditionSorting = 142,

        /// <summary>
        /// Indicates the dynamic governance in the DDG import/export
        /// </summary>
        [EnumMember]
        DynamicGovernance = 143,

        /// <summary>
        /// Indicates the dynamic governance sorting in the DDG import/export
        /// </summary>
        [EnumMember]
        DynamicGovernanceSorting = 144,

        /// <summary>
        /// Indicates the system messages in the DDG import/export
        /// </summary>
        [EnumMember]
        SystemMessages = 145,

        // Stored in DB as Byte. Max value here can be Byte.MaxValue, 255.
    }

    /// <summary>
    /// Defines user actions 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum UserAction
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// View Permission
        /// </summary>
        [EnumMember]
        View = 4,

        /// <summary>
        /// Add permission
        /// </summary>
        [EnumMember]
        Add = 1,

        /// <summary>
        /// Update permission
        /// </summary>
        [EnumMember]
        Update = 2,

        /// <summary>
        /// Delete permission
        /// </summary>
        [EnumMember]
        Delete = 3,

        /// <summary>
        /// Export permission
        /// </summary>
        [EnumMember]
        Export = 6,

        /// <summary>
        /// Import permission
        /// </summary>
        [EnumMember]
        Import = 5,

        /// <summary>
        /// LinkComponents permission
        /// </summary>
        [EnumMember]
        LinkComponents = 7,

        /// <summary>
        /// Execute permission
        /// </summary>
        [EnumMember]
        Execute = 8,

        /// <summary>
        /// Workflow Actions
        /// </summary>
        [EnumMember]
        WorkflowActions = 9,

        /// <summary>
        /// Schedule Actions
        /// </summary>
        [EnumMember]
        Schedule = 10,

        /// <summary>
        /// Subscribe Actions
        /// </summary>
        [EnumMember]
        Subscribe = 11,


        /// <summary>
        /// Reclassify Actions
        /// </summary>
        [EnumMember]
        Reclassify = 12,

        /// <summary>
        /// All Actions
        /// </summary>
        [EnumMember]
        All = 13,

        /// <summary>
        /// No Access
        /// </summary>
        [EnumMember]
        None = 14
    }

    /// <summary>
    /// Defines object actions 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ObjectAction
    {
        /// <summary>
        /// Read the object
        /// </summary>
        [EnumMember]
        Read = 0,
        /// <summary>
        /// Create the object
        /// </summary>
        [EnumMember]
        Create = 1,
        /// <summary>
        /// Update the object
        /// </summary>
        [EnumMember]
        Update = 2,
        /// <summary>
        /// Delete the object
        /// </summary>
        [EnumMember]
        Delete = 3,
        /// <summary>
        /// Action is not defined
        /// </summary>
        [EnumMember]
        Unknown = 4,
        /// <summary>
        /// Ignore the object
        /// </summary>
        [EnumMember]
        Ignore = 5,
        /// <summary>
        /// Exclude the object from collection (for relationships)
        /// </summary>
        [EnumMember]
        Exclude = 6,
        /// <summary>
        /// Rename the object(for DBColumn)
        /// </summary>
        [EnumMember]
        Rename = 7,
        /// <summary>
        /// UnExclude relationships
        /// </summary>
        [EnumMember]
        UnExclude = 8,
        /// <summary>
        /// Reclassify the object
        /// </summary>
        [EnumMember]
        Reclassify = 9,
        /// <summary>
        /// Reparent the object
        /// </summary>
        [EnumMember]
        ReParent = 10,
        /// <summary>
        /// Publish the object
        /// </summary>
        [EnumMember]
        Publish = 11,
        /// <summary>
        /// Replace the object
        /// </summary>
        [EnumMember]
        Replace = 12,
        /// <summary>
        /// Update due to Impacted change.
        /// </summary>
        [EnumMember]
        ImpactedUpdate = 13,
        /// <summary>
        /// Update due to Impacted change.
        /// </summary>
        [EnumMember]
        ImpactedReclassify = 14
    }

    /// <summary>
    /// Defines performed actions on any object
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum PerformedAction
    {
        /// <summary>
        /// None - No Action was performed.
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// Created
        /// </summary>
        [EnumMember]
        Created = 1,
        /// <summary>
        /// Updated
        /// </summary>
        [EnumMember]
        Updated = 2,
        /// <summary>
        /// Deleted
        /// </summary>
        [EnumMember]
        Deleted = 3,
        /// <summary>
        /// Ignored
        /// </summary>
        [EnumMember]
        Ignored = 4,
        /// <summary>
        /// Renamed
        /// </summary>
        [EnumMember]
        Renamed = 5,
        /// <summary>
        /// Reclassify the object
        /// </summary>
        [EnumMember]
        Reclassified = 6,
        /// <summary>
        /// Reparent the object
        /// </summary>
        [EnumMember]
        ReParented = 7,
        /// <summary>
        /// Unknown - This is not same as None
        /// </summary>
        [EnumMember]
        Unknown = 8
    }
    /// <summary>
    /// Indicates the source of attribute value.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeValueSource
    {
        /// <summary>
        /// Attribute value is overridden
        /// </summary>
        [EnumMember]
        Overridden = 0,
        /// <summary>
        /// Attribute value is inherited
        /// </summary>
        [EnumMember]
        Inherited = 1,
        /// <summary>
        /// Master attribute value
        /// </summary>
        [EnumMember]
        Master = 2,
        /// <summary>
        /// Attribute value source is not known
        /// </summary>
        [EnumMember]
        Unknown = 3
    }

    /// <summary>
    /// Defines Basic attribute types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeTypeEnum
    {
        /// <summary>
        /// Attribute type Simple
        /// </summary>
        [EnumMember]
        Simple = 0,

        /// <summary>
        /// Attribute type is Simple collection
        /// </summary>
        [EnumMember]
        SimpleCollection = 1,

        /// <summary>
        /// Attribute type is Complex
        /// </summary>
        [EnumMember]
        Complex = 2,

        /// <summary>
        /// Attribute type is Complex collection
        /// </summary>
        [EnumMember]
        ComplexCollection = 3,

        /// <summary>
        /// Attribute type is not specified as simple or complex. System attributes are considered to be unknown
        /// </summary>
        [EnumMember]
        Unknown = 4,

        /// <summary>
        /// Indicates attribute group
        /// </summary>
        [EnumMember]
        AttributeGroup = 5
    }

    /// <summary>
    /// Defines inheritance modes
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum InheritanceMode
    {
        /// <summary>
        /// Direct mode for relationship
        /// </summary>
        [EnumMember]
        Direct = 0,

        /// <summary>
        /// Derived mode for relationship
        /// </summary>
        [EnumMember]
        Derived = 1,

        /// <summary>
        /// Inherited mode for relationship
        /// </summary>
        [EnumMember]
        Inherited = 2,

        /// <summary>
        /// InheritedDerived mode for relationship
        /// </summary>
        [EnumMember]
        InheritedDerived = 3,
    }

    /// <summary>
    /// Defines relationship status.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RelationshipStatus
    {
        /// <summary>
        /// relationship status is active
        /// </summary>
        [EnumMember]
        Active = 0,

        /// <summary>
        /// relationship status is excluded
        /// </summary>
        [EnumMember]
        Excluded = 1,

        /// <summary>
        /// relationship status is implied.
        /// </summary>
        [EnumMember]
        Implied = 2
    }

    /// <summary>
    /// Defines catalog branch levels
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ContainerBranchLevel
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        Node = 1,

        /// <summary>
        /// Component
        /// </summary>
        [EnumMember]
        Component = 2,

        /// <summary>
        /// Configuration
        /// </summary>
        [EnumMember]
        Configuration = 3
    }

    /// <summary>
    /// Command type used for DBCommandProperties.CommandType.
    /// Indicates type of command text. Will be mapped to Command.CommanType
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CommandTypeEnum
    {
        /// <summary>
        /// Text
        /// </summary>
        [EnumMember]
        Text = 0,

        /// <summary>
        /// Stored Procedure
        /// </summary>
        [EnumMember]
        StoredProcedure = 1,

        /// <summary>
        /// Direct Table
        /// </summary>
        [EnumMember]
        TableDirect = 2
    }




    /// <summary>
    /// Defines object serialization type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ObjectSerialization
    {
        /// <summary>
        /// ProcessingOnly
        /// </summary>
        [EnumMember]
        ProcessingOnly = 1,

        /// <summary>
        /// UIRender
        /// </summary>
        [EnumMember]
        UIRender = 2,

        /// <summary>
        /// Compact
        /// </summary>
        [EnumMember]
        Compact = 3,

        /// <summary>
        /// Full Object
        /// </summary>
        [EnumMember]
        Full = 4,

        /// <summary>
        /// External
        /// </summary>
        [EnumMember]
        External = 5,

        /// <summary>
        /// DataStorage
        /// </summary>
        [EnumMember]
        DataStorage = 6,

        /// <summary>
        /// Serialization type to be used for 
        /// </summary>
        [EnumMember]
        DataTransfer = 7
    }

    /// <summary>
    /// Defines attribute model type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeModelType
    {
        /// <summary>
        /// Attribute Master Model
        /// </summary>
        [EnumMember]
        [LocalizationCode("100161")]
        AttributeMaster = 0,

        /// <summary>
        /// Common Attribute Model
        /// </summary>
        [EnumMember]
        [LocalizationCode("100257")]
        Common = 1,

        /// <summary>
        /// Category Attribute Model
        /// </summary>
        [EnumMember]
        [LocalizationCode("100205")]
        Category = 2,

        /// <summary>
        /// System Attribute Model
        /// </summary>
        [EnumMember]
        [LocalizationCode("110550")]
        System = 3,

        /// <summary>
        /// Relationship Attribute Model
        /// </summary>
        [EnumMember]
        [LocalizationCode("110485")]
        Relationship = 4,

        /// <summary>
        /// All model types
        /// </summary>
        [EnumMember]
        [LocalizationCode("111494")]
        All = 5,

        /// <summary>
        /// Model Type is not defined
        /// </summary>
        [EnumMember]
        [LocalizationCode("112717")]
        Unknown = 6,

        /// <summary>
        /// Indicates Attribute group
        /// </summary>
        [EnumMember]
        [LocalizationCode("111317")]
        AttributeGroup = 7,

        /// <summary>
        /// Indicates MetaData Attribute 
        /// </summary>
        [EnumMember]
        MetaDataAttribute = 8,

        /// <summary>
        /// Indicates common attribute group
        /// </summary>
        [EnumMember]
        CommonAttributeGroup = 9,

        /// <summary>
        /// Indicates category attribute group
        /// </summary>
        [EnumMember]
        CategoryAttributeGroup = 10,

        /// <summary>
        /// Indicates relationship attribute group
        /// </summary>
        [EnumMember]
        RelationshipAttributeGroup = 11
    }

    /// <summary>
    /// Defines entity process mode 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ProcessingMode
    {
        /// <summary>
        /// Process in Sync
        /// </summary>
        [EnumMember]
        Sync = 0,

        /// <summary>
        /// Async Create
        /// </summary>
        [EnumMember]
        AsyncCreate = 1,

        /// <summary>
        /// Async Update
        /// </summary>
        [EnumMember]
        AsyncUpdate = 2,

        /// <summary>
        /// Async Delete
        /// </summary>
        [EnumMember]
        AsyncDelete = 3,

        /// <summary>
        /// Async mode. Applies for any operation.
        /// </summary>
        [EnumMember]
        Async = 4,

        /// <summary>
        /// None. Specifies no processing
        /// </summary>
        [EnumMember]
        None = 5
    }


    /// <summary>
    /// Defines Dynamic Table type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DynamicTableType
    {
        /// <summary>
        /// Table Type is not defined
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Lookup Table Type
        /// </summary>
        [EnumMember]
        Lookup = 6,

        /// <summary>
        /// UOM Table Type
        /// </summary>
        [EnumMember]
        UOM = 7,

        /// <summary>
        /// List Table Type
        /// </summary>
        [EnumMember]
        List = 8,

        /// <summary>
        /// Complex Table Type
        /// </summary>
        [EnumMember]
        Complex = 9,

        /// <summary>
        /// Locale Lookup
        /// </summary>
        [EnumMember]
        LocaleLookup = 10
    }

    /// <summary>
    /// Defines Replication type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ReplicationType
    {
        /// <summary>
        /// Include Replication type
        /// </summary>
        [EnumMember]
        Include = 6,

        /// <summary>
        /// Exclude Replication type
        /// </summary>
        [EnumMember]
        Exclude = 7,

        /// <summary>
        /// Remove Replication type
        /// </summary>
        [EnumMember]
        Remove = 8,

        /// <summary>
        /// Replication type is not defined
        /// </summary>
        [EnumMember]
        Unknown = 10
    }

    /// <summary>
    /// Defines Constraint Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ConstraintType
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Default Value
        /// </summary>
        [EnumMember]
        DefaultValue = 1,

        /// <summary>
        /// Unique
        /// </summary>
        [EnumMember]
        Unique = 2,

        /// <summary>
        /// Check
        /// </summary>
        [EnumMember]
        Check = 3,

        /// <summary>
        /// ForeignKey
        /// </summary>
        [EnumMember]
        ForeignKey = 4
    }

    /// <summary>
    /// Defines the conditional operators
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ConditionalOperator
    {
        /// <summary>
        /// Represents the default operator
        /// </summary>
        [EnumMember]
        NONE = 0,

        /// <summary>
        /// Represents the AND operator
        /// </summary>
        [EnumMember]
        AND = 1,

        /// <summary>
        /// Represents the OR operator
        /// </summary>
        [EnumMember]
        OR = 2
    }

    /// <summary>
    /// Defines lookup search operators
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LookupSearchOperatorEnum
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// '=' search operator
        /// </summary>
        [EnumMember]
        EqualTo = 1,

        /// <summary>
        /// 'Contains' search operator
        /// </summary>
        [EnumMember]
        Contains = 2,

        /// <summary>
        /// 'NotContains' search operator
        /// </summary>
        [EnumMember]
        NotContains = 3
    }

    /// <summary>
    /// Defines search operators
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SearchOperator
    {
        /// <summary>
        /// '=' search operator
        /// </summary>
        [EnumMember]
        EqualTo = 1,

        /// <summary>
        /// 'Contains' search operator
        /// </summary>
        [EnumMember]
        Contains = 2,

        /// <summary>
        /// 'NotContains' search operator
        /// </summary>
        [EnumMember]
        NotContains = 3,

        /// <summary>
        /// 'HasValue' search operator
        /// </summary>
        [EnumMember]
        HasValue = 4,

        /// <summary>
        /// 'HasNoValue' search operator
        /// </summary>
        [EnumMember]
        HasNoValue = 5,

        /// <summary>
        /// 'In' search operator
        /// </summary>
        [EnumMember]
        In = 6,

        /// <summary>
        /// 'In' search operator
        /// </summary>
        [EnumMember]
        NotIn = 7,

        /// <summary>
        /// '&lt;' search operator
        /// </summary>
        [EnumMember]
        LessThan = 8,

        /// <summary>
        /// '&gt;' search operator
        /// </summary>
        [EnumMember]
        GreaterThan = 9,

        /// <summary>
        /// '&gt;=' search operator
        /// </summary>
        [EnumMember]
        GreaterThanOrEqualTo = 10,

        /// <summary>
        /// '&lt;=' search operator
        /// </summary>
        [EnumMember]
        LessThanOrEqualTo = 11,

        /// <summary>
        /// 'like' search operator
        /// </summary>
        [EnumMember]
        Like = 12,

        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 13,

        /// <summary>
        /// Path contains
        /// </summary>
        [EnumMember]
        SubsetOf = 14,
    }

    /// <summary>
    /// Defines the type of lookup value.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LookupValueFilterType
    {
        /// <summary>
        /// Lookup master. This will return all columns in a lookup table
        /// </summary>
        [EnumMember]
        LookupMaster = 0,

        /// <summary>
        /// Returns lookup attribute columns based on mapping with given attribute
        /// </summary>
        [EnumMember]
        LookupAttribute = 1
    }

    /// <summary>
    /// Defines the filtering type of lookup tables.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LookupTableFilterType
    {
        /// <summary>
        /// All. This will return all lookup tables
        /// </summary>
        [EnumMember]
        All = 0,

        /// <summary>
        /// LookupWithUniqueColumn. Returns lookup tables having at least one unique column
        /// </summary>
        [EnumMember]
        LookupWithUniqueColumn = 1
    }

    /// <summary>
    /// Defines relationship direction
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RelationshipDirection
    {
        /// <summary>
        /// Represents enum value for None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Represents Down direction used for 'Parent-Child', 'Master-Slave' relationships
        /// </summary>
        [EnumMember]
        Down = 1,

        /// <summary>
        /// Represents Up direction used for 'Parent-Child' relationships
        /// </summary>
        [EnumMember]
        Up = 2,

        /// <summary>
        /// Represents Parallel used for 'Siblings' relationships
        /// </summary>
        [EnumMember]
        Parallel = 3,

        /// <summary>
        /// Represents enum value for non-deterministic relationship direction
        /// </summary>
        [EnumMember]
        Unknown = 4
    }

    /// <summary>
    /// Defines the operation for dimension attributes of a entity hierarchy level
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DimensionOperation : int
    {
        /// <summary>
        /// Cartesian(multiplication) operation between values of various dimension attributes
        /// </summary>
        [EnumMember]
        CARTESIAN = 0,

        /// <summary>
        /// Union(append) operation between values of various dimension attributes
        /// </summary>
        [EnumMember]
        UNION = 1,

        /// <summary>
        /// Custom operation done by a business rule
        /// </summary>
        [EnumMember]
        BUSINESS_RULE = 2
    }

    /// <summary>
    /// Defines the System Attributes and their IDs
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SystemAttributes : int
    {
        /// <summary>
        /// Used to mark an entity as excluded. It is set by entity hierarchy module
        /// </summary>
        [EnumMember]
        Excluded = 88,

        /// <summary>
        /// Indicates Id of the DQM DataQualityScore attribute
        /// </summary>
        [EnumMember]
        DataQualityScore = 101,

        /// <summary>
        /// Indicates Id of the DQM DataQualityStatus attribute
        /// </summary>
        [EnumMember]
        DataQualityStatus = 102,

        /// <summary>
        /// Indicates Id of the CreateDateTime attribute
        /// </summary>
        [EnumMember]
        CreateDateTime = 103,

        /// <summary>
        /// Indicates Id of the CreateUser attribute
        /// </summary>
        [EnumMember]
        CreateUser = 104,

        /// <summary>
        /// Indicates Id of the CreateProgram attribute
        /// </summary>
        [EnumMember]
        CreateProgram = 105,

        /// <summary>
        /// Indicates Id of the LastUpdateDateTime attribute
        /// </summary>
        [EnumMember]
        LastUpdateDateTime = 106,

        /// <summary>
        /// Indicates Id of the LastUpdateUser attribute
        /// </summary>
        [EnumMember]
        LastUpdateUser = 107,

        /// <summary>
        /// Indicates Id of the LastUpdateProgram attribute
        /// </summary>
        [EnumMember]
        LastUpdateProgram = 108,

        /// <summary>
        /// Indicates Id of the EntityIdentifier1 attribute
        /// </summary>
        [EnumMember]
        EntityIdentifier1 = 109,

        /// <summary>
        /// Indicates Id of the EntityIdentifier2 attribute
        /// </summary>
        [EnumMember]
        EntityIdentifier2 = 110,

        /// <summary>
        /// Indicates Id of the EntityIdentifier3 attribute
        /// </summary>
        [EnumMember]
        EntityIdentifier3 = 111,

        /// <summary>
        /// Indicates Id of the EntityIdentifier4 attribute
        /// </summary>
        [EnumMember]
        EntityIdentifier4 = 112,

        /// <summary>
        /// Indicates Id of the EntityIdentifier5 attribute
        /// </summary>
        [EnumMember]
        EntityIdentifier5 = 113,

        /// <summary>
        /// Indicates Id of the Entity Self Valid  attribute
        /// </summary>
        [EnumMember]
        EntitySelfValid = 114,

        /// <summary>
        /// Indicates Id of the Entity Meta Data Valid  attribute
        /// </summary>
        [Description("Medatata")]
        [EnumMember]
        EntityMetaDataValid = 115,

        /// <summary>
        /// Indicates Id of the Entity Common Attributes Valid  attribute
        /// </summary>
        [Description("Common Attributes")]
        [EnumMember]
        EntityCommonAttributesValid = 116,

        /// <summary>
        /// Indicates Id of the Entity Category Attributes Valid  attribute
        /// </summary>
        [Description("Category Attributes")]
        [EnumMember]
        EntityCategoryAttributesValid = 117,

        /// <summary>
        /// Indicates Id of the Entity Relationships Valid attribute
        /// </summary>
        [Description("Relationships")]
        [EnumMember]
        EntityRelationshipsValid = 118,

        /// <summary>
        /// Indicates Id of the Entity Variant Valid attribute
        /// </summary>
        [Description("Variants")]
        [EnumMember]
        EntityVariantValid = 119,

        /// <summary>
        /// Indicates Id of the Entity Extension Valid attribute
        /// </summary>
        [Description("Extensions")]
        [EnumMember]
        EntityExtensionsValid = 120,

        /// <summary>
        /// Indicates Id of the Entity Reapproval Attribute
        /// </summary>
        [Description("Entity ReApproval")]
        [EnumMember]
        EntityNeedsReApproval = 121,

        /// <summary>
        /// Indicates Id of the Lifecycle Status Attribute
        /// </summary>
        [Description("Lifecycle Status")]
        [EnumMember]
        LifecycleStatus = 122
    }

    /// <summary>
    /// Defines Attribute Validation state value  
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ValidityStateValue
    {
        /// <summary>
        /// Validation state is unknown. 
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Unknown when validation state has not checked yet. 
        /// </summary>
        [EnumMember]
        [Description("Not checked")]
        NotChecked = 1,

        /// <summary>
        /// Valid  when validation state is true 
        /// </summary>
        [EnumMember]
        [Description("Valid")]
        Valid = 2,

        /// <summary>
        /// NotValid  when validation state is false 
        /// </summary>
        [EnumMember]
        [Description("Invalid")]
        InValid = 3
    }

    /// <summary>
    /// Defines processing is locked by which process operation
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LockType
    {
        /// <summary>
        /// Indicates Unknown lock type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Indicates processing is locked by entity family process operation
        /// </summary>
        [Description("EntityFamily")]
        [EnumMember]
        EntityFamily = 1,

        /// <summary>
        /// Indicates processing is locked by promote process operation
        /// </summary>
        [Description("Promote")]
        [EnumMember]
        Promote = 2,

        /// <summary>
        /// Indicates processing is locked by revalidate operation
        /// </summary>
        [Description("Revalidate")]
        [EnumMember]
        Revalidate = 3
    }

    /// <summary>
    /// Defines Attribute Panel Item Source
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributePanelItemSource
    {
        /// <summary>
        /// Unknown Attribute Panel Item Source
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// CommonAttribute Attribute Panel Item Source
        /// </summary>
        [EnumMember]
        CommonAttr = 1,

        /// <summary>
        /// AutoGen Attribute Panel Item Source
        /// </summary>
        [EnumMember]
        AutoGen = 2
    }

    /// <summary>
    /// Defines View Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ViewType
    {
        /// <summary>
        /// Unknown View Type
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// StateView Type
        /// </summary>
        [EnumMember]
        [Obsolete]
        StateView = 1,

        /// <summary>
        /// MyView Type
        /// </summary>
        [EnumMember]
        MyView = 2
    }

    /// <summary>
    /// Security User type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SecurityUserType
    {
        /// <summary>
        /// Unknown User Type
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Internal user
        /// </summary>
        [EnumMember]
        Internal = 1,
        /// <summary>
        /// Supplier user
        /// </summary>
        [EnumMember]
        Supplier = 2,

        /// <summary>
        /// Manufacturer user
        /// </summary>
        [EnumMember]
        Manufacturer = 3,

        /// <summary>
        /// Customer user
        /// </summary>
        [EnumMember]
        Customer = 4
    }

    /// <summary>
    /// Data Display Mode
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataDisplayMode
    {
        /// <summary>
        /// Unknown Data Display Mode
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// GridView Data Display Mode
        /// </summary>
        [EnumMember]
        GridView = 1,

        /// <summary>
        /// FormView Data Display Mode
        /// </summary>
        [EnumMember]
        FormView = 2
    }

    /// <summary>
    /// Defines the various sort orders
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SortOrder
    {
        /// <summary>
        /// Indicates none of SortOrder specified.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Ascending Sort order
        /// </summary>
        [EnumMember]
        Ascending = 1,

        /// <summary>
        /// Descending Sort order
        /// </summary>
        [EnumMember]
        Descending = 2
    }

    #endregion

    #region MDM Application, Modules and Publisher

    /// <summary>
    /// Defines the various System within MDMCenter
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMCenterSystem
    {
        /// <summary>
        /// Web
        /// </summary>
        [EnumMember]
        Web = 1,

        /// <summary>
        /// Job Service
        /// </summary>
        [EnumMember]
        JobService = 2,

        /// <summary>
        /// Web Server
        /// </summary>
        [EnumMember]
        WebService = 3,

        /// <summary>
        /// WCF Server
        /// </summary>
        [EnumMember]
        WcfService = 4,

        /// <summary>
        /// Workflow Service
        /// </summary>
        [EnumMember]
        WorkFlowService = 5,

        /// <summary>
        /// Business Rule Service
        /// </summary>
        [EnumMember]
        BusinessRuleService = 6
    }

    /// <summary>
    /// Enum for different MDM Publishers(Combination of MDMApplication + MDM Module enums)
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMPublisher
    {
        /// <summary>
        /// Unknown Publisher
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// UIProcess
        /// </summary>
        [EnumMember]
        MDM_UIProcess = 1,

        /// <summary>
        /// Import
        /// </summary>
        [EnumMember]
        MDM_Import = 2,

        /// <summary>
        /// Export
        /// </summary>
        [EnumMember]
        MDM_Export = 3,

        /// <summary>
        /// UI Process from Vendor Portal
        /// </summary>
        [EnumMember]
        VendorPortal_UIProcess = 4,

        /// <summary>
        /// Import from Vendor Portal
        /// </summary>
        [EnumMember]
        VendorPortal_Import = 5,

        /// <summary>
        /// Export from Vendor Portal
        /// </summary>
        [EnumMember]
        VendorPortal_Export = 6,

        /// <summary>
        /// UI Process from MAM
        /// </summary>
        [EnumMember]
        MAM_UIProcess = 7,

        /// <summary>
        /// Import from MAM
        /// </summary>
        [EnumMember]
        MAM_Import = 8,

        /// <summary>
        /// Export from MAM
        /// </summary>
        [EnumMember]
        MAM_Export = 9
    }

    /// <summary>
    /// Enum for different Applications of MDMCenter.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMCenterApplication
    {
        /// <summary>
        /// MDMCenter - Core 
        /// </summary>
        [EnumMember]
        MDMCenter = 0,

        /// <summary>
        /// Product Information Manager as MDMCenter's application
        /// </summary>
        [EnumMember]
        PIM = 1,

        /// <summary>
        /// Job service in PIM as MDMCenter's application
        /// </summary>
        [EnumMember]
        JobService = 2,

        /// <summary>
        /// Vendor portal as MDMCenter application
        /// </summary>
        [EnumMember]
        VendorPortal = 3,

        /// <summary>
        /// MAM as MDMCenter application
        /// </summary>
        [EnumMember]
        MAM = 4,

        /// <summary>
        /// Windows workflow as application
        /// </summary>
        [EnumMember]
        WindowsWorkflow = 5,

        /// <summary>
        /// Data Quality Management as MDMCenter application
        /// </summary>
        [EnumMember]
        DataQualityManagement = 6,

        /// <summary>
        /// Jigsaw as MDMCenter Application
        /// </summary>
        [EnumMember]
        Jigsaw = 8
    }

    /// <summary>
    /// Enum for different modules in MDMSystem.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMCenterModules
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Entity CRUM
        /// </summary>
        [EnumMember]
        Entity = 1,
        /// <summary>
        /// Entity search
        /// </summary>
        [EnumMember]
        Search = 2,
        /// <summary>
        /// Import
        /// </summary>
        [EnumMember]
        Import = 3,
        /// <summary>
        /// Export
        /// </summary>
        [EnumMember]
        Export = 4,
        /// <summary>
        /// Staging
        /// </summary>
        [EnumMember]
        Staging = 5,
        /// <summary>
        /// PDR
        /// </summary>
        [EnumMember]
        PDR = 6,
        /// <summary>
        /// DQM
        /// </summary>
        [EnumMember]
        DQM = 7,
        /// <summary>
        /// DQM
        /// </summary>
        [EnumMember]
        Promote = 8,
        /// <summary>
        /// Data validation 
        /// </summary>
        [EnumMember]
        Validation = 9,
        /// <summary>
        /// Attribute import
        /// </summary>
        [EnumMember]
        AttributeImport = 10,
        /// <summary>
        /// Category attribute mapping
        /// </summary>
        [EnumMember]
        CategoryMapping = 11,
        /// <summary>
        /// Taxonomy import
        /// </summary>
        [EnumMember]
        TaxonomyImport = 12,
        /// <summary>
        /// Attribute export
        /// </summary>
        [EnumMember]
        AttributeExport = 13,
        /// <summary>
        /// Attribute matching
        /// </summary>
        [EnumMember]
        AttributeMatching = 14,
        /// <summary>
        /// Attribute extraction
        /// </summary>
        [EnumMember]
        AttributeExtraction = 15,
        /// <summary>
        /// Attribute Generation TODO: What does AttributeGenerationMC mean?
        /// </summary>
        [EnumMember]
        AttributeGenerationMC = 16,
        /// <summary>
        /// Bulk operation 
        /// </summary>
        [EnumMember]
        BulkOperation = 17,
        /// <summary>
        /// Attribute Generation
        /// </summary>
        [EnumMember]
        AttributeGeneration = 18,
        /// <summary>
        /// Synchronization TODO : What is Synchronization  job?
        /// </summary>
        [EnumMember]
        Synchronization = 19,
        /// <summary>
        /// UIProcess
        /// </summary>
        [EnumMember]
        UIProcess = 20,
        /// <summary>
        /// Denorm
        /// </summary>
        [EnumMember]
        Denorm = 21,
        /// <summary>
        /// SubscriberUIProcess
        /// </summary>
        [EnumMember]
        SubscriberUIProcess = 22,
        /// <summary>
        /// MDM Advance Workflow
        /// </summary>
        [EnumMember]
        MDMAdvanceWorkflow = 23,
        /// <summary>
        /// MSWorkflowFoundation
        /// </summary>
        [EnumMember]
        WindowsWorkflow = 24,
        /// <summary>
        /// Distributor
        /// </summary>
        [EnumMember]
        Distributor = 25,
        /// <summary>
        /// Monitoring service
        /// </summary>
        [EnumMember]
        Monitoring = 26,
        /// <summary>
        /// Security
        /// </summary>
        [EnumMember]
        Security = 27,
        /// <summary>
        /// Modeling
        /// </summary>
        [EnumMember]
        Modeling = 28,
        /// <summary>
        /// EntityExport
        /// </summary>
        [EnumMember]
        EntityExport = 29,
        /// <summary>
        /// Integration
        /// </summary>
        [EnumMember]
        Integration = 30,

        /// <summary>
        /// Represents the lookup export module
        /// </summary>
        [EnumMember]
        LookupExport = 31,

        /// <summary>
        /// Represents the translation  module
        /// </summary>
        [EnumMember]
        TMSConnector = 32,

        /// <summary>
        /// Represents the data model import module
        /// </summary>
        [EnumMember]
        DataModelImport = 33,

        /// <summary>
        /// Represents instrumentation module for Logging/Tracing/Diagnostics/Exceptionhandling
        /// </summary>
        [EnumMember]
        Instrumentation = 34,

        /// <summary>
        /// Represents configuration module for MDMCenter
        /// </summary>
        [EnumMember]
        Configuration = 35,

        /// <summary>
        /// Represents the data model export module
        /// </summary>
        [EnumMember]
        DataModelExport = 36,

        /// <summary>
        /// Represent application configuration module for custom application config
        /// </summary>
        [EnumMember]
        CustomApplicationConfig = 62,

        /// <summary>
        /// Represent DDG export module
        /// </summary>
        [EnumMember]
        DDGExport = 63,

        /// <summary>
        /// Represent DDG import module
        /// </summary>
        [EnumMember]
        DDGImport = 64,

        /// <summary>
        /// Represent DDG module
        /// </summary>
        [EnumMember]
        DDG = 65,

        /// <summary>
        /// Represent Jigsaw Integration Module
        /// </summary>
        [EnumMember]
        JigsawIntegration = 66,

        /// <summary>
        /// Represent re-validate Module
        /// </summary>
        [EnumMember]
        Revalidate = 67
    }

    /// <summary>
    /// Enum for actions on a module.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMCenterModuleAction
    {
        /// <summary>
        /// Action is not known
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Create action
        /// </summary>
        [EnumMember]
        Create = 1,
        /// <summary>
        /// Update action
        /// </summary>
        [EnumMember]
        Update = 2,
        /// <summary>
        /// Delete action
        /// </summary>
        [EnumMember]
        Delete = 3,
        /// <summary>
        /// Read action 
        /// </summary>
        [EnumMember]
        Read = 4,
        /// <summary>
        /// Search
        /// </summary>
        [EnumMember]
        Search = 5,
        /// <summary>
        /// Execute (Example: import, export)
        /// </summary>
        [EnumMember]
        Execute = 6
    }

    /// <summary>
    /// Enum various trace sources of MDM
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMTraceSource
    {
        /// <summary>
        /// General. Used for any general trace.
        /// </summary>
        [EnumMember]
        General = 0,

        /// <summary>
        /// Application Level Traces
        /// </summary>
        [EnumMember]
        Application = 1,

        /// <summary>
        /// DataModel
        /// </summary>
        [EnumMember]
        DataModel = 2, //Includes knowledge management also

        /// <summary>
        /// Entity
        /// </summary>
        [EnumMember]
        Entity = 3,

        /// <summary>
        /// EntityGet
        /// </summary>
        [EnumMember]
        EntityGet = 4,

        /// <summary>
        /// EntityProcess
        /// </summary>
        [EnumMember]
        EntityProcess = 5,

        /// <summary>
        /// AttributeModel
        /// </summary>
        [EnumMember]
        AttributeModel = 6,

        /// <summary>
        /// AttributeModelGet
        /// </summary>
        [EnumMember]
        AttributeModelGet = 7,

        /// <summary>
        /// AttributeModelProcess
        /// </summary>
        [EnumMember]
        AttributeModelProcess = 8,

        /// <summary>
        /// Attribute
        /// </summary>
        [EnumMember]
        Attribute = 9,

        /// <summary>
        /// AttributeGet
        /// </summary>
        [EnumMember]
        AttributeGet = 10,

        /// <summary>
        /// AttributeProcess
        /// </summary>
        [EnumMember]
        AttributeProcess = 11,

        /// <summary>
        /// Relationship
        /// </summary>
        [EnumMember]
        Relationship = 12,

        /// <summary>
        /// RelationshipGet
        /// </summary>
        [EnumMember]
        RelationshipGet = 13,

        /// <summary>
        /// RelationshipProcess
        /// </summary>
        [EnumMember]
        RelationshipProcess = 14,

        /// <summary>
        /// ExtensionRelationship
        /// </summary>
        [EnumMember]
        ExtensionRelationship = 15,

        /// <summary>
        /// ExtensionRelationshipGet
        /// </summary>
        [EnumMember]
        ExtensionRelationshipGet = 16,

        /// <summary>
        /// ExtensionRelationshipProcess
        /// </summary>
        [EnumMember]
        ExtensionRelationshipProcess = 17,

        /// <summary>
        /// HierarchyRelationship
        /// </summary>
        [EnumMember]
        HierarchyRelationship = 18,

        /// <summary>
        /// HierarchyRelationshipGet
        /// </summary>
        [EnumMember]
        HierarchyRelationshipGet = 19,

        /// <summary>
        /// HierarchyRelationshipProcess
        /// </summary>
        [EnumMember]
        HierarchyRelationshipProcess = 20,

        /// <summary>
        /// EntitySearch
        /// </summary>
        [EnumMember]
        EntitySearch = 21,

        /// <summary>
        /// APIFramework
        /// </summary>
        [EnumMember]
        APIFramework = 22,

        /// <summary>
        /// AdvancedWorkflow
        /// </summary>
        [EnumMember]
        AdvancedWorkflow = 23,

        /// <summary>
        /// STF
        /// </summary>
        [EnumMember]
        STF = 24,

        /// <summary>
        /// SecurityService
        /// </summary>
        [EnumMember]
        SecurityService = 25,

        /// <summary>
        /// CacheManager
        /// </summary>
        [EnumMember]
        CacheManager = 26,

        /// <summary>
        /// BusinessRulesEngine
        /// </summary>
        [EnumMember]
        BusinessRulesEngine = 27,

        /// <summary>
        /// DenormProcess
        /// </summary>
        [EnumMember]
        DenormProcess = 28,

        /// <summary>
        /// ParallelProcessingEngine
        /// </summary>
        [EnumMember]
        ParallelProcessingEngine = 29,

        /// <summary>
        /// JobService
        /// </summary>
        [EnumMember]
        JobService = 30,

        /// <summary>
        /// Imports
        /// </summary>
        [EnumMember]
        Imports = 31,

        /// <summary>
        /// Exports
        /// </summary>
        [EnumMember]
        Exports = 32,

        /// <summary>
        /// UI
        /// </summary>
        [EnumMember]
        UI = 33,

        /// <summary>
        /// Configuration
        /// </summary>
        [EnumMember]
        Configuration = 34,

        /// <summary>
        /// Lookup
        /// </summary>
        [EnumMember]
        Lookup = 35,

        /// <summary>
        /// LookupGet
        /// </summary>
        [EnumMember]
        LookupGet = 36,

        /// <summary>
        /// LookupProcess
        /// </summary>
        [EnumMember]
        LookupProcess = 37,

        /// <summary>
        /// EntityHierarchy
        /// </summary>
        [EnumMember]
        EntityHierarchy = 38,

        /// <summary>
        /// EntityHierarchyGet
        /// </summary>
        [EnumMember]
        EntityHierarchyGet = 39,

        /// <summary>
        /// EntityHierarchyProcess
        /// </summary>
        [EnumMember]
        EntityHierarchyProcess = 40,

        /// <summary>
        /// MergeCopy
        /// </summary>
        [EnumMember]
        MergeCopy = 41,

        /// <summary>
        /// EntityExportProfileGet
        /// </summary>
        [EnumMember]
        EntityExportProfileGet = 42,

        /// <summary>
        /// EntityExportProfileProcess
        /// </summary>
        [EnumMember]
        EntityExportProfileProcess = 43,

        /// <summary>
        /// Data Quality Management
        /// </summary>
        [EnumMember]
        DQM = 44,

        /// <summary>
        /// DQM Validation Process
        /// </summary>
        [EnumMember]
        DQMValidationProcess = 45,

        /// <summary>
        /// DataQualityIndicator Calculation Process
        /// </summary>
        [EnumMember]
        DataQualityIndicatorCalculationProcess = 46,

        /// <summary>
        /// Entity Cache Process
        /// </summary>
        [EnumMember]
        EntityCacheProcess = 47,

        /// <summary>
        /// Integration
        /// </summary>
        [EnumMember]
        Integration = 48,

        /// <summary>
        /// DQM Normalization Process
        /// </summary>
        [EnumMember]
        DQMNormalizationProcess = 49,

        /// <summary>
        /// DQM Matching Process
        /// </summary>
        [EnumMember]
        DQMMatchingProcess = 50,

        /// <summary>
        /// DQM Merging Process
        /// </summary>
        [EnumMember]
        DQMMergingProcess = 51,

        /// <summary>
        /// Lookup Import
        /// </summary>
        [EnumMember]
        LookupImport = 52,

        /// <summary>
        /// Lookup Export
        /// </summary>
        [EnumMember]
        LookupExport = 53,

        /// <summary>
        /// Job Polling
        /// </summary>
        [EnumMember]
        JobPolling = 54,

        /// <summary>
        /// Category Get
        /// </summary>
        [EnumMember]
        CategoryGet = 55,

        /// <summary>
        /// Category Process
        /// </summary>
        [EnumMember]
        CategoryProcess = 56,

        /// <summary>
        /// Translation Export Profile Get
        /// </summary>
        [EnumMember]
        TranslationExportProfileGet = 57,

        /// <summary>
        /// Translation Management System Connector
        /// </summary>
        [EnumMember]
        TMSConnector = 58,

        /// <summary>
        /// DQM MergePlanning Process
        /// </summary>
        [EnumMember]
        DQMMergePlanningProcess = 59,

        /// <summary>
        /// DataModel Import
        /// </summary>
        [EnumMember]
        DataModelImport = 60,

        /// <summary>
        /// CategorySearch
        /// </summary>
        [EnumMember]
        CategorySearch = 61,

        /// <summary>
        /// DataModel Import
        /// </summary>
        [EnumMember]
        DataModelExport = 62,

        /// <summary>
        /// Translation Management System Connector
        /// </summary>
        [EnumMember]
        CustomCode = 63,

        /// <summary>
        /// Instrumentation module
        /// </summary>
        [EnumMember]
        Instrumentation = 64,

        /// <summary>
        /// DiagnosticService
        /// </summary>
        [EnumMember]
        DiagnosticService = 65,

        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        UnKnown = 66,

        /// <summary>
        /// StronglyTypedEntity
        /// </summary>
        [EnumMember]
        StronglyTypedEntity = 67,

        /// <summary>
        /// DataAuditReport
        /// </summary>
        [EnumMember]
        DataAuditReport = 68,

        /// <summary>
        /// Bulk edit traces
        /// </summary>
        [EnumMember]
        AttributeSelection = 69,

        /// <summary>
        /// Attribute selection (Export, Bulk edit and etc) traces
        /// </summary>
        [EnumMember]
        BulkEdit = 70,

        /// <summary>
        /// DDG
        /// </summary>
        [EnumMember]
        DDG = 71,

        /// <summary>
        /// DDG Get
        /// </summary>
        [EnumMember]
        DDGGet = 72,

        /// <summary>
        /// DDG Process
        /// </summary>
        [EnumMember]
        DDGProcess = 73,

        /// <summary>
        /// DDG Export Traces
        /// </summary>
        [EnumMember]
        DDGExport = 74,

        /// <summary>
        /// DDG Import Traces
        /// </summary>
        [EnumMember]
        DDGImport = 75,

        /// <summary>
        /// Jigsaw Integration
        /// </summary>
        [EnumMember]
        JigsawIntegration = 76,

        /// <summary>
        /// Entity Family Process
        /// </summary>
        [EnumMember]
        EntityFamilyProcess = 77,

        /// <summary>
        /// DataModelExportProfileGet
        /// </summary>
        [EnumMember]
        DataModelExportProfileGet = 78,

        /// <summary>
        /// DataModelExportProfileProcess
        /// </summary>
        [EnumMember]
        DataModelExportProfileProcess = 79
    }

    /// <summary>
    /// Specifies all the feature config disable level
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum FeatureConfigDisableLevel
    {
        /// <summary>
        /// Represents the module is not disable
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Represents the module is disable
        /// </summary>
        [EnumMember]
        Module = 1,

        /// <summary>
        /// Represents the version is disable
        /// </summary>
        [EnumMember]
        Version = 2,
    }


    #endregion

    #region JobService enums

    /// <summary>
    /// JobAction used for UserAction of JobService
    /// Indicates type of UserAction.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobAction
    {
        /// <summary>
        /// Indicates Pause
        /// </summary>
        [EnumMember]
        Pause = 0,

        /// <summary>
        /// Indicates Continue
        /// </summary>
        [EnumMember]
        Continue = 1,

        /// <summary>
        /// Indicates Abort
        /// </summary>
        [EnumMember]
        Abort = 2,

        /// <summary>
        /// Indicates Retry
        /// </summary>
        [EnumMember]
        Retry = 3,

        /// <summary>
        /// Indicates Delete
        /// </summary>
        [EnumMember]
        Delete = 4,

        /// <summary>
        /// Indicates None
        /// </summary>
        [EnumMember]
        None = 5
    }

    /// <summary>
    /// JobStatus used for JobService
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobStatus
    {
        /// <summary>
        /// Indicates Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        UnKnown = 0,

        /// <summary>
        /// Indicates Stopping
        /// </summary>
        [EnumMember]
        [Description("Stopping")]
        Stopping = 1,

        /// <summary>
        /// Indicates Paused
        /// </summary>
        [EnumMember]
        [Description("Paused")]
        Paused = 2,

        /// <summary>
        /// Indicates Running
        /// </summary>
        [EnumMember]
        [Description("Running")]
        Running = 3,

        /// <summary>
        /// Indicates COmpleted
        /// </summary>
        [EnumMember]
        [Description("Completed")]
        Completed = 4,

        /// <summary>
        /// Indicated Aborted
        /// </summary>
        [EnumMember]
        [Description("Aborted")]
        Aborted = 5,

        /// <summary>
        /// Indicates Deleted
        /// </summary>
        [EnumMember]
        [Description("Deleted")]
        Deleted = 6,

        /// <summary>
        /// Indicates Pending
        /// </summary>
        [EnumMember]
        [Description("Pending")]
        Pending = 7,

        /// <summary>
        /// Indicates Completed With Errors
        /// </summary>
        [EnumMember]
        [Description("Completed With Errors")]
        CompletedWithErrors = 8,

        /// <summary>
        /// Indicates Queued
        /// </summary>
        [EnumMember]
        [Description("Queued")]
        Queued = 9,

        /// <summary>
        /// Indicates COmpleted with Warnings
        /// </summary>
        [EnumMember]
        [Description("Completed With Warnings")]
        CompletedWithWarnings = 10,

        /// <summary>
        /// Indicates Completed With Warnings And Errors
        /// </summary>
        [EnumMember]
        [Description("Completed With Warnings And Errors")]
        CompletedWithWarningsAndErrors = 11,

        /// <summary>
        /// Indicates Ignored
        /// </summary>
        [EnumMember]
        [Description("Ignored")]
        Ignored = 12
    }

    /// <summary>
    /// Avaialable Job Types in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobType
    {
        /// <summary>
        /// Indicates UnKnown
        /// </summary>
        [EnumMember]
        [Description("All")]
        UnKnown = 0,

        /// <summary>
        /// Indicates EntityImport
        /// </summary>
        [EnumMember]
        [Description("Entity Import")]
        EntityImport = 1,

        /// <summary>
        /// Indicates EntityExport
        /// </summary>
        [EnumMember]
        [Description("Catalog Export")]
        EntityExport = 2,

        /// <summary>
        /// Indicates AttributeModel Import
        /// </summary>
        [EnumMember]
        [Description("Attribute Model Import")]
        AttributeModelImport = 3,

        /// <summary>
        /// Indicates DataModel Mapping Import
        /// </summary>
        [EnumMember]
        [Description("Data Model Mapping Import")]
        DataModelMappingImport = 4,

        /// <summary>
        /// Indicates Attribute Matching
        /// </summary>
        [EnumMember]
        [Description("Attribute Matching")]
        AttributeMatching = 5,

        /// <summary>
        /// Indicates BulkOperation
        /// </summary>
        [EnumMember]
        [Description("Bulk Operation")]
        BulkOperation = 7,

        /// <summary>
        /// Indicates Import
        /// </summary>
        [EnumMember]
        [Description("Import")]
        Import = 8,

        /// <summary>
        /// Indicates Lookup Import
        /// </summary>
        [EnumMember]
        [Description("Lookup Import")]
        LookupImport = 12,

        /// <summary>
        /// Indicates Custom
        /// </summary>
        [EnumMember]
        [Description("Custom")]
        Custom = 13,

        /// <summary>
        /// Indicates DQM jobs
        /// </summary>
        [EnumMember]
        [Description("Data Quality Management")]
        DataQualityManagement = 14,

        /// <summary>
        ///Represent the lookup export job type.
        /// </summary>
        [EnumMember]
        [Description("Lookup Export")]
        LookupExport = 15,

        /// <summary>
        ///Represent the DataModel Import job type
        /// </summary>
        [EnumMember]
        [Description("Data Model Import")]
        DataModelImport = 16,

        /// <summary>
        /// Represents Excel Diagnostic Export job type
        /// </summary>
        [EnumMember]
        [Description("Diagnostic Report Export")]
        DiagnosticReportExport = 17,

        /// <summary>
        /// Represent the DDG Import Job Type
        /// </summary>
        [EnumMember]
        [Description("Dynamic Data Governance Import")]
        DDGImport = 18
    }

    /// <summary>
    /// Available Job Sub Types in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobSubType
    {
        /// <summary>
        /// Indicates All
        /// </summary>
        [EnumMember]
        All = 0,

        /// <summary>
        /// Indicates Excel
        /// </summary>
        [EnumMember]
        Excel,

        /// <summary>
        /// Indicates Custom Xml
        /// </summary>
        [EnumMember]
        CustomXml,

        /// <summary>
        /// Indicates RSXml
        /// </summary>
        [EnumMember]
        RSXml,

        /// <summary>
        /// Indicates API
        /// </summary>
        [EnumMember]
        API,

        /// <summary>
        /// Indicates Csv
        /// </summary>
        [EnumMember]
        Csv,

        /// <summary>
        /// Indicates Tab
        /// </summary>
        [EnumMember]
        Tab,

        /// <summary>
        /// Indicates Data Cleansing process
        /// </summary>
        [EnumMember]
        DataCleansing,

        /// <summary>
        /// Indicates normalization process
        /// </summary>
        [EnumMember]
        Normalization,

        /// <summary>
        /// Indicates attribute extraction process
        /// </summary>
        [EnumMember]
        AttributeExtraction,

        /// <summary>
        /// Indicates Custom
        /// </summary>
        [EnumMember]
        Custom,

        /// <summary>
        /// Indicates UnKnown
        /// </summary>
        [EnumMember]
        UnKnown
    }

    /// <summary>
    /// Job Service Type indicates the role of a given job service.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobServiceType
    {
        /// <summary>
        /// Import Server
        /// </summary>
        [EnumMember]
        ImportServer = 0,

        /// <summary>
        /// Export Server
        /// </summary>
        [EnumMember]
        ExportServer = 1,

        /// <summary>
        /// All
        /// </summary>
        [EnumMember]
        All = 2
    }

    /// <summary>
    /// Defines how often a job should be executed.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RecurrenceFrequencyOptions
    {
        /// <summary>
        /// Daily
        /// </summary>
        [EnumMember]
        Daily = 0,

        /// <summary>
        /// Weekly
        /// </summary>
        [EnumMember]
        Weekly = 1,

        /// <summary>
        /// Monthly
        /// </summary>
        [EnumMember]
        Monthly = 2
    }

    /// <summary>
    /// Defines  the different object types for each item that is being processed by the job
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobObjectType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// LookUp
        /// </summary>
        [EnumMember]
        LookUp = 1,

        /// <summary>
        /// DataModel
        /// </summary>
        [EnumMember]
        DataModel = 2
    }

    /// <summary>
    /// Defines  the different Job Result Summary Types for each item that is being processed by the job
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobResultsReturnType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// LookUp
        /// </summary>
        [EnumMember]
        [Description("ObjectTypeSummary")]
        ObjectTypeSummary = 1,

        /// <summary>
        /// DataModel
        /// </summary>
        [EnumMember]
        [Description("ObjectTypeDetail")]
        ObjectTypeDetail = 2
    }

    /// <summary>
    /// Defines how often a job should be executed within a given day.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DailyFrequencyOptions
    {
        /// <summary>
        /// Once
        /// </summary>
        [EnumMember]
        Once = 0,

        /// <summary>
        /// Hourly
        /// </summary>
        [EnumMember]
        Hourly = 1,

        /// <summary>
        /// EveryMinute
        /// </summary>
        [EnumMember]
        EveryMinute = 2
    }

    /// <summary>
    /// Defines when within a given month a job should be executed.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MonthlyFrequencyOptions
    {
        /// <summary>
        /// Daily
        /// </summary>
        [EnumMember]
        Daily = 0,

        /// <summary>
        /// Weekly
        /// </summary>
        [EnumMember]
        Weekly = 1
    }

    #endregion

    #region Workflow enums

    /// <summary>
    /// Define how the workflow instances are to be invoked.
    /// Handle single item per workflow instance or
    /// handle bulk per workflow instance.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowInstanceRunOptions
    {
        /// <summary>
        /// Run As Multiple Instances
        /// </summary>
        [EnumMember]
        RunAsMultipleInstances = 1,

        /// <summary>
        /// Run As Single Instance
        /// </summary>
        [EnumMember]
        [Obsolete("This option has been obsoleted. Please use 'RunAsMultipleInstances' instead of this.")]
        RunAsSingleInstance = 2
    }

    /// <summary>
    /// Specify the type of assignment rule used for an activity
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AssignmentType
    {
        /// <summary>
        /// Assigns to a user specified in AssignedUsers and AssignedRoles in a round robin fashion
        /// </summary>
        [EnumMember]
        RoundRobin = 0,

        /// <summary>
        /// Assigns to a user specified in AssignedUsers and AssignedRoles having least count of pending activities
        /// </summary>
        [EnumMember]
        LeastQueueSize = 1,

        /// <summary>
        /// Queues activities in an Unassigned pool. User in AssignedUsers and AssignedRoles has to pick the activity
        /// </summary>
        [EnumMember]
        Queue = 2,

        /// <summary>
        /// Determination of assignment is based on business rule
        /// </summary>
        [EnumMember]
        BusinessRule = 3,

        /// <summary>
        /// Determination of assignment is based on Table
        /// </summary>
        [EnumMember]
        ConfigurationTable = 4
    }

    /// <summary>
    /// Defines workflow instance states
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowInstanceState
    {
        /// <summary>
        /// Workflow instance is running
        /// Comprises Started, Idle, Persisted, Resumed, Suspended, Unloaded, UnhandledException and Unsuspended
        /// </summary>
        [EnumMember]
        Running = 1,

        /// <summary>
        /// Workflow instance is aborted because of Unhandled Exception. Overridden behaviour.
        /// </summary>
        [EnumMember]
        Aborted = 2,

        /// <summary>
        /// Workflow instance is completed successfully.
        /// </summary>
        [EnumMember]
        Completed = 3,

        /// <summary>
        /// Workflow instance is terminated because of Unhandled Exception. Default behaviour.
        /// </summary>
        [EnumMember]
        Terminated = 4,

        /// <summary>
        /// Workflow instance is terminated because of Unhandled Exception. Default behaviour.
        /// </summary>
        [EnumMember]
        Started = 5,
    }

    /// <summary>
    /// Defines Activity states
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowActivityState
    {
        /// <summary>
        /// Activity is canceled because of Cancel action on instance
        /// </summary>
        [EnumMember]
        Canceled = 1,

        /// <summary>
        /// Activity is closed
        /// </summary>
        [EnumMember]
        Closed = 2,

        /// <summary>
        /// Activity is executing
        /// </summary>
        [EnumMember]
        Executing = 3,

        /// <summary>
        /// Activity is faulted because of unhandled exception within activity
        /// </summary>
        [EnumMember]
        Faulted = 4
    }

    /// <summary>
    /// this Enum is for type of node in Workflow Tree
    /// All 6 are Metadata Attribute in tb_Attribute
    /// value here shows PK_Attribute for each attribute
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowNodeType
    {
        /// <summary>
        /// Workflow 
        /// </summary>
        [EnumMember]
        Workflow = 42,

        /// <summary>
        /// Workflow State
        /// </summary>
        [EnumMember]
        WorkflowState = 67,

        /// <summary>
        /// Workflow Assignment 
        /// </summary>
        [EnumMember]
        WorkflowAssignment = 94,

        /// <summary>
        /// Windows Workflow
        /// </summary>
        [EnumMember]
        WindowsWorkflow = 91,

        /// <summary>
        /// Windows Workflow Activity
        /// </summary>
        [EnumMember]
        WindowsWorkflowActivity = 92,

        /// <summary>
        /// Windows Workflow Assignment
        /// </summary>
        [EnumMember]
        WindowsWorkflowAssignment = 93
    }

    /// <summary>
    /// Specifies the comments property for an activity action
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CommentsRequired
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Optional
        /// </summary>
        [EnumMember]
        Optional = 1,

        /// <summary>
        /// Mandatory
        /// </summary>
        [EnumMember]
        Mandatory = 2
    }

    /// <summary>
    /// Specifies type of workflow
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// No workflow
        /// </summary>
        [EnumMember]
        [LocalizationCode("114162")]
        NoWorkflow = 1,

        /// <summary>
        /// Engine based workflow
        /// </summary>
        [EnumMember]
        [LocalizationCode("114161")]
        Engine = 2,

        /// <summary>
        /// Inherit from parent based workflow
        /// </summary>
        [EnumMember]
        [LocalizationCode("114163")]
        InheritParent = 4
    }

    /// <summary>
    /// Specifys the worflow state associated of the mdm object
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WorkflowStateFamilyType
    {
        /// <summary>
        /// Specifies the workflow state applies to the object
        /// </summary>
        [EnumMember]
        Self = 0,
        /// <summary>
        /// Represent the worflow state applies of the parent object
        /// </summary>
        [EnumMember]
        Family = 1,
        /// <summary>
        /// Prepresents the workflow state applies of the family object
        /// </summary>
        [EnumMember]
        GlobalFamily = 2
    }

    #endregion

    #region Message enums
    /// <summary>
    /// Indicates the type of Locale message   class
    /// </summary>
    [DataContract]
    public enum MessageClassEnum
    {
        /// <summary>
        /// Unknown type of message
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Message for Success
        /// </summary>
        [EnumMember]
        Success = 1,

        /// <summary>
        /// Message for Error
        /// </summary>
        [EnumMember]
        Error = 2,

        /// <summary>
        /// Message giving warning
        /// </summary>
        [EnumMember]
        Warning = 3,

        /// <summary>
        /// Message giving information
        /// </summary>
        [EnumMember]
        Information = 4,

        /// <summary>
        /// Static Text Message
        /// </summary>
        [EnumMember]
        StaticText = 5,

        /// <summary>
        /// Time Span (milliseconds) Message
        /// </summary>
        [EnumMember]
        TimeSpan = 6,

        /// <summary>
        /// Activity Start Message
        /// </summary>
        [EnumMember]
        ActivityStart = 7,

        /// <summary>
        /// Activity Stop Message
        /// </summary>
        [EnumMember]
        ActivityStop = 8,

        /// <summary>
        /// A Verbose Message
        /// </summary>
        [EnumMember]
        Verbose = 9
    }

    /// <summary>
    /// Indicates the type of message
    /// </summary>
    [DataContract]
    public enum MessageType
    {
        /// <summary>
        /// Indicates information m
        /// </summary>
        [EnumMember]
        Info,

        /// <summary>
        /// Indicates Alert
        /// </summary>
        [EnumMember]
        Alert,

        /// <summary>
        /// Indicates Workflow
        /// </summary>
        [EnumMember]
        Workflow,

        /// <summary>
        /// Indicates Custom
        /// </summary>
        [EnumMember]
        Custom,

        /// <summary>
        /// Indicates Security
        /// </summary>
        [EnumMember]
        Security
    }

    /// <summary>
    /// Indicates the state of message
    /// </summary>
    [DataContract]
    public enum MessageState
    {
        /// <summary>
        /// Indicates Pending
        /// </summary>
        [EnumMember]
        Pending,

        /// <summary>
        /// Indicates Complete
        /// </summary>
        [EnumMember]
        Complete
    }

    /// <summary>
    /// Indicate the state of message processing
    /// </summary>
    [DataContract]
    public enum MessageFlag
    {
        /// <summary>
        /// Indicates Processed
        /// </summary>
        [EnumMember]
        Processed,

        /// <summary>
        /// Indicates Pending
        /// </summary>
        [EnumMember]
        Pending
    }

    /// <summary>
    /// Indicates the format of mail
    /// </summary>
    [DataContract]
    public enum MailFormat
    {
        /// <summary>
        /// Indicates HTML
        /// </summary>
        [EnumMember]
        HTML,

        /// <summary>
        /// Indicates Text
        /// </summary>
        [EnumMember]
        Text
    }

    #endregion

    #region Configuration enum

    /// <summary>
    /// Specifies all the available Event Sources
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EventSource
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// ProductCenter Event Source
        /// </summary>
        [EnumMember]
        ProductCenter = 1,
        /// <summary>
        /// AdvacedSearch Event Source
        /// </summary>
        [EnumMember]
        AdvacedSearch = 2,
        /// <summary>
        /// DQMGuidedSearch Event Source
        /// </summary>
        [EnumMember]
        DQMGuidedSearch = 3,
        /// <summary>
        /// PNMatching Event Source
        /// </summary>
        [EnumMember]
        PNMatching = 4,
        /// <summary>
        /// DescriptionMatching Event Source
        /// </summary>
        [EnumMember]
        DescriptionMatching = 5,
        /// <summary>
        /// AutoClassification Event Source
        /// </summary>
        [EnumMember]
        AutoClassification = 6,
        /// <summary>
        /// PromoteDemote Event Source
        /// </summary>
        [EnumMember]
        PromoteDemote = 7,
        /// <summary>
        /// Relationship Event Source
        /// </summary>
        [EnumMember]
        Relationship = 8,
        /// <summary>
        /// Home Event Source
        /// </summary>
        [EnumMember]
        Home = 9,
        /// <summary>
        /// Normalization Event Source
        /// </summary>
        [EnumMember]
        Normalization = 10,
        /// <summary>
        /// AttributeExtraction Event Source
        /// </summary>
        [EnumMember]
        AttributeExtraction = 11,
        /// <summary>
        /// Validation Event Source
        /// </summary>
        [EnumMember]
        Validation = 12,
        /// <summary>
        /// Application Event Source
        /// </summary>
        [EnumMember]
        NodeTypeApplications = 13,
        /// <summary>
        /// DocumentExplorer Event Source
        /// </summary>
        [EnumMember]
        DocumentExplorer = 14,
        /// <summary>
        /// ItemEdit Event Source
        /// </summary>
        [EnumMember]
        ItemEdit = 15,
        /// <summary>
        /// ChildEntity Event Source
        /// </summary>
        [EnumMember]
        ChildEntityDetail = 16,
        /// <summary>
        /// StageTransitionService Event Source
        /// </summary>
        [EnumMember]
        StageTransitionService = 17,
        /// <summary>
        /// Default Attribute Value Event Source
        /// </summary>
        [EnumMember]
        DefaultValue = 18,
        /// <summary>
        /// EntityDetail Event Source
        /// </summary>
        [EnumMember]
        EntityDetail = 19,
        /// <summary>
        /// EntityExplorer Event Source
        /// </summary>
        [EnumMember]
        EntityExplorer = 20,
        /// <summary>
        /// Meta Data Event Source
        /// </summary>
        [EnumMember]
        MetaData = 21,
        ///<summary>
        /// Entity Editor Event Source
        ///</summary>
        [EnumMember]
        EntityEditor = 22,
        ///<summary>
        /// Maintain View Event Source
        ///</summary>
        [EnumMember]
        ConfigurableAttributeGroups = 23,
        /// <summary>
        /// Rule based attribute groups
        /// </summary>
        [EnumMember]
        RuleBasedAttributeGroups = 24,
        /// <summary>
        /// Entity
        /// </summary>
        [EnumMember]
        Entity = 25,
        /// <summary>
        /// Source for Entity Common Attributes Updating event
        /// </summary>
        [EnumMember]
        EntityCommonAttributesUpdating = 28,
        /// <summary>
        /// Source for Entity Category Attributes Updating event
        /// </summary>
        [EnumMember]
        EntityCategoryAttributesUpdating = 29,
        /// <summary>
        /// Source for Entity System Attributes Updating event
        /// </summary>
        [EnumMember]
        EntitySystemAttributesUpdating = 30,
        /// <summary>
        /// Source for Entity Common Attributes Updated event
        /// </summary>
        [EnumMember]
        EntityCommonAttributesUpdated = 31,
        /// <summary>
        /// Source for Entity Category Attributes Updated event
        /// </summary>
        [EnumMember]
        EntityCategoryAttributesUpdated = 32,
        /// <summary>
        /// Source for Entity System Attributes Updated event
        /// </summary>
        [EnumMember]
        EntitySystemAttributesUpdated = 33,
        /// <summary>
        /// Publish Entity Attribute
        /// </summary>
        [EnumMember]
        EntityAttributePublish = 36,
        /// <summary>
        /// Vendor Portal Event Source
        /// </summary>
        [EnumMember]
        VendorPortal = 41,
        /// <summary>
        /// Vendor Portal Entity Explorer event source
        /// </summary>
        [EnumMember]
        VPEntityExplorer = 42,
        /// <summary>
        /// Vendor Portal Entity Editor event source
        /// </summary>
        [EnumMember]
        VPEntityEditor = 43,
        /// <summary>
        /// Vendor Portal Stage Transition Pre Process event source
        /// </summary>
        [EnumMember]
        VPStageTransitionPreProcess = 44,
        /// <summary>
        /// Vendor Portal Stage Transition Post Process event source
        /// </summary>
        [EnumMember]
        VPStageTransitionPostProcess = 45,
        /// <summary>
        /// Vendor Portal State Transition Pre Process event source
        /// </summary>
        [EnumMember]
        VPStateTransitionPreProcess = 46,
        /// <summary>
        /// Vendor Portal State Transition Post Process event source
        /// </summary>
        [EnumMember]
        VPStateTransitionPostProcess = 47,
        /// <summary>
        /// Source for Entity processing event
        /// </summary>
        [EnumMember]
        EntityProcessing = 48,
        /// <summary>
        /// Source for Entity processed event
        /// </summary>
        [EnumMember]
        EntityProcessed = 49,
        /// <summary>
        /// Source for Entity Hierarchy Relationship processing event
        /// </summary>
        [EnumMember]
        HierarchyRelationshipProcessing = 50,
        /// <summary>
        /// Source for Entity Hierarchy Relationship processed event
        /// </summary>
        [EnumMember]
        HierarchyRelationshipProcessed = 51,

        /// <summary>
        /// MDMCenter event source
        /// </summary>
        [EnumMember]
        MDMCenter = 53,
        /// <summary>
        /// Entity Hierarchy event source
        /// </summary>
        [EnumMember]
        EntityHierarchy = 54,
        /// <summary>
        /// MAM event source
        /// </summary>
        [EnumMember]
        MAM = 55,
        /// <summary>
        /// MAM Entity Explorer event source
        /// </summary>
        [EnumMember]
        MAMEntityExplorer = 56,
        /// <summary>
        /// MAM Entity Editor event source
        /// </summary>
        [EnumMember]
        MAMEntityEditor = 57,
        /// <summary>
        /// VendorPortal VPBulkLoader
        /// </summary>
        [EnumMember]
        VPBulkLoader = 77,

        /// <summary>
        /// Entity copy/merge pre event
        /// </summary>
        [EnumMember]
        EntityMerging = 80,

        /// <summary>
        /// Entity copy/merge post event
        /// </summary>
        [EnumMember]
        EntityMerged = 81,

        /// <summary>
        /// Entity transitioning for workflow
        /// </summary>
        [EnumMember]
        EntityTransitioning = 82,
        /// <summary>
        /// Entity transitioned for workflow
        /// </summary>
        [EnumMember]
        EntityTransitioned = 83,

        /// <summary>
        /// EntityAssignmentChanging
        /// </summary>
        [EnumMember]
        EntityAssignmentChanging = 85,

        /// <summary>
        /// EntityAssignmentChanged
        /// </summary>
        [EnumMember]
        EntityAssignmentChanged = 86,

        /// <summary>
        /// Configuration For EntityHierarchyPanel.
        /// </summary>
        [EnumMember]
        EntityHierarchyConfiguration = 87,

        /// <summary>
        /// Configuration For EntityDataQualityValidation.
        /// </summary>
        [EnumMember]
        EntityDataQualityValidation = 94,

        /// <summary>
        /// DataQualityManagement WebApp Event Source
        /// </summary>
        [EnumMember]
        DataQualityManagement = 95,

        /// <summary>
        /// Data Quality Management Dashboard Event Source
        /// </summary>
        [EnumMember]
        DQMDashboard = 96,

        /// <summary>
        /// MDM Complex Attribute Event Source
        /// </summary>
        [EnumMember]
        ComplexAttributeEditor = 97,

        /// <summary>
        /// VP Complex Attribute Event Source
        /// </summary>
        [EnumMember]
        VPComplexAttributeEditor = 98,

        /// <summary>
        /// MDM Entity History View Event Source
        /// </summary>
        [EnumMember]
        EntityHistoryView = 99,

        /// <summary>
        /// VP Entity History View Event Source
        /// </summary>
        [EnumMember]
        VPEntityHistoryView = 100,

        /// <summary>
        /// VP Catalog View Event Source
        /// </summary>
        [EnumMember]
        VPCatalogView = 101,

        /// <summary>
        /// EntityNormalization Event Source
        /// </summary>
        [EnumMember]
        EntityNormalization = 102,

        /// <summary>
        /// Catalog view event source.
        /// </summary>
        [EnumMember]
        CatalogView = 103,


        /// <summary>
        /// EntityValidation Event Source.
        /// </summary>
        [EnumMember]
        EntityValidation = 106,

        /// <summary>
        /// Matching suspects UI Event Source.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUI = 108,

        /// <summary>
        /// Matching Results UI Event Source
        /// </summary>
        [EnumMember]
        MatchingResultsUI = 109,

        /// <summary>
        /// MDM Hierarchical Attribute Event Source
        /// </summary>
        [EnumMember]
        HierarchicalAttributeEditor = 110,

        /// <summary>
        /// VP Hierarchical Attribute Event Source
        /// </summary>
        [EnumMember]
        VPHierarchicalAttributeEditor = 111,

        /// <summary>
        /// DDG UI Event Source
        /// </summary>
        [EnumMember]
        DDG = 112,

        /// <summary>
        /// Entity Family Detail View event source
        /// </summary>
        [EnumMember]
        EntityFamilyDetailView = 113
    }

    /// <summary>
    /// Specifies all the available MDM Events
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMEvent
    {
        /// <summary>
        /// Represents unknown.
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Represents the entity loading event.
        /// </summary>
        [EnumMember]
        EntityLoading = 35,

        /// <summary>
        /// EntityLoaded event
        /// </summary>
        [EnumMember]
        EntityLoaded = 22,

        /// <summary>
        /// Source for Entity Validation event
        /// </summary>
        [EnumMember]
        EntityValidate = 1,

        /// <summary>
        /// Entity Attributes Creating
        /// </summary>
        [EnumMember]
        EntityAttributesCreating = 59,

        /// <summary>
        /// Entity RelationShips Creating
        /// </summary>
        [EnumMember]
        EntityRelationShipsCreating = 2,

        /// <summary>
        /// Entity Hierarchy Creating
        /// </summary>
        [EnumMember]
        EntityHierarchyCreating = 3,

        /// <summary>
        /// Entity Extensions Creating
        /// </summary>
        [EnumMember]
        EntityExtensionsCreating = 4,

        /// <summary>
        /// Source for Entity Creating event
        /// </summary>
        [EnumMember]
        EntityCreating = 5,

        /// <summary>
        /// Entity Created Aync Pre event
        /// </summary>
        [EnumMember]
        EntityCreatePostProcessStarting = 29,

        /// <summary>
        /// Entity Updated Pre Event
        /// </summary>
        [EnumMember]
        EntityUpdatePostProcessStarting = 30,

        /// <summary>
        /// Entity Delete Aync Pre event
        /// </summary>
        [EnumMember]
        EntityDeletePostProcessStarting = 33,

        /// <summary>
        /// Entity Reclassifying 
        /// </summary>
        [EnumMember]
        EntityReclassifying = 10,

        /// <summary>
        /// Entity Attributes Updating
        /// </summary>
        [EnumMember]
        EntityAttributesUpdating = 12,

        /// <summary>
        /// Entity RelationShips Updating
        /// </summary>
        [EnumMember]
        EntityRelationShipsUpdating = 13,

        /// <summary>
        /// Entity Hierarchy Updating
        /// </summary>
        [EnumMember]
        EntityHierarchyUpdating = 14,

        /// <summary>
        /// Entity child hierarchy changed event
        /// </summary>
        [EnumMember]
        EntityHierarchyChanged = 23,

        /// <summary>
        /// Entity Extensions Updating
        /// </summary>
        [EnumMember]
        EntityExtensionsUpdating = 15,

        /// <summary>
        /// Entity child extension changed event
        /// </summary>
        [EnumMember]
        EntityExtensionsChanged = 24,

        /// <summary>
        /// Source for Entity Updating event
        /// </summary>
        [EnumMember]
        EntityUpdating = 16,

        /// <summary>
        /// Entity Attributes Created
        /// </summary>
        [EnumMember]
        EntityAttributesCreated = 60,

        /// <summary>
        /// Entity RelationShips Creating
        /// </summary>
        [EnumMember]
        EntityRelationShipsCreated = 6,

        /// <summary>
        /// Entity Hierarchy Created
        /// </summary>
        [EnumMember]
        EntityHierarchyCreated = 7,

        /// <summary>
        /// Entity Extensions Created
        /// </summary>
        [EnumMember]
        EntityExtensionsCreated = 8,

        /// <summary>
        /// Source for Entity Created event
        /// </summary>
        [EnumMember]
        EntityCreated = 9,

        /// <summary>
        /// Entity Created Aync Post event
        /// </summary>
        [EnumMember]
        EntityCreatePostProcessCompleted = 31,

        /// <summary>
        /// Entity Updated  Async Post Event
        /// </summary>
        [EnumMember]
        EntityUpdatePostProcessCompleted = 32,

        /// <summary>
        /// Entity Delete Async Post Event
        /// </summary>
        [EnumMember]
        EntityDeletePostProcessCompleted = 34,

        /// <summary>
        /// Entity Reclassified
        /// </summary>
        [EnumMember]
        EntityReclassified = 11,

        /// <summary>
        /// Entity Attributes Updated
        /// </summary>
        [EnumMember]
        EntityAttributesUpdated = 17,

        /// <summary>
        /// Entity RelationShips Updated
        /// </summary>
        [EnumMember]
        EntityRelationShipsUpdated = 18,

        /// <summary>
        /// Entity Hierarchy Updated
        /// </summary>
        [EnumMember]
        EntityHierarchyUpdated = 19,

        /// <summary>
        /// Entity Extensions Updated
        /// </summary>
        [EnumMember]
        EntityExtensionsUpdated = 20,

        /// <summary>
        /// Source for Entity Updated event
        /// </summary>
        [EnumMember]
        EntityUpdated = 21,

        /// <summary>
        /// Entity Family Promote Qualifying
        /// </summary>
        [EnumMember]
        EntityFamilyPromoteQualifying = 87,

        /// <summary>
        /// Entity Family Promoted
        /// </summary>
        [EnumMember]
        EntityFamilyPromoted = 88,

        /// <summary>
        /// Entity variants changing
        /// </summary>
        [EnumMember]
        EntityVariantsChanging = 89,

        /// <summary>
        /// Entity extension changing
        /// </summary>
        [EnumMember]
        EntityExtensionsChanging = 90,

        /// <summary>
        /// Workflow Activity Entry Criteria Execution
        /// </summary>
        [EnumMember]
        WorkflowActivityEntryCriteria = 91,

        /// <summary>
        /// Workflow Activity Exit Criteria Execution
        /// </summary>
        [EnumMember]
        WorkflowActivityExitCriteria = 92,

        /// <summary>
        /// Workflow Systen Activity ExecuteBusinessRules async Evaluation. 
        /// </summary>
        [EnumMember]
        WorkflowAsyncBusinessRuleExecution = 93,

    }

    /// <summary>
    /// Specifies all the available Event Subscribers 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EventSubscriber
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// ProductCenter Event Subscriber
        /// </summary>
        [EnumMember]
        ProductCenter = 1,
        /// <summary>
        /// WebUI Event Subscriber
        /// </summary>
        [EnumMember]
        WebUI = 2,
        /// <summary>
        /// AdvancedSearchPage Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPage = 3,
        /// <summary>
        /// AdvancedSearchPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageGrid = 4,
        /// <summary>
        /// AdvancedSearchPageSearchAttributes Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageSearchAttributes = 5,
        /// <summary>
        /// AdvancedSearchPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageToolBar = 6,
        /// <summary>
        /// DQMGuidedSearchPage Event Subscriber
        /// </summary>
        [EnumMember]
        DQMGuidedSearchPage = 7,
        /// <summary>
        /// DQMGuidedSearchPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        DQMGuidedSearchPageGrid = 8,
        /// <summary>
        /// DQMGuidedSearchPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        DQMGuidedSearchPageToolBar = 9,
        /// <summary>
        /// ServiceResultsPage Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceResultsPage = 10,
        /// <summary>
        /// ServiceResultsPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceResultsPageGrid = 11,
        /// <summary>
        /// ServiceResultsPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceResultsPageToolBar = 12,
        /// <summary>
        /// ServiceResultsPageDataSource Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceResultsPageDataSource = 13,
        /// <summary>
        /// ServiceSelectorPage Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceSelectorPage = 14,
        /// <summary>
        /// ServiceSelectorPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceSelectorPageToolBar = 15,
        /// <summary>
        /// ServiceSelectorAdvancedSearch Event Subscriber
        /// </summary>
        [EnumMember]
        ServiceSelectorAdvancedSearch = 16,
        /// <summary>
        /// AdvancedSearchPageToolBarRelationship Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageToolBarRelationship = 17,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        [EnumMember]
        RelationshipPage = 18,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        RelationshipPageToolBar = 19,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        [EnumMember]
        WhereUsedPage = 20,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        WhereUsedPageToolBar = 21,
        /// <summary>
        /// RelationshipPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        WhereUsedPageGrid = 22,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        [EnumMember]
        HomePage = 23,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        HomePagePanelBar = 24,
        /// <summary>
        /// ApplicationPage Event Subscriber
        /// </summary>
        [EnumMember]
        NodeTypeApplicationsPage = 25,
        /// <summary>
        /// NodeTypeApplicationsPageToolbar Event Subscriber
        /// </summary>
        [EnumMember]
        NodeTypeApplicationsPageToolBar = 26,
        /// <summary>
        /// NodeTypeApplicationsPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        NodeTypeApplicationsPageGrid = 27,
        /// <summary>
        /// NodeTypeApplicationsPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        NodeTypeApplicationsPageControls = 28,
        /// <summary>
        /// RelationshipPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        RelationshipPageGrid = 29,
        /// <summary>
        /// DocumentBrowserPage Event Subscriber
        /// </summary>
        [EnumMember]
        DocumentBrowserPage = 30,
        /// <summary>
        /// DocumentBrowserPageDocumentExplorer Event Subscriber
        /// </summary>
        [EnumMember]
        DocumentBrowserPageDocumentExplorer = 31,
        /// <summary>
        /// DocumentBrowserPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        DocumentBrowserPageGrid = 32,
        /// <summary>
        /// DocumentBrowserPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageToolBarDocumentAssociation = 33,
        /// <summary>
        /// ItemEditPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        ItemEditPage = 34,
        /// <summary>
        /// ItemEditPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        ItemEditPageToolBar = 35,
        /// <summary>
        /// ChildEntityDetailPage Event Subscriber
        /// </summary>
        [EnumMember]
        ChildEntityDetailPage = 36,
        /// <summary>
        /// ChildEntityDetailGrid Event Subscriber
        /// </summary>
        [EnumMember]
        ChildEntityDetailGrid = 37,
        /// <summary>
        /// ChildEntityDetailGrid Event Subscriber
        /// </summary>
        [EnumMember]
        ChildEntityDetailToolBar = 38,
        /// <summary>
        /// EntityDetailPage Event Subscriber
        /// </summary>
        [EnumMember]
        EntityDetailPage = 39,
        /// <summary>
        /// EntityDetailRibbonBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityDetailPageRibbonBar = 40,
        /// <summary>
        /// EntityDetailToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityDetailPageActionToolBar = 41,
        /// <summary>
        /// EntityDetailStageTransitionToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityDetailPageStageTransitionToolBar = 42,
        /// <summary>
        /// EntityDetailAssignmentToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityDetailPageAssignmentToolBar = 43,
        /// <summary>
        /// EntityExplorerPage Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPage = 44,
        /// <summary>
        /// EntityExplorerPageGrid Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageGrid = 45,
        /// <summary>
        /// EntityExplorerPageSearchAttributes Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageSearchAttributes = 46,
        /// <summary>
        /// EntityExplorerPageRibbonBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageRibbonBar = 47,
        /// <summary>
        /// EntityExplorerPageActionToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageActionToolBar = 48,
        /// <summary>
        /// EntityExplorerPageStageTransitionToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageStageTransitionToolBar = 49,
        /// <summary>
        /// EntityExplorerPageAssignmentToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageAssignmentToolBar = 50,
        /// <summary>
        /// MetaDataPage Event Subscriber
        /// </summary>
        [EnumMember]
        MetaDataPage = 51,
        /// <summary>
        /// MetaDataPageToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        MetaDataPageToolBar = 52,
        /// <summary>
        /// AdvancedSearchPageToolBarTestRuleSet Event Subscriber
        /// </summary>
        [EnumMember]
        AdvancedSearchPageToolBarTestRuleSet = 53,
        /// <summary>
        /// EntityEditorPage Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorPage = 54,
        /// <summary>
        /// EntityEditorPageNavigationPanel Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorPageNavigationPanel = 55,
        /// <summary>
        /// EntityEditorPageOtherViewsPanel Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorPageOtherViewsPanel = 56,
        /// <summary>
        /// EntityEditorStandardViewsTreeView Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorStandardViewsTreeView = 57,
        /// <summary>
        /// EntityEditorMyViewsTreeView Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorMyViewsTreeView = 59,
        /// <summary>
        /// EntityEditorContainersTreeView Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorContainersTreeView = 60,
        /// <summary>
        /// EntityEditorDefaultSettings Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorDefaultSettings = 61,
        /// <summary>
        /// EntityEditorPage NavigationPanel StateViews
        /// </summary>
        [EnumMember]
        EntityEditorPageNavigationPanelStateViews = 62,
        /// <summary>
        /// EntityExplorerPage Left Panel Bar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityExplorerPageLeftPanelBar = 63,
        /// <summary>
        /// EntityEditorPage BreadCrumb 
        /// </summary>
        [EnumMember]
        EntityEditorPageBreadCrumb = 64,
        /// <summary>
        /// EntityEditorPage Title
        /// </summary>
        [EnumMember]
        EntityEditorPageTitle = 65,
        /// <summary>
        /// Entity Explorer Page Windows Workflow Action ToolBar
        /// </summary>
        [EnumMember]
        EntityExplorerPageWindowsWorkflowActionToolBar = 66,
        /// <summary>
        /// Entity Explorer Page Windows Workflow Assignment ToolBar
        /// </summary>
        [EnumMember]
        EntityExplorerPageWindowsWorkflowAssignmentToolBar = 67,
        /// <summary>
        /// Entity Detail Page Windows Workflow Action ToolBar
        /// </summary>
        [EnumMember]
        EntityDetailPageWindowsWorkflowActionToolBar = 68,
        /// <summary>
        /// Entity Detail Page Windows Workflow Assignment ToolBar
        /// </summary>
        [EnumMember]
        EntityDetailPageWindowsWorkflowAssignmentToolBar = 69,
        /// <summary>
        /// Entity Editor Other views Panel WorkflowTasks Tree view
        /// </summary>
        [EnumMember]
        EntityEditorPageOtherviewsPanelWorkflowTasksTreeView = 70,
        /// <summary>
        /// Entity Editor Container Panel Data Config
        /// </summary>
        [EnumMember]
        EntityEditorContainerPanelDataConfig = 71,
        /// <summary>
        /// Vendor Portal Event Subscriber
        /// </summary>
        [EnumMember]
        VendorPortal = 72,
        /// <summary>
        /// Vendor Portal Web UI Event Subscriber
        /// </summary>
        [EnumMember]
        VPWebUI = 73,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPage = 74,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Menu Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageMenu = 75,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Search Grid Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageSearchGrid = 76,
        /// <summary>
        /// Vendor Portal Entity Explorer Page TreeView Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageTreeView = 77,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Basic Actions Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageBasicActionsToolBar = 78,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Stage Transitions Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageStageTransitionToolBar = 79,
        /// <summary>
        /// Vendor Portal Entity Editor Page Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityEditorPage = 80,
        /// <summary>
        /// Vendor Portal Entity Editor Page View Panel Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityEditorPageViewPanel = 81,
        /// <summary>
        /// Vendor Portal Entity Editor Page Basic Actions Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityEditorPageBasicActionsToolBar = 82,
        /// <summary>
        /// Vendor Portal Entity Editor Page Stage Transitions Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityEditorPageStageTransitionToolBar = 83,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Report Grid Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageReportGrid = 84,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Report Search Panel Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageSearchPanel = 85,
        /// <summary>
        /// Vendor Portal Search Configuration Event Subscriber
        /// </summary>
        [EnumMember]
        VPSearchConfiguration = 86,
        /// <summary>
        /// Vendor Portal Entity Editor Page Tab Title Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityEditorPageTabTitle = 87,
        /// <summary>
        /// EntityEditorPage OtherViewsPanel Configuration Event Subscriber
        /// </summary>
        [EnumMember]
        EntityEditorPageOtherViewsPanelConfiguration = 88,
        /// <summary>
        /// MDMCenter Event Subscriber
        /// </summary>
        [EnumMember]
        MDMCenter = 89,
        /// <summary>
        /// Entity Hierarchy Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHierarchy = 90,
        /// <summary>
        /// Entity Hierarchy Matrix ToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHierarchyMatrixToolBar = 91,
        /// <summary>
        /// Entity Hierarchy Level Detail ToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHierarchyLevelDetailToolBar = 92,
        /// <summary>
        /// MAM Event Subscriber
        /// </summary>
        [EnumMember]
        MAM = 93,
        /// <summary>
        /// MAM web UI event subscriber
        /// </summary>
        [EnumMember]
        MAMWebUI = 94,
        /// <summary>
        /// MAM entity explorer event subscriber
        /// </summary>
        [EnumMember]
        MAMEntityExplorerPage = 95,
        /// <summary>
        /// MAM entity editor event subscriber
        /// </summary>
        [EnumMember]
        MAMEntityEditorPage = 96,
        /// <summary>
        /// Entity Hierarchy Rule ToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHierarchyRuleToolBar = 97,
        /// <summary>
        /// Entity Hierarchy Rule Grid Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHierarchyLevelDetailGrid = 98,
        /// <summary>
        /// MDMCenter Search Manager Event Subscriber
        /// </summary>
        [EnumMember]
        SearchManager = 99,
        /// <summary>
        /// MDMCenter Search Criteria Event Subscriber
        /// </summary>
        [EnumMember]
        SearchCriteria = 100,
        /// <summary>
        /// MDMCenter Search Weightage Event Subscriber
        /// </summary>
        [EnumMember]
        SearchWeightage = 101,
        /// <summary>
        /// MDMCenter Search Workflow Event Subscriber
        /// </summary>
        [EnumMember]
        WorkflowSearch = 102,
        /// <summary>
        /// MDMCenter Search Configuration Event Subscriber
        /// </summary>
        [EnumMember]
        MDMCenterSearchConfiguration = 113,
        /// <summary>
        /// MDMCenter Locale Configuration Event Subscriber
        /// </summary>
        [EnumMember]
        LocaleConfiguration = 114,
        /// <summary>
        /// Vendor Portal Bulk Loader Page
        /// </summary>
        [EnumMember]
        VPBulkLoaderPage = 115,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Import Templates List
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageImportTemplatesList = 116,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Grid
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageBatchJobGrid = 117,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Import Profiles List
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageImportProfilesList = 118,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job ToolBar
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageBatchJobToolBar = 119,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Entity Assignment ToolBar
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageEntityAssignmentToolBar = 120,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Details ToolBar
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageBatchJobDetailsToolBar = 121,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Details Grid
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageBatchJobDetailsGrid = 122,
        /// <summary>
        /// MetaDataPagePanelBar
        /// </summary>
        [EnumMember]
        MetaDataPagePanelBar = 123,
        /// <summary>
        /// Vendor Portal Entity Editor Page Settings
        /// </summary>
        [EnumMember]
        VPEntityEditorPageSettings = 124,
        /// <summary>
        /// Vendor Portal Entity Editor Page Workflow Stage ToolBar
        /// </summary>
        [EnumMember]
        VPEntityEditorPageWorkflowStageToolBar = 125,
        /// <summary>
        /// Vendor Portal Entity Editor Page Entity Assignment Toolbar
        /// </summary>
        [EnumMember]
        VPEntityEditorPageEntityAssignmentToolBar = 126,
        /// <summary>
        /// Vendor Portal Entity Editor Page Entity Hierarchy Simple Grid
        /// </summary>
        [EnumMember]
        VPEntityEditorPageEntityHierarchySimpleGrid = 127,
        /// <summary>
        /// Vendor Portal Entity Explorer page Export Toolbar
        /// </summary>
        [EnumMember]
        VPEntityExplorerPageExportToolBar = 128,
        /// <summary>
        /// Vendor Portal Bulk Loader page Settings
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageDownloadTemplateSettings = 129,
        /// <summary>
        /// Vendor Portal Done Report page
        /// </summary>
        [EnumMember]
        VPDoneReportPage = 130,
        /// <summary>
        /// Vendor Portal Done Report page Grid
        /// </summary>
        [EnumMember]
        VPDoneReportPageGrid = 131,
        /// <summary>
        /// Vendor Portal Done Report page Settings
        /// </summary>
        [EnumMember]
        VPDoneReportPageSettings = 132,
        /// <summary>
        /// Vendor Portal Entity Editor page Entity Hierarchy Simple Actions ToolBar
        /// </summary>
        [EnumMember]
        VPEntityEditorPageEntityHierarchySimpleActionsToolBar = 133,
        /// <summary>
        /// Vendor Portal Entity Editor page Entity Comparer
        /// </summary>
        [EnumMember]
        VPEntityEditorPageEntityComparerSettings = 134,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Batch Job Details ToolBar
        /// </summary>
        [EnumMember]
        VPEntityEditorPageHierarchySimpleBatchJobDetailsToolBar = 135,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Import Profiles List
        /// </summary>
        [EnumMember]
        VPEntityEditorPageHierarchySimpleImportProfilesList = 136,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Batch Job Details Grid
        /// </summary>
        [EnumMember]
        VPEntityEditorPageHierarchySimpleBatchJobDetailsGrid = 137,
        /// <summary>
        /// EntityHierarchyConfiguration
        /// </summary>
        [EnumMember]
        EntityHierarchyConfiguration = 138,
        /// <summary>
        /// Vendor Portal Editor page Entity Hierarchy Simple Batch Job Details Download Settings
        /// </summary>
        [EnumMember]
        VPEntityEditorPageHierarchySimpleBatchJobDetailsDownloadSettings = 139,
        /// <summary>
        /// Vendor Portal Bulk Loader Batch Job Details Download Settings
        /// </summary>
        [EnumMember]
        VPBulkLoaderPageBatchJobDetailsDownloadSettings = 140,

        /// <summary>
        /// Data Quality Management Event Subscriber
        /// </summary>
        [EnumMember]
        DataQualityManagement = 141,

        /// <summary>
        /// Data Quality Management Dashboard Event Subscriber
        /// </summary>
        [EnumMember]
        DQMDashboard = 142,

        /// <summary>
        /// DQM Report Page Basic Actions ToolBar Event Subscriber
        /// </summary>
        [EnumMember]
        DQMReportPageBasicActionsToolBar = 143,

        /// <summary>
        /// DQM Dashboard page Settings Event Subscriber
        /// </summary>
        [EnumMember]
        DQMDashboardPageSettings = 144,

        /// <summary>
        /// DQM Dashboard page Report Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DQMReportPageReportGrid = 145,

        /// <summary>
        /// DQM Entity Set Report page Report Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DQMEntitySetReportPageReportGrid = 146,

        /// <summary>
        /// DQM DataQualityIndicator Management page Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DQMDataQualityIndicatorManagementPageGrid = 147,

        /// <summary>
        /// DQM DataQualityIndicator Management page Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DQMDataQualityIndicatorManagementPageToolbar = 148,

        /// <summary>
        /// DQM Validation Profile Management page Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DQMValidationProfileManagementPageToolbar = 149,

        /// <summary>
        /// DQM Validation Jobs page Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DQMValidationJobsPageToolbar = 150,

        /// <summary>
        /// MDM Complex Attribute page Event Subscriber
        /// </summary>
        [EnumMember]
        ComplexAttributeEditorPage = 151,

        /// <summary>
        /// MDM Complex Attribute page Tool Bar Event Subscriber
        /// </summary>
        [EnumMember]
        ComplexAttributeEditorPageToolbar = 152,

        /// <summary>
        /// MDM Complex Attribute page grid Event Subscriber
        /// </summary>
        [EnumMember]
        ComplexAttributeEditorPageGrid = 153,

        /// <summary>
        /// VP Complex Attribute page Event Subscriber
        /// </summary>
        [EnumMember]
        VPComplexAttributeEditorPage = 154,

        /// <summary>
        /// VP Complex Attribute page Tool bar Event Subscriber
        /// </summary>
        [EnumMember]
        VPComplexAttributeEditorPageToolbar = 155,

        /// <summary>
        /// MDM Complex Attribute page grid Event Subscriber
        /// </summary>
        [EnumMember]
        VPComplexAttributeEditorPageGrid = 156,

        /// <summary>
        /// MDM Entity History View page Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHistoryViewPage = 157,

        /// <summary>
        /// MDM Entity History View page grid Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHistoryViewPageGrid = 158,

        /// <summary>
        /// VP Entity History View page Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityHistoryViewPage = 159,

        /// <summary>
        /// MDM Entity History View page grid Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityHistoryViewPageGrid = 160,

        /// <summary>
        /// VP Catalog View page Event Subscriber
        /// </summary>
        [EnumMember]
        VPCatalogViewPage = 161,

        /// <summary>
        /// VP Catalog View page Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        VPCatalogViewPageActionsToolbar = 162,

        /// <summary>
        /// VP catalog View page search grid Event Subscriber
        /// </summary>
        [EnumMember]
        VPCatalogViewPageSearchGrid = 163,

        /// <summary>
        /// VP Catalog View page search panel Event Subscriber
        /// </summary>
        [EnumMember]
        VPCatalogViewPageSearchPanel = 164,

        /// <summary>
        /// MDM entity history exclude list Event Subscriber
        /// </summary>
        [EnumMember]
        EntityHistoryViewPageExcludeList = 165,

        /// <summary>
        /// VP entity history exclude list Event Subscriber
        /// </summary>
        [EnumMember]
        VPEntityHistoryViewPageExcludeList = 166,

        /// <summary>
        /// DQM DataNormalization Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalization = 167,

        /// <summary>
        /// DQM DataNormalization Profile Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationProfileManagementPageBasicActionsToolBar = 168,

        /// <summary>
        /// DQM DataNormalization Profile Management page Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationProfileManagementPageGrid = 169,

        /// <summary>
        /// DQM DataNormalization Rule Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationRuleManagementPageBasicActionsToolBar = 170,

        /// <summary>
        /// DQM DataNormalization Rule Management page Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationRuleManagementPageGrid = 171,

        /// <summary>
        /// catalog view event subscriber
        /// </summary>
        [EnumMember]
        CatalogViewPage = 172,

        /// <summary>
        /// relationship page catalog view action toolbar event subscriber
        /// </summary>
        [EnumMember]
        CatalogViewRelationshipPageActionToolbar = 173,

        /// <summary>
        /// catalog view page search grid event subscriber
        /// </summary>
        [EnumMember]
        CatalogViewPageSearchGrid = 174,

        /// <summary>
        /// catalog view page search panel event subscriber
        /// </summary>
        [EnumMember]
        CatalogViewPageSearchPanel = 175,

        /// <summary>
        /// DQM DataNormalization Rule Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationReportPageBasicActionsToolBar = 176,

        /// <summary>
        /// DQM DataNormalization Rule Management page Grid Event Subscriber
        /// </summary>
        [EnumMember]
        DataNormalizationReportPageGrid = 177,

        /// <summary>
        /// Entity Editor Variants Panel Data Config
        /// </summary>
        [EnumMember]
        VariantsPanelDataConfig = 178,

        /// <summary>
        /// Entity hierarchy level detail page catalog view action toolbar event subscriber
        /// </summary>
        [EnumMember]
        CatalogViewEHPageActionToolbar = 179,

        /// <summary>
        ///DQM validation Profile management page grid event subscriber
        /// </summary>
        [EnumMember]
        DQMValidationProfileManagementPageGrid = 180,

        /// <summary>
        /// Job page grid of DQM validation event subscriber
        /// </summary>
        [EnumMember]
        DQMValidationJobsPageGrid = 181,

        /// <summary>
        /// DQM data merge event subscriber
        /// </summary>
        [EnumMember]
        DataMerge = 182,

        /// <summary>
        /// DataMerge Planning rule set basic action toolbar event subscriber
        /// </summary>
        [EnumMember]
        DataMergeMergePlanningProfilePageBasicActionsToolBar = 183,

        /// <summary>
        /// DataMerger Planning rule set grid event subscriber
        /// </summary>
        [EnumMember]
        DataMergeMergePlanningProfilePageGrid = 184,

        /// <summary>
        /// DataMerge Source Management page basic action toolbar event subscriber
        /// </summary>
        [EnumMember]
        DataMergeSourceManagementPageBasicActionsToolBar = 185,

        /// <summary>
        /// Data merge source management page grid event subscriber
        /// </summary>
        [EnumMember]
        DataMergeSourceManagementPageGrid = 186,

        /// <summary>
        /// Translation configuration event subscriber
        /// </summary>
        [EnumMember]
        TranslationConfiguration = 187,


        /// <summary>
        /// DataMerge Merge Report page basic action toolbar event subscriber
        /// </summary>
        [EnumMember]
        DataMergeMergeReportPageBasicActionsToolBar = 188,

        /// <summary>
        /// Data merge source management page grid event subscriber
        /// </summary>  
        [EnumMember]
        DataMergeMergeReportPageGrid = 189,

        /// <summary>
        /// DQM data validation event subscriber
        /// </summary>
        [EnumMember]
        DataValidation = 190,

        /// <summary>
        /// DQM DataValidation Profile Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DataValidationProfileManagementPageBasicActionsToolBar = 191,

        /// <summary>
        /// DQM DataValidation Profile Management page Grid event subscriber
        /// </summary>
        [EnumMember]
        DataValidationProfileManagementPageGrid = 192,

        /// <summary>
        /// DQM DataMerge Profile Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        [EnumMember]
        DataMergeProfileManagementPageBasicActionsToolBar = 193,

        /// <summary>
        /// DQM DataMerge Profile Management page Grid event subscriber
        /// </summary>
        [EnumMember]
        DataMergeProfileManagementPageGrid = 194,

        /// <summary>
        /// Entity Type Panel event subscriber
        /// </summary>
        [EnumMember]
        EntityTypePanel = 209,

        /// <summary>
        /// DQM matching suspects UI event subscriber.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUI = 198,

        /// <summary>
        /// DQM matching suspects UI title config event subscriber.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUITitleConfig = 199,

        /// <summary>
        /// DQM DataMerge SurvivorshipRule Management page BasicActionToolbar event subscriber
        /// </summary>
        [EnumMember]
        DataMergeSurvivorshipRuleManagementPageBasicActionToolbar = 200,

        /// <summary>
        /// DQM DataMerge SurvivorshipRule Management page Grid event subscriber
        /// </summary>
        [EnumMember]
        DataMergeSurvivorshipRuleManagementPageGrid = 201,

        /// <summary>
        /// DQM matching suspects UI page config event subscriber.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUIPage = 202,

        /// <summary>
        /// Translation Configuration page grid event subscriber.
        /// </summary>
        [EnumMember]
        TranslationConfigPageGrid = 203,

        /// <summary>
        /// DQM matching suspects UI Grid config event subscriber.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUIGrid = 204,

        /// <summary>
        /// DQM matching suspects UI Grid config event subscriber.
        /// </summary>
        [EnumMember]
        LookupTranslationConfigGrid = 205,

        /// <summary>
        /// Application Context Configuration
        /// </summary>
        [EnumMember]
        ApplicationContextConfiguration = 206,

        /// <summary>
        /// DQM matching results UI event subscriber.
        /// </summary>
        [EnumMember]
        MatchingResultsUI = 207,

        /// <summary>
        /// DQM matching results UI Grid config event subscriber.
        /// </summary>
        [EnumMember]
        MatchingResultsUIGrid = 208,

        /// <summary>
        /// Lookup translation toolbar event subscriber
        /// </summary>
        [EnumMember]
        MaintainLookupTranslationPageToolbar = 210,

        /// <summary>
        /// Entity translation toolbar event subscriber
        /// </summary>
        [EnumMember]
        MaintainEntityTranslationPageToolbar = 211,

        /// <summary>
        /// Matching results UI page
        /// </summary>
        [EnumMember]
        MatchingResultsUIPage = 212,

        /// <summary>
        /// MDM Workflow Comment history grid event subscriber
        /// </summary>
        [EnumMember]
        WorkflowCommentsViewPageGrid = 213,

        /// <summary>
        /// MDM Hierarchical Attribute page Event Subscriber
        /// </summary>
        [EnumMember]
        HierarchicalAttributeEditorPage = 215,

        /// <summary>
        /// MDM Hierarchical Attribute page Tool Bar Event Subscriber
        /// </summary>
        [EnumMember]
        HierarchicalAttributeEditorPageToolbar = 216,

        /// <summary>
        /// VP Hierarchical Attribute page Event Subscriber
        /// </summary>
        [EnumMember]
        VPHierarchicalAttributeEditorPage = 217,

        /// <summary>
        /// VP Complex Attribute page Tool bar Event Subscriber
        /// </summary>
        [EnumMember]
        VPHierarchicalAttributeEditorPageToolbar = 218,

        /// <summary>
        /// Entity Family Detail View Subscriber
        /// </summary>
        [EnumMember]
        EntityFamilyDetailViewPageToolbar = 226,

        /// <summary>
        /// DQM matching suspects UI Toolbar config event subscriber.
        /// </summary>
        [EnumMember]
        MatchingSuspectsUIToolbar = 227

    }

    /// <summary>
    /// Specifies all the available ApplicationContextType 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ApplicationContextType
    {
        /// <summary>
        /// Use_Role_Category_NodeType_Catalog_Org
        /// </summary>
        [EnumMember]
        URCNCO = 1,

        /// <summary>
        /// Use_Role_NodeType_Category_Catalog_Org
        /// </summary>
        [EnumMember]
        URNCCO = 2,

        /// <summary>
        /// Use_Role_Catalog_Org
        /// </summary>
        [EnumMember]
        URCO = 3,

        /// <summary>
        /// Use_Role_Org
        /// </summary>
        [EnumMember]
        URO = 4,

        /// <summary>
        /// Use_Role_NodeType_Catalog_Org
        /// </summary>
        [EnumMember]
        URNCO = 5,

        /// <summary>
        /// Use_Role_NodeType_Org
        /// </summary>
        [EnumMember]
        URNO = 6,

        /// <summary>
        /// SYSTEM
        /// </summary>
        [EnumMember]
        SYSTEM = 7,

        /// <summary>
        /// ROLE
        /// </summary>
        [EnumMember]
        ROLE = 8,

        /// <summary>
        /// USER
        /// </summary>
        [EnumMember]
        USER = 9,

        /// <summary>
        /// User_Role
        /// </summary>
        [EnumMember]
        UR = 10,

        /// <summary>
        /// Use_Role_Category_RelationshipType_NodeType_Catalog_Org
        /// </summary>
        [EnumMember]
        URCNRCO = 11,

        /// <summary>
        /// LOCALE
        /// </summary>
        [EnumMember]
        LOCALE = 12,

        /// <summary>
        /// Attribute_Category_NodeType_Category_Org
        /// </summary>
        [EnumMember]
        ACNCO = 13,

        /// <summary>
        /// User_Role_Locale_AttributeSrc_AttributeTgt_Category_RelationshipType_NodeType_Catalog_Org
        /// </summary>
        [EnumMember]
        URLAACRNCO = 14,

        /// <summary>
        /// Locale_AttributeTgt_CNode_Category_RelationshipType_NodeType_Catalog_Org
        /// </summary>
        [EnumMember]
        LACCRNCO = 15,

        /// <summary>
        /// Category_Catalog_Org
        /// </summary>
        [EnumMember]
        CCO = 16,

        /// <summary>
        /// NodeType_Category_Catalog_Org
        /// </summary>
        [EnumMember]
        NCCO = 17,

        /// <summary>
        /// Dependent Attributes
        /// </summary>
        [EnumMember]
        DA = 20,

        /// <summary>
        /// Business Rules
        /// </summary>
        [EnumMember]
        BR = 21,

        /// <summary>
        /// Catalog_Category
        /// </summary>
        [EnumMember]
        CC = 22
    }

    /// <summary>
    /// Defines the Match context
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ApplicationContextMatchType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Nearest match
        /// </summary>
        [EnumMember]
        NearestMatch = 1,

        /// <summary>
        /// All possible matches
        /// </summary>
        [EnumMember]
        PossibleMatches = 2,

        /// <summary>
        /// Match by object type mapping
        /// </summary>
        [EnumMember]
        MatchByObjectTypeMap = 3,

        /// <summary>
        /// Match by object type rule mapping
        /// </summary>
        [EnumMember]
        MatchByObjectTypeRuleMap = 4,

        /// <summary>
        /// Exact match
        /// </summary>
        [EnumMember]
        ExactMatch = 5
    }

    /// <summary>
    /// Specifies all the available MDM Center Extension Modules
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMCenterExtensionEnum
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// MDM Center Module
        /// </summary>
        [EnumMember]
        MDMCenter = 1,

        /// <summary>
        /// 1 World Sync Module
        /// </summary>
        [EnumMember]
        OneWorldSync = 2,

        /// <summary>
        /// SMB packaged solution Module
        /// </summary>
        [EnumMember]
        SMBPackagedSolution = 3,

        /// <summary>
        /// Customization Module
        /// </summary>
        [EnumMember]
        Customization = 50
    }

    #endregion

    #region Operation Result enums

    /// <summary>
    /// Defines the status of operation result
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum OperationResultStatusEnum
    {
        /// <summary>
        /// Indicates none of the actions taken on the workitems
        /// </summary>
        [EnumMember]
        None = 1,
        /// <summary>
        /// Indicates current work item is pending to be executed
        /// </summary>
        [EnumMember]
        Pending = 2,
        /// <summary>
        /// Indicates all workitems are successful
        /// </summary>
        [EnumMember]
        Successful = 3,
        /// <summary>
        /// Indicates some work items are successful and some are errored out
        /// </summary>
        [EnumMember]
        CompletedWithErrors = 4,
        /// <summary>
        /// Indicates all workitems failed.
        /// </summary>
        [EnumMember]
        Failed = 5,
        /// <summary>
        /// Indicates some work items are successful and some are having Warnings
        /// </summary>
        [EnumMember]
        CompletedWithWarnings = 6
    }

    /// <summary>
    /// Defines the type of operation result
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum OperationResultType
    {
        /// <summary>
        /// Error result type
        /// </summary>
        [EnumMember]
        Error = 1,

        /// <summary>
        /// Information result type
        /// </summary>
        [EnumMember]
        Information = 2,

        /// <summary>
        /// Warning result type
        /// </summary>
        [EnumMember]
        Warning = 3,

        /// <summary>
        /// Ignore the result
        /// </summary>
        [EnumMember]
        Ignore = 4
    }

    #endregion Operation Result enums

    #region CompletionCriterionEnum

    /// <summary>
    /// Determines the completion criteria for a View
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CompletionCriterionEnum
    {
        /// <summary>
        /// Indicates there is no completion criteria
        /// </summary>
        [EnumMember]
        None = 1,

        /// <summary>
        /// Indicates a List of Required attributes will is a completion criterion
        /// </summary>
        [EnumMember]
        List = 2,

        /// <summary>
        /// Indicates all required attributes should be filled.
        /// </summary>
        [EnumMember]
        Required = 3,

        /// <summary>
        /// Indicates All visible attributes should be filled.
        /// </summary>
        [EnumMember]
        All = 4

    }

    #endregion CompletionCriterionEnum

    #region Cache enums

    /// <summary>
    /// Defines cache modes available in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CacheType
    {
        /// <summary>
        /// Internal ASP.NET caching
        /// </summary>
        [EnumMember]
        AspNet = 0,
        /// <summary>
        /// Framework 4.0 Runtime caching
        /// </summary>
        [EnumMember]
        Framework4 = 1,
        /// <summary>
        /// Distributed Memcached
        /// </summary>
        [EnumMember]
        Memcached = 2,
        /// <summary>
        /// AppFabric Caching
        /// </summary>
        [EnumMember]
        AppFabric = 3,
        /// <summary>
        /// AppFabric caching with notification enabled
        /// </summary>
        [EnumMember]
        AppFabricWithNotificationEnabled = 4,
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        UnKnown = 5,
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 6
    }

    /// <summary>
    /// Defines cache modes available in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ImpactType
    {
        /// <summary>
        /// Just Base entity
        /// </summary>
        [EnumMember]
        Base = 0,
        /// <summary>
        /// Local Attributes
        /// </summary>
        [EnumMember]
        LocalAttributes = 1,
        /// <summary>
        /// Inherited Attributes
        /// </summary>
        [EnumMember]
        InheritedAttributes = 2,
        /// <summary>
        /// Relationships
        /// </summary>
        [EnumMember]
        Relationships = 3,
        /// <summary>
        /// Extension Relationships
        /// </summary>
        [EnumMember]
        ExtensionRelationships = 4,
        /// <summary>
        /// Hierarhcy Relationships
        /// </summary>
        [EnumMember]
        HierarchyRelationships = 5,
        /// <summary>
        /// All
        /// </summary>
        [EnumMember]
        All = 6,
    }

    /// <summary>
    /// Cache status - is it ditry or not
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CacheStatus
    {
        /// <summary>
        /// Cache status is unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Entity's cache is dirty because it is impacted from some parent attribute value change.
        /// In this case entity is available in ImpactedEntity table.
        /// </summary>
        [EnumMember]
        DirectDirty = 1,
        /// <summary>
        /// Entity's cache is dirty as entity's one of the parent is changed. But parent entity's change's impact is not calculated yet. 
        /// In this case entity is available in not in ImpactedEntity table, But one of its parent is available in ActivitLog table
        /// </summary>
        [EnumMember]
        InDirectDirty = 2,
        /// <summary>
        /// Entity's cache is clear.
        /// </summary>
        [EnumMember]
        UpToDate = 3
    }
    #endregion

    #region Import enums

    /// <summary>
    /// Collection processing type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CollectionProcessingType
    {
        /// <summary>
        /// Merge collection values with existing values
        /// </summary>
        [EnumMember]
        Merge = 0,

        /// <summary>
        /// Replace collection values
        /// </summary>
        [EnumMember]
        Replace = 1,

        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 2
    }

    /// <summary>
    /// Enum specifying Type of the Import Processing
    /// </summary>
    public enum ImportProcessingType
    {
        /// <summary>
        /// Validations for staging data is carried out. No data is inserted in to the database.
        /// </summary>
        ValidationOnly = 0,

        /// <summary>
        /// Entities from the statging are validated and inserted in to the database.
        /// </summary>
        ValidateAndProcess = 1,

        /// <summary>
        /// Validate match and merge
        /// </summary>
        ValidateMatchAndMerge = 2
    }

    /// <summary>
    /// Enum specifying mode of the import
    /// </summary>
    public enum ImportMode
    {
        /// <summary>
        /// Indicates that we process entity and attributes using the entity object model itself. This will ensure transactional consistency.
        /// This can be used for both initial load and daily import
        /// </summary>
        Merge = 0,

        /// <summary>
        /// Indicates that we process the entity using entity object model and the attributes using bulk mode. This uses the fast bulk insert option
        /// assumes that only new data is being processed. This is ideal for new data.
        /// </summary>
        InitialLoad = 1,

        /// <summary>
        /// Indicates that we are initial loading the complex attribute.
        /// </summary>
        ComplextAttribute = 2,

        /// <summary>
        /// Indicates that we process the only relationship data. 
        /// </summary>
        RelationshipLoad = 3,

        /// <summary>
        /// Indicates that we process the only extension relationship (MDL) data. 
        /// </summary>
        ExtensionRelationshipLoad = 4,

        /// <summary>
        /// Indicates that we process by the Entity Hierarchy levels. 
        /// </summary>
        EntityHierarchyLoad = 5,

        /// <summary>
        /// Indicates that we process by the Entity Hierarchy levels. 
        /// </summary>
        EntityExtensionRelationshipAndHierarchyLoad = 6,

        /// <summary>
        /// UnKnown
        /// </summary>
        UnKnown = 7,

        /// <summary>
        /// Indicates that we process the relationships using entity/relationship object model and the attributes using bulk mode. This uses the fast bulk insert option
        /// assumes that only new data is being processed. This is ideal for new data.
        /// </summary>
        RelationshipInitialLoad = 8,

    }

    /// <summary>
    /// Defines the context for which a import provider needs to fetch the entities.
    /// </summary>
    public enum EntityProviderContextType
    {
        /// <summary>
        /// Return everything without any context
        /// </summary>
        All = 0,

        /// <summary>
        /// Return only entities for the given entity type
        /// </summary>
        EntityType = 1,

        /// <summary>
        /// Return only entities for the given containers
        /// </summary>
        Container = 2,

        /// <summary>
        /// Return only entities for the given Organization
        /// </summary>
        Organization = 3
    }

    /// <summary>
    /// List of execution step types
    /// </summary>
    public enum ExecutionStepType
    {
        /// <summary>
        /// Core Step
        /// </summary>
        [EnumMember]
        Core = 0,

        /// <summary>
        /// Custom Step
        /// </summary>
        [EnumMember]
        Custom = 1,

        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        UnKnown = 2
    }

    /// <summary>
    /// List of import source types
    /// </summary>
    public enum ImportSourceType
    {
        /// <summary>
        /// StagingDB10
        /// </summary>
        [EnumMember]
        StagingDB10 = 0,

        /// <summary>
        /// Custom Step
        /// </summary>
        [EnumMember]
        FlatStagingDB10 = 1,

        /// <summary>
        /// RSMAM10 provider
        /// </summary>
        RSMAM10 = 2,

        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        UnKnown = 3,

        /// <summary>
        /// Generic provider
        /// </summary>
        Generic10 = 4,

        /// <summary>
        /// RSXml41
        /// </summary>
        [EnumMember]
        RSXml41 = 5,

        /// <summary>
        /// RSXml45
        /// </summary>
        [EnumMember]
        RSXml45 = 6,

        /// <summary>
        /// RSDsv10
        /// </summary>
        [EnumMember]
        RSDsv10 = 7,

        /// <summary>
        /// RSExcel12
        /// </summary>
        [EnumMember]
        RSExcel12 = 8,

        /// <summary>
        /// Lookup data xml 10
        /// </summary>
        LookupData10 = 9,

        /// <summary>
        /// Lookup data Excel 10
        /// </summary>
        LookupExcel10 = 10,

        /// <summary>
        /// Represents the Riversand Lookup Excel Source type, And the Version 1.0
        /// </summary>
        [EnumMember]
        RSLookupExcel10 = 11,

        /// <summary>
        /// Represents the Riversand Lookup XML Source type, And the Version is 1.0
        /// </summary>
        [EnumMember]
        RSLookupXml10 = 12,

        /// <summary>
        /// Represents the Riversand Lookup Generic Source type, And the Version is 1.0
        /// </summary>
        [EnumMember]
        RSLookupGeneric10 = 13,

        /// <summary>
        ///  Represents the Riversand Lookup DSV Source type, And the Version is 1.0
        /// </summary>
        [EnumMember]
        RSLookupDSV10 = 14,

        /// <summary>
        ///  Represents the Riversand Lookup DSV Source type, And the Version is 1.0
        /// </summary>
        [EnumMember]
        RSXliff10 = 15,

        /// <summary>
        ///  Represents the Riversand Lookup DSV Source type, And the Version is 1.0
        /// </summary>
        [EnumMember]
        RSLookupXliff10 = 16,

        /// <summary>
        ///  Represents the Riversand DataModel Excel Source type, And the Version
        /// </summary>
        [EnumMember]
        RSDataModelExcel = 17,

        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Generic11 = 18,

        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Generic12 = 19,

        /// <summary>
        /// Represents the Riversand DDG Excel Source type
        /// </summary>
        [EnumMember]
        RSDDGExcel = 20,

    }

    /// <summary>
    /// List of import provider types
    /// </summary>
    public enum ImportProviderType
    {
        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// FileSystemMonitor 
        /// </summary>
        [EnumMember]
        FileSystemMonitor = 1,

        /// <summary>
        /// MSMQ
        /// </summary>
        [EnumMember]
        MSMQ = 2,

        /// <summary>
        /// JMSQ
        /// </summary>
        [EnumMember]
        JMSQ = 3,

        /// <summary>
        /// TibcoJMSQ
        /// </summary>
        [EnumMember]
        TibcoJMSQ = 4,

        /// <summary>
        /// FTP
        /// </summary>
        [EnumMember]
        FTP = 5
    }

    /// <summary>
    /// List of import source types
    /// </summary>
    public enum ImportProviderBatchingType
    {
        /// <summary>
        /// Supports only a single batch at a time. This applies to file based providers.
        /// </summary>
        [EnumMember]
        Single = 0,

        /// <summary>
        /// Supports multiple batches. This applies to database based providers.
        /// </summary>
        [EnumMember]
        Multiple = 1,

        /// <summary>
        /// No support. Always provides data in one call.
        /// </summary>
        [EnumMember]
        None
    }

    /// <summary>
    /// List of import apping types available
    /// </summary>
    public enum MappingDataType
    {
        /// <summary>
        /// DBColumn
        /// </summary>
        [EnumMember]
        DBColumn = 0,

        /// <summary>
        /// XmlNode
        /// </summary>
        [EnumMember]
        XmlNode = 1,

        /// <summary>
        /// XmlAttribute
        /// </summary>
        [EnumMember]
        XmlAttribute = 2,

        /// <summary>
        /// ExcelColumn
        /// </summary>
        [EnumMember]
        ExcelColumn = 3,

        /// <summary>
        /// MDMEntity
        /// </summary>
        [EnumMember]
        MDMEntityObject = 4,

        /// <summary>
        /// MDMAttribute
        /// </summary>
        [EnumMember]
        MDMAttributeObject = 5,

        /// <summary>
        /// EntityTable
        /// </summary>
        [EnumMember]
        EntityData = 6,

        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        AttributeData = 7,

        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        UnKnown = 8,

        /// <summary>
        /// EntityIdentifier
        /// </summary>
        [EnumMember]
        EntityIdentifier = 9
    }

    /// <summary>
    /// List of import mapping modes
    /// </summary>
    public enum MappingMode
    {
        /// <summary>
        /// Implicit
        /// </summary>
        [EnumMember]
        Implicit = 0,

        /// <summary>
        /// Explicit
        /// </summary>
        [EnumMember]
        Explicit = 1,

        /// <summary>
        /// InputField
        /// </summary>
        [EnumMember]
        InputField = 2,

        /// <summary>
        /// Custom
        /// </summary>
        [EnumMember]
        Custom = 3
    }

    /// <summary>
    /// List of object types supported for EntityMetadata Map
    /// </summary>
    public enum EntityMetadataMapObjectType
    {
        /// <summary>
        /// ShortNameMap
        /// </summary>
        [EnumMember]
        ShortNameMap = 1,

        /// <summary>
        /// LongNameMap
        /// </summary>
        [EnumMember]
        LongNameMap = 2,

        /// <summary>
        /// ContainerMap
        /// </summary>
        [EnumMember]
        ContainerMap = 3,

        /// <summary>
        ///EntityTypeMap
        /// </summary>
        [EnumMember]
        EntityTypeMap = 4,

        /// <summary>
        /// SourceCategoryMap
        /// </summary>
        [EnumMember]
        SourceCategoryMap = 5,

        /// <summary>
        /// TargetCategoryMap 
        /// </summary>
        [EnumMember]
        TargetCategoryMap = 6,

        /// <summary>
        /// HierarchyParentEntityMap 
        /// </summary>
        [EnumMember]
        HierarchyParentEntityMap = 7,

        /// <summary>
        /// MDLParentEntityMap
        /// </summary>
        [EnumMember]
        MDLParentEntityMap = 8,

        /// <summary>
        /// unKnown
        /// </summary>
        [EnumMember]
        UnKnown = 0
    }



    #endregion

    #region Attribute Master Info Enums

    /// <summary>
    /// Indicates the attribute data type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeDataType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Date
        /// </summary>
        [EnumMember]
        Date = 1,
        /// <summary>
        /// String
        /// </summary>
        [EnumMember]
        String = 2,
        /// <summary>
        /// Decimal
        /// </summary>
        [EnumMember]
        Decimal = 3,
        /// <summary>
        /// Boolean
        /// </summary>
        [EnumMember]
        Boolean = 4,
        /// <summary>
        /// Image
        /// </summary>
        [EnumMember]
        Image = 5,
        /// <summary>
        /// Integer
        /// </summary>
        [EnumMember]
        Integer = 7,
        /// <summary>
        /// File
        /// </summary>
        [EnumMember]
        File = 8,
        /// <summary>
        /// Fraction
        /// </summary>
        [EnumMember]
        Fraction = 10,
        /// <summary>
        /// URL
        /// </summary>
        [EnumMember]
        URL = 12,
        /// <summary>
        /// Image URL
        /// </summary>
        [EnumMember]
        ImageURL = 13,
        /// <summary>
        /// DateTime
        /// </summary>
        [EnumMember]
        DateTime = 14,

        /// <summary>
        /// Hierarchical attribute where structure is defined by data model.
        /// </summary>
        [EnumMember]
        Hierarchical = 15
    }

    /// <summary>
    /// Indicates the attribute data type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeDisplayType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Date
        /// </summary>
        [EnumMember]
        Date = 1,
        /// <summary>
        /// Image
        /// </summary>
        [EnumMember]
        Image = 2,
        /// <summary>
        /// TextBox
        /// </summary>
        [EnumMember]
        TextBox = 3,
        /// <summary>
        /// NumericTextBox
        /// </summary>
        [EnumMember]
        NumericTextBox = 4,
        /// <summary>
        /// CheckBox
        /// </summary>
        [EnumMember]
        CheckBox = 7,
        /// <summary>
        /// TextArea 
        /// </summary>
        [EnumMember]
        TextArea = 8,
        /// <summary>
        /// File
        /// </summary>
        [EnumMember]
        DropDown = 9,
        /// <summary>
        /// File
        /// </summary>
        [EnumMember]
        File = 10,
        /// <summary>
        /// URL
        /// </summary>
        [EnumMember]
        URL = 12,
        ///// <summary>
        ///// Image URL
        ///// </summary>
        //[EnumMember]
        //PivotTable = 14,
        ///// <summary>
        ///// ExtLink
        ///// </summary>
        //[EnumMember]
        //ExtLink = 16,
        /// <summary>
        /// LookupTable
        /// </summary>
        [EnumMember]
        LookupTable = 18,
        /// <summary>
        /// ImageURL
        /// </summary>
        [EnumMember]
        ImageURL = 19,
        /// <summary>
        /// FileBrowser
        /// </summary>
        [EnumMember]
        FileBrowser = 21,
        /// <summary>
        /// DateTime
        /// </summary>
        [EnumMember]
        DateTime = 22,
        /// <summary>
        /// PickButton
        /// </summary>
        [EnumMember]
        PickButton = 23
    }


    /// <summary>
    /// Indicates the list of meta data attribute
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MetaDataAttributeList
    {
        /// <summary>
        /// Short Name
        /// </summary>
        [EnumMember]
        ShortName = 22,
        /// <summary>
        /// Long Name
        /// </summary>
        [EnumMember]
        LongName = 23,
        /// <summary>
        /// Entity Type Id
        /// </summary>
        [EnumMember]
        EntityTypeID = 27,
        /// <summary>
        /// Organization Id
        /// </summary>
        [EnumMember]
        OrganizationID = 71,
        /// <summary>
        /// Container Id
        /// </summary>
        [EnumMember]
        ContainerID = 72,
        /// <summary>
        /// Container Id
        /// </summary>
        [EnumMember]
        CategoryID = 73,
        /// <summary>
        /// Entity Id
        /// </summary>
        [EnumMember]
        EntityId = 20,

        /// <summary>
        /// Parent Entity Id
        /// </summary>
        [EnumMember]
        ParentEntityId = 74,

        /// <summary>
        /// CategoryPath
        /// </summary>
        [EnumMember]
        CategoryPath = 75

    }

    #endregion

    #region Locale Enums

    /// <summary>
    /// Indicates locales in the system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LocaleEnum
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Culture("Unknown")]
        [Description("Unknown")]
        UnKnown = 0,

        /// <summary>
        /// English world wide
        /// </summary>
        [EnumMember]
        [Culture("en-US")]
        [Description("English World Wide")]
        en_WW = 1,

        /// <summary>
        /// French World Wide
        /// </summary>
        [EnumMember]
        [Culture("fr-FR")]
        [Description("French World Wide")]
        fr_WW = 2,

        /// <summary>
        /// Spanish World Wide
        /// </summary>
        [EnumMember]
        [Culture("es-MX")]
        [Description("Spanish World Wide")]
        sp_WW = 3,

        /// <summary>
        /// German World Wide
        /// </summary>
        [EnumMember]
        [Culture("de-DE")]
        [Description("German World Wide")]
        de_WW = 4,

        /// <summary>
        /// Chinese World Wide
        /// </summary>
        [EnumMember]
        [Culture("zh-CN")]
        [Description("Chinese World Wide")]
        zh_WW = 5,

        /// <summary>
        /// Danish Denmark
        /// </summary>
        [EnumMember]
        [Culture("da-DK")]
        [Description("Danish Denmark")]
        da_DK = 16,

        /// <summary>
        /// German Austria
        /// </summary>
        [EnumMember]
        [Culture("de-AT")]
        [Description("German Austria")]
        de_AT = 18,

        /// <summary>
        /// German Switzerland
        /// </summary>
        [EnumMember]
        [Culture("de-CH")]
        [Description("German Switzerland")]
        de_CH = 19,

        /// <summary>
        /// German Germany
        /// </summary>
        [EnumMember]
        [Culture("de-DE")]
        [Description("German Germany")]
        de_DE = 21,

        /// <summary>
        /// Greek Greece
        /// </summary>
        [EnumMember]
        [Culture("el-GR")]
        [Description("Greek Greece")]
        el_GR = 22,

        /// <summary>
        /// English Canada
        /// </summary>
        [EnumMember]
        [Culture("en-CA")]
        [Description("English Canada")]
        en_CA = 24,

        /// <summary>
        /// English United Kingdom 
        /// </summary>
        [EnumMember]
        [Culture("en-GB")]
        [Description("English United Kingdom")]
        en_GB = 25,

        /// <summary>
        /// English Ireland
        /// </summary>
        [EnumMember]
        [Culture("en-IE")]
        [Description("English Ireland")]
        en_IE = 26,

        /// <summary>
        /// English United States
        /// </summary>
        [EnumMember]
        [Culture("en-US")]
        [Description("English United States")]
        en_US = 27,

        /// <summary>
        /// Spanish Spain
        /// </summary>
        [EnumMember]
        [Culture("es-ES")]
        [Description("Spanish Spain")]
        es_ES = 28,

        /// <summary>
        /// Finnish Finland
        /// </summary>
        [EnumMember]
        [Culture("fi-FI")]
        [Description("Finnish Finland")]
        fi_FI = 29,

        /// <summary>
        /// French Belgium
        /// </summary>
        [EnumMember]
        [Culture("fr-BE")]
        [Description("French Belgium")]
        fr_BE = 31,

        /// <summary>
        /// French Canada
        /// </summary>
        [EnumMember]
        [Culture("fr-CA")]
        [Description("French Canada")]
        fr_CA = 32,

        /// <summary>
        /// French Switzerland
        /// </summary>
        [EnumMember]
        [Culture("fr-CH")]
        [Description("French Switzerland")]
        fr_CH = 33,

        /// <summary>
        /// French France
        /// </summary>
        [EnumMember]
        [Culture("fr-FR")]
        [Description("French France")]
        fr_FR = 34,

        /// <summary>
        /// Italian Switzerland
        /// </summary>
        [EnumMember]
        [Culture("it-CH")]
        [Description("Italian Switzerland")]
        it_CH = 35,

        /// <summary>
        /// Italian Italy
        /// </summary>
        [EnumMember]
        [Culture("it-IT")]
        [Description("Italian Italy")]
        it_IT = 36,

        /// <summary>
        /// Japanese Japan
        /// </summary>
        [EnumMember]
        [Culture("ja-JP")]
        [Description("Japanese Japan")]
        ja_JP = 38,

        /// <summary>
        /// Korean Korea
        /// </summary>
        [EnumMember]
        [Culture("ko-KR")]
        [Description("Korean Korea")]
        ko_KR = 39,

        /// <summary>
        /// Dutch Belgium
        /// </summary>
        [EnumMember]
        [Culture("nl-BE")]
        [Description("Dutch Belgium")]
        nl_BE = 40,

        /// <summary>
        /// Dutch Netherlands
        /// </summary>
        [EnumMember]
        [Culture("nl-NL")]
        [Description("Dutch Netherlands")]
        nl_NL = 41,

        /// <summary>
        /// Norwegian (Nynorsk) Norway
        /// </summary>
        [EnumMember]
        [Culture("nn-NO")]
        [Description("Norwegian (Nynorsk) Norway")]
        no_NO = 42,

        /// <summary>
        /// Norwegian (Bokmål) Norway
        /// </summary>
        [EnumMember]
        [Culture("nb-NO")]
        [Description("Norwegian (Bokmål) Norway")]
        no_NO_B = 43,

        /// <summary>
        /// Portuguese Portugal
        /// </summary>
        [EnumMember]
        [Culture("pt-PT")]
        [Description("Portuguese Portugal")]
        pt_PT = 44,

        /// <summary>
        /// Swedish Sweden
        /// </summary>
        [EnumMember]
        [Culture("sv-SE")]
        [Description("Swedish Sweden")]
        sv_SE = 45,

        /// <summary>
        /// Turkish Turkey
        /// </summary>
        [EnumMember]
        [Culture("tr-TR")]
        [Description("Turkish Turkey")]
        tr_TR = 46,

        /// <summary>
        /// Chinese (Simplified) China
        /// </summary>
        [EnumMember]
        [Culture("zh-CN")]
        [Description("Chinese (Simplified) China")]
        zh_CN = 47,

        /// <summary>
        /// Chinese (Traditional) Taiwan
        /// </summary>
        [EnumMember]
        [Culture("zh-TW")]
        [Description("Chinese (Traditional) Taiwan")]
        zh_TW = 48,

        /// <summary>
        /// Aya Neth
        /// </summary>
        [EnumMember]
        [Culture("sq-AL")]
        [Description("Aya Neth")]
        ay_AL = 49,

        /// <summary>
        /// Thai _ India
        /// </summary>
        [EnumMember]
        [Culture("th-TH")]
        [Description("Thai _ India")]
        th_IN = 52,

        /// <summary>
        /// Spanish Venezuela
        /// </summary>
        [EnumMember]
        [Culture("es-VE")]
        [Description("Spanish Venezuela")]
        es_VE = 56,

        /// <summary>
        /// Italian WorldWide
        /// </summary>
        [EnumMember]
        [Culture("it-IT")]
        [Description("Italian WorldWide")]
        it_WW = 59,

        /// <summary>
        /// Spanish Argentina
        /// </summary>
        [EnumMember]
        [Culture("es-AR")]
        [Description("Spanish Argentina")]
        es_AR = 60,

        /// <summary>
        /// Neutral locale
        /// </summary>
        [EnumMember]
        [Culture("Neutral")]
        [Description("Neutral")]
        Neutral = 61,

        /// <summary>
        /// Japanese World Wide
        /// </summary>
        [EnumMember]
        [Culture("ja-JP")]
        [Description("Japanese World Wide")]
        ja_WW = 70,

        /// <summary>
        /// Bulgarian Bulgaria
        /// </summary>
        [EnumMember]
        [Culture("bg-BG")]
        [Description("Bulgarian Bulgaria")]
        bg_BG = 71,

        /// <summary>
        /// Czech Czech republic
        /// </summary>
        [EnumMember]
        [Culture("cs-CZ")]
        [Description("Czech Czech republic")]
        cs_CZ = 72,

        /// <summary>
        /// Estonian Estonia
        /// </summary>
        [EnumMember]
        [Culture("et-EE")]
        [Description("Estonian Estonia")]
        et_EE = 73,

        /// <summary>
        /// Croatian Croatia
        /// </summary>
        [EnumMember]
        [Culture("hr-HR")]
        [Description("Croatian Croatia")]
        hr_HR = 74,

        /// <summary>
        /// Hungarian Hungary
        /// </summary>
        [EnumMember]
        [Culture("hu-HU")]
        [Description("Hungarian Hungary")]
        hu_HU = 75,

        /// <summary>
        /// Ukrainian Ukraine
        /// </summary>
        [EnumMember]
        [Culture("uk-UA")]
        [Description("Ukrainian Ukraine")]
        uk_UA = 76,

        /// <summary>
        /// Lithuanian Lithuania
        /// </summary>
        [EnumMember]
        [Culture("lt-LT")]
        [Description("Lithuanian Lithuania")]
        lt_LT = 77,

        /// <summary>
        /// Latvian Latvia
        /// </summary>
        [EnumMember]
        [Culture("lv-LV")]
        [Description("Latvian Latvia")]
        lv_LV = 78,

        /// <summary>
        /// Polish Poland
        /// </summary>
        [EnumMember]
        [Culture("pl-PL")]
        [Description("Polish Poland")]
        pl_PL = 79,

        /// <summary>
        /// Romanian Romania
        /// </summary>
        [EnumMember]
        [Culture("ro-RO")]
        [Description("Romanian Romania")]
        ro_RO = 80,

        /// <summary>
        /// Russian Russia
        /// </summary>
        [EnumMember]
        [Culture("ru-RU")]
        [Description("Russian Russia")]
        ru_RU = 81,

        /// <summary>
        /// Slovak Slovakia
        /// </summary>
        [EnumMember]
        [Culture("sk-SK")]
        [Description("Slovak Slovakia")]
        sk_SK = 82,

        /// <summary>
        /// Slovenian Slovenia
        /// </summary>
        [EnumMember]
        [Culture("sl-SI")]
        [Description("Slovenian Slovenia")]
        sl_SI = 83,

        /// <summary>
        /// Croatian Croatia
        /// </summary>
        [EnumMember]
        [Culture("hr-HR")]
        [Description("Croatian Croatia")]
        cr_CR = 84,

        /// <summary>
        /// Spanish Mexico
        /// </summary>
        [EnumMember]
        [Culture("es-MX")]
        [Description("Spanish Mexico")]
        es_MX = 85,

        /// <summary>
        /// Greek Greece
        /// </summary>
        [EnumMember]
        [Culture("el-GR")]
        [Description("Greek Greece")]
        gr_GR = 86,

        /// <summary>
        /// Slovene Slovenia
        /// </summary>
        [EnumMember]
        [Culture("sl-SI")]
        [Description("Slovene Slovenia")]
        sl_SL = 87,

        /// <summary>
        /// Turkish Turkey
        /// </summary>
        [EnumMember]
        [Culture("tr-TR")]
        [Description("Turkish Turkey")]
        tk_TK = 88,

        /// <summary>
        /// Portuguese Brazil
        /// </summary>
        [EnumMember]
        [Culture("pt-BR")]
        [Description("Portuguese Brazil")]
        pt_BR = 89,

        /// <summary>
        /// Serbian (Cyrillic) - Serbia
        /// </summary>
        [EnumMember]
        [Culture("sr-Cyrl-RS")]
        [Description("Serbian (Cyrillic) - Serbia")]
        sr_Cyrl_RS = 90,

        /// <summary>
        /// Serbian (Latin) - Serbia
        /// </summary>
        [EnumMember]
        [Culture("sr-Latn-RS")]
        [Description("Serbian (Latin) - Serbia")]
        sr_Latn_RS = 91,

        /// <summary>
        /// Icelandic Iceland
        /// </summary>
        [EnumMember]
        [Culture("is-IS")]
        [Description("Icelandic Iceland")]
        is_IS = 92,

        /// <summary>
        /// Macedonian Macedonia
        /// </summary>
        [EnumMember]
        [Culture("mk-MK")]
        [Description("Macedonian Macedonia")]
        mk_MK = 93,

        /// <summary>
        /// English South Africa
        /// </summary>
        [EnumMember]
        [Culture("en-ZA")]
        [Description("English South Africa")]
        en_ZA = 94,

        /// <summary>
        /// Spanish Chile
        /// </summary>
        [EnumMember]
        [Culture("es-CL")]
        [Description("Spanish Chile")]
        es_CL = 95,

        /// <summary>
        /// Spanish Colombia
        /// </summary>
        [EnumMember]
        [Culture("es-CO")]
        [Description("Spanish Colombia")]
        es_CO = 96,

        /// <summary>
        /// Spanish Ecuador
        /// </summary>
        [EnumMember]
        [Culture("es-EC")]
        [Description("Spanish Ecuador")]
        es_EC = 97,

        /// <summary>
        /// Spanish Guatemala
        /// </summary>
        [EnumMember]
        [Culture("es-GT")]
        [Description("Spanish Guatemala")]
        es_GT = 98,

        /// <summary>
        /// Spanish Paraguay
        /// </summary>
        [EnumMember]
        [Culture("es-PY")]
        [Description("Spanish Paraguay")]
        es_PY = 99,

        /// <summary>
        /// Spanish Peru
        /// </summary>
        [EnumMember]
        [Culture("es-PE")]
        [Description("Spanish Peru")]
        es_PE = 100,

        /// <summary>
        /// Spanish Uruguay
        /// </summary>
        [EnumMember]
        [Culture("es-UY")]
        [Description("Spanish Uruguay")]
        es_UY = 101,

        /// <summary>
        /// English Australia
        /// </summary>
        [EnumMember]
        [Culture("en-AU")]
        [Description("English Australia")]
        en_AU = 102,

        /// <summary>
        /// English New Zealand
        /// </summary>
        [EnumMember]
        [Culture("en-NZ")]
        [Description("English New Zealand")]
        en_NZ = 103,

        /// <summary>
        /// Armenian Armenia
        /// </summary>
        [EnumMember]
        [Culture("hy-AM")]
        [Description("Armenian Armenia")]
        hy_AM = 104,

        /// <summary>
        /// Georgian Georgia
        /// </summary>
        [EnumMember]
        [Culture("ka-GE")]
        [Description("Georgian Georgia")]
        ka_GE = 105,

        /// <summary>
        /// Indonesian Indonesia
        /// </summary>
        [EnumMember]
        [Culture("id-ID")]
        [Description("Indonesian Indonesia")]
        id_ID = 106,

        /// <summary>
        /// English Malaysia
        /// </summary>
        [EnumMember]
        [Culture("en-MY")]
        [Description("English Malaysia")]
        en_MY = 107,

        /// <summary>
        /// English Republic of the Philippines
        /// </summary>
        [EnumMember]
        [Culture("en-PH")]
        [Description("English Republic of the Philippines")]
        en_PH = 108,

        /// <summary>
        /// Thai Thailand
        /// </summary>
        [EnumMember]
        [Culture("th-TH")]
        [Description("Thai Thailand")]
        th_TH = 109,

        /// <summary>
        /// Vietnamese Vietnam
        /// </summary>
        [EnumMember]
        [Culture("vi-VN")]
        [Description("Vietnamese Vietnam")]
        vi_VN = 110,

        /// <summary>
        /// All Locale
        /// </summary>
        [EnumMember]
        [Description("All supported locales")]
        rs_ALL = 111

    }

    /// <summary>
    /// Indicates locales in the system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LocaleType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Data Locale type
        /// </summary>
        [EnumMember]
        DataLocale = 1,

        /// <summary>
        /// UI Locale type
        /// </summary>
        [EnumMember]
        UILocale = 2
    }

    #endregion

    #region Authentication Enums

    /// <summary>
    /// Defines authentication types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AuthenticationType
    {
        /// <summary>
        /// Forms Authentication 
        /// </summary>
        [EnumMember]
        Forms = 0,

        /// <summary>
        /// Windows authentication
        /// </summary>
        [EnumMember]
        Windows = 1,

        /// <summary>
        /// Claims based authentication
        /// </summary>
        [EnumMember]
        Claims = 2,

        /// <summary>
        /// Hybrid
        /// </summary>
        [EnumMember]
        Hybrid = 3,

        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        Unknown = 4,

        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 5
    }

    /// <summary>
    /// Defines the claims based authentication types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ClaimsBasedAuthenticationType
    {
        /// <summary>
        /// None - Specifies that Claims based authentication is disabled. 
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Represents Claims based authentication using WSFederation.
        /// </summary>
        [EnumMember]
        WSFederation = 1,

        /// <summary>
        /// Represents Claims based authentication using SAML2.
        /// </summary>
        [EnumMember]
        SAML2 = 2
    }

    /// <summary>
    /// Defines WCFBindingType
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WCFBindingType
    {
        /// <summary>
        /// Forms Authentication 
        /// </summary>
        [EnumMember]
        NetTcpBinding = 1,

        /// <summary>
        /// Windows authentication
        /// </summary>
        [EnumMember]
        WSHttpBinding = 2,

        /// <summary>
        /// Hybrid
        /// </summary>
        [EnumMember]
        BasicHttpBinding = 3,

        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        Unknown = 4,

        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 5
    }

    /// <summary>
    /// Defines all MDM WCF services
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMWCFServiceList
    {
        /// <summary>
        /// AuthenticationService 
        /// </summary>
        [EnumMember]
        AuthenticationService = 1,

        /// <summary>
        /// CoreService
        /// </summary>
        [EnumMember]
        CoreService = 2,

        /// <summary>
        /// ConfigurationService 
        /// </summary>
        [EnumMember]
        ConfigurationService = 3,

        /// <summary>
        /// DataModelService
        /// </summary>
        [EnumMember]
        DataModelService = 4,

        /// <summary>
        /// DataService 
        /// </summary>
        [EnumMember]
        DataService = 5,

        /// <summary>
        /// IntegrationService 
        /// </summary>
        [EnumMember]
        IntegrationService = 6,

        /// <summary>
        /// KnowledgeBaseService
        /// </summary>
        [EnumMember]
        KnowledgeBaseService = 7,

        /// <summary>
        /// MessageService
        /// </summary>
        [EnumMember]
        MessageService = 8,

        /// <summary>
        /// SecurityService
        /// </summary>
        [EnumMember]
        SecurityService = 9,

        /// <summary>
        /// Workflow Service
        /// </summary>
        [EnumMember]
        WorkflowService = 11,

        /// <summary>
        /// Workflow designer Service
        /// </summary>
        [EnumMember]
        WorkflowDesignerService = 12,

        /// <summary>
        /// Denorm Service
        /// </summary>
        [EnumMember]
        DenormService = 13,

        /// <summary>
        /// Matching Service
        /// </summary>
        [EnumMember]
        MatchingService = 14,

        /// <summary>
        /// Internal common service
        /// </summary>
        [EnumMember]
        InternalCommonService = 15
    }

    #endregion

    #region Denorm Enum

    /// <summary>
    /// Defines all type of Denorm
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DenormType
    {
        /// <summary>
        /// Metadata Denorm
        /// </summary>
        [EnumMember]
        Metadata = 1,

        /// <summary>
        /// Category Denorm
        /// </summary>
        [EnumMember]
        Category = 2,

        /// <summary>
        /// Delta Denorm
        /// </summary>
        [EnumMember]
        Delta = 3,

        /// <summary>
        /// All denorm
        /// </summary>
        [EnumMember]
        All = 4,

        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        UnKnown = 5,
    }

    /// <summary>
    /// Defines entity comparison type. Is entity is to be compared for current entity or impacted entities
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RefreshType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates full refresh. Refreshes all attributes.
        /// </summary>
        [EnumMember]
        Full = 1,

        /// <summary>
        /// Indicates delta refresh.
        /// </summary>
        [EnumMember]
        Delta = 2
    }

    /// <summary>
    /// Defines entity comparison type. Is entity is to be compared for current entity or impacted entities
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CoreDataProcessorList
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        ///// <summary>
        ///// Attribute/Relationship denorm data processor
        ///// </summary>
        //[EnumMember]
        //[Description("Attribute/Relationship Denorm Processor")]
        //EntityDenormProcessor = 1,

        /// <summary>
        /// Delta export processor
        /// </summary>
        [EnumMember]
        [Description("DeltaExportProcessor")]
        DeltaExportProcessor = 3,

        /// <summary>
        /// Entity activity log processor
        /// </summary>
        [EnumMember]
        [Description("EntityActivityLogProcessor")]
        EntityActivityLogProcessor = 4,

        /// <summary>
        /// Entity queue processor
        /// </summary>
        [EnumMember]
        [Description("EntityQueueProcessor")]
        EntityQueueProcessor = 5,

        /// <summary>
        /// Entity hierarchy processor
        /// </summary>
        [EnumMember]
        [Description("EntityHierarchyProcessor")]
        EntityHierarchyProcessor = 6,

        /// <summary>
        /// Entity extension processor
        /// </summary>
        [EnumMember]
        [Description("EntityExtensionProcessor")]
        EntityExtensionProcessor = 7,

        /// <summary>
        /// Entity Async Processor
        /// </summary>
        [EnumMember]
        [Description("EntityAsyncProcessor")]
        EntityAsyncProcessor = 8,

        /// <summary>
        /// DQM summarization processor
        /// </summary>
        [EnumMember]
        [Description("SummarizationProcessor")]
        SummarizationProcessor = 10,

        /// <summary>
        /// Entity Cache loading processor
        /// </summary>
        [EnumMember]
        [Description("EntityCacheProcessor")]
        EntityCacheProcessor = 13,

        /// <summary>
        /// Integration qualifying queue processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationQualifyingQueueProcessor")]
        IntegrationQualifyingQueueProcessor = 14,

        /// <summary>
        /// Integration qualifying queue load processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationQualifyingQueueLoadProcessor")]
        IntegrationQualifyingQueueLoadProcessor = 15,

        /// <summary>
        /// Integration outbound queue processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationQualifyingQueueLoadProcessor")]
        IntegrationOutboundQueueProcessor = 16,

        /// <summary>
        /// DQM Job Finalization Processor
        /// </summary>
        [EnumMember]
        [Description("DQMJobFinalizationProcessor")]
        DQMJobFinalizationProcessor = 17,

        /// <summary>
        /// DQM Job Normalization Processor
        /// </summary>
        [EnumMember]
        [Description("NormalizationProcessor")]
        NormalizationProcessor = 18,

        /// <summary>
        /// DQM Job Initialization Processor
        /// </summary>
        [EnumMember]
        [Description("DQMJobInitializationProcessor")]
        DQMJobInitializationProcessor = 19,

        /// <summary>
        /// Integration inbound queue processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationInboundQueueProcessor")]
        IntegrationInboundQueueProcessor = 20,

        /// <summary>
        /// Validation processor
        /// </summary>
        [EnumMember]
        [Description("ValidationProcessor")]
        ValidationProcessor = 21,

        /// <summary>
        /// Aggregation inbound queue processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationInboundAggregationQueueProcessor")]
        IntegrationInboundAggregationQueueProcessor = 22,

        /// <summary>
        /// Aggregation outbound queue processor
        /// </summary>
        [EnumMember]
        [Description("IntegrationOutboundAggregationQueueProcessor")]
        IntegrationOutboundAggregationQueueProcessor = 23,

        /// <summary>
        /// Match Processor.
        /// </summary>
        [EnumMember]
        [Description("MatchProcessor")]
        MatchProcessor = 24,

        /// <summary>
        /// Merging processor
        /// </summary>
        [EnumMember]
        [Description("MergingProcessor")]
        MergingProcessor = 25,

        /// <summary>
        /// Match Store Load processor
        /// </summary>
        [EnumMember]
        [Description("MatchStoreDeltaLoadProcessor")]
        MatchStoreDeltaLoadProcessor = 26,

        /// <summary>
        /// Merge Planning processor
        /// </summary>
        [EnumMember]
        [Description("MergePlanningProcessor")]
        MergePlanningProcessor = 27,

        /// <summary>
        /// Match Store Initial Load Processor
        /// </summary>
        [EnumMember]
        [Description("MatchStoreInitialLoadProcessor")]
        MatchStoreInitialLoadProcessor = 28,

        /// <summary>
        /// Diagnostic data processor
        /// </summary>
        [EnumMember]
        [Description("DiaganosticDataProcessor")]
        DiaganosticDataProcessor = 29,

        /// <summary>
        /// Strongly Typed Entity Cache Processor
        /// </summary>
        [EnumMember]
        [Description("StronglyTypedEntityCacheProcessor")]
        StronglyTypedEntityCacheProcessor = 30,

        /// <summary>
        /// Entity Family Processor
        /// </summary>
        [EnumMember]
        [Description("EntityFamilyProcessor")]
        EntityFamilyProcessor = 31,

        /// <summary>
        /// Promote Processor
        /// </summary>
        [EnumMember]
        [Description("PromoteProcessor")]
        PromoteProcessor = 32,

        /// <summary>
        /// DDG processor
        /// </summary>
        [EnumMember]
        [Description("DDGProcessor")]
        DDGProcessor = 33,

        /// <summary>
        /// Jigsaw Integration data processor
        /// </summary>
        [EnumMember]
        [Description("JigsawIntegrationInbondProcessor")]
        JigsawIntegrationInbondProcessor = 34,

        /// <summary>
        /// Jigsaw Data Synchronization data processor
        /// </summary>
        [EnumMember]
        [Description("JigsawDataSynchronizationProcessor")]
        JigsawDataSynchronizationProcessor = 35,

        /// <summary>
        /// Metadata Change processor
        /// </summary>
        [EnumMember]
        [Description("MetadataChangeProcessor")]
        MetadataChangeProcessor = 36
    }

    /// <summary>
    /// Defines the Impacted entity Process Status.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ProcessingStatus
    {
        /// <summary>
        /// Entity which is currently being processed
        /// </summary>
        [EnumMember]
        Current = 0,

        /// <summary>
        ///  Entity which is already processed
        /// </summary>
        [EnumMember]
        History = 1,

        /// <summary>
        /// Entity which has to be processed
        /// </summary>
        [EnumMember]
        Pending = 2,

        /// <summary>
        /// All actions
        /// </summary>
        [EnumMember]
        All = 3
    }

    /// <summary>
    /// Defines the EntityActivityLogItemState
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityActivityLogItemState
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("UnKnown")]
        UnKnown = 0,

        /// <summary>
        /// Initial
        /// </summary>
        [EnumMember]
        [Description("Initial")]
        Initial = 1,

        /// <summary>
        /// Loading
        /// </summary>
        [EnumMember]
        [Description("Loading")]
        Loading = 2,

        /// <summary>
        /// Loaded
        /// </summary>
        [EnumMember]
        [Description("Loaded")]
        Loaded = 3,

        /// <summary>
        /// Processing
        /// </summary>
        [EnumMember]
        [Description("Processing")]
        Processing = 4,

        /// <summary>
        /// Processed
        /// </summary>
        [EnumMember]
        [Description("Processed")]
        Processed = 5
    }

    /// <summary>
    /// Defines the JobQueueItemState
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JobQueueItemState
    {
        /// <summary>
        /// Initial
        /// </summary>
        [EnumMember]
        [Description("Initial")]
        Initial = 0,

        /// <summary>
        /// Initializing
        /// </summary>
        [EnumMember]
        [Description("Initializing")]
        Initializing = 1,

        /// <summary>
        /// Initialized
        /// </summary>
        [EnumMember]
        [Description("Initialized")]
        Initialized = 2,

        /// <summary>
        /// Processing
        /// </summary>
        [EnumMember]
        [Description("Processing")]
        Processing = 3,

        /// <summary>
        /// Finalizing
        /// </summary>
        [EnumMember]
        [Description("Finalizing")]
        Finalizing = 4,

        /// <summary>
        /// Processed
        /// </summary>
        [EnumMember]
        [Description("Processed")]
        Processed = 5,

        /// <summary>
        /// Canceled
        /// </summary>
        [EnumMember]
        [Description("Canceled")]
        Canceled = 6
    }

    /// <summary>
    /// Defines the list of services running in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMServiceType
    {
        /// <summary>
        /// Unknown Service Type
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Parallel processing engine
        /// </summary>
        [EnumMember]
        APIEngine = 1,

        /// <summary>
        /// Job service
        /// </summary>
        [EnumMember]
        JobService = 2,

        /// <summary>
        /// Matching Service monitoring type
        /// </summary>
        [EnumMember]
        MatchingService = 3,

        /// <summary>
        /// Exclude from DNI
        /// </summary>
        [EnumMember]
        Flattened = 4,

        /// <summary>
        /// Exclude from DN_Search
        /// </summary>
        [EnumMember]
        Searchable = 5,

        /// <summary>
        /// Exclude from DN
        /// </summary>
        [EnumMember]
        FlattenedColumnar = 6,

        /// <summary>
        /// Web Server
        /// </summary>
        [EnumMember]
        Web = 7
    }

    /// <summary>
    /// Defines the types of services running in MDM system
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMServiceSubType
    {
        /// <summary>
        /// Unknown Service SubType
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Import as a sub type of job service
        /// </summary>
        [EnumMember]
        Import = 1,

        /// <summary>
        /// Export as a sub type of job service
        /// </summary>
        [EnumMember]
        Export = 2,

        /// <summary>
        /// Parallel processing engine as sub type of APIEngine
        /// </summary>
        [EnumMember]
        ParallelProcessingEngine = 3,

        /// <summary>
        /// Matching engine as a subtype of MatchingService
        /// </summary>
        [EnumMember]
        MatchingEngine = 4,

        /// <summary>
        /// ALL as a subtype of Job Service. This type is set when the job service handle both Import and Export
        /// </summary>
        [EnumMember]
        ALL = 5
    }

    /// <summary>
    /// Defines entity activity
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityActivityList
    {
        /// <summary>
        /// Any
        /// </summary>
        [EnumMember]
        Any = 0,

        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        UnKnown = 100,

        /// <summary>
        /// AttributeUpdate
        /// </summary>
        [EnumMember]
        AttributeUpdate = 1,

        /// <summary>
        /// ParentHierarchyChange
        /// </summary>
        [EnumMember]
        ParentHierarchyChange = 3,

        /// <summary>
        /// ParentExtensionChange
        /// </summary>
        [EnumMember]
        ParentExtensionChange = 4,

        /// <summary>
        /// EntityReclassify
        /// </summary>
        [EnumMember]
        EntityReclassify = 5,

        /// <summary>
        /// Rename
        /// </summary>
        [EnumMember]
        Rename = 6,

        /// <summary>
        /// ChildHierarchyChange
        /// </summary>
        [EnumMember]
        ChildHierarchyChange = 7,

        /// <summary>
        /// ChildExtensionChange
        /// </summary>
        [EnumMember]
        ChildExtensionChange = 8,

        /// <summary>
        /// EntityCreate
        /// </summary>
        [EnumMember]
        EntityCreate = 9,

        /// <summary>
        /// EntityUpdate
        /// </summary>
        [EnumMember]
        EntityUpdate = 10,

        /// <summary>
        /// EntityDelete
        /// </summary>
        [EnumMember]
        EntityDelete = 11,

        /// <summary>
        /// EntityAsyncCreate
        /// </summary>
        [EnumMember]
        EntityAsyncCreate = 12,

        /// <summary>
        /// EntityAsyncUpdate
        /// </summary>
        [EnumMember]
        EntityAsyncUpdate = 13,

        /// <summary>
        /// EntityAsyncDelete
        /// </summary>
        [EnumMember]
        EntityAsyncDelete = 14,

        /// <summary>
        /// Asynchronous loading of entities in cache
        /// </summary>
        [EnumMember]
        [Description("Asynchronous loading of entities in cache")]
        EntityCacheLoad = 19,

        /// <summary>
        /// Asynchronous loading of entities in cache
        /// </summary>
        [EnumMember]
        [Description("Workflow Attribute Update for an entity")]
        WorkflowAttributeUpdate = 20,
        /// <summary>
        /// Relationship Create Activity
        /// </summary>
        [EnumMember]
        [Description("Relationship Create Activity")]
        RelationshipCreate = 21,

        /// <summary>
        /// RelationshipUpdate
        /// </summary>
        [EnumMember]
        [Description("Relationship Update Activity")]
        RelationshipUpdate = 22,

        /// <summary>
        /// Relationship Delete Activity
        /// </summary>
        [EnumMember]
        [Description("Relationship Delete Activity")]
        RelationshipDelete = 23,

        /// <summary>
        /// Relationship Attribute Update Activity
        /// </summary>
        [EnumMember]
        [Description("Relationship Attribute Update Activity")]
        RelationshipAttributeUpdate = 24,

        /// <summary>
        /// Relationship Attribute Update Activity
        /// </summary>
        [EnumMember]
        [Description("Entity ReParent Activity")]
        EntityReParent = 25,

        /// <summary>
        /// VariantsChange
        /// </summary>
        [EnumMember]
        [Description("Variants change for an entity")]
        VariantsChange = 26,

        /// <summary>
        /// ExtensionChange
        /// </summary>
        [EnumMember]
        [Description("Extension change for an entity")]
        ExtensionChange = 27,

        /// <summary>
        /// Promote
        /// </summary>
        [EnumMember]
        [Description("Promote for an entity")]
        Promote = 28,

        /// <summary>
        /// AutoPromote
        /// </summary>
        [EnumMember]
        [Description("Auto Promote for an entity")]
        AutoPromote = 29,

        /// <summary>
        /// EmergencyPromote
        /// </summary>
        [EnumMember]
        [Description("Emergency Promote for an entity")]
        EmergencyPromote = 30,

        /// <summary>
        /// UpstreamPromote
        /// </summary>
        [EnumMember]
        [Description("Upstream Promote for an entity")]
        UpstreamPromote = 31,

        /// <summary>
        /// CategoryPromote
        /// </summary>
        [EnumMember]
        [Description("Category Promote for an entity")]
        CategoryPromote = 32,

        /// <summary>
        /// WorkflowChange
        /// </summary>
        [EnumMember]
        [Description("Workflow change for an entity")]
        WorkflowChange = 33,

        /// <summary>
        /// EntityAsyncWorkflowActivityBusinessRules
        /// </summary>
        [EnumMember]
        [Description("ExecuteEntityBusinessRules for an entity asynchronously")]
        EntityAsyncWorkflowActivityBusinessRules = 34,

        /// <summary>
        /// Entity Re-validate
        /// </summary>
        [EnumMember]
        [Description("Entity re-validate")]
        EntityRevalidate = 35,

        /// <summary>
        /// Category Change
        /// </summary>
        [EnumMember]
        [Description("Category Change")]
        CategoryChange = 36,

        /// <summary>
        /// Metadata Change
        /// </summary>
        [EnumMember]
        [Description("Metadata Change")]
        MetadataChange = 37
    }

    #endregion

    #region Template Enums

    /// <summary>
    /// Defines the Type of Template
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum TemplateType
    {
        /// <summary>
        /// Unknown Template
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Import Template
        /// </summary>
        [EnumMember]
        Import = 1,

        /// <summary>
        /// Export Template
        /// </summary>
        [EnumMember]
        Export = 2
    }

    #endregion

    #region InjectionSearchCriteria

    /// <summary>
    /// Defines the how the Value of an Attribute is injected into search criteria
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeValueInjectionMode
    {
        /// <summary>
        /// merge the value if attribute exists in the search criteria
        /// </summary>
        [EnumMember]
        Merge = 0,

        /// <summary>
        /// flush the value of the existing attribute and add the new value
        /// </summary>
        [EnumMember]
        FlushAndFill = 1,

    }

    #endregion  InjectionSearchCriteria

    #region ReParent Type

    /// <summary>
    /// Defines the type of ReParenting
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ReParentTypeEnum
    {
        /// <summary>
        /// UnKnown
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Category ReParent
        /// </summary>
        [EnumMember]
        CategoryReParent = 1,

        /// <summary>
        /// Hierarchy ReParent
        /// </summary>
        [EnumMember]
        HiearchyReParent = 2,

        /// <summary>
        /// Extension ReParent
        /// </summary>
        [EnumMember]
        ExtensionReParent = 3
    }

    #endregion

    #region Export Enums

    /// <summary>
    /// Attribute column header options RSExcel export
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RSExcelAttributeColumnHeaderType
    {
        /// <summary>
        /// Attribute and attribute parent short name 
        /// </summary>
        [EnumMember]
        [Description("Attribute And Attribute Parent Long Name")]
        AttributeAndAttributeParentLongName = 0,

        /// <summary>
        /// Attribute short name 
        /// </summary>
        [EnumMember]
        [Description("Attribute Short Name")]
        AttributeShortName = 1,

        /// <summary>
        /// Attribute and attribute parent long name
        /// </summary>
        [EnumMember]
        [Description("Attribute And Attribute Parent Short Name")]
        AttributeAndAttributeParentShortName = 2,

        /// <summary>
        /// Attribute and attribute parent short name with locale
        /// </summary>
        [EnumMember]
        [Description("Attribute And Attribute Parent Short Name With Locale")]
        AttributeAndAttributeParentShortNameWithLocale = 3,

        /// <summary>
        /// Attribute long name
        /// </summary>
        [EnumMember]
        [Description("Attribute Long Name")]
        AttributeLongName = 4,

        /// <summary>
        /// Attribute and attribute parent long name with locale
        /// </summary>
        [EnumMember]
        [Description("Attribute And Attribute Parent Long Name With Locale")]
        AttributeAndAttributeParentLongNameWithLocale = 5
    }

    /// <summary>
    /// Column value for category path 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RSExcelCategoryPathType
    {
        /// <summary>
        /// Category short name path
        /// </summary>
        [EnumMember]
        ShortNamePath = 0,

        /// <summary>
        /// Category long name path
        /// </summary>
        [EnumMember]
        LongNamePath = 1
    }

    /// <summary>
    /// Type of export subscriber
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportSubscriberType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// File folder
        /// </summary>
        [EnumMember]
        FileSystem = 1,

        /// <summary>
        /// FTP
        /// </summary>
        [EnumMember]
        FTP = 2,

        /// <summary>
        /// MSMQ
        /// </summary>
        [EnumMember]
        MSMQ = 3,

        /// <summary>
        /// JSMQ
        /// </summary>
        [EnumMember]
        JMSQ = 4,

        /// <summary>
        /// Tibco JMSQ
        /// </summary>
        [EnumMember]
        TibcoJMSQ = 5,

        /// <summary>
        /// MQ Series
        /// </summary>
        [EnumMember]
        MQSeries = 6,

        /// <summary>
        /// AMQP
        /// </summary>
        [EnumMember]
        AMQP = 7
    }

    /// <summary>
    /// Type of export job status
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportJobStatus
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// OnBegin
        /// </summary>
        [EnumMember]
        Begin = 1,

        /// <summary>
        /// OnComplete
        /// </summary>
        [EnumMember]
        Complete = 2,

        /// <summary>
        /// OnFailure
        /// </summary>
        [EnumMember]
        Failure = 3,

        /// <summary>
        /// OnSuccess
        /// </summary>
        [EnumMember]
        Success = 4,

        /// <summary>
        /// Ignored
        /// </summary>
        [EnumMember]
        Ignore = 5
    }

    /// <summary>
    /// Type of export profile
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportProfileType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// UIEntityExport
        /// </summary>
        [EnumMember]
        EntityExportUIProfile = 1,

        /// <summary>
        /// SyndicationEntityExport
        /// </summary>
        [EnumMember]
        EntityExportSyndicationProfile = 2,

        /// <summary>
        /// UILookupExport
        /// </summary>
        [EnumMember]
        LookupExportUIProfile = 3,

        /// <summary>
        /// UILookupExport
        /// </summary>
        [EnumMember]
        LookupExportSyndicationProfile = 4,

        /// <summary>
        /// Translation Export Profile
        /// </summary>
        [EnumMember]
        EntityTranslationExportProfile = 5,

        /// <summary>
        /// Lookup Translation Export Profile
        /// </summary>
        [EnumMember]
        LookupTranslationExportProfile = 6,

        /// <summary>
        /// Data Model Export UI Profile
        /// </summary>
        [EnumMember]
        DataModelExportUIProfile = 7,

        /// <summary>
        /// Dynamic Governance Export UI Profile
        /// </summary>
        [EnumMember]
        DynamicGovernanceExportUIProfile = 8
    }

    /// <summary>
    /// Export execution model
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportExecutionMode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Full
        /// </summary>
        [EnumMember]
        [Description("Full")]
        Full = 1,

        /// <summary>
        /// Delta
        /// </summary>
        [EnumMember]
        [Description("Delta")]
        Delta = 2
    }

    /// <summary>
    /// Export execution delta type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportDeltaExecutionType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Full
        /// </summary>
        [EnumMember]
        Item = 1,

        /// <summary>
        /// Delta
        /// </summary>
        [EnumMember]
        Attribute = 2
    }

    /// <summary>
    /// Export execution delta type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportFileSplitType
    {
        /// <summary>
        /// No split
        /// </summary>
        [EnumMember]
        NoSplit = 0,

        /// <summary>
        /// Split the file by entity type.
        /// </summary>
        [EnumMember]
        SplitByEntityType = 1,

        /// <summary>
        /// Split the file by locale.
        /// </summary>
        [EnumMember]
        SplitByLocale = 2,

        /// <summary>
        /// Split the file by entity count.
        /// </summary>
        [EnumMember]
        SplitByEntityCount = 3,

        /// <summary>
        /// Split the file by entity type and locale.
        /// </summary>
        [EnumMember]
        SplitByEntityTypeAndLocale = 4,

        /// <summary>
        /// Split the file by entity type and count.
        /// </summary>
        [EnumMember]
        SplitByEntityTypeAndCount = 5,

        /// <summary>
        /// Split the file by locale and count.
        /// </summary>
        [EnumMember]
        SplitByEntityLocaleAndCount = 6,


        /// <summary>
        /// All.
        /// </summary>
        [EnumMember]
        All = 7
    }


    /// <summary>
    /// Export Entity Group By
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportEntityGroupBy
    {
        /// <summary>
        /// None 
        /// </summary>
        [EnumMember]
        [Description("None")]
        None = 0,

        /// <summary>
        /// Family Together - 
        /// </summary>
        [EnumMember]
        [Description("Entity Family")]
        FamilyTogether = 1,

        /// <summary>
        /// Entity Type Together - 
        /// </summary>
        [EnumMember]
        [Description("Entity Type")]
        EntityTypeTogether = 2
    }

    /// <summary>
    /// Export Attribute Sort Order
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportAttributeSortOrder
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        [Description("None")]
        None = 0,

        /// <summary>
        /// Sort Oder and Alphabetical
        /// </summary>
        [EnumMember]
        [Description("Sort Order then Alphabetical")]
        SortOrderThenByAlphabetical = 1,

        /// <summary>
        /// Alphabetical
        /// </summary>
        [EnumMember]
        [Description("Alphabetical")]
        Alphabetical = 2
    }

    /// <summary>
    /// Export Entity Type Modes
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityTypeMode
    {
        /// <summary>
        /// Indicates Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Export includes all promotable Container extensions will be part of the Entity Family
        /// </summary>
        [EnumMember]
        FullFamily = 1,

        /// <summary>
        /// Export includes entity family in only specific container
        /// </summary>
        [EnumMember]
        VariantFamily = 2,

        /// <summary>
        /// Export Includes the extension parent lineage
        /// </summary>
        [EnumMember]
        MyModifiedFamilyLineage = 3,

        /// <summary>
        ///  Exports only root level 
        /// </summary>
        [EnumMember]
        OnlyRoot = 4,

        /// <summary>
        ///  Exports only leaf level 
        /// </summary>
        [EnumMember]
        OnlyLeaf = 5
    }

    /// <summary>
    /// Export Entity Variant
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportEntityVariantMode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Include Variant Children
        /// </summary>
        [EnumMember]
        IncludeVariantChildren = 1,

        /// <summary>
        /// Include Variant Lineage
        /// </summary>
        [EnumMember]
        IncludeVariantLineage = 2,

        /// <summary>
        /// Include Variant Children and Lineage
        /// </summary>
        [EnumMember]
        IncludeVariantChildrenAndLineage = 3

    }

    /// <summary>
    /// Export Entity Extension
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportEntityExtensionMode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Include Extension Children
        /// </summary>
        [EnumMember]
        IncludeExtensionChildren = 1,

        /// <summary>
        /// Include Extension Lineage
        /// </summary>
        [EnumMember]
        IncludeExtensionLineage = 2,

        /// <summary>
        /// Include Extension Children and Lineage
        /// </summary>
        [EnumMember]
        IncludeExtensionChildrenAndLineage = 3

    }

    #region Lookup Export Enums

    /// <summary>
    ///Specifies the options available for lookup export group order
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LookupExportGroupOrder
    {
        /// <summary>
        /// Represents the un-identified group order
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Specifies column name as the lookup group order
        /// </summary>
        [EnumMember]
        ColumnName = 1,

        /// <summary>
        /// Specifies Locale as the lookup group order
        /// </summary>
        [EnumMember]
        Locale = 2
    }

    /// <summary>
    /// Specifies the lookup export file formats.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LookupExportFileFormat
    {
        /// <summary>
        /// Represents the un-identified file format
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Represents the RsLookup Excel 1.0 version file format.
        /// Supported file format is xlsx
        /// </summary>
        [EnumMember]
        RSLookupExcel10 = 1,

        /// <summary>
        /// Represents the RSLookup Xml 1.0 version file format.
        /// Supported file format is xml
        /// </summary>
        [EnumMember]
        RSLookupXml10 = 2
    }

    #endregion

    /// <summary>
    /// Export Formatter Export Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportFormatterExportType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Entity,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Lookup,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Translation

    }

    #endregion Export Enums

    #region Execution Type

    /// <summary>
    /// Defines the execution type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExecutionType
    {
        /// <summary>
        /// Indicates Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates Synchonized
        /// </summary>
        [EnumMember]
        Synchonized = 1,

        /// <summary>
        /// Indicates Asynchronized
        /// </summary>
        [EnumMember]
        Asynchronized = 2
    }

    #endregion

    #region Entity History Enums

    /// <summary>
    /// Indicates sub type for change done for an entity
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityChangeType
    {
        /// <summary>
        /// Indicates Unknown
        /// </summary>
        [EnumMember]
        [UIDisplayName("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Indicates Metadata
        /// </summary>
        [EnumMember]
        [UIDisplayName("Metadata")]
        Metadata = 1,

        /// <summary>
        /// Indicates Metadata_LongName
        /// </summary>
        [EnumMember]
        [UIDisplayName("Metadata")]
        Metadata_LongName = 2,

        /// <summary>
        /// Indicates Metadata_Category
        /// </summary>
        [EnumMember]
        [UIDisplayName("Metadata")]
        Metadata_Category = 3,

        /// <summary>
        /// Indicates CommonAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Common Attribute")]
        CommonAttribute = 4,

        /// <summary>
        /// Indicates CommonComplexAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Common Attribute")]
        CommonComplexAttribute = 5,

        /// <summary>
        /// Indicates TechnicalAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Technical Attribute")]
        TechnicalAttribute = 6,

        /// <summary>
        /// Indicates TechnicalComplexAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Technical Attribute")]
        TechnicalComplexAttribute = 7,

        /// <summary>
        /// Indicates RelationshipAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Relationship Attribute")]
        RelationshipAttribute = 8,

        /// <summary>
        /// Indicates Relationship
        /// </summary>
        [EnumMember]
        [UIDisplayName("Relationship")]
        Relationship = 9,

        /// <summary>
        /// Indicates ExtensionRelationship
        /// </summary>
        [EnumMember]
        [UIDisplayName("Extension Relationship")]
        ExtensionRelationship = 10,

        /// <summary>
        /// Indicates HierarchyRelationship
        /// </summary>
        [EnumMember]
        [UIDisplayName("Hierarchy Relationship")]
        HierarchyRelationship = 11,

        /// <summary>
        /// Indicates N_Level_HierarchyRelationship
        /// </summary>
        [EnumMember]
        [UIDisplayName("Hierarchy Relationship")]
        N_Level_HierarchyRelationship = 12,

        /// <summary>
        /// Indicates Workflow
        /// </summary>
        [EnumMember]
        [UIDisplayName("Workflow")]
        Workflow = 13,

        /// <summary>
        /// Indicates Workflow_StageTransition
        /// </summary>
        [EnumMember]
        [UIDisplayName("Workflow")]
        Workflow_StageTransition = 14,

        /// <summary>
        /// Indicates Workflow_Assignment
        /// </summary>
        [EnumMember]
        [UIDisplayName("Workflow")]
        Workflow_Assignment = 15,

        /// <summary>
        /// Indicates CommonFileOrImageAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Common Attribute")]
        CommonFileOrImageAttribute = 16,

        /// <summary>
        /// Indicates CommonURLAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Common Attribute")]
        CommonURLAttribute = 17,

        /// <summary>
        /// Indicates TechnicalFileOrImageAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Technical Attribute")]
        TechnicalFileOrImageAttribute = 18,

        /// <summary>
        /// Indicates TechnicalURLAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Technical Attribute")]
        TechnicalURLAttribute = 19,

        /// <summary>
        /// Indicates CommonHierarchicalAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Common Attribute")]
        CommonHierarchicalAttribute = 20,

        /// <summary>
        /// Indicates TechnicalHierarchicalAttribute
        /// </summary>
        [EnumMember]
        [UIDisplayName("Technical Attribute")]
        TechnicalHierarchicalAttribute = 21,

        /// <summary>
        /// Indicates Promote_Workflow
        /// </summary>
        [EnumMember]
        [UIDisplayName("Promote")]
        Promote = 22,

        /// <summary>
        /// Indicates Promote_Workflow
        /// </summary>
        [EnumMember]
        [UIDisplayName("Workflow Based Promote")]
        Promote_Workflow = 23,

        /// <summary>
        /// Indicates Promote_DDG
        /// </summary>
        [EnumMember]
        [UIDisplayName("DDG Based Promote")]
        Promote_DDG = 24,

        /// <summary>
        /// Indicates Promote_DDG
        /// </summary>
        [EnumMember]
        [UIDisplayName("Auto Promote")]
        AutoPromote = 25,

        /// <summary>
        /// Indicates Promote_DDG
        /// </summary>
        [EnumMember]
        [UIDisplayName("Emergency Promote")]
        EmergencyPromote = 26,

        /// <summary>
        /// Indicates PromoteQualificationFailure
        /// </summary>
        [EnumMember]
        [UIDisplayName("Promote Qualification Failure")]
        PromoteQualificationFailure = 27,
    }

    /// <summary>
    /// Indicates type of change for entity history exclude list
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityHistoryExcludeChangeType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates common attribute type
        /// </summary>
        [EnumMember]
        CommonAttribute = 1,

        /// <summary>
        /// Indicates technical attribute type
        /// </summary>
        [EnumMember]
        TechnicalAttribute = 2,

        /// <summary>
        /// Indicates relationship attribute type
        /// </summary>
        [EnumMember]
        RelationshipAttribute = 3,

        /// <summary>
        /// Indicates relationship type
        /// </summary>
        [EnumMember]
        RelationshipType = 4,

        /// <summary>
        /// Indicates extension relationship of container
        /// </summary>
        [EnumMember]
        ExtensionRelationship_Container = 5,

        /// <summary>
        /// Indicates hierarchy relationship of child entity type
        /// </summary>
        [EnumMember]
        HierarchyRelationship_ChildEntityType = 6,

        /// <summary>
        /// Indicates workflow
        /// </summary>
        [EnumMember]
        Workflow = 7
    }

    #endregion

    #region Dependency Attribute Type Enumerations

    /// <summary>
    /// Defines the Dependency Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DependencyType
    {
        /// <summary>
        /// Indicates Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates Parent 
        /// </summary>
        [EnumMember]
        Parent = 1,

        /// <summary>
        /// Indicates Child
        /// </summary>
        [EnumMember]
        Child = 2,

        /// <summary>
        /// Indicates Current/Myself
        /// </summary>
        [EnumMember]
        Current = 3

    }

    #endregion

    #region Other Enums

    /// <summary>
    /// Holds the various values for cache status of an entity based on its components. 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityCacheComponentEnum
    {
        /// <summary>
        /// Specifies that none of the components are dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Specifies that the base entity is dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        BaseEntity = 1,

        /// <summary>
        /// Specifies that the Inherited attribute is dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        InheritedAttributes = 2,

        /// <summary>
        /// Specifies that the Overridden attribute is dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        OverriddenAttributes = 4,

        /// <summary>
        /// Specifies that the Relationship is dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        Relationships = 8,

        /// <summary>
        /// Specifies that the Extensions are dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        Extensions = 16,

        /// <summary>
        /// Specifies that the hierarchies are dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        Hierarchies = 32,

        /// <summary>
        /// Specifies that the Workflow is dirty in cache for an entity.
        /// </summary>
        [EnumMember]
        Workflow = 64
    }

    /// <summary>
    /// Specifies the various object types for which EntityCacheLoadContext are created.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityCacheLoadContextTypeEnum
    {
        /// <summary>
        /// Specifies the Organization object.
        /// </summary>
        [EnumMember]
        Organization,

        /// <summary>
        /// Specifies the Container object.
        /// </summary>
        [EnumMember]
        Container,

        /// <summary>
        /// Specifies the Attribute object.
        /// </summary>
        [EnumMember]
        Attribute,

        /// <summary>
        /// Specifies the Category object.
        /// </summary>
        [EnumMember]
        Category,

        /// <summary>
        /// Specifies the Entity Type object.
        /// </summary>
        [EnumMember]
        EntityType,

        /// <summary>
        /// Specifies the relationship type object.
        /// </summary>
        [EnumMember]
        RelationshipType
    }

    /// <summary>
    /// Specifies the various modes for DataAccess calls
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum LegacyDataAccessCallsMode
    {
        /// <summary>
        /// Specifies the Direct Database call.
        /// </summary>
        [EnumMember]
        DirectDBCall,

        /// <summary>
        /// Specifies the Service call.
        /// </summary>
        [EnumMember]
        ServiceCall
    }

    /// <summary>
    /// Specifies the various modes of User credentials requests
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum UserCredentialRequestType
    {
        /// <summary>
        /// Specifies the request for LoginId
        /// </summary>
        [EnumMember]
        LoginId,

        /// <summary>
        /// Specifies the request for Password
        /// </summary>
        [EnumMember]
        Password
    }

    /// <summary>
    /// Specifies the various modes of User credentials requests
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum PanelItemType
    {
        /// <summary>
        /// Input type
        /// </summary>
        [EnumMember]
        [UIDisplayName("Input")]
        Input = 0,

        /// <summary>
        /// Lookup type
        /// </summary>
        [EnumMember]
        [UIDisplayName("Lookup")]
        Lookup = 1,

        /// <summary>
        /// Two dropdowsn. Container's dropdown depends on Organization's dropdown
        /// </summary>
        [EnumMember]
        [UIDisplayName("OrganizationContainerFilter")]
        OrganizationContainerFilter = 2,

        /// <summary>
        /// Smart Input type
        /// </summary>
        [EnumMember]
        [UIDisplayName("SmartInput")]
        SmartInput = 3
    }

    /// <summary>
    /// Specifies all existing system sources
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SystemSource
    {
        /// <summary>
        /// Unknown source
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// User source
        /// </summary>
        [EnumMember]
        [Description("User")]
        User = 1,

        /// <summary>
        /// DefaultValue source
        /// </summary>
        [EnumMember]
        [Description("System")]
        System = 2
    }

    /// <summary>
    /// Specifies all existing profile types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ProfileType
    {
        /// <summary>
        /// Unknown profile type
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// Export profile type
        /// </summary>
        [EnumMember]
        [Description("Export")]
        Export = 1,

        /// <summary>
        /// Validation profile type
        /// </summary>
        [EnumMember]
        [Description("Validation")]
        Validation = 2,

        /// <summary>
        /// Normalization profile type
        /// </summary>
        [EnumMember]
        [Description("Normalization")]
        Normalization = 3,

        /// <summary>
        /// Merging profile type
        /// </summary>
        [EnumMember]
        [Description("Merging")]
        Merging = 4,

        /// <summary>
        /// Matching profile type
        /// </summary>
        [EnumMember]
        [Description("Matching")]
        Matching = 5
    }

    #endregion

    #region Integration Enums

    /// <summary>
    /// Specifies type of integration
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum IntegrationType
    {
        /// <summary>
        /// Specifics integration type is not known.
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Specifies outbound integration
        /// </summary>
        [EnumMember]
        Outbound = 1,

        /// <summary>
        /// Specifies inbound integration
        /// </summary>
        [EnumMember]
        Inbound = 2
    }

    /// <summary>
    /// Specifies the result of qualification process for integration message
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MessageQualificationStatusEnum
    {
        /// <summary>
        /// Specifics qualification result is not known.
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Specifies that message is qualified
        /// </summary>
        [EnumMember]
        Qualified = 1,

        /// <summary>
        /// Specifies that message is not qualified
        /// </summary>
        [EnumMember]
        Disqualified = 2,

        /// <summary>
        /// Specifies message needs to be picked up for qualification after given time. The time can be specified in QualifyingQueueItem's ScheduledQualifierTime property.
        /// </summary>
        [EnumMember]
        WaitingToQualify = 3
    }


    #endregion Integration Enums

    #region DataModel Enums
    /// <summary>
    /// Defines DataModel Job queue activities types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelJobType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Normalization
        /// </summary>
        [EnumMember]
        [Description("Import")]
        Import = 1,

        /// <summary>
        /// Matching
        /// </summary>
        [EnumMember]
        [Description("Export")]
        Export = 2
    }

    /// <summary>
    /// List of actions available to the datamodel activity log
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataModelActivityList
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        #region attributeModel activities

        /// <summary>
        /// for updating attribute model via UI or import
        /// </summary>
        [EnumMember]
        AttributeModelUpdate = 1,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        AttributeModelCreate = 2,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        AttributeModelRename = 3,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        AttributeModelDelete = 4,

        #endregion attributeModel

        #region relationshiptype activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeCreate = 10,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeUpdate = 11,

        /// <summary>
        ///  
        /// </summary>
        [EnumMember]
        RelationshipTypeDelete = 12,

        #endregion relationshiptype

        #region EntityTypeAttributeMapping activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        EntityTypeAttributeMappingCreate = 20,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        EntityTypeAttributeMappingUpdate = 21,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        EntityTypeAttributeMappingDelete = 22,

        #endregion EntityTypeAttributeMapping activities

        #region ContainerEntityTypeAttributeMapping activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerEntityTypeAttributeMappingCreate = 30,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerEntityTypeAttributeMappingUpdate = 31,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerEntityTypeAttributeMappingDelete = 32,

        #endregion ContainerEntityTypeAttributeMapping activities

        #region ContainerRelationshipTypeEntityTypeMapping activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeEntityTypeMappingCreate = 40,


        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeEntityTypeMappingUpdate = 41,


        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerRelationshipTypeEntityTypeMappingDelete = 42,

        #endregion ContainerRelationshipTypeEntityTypeMapping activities

        #region RelationshipTypeAttributeMapping activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeAttributeMappingCreate = 50,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeAttributeMappingUpdate = 51,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeAttributeMappingDelete = 52,

        #endregion RelationshipTypeAttributeMapping activities

        #region RelationshipTypeEntityTypeMapping activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeEntityTypeMappinCreate = 60,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeEntityTypeMappingUpdate = 61,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        RelationshipTypeEntityTypeMappingDelete = 62,

        #endregion RelationshipTypeEntityTypeMapping activities

        #region lookup activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        LookupRowCreate = 70,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        LookupRowUpdate = 71,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        LookupRowDelete = 72,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        LookUpTableUpdate = 73,

        #endregion

        #region EntityType activities
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        EntityTypeUpdate = 80,

        #endregion EntityType activities

        #region Organization activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        OrganizationUpdate = 90,

        #endregion Organization activities

        #region Hierarchy activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        HierarchyUpdate = 100,

        #endregion Hierarchy activities

        #region Container activities

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ContainerUpdate = 110,

        #endregion Container activities

        #region CategoryAttributeMapping activities

        /// <summary>
        /// CategoryAttributeMapping Create
        /// </summary>
        [EnumMember]
        CategoryAttributeMappingCreate = 120,

        /// <summary>
        /// CategoryAttributeMapping Update
        /// </summary>
        [EnumMember]
        CategoryAttributeMappingUpdate = 121,

        /// <summary>
        /// CategoryAttributeMapping Delete
        /// </summary>
        [EnumMember]
        CategoryAttributeMappingDelete = 122

        #endregion CategoryAttributeMapping activities

    }

    #endregion

    #region DQM Enums

    /// <summary>
    /// Defines DQM Job queue activities types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DQMJobType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Normalization
        /// </summary>
        [EnumMember]
        [Description("Normalization")]
        Normalization = 1,

        /// <summary>
        /// Matching
        /// </summary>
        [EnumMember]
        [Description("Matching")]
        Matching = 2,

        /// <summary>
        /// Summarization
        /// </summary>
        [EnumMember]
        [Description("Summarization")]
        Summarization = 3,

        /// <summary>
        /// Validation
        /// </summary>
        [EnumMember]
        [Description("Validation")]
        Validation = 4,

        /// <summary>
        /// Merging
        /// </summary>
        [EnumMember]
        [Description("Merging")]
        Merging = 5,

        /// <summary>
        /// Store Load
        /// </summary>
        [EnumMember]
        [Description("Store Delta Load")]
        MatchStoreDeltaLoad = 6,

        /// <summary>
        /// MatchStoreInitialLoad
        /// </summary>
        //[Obsolete]
        [EnumMember]
        [Description("Store Initial Load")]
        MatchStoreInitialLoad = 7,

        /// <summary>
        /// Merge Planning
        /// </summary>
        [EnumMember]
        [Description("MergePlanning")]
        MergePlanning = 8,

        /// <summary>
        /// Merge Planning
        /// </summary>
        [EnumMember]
        [Description("Jigsaw Data Synchronization")]
        JigsawDataSynchronization = 9
    }

    /// <summary>
    /// Defines DQM Matching Service
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DQMMatchingProvider
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// MDM Center Search
        /// </summary>
        [EnumMember]
        [Description("MDMCenterMatching")]
        MDMCenterMatching = 1,

        /// <summary>
        /// Netrics Search
        /// </summary>
        [EnumMember]
        [Description("NetricsMatching")]
        NetricsMatching = 2
    }

    /// <summary>
    /// Defines Match Result Set Aggregate Function
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MatchResultSetAggregateFunction
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// MDM Center Search
        /// </summary>
        [EnumMember]
        [Description("Match Result Count")]
        MatchResultsCount = 1,

        /// <summary>
        /// Best Match Score
        /// </summary>
        [EnumMember]
        [Description("Best Match Score")]
        BestMatchScore = 2,

        /// <summary>
        /// Worst Match Score
        /// </summary>
        [EnumMember]
        [Description("Worst Match Score")]
        WorstMatchScore = 3
    }

    /// <summary>
    /// Defines Match Result Set Aggregate Function
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MergeAction
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Discard	
        /// </summary>
        [EnumMember]
        [Description("Discard")]
        Discard,

        /// <summary>
        /// Ask User
        /// </summary>
        [EnumMember]
        [Description("Needs Manual Review")]
        NeedsManualReview,

        /// <summary>
        /// Merge
        /// </summary>
        [EnumMember]
        [Description("Merge Into Existing")]
        Merge,

        /// <summary>
        /// Create
        /// </summary>
        [EnumMember]
        [Description("Treat As New")]
        TreatAsNew,

        /// <summary>
        /// CreateLink
        /// </summary>
        [EnumMember]
        [Description("Create Link")]
        CreateLink,

        /// <summary>
        /// SystemCreate
        /// </summary>
        [EnumMember]
        [Description("System Create")]
        SystemCreate,

        /// <summary>
        /// SystemMerge
        /// </summary>
        [EnumMember]
        [Description("System Merge")]
        SystemMerge,

        /// <summary>
        /// Custom
        /// </summary>
        [EnumMember]
        [Description("Custom")]
        Custom
    }

    /// <summary>
    /// Defines Collection Strategy for Survivorship Rule
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CollectionStrategy
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Flush And Fill
        /// </summary>
        [EnumMember]
        [Description("Flush And Fill")]
        FlushAndFill = 1,

        /// <summary>
        /// Union
        /// </summary>
        [EnumMember]
        [Description("Union")]
        Union = 2
    }

    /// <summary>
    /// Defines Strategy Priority for Survivorship Rule
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RulesetStrategy
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [Description("Unknown")]
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Recent
        /// </summary>
        [EnumMember]
        [Description("Recent")]
        Recent = 2,

        /// <summary>
        /// Source
        /// </summary>
        [EnumMember]
        [Description("Source")]
        Source = 5,

        /// <summary>
        /// External
        /// </summary>
        [EnumMember]
        [Description("External")]
        External = 6
    }

    /// <summary>
    /// Enum for different Application type.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ApplicationType
    {
        /// <summary>
        /// Application type UI
        /// </summary>
        [EnumMember]
        UI = 0,

        /// <summary>
        /// Application type DB
        /// </summary>
        [EnumMember]
        DB = 1,


        /// <summary>
        /// Application type API
        /// </summary>
        [EnumMember]
        API = 2
    }

    /// <summary>
    /// Enum for different DataQualityStatuses
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataQualityStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Valid
        /// </summary>
        [EnumMember]
        [Description("Valid")]
        Valid = 1,

        /// <summary>
        /// Invalid
        /// </summary>
        [EnumMember]
        [Description("Invalid")]
        Invalid = 2,


        /// <summary>
        /// Critical
        /// </summary>
        [Description("Critical")]
        [EnumMember]
        Critical = 3
    }

    /// <summary>
    /// Enum for different MergeResultStatuses
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MergeResultStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Successful
        /// </summary>
        [EnumMember]
        [Description("Successful")]
        Successful = 1,

        /// <summary>
        /// Failed
        /// </summary>
        [EnumMember]
        [Description("Failed")]
        Failed = 2,

        /// <summary>
        /// Not Applied
        /// </summary>
        [Description("Not Applied")]
        [EnumMember]
        NotApplied = 3
    }

    /// <summary>
    /// Enum for different DataQualityStatuses
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataStoreFieldType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Unknown Type")]
        Unknown = 0,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Non Searchable Text")]
        NonSearchableText = 4,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Searchable Text")]
        SearchableText = 5,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Int")]
        Int = 6,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Float")]
        Float = 8,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Date")]
        Date = 10,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("DateTime")]
        DateTime = 12,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Variable column will be stored as Name Value Pairs")]
        VariableColumn = 15
    }

    /// <summary>
    /// Enum for determining options to store collection values.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataStoreCollectionStorageType
    {
        /// <summary>
        /// Unknown or Not applicable.
        /// </summary>
        [EnumMember]
        [Description("Not Applicable")]
        NotApplicable = 0,
        /// <summary>
        /// Store as concatenated values.
        /// </summary>
        [EnumMember]
        [Description("Store as concatenated values")]
        ConcatenatedValues = 1,

        /// <summary>
        /// Store as variable column
        /// </summary>
        [EnumMember]
        [Description("Variable Column")]
        AttributeIdValuePair = 2
    }

    /// <summary>
    /// Enum for determining initial load status.
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DataStoreLoadStatus
    {
        /// <summary>
        /// Not loaded yet.
        /// </summary>
        [EnumMember]
        [Description("Not Loaded")]
        NotLoaded = 0,
        /// <summary>
        /// In Progress.
        /// </summary>
        [EnumMember]
        [Description("In Progress")]
        InProgress = 1,

        /// <summary>
        /// Load Failed
        /// </summary>
        [EnumMember]
        [Description("Failed")]
        Failed = 2,

        /// <summary>
        /// Load Completed
        /// </summary>
        [EnumMember]
        [Description("Load Completed")]
        Completed = 3,

        /// <summary>
        /// Scheduled
        /// </summary>
        [EnumMember]
        [Description("Scheduled")]
        Scheduled = 4
    }

    /// <summary>
    /// Enum for Merge modes
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MergeMode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Merge Existing
        /// </summary>
        [EnumMember]
        [Description("Merge Existing")]
        MergeExisting = 1,

        /// <summary>
        /// Create New Only
        /// </summary>
        [EnumMember]
        [Description("Create New Only")]
        CreateNewOnly = 2,

        /// <summary>
        /// Merge And Create
        /// </summary>
        [EnumMember]
        [Description("Merge And Create")]
        MergeAndCreate = 3,

        /// <summary>
        /// Simulate
        /// </summary>
        [EnumMember]
        [Description("Simulate")]
        Simulate = 4
    }

    /// <summary>
    /// Enum to define Save Attributes options for Matching
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum SaveAttributeValuesType
    {

        /// <summary>
        /// All of the provided attributes will be saved
        /// </summary>
        [EnumMember]
        [Description("All Attributes")]
        AllAttributes = 0,
        /// <summary>
        /// None of the attributes will be saved
        /// </summary>
        [EnumMember]
        [Description("None")]
        None = 1
    }

    /// <summary>
    /// This enum defines matching engine type
    /// currently Tibco engine is supported
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MatchingEngineType
    {
        /// <summary>
        /// Tibco engine
        /// </summary>
        [EnumMember]
        [Description("Tibco Patterns Engine")]
        DefaultEngineType = 100,

        /// <summary>
        /// Trillium engine
        /// </summary>
        [EnumMember]
        [Description("Trillium Engine")]
        TrilliumEngineType,

        /// <summary>
        /// Riversand matching engine
        /// </summary>
        [EnumMember]
        [Description("Riversand Matching Engine")]
        RiversandMatchingEngine
    }

    /// <summary>
    /// Enum that defines possible statuses of MergePlan user review
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MergePlanUserReviewStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Pending
        /// </summary>
        [EnumMember]
        [Description("Pending")]
        Pending = 1,

        /// <summary>
        /// Complete
        /// </summary>
        [EnumMember]
        [Description("Complete")]
        Complete = 2,

        /// <summary>
        /// NotRequired
        /// </summary>
        [EnumMember]
        [Description("NotRequired")]
        NotRequired = 3
    }

    /// <summary>
    /// Enum that defines status of merging for current source entity
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MergeStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Pending
        /// </summary>
        [EnumMember]
        [Description("Pending")]
        Pending = 1,

        /// <summary>
        /// Complete
        /// </summary>
        [EnumMember]
        [Description("Complete")]
        Complete = 2,

        /// <summary>
        /// Failed
        /// </summary>
        [EnumMember]
        [Description("Failed")]
        Failed = 3,

        /// <summary>
        /// Failed
        /// </summary>
        [EnumMember]
        [Description("N/A")]
        NotApplied = 4
    }

    /// <summary>
    /// Enum that defines all exposable flags for merge context
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExposableMergeContextFlag
    {
        /// <summary>
        /// CopyRelationshipAttributes
        /// </summary>
        [EnumMember]
        [Description("Copy Relationship Attributes")]
        CopyRelationshipAttributes = 1,

        /// <summary>
        /// CopyRelationships
        /// </summary>
        [EnumMember]
        [Description("Copy Relationships")]
        CopyRelationships = 2,

        /// <summary>
        /// CopySystemAttributes
        /// </summary>
        [EnumMember]
        [Description("Copy System Attributes")]
        CopySystemAttributes = 3,

        /// <summary>
        /// CopyTechnicalAttributes
        /// </summary>
        [EnumMember]
        [Description("Copy Technical Attributes")]
        CopyTechnicalAttributes = 4,

        /// <summary>
        /// CopyCommonAttributes
        /// </summary>
        [EnumMember]
        [Description("Copy Common Attributes")]
        CopyCommonAttributes = 5,

        /// <summary>
        /// CopyComplexAttributes
        /// </summary>
        [EnumMember]
        [Description("Copy Complex Attributes")]
        CopyComplexAttributes = 6,

        /// <summary>
        /// ProcessOnlyFirstLevelFlag
        /// </summary>
        [EnumMember]
        [Description("Process Only First Level Entities")]
        ProcessOnlyFirstLevelFlag = 7,

        /// <summary>
        /// ProcessRelationshipsFlag
        /// </summary>
        [EnumMember]
        [Description("Process Relationships Flag")]
        ProcessRelationshipsFlag = 8,

        /// <summary>
        /// ProcessRelationshipsOnlyAtFirstLevelFlag
        /// </summary>
        [EnumMember]
        [Description("Process Only First Level Relationships")]
        ProcessRelationshipsOnlyAtFirstLevelFlag = 9
    }

    /// <summary>
    /// Enum that defines the operands for a Match Predicate
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MatchPredicateOperand
    {
        /// <summary>
        /// Equals
        /// </summary>
        [EnumMember]
        [Description("Equals")]
        EQUALS = 0,

        /// <summary>
        /// Equals (Insensitive)
        /// </summary>
        [EnumMember]
        [Description("Equals (Insensitive)")]
        INSEN_EQUALS,

        /// <summary>
        /// Less Than
        /// </summary>
        [EnumMember]
        [Description("Less Than")]
        LESSTHAN,

        /// <summary>
        /// Less Than (Insensitive)
        /// </summary>
        [EnumMember]
        [Description("Less Than (Insensitive)")]
        INSEN_LESSTHAN,

        /// <summary>
        /// Less Than Or Equals
        /// </summary>
        [EnumMember]
        [Description("Less Than Or Equals")]
        LESSTHANOREQ,

        /// <summary>
        /// Less Than Or Equals (Insensitive)
        /// </summary>
        [EnumMember]
        [Description("Less Than Or Equals (Insensitive)")]
        INSEN_LESSTHANOREQ,

        /// <summary>
        /// Greater Than
        /// </summary>
        [EnumMember]
        [Description("Greater Than")]
        GREATERTHAN,

        /// <summary>
        /// Greater Than (Insensitive)
        /// </summary>
        [EnumMember]
        [Description("Greater Than (Insensitive)")]
        INSEN_GREATERTHAN,

        /// <summary>
        /// Greater Than Or Equals
        /// </summary>
        [EnumMember]
        [Description("Greater Than Or Equals")]
        GREATERTHANOREQ,

        /// <summary>
        /// Greater Than Or Equals (Insensitive)
        /// </summary>
        [EnumMember]
        [Description("Greater Than Or Equals (Insensitive)")]
        INSEN_GREATERTHANOREQ,
    }
    #endregion

    #region Category Field Enum

    /// <summary>
    /// Category Field Enum
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum CategoryField
    {
        /// <summary>
        /// Category Id
        /// </summary>
        [EnumMember]
        [Description("Category Id")]
        Id = 0,

        /// <summary>
        /// Category ShortName
        /// </summary>
        [EnumMember]
        [Description("Category ShortName")]
        Name = 1,

        /// <summary>
        /// Category LongName
        /// </summary>
        [EnumMember]
        [Description("Category LongName")]
        LongName = 2,

        /// <summary>
        /// Category ShortName Path
        /// </summary>
        [EnumMember]
        [Description("Category ShortName Path")]
        Path = 3,

        /// <summary>
        /// Category LongName Path
        /// </summary>
        [EnumMember]
        [Description("Category LongName Path")]
        LongNamePath = 4,

        /// <summary>
        /// Parent Category Id
        /// </summary>
        [EnumMember]
        [Description("Parent Category Id")]
        ParentCategoryId = 5
    }

    #endregion

    #region TMS Connector Enums

    /// <summary>
    /// Translation Export Target Value Types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum TranslationExportTargetValueType
    {
        /// <summary>
        /// Specifies attribute's source locale value
        /// </summary>
        [EnumMember]
        SourceValue = 0,

        /// <summary>
        /// Specifies attribute's target locale value
        /// </summary>
        [EnumMember]
        TargetValue = 1,

        /// <summary>
        /// Specifies empty value
        /// </summary>
        [EnumMember]
        EmptyValue = 2,

        /// <summary>
        /// Unknown type
        /// </summary>
        [EnumMember]
        Unknown = 3
    }

    /// <summary>
    /// Translation state of a content
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum TranslationState
    {
        /// <summary>
        /// Specifies that content is translated
        /// </summary>
        [EnumMember]
        [UIDisplayName("translated")]
        Translated = 0,

        /// <summary>
        /// Specifies that content needs translation
        /// </summary>
        [EnumMember]
        [UIDisplayName("needs-translation")]
        NeedsTranslation = 1
    }

    #endregion

    #region RoleType

    /// <summary>
    /// Indicates type of role
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RoleType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates system administrator role
        /// </summary>
        [EnumMember]
        SystemAdmin = 1,

        /// <summary>
        /// Indicates bulk administrator role
        /// </summary>
        [EnumMember]
        BulkAdmin = 2,

        /// <summary>
        /// Indicates bulk administrator with roles
        /// </summary>
        [EnumMember]
        BulkAdminWithRoles = 3,
    }
    #endregion

    #region WordList Enums

    /// <summary>
    /// Indicates word element columns
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum WordElementColumn
    {
        /// <summary>
        /// Indicates unknown column
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Indicates Word column
        /// </summary>
        [EnumMember]
        [Description("Word")]
        Word = 1,

        /// <summary>
        /// Indicates Substitute column
        /// </summary>
        [EnumMember]
        [Description("Substitute")]
        Substitute = 2,

        /// <summary>
        /// Indicates Sequence column
        /// </summary>
        [EnumMember]
        [Description("Sequence")]
        Sequence = 3
    }

    #endregion

    #region Match Score Combiner Enum

    /// <summary>
    /// This enum hold the different kind of Matching Condition Combiner Type.
    /// </summary>
    [DataContract]
    public enum MatchScoreCombinerType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        AND,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        OR,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        NOT,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        NOTSET
    }

    #endregion

    #region Diagnostics Enums

    /// <summary>
    /// System Diagnostic Type
    /// </summary>
    [DataContract]
    public enum SystemDiagnosticType
    {
        /// <summary>
        /// CPU
        /// </summary>
        [EnumMember]
        CPU,

        /// <summary>
        /// Memory
        /// </summary>
        [EnumMember]
        Memory,

        /// <summary>
        /// Input/Output
        /// </summary>
        [EnumMember]
        IO,

        /// <summary>
        /// Wait 
        /// </summary>
        [EnumMember]
        Wait,

        /// <summary>
        /// Locks
        /// </summary>
        [EnumMember]
        Locks,

        /// <summary>
        /// Cache
        /// </summary>
        [EnumMember]
        Cache,

        /// <summary>
        /// Index
        /// </summary>
        [EnumMember]
        Index,

        /// <summary>
        /// Statistics
        /// </summary>
        [EnumMember]
        Statistics,

        /// <summary>
        /// Full Text
        /// </summary>
        [EnumMember]
        Fulltext,

        /// <summary>
        /// Table
        /// </summary>
        [EnumMember]
        Table,

        /// <summary>
        /// Store Procedure
        /// </summary>
        [EnumMember]
        StoreProcedure,

        /// <summary>
        /// Job Data
        /// </summary>
        [EnumMember]
        JobData
    }

    /// <summary>
    /// Diagnostic Tools Report Types
    /// </summary>
    [DataContract]
    public enum DiagnosticToolsReportType
    {

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Unknown")]
        Unknown,

        /// <summary>
        /// security check report
        /// </summary>
        [EnumMember]
        [Description("Security Integrity Check")]
        SecurityIntegrityCheck,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Entity Integrity Check")]
        EntityIntegrityCheck,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Entity Attribute Integrity Check")]
        EntityAttributeIntegrityCheck,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Entity Relationship Integrity Check")]
        EntityRelationshipIntegrityCheck,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Category Attribute Integrity Check")]
        CategoryAttributeIntegrityCheck,

        /// <summary>
        /// compare entity report
        /// </summary>
        [EnumMember]
        [Description("Entity Compare")]
        EntityCompare,

        /// <summary>
        /// denorm integrity check report
        /// </summary>
        [EnumMember]
        [Description("Denorm Integrity Check")]
        DenormIntegrityCheck,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Core Denorm Compare")]
        CoreDenormCompare,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Application DashBoard")]
        ApplicationDashBoard,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("Data Model Integrity Check")]
        DataModelIntegrityCheck
    }

    /// <summary>
    /// diagnostic tools report subtype
    /// </summary>
    public enum DiagnosticToolsReportSubType
    {
        /// <summary>
        /// subtype1
        /// </summary>
        [EnumMember]
        Unknown,
    }


    /// <summary>
    /// diagnostic tools report subtype
    /// </summary>
    public enum DiagnosticReportProfileType
    {
        /// <summary>
        /// subtype1
        /// </summary>
        [EnumMember]
        Default,

        /// <summary>
        /// subtype1
        /// </summary>
        [EnumMember]
        Unknown,
    }

    /// <summary>
    /// System Diagnostic Sub Type
    /// </summary>
    [DataContract]
    public enum SystemDiagnosticSubType
    {
        /// <summary>
        /// SQL Processor Utilization
        /// </summary>
        [Description("SQL Processor Utilization")]
        [EnumMember]
        SQLProcessorUtilization,

        /// <summary>
        /// CPU Intensive Queries
        /// </summary>
        [Description("CPU Intensive Queries")]
        [EnumMember]
        CPUIntensiveQueries,

        /// <summary>
        /// Average CPU Consuming Queries
        /// </summary>
        [Description("Average CPU Consuming Queries")]
        [EnumMember]
        AverageCPUConsumingQueries,

        /// <summary>
        /// Memory Information
        /// </summary>
        [Description("Memory Information")]
        [EnumMember]
        MemoryInformation,

        /// <summary>
        /// Memory Grants Pending
        /// </summary>
        [Description("Memory Grants Pending")]
        [EnumMember]
        MemoryGrantsPending,

        /// <summary>
        /// Memory Pressure
        /// </summary>
        [Description("Memory Pressure")]
        [EnumMember]
        MemoryPressure,

        /// <summary>
        /// AVGIO
        /// </summary>
        [Description("AVGIO")]
        [EnumMember]
        AVGIO,

        /// <summary>
        /// Health
        /// </summary>
        [Description("Health")]
        [EnumMember]
        Health,

        /// <summary>
        /// IOPercent
        /// </summary>
        [Description("IOPercent")]
        [EnumMember]
        IOPercent,

        /// <summary>
        /// IOQueries
        /// </summary>
        [Description("IOQueries")]
        [EnumMember]
        IOQueries,

        /// <summary>
        /// Session
        /// </summary>
        [Description("Session")]
        [EnumMember]
        Session,

        /// <summary>
        /// Blocks
        /// </summary>
        [Description("Blocks")]
        [EnumMember]
        Blocks,

        /// <summary>
        /// HighLevel
        /// </summary>
        [Description("HighLevel")]
        [EnumMember]
        HighLevel,

        /// <summary>
        /// Compiled Plan
        /// </summary>
        [Description("Compiled Plan")]
        [EnumMember]
        CompiledPlan,

        /// <summary>
        /// Corrupted Plan
        /// </summary>
        [Description("Corrupted Plan")]
        [EnumMember]
        CorruptedPlan,

        /// <summary>
        /// IndexUpdate
        /// </summary>
        [Description("IndexUpdate")]
        [EnumMember]
        IndexUpdate,

        /// <summary>
        /// MissingIndex
        /// </summary>
        [Description("MissingIndex")]
        [EnumMember]
        MissingIndex,

        /// <summary>
        /// UnusedIndex
        /// </summary>
        [Description("UnusedIndex")]
        [EnumMember]
        UnusedIndex,

        /// <summary>
        /// UnusedIndex-CommonlyUsedTables
        /// </summary>
        [Description("UnusedIndex-CommonlyUsedTables")]
        [EnumMember]
        UnusedIndexCommonlyUsedTables,

        /// <summary>
        /// PopulationDetails
        /// </summary>
        [Description("PopulationDetails")]
        [EnumMember]
        PopulationDetails,

        /// <summary>
        /// Status
        /// </summary>
        [Description("Status")]
        [EnumMember]
        Status,

        /// <summary>
        /// All Table Usage
        /// </summary>
        [Description("AllTableUsage")]
        [EnumMember]
        AllTableUsage,

        /// <summary>
        /// Frequent Table Usage
        /// </summary>
        [Description("FrequentTableUsage")]
        [EnumMember]
        FrequentTableUsage,

        /// <summary>
        /// Frequent Table Framentation
        /// </summary>
        [Description("FrequentTableFramentation")]
        [EnumMember]
        FrequentTableFramentation,

        /// <summary>
        /// Table Space
        /// </summary>
        [Description("TableSpace")]
        [EnumMember]
        TableSpace,

        /// <summary>
        /// Long Running
        /// </summary>
        [Description("LongRunning")]
        [EnumMember]
        LongRunning,

        /// <summary>
        /// IO
        /// </summary>
        [Description("IO")]
        [EnumMember]
        IO,

        /// <summary>
        /// Most Used
        /// </summary>
        [Description("MostUsed")]
        [EnumMember]
        MostUsed,

        /// <summary>
        /// Schedule
        /// </summary>
        [Description("Schedule")]
        [EnumMember]
        Schedule
    }

    /// <summary>
    /// Application Diagnostic Type
    /// </summary>
    [DataContract]
    public enum ApplicationDiagnosticType
    {
        /// <summary>
        /// Denorm Speed
        /// </summary>
        [Description("Denorm Speed")]
        [EnumMember]
        DenormSpeed,

        /// <summary>
        /// Pending Denorm
        /// </summary>
        [Description("Pending Denorm")]
        [EnumMember]
        PendingDenorm,

        /// <summary>
        /// Parallel Processor Status
        /// </summary>
        [Description("Parallel Processor Status")]
        [EnumMember]
        ParallelProcessorStatus,

        /// <summary>
        /// Cache Status
        /// </summary>
        [Description("Cache Status")]
        [EnumMember]
        CacheStatus,

        /// <summary>
        /// Entity Status
        /// </summary>
        [Description("Entity Status")]
        [EnumMember]
        EntityStatus,

        /// <summary>
        /// Application Check
        /// </summary>
        [Description("Application Check")]
        [EnumMember]
        ApplicationCheck
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    public enum TracingMode
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        OperationTracing = 1,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        SelectiveComponentTracing = 2,
    }


    /// <summary>
    /// Diagnostic storage mode
    /// </summary>
    [DataContract]
    [Serializable]
    public enum DiagnosticTracingStorageMode
    {
        /// <summary>
        /// Store only in the database
        /// </summary>
        [EnumMember]
        DatabaseOnly = 0,

        /// <summary>
        /// Store only in file
        /// </summary>
        [EnumMember]
        FileOnly = 1,

        /// <summary>
        /// Store in both database and file
        /// </summary>
        [EnumMember]
        DatabaseAndFile = 2,

        /// <summary>
        /// Store data into legacy svc file in the sync mode
        /// </summary>
        [EnumMember]
        Legacy = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    public enum TracingLevel
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Basic = 1,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Detail = 2,
    }

    /// <summary>
    /// Defines the diagnostic relative data type.
    /// </summary>
    [DataContract]
    [Serializable]
    public enum DiagnosticRelativeDataType
    {
        /// <summary>
        /// Represents the message data as for related diagnostic record.
        /// </summary>
        [EnumMember]
        MessageData = 1,

        /// <summary>
        /// Represents the context data as for related diagnostic record.
        /// </summary>
        [EnumMember]
        ContextData = 2
    }

    #endregion

    #region UI related Enums

    /// <summary>
    /// Defines alignment values for UI rendering
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum TextAlignment
    {
        /// <summary>
        /// Left align
        /// </summary>
        [EnumMember]
        [Description("left")]
        left = 0,

        /// <summary>
        /// Right align
        /// </summary>
        [EnumMember]
        [Description("right")]
        right = 1,

        /// <summary>
        /// Center align
        /// </summary>
        [EnumMember]
        [Description("center")]
        center = 2
    }

    /// <summary>
    /// Defines format to be used for Grid
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum GridFormat
    {
        /// <summary>
        /// Grid in Standard format
        /// </summary>
        [EnumMember]
        StandardGrid = 0,

        /// <summary>
        /// Grid in Tree format
        /// </summary>
        [EnumMember]
        TreeGrid = 1
    }

    /// <summary>
    /// Defines caller page and their values
    /// </summary>
    public enum CallerPage
    {
        /// <summary>
        /// Unknown 
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Entity Detail
        /// </summary>
        [EnumMember]
        EntityDetail = 1,

        /// <summary>
        /// EntityExplorer
        /// </summary>
        [EnumMember]
        EntityExplorer = 2
    }

    #endregion UI related Enums

    #region DateInterval
    /// <summary>
    /// Defines format to be used for Grid
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DateInterval
    {
        /// <summary>
        /// Months ( 1 through 12 )
        /// </summary>
        [EnumMember]
        Months = 0,

        /// <summary>
        /// Grid in Standard format
        /// </summary>
        [EnumMember]
        Days = 1,

        /// <summary>
        /// Hour (1 through 24)
        /// </summary>
        [EnumMember]
        Hours = 2,

        /// <summary>
        /// Minute (0 through 59)
        /// </summary>
        [EnumMember]
        Minutes = 3,

        /// <summary>
        /// Second (0 through 59)
        /// </summary>
        [EnumMember]
        Seconds = 4

    }

    #endregion

    #region Expression Parser Enums

    /// <summary>
    /// Defines Operator Binding Options
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum OperatorBinding
    {
        /// <summary>
        /// Left Binding
        /// </summary>
        [EnumMember]
        Left,

        /// <summary>
        /// Right Binding
        /// </summary>
        [EnumMember]
        Right
    }

    /// <summary>
    /// Operator Expression Type Enums
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum OperatorExpressionType
    {
        /// <summary>
        /// Unary Operator
        /// </summary>
        [EnumMember]
        Unary,
        /// <summary>
        /// Binary Operator
        /// </summary>
        [EnumMember]
        Binary,
        /// <summary>
        /// Tertiary Operator
        /// </summary>
        [EnumMember]
        Tertiary,
        /// <summary>
        /// No Operator
        /// </summary>
        [EnumMember]
        None,
        /// <summary>
        /// InstanceMethodCall Operator
        /// </summary>
        [EnumMember]
        InstanceMethodCall
    }

    /// <summary>
    /// Parameter Type Enums
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ParameterType
    {
        /// <summary>
        /// Variable Parameter Type
        /// </summary>
        [EnumMember]
        Variable,
        /// <summary>
        /// InputArgument Parameter Type
        /// </summary>
        [EnumMember]
        InputArgument
    }

    /// <summary>
    /// Expression Token Type Enums
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExpressionTokenType
    {
        /// <summary>
        /// Operator Expression Token Type
        /// </summary>
        [EnumMember]
        Operator,
        /// <summary>
        /// Breaker Expression Token Type
        /// </summary>
        [EnumMember]
        Breaker,
        /// <summary>
        /// Custom Expression Token Type
        /// </summary>
        [EnumMember]
        Custom,
        /// <summary>
        /// Parameter Expression Token Type
        /// </summary>
        [EnumMember]
        Parameter,

        /// <summary>
        /// Value Token Type
        /// </summary>
        [EnumMember]
        Value
    }

    #endregion Expression Parser Enums

    #region EntityIdentifier Enums
    /// <summary>
    /// Indicates type of ID Field
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityIdentificationFieldType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates attribute
        /// </summary>
        [EnumMember]
        Attribute = 1,

        /// <summary>
        /// Indicates internal metadata property
        /// </summary>
        [EnumMember]
        Metadata = 2

    }

    /// <summary>
    /// Indicates behavior of Identifier 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityIdentificationBehavior
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates move to next rule
        /// </summary>
        [EnumMember]
        CheckNextRule = 1,

        /// <summary>
        /// Indicates stop processing and set action as Create
        /// </summary>
        [EnumMember]
        Create = 2,

        /// <summary>
        /// Indicates throw error
        /// </summary>
        [EnumMember]
        RaiseError = 3

    }

    /// <summary>
    /// Indicates type of identification service
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum EntityIdentificationServiceType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates attribute/metadata field id service
        /// </summary>
        [EnumMember]
        FieldBased = 1,

        /// <summary>
        /// Indicates BR
        /// </summary>
        [EnumMember]
        RuleEngine = 2,

        /// <summary>
        /// Indicates matching
        /// </summary>
        [EnumMember]
        MatchingEngine = 3

    }

    #endregion

    #region Entity Proessing Options

    /// <summary>
    /// Defines the behavior of the attribute compare and merge. This behavior is applicable for common, technical and relationship attributes
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum AttributeCompareAndMergeBehavior
    {
        /// <summary>
        /// Only the Overridden values will be used for compare and merge logic.
        /// </summary>
        [EnumMember]
        CompareOverriddenValuesOnly = 0,

        /// <summary>
        /// Comparison will also include overridden values and if no overridden values are present will compare against inherited values if available.
        /// </summary>
        [EnumMember]
        CompareOverriddenAndInheritedValues = 1
    }

    #endregion

    #region DDG Enums

    #region Business Rule Types

    /// <summary>
    /// Defines the possible Business Rule types
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMRuleType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        UnKnown = 0,

        /// <summary>
        /// Represents governance business rule
        /// </summary>
        [EnumMember]
        Governance = 1,

        /// <summary>
        /// Represents content generation business rule
        /// </summary>
        [EnumMember]
        ContentGeneration = 2,

        /// <summary>
        /// Represents business condition rule
        /// </summary>
        [EnumMember]
        BusinessCondition = 3
    }

    #endregion Business Rule Types

    #region Revalidate Mode Types

    /// <summary>
    /// Indicates the revalidate mode to trigger dynamic data governance rules
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RevalidateMode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates full revalidation of rules
        /// </summary>
        [EnumMember]
        Full = 1,

        /// <summary>
        /// Indicates delta revalidation of rules.
        /// </summary>
        [EnumMember]
        Delta = 2
    }

    #endregion


    #region Event Type

    /// <summary>
    /// Defines the MDMEvent Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum MDMEventType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// All
        /// </summary>
        [EnumMember]
        All = 1,

        /// <summary>
        /// UI
        /// </summary>
        [EnumMember]
        UI = 2,

        /// <summary>
        /// Import
        /// </summary>
        [EnumMember]
        Import = 3,

        /// <summary>
        /// Manual run
        /// </summary>
        [EnumMember]
        ManualRun = 4,
    }

    #endregion Event Type

    #region Display Type

    /// <summary>
    /// Defines the Display Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DisplayType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// AttributeList
        /// </summary>
        [EnumMember]
        AttributeList = 1,

        /// <summary>
        /// RelationshipType
        /// </summary>
        [EnumMember]
        RelationshipType = 2,

        /// <summary>
        /// VariantConfigurator
        /// </summary>
        [EnumMember]
        VariantConfigurator = 3
    }

    #endregion Display Type

    #region Rule Status

    /// <summary>
    /// Defines the Rule Status
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum RuleStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Publish
        /// </summary>
        [EnumMember]
        Published = 1,

        /// <summary>
        /// Draft
        /// </summary>
        [EnumMember]
        Draft = 2
    }

    #endregion Rule Status

    /// <summary>
    /// Defines the DDG Caller Module
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DDGCallerModule
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Entity get
        /// </summary>
        [EnumMember]
        EntityGet = 1,

        /// <summary>
        /// Entity Process
        /// </summary>
        [EnumMember]
        EntityProcess = 2,

        /// <summary>
        /// Workflow
        /// </summary>
        [EnumMember]
        Workflow = 3,

        /// <summary>
        /// Promote process
        /// </summary>
        [EnumMember]
        PromoteProcess = 4,

        /// <summary>
        /// Entity family process
        /// </summary>
        [EnumMember]
        EntityFamilyProcess = 5,

        /// <summary>
        /// Represents re-validate
        /// </summary>
        [EnumMember]
        Revalidate = 6,

        /// <summary>
        /// Full entity re-validate
        /// </summary>
        [EnumMember]
        FullEntityRevalidate = 7,

        /// <summary>
        /// Full entity re-validate
        /// </summary>
        [EnumMember]
        DeltaEntityRevalidate = 8,

        /// <summary>
        /// Full entity re-validate
        /// </summary>
        [EnumMember]
        TrialRun = 9
    }

    /// <summary>
    /// Defines the DDG keyword category
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DDGKeywordCategory
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Attribute Keywords
        /// </summary>
        [EnumMember]
        AttributeKeywords = 1,

        /// <summary>
        /// Attribute collection Keywords
        /// </summary>
        [EnumMember]
        AttributeCollectionKeywords = 2,

        /// <summary>
        /// Attribute Group Keywords
        /// </summary>
        [EnumMember]
        AttributeGroupKeywords = 3,

        /// <summary>
        /// Extension Keywords
        /// </summary>
        [EnumMember]
        ExtensionKeywords = 4,

        /// <summary>
        /// Relationship Keywords
        /// </summary>
        [EnumMember]
        RelationshipKeywords = 5,

        /// <summary>
        /// Hierarchy Keywords
        /// </summary>
        [EnumMember]
        HierarchyKeywords = 6,

        /// <summary>
        /// Rollup Keywords
        /// </summary>
        [EnumMember]
        RollupKeywords = 7,

        /// <summary>
        /// Rollup Helper Keywords
        /// </summary>
        [EnumMember]
        RollupHelperKeywords = 8,

        /// <summary>
        /// Workflow Keywords
        /// </summary>
        [EnumMember]
        WorkflowKeywords = 9,

        /// <summary>
        /// Other keywords
        /// </summary>
        [EnumMember]
        OtherKeywords = 10,

        /// <summary>
        /// Native Keywords
        /// </summary>
        [EnumMember]
        NativeKeywords = 11,

        /// <summary>
        /// Export keywords
        /// </summary>
        [EnumMember]
        ExportKeywords = 12,

        /// <summary>
        /// AttributeModel Keywords
        /// </summary>
        [EnumMember]
        AttributeModelKeywords = 13,

        /// <summary>
        /// Default Qualified AttributeModel Keywords
        /// </summary>
        [EnumMember]
        DefaultQualifiedAttributeModelKeywords = 14,

        /// <summary>
        /// Promote keywords
        /// </summary>
        [EnumMember]
        PromoteKeywords = 15,

        /// <summary>
        /// ExternalBR keywords
        /// </summary>
        [EnumMember]
        ExternalBRKeywords = 16,

        /// <summary>
        /// Required attribute keywords
        /// </summary>
        [EnumMember]
        RequiredAttributeKeywords = 17,

        /// <summary>
        /// Entity model keywords
        /// </summary>
        [EnumMember]
        EntityModelKeywords = 18,

        /// <summary>
        /// Extension Core Keywords
        /// </summary>
        [EnumMember]
        ExtensionCoreKeywords = 19,

        /// <summary>
        /// Required attribute group keywords
        /// </summary>
        [EnumMember]
        RequiredAttributeGroupKeywords = 20,

        /// <summary>
        /// Default Entity Model Keywords
        /// </summary>
        [EnumMember]
        DefaultEntityModelKeywords = 21
    }

    #endregion DDG Enums

    #region Container Enums

    /// <summary>
    /// Specifies type of container
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ContainerType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Master collaboration container
        /// </summary>
        [EnumMember]
        [LocalizationCode("114159")]
        MasterCollaboration = 1,
        /// <summary>
        /// Extension collboration container
        /// </summary>
        [EnumMember]
        [LocalizationCode("114160")]
        ExtensionCollaboration = 2,
        /// <summary>
        /// Upstream container
        /// </summary>
        [EnumMember]
        [LocalizationCode("114155")]
        Upstream = 3,
        /// <summary>
        /// Master approved container
        /// </summary>
        [EnumMember]
        MasterApproved = 4,
        /// <summary>
        /// Extension approved container
        /// </summary>
        [EnumMember]
        ExtensionApproved = 5
    }

    #endregion Container Enums

    #region Jigsaw Enums

    /// <summary>
    /// Specifies type of Integration App type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JigsawIntegrationAppName
    {
        /// <summary>
        /// Manager Govern App Broker
        /// </summary>
        [EnumMember]
        manageGovernApp = 1,

        /// <summary>
        /// Match Application
        /// </summary>
        [EnumMember]
        matchApp = 2,

        /// <summary>
        /// App Config Manage Application
        /// </summary>
        [EnumMember]
        appConfigManageApp = 3
    }

    /// <summary>
    /// Defines the type of jigsaw caller process
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JigsawCallerProcessType
    {
        /// <summary>
        /// Data Quality Message
        /// </summary>
        [EnumMember]
        DataQualityMessage = 1,

        /// <summary>
        /// App Config Manage Message
        /// </summary>
        [EnumMember]
        AppConfigManageMessage = 2,

        /// <summary>
        /// The export
        /// </summary>
        [EnumMember]
        ExportEvent = 3,

        /// <summary>
        /// The promote
        /// </summary>
        [EnumMember]
        PromoteEvent = 4,

        /// <summary>
        /// The promote
        /// </summary>
        [EnumMember]
        WorkflowEvent = 5
    }

    /// <summary>
    /// Defines the type of jigsaw integration broker
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum JigsawIntegrationBrokerType
    {
        /// <summary>
        /// Broker based on Kafka.Net 
        /// </summary>
        [EnumMember]
        CSharpClientKafkaNetBroker = 1,

        /// <summary>
        /// File Folder based broker
        /// </summary>
        [EnumMember]
        FileFolderBroker = 2,

        /// <summary>
        /// KafkaDashNetBroker 
        /// </summary>
        [EnumMember]
        KafkaDashNetBroker = 3,

        /// <summary>
        /// Null Message Broker which will not send any message. Messages are available for testing.
        /// </summary>
        [EnumMember]
        NullMessageBroker = 4
    }

    #endregion

    #region ReasonType Enums

    /// <summary>
    /// Specifies type of reason
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ReasonType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember]
        Unknown = 1,

        /// <summary>
        /// Duplicate Entity  
        /// </summary>
        [EnumMember]
        DuplicateEntity = 2,

        /// <summary>
        /// Deleted Category
        /// </summary>
        [EnumMember]
        DeletedCategory = 3,

        /// <summary>
        /// Incorrect Extension  
        /// </summary>
        [EnumMember]
        IncorrectExtension = 4,

        /// <summary>
        /// Incorrect Domain  
        /// </summary>
        [EnumMember]
        IncorrectDomain = 5,

        /// <summary>
        /// Variant Parent Missing or Incorrect  
        /// </summary>
        [EnumMember]
        VariantParentMissingOrIncorrect = 6,

        /// <summary>
        /// Data Type  
        /// </summary>
        [EnumMember]
        DataType = 7,

        /// <summary>
        /// Required Attribute  
        /// </summary>
        [EnumMember]
        RequiredAttribute = 8,

        /// <summary>
        /// Value Length  
        /// </summary>
        [EnumMember]
        ValueLength = 9,

        /// <summary>
        /// Value Incorrect  
        /// </summary>
        [EnumMember]
        ValueIncorrect = 10,

        /// <summary>
        /// Value Precision  
        /// </summary>
        [EnumMember]
        ValuePrecision = 11,

        /// <summary>
        /// Valid Date  
        /// </summary>
        [EnumMember]
        ValidDate = 12,

        /// <summary>
        /// Fraction Check  
        /// </summary>
        [EnumMember]
        FractionCheck = 13,

        /// <summary>
        /// UOM Check  
        /// </summary>
        [EnumMember]
        UOMCheck = 14,

        /// <summary>
        /// Custom Expression Check  
        /// </summary>
        [EnumMember]
        CustomExpressionCheck = 15,

        /// <summary>
        /// Duplicate Values Check  
        /// </summary>
        [EnumMember]
        DuplicateValuesCheck = 16,

        /// <summary>
        /// Relationship Check  
        /// </summary>
        [EnumMember]
        RelationshipCheck = 17,

        /// <summary>
        /// Relationship Valid  
        /// </summary>
        [EnumMember]
        RelationshipValid = 18,

        /// <summary>
        /// Attribute Check  
        /// </summary>
        [EnumMember]
        AttributeCheck = 19,

        /// <summary>
        /// Cardinality Check  
        /// </summary>
        [EnumMember]
        CardinalityCheck = 20,

        /// <summary>
        /// Not specified  
        /// </summary>
        [EnumMember]
        NotSpecified = 21,

        /// <summary>
        /// System Error 
        /// </summary>
        [EnumMember]
        SystemError = 22,

        /// <summary>
        /// Entity Reference Check 
        /// </summary>
        [EnumMember]
        EntityReferencesCheck = 23,

        /// <summary>
        /// Invalid Data Locale
        /// </summary>
        [EnumMember]
        InvalidDataLocale = 24,

        /// <summary>
        /// Application Error 
        /// </summary>
        [EnumMember]
        ApplicationError = 25,

        /// <summary>
        /// Data Model Violation
        /// </summary>
        [EnumMember]
        DataModelViolation = 26,

        /// <summary>
        /// Missing or deleted entity
        /// </summary>
        [EnumMember]
        MissingOrDeletedEntity = 27,

        /// <summary>
        /// Incorrect Category
        /// </summary>
        [EnumMember]
        IncorrectCategory = 28,

        /// <summary>
        /// Entity lock
        /// </summary>
        [EnumMember]
        EntityLocked = 29,

        /// <summary>
        /// Permission Check
        /// </summary>
        [EnumMember]
        PermissionCheck = 30
    }

    #endregion
}