using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MDM.ExcelUtility
{
    using MDM.Utility;

    public sealed class RSExcelConstants
    {
        public const String MetadataSheetName = ExternalFileReader.MetadataSheetName; //This is moved to Utility.dll as ExternalFileReader needs this constant value
        public const String AttributeMetadataSheetName = "Attribute Metadata";
        public const String EntityInfoSheetName = "Entity Info";
        public const String EntityDataSheetName = "Entities";
        public const String RelationshipSheetName = "Relationships";
        public const String ComplexAttributeTemplateSheetName = "Complex Attribute Template";
        public const String LookupDictionarySheetName = "Lookup Table Dictionary";

        public const String ErrorsColumnName = "Errors";
        public const String WarningsColumnName = "Warnings";

        public const String LookupValuesSeparator = "||";

        public const String DefaultCommonAttributeFontRgb = "FF00008B";
        public const String DefaultRequiredFontRgb = "FFFF0000";
        public const String DefaultErroredForeground = "F25252";

        public const String DefaultSystemAttrFontRgb = "95b3d7";
        public const String DefaultWorkflowAttrFontRgb = "b6dde8";

        public const String DefaultCommonAttributeBackGroundColorFillRgb = "FFC5D9F1";
        public const String DefaultTechnicalAttributeBackGroundColorFillRgb = "FFD8E4BC";
        public const String DefaultRelationshipAttributeBackGroundColorFillRgb = "FFC5D9F1";

        public const String ExternalValidationFormulaFormat = "'" + LookupDictionarySheetName + "'!${0}${1}:${0}${2}";
        public const String ColumnRangeFormat = "{0}2:{0}1048576";

        public const String ErrorCaptionMessageCode = "112025";
        public const String ExpectedValuesMessageCode = "112029";
        public const String MandatoryFieldMessageCode = "112028";
        public const String NotificationCaptionMessageCode = "112024";
        public const String OptionalFieldMessageCode = "112027";
        public const String PredefinedValuesMessageCode = "112031";
        public const String ProvideValueMessageCode = "112026";
        public const String ValidDateValueMessageCode = "112030";
        public const String ValidDecimalValueMessageCode = "112033";
        public const String ValidIntegerValueMessageCode = "112055";
        public const String ValueCriteriaMessageCode = "112034";
        public const String ValueGreaterOrEqualMessageCode = "112035";
        public const String ValueLengthMessageCode = "112032";
        public const String ValueLessOrEqualMessageCode = "112036";
        public const String ContextLookupValueWarningMessageCode = "113961";

        public const UInt32 DefaultComplexAttributeTableRowIndex = 20;
        public const String ComplexAttributeMetadataTableCaption = "Complex Attribute Metadata";
        internal const String ComplexAttributeKeyFormat = "{0}//{1}";
        internal const String ComplexAttributeValueFormat = "{0}_{1}";

        public const String BlankText = "[BLANK]";

        #region Public Dictionaries

        public static ConcurrentDictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>> MetadataTemplateFields = new ConcurrentDictionary<MetadataTemplateFieldEnum, KeyValuePair<String, String>>();

        public static ConcurrentDictionary<EntityInfoTemplateFieldEnum, String> EntityStateInfoTemplateColumns = new ConcurrentDictionary<EntityInfoTemplateFieldEnum, String>();

        public static ConcurrentDictionary<EntityDataTemplateFieldEnum, String> EntityDataTemplateColumns = new ConcurrentDictionary<EntityDataTemplateFieldEnum, String>();

        public static ConcurrentDictionary<RelationshipDataTemplateFieldEnum, String> RelationshipDataTemplateColumns = new ConcurrentDictionary<RelationshipDataTemplateFieldEnum, String>();

        public static ConcurrentDictionary<String, String> AttributeMetadataTemplateColumns = new ConcurrentDictionary<String, String>();

        public static ConcurrentDictionary<ComplexAttributeTemplateFieldEnum, String> ComplexAttributeTemplateColumns = new ConcurrentDictionary<ComplexAttributeTemplateFieldEnum, String>();

        #endregion

        static RSExcelConstants()
        {
            CollectionSeparator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.CollectionSeparator");
            UomSeparator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.UomSeparator");
            BuildTemplateColumnDictionaries();
        }

        public static string CollectionSeparator { get; set; }
        public static string UomSeparator { get; set; }

        private static void BuildTemplateColumnDictionaries()
        {
            // Fields on Metadata sheet
            // Entity metadata
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultOrganization, new KeyValuePair<String, String>("Default Organization", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultContainer, new KeyValuePair<String, String>("Default Container", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultEntityType, new KeyValuePair<String, String>("Default Entity Type", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultCateogryPath, new KeyValuePair<String, String>("Default Category Path", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultParentExtensionContainer, new KeyValuePair<String, String>("Default Parent Extension Container", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultParentExtensionCategoryPath, new KeyValuePair<String, String>("Default Parent Extension Category Path", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultDataLocale, new KeyValuePair<String, String>("Default Data Locale", String.Empty));

            // Relationship metadata
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultFromEntityOrganization, new KeyValuePair<String, String>("Default From Entity Organization", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultFromEntityContainer, new KeyValuePair<String, String>("Default From Entity Container", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultFromEntityEntityType, new KeyValuePair<String, String>("Default From Entity Type", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultFromEntityCateogryPath, new KeyValuePair<String, String>("Default From Entity Category Path", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultToEntityOrganization, new KeyValuePair<String, String>("Default To Entity Organization", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultToEntityContainer, new KeyValuePair<String, String>("Default To Entity Container", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultToEntityEntityType, new KeyValuePair<String, String>("Default To Entity Type", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DefaultToEntityCateogryPath, new KeyValuePair<String, String>("Default To Entity Category Path", String.Empty));

            // Processing options
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.CollectionSeparator, new KeyValuePair<String, String>("Collection Separator", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.UomSeparator, new KeyValuePair<String, String>("UOM Separator", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.DeleteKeyword, new KeyValuePair<String, String>("Keyword To Delete Value", String.Empty));
            MetadataTemplateFields.TryAdd(MetadataTemplateFieldEnum.BlankKeyword, new KeyValuePair<String, String>("Keyword To Clear Value", String.Empty));

            //Fields on Entity info sheet
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.Id, "Id");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.ExtenalId, "External Id");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.EntityType, "Entity Type");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.CategoryPath, "Category Path");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.Container, "Container");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.InfoType, "Info Type");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.Name, "Name");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.Value, "Value");
            EntityStateInfoTemplateColumns.TryAdd(EntityInfoTemplateFieldEnum.MessageCodes, "Message Codes");

            // Fields on Entity Sheet
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.Id, "Id");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ExtenalId, "External Id");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.LongName, "Long Name");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.EntityType, "Entity Type");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.CategoryPath, "Category Path");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.CategoryLongNamePath, "Category Long Name Path");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.TargetCategoryPath, "Target Category Path");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.Container, "Container");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.Organization, "Organization");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ParentExternalId, "Parent External Id");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ParentExtensionExternalId, "Parent Extension External Id");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ParentExtensionCategoryPath, "Parent Extension Category Path");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ParentExtensionContainer, "Parent Extension Container");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.ParentExtensionOrganization, "Parent Extension Organization");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.Action, "Entity Action");
            EntityDataTemplateColumns.TryAdd(EntityDataTemplateFieldEnum.IsProcessed, "$Processed$");

            // Fields on Relationship Sheet
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.Id, "Id");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.RelationshipExternalId, "Relationship External Id");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.RelationshipType, "Relationship Type");

            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.FromEntityExternalId, "From Entity External Id");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.FromEntityEntityType, "From Entity Type");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.FromEntityCategoryPath, "From Entity Category Path");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.FromEntityContainer, "From Entity Container");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.FromEntityOrganization, "From Entity Organization");

            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.ToEntityExternalId, "To Entity External Id");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.ToEntityEntityType, "To Entity Type");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.ToEntityCategoryPath, "To Entity Category Path");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.ToEntityContainer, "To Entity Container");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.ToEntityOrganization, "To Entity Organization");

            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.IsProcessed, "$Processed$");
            RelationshipDataTemplateColumns.TryAdd(RelationshipDataTemplateFieldEnum.Action, "Action");

            // Complex Attribute Sheet
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.Id, "Id");
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.ExtenalId, "External Id");
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.LongName, "Long Name");

            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.EntityType, "Entity Type");
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.CategoryPath, "Category Path");
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.Container, "Container");

            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.Organization, "Organization");
            ComplexAttributeTemplateColumns.TryAdd(ComplexAttributeTemplateFieldEnum.Locale, "Locale");
        }
    }

    #region Enums

    public enum DataLoadType
    {
        EntityWithAttributes,
        EntityWithAttributesAndRelationships,
        EntityRelationshipsOnly
    }

    public enum MetadataTemplateFieldEnum
    {
        DefaultOrganization,
        DefaultContainer,
        DefaultEntityType,
        DefaultCateogryPath,
        DefaultParentExtensionContainer,
        DefaultParentExtensionCategoryPath,
        DefaultDataLocale,

        DefaultFromEntityOrganization,
        DefaultFromEntityContainer,
        DefaultFromEntityEntityType,
        DefaultFromEntityCateogryPath,

        DefaultToEntityOrganization,
        DefaultToEntityContainer,
        DefaultToEntityEntityType,
        DefaultToEntityCateogryPath,

        CollectionSeparator,
        UomSeparator,
        DeleteKeyword,
        BlankKeyword,

        ComplexAttributes
    }

    public enum EntityInfoTemplateFieldEnum
    {
        Id,
        ExtenalId,
        EntityType,
        CategoryPath,
        Container,
        InfoType,
        Name,
        Value,
        MessageCodes
    }

    public enum EntityDataTemplateFieldEnum
    {
        Id,
        ExtenalId,
        LongName,
        EntityType,
        CategoryPath,
        CategoryLongNamePath,
        TargetCategoryPath,
        Container,
        Organization,
        ParentExternalId,
        ParentExtensionExternalId,
        ParentExtensionCategoryPath,
        ParentExtensionContainer,
        ParentExtensionOrganization,
        IsProcessed,
        Action
    }

    public enum RelationshipDataTemplateFieldEnum
    {
        Id,
        RelationshipExternalId,
        RelationshipType,

        FromEntityExternalId,
        FromEntityEntityType,
        FromEntityCategoryPath,
        FromEntityContainer,
        FromEntityOrganization,

        ToEntityExternalId,
        ToEntityEntityType,
        ToEntityCategoryPath,
        ToEntityContainer,
        ToEntityOrganization,

        IsProcessed,
        Action
    }

    public enum ComplexAttributeTemplateFieldEnum
    {
        Id,
        ExtenalId,
        LongName,
        EntityType,
        CategoryPath,
        Container,
        Organization,
        Locale
    }

    #endregion
}
