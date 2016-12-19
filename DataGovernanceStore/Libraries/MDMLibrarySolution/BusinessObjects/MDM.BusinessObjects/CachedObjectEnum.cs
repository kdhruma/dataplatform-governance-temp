using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.Core;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CacheKeyEnum : MDMEnum<CacheKeyEnum>
    {
        private String _keyFormat;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String KeyFormat
        {
            get { return _keyFormat; }
            set { _keyFormat = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordinal"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="keyFormat"></param>
        public CacheKeyEnum(Int32 ordinal, String name, String displayName, String keyFormat)
            :base(ordinal, name, displayName)
        {
            _keyFormat = keyFormat;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public sealed class EntityCacheKeyEnum : CacheKeyEnum
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [DataMember]
        public static EntityCacheKeyEnum EntityBaseCacheKey = new EntityCacheKeyEnum(1, "EntityBaseCacheKey", "Entity  Base", "EN.E{0}.T{1}_L{2}.Base");
        
        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public static EntityCacheKeyEnum EntityBaseWithContainerCacheKey = new EntityCacheKeyEnum(2, "EntityBaseWithContainerCacheKey", "Entity Base With Container", "EN.E{0}.T{1}_CON{2}_L{3}.Base");

        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public static EntityCacheKeyEnum EntityBaseWithSourcesCacheKey = new EntityCacheKeyEnum(3, "EntityBaseWithSourcesCacheKey", "Entity  Base With Sources", "EN.E{0}.T{1}_L{2}.Base.SRC");
        
        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public static EntityCacheKeyEnum EntityBaseWithSourcesAndContainerCacheKey = new EntityCacheKeyEnum(4, "EntityBaseWithSourcesAndContainerCacheKey", "Entity  Base With Sources And Container", "EN.E{0}.T{1}_CON{2}_L{3}.Base.SRC");
        
        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public static EntityCacheKeyEnum EntityLocalAttributesCacheKey = new EntityCacheKeyEnum(5, "EntityLocalAttributesCacheKey", "Entity Local Attributes", "EN.E{0}.T{1}_L{2}.ELA");

        /// <summary>
        /// 
        /// </summary>
         [DataMember]
        public static EntityCacheKeyEnum EntityInheritedAttributesCacheKey = new EntityCacheKeyEnum(6, "EntityInheritedAttributesCacheKey", "Entity  Inherited Attributes", "EN.E{0}.T{1}_L{2}.EIA");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordinal"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="keyFormat"></param>
        public EntityCacheKeyEnum(Int32 ordinal, String name, String displayName, String keyFormat)
            :base(ordinal, name, displayName, keyFormat)
        {
        }
    }

    //public enum CachedObjectEnum
    //{
    //    AttributeModel,
    //    AttributeModelTypeCommon,
    //    AttributeModelTypeMetadataAttribute,
    //    AttributeModelTypeRelationship,
    //    AttributeModelTypeSystem,
    //    Category,
    //    CategoryAttributeMapping,
    //    CategoryLocal,
    //    ClaimTypeMappings,
    //    ConnectorProfilesAll,
    //    ContainerLocal,
    //    DataQualityClassesAll,
    //    DQMConfigAll,
    //    DQMProfileGroupProfileAll,
    //    DQMRuleAll,
    //    Entity,
    //    EntityType,
    //    FatalException,
    //    FilterCriteriaSettingsAll,
    //    FormAuthentication,
    //    HierarchyAll,
    //    IntegrationDimensionAll,
    //    IntegrationMessageTypes,
    //    DataQualityIndicatorAll,
    //    Locale,
    //    LocaleAll,
    //    LocaleByOrg,
    //    LocaleMessage,
    //    LookupData,
    //    MatchingProfileAll,
    //    MatchStoresAll,
    //    MDMAppConfig,
    //    MDMAppConfigItemsByEventSourceAndSubscriber,
    //    MDMAppConfigItemsList,
    //    MDMFeatureConfig,
    //    MDMObjectTypeAll,
    //    MDMRelationshipDenormPrcessSettings,
    //    MenuObject,
    //    MergePlanningProfilesAll,
    //    MergeProfileAll,
    //    MergeResults,
    //    MetadataConfig,
    //    NormalizationProfileAll,
    //    Organization,
    //    RelationshipType,
    //    RoleMappings,
    //    SecurityPermissionDefinition,
    //    SecurityPrincipal,
    //    ServerInfo,
    //    SourcesAll,
    //    SourceValue,
    //    SurvivorshipRuleSetAll,
    //    UserPermission,
    //    UserRole,
    //    ValidationProfileAll,
    //    VendorUser,
    //    WordList,
    //    WordListsAll
    //}
}
