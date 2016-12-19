using System;
using System.Globalization;
using System.Transactions;

namespace MDM.Core
{
    /// <summary>
    /// Contains the constants in the application
    /// </summary>
    public class Constants
    {
        #region Fields

        /// <summary>
        /// EntityTypeId for category entity type. Category is the pre-defined entity type in MDMCenter
        /// </summary>
        public static Int32 CATEGORY_ENTITYTYPE = 6;

        /// <summary>
        /// EntityTypeId for Entity entity type. Entity is the pre-defined entity type in MDMCenter
        /// </summary>
        public static Int32 ENTITY_ENTITYTYPE = 7;

        /// <summary>
        /// EntityTypeId for ChildEntity entity type. ChildEntity is the pre-defined entity type in MDMCenter
        /// </summary>
        public static Int32 CHILDENTITY_ENTITYTYPE = 15;

        /// <summary>
        /// Transaction code for Add action
        /// </summary>
        public static String TRANSACTIONCODE_ADD = "PHNA01";

        /// <summary>
        /// Transaction code for Update action
        /// </summary>
        public static String TRANSACTIONCODE_UPDATE = "PHNU01";

        /// <summary>
        /// Transaction code for Delete action
        /// </summary>
        public static String TRANSACTIONCODE_DELETE = "PHND01";

        /// <summary>
        /// Transaction isolation level for applications
        /// </summary>
        public static IsolationLevel DEFAULT_ISOLATION_LEVEL = IsolationLevel.ReadCommitted;

        /// <summary>
        /// Transaction timeout for sync process 
        /// </summary>
        public static TimeSpan TRANSACTION_TIMEOUT_SYNCPROCESS = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Transaction timeout for async process 
        /// </summary>
        public static TimeSpan TRANSACTION_TIMEOUT_ASYNCPROCESS = new TimeSpan(0, 5, 0);

        /// <summary>
        /// Constant represents Application level tracing enabled/disabled
        /// </summary>
        public static Boolean TRACING_ENABLED = false;

        /// <summary>
        /// Constant represents performance tracing enabled/disabled
        /// </summary>
        public static Boolean PERFORMANCE_TRACING_ENABLED = false;

        /// <summary>
        /// Constant represents empty attribute instance value ref id.
        /// </summary>
        public static Int32 COMPLEX_ATTRIBUTE_EMPTY_INSTANCE_VALUE_REF_ID = -100000;

        /// <summary>
        /// Constant represents storage culture information
        /// </summary>
        public static CultureInfo STORAGE_CULTUREINFO = new CultureInfo("en-US");

        /// <summary>
        /// Constant represents storage number style
        /// </summary>
        public static NumberStyles STORAGE_NUMBER_STYLE = NumberStyles.Integer;

        /// <summary>
        /// Constant represents storage date time styale
        /// </summary>
        public static DateTimeStyles STORAGE_DATETIME_STYLE = DateTimeStyles.None;

        /// <summary>
        /// 
        /// </summary>
        public static String XML_SPECIAL_CHARACTERS = "[]";

        /// <summary>
        /// Constant represents the format string for UOM column id's.
        /// </summary>
        public static String UOM_COLUMN_ID_FORMAT = "FK_UOM_{0}";

        /// <summary>
        /// Constant represents when a preloaded attribute model is not required (in BR)
        /// Use Case : If the BR preloads an attribute and also specifies IsModelRequired = false then this AttributeModel shoud NOT be loaded in the 
        /// entity.AttributeModels collection. By not loading this attributeModel, the entityEditor page will not render it on the screen
        /// Fix for Bug #126989
        /// </summary>
        public const String ATTRIBUTE_MODEL_NOT_REQUIRED = "ATTRIBUTEMODELNOTREQUIRED";

        /// <summary>
        /// Constant represents whether the attribute model is used only for inherited value calculation and not for returning in result set
        /// </summary>
        public const String ATTRIBUTE_MODEL_ONLY_FOR_INHERITED_VALUE_CALCULATION = "INCLUDE_ONLY_FOR_INHERITED_VALUE_CALCULATION";

        /// <summary>
        /// Constant represents first level relationship 
        /// </summary>
        public const Int16 RELATIONSHIP_LEVEL_ONE = 1;

        /// <summary>
        /// Guid representing Profile Tracing
        /// </summary>
        public static Guid ProfileTracingOperationId = new Guid("5454F255-5034-4CB7-AE2A-4BAFEAF8FFEB");

        /// <summary>
        /// Guid representing System Tracing
        /// </summary>
        public static Guid SystemTracingOperationId = new Guid("083C7A20-272A-4262-A741-3600FEE58844");

        /// <summary>
        /// Constant represents the string path separator
        /// </summary>
        public static String STRING_PATH_SEPARATOR = "#@#";

        /// <summary>
        /// Storage mode for the diagnostic tracing
        /// </summary>
        public static DiagnosticTracingStorageMode DIAGNOSTIC_TRACING_STORAGE_MODE = DiagnosticTracingStorageMode.DatabaseOnly;

        /// <summary>
        /// Storage mode for the diagnostic tracing
        /// </summary>
        public static String DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP = "MDMTraceSource.Imports|MDMTraceSource.Exports";

        /// <summary>
        /// Specifies Invariant date format
        /// </summary>
        public static String INVARIANT_DATE_FORMAT = "MM/dd/yyyy";

        /// <summary>
        /// Specifies date time in 24 hours format 
        /// </summary>
        public static String DATETIME_IN_24_HOURS_FORMAT = "MM/dd/yyyy HH:mm:ss";

        /// <summary>
        /// Specifies the min value allowed for fractions.
        /// </summary>
        public const String FRACTION_MINVALUE = "-100000000000000.0000";

        /// <summary>
        /// Specifies the max value allowed for fractions.
        /// </summary>
        public const String FRACTION_MAXVALUE = "999999999999999.9999";

        /// <summary>
        /// Specifies Category Source Name Mapping in Import Profile
        /// </summary>
        public const String CATEGORY_SOURCE_NAME_MAPPING = "CategoryId";

        /// <summary>
        /// Specifies Category Target Name Mapping in Import Profile
        /// </summary>
        public const String CATEGORY_TARGET_NAME_MAPPING = "FK_Category";

        /// <summary>
        /// Specifies lookup column name which is used as organization context
        /// </summary>
        public const String LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME = "OrganizationIdList";

        /// <summary>
        /// Specifies lookup column name which is used as container context
        /// </summary>
        public const String LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME = "ContainerIdList";

        /// <summary>
        /// Specifies lookup column name which is used as category path context
        /// </summary>
        public const String LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME = "CategoryPathList";

        /// <summary>
        /// Specifies date format which is used to display Date attribute value to user
        /// </summary>
        public const String SHORT_DATE_FORMAT = "d";

        /// <summary>
        /// Specifies date format which is used to display Date Time attribute value to user
        /// </summary>
        public const String GENERAL_LONG_DATE_FORMAT = "G";
        /// <summary>
        /// Specifies regex pattern which is used to replace all special characters other than alphabets and numeric.
        /// </summary>
        public const String ALLOWED_CHAR_REGEX_PATTERN = @"[^0-9A-Za-zА-Яа-яЁё]";

        /// <summary>
        /// Specifies the web event handlers event name
        /// </summary>
        public const String WEB_EVENT_HANDLERS_EVENT_NAME = "WebEventHandlers";

        /// <summary>
        /// Specifies the profile name for all attributes and all relationships for export to excel page.
        /// </summary>
        public const String ALL_ATTRIBUTES_AND_ALL_RELATIONSHIPS = "All Attributes and All Relationships (System)";
        /// <summary>
        /// Constant represents whether to track history or not without changing attribute value
        /// </summary>
        public static Boolean TRACK_HISTORY_ONLY_ON_VALUE_CHANGE = true;

        /// <summary>
        /// Specifies the state view attributes group name
        /// </summary>
        public const String STATE_VIEW_ATTRIBUTES_GROUP_NAME = "State View Attributes";

        /// <summary>
        /// Constant represents the referential lookup column name format
        /// </summary>
        public static String LOOKUP_RELATIONSHIP_COLUMN_NAME_FORMAT = "{0}_{1}_DisplayFormat";

        /// <summary>
        /// Specifies the regular expression for date validation
        /// </summary>
        public const String DATE_VALIDATION_EXPRESSION = @"^(?:(?:(?:0?[13578]|1[02])(\/)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{4})$|^(?:0?2(\/)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{4})$";

        /// <summary>
        /// Specifies the regular expression for image validation
        /// </summary>
        public const String IMAGE_VALIDATION_EXPRESSION = @"/([A-Z,a-z,0-9,_,\-,\$,\#,\@,\!,\^,\&,\+,\%,\(,\),\=\[,\],\\,\/,\s,:])*.(jpg|JPG|jpeg|JPEG|GIF|gif|bmp|BMP|png|PNG|tiff|TIFF|EXIF|exif)/";

        /// <summary>
        /// Specifies the regular expression for image URL validation
        /// </summary>
        public const String IMAGEURL_VALIDATION_EXPRESSION = @"(((((h|H)(t|T))|((f|F)))(t|T)(p|P)(s|S?)://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)?([A-Z,a-z,0-9,_,\-,\$,\#,\@,\!,\^,\&,\+,\%,\(,\),\=,\[,\],\\,\/,\s,:,\.])*jpg|JPG|jpeg|JPEG|GIF|gif|bmp|BMP|png|PNG|tiff|TIFF|EXIF|exif$";

        /// <summary>
        /// Specifies the regular expression for URL validation
        /// </summary>
        public const String URL_VALIDATION_EXPRESSION = @"((((((h|H)(t|T))|((f|F)))(t|T)(p|P)(s|S?)://)|(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*)))?((([\w-]+\.)+[\w-]+(/[\w- ./?%&#=]*)?)|(([\w-]+/)+[\w-]+[\.]([\w- /?%&#=]*)?)))";

        /// <summary>
        /// Specifies the regular expression for fraction validation of mixed number
        /// </summary>
        public const String MIXED_FRACTION_VALIDATION_EXPRESSION = @"^-?\d+[\ ,\-,_]{1}\d+\/{1}\d+$";

        /// <summary>
        /// Specifies the regular expression for fraction validation of whole number
        /// It is not the mixed fraction. It checks for valid whole number or fraction
        /// </summary>
        public const String WHOLENUMBER_FRACTION_VALIDATION_EXPRESSION = @"^-?\d+$|^-?\d+\/{1}\d+$";

        /// <summary>s
        /// Specifies data model changes logging is on or off
        /// </summary>
        public const String ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY = @"MDMCenter.DataModelActivityLog.Enabled";

        /// <summary>
        /// Specifies whether lookup tables changes are logged or not
        /// </summary>
        public const String ACTIVITYLOG_LOOKUPVALUES_ENABLED = @"MDMCenter.LookupValuesActivityLog.Enabled";

        /// <summary>
        /// Serialization used in persisting hierarchical attribute xml. A change to this will result in data migration step when customers upgrade.
        /// </summary>
        public const ObjectSerialization HIERARCHICAL_ATTRIBUTE_SERIALIZATION = ObjectSerialization.DataTransfer;

        /// <summary>
        /// Specifies the name of system admin role
        /// </summary>
        public const String SYSTEM_ADMIN_USER_ROLE = "SystemAdmin";

        /// <summary>
        /// Specifies the name of bulk admin role
        /// </summary>
        public const String BULK_ADMIN_USER_ROLE = "Bulk Admin";

        /// <summary>
        /// Specifies the name of translation admin role
        /// </summary>
        public const String TRANSLATION_ADMIN_USER_ROLE = "Translation Admin";

        /// <summary>
        /// Specifies the name of lookup security lookup table name
        /// </summary>
        public const String LOOKUP_SECURITY_TABLE_NAME = "Lookup_Security";
        /// <summary>
        /// Specifies first level for entity variants
        /// </summary>
        public const Int16 VARIANT_LEVEL_ONE = 1;

        /// <summary>
        /// Specifies first level for container hierarchy
        /// </summary>
        public const Int16 CONTAINER_LEVEL_ONE = 1;

        /// <summary>
        /// Constant to be used to indicate the DDG object type by the ApplicationContext constructor
        /// </summary>
        public static Int32 DDG_OBJECTTYPE_ID = 5;

        /// <summary>
        /// Constant indicating name for application context
        /// NOTE : it is being used for Application Context Mapping and EVD Mapping.
        /// </summary>
        public const String APPLICATION_CONTEXT_NAME = "ORG{0}_CT{1}_CAT{2}_ET{3}_EN{4}_ATTR{5}_RT{6}_ROLE{7}_USER{8}_TYPE{9}";

        /// <summary>
        /// Default sort order for Rules Mapped to BC or BR, BC mapped to RuleMapContext
        /// </summary>
        public const Int32 DDG_DEFAULT_SORTORDER = 100;

        /// <summary>
        /// Indicates the shortname property of mdmobject
        /// </summary>
        public const String SHORTNAME = "NAME";

        /// <summary>
        /// Indicates the longname property of mdmobejct
        /// </summary>
        public const String LONGNAME = "LONGNAME";

        #endregion Fields
    }

    /// <summary>
    /// Holds constants which are used for building Cache keys.
    /// </summary>
    public class CacheKeyContants
    {
        #region Fields

        /// <summary>
        /// Specifies the cache key Prefix for local cache version.
        /// </summary>
        public const String LOCAL_VERSION_PREFIX_KEY = "LOCAL_VER.";

        /// <summary>
        /// Specifies the cache key Prefix for distributed cache version.
        /// </summary>
        public const String DISTRIBUTED_VERSION_PREFIX_KEY = "DIST_VER.";

        /// <summary>
        /// Specifies the cache key Prefix for app configuration.
        /// </summary>
        public const String APP_CONFIG_CACHE_KEY_PREFIX = "APP_CONFIG.";

        /// <summary>
        /// Specifies the cache key Application tracing enabled/disabled.
        /// </summary>
        public const String TRACING_ENABLED = "APP_CONFIG.MDMCenter.Diagnostics.Tracing.Enabled";

        /// <summary>
        /// Specifies the cache key performance tracing enabled/disabled.
        /// </summary>
        public const String PERFORMANCE_TRACING_ENABLED = "APP_CONFIG.MDMCenter.Diagnostics.PerformanceTracing.Enabled";

        /// <summary>
        /// Transaction isolation level for applications
        /// </summary>
        public const String DEFAULT_ISOLATION_LEVEL = "DEFAULT_ISOLATION_LEVEL";

        /// <summary>
        /// Transaction timeout for sync process 
        /// </summary>
        public const String TRANSACTION_TIMEOUT_SYNCPROCESS = "TRANSACTION_TIMEOUT_SYNCPROCESS";

        /// <summary>
        /// Transaction timeout for async process 
        /// </summary>
        public const String TRANSACTION_TIMEOUT_ASYNCPROCESS = "TRANSACTION_TIMEOUT_ASYNCPROCESS";

        /// <summary>
        /// Specifies the cache key for transaction settings. 
        /// </summary>
        public const String TRANSACTION_SETTINGS = "APP_CONFIG.MDMCenter.TransactionSettings";

        /// <summary>
        /// Specifies the cache key prefix for category search filter. 
        /// </summary>
        public const String CATEGORY_SEARCH_FILTER_KEY_PREFIX = "DT_Tax";

        /// <summary>
        /// Specifies the cache key for categories changed notification. 
        /// </summary>
        public const String CATEGORIES_CHANGED_KEY = "CATEGORIES_CHANGED";

        /// <summary>
        /// Specifies the cache key Prefix for MDM feature configuration.
        /// </summary>
        public const String MDMFEATURE_CONFIG_CACHE_KEY_PREFIX = "MDMFEATURE_CONFIG.";

        /// <summary>
        /// 
        /// </summary>
        public const String MDMCENTER_CACHEOBJECT_CONFIG = "MDMCenter.CacheObject.Configuration";

        /// <summary>
        /// Specifies the diagnostic storage mode.
        /// </summary>
        public const String DIAGNOSTIC_TRACING_STORAGE_MODE = "APP_CONFIG.MDMCenter.Diagnostics.Tracing.StorageMode";

        /// <summary>
        /// Specifies the diagnostic storage mode.
        /// </summary>
        public const String DIAGNOSTIC_SYSTEMLOG_LEGACYSOURCESTOSKIP = "APP_CONFIG.MDMCenter.Diagnostics.SystemLog.LegacyTraceSourcesToSkip";

        /// <summary>
        /// Specifies the diagnostic storage mode.
        /// </summary>
        public const String DIAGNOSTIC_TRACING_PROFILE = "APP_CONFIG.MDMCenter.TracingProfile";

        /// <summary>
        /// Specifies the cache key for track history based on source tracking module enabled without changing attribute value
        /// </summary>
        public const String TRACK_HISTORY_ONLY_ON_VALUE_CHANGE = "APP_CONFIG.MDMCenter.Entity.SourceTracking.TrackHistoryOnlyOnValueChange";

        /// <summary>
        /// Specifies the cache key Prefix for attribute models cache.
        /// </summary>
        public const String ATTRIBUTE_SEARCH_CACHE_KEY_PREFIX = "ATTRIBUTE_SEARCH";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if attribute models are changed.
        /// </summary>
        public const String ATTRIBUTE_MODELS_CHANGED_KEY = "ATTRIBUTE_MODELS_CHANGED";

        /// <summary>
        /// Specifies the cache key Prefix for relationship type hierarchy cache.
        /// </summary>
        public const String RELATIONSHIP_HIERARCHY_CACHE_KEY_PREFIX = "RELATIONSHIP_HIERARCHY";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if relationship type hierarchy are changed.
        /// </summary>
        public const String RELATIONSHIP_HIERARCHY_CHANGED_KEY = "RELATIONSHIP_HIERARCHY_CHANGED";

        /// <summary>
        /// Specifies the cache key Prefix for common attribute mapping cache.
        /// </summary>
        public const String COMMON_ATTRIBUTE_CACHE_KEY_PREFIX = "COMMON_ATTRIBUTE";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if common attribute mapping is changed.
        /// </summary>
        public const String COMMON_ATTRIBUTE_CHANGED_KEY = "COMMON_ATTRIBUTE_CHANGED";

        /// <summary>
        /// Specifies the cache key suffix for attributes permissions cache.
        /// </summary>
        public const String USERROLE_PERMISSIONS_CACHE_KEY_SUFFIX = "USERROLE_PERMISSIONS";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if permissions of attributes are changed.
        /// </summary>
        public const String USERROLE_PERMISSIONS_CHANGED_KEY = "USERROLE_PERMISSIONS_CHANGED";

        /// <summary>
        /// Specifies the cache key Prefix for container entity type mapping cache.
        /// </summary>
        public const String CONTAINER_ENTITYTYPE_MAPPING_CACHE_KEY_PREFIX = "CONTAINER_ENTITYTYPE_MAPPING";

        /// <summary>
        /// Specifies the cache key Prefix for meta data page config.
        /// </summary>
        public const String METADATA_PAGE_CONFIG_KEY_PREFIX = "MetaDataPageConfig";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if container entity type mapping is changed.
        /// </summary>
        public const String CONTAINER_ENTITYTYPE_MAPPING_CHANGED_KEY = "CONTAINER_ENTITYTYPE_MAPPING_CHANGED";

        /// <summary>
        /// Specifies the cache key Prefix for technical attribute mapping cache.
        /// </summary>
        public const String TECHNICAL_ATTRIBUTE_CACHE_KEY_PREFIX = "TECHNICAL_ATTRIBUTE";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if technical attribute mapping is changed.
        /// </summary>
        public const String TECHNICAL_ATTRIBUTE_CHANGED_KEY = "TECHNICAL_ATTRIBUTE_CHANGED";

        /// <summary>
        /// Specifies the cache key prefix which is used for MDM event handlers.
        /// </summary>
        public const String MDM_EVENT_HANDLER_CACHE_KEY_PREFIX = "MDM_EVENT_HANDLER_";

        /// <summary>
        /// Specifies the cache key which is used for cache notification if MDM Event handlers are reset.
        /// </summary>
        public const String MDM_EVENT_HANDLER_RESET_KEY = "MDM_EVENT_HANDLER_RESET_KEY";

        /// <summary>
        /// Specifies the cache key for Web Event Handlers.
        /// </summary>
        public const String MDM_WEB_EVENT_HANDLERS_CACHE_KEY = "MDM_WEB_EVENT_HANDLERS";

        /// <summary>
        /// Specifies the session data locale for user cache key prefix 
        /// </summary>
        public const String SESSION_DATA_LOCALE_FOR_USER_PREFIX = "RS_SessionDataLocale_UserName";

        /// <summary>
        /// Specifies cache key prefix to cache parsed DDG rule
        /// </summary>
        public const String PARSED_RULE_CACHE_PREFIX = "MDM_ParsedRule_Id-";

        #endregion Fields
    }

    /// <summary>
    /// Constants for export Profile
    /// </summary>
    public class ExportProfileConstants
    {
        /// <summary>
        /// Export Setting Description
        /// </summary>
        public const String DESCRIPTION = "Description";

        /// <summary>
        ///  Export Setting ApprovedCopy
        /// </summary>
        public const String APPROVEDCOPY = "ApprovedCopy";

        /// <summary>
        ///  Export Setting IsCategoryExport
        /// </summary>
        public const String IS_CATEGORY_EXPORT = "IsCategoryExport";

        /// <summary>
        /// SUBSCRIBER_QUEUE_LABEL
        /// </summary>
        public const String SUBSCRIBERQUEUELABEL = "SubscriberQueueLabel";
        /// <summary>
        /// INCLUDE_VARIANT_MODE
        /// </summary>
        public const String INCLUDEVARIANTMODE = "Include Variant Mode";
        /// <summary>
        /// INCLUDE_EXTENSION_MODE
        /// </summary>
        public const String INCLUDEEXTENSIONMODE = "Include Extension Mode";
        /// <summary>
        /// INCLUDE_INHERITABLE_ATTRIBUTES
        /// </summary>
        public const String INCLUDEINHERITABLEATTRIBUTES = "Include Inheritable Attributes";
        /// <summary>
        /// INCLUDE_BUSINESS_CONDITIONS
        /// </summary>
        public const String INCLUDEBUSINESSCONDITIONS = "Include Business Conditions";
        /// <summary>
        /// INCLUDE_ENTITY_STATES
        /// </summary>
        public const String INCLUDEENTITYSTATES = "Include Entity States";
        /// <summary>
        /// SEND_ONLY_IF_ITEM_COUNT_IS_MORE_THAN_ZERO
        /// </summary>
        public const String SENDONLYIFITEMCOUNTISMORETHANZERO = "SendOnlyIfItemCountIsMoreThanZero";
        /// <summary>
        /// APPLY_EXPORT_MASK_TO_LOOK_UP_ATTRIBUTE
        /// </summary>
        public const String APPLYEXPORTMASKTOLOOKUPATTRIBUTE = "ApplyExportMaskToLookupAttribute";
        /// <summary>
        /// EXPORT_FILE_SPLIT_TYPE
        /// </summary>
        public const String EXPORTFILESPLITTYPE = "ExportFileSplitType";
        /// <summary>
        /// ATTRIBUTE_HEADER_FORMAT
        /// </summary>
        public const String ATTRIBUTEHEADERFORMAT = "AttributeHeaderFormat";
        /// <summary>
        /// INCLUDE_CATEGORY_LONG_NAME_PATH
        /// </summary>
        public const String INCLUDECATEGORYLONGNAMEPATH = "IncludeCategoryLongNamePath";
        /// <summary>
        /// SORT_ENTITIES_BY
        /// </summary>
        public const String SORTENTITIESBY = "SortEntitiesBy";
        /// <summary>
        /// SORT_ATTRIBUTES_BY
        /// </summary>
        public const String SORTATTRIBUTESBY = "SortAttributesBy";
        /// <summary>
        /// BATCH_SIZE
        /// </summary>
        public const String BATCHSIZE = "BatchSize";
        /// <summary>
        /// EXECUTION_MODE
        /// </summary>
        public const String EXECUTIONMODE = "ExecutionMode";
        /// <summary>
        /// LABEL
        /// </summary>
        public const String LABEL = "Label";

    }

    /// <summary>
    /// Constants for LifecycleStatus attribute
    /// </summary>
    public sealed class LifecycleStatusValues
    {
        /// <summary>
        /// Indicates that the entity is in "New" state
        /// </summary>
        public const String New = "New";

        /// <summary>
        /// Indicates that the entity is in "In Progress" state
        /// </summary>
        public const String InProgress = "In Progress";

        /// <summary>
        /// Indicates that the entity is in "Promoted" state
        /// </summary>
        public const String Promoted = "Promoted";

        /// <summary>
        /// Indicates that the entity is in "Pending Match Review" state
        /// </summary>
        public const String PendingMatchReview = "Pending Match Review";

        /// <summary>
        /// Indicates that the entity is in "Marked for Deletion" state
        /// </summary>
        public const String MarkedForDeletion = "Marked for Deletion";
    }
}
