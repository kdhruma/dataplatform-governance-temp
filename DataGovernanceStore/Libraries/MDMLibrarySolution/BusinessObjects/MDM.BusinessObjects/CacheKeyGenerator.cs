using System;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using System.Collections.Generic;

    /// <summary>
    /// Cache key is utility class to provide cache keys for various Business objects
    /// </summary>
    public sealed class CacheKeyGenerator
    {
        #region Entity Cache Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityBaseCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.Base", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.Base", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityBaseWithSourcesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.Base.SRC", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.Base.SRC", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityLocalAttributesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            //Category level attributes are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.ELA", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.ELA", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityLocalAttributesWithSourcesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            //Category level attributes are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.ELA.SRC", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.ELA.SRC", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityInheritedAttributesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            //Category level attributes are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.EIA", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.EIA", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityInheritedAttributesWithSourcesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            //Category level attributes are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_L{3}.EIA.SRC", entityId, "CAT", containerId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}_L{2}.EIA.SRC", entityId, "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public static String GetEntityHierarchyRelationshipsCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId)
        {
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}.EHR", entityId, "CAT", containerId);
            else
                return String.Format("EN.E{0}.T{1}.EHR", entityId, "EN");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static String GetEntityExtensionRelationshipsCacheKey(Int64 entityId)
        {
            return String.Format("EN.E{0}.T{1}.EER", entityId, "EN");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static String GetEntityEditorEntityCacheKey(Int64 entityId, AttributeModelType attributeModelType, Int32 userId)
        {
            return String.Format("RS_EntityEditor_Entity_AttributeModelType:{0}_UserId:{1}_EntityId:{2}", attributeModelType, userId, entityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="operation"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static String GetEntityEditorEntityDiagonosticReportCacheKey(Int64 entityId, String operation, Int32 userId)
        {
            return String.Format("RS_EntityEditor_DiagnosticReport_Operation:{0}_UserId:{1}_EntityId:{2}", operation, userId, entityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static String GetEntityImportJobDiagonosticReportCacheKey(Int32 userId, Int32 jobId)
        {
            return String.Format("RS_ImportJob_DiagnosticReport:UserId:{0}_JobId:{1}", userId, jobId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static String GetEntityExportJobDiagonosticReportCacheKey(Int32 userId, Int32 jobId)
        {
            return String.Format("RS_ExportJob_DiagnosticReport:UserId:{0}_JobId:{1}", userId, jobId);
        }

        #region Relationships Cache Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipParentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 relationshipParentId, Int16 level)
        {
            //Category level relationships are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_RT{3}_RELPID{4}_LVL{5}.ER", entityId, "CAT", containerId, relationshipTypeId, relationshipParentId, level);
            else
                return String.Format("EN.E{0}.T{1}_RT{2}_RELPID{3}_LVL{4}.ER", entityId, "EN", relationshipTypeId, relationshipParentId, level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipParentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsWithSourcesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 relationshipParentId, Int16 level)
        {
            //Category level relationships are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}_CON{2}_RT{3}_RELPID{4}_LVL{5}.ER.SRC", entityId, "CAT", containerId, relationshipTypeId, relationshipParentId, level);
            else
                return String.Format("EN.E{0}.T{1}_RT{2}_RELPID{3}_LVL{4}.ER.SRC", entityId, "EN", relationshipTypeId, relationshipParentId, level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsAttributesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 relationshipId, LocaleEnum locale)
        {
            //Category level attributes are always stored in context of container..thus it would have separate keys
            if (entityTypeId == 6)
                return String.Format("EN.E{0}.T{1}.CON{2}.RT{3}.RELID{4}.L{5}.ERA", entityId, "CAT", containerId, relationshipTypeId, relationshipId, (Int32)locale);
            else
                return String.Format("EN.E{0}.T{1}.RT{2}.RELID{3}_L{4}.ERA", entityId, "EN", relationshipTypeId, relationshipId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="relationshipId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsAttributesWithSourcesCacheKey(Int64 entityId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 relationshipId, LocaleEnum locale)
        {
            return String.Concat(GetEntityRelationshipsAttributesCacheKey(entityId, containerId, entityTypeId, relationshipTypeId, relationshipId, locale), ".SRC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsCacheKey(Int64 relationshipId, LocaleEnum locale)
        {
            return String.Format("EN.RELID{0}.L{1}.ER", relationshipId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityRelationshipsWithSourcesCacheKey(Int64 relationshipId, LocaleEnum locale)
        {
            return String.Format("EN.RELID{0}.L{1}.ER.SRC", relationshipId, (Int32)locale);
        }

		/// <summary>
        /// Generates entity relationships cache keys for all level
        /// </summary>
        /// <param name="entityId">Indicates entity id for which cache key need to be created.</param>
        /// <param name="containerId">Indicates container id for which cache key need to be created.</param>
        /// <param name="entityTypeId">Indicates entity type id for which cache key need to be created.</param>
        /// <returns>Returns the get entity relationships details cache key for the specified locale.</returns>
        public static String GetEntityRelationshipsCacheKeyForAllLevel(Int64 entityId, Int32 containerId, Int32 entityTypeId)
        {
            //Category level relationships are always stored in context of container..thus it would have separate keys
            if (entityTypeId == Constants.CATEGORY_ENTITYTYPE)
                return String.Format("EN.E{0}.T{1}_CON{2}.AllLevel", entityId, "CAT", containerId);
            else
                return String.Format("EN.E{0}.T{1}.AllLevel", entityId, "EN");
        }


        #endregion

        #region Relationshsip Cardinality Cache Key

        /// <summary>
        /// Generates relationship cardinality Cache key
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indicates container Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <returns>Cache key for relationship cardinality</returns>
        public static String GetRelationshipCardinalitiesCacheKey(Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId)
        {
            return String.Format("REL.CAR_RT{0}.CON{1}.FET{2}", relationshipTypeId, containerId, fromEntityTypeId);
        }

        #endregion Relationshsip Cardinality Cache Key

        #endregion

        #region Entity History Details Templates Cache Key

        /// <summary>
        /// Generates Entity History Details Template Cache key
        /// </summary>
        /// <param name="locale">Indicate the locale for which cache key need to be created.</param>
        /// <returns>Returns the Entity History details template cache key for the specifiied locale.</returns>
        public static String GetEntityHistoryDetailsTemplateCacheKey(LocaleEnum locale)
        {
            return String.Format("EN.T{0}_L{1}.EHDT", "EN", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUIDate"></param>
        /// <param name="currentUILocale"></param>
        /// <param name="currentUITimeZone"></param>
        /// <returns></returns>
        public static String GetAvailableModifiedDayGroupsCacheKey(DateTime currentUIDate, LocaleEnum currentUILocale, String currentUITimeZone)
        {
            return String.Format("EN.MDG_D{0}_L{1}.T{2}.EHDT", currentUIDate.Date, currentUILocale.ToString(), currentUITimeZone);
        }

        #endregion

        #region Attribute Model Cache Keys

        /// <summary>
        /// Generates Attribute Model Cache Key
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetAttributeModelCacheKey(Int32 attributeId, AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, LocaleEnum locale)
        {
            String cacheKey = String.Empty;

            if (attributeModelType == AttributeModelType.Common)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_CON{2}_ET{3}_L{4}", attributeId, "CA", containerId, entityTypeId, (Int32)locale);
            }
            else if (attributeModelType == AttributeModelType.Category)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_CAT{2}_L{3}", attributeId, "TA", categoryId, (Int32)locale);
            }
            else if (attributeModelType == AttributeModelType.Relationship)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_CON{2}_RT{3}_L{4}", attributeId, "RA", containerId, relationshipTypeId, (Int32)locale);
            }
            else if (attributeModelType == AttributeModelType.AttributeMaster || attributeModelType == AttributeModelType.AttributeGroup)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_L{2}", attributeId, "Base", (Int32)locale);
            }
            else if (attributeModelType == AttributeModelType.System)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_L{2}", attributeId, "SYS", (Int32)locale);
            }
            else if (attributeModelType == AttributeModelType.MetaDataAttribute)
            {
                cacheKey = String.Format("AM.A{0}.TYPE{1}_L{2}", attributeId, "MA", (Int32)locale);
            }
            return cacheKey;
        }

        /// <summary>
        /// Returns the cache key for the AttributeModelContext.
        /// </summary>
        /// <param name="attributeModelContext"></param>
        /// <returns></returns>
        public static String GetAttributeModelCacheKey(AttributeModelContext attributeModelContext)
        {
            LocaleEnum localeEnum = LocaleEnum.en_WW;

            var attributeModelContextLocales = attributeModelContext.GetLocales();

            if (attributeModelContextLocales != null && attributeModelContextLocales.Count > 0)
                localeEnum = attributeModelContext.Locales[0];

            return CacheKeyGenerator.GetAttributeModelCacheKey(0, attributeModelContext.AttributeModelType, attributeModelContext.ContainerId,
                attributeModelContext.EntityTypeId, attributeModelContext.RelationshipTypeId, attributeModelContext.CategoryId, localeEnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static AttributeModelContext GetAttributeModelContext(String cacheKey)
        {
            AttributeModelContext attributeModelContext = null;

            Type type = GetObjectType(cacheKey);

            if (type == typeof(AttributeModel))
            {
                attributeModelContext = new AttributeModelContext();

                Int32 contextStartIndex = cacheKey.LastIndexOf('.');
                contextStartIndex++;

                String contextString = cacheKey.Substring(contextStartIndex);

                if (contextString.StartsWith("TYPECA"))
                {
                    attributeModelContext.AttributeModelType = AttributeModelType.Common;

                    String[] contextData = contextString.Split(new char[] { '_' });

                    if (contextData != null && contextData.Length == 4)
                    {
                        String strContainerId = contextData[1].Substring(3); //remove starting "CON"
                        String strEntityTypeId = contextData[2].Substring(2); // remove starting "ET"
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        locale = (LocaleEnum)ValueTypeHelper.Int32TryParse(contextData[3].Substring(1), 0);// remove "L" and convert to enum

                        attributeModelContext.ContainerId = ValueTypeHelper.Int32TryParse(strContainerId, 0);
                        attributeModelContext.EntityTypeId = ValueTypeHelper.Int32TryParse(strEntityTypeId, 0);
                        attributeModelContext.Locales = new Collection<LocaleEnum> {locale};
                    }
                }
                else if (contextString.StartsWith("TYPETA"))
                {
                    attributeModelContext.AttributeModelType = AttributeModelType.Category;

                    String[] contextData = contextString.Split(new char[] { '_' });

                    if (contextData != null && contextData.Length == 3)
                    {
                        String strCategoryId = contextData[1].Substring(3); //remove starting "CAT"
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        locale = (LocaleEnum)ValueTypeHelper.Int32TryParse(contextData[2].Substring(1), 0);// remove "L" and convert to enum

                        attributeModelContext.CategoryId = ValueTypeHelper.Int32TryParse(strCategoryId, 0);
                        attributeModelContext.Locales = new Collection<LocaleEnum> { locale };
                    }
                }
                else if (contextString.StartsWith("TYPERA"))
                {
                    attributeModelContext.AttributeModelType = AttributeModelType.Relationship;

                    String[] contextData = contextString.Split(new char[] { '_' });

                    if (contextData != null && contextData.Length == 4)
                    {
                        String strContainerId = contextData[1].Substring(3); //remove starting "CON"
                        String strRelationshipTypeId = contextData[2].Substring(2); // remove starting "RT"
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        locale = (LocaleEnum)ValueTypeHelper.Int32TryParse(contextData[3].Substring(1), 0);// remove "L" and convert to enum

                        attributeModelContext.ContainerId = ValueTypeHelper.Int32TryParse(strContainerId, 0);
                        attributeModelContext.RelationshipTypeId = ValueTypeHelper.Int32TryParse(strRelationshipTypeId, 0);
                        attributeModelContext.Locales = new Collection<LocaleEnum> { locale };
                    }
                }
            }

            return attributeModelContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllAttributeUniqueIdentifierToAttributeIdMapsCacheKey()
        {
            return "GetAllAttributeUniqueIdentifierToAttributeIdMaps";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllAttributeNameToAttributeIdMapsCacheKey()
        {
            return "GetAllAttributeNameToAttributeIdMaps";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetAttributeModelLocalePropertiesCacheKey(LocaleEnum locale)
        {
            return String.Format("AM.Localized_L{0}", (Int32)locale);
        }

        #endregion

        #region Category Cache Keys

        /// <summary>
        /// Method that wil return GetCategoryLocalePropertiesCacheKey based on Locale and hierarchyId
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        public static String GetCategoryLocalePropertiesCacheKey(LocaleEnum locale, Int32 hierarchyId)
        {
            return String.Format("CAT_L{0}_H{1}", (Int32)locale, hierarchyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetBaseCategoriesCacheKey(Int32 hierarchyId)
        {
            return String.Format("AllCategoriesWithBasicProperties_Hierarchy{0}", hierarchyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllBaseAttributeModelsCacheKey()
        {
            return "AllAttributeModelsWithBasicProperties";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllBaseAttributeModelsAllPropertiesCacheKey()
        {
            return "AllAttributeModelsWithAllProperties";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelBaseAttributeModelsCacheKey()
        {
            return "AllAttributeModelsWithBasicProperties_CachedDataModel";
        }

        /// <summary>
        /// Gets the attribute model dependency cache keys for all attributes
        /// </summary>
        /// <returns></returns>
        public static String GetAttributeModelDependenciesCacheKey()
        {
            return "AllAttributeModelDependencies";
        }

        /// <summary>
        /// Gets the attribute model dependency cache key based on the ApplicationContext and attribute id
        /// </summary>
        /// <returns></returns>
        public static String GetAttributeModelDependencyCacheKey(ApplicationContext applicationContext, Int32 attributeId)
        {
            if (applicationContext == null)
            {
                throw new ArgumentNullException(String.Format("GetAttributeModelDependencyCacheKey : ApplicationContext is null : Id - {0}", attributeId.ToString()));
            }

            return String.Format("ORG{0}CON{1}CAT{2}ET{3}AM{4}", applicationContext.OrganizationId, applicationContext.ContainerId, applicationContext.CategoryId,
                applicationContext.EntityTypeId, attributeId);
        }


        #endregion

        #region Lookup Data cache keys

        /// <summary>
        /// Get the lookup data cache key based on attribute Id and locale
        /// </summary>
        /// <param name="attributeId">Indicates the attribute Id</param>
        /// <param name="locale">Indicates the locale</param>
        /// <returns>Reruns the cache key</returns>
        public static String GetLookupDataCacheKey(Int32 attributeId, LocaleEnum locale)
        {
            return String.Format("LK.A{0}_L{1}", attributeId, (Int32)locale);
        }

        /// <summary>
        /// Get the lookup data cache key based on lookup table name and locale
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <returns>Reruns the cache key</returns>
        public static String GetLookupDataCacheKey(String lookupTableName, LocaleEnum locale)
        {
            return String.Format("LK.NM[{0}]_L{1}", lookupTableName.ToLower(), (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupTableName"></param>
        /// <param name="locale"></param>
        /// <param name="uniqueIdentifier"></param>
        /// <returns></returns>
        /// Note: This is an OBSELETE key need to remove this soon.
        public static String GetLookupDataCacheKey(String lookupTableName, LocaleEnum locale, String uniqueIdentifier)
        {
            return String.Format("LK.NM[{0}]_L{1}_UI{2}", lookupTableName, (Int32)locale, uniqueIdentifier);
        }

        /// <summary>
        ///  Gets the lookup object cache key for web server list based on lookup table name 
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <returns>Reruns the cache key</returns>
        public static String GetDirtyLookupObjectWebServerListCacheKey(String lookupTableName)
        {
            return String.Format("WS.LK.L{0}", lookupTableName.ToLower());
        }

        #endregion

        #region Entity Map Cache Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="externalId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static String GetEntityMapCacheKey(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId)
        {
            return String.Format("ENMAP.SYS{0}_EXT{1}_CON{2}_ET{3}_CAT{4}", systemId, externalId, containerId, entityTypeId, categoryId);
        }

        #endregion

        #region Organization Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetOrganizationCacheKey(Int32 organizationId, LocaleEnum locale)
        {
            return String.Format("ORG.O{0}_L{1}", organizationId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllOrganizationCacheKey()
        {
            return String.Format("ORG");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllOrganizationsWithAttributesCacheKey()
        {
            return String.Format("ORG_A");
        }

        #endregion

        #region Entity Type Keys

        /// <summary>
        /// Method that will return GetAllEntityTypesCacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetAllEntityTypesCacheKey()
        {
            return String.Format("ETYPE_{0}", "ALL");
        }

        /// <summary>
        /// Method that will return GetAllContainerEntityTypeMappingsCacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetAllContainerEntityTypeMappingsCacheKey()
        {
            return String.Format("CONTAINER_ETYPE_MAPPING_{0}", "ALL");
        }

        /// <summary>
        /// Method that will return GetAllCachedDataModelEntityTypesCacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelEntityTypesCacheKey()
        {
            return String.Format("ETYPE_CachedDataModel_{0}", "ALL");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityTypeCacheKey(Int32 entityType, LocaleEnum locale)
        {
            return String.Format("ETYPE.E{0}_L{1}", entityType, (Int32)locale);
        }

        #endregion

        #region Relationship Type Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipType"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetRelationShipTypeCacheKey(Int32 relationshipType, LocaleEnum locale)
        {
            return String.Format("RTYPE.R{0}_L{1}", relationshipType, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelRelationshipTypesCacheKey()
        {
            return String.Format("RTYPE_CachedDataModel_{0}", "ALL");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllRelationshipTypesCacheKey()
        {
            return "RS_RelationshipTypes";
        }

        #endregion

        #region Container Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetContainerCacheKey(Int32 containerId, LocaleEnum locale)
        {
            return String.Format("CON.C{0}_L{1}", containerId, (Int32)locale);
        }

        /// <summary>
        /// Method that will return GetEntityExportProfileCacheKey based on Locale and containerId
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetEntityExportProfileCacheKey(Int32 containerId, LocaleEnum locale)
        {
            return String.Format("EEP.P{0}_L{1}", containerId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllContainersCacheKey()
        {
            return String.Format("CON");
        }

        /// <summary>
        /// Method that will return GetAllCachedDataModelContainersCacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelContainersCacheKey()
        {
            return "CON_CachedDataModel";
        }

        /// <summary>
        /// Returns the cache key for all organizations for cached data model
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelOrganizationsCacheKey()
        {
            return "ORG_CachedDataModel";
        }

        /// <summary>
        /// Returns the cache key for all hierarchies for cached data model
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelHierarchiesCacheKey()
        {
            return "HIER_CachedDataModel";
        }

        /// <summary>
        /// Returns the cache key for all UOM's for cached data model
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelUOMCacheKey()
        {
            return "UOM_CachedDataModel";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllEntityExportProfilesCacheKey()
        {
            return String.Format("EEP");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllExportProfilesCacheKey()
        {
            return String.Format("ALLExportProfiles");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllExportDataFormattersCacheKey()
        {
            return String.Format("ALLExportDataFormatters");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllContainersWithAttributesCacheKey()
        {
            return String.Format("CON_A");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllUomsCacheKey()
        {
            return String.Format("UOM");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetInheritancePathCacheKey()
        {
            return "RS_ContainerInheritancePath";
        }

        #endregion

        #region Category Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetCategoryCacheKey(Int32 categoryId, LocaleEnum locale)
        {
            return String.Format("CAT.C{0}_L{1}", categoryId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="catalogId"></param>
        /// <param name="dataLocale"></param>
        /// <param name="loginUser"></param>
        /// <param name="isCategoryPermissionsEnabled"></param>
        /// <returns></returns>
        public static String GetAllCategoriesCacheKey(Int32 hierarchyId, Int32 catalogId, LocaleEnum dataLocale, String loginUser, Boolean isCategoryPermissionsEnabled = false)
        {
            if (!isCategoryPermissionsEnabled)
                return String.Format("ALL_CAT_TAX{0}_CON{1}_L{2}", hierarchyId, catalogId, (Int32)dataLocale);
            else
                return String.Format("ALL_CAT_TAX{0}_CON{1}_L{2}_U{3}", hierarchyId, catalogId, (Int32)dataLocale, loginUser);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetCategoriesSearchFilterCacheKey(String loginUser, String alternativeTaxonomyId)
        {
            return String.Format("{0}_{1}_{2}", CacheKeyContants.CATEGORY_SEARCH_FILTER_KEY_PREFIX, loginUser, alternativeTaxonomyId);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllCachedDataModelCategoriesCacheKey()
        {
            return "CAT_CDM";
        }

        #endregion

        #region Security Token Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static String GetFormAuthenticationTicketCacheKey(String userName)
        {
            return "FormUserAuthenticationTicket_" + userName;
        }

        #endregion

        #region Get Object Type

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static Type GetObjectType(String cacheKey)
        {
            Type type = null;

            if (String.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentException("cacheKey is null");

            cacheKey = cacheKey.ToUpper();

            if (cacheKey.StartsWith("EN"))
            {
                type = GetEntityDataObjectType(cacheKey);
            }
            else if (cacheKey.StartsWith("AM"))
            {
                type = typeof(AttributeModel);
            }
            else if (cacheKey.StartsWith("LK"))
            {
                type = typeof(Lookup);
            }
            else if (cacheKey.StartsWith("ENMAP"))
            {
                type = typeof(EntityMap);
            }
            else if (cacheKey.StartsWith("ORG"))
            {
                type = typeof(Organization);
            }
            else if (cacheKey.StartsWith("ETYPE"))
            {
                type = typeof(EntityType);
            }
            else if (cacheKey.StartsWith("CON"))
            {
                type = typeof(Container);
            }
            else if (cacheKey.StartsWith("RTYPE"))
            {
                type = typeof(RelationshipType);
            }
            else if (cacheKey.StartsWith("CAT_L") || cacheKey.StartsWith("ALLCATEGORIES"))
            {
                type = typeof(CategoryCollection);
            }
            else if (cacheKey.StartsWith("ALLHIERARCHIES"))
            {
                type = typeof(Hierarchy);
            }
            return type;
        }

        private static Type GetEntityDataObjectType(String cacheKey)
        {
            Type type = null;

            if (cacheKey.Contains(".TEN") && cacheKey.EndsWith("BASE"))
            {
                type = typeof(Entity);
            }
            else if (cacheKey.Contains(".TCAT_") && cacheKey.EndsWith("BASE"))
            {
                type = typeof(Category);
            }
            else if (cacheKey.EndsWith("ELA"))
            {
                type = typeof(AttributeCollection);
            }
            else if (cacheKey.EndsWith("EIA"))
            {
                type = typeof(AttributeCollection);
            }
            else if (cacheKey.EndsWith("EHR"))
            {
                type = typeof(HierarchyRelationshipCollection);
            }
            else if (cacheKey.EndsWith("EER"))
            {
                type = typeof(ExtensionRelationshipCollection);
            }
            else if (cacheKey.EndsWith("ER"))
            {
                type = typeof(RelationshipCollection);
            }
            return type;
        }

        #endregion

        #region Get Object Id

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static Int64 GetObjectId(String cacheKey)
        {
            Int64 objectId = -1;
            Type type = GetObjectType(cacheKey);

            if (type != null)
            {
                if (type == typeof(AttributeModel))
                {
                    return FindObjectId(cacheKey, "_", ".A");
                }
                else if (type == typeof(Lookup))
                {
                    return FindObjectId(cacheKey, "_", ".A");
                }
                else if (type == typeof(Organization))
                {
                    return FindObjectId(cacheKey, "_", ".O");
                }
                else if (type == typeof(EntityType))
                {
                    return FindObjectId(cacheKey, "_", ".E");
                }
                else if (type == typeof(Container))
                {
                    return FindObjectId(cacheKey, "_", ".C");
                }
                else if (type == typeof(Category))
                {
                    return FindObjectId(cacheKey, "_", ".C");
                }
                else if (type == typeof(RelationshipType))
                {
                    return FindObjectId(cacheKey, "_", ".R");
                }
            }
            return objectId;
        }

        #endregion

        #region Locale Message Cache Keys

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetLocaleMessageCacheKey(LocaleEnum locale)
        {
            return String.Concat("UILocaleMessage_", locale);
        }

        #endregion

        #region Locale Cache Keys

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllLocalesCacheKey()
        {
            return "RS_Locales_All";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAvailableLocalesCacheKey()
        {
            return "RS_Locales";
        }

        /// <summary>
        /// Returns the Available LocaleEnums CacheKey.
        /// </summary>
        /// <returns>A String representing the cache key</returns>
        public static String GetAvailableLocaleEnumsCacheKey()
        {
            return "RS_LocaleEnums";
        }

        #endregion

        #region Menu Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="locale"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public static String GetMenuCacheKey(String loginUser, LocaleEnum locale, MDMCenterApplication application)
        {
            return String.Format("UM.A{0}.U{1}_L{2}", application, loginUser.ToLowerInvariant(), (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="locale"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public static String GetMenuObjectCacheKey(String loginUser, LocaleEnum locale, MDMCenterApplication application)
        {
            return String.Format("UMO.A{0}.U{1}_L{2}", application, loginUser.ToLowerInvariant(), (Int32)locale);
        }

        #endregion

        #region Security Principal Cache Key

        /// <summary>
        /// Generates the Cache key for SecurityPrincipal.
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public static String GetSecurityPrincipalCacheKey(String userLoginName)
        {
            return String.Format("SecurityPrincipal_{0}", userLoginName.ToLower());
        }

        #endregion

        #region Security Permission Definition Cache Key

        /// <summary>
        /// Security Permission Definition Cache Key
        /// </summary>
        /// <param name="roleId">Indicates Role Id</param>
        /// <returns>Returns the cache key for Security Permission Definition</returns>
        public static String GetSecurityPermissionDefinitionCacheKey(Int32 roleId)
        {
            return String.Format("SPD.R{0}", roleId);
        }

        /// <summary>
        /// User Permissions Cache Key
        /// </summary>
        /// <param name="userId">Indicates User Id</param>
        /// <returns>Returns the cache key for User Permissions </returns>
        public static String GetUserPermissionsCacheKey(Int32 userId)
        {
            return String.Format("RS_DataSecurity_UserId:{0}", userId);
        }

        #endregion

        #region Security Role Cache Key

        /// <summary>
        /// User Roles Cache Key
        /// </summary>
        /// <param name="userLoginName">Login Name of the user</param>
        /// <returns>Returns the cache key for User Roles</returns>
        public static String GetUserRolesCacheKey(String userLoginName)
        {
            return String.Format("UR_U:{0}", userLoginName.ToLowerInvariant());
        }

        #endregion

        #region Role Permissions Changed Key

        /// <summary>
        /// Get user role permissions changed key
        /// </summary>
        /// <param name="userLoginId">Indicates the Id of the logged in user</param>
        /// <returns>RolePermissionsChangedKey</returns>
        public static String GetUserRolePermissionsChangedKey(Int64 userLoginId)
        {
            return String.Format("{0}_{1}", CacheKeyContants.USERROLE_PERMISSIONS_CHANGED_KEY, userLoginId);
        }

        /// <summary>
        /// Get user role permissions cache key
        /// </summary>
        /// <param name="userLoginId">Indicates the Id of the logged in user</param>
        /// <returns>RolePermissionsCacheKey</returns>
        public static String GetUserRolePermissionsCacheKey(Int64 userLoginId)
        {
            return String.Format("{0}_{1}", userLoginId, CacheKeyContants.USERROLE_PERMISSIONS_CACHE_KEY_SUFFIX);
        }

        #endregion Role Permissions Changed Key

        #region Metadata page config

        /// <summary>
        /// Get MetaData Page Config Context Key
        /// </summary>
        public static String GetMetaDataPageConfigCacheKey(Int32 roleId, Int32 userId, Int32 organizationId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            String cacheKey = String.Empty;
            Int32 source = (Int32)EventSource.MetaData;
            Int32 subscriber = (Int32)EventSubscriber.MetaDataPage;

            cacheKey = String.Format("MetaDataPageCofig_EventSource:{0}_EventSubscriber:{1}_Role:{2}_User:{3}_Org:{4}_Container:{5}_EntityType:{6}_Locale:{7}",
                                             source, subscriber, roleId, userId, organizationId, containerId, entityTypeId, locale.ToString());

            return cacheKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="containerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static String GetEntityHistoryExcludeListConfigCacheKey(Int32 roleId, Int32 userId, Int32 organizationId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, Int64 entityId)
        {
            String cacheKey = String.Empty;
            Int32 source = (Int32)EventSource.MetaData;
            Int32 subscriber = (Int32)EventSubscriber.MetaDataPage;

            cacheKey = String.Format("EntityHistoryExcludeListConfig_EventSource:{0}_EventSubscriber:{1}_Role:{2}_User:{3}_Org:{4}_Container:{5}_Category:{6}_EntityType:{7}_EntityId:{8}",
                                             source, subscriber, roleId, userId, organizationId, containerId, categoryId, entityTypeId, entityId);

            return cacheKey;
        }

        #endregion

        #region Vendor User

        /// <summary>
        /// Gets the key for the vendor user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static String GetVendorCacheKeyForUser(Int32 userId)
        {
            return String.Format("VENDOR.USER{0}", userId.ToString());
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// Finds the requested string pattern. The keyes are of the form YYYY.KNNN_
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="splitCharacter"></param>
        /// <param name="keyCharacter"></param>
        /// <returns></returns>
        private static Int64 FindObjectId(String inputString, String splitCharacter, String keyCharacter)
        {
            String stringValue = String.Empty;
            Int64 objectId = -1;

            if (String.IsNullOrEmpty(splitCharacter))
                return objectId;

            if (String.IsNullOrEmpty(keyCharacter))
                return objectId;

            Int32 keyCharacterLength = keyCharacter.Length;

            Int32 firstSplit = inputString.IndexOf(splitCharacter);
            Int32 firstKey = inputString.IndexOf(keyCharacter);

            if (firstSplit > 0 && firstKey > 0)
            {
                stringValue = inputString.Substring(firstKey + keyCharacterLength, firstSplit - firstKey - keyCharacterLength);

                objectId = Convert.ToInt64(stringValue);
            }
            return objectId;
        }

        #endregion

        #region EntityHierarchy Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static String GetEntityHierarchyMatrixCacheKey(Int64 entityId)
        {
            return String.Format("EntityHierarchyMatrix_EntityId_{0}", entityId);
        }

        #endregion

        #region Entity Variant Cache Key

        /// <summary>
        /// Get the cache key for entity variant definitions
        /// </summary>
        /// <returns>Returns the cache key for entity variant definitions</returns>
        public static String GetAllEntityVariantDefinitions()
        {
            return "EVDs";
        }

        #endregion

        #region Entity Variant Definition Mapping Cache Key

        /// <summary>
        /// Get the cache key for entity variant definition mappings
        /// </summary>
        /// <returns>Returns the cache key for entity variant definition mappings</returns>
        public static String GetAllEntityVariantDefinitionMappings()
        {
            return "EVD_Mappings";
        }

        #endregion Entity Variant Definition Mapping Cache Key

        #region Misc Cache Keys

        /// <summary>
        /// Returns the appconfig cache key.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static String GetAppConfigCacheKey(String cacheKey)
        {
            return String.Format("{0}{1}", CacheKeyContants.APP_CONFIG_CACHE_KEY_PREFIX, cacheKey);
        }

        /// <summary>
        /// Returns the MDM Feature cache key.
        /// </summary>
        /// <param name="cacheKey">The MDM Feature cache key.</param>
        /// <returns>MDM Feature cache key with prefix MDMFEATURE_CONFIG.</returns>
        public static String GetMDMFeatureConfigCacheKey(String cacheKey)
        {
            return String.Format("{0}{1}", CacheKeyContants.MDMFEATURE_CONFIG_CACHE_KEY_PREFIX, cacheKey);
        }

        /// <summary>
        /// Generates the prefix based on the input for mdmfeatureconfig key which will be used to store values in cache
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>    
        /// <returns>Returns the key to set MDM Feature configurations data in cache</returns>
        public static String GetMDMFeatureConfigKey(MDMCenterApplication application, String moduleName, String version)
        {
            return String.Format("{0}_{1}_{2}", application, moduleName, version);
        }

        /// <summary>
        /// Returns the appconfig cache key.
        /// </summary>
        /// <returns></returns>
        public static String GetMDMTraceConfigCacheKey()
        {
            return "MDMTraceConfigCacheKey";
        }

        /// <summary>
        /// Returns raw appconfigs (requested using EventSource and EventSubscriber pair) list cache key prefix
        /// </summary>
        /// <returns></returns>
        public static String GetApplicationConfigurationItemsByEventSourceAndSubscriberCacheKeyPrefix()
        {
            return "RawAppCfgItemsList_ESrc:";
        }

        /// <summary>
        /// Returns raw appconfigs list cache key
        /// </summary>
        /// <returns></returns>
        public static String GetApplicationConfigurationItemsListCacheKey(EventSource eventSource, EventSubscriber? eventSubscriber)
        {
            return GetApplicationConfigurationItemsByEventSourceAndSubscriberCacheKeyPrefix() + String.Format("{0}_ESub:{1}", (Int32)eventSource, eventSubscriber.HasValue ? ((Int32)eventSubscriber).ToString() : "null");
        }

        /// <summary>
        /// Returns Relationship Denorm Processing Settings CacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetRelationshipDenormProcessingSettingsCacheKey()
        {
            return "RelationshipDenormProcessingSettings";
        }

        /// <summary>
        /// Returns Role Mappings CacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetRoleMappingsCacheKey()
        {
            return "RoleMappings";
        }

        /// <summary>
        /// Returns Claim Types Mappings CacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetClaimTypesMappingCacheKey()
        {
            return "ClaimTypesMapping";
        }

        /// <summary>
		/// Returns Fatal Exception CacheKey
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static String GetFatalExceptionCacheKey(String userName)
        {
            return String.Format("FatalExceptionDetail_{0}", userName);
        }

        /// <summary>
		/// Returns Expression Data CacheKey
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static String GetExpressionDataCacheKey(String expression)
		{
			return String.Format("EP.NM[{0}]", expression.GetHashCode());
		}

        /// <summary>
        /// Gets the user session data locale cache key.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Cache key for the session data locale of the user</returns>
        public static String GetUserSessionDataLocaleCacheKey(String userName)
        {
            return String.Format("{0}:{1}", CacheKeyContants.SESSION_DATA_LOCALE_FOR_USER_PREFIX, userName.ToLowerInvariant());
        }

        #endregion

        #region Attribute Mapping Cache Key

        /// <summary>
        /// Generates Attribute Model Cache Key
        /// </summary>
        /// <param name="attributeModelType"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="isKeyForCompleteDetails"></param>
        /// <returns></returns>
        public static String GetAttributeModelMappingCacheKey(AttributeModelType attributeModelType, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Boolean isKeyForCompleteDetails = true)
        {
            String cacheKey = String.Empty;

            if (attributeModelType == AttributeModelType.Common)
            {
                cacheKey = String.Format("AM.TYPE{0}_CON{1}_ET{2}_{3}", "CA", containerId, entityTypeId, (isKeyForCompleteDetails ? "Complete" : "Partial"));
            }
            else if (attributeModelType == AttributeModelType.Category)
            {
                cacheKey = String.Format("AM.TYPE{0}_CAT{1}_{2}", "TA", categoryId, (isKeyForCompleteDetails ? "Complete" : "Partial"));
            }
            else if (attributeModelType == AttributeModelType.Relationship)
            {
                cacheKey = String.Format("AM.TYPE{0}_CON{1}_RT{2}_{3}", "RA", containerId, relationshipTypeId, (isKeyForCompleteDetails ? "Complete" : "Partial"));
            }
            else if (attributeModelType == AttributeModelType.System || attributeModelType == AttributeModelType.MetaDataAttribute)
            {
                cacheKey = String.Format("AM.TYPE{0}", "SYS_MA");
            }

            return cacheKey;
        }

        /// <summary>
        /// Generates All Attribute Model Cache Key
        /// </summary>
        /// <param name="attributeModelType"></param>
        /// <returns></returns>
        public static String GetAllAttributeModelMappingCacheKey(AttributeModelType attributeModelType)
        {
            String cacheKey = String.Empty;

            if (attributeModelType == AttributeModelType.Common)
            {
                cacheKey = String.Format("AM.TYPE{0}_ALL", "CA");
            }
            else if (attributeModelType == AttributeModelType.Category)
            {
                cacheKey = String.Format("AM.TYPE{0}_ALL", "TA");
            }
            else if (attributeModelType == AttributeModelType.Relationship)
            {
                cacheKey = String.Format("AM.TYPE{0}_ALL", "RA");
            }

            return cacheKey;
        }

        #endregion Attribute Mapping Cache Key

        #region Hierarchy Cache Key

        /// <summary>
        /// Get cache key for storing all the taxonomies
        /// </summary>
        /// <returns></returns>
        public static String GetAllHierarchiesCacheKey()
        {
            return "AllHierarchies";
        }

        #endregion hierarchy Cache Key

        #region Category - Attribute Mapping Cache Key

        /// <summary>
        /// Gets the cache key for all category attribute mappings.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Cache key for the category - attribute mapping</returns>
        public static String GetCategoryAttributeMappingCacheKey(Int64 categoryId)
        {
            return String.Format("CategoryAttributeMappingCacheKey_{0}", categoryId);
        }

        #endregion Category - Attribute Mapping Cache Key

        #region DataQualityIndicator Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllDataQualityIndicatorCacheKey()
        {
            return String.Format("AllDataQualityIndicators");
        }

        #endregion

        #region ValidationProfile Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllValidationProfilesCacheKey()
        {
            return String.Format("AllValidationProfiles");
        }

        #endregion

        #region NormalizationProfile Cache Key

        /// <summary>
        /// Generates AllNormalizationProfiles cache record key
        /// </summary>
        /// <returns>Returns AllNormalizationProfiles cache record key</returns>
        public static String GetAllNormalizationProfilesCacheKey()
        {
            return String.Format("AllNormalizationProfiles");
        }

        #endregion

        #region MatchingProfile Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllMatchingProfilesCacheKey()
        {
            return String.Format("AllMatchingProfiles");
        }

        #endregion

        #region MatchingRule Cache Key

        /// <summary>
        /// Get cache key of all matching rules
        /// </summary>
        /// <returns>Returns cache key of all matching rules</returns>
        public static String GetAllMatchingRulesCacheKey()
        {
            return String.Format("AllMatchingRules");
        }

        #endregion

        #region DQMRule Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns></returns>
        public static String GetDQMRulesByJobTypeCacheKey(DQMJobType jobType)
        {
            return String.Format("AllDQMRules_{0}", jobType.ToString());
        }

        #endregion

        #region DQMConfig Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns></returns>
        public static String GetDQMConfigsByJobTypeCacheKey(DQMJobType jobType)
        {
            return String.Format("AllDQMConfigs_{0}", jobType.ToString());
        }

        #endregion

        #region DQMConfig Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetDQMProfileGroupProfilesByJobTypeCacheKey()
        {
            return String.Format("AllDQMProfileGroupProfiles");
        }

        #endregion

        #region Match Store Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllMatchStoresCacheKey()
        {
            return String.Format("AllMatchStores");
        }

        #endregion

        #region SurvivorshipRulesets Cache Key

        /// <summary>
        /// Generates AllSurvivorshipRulesets cache record key
        /// </summary>
        /// <returns>Returns AllSurvivorshipRulesets cache record key</returns>
        public static String GetAllSurvivorshipRulesetsCacheKey()
        {
            return String.Format("AllSurvivorshipRulesets");
        }

        #endregion

        #region MergePlanningProfiles Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllMatchReviewProfilesCacheKey()
        {
            return String.Format("AllMatchReviewProfiles");
        }

        #endregion

        #region MergeResults Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetMergeResultsCacheKey()
        {
            return String.Format("MergeResults");
        }

        #endregion

        #region MergingProfile Cache Key

        /// <summary>
        /// Cache Key for Merging Profiles
        /// </summary>
        /// <returns></returns>
        public static String GetAllMergingProfilesCacheKey()
        {
            return String.Format("AllMergingProfiles");
        }

        #endregion

        #region FilterCriteriaSettings Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllFilterCriteriaSettingsCacheKey()
        {
            return String.Format("AllFilterCriteriaSettings");
        }

        #endregion

        #region GetAllDataQualityClasses Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllDataQualityClassesCacheKey()
        {
            return String.Format("AllDataQualityClasses");
        }

        #endregion

        #region Word List Cache Key

        /// <summary>
        /// Returns WordLists' cache key prefix
        /// </summary>
        /// <returns></returns>
        public static String GetWordListsCacheKeyPrefix()
        {
            return "WordLists_Prefix:";
        }

        /// <summary>
        /// Get WordLists cache key for all wordlists
        /// </summary>
        /// <returns></returns>
        public static String GetAllWordListsCacheKey()
        {
            return String.Concat(GetWordListsCacheKeyPrefix(), "All");
        }

        /// <summary>
        /// Get WordLists cache key for provided key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetWordListsCacheKey(String key)
        {
            return String.Concat(GetWordListsCacheKeyPrefix(), key);
        }

        #endregion

        #region ConnectorProfiles Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllConnectorProfilesCacheKey()
        {
            return "AllConnectorProfiles";
        }

        #endregion

        #region MDMObjectType Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllMDMObjectTypesCacheKey()
        {
            return "AllMDMObjectType";
        }

        #endregion

        #region IntegrationItemDimensionType Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllIntegrationItemDimensionTypesCacheKey()
        {
            return "AllIntegrationItemDimensionType";
        }

        #endregion

        #region AllGetAllIntegrationMessageTypes Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllIntegrationMessageTypesCacheKey()
        {
            return "AllIntegrationMessageTypes";
        }

        #endregion

        #region Source Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetAllSourcesCacheKey()
        {
            return String.Format("AllSources");
        }

        #endregion

        #region Get All ServerInfo

        /// <summary>
        /// Generates server name and id Cache key
        /// </summary>
        /// <returns></returns>
        public static String GetServerInfoCacheKey()
        {
            return String.Format("ServerInfo");
        }

        #endregion

        #region SourceValue Cache Key

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String GetSourcesValueCacheKey(Int64 entityId, LocaleEnum locale)
        {
            return String.Format("SRC.E_{0}.L_{1}", entityId, locale);
        }

        #endregion

        #region Diagnostics Cache Key
        /// <summary>
        /// GetRuntimeDiagnosticsJobIdCacheKey
        /// </summary>
        /// <returns></returns>
        public static String GetRuntimeDiagnosticsJobIdCacheKey()
        {
            return String.Format("ImportExportRuntimeDiagnosticsJobId");
        }
        
        #endregion

        #region ContainerRelationshipTypeEntityTypeMapping Cache key

        /// <summary>
        /// Get all ContainerRelationshipTypeEntityTypeMapping cache key.
        /// </summary>
        /// <returns></returns>
        public static String GetContainerRelationshipTypeEntityTypeMappingCacheKey()
        {
            return "AllContainerRelationshipTypeEntityTypeMappings";
        }

        #endregion ContainerRelationshipTypeEntityTypeMapping Cache key

        #region DataModel Exclusion Cache Key

        /// <summary>
        /// Generates DataModel Exclusion Cache Key
        /// </summary>
        /// <returns>DataModel Exclusion Cache Key</returns>
        public static String GetDataModelServiceExclusionContextCacheKey()
        {
            return "ALLDataModelExclusionContext";
        }

        #endregion DataModel Exclusion Cache Key

        #region MDMEvents Cache Key

        /// <summary>
        /// Gets the MDMEvent handler cache key name
        /// </summary>
        /// <param name="eventHandlerId">Indicates the event handler Id</param>
        /// <returns>Returns the MDMEvent handlers cache key name</returns>
        public static String GetMDMEventHandlerCacheKey(Int32 eventHandlerId)
        {
            return String.Format("{0}{1}", CacheKeyContants.MDM_EVENT_HANDLER_CACHE_KEY_PREFIX, eventHandlerId);
        }

        /// <summary>
        /// Gets the MDMEvent Information cache key name
        /// </summary>
        /// <returns>Returns the MDMEvent Information cache key name</returns>
        public static String GetMDMEventInfoCacheKey()
        {
            return "MDMEventInformation";
        }

        /// <summary>
        /// Gets the MDM web event handlers cache key.
        /// </summary>
        /// <returns></returns>
        public static String GetMDMWebEventHandlersCacheKey()
        {
            return CacheKeyContants.MDM_WEB_EVENT_HANDLERS_CACHE_KEY;
        }

        #endregion MDMEvents Cache Key

        #region Complex attribute cache key

        /// <summary>
        /// Gets the complex attribute cache key.
        /// </summary>
        /// <returns>Returns the complex attribute cache key name</returns>
        public static String GetComplexAttributeCacheKey(String complexAttributeName, LocaleEnum locale)
        {
            return String.Format("{0}_{1}", complexAttributeName, locale.ToString());
        }

        #endregion Complex attribute cache key

        #region ApplicationContext Cache Key

        /// <summary>
        /// Gets the applicationcontext cache key.
        /// </summary>
        /// <returns>Returns the applicationcontext cache key name</returns>
        public static String GetApplicationContextsCacheKey()
        {
            return "APPLICATION_CONTEXTS";
        }

        #endregion ApplicationContext Cache Key

        #region Business Rule Management Cache keys

        /// <summary>
        /// Gets the MDMRules cache key name
        /// </summary>
        /// <returns>Returns the MDMRules cache key name</returns>
        public static String GetMDMRulesCacheKey()
        {
            return "MDM_RULES";
        }

        /// <summary>
        /// Gets the MDMRule maps cache key name
        /// </summary>
        /// <returns>Returns the MDMRule maps cache key name</returns>
        public static String GetMDMRuleMapsCacheKey()
        {
            return "MDMRuleMaps";
        }

        /// <summary>
        /// Gets the MDMRuleKeyword's all Groups cache key name
        /// </summary>
        /// <returns>Returns the MDMRuleKeyword's all Groups cache key name</returns>
        public static String GetMDMRuleKeywordAllGroupsCacheKey()
        {
            return "MDMRuleKeywordGroups";
        }

        /// <summary>
        /// Gets the MDMRule all Keywords cache key name
        /// </summary>
        /// <returns>Returns the MDMRule all Keywords cache key name</returns>
        public static String GetMDMRuleAllKeywordsCacheKey()
        {
            return "MDMRuleKeywords";
        }

        /// <summary>
        /// Gets the MDMRule Keywords (related to specified group) cache key name
        /// </summary>
        /// <param name="mdmRuleKeywordGroupId">Indicates the MDMRule Keyword Group Id</param>
        /// <returns>Returns the MDMRule Keywords (related to specified group) cache key name</returns>
        public static String GetMDMRuleKeywordsForGroupCacheKey(Int32 mdmRuleKeywordGroupId)
        {
            return String.Format("MDMRuleKeywords_GrId:{0}", mdmRuleKeywordGroupId);
        }

        /// <summary>
        /// Gets the WorkflowInfo cache key name
        /// </summary>
        /// <returns>Returns the WorkflowInfo cache key name</returns>
        public static String GetWorkflowInfoCacheKey()
        {
            return "WorkflowInfo";
        }

        #endregion Business Rule Management Cache keys

        #region SelectorConfiguration Cache Key

        /// <summary>
        /// User SelectorConfiguration Cache Key
        /// </summary>
        /// <param name="userId">Indicates User Id</param>
        /// <returns>Returns the cache key for User SelectorConfiguration </returns>
        public static String GetUserSelectorConfigurationCacheKey(Int32 userId)
        {
            return String.Format("SelectorConfiguration_UserId:{0}", userId);
        }

        #endregion

        /// <summary>
        /// Gets the Workflow Cache Key Name
        /// </summary>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <returns>Returns the Worflow cache key name</returns>
        public static String GetWorkflowCacheKey(String workflowName)
        {
            return String.Format("RS_Workflow_{0}", workflowName);
        }

        /// <summary>
        /// Gets the Workflow Version Cache Key Name
        /// </summary>
        /// <param name="workflowVersionId">Indicates the workflow version id</param>
        /// <returns>Returns the Worflow version cache key name</returns>
        public static String GetWorkflowVersionCacheKey(Int32 workflowVersionId)
        {
            return String.Format("RS_Workflow_Version_{0}", workflowVersionId);
        }

        /// <summary>
        /// Gets the Workflow Version Cache Key Name
        /// </summary>
        /// <param name="workflowVersionId">Indicates the workflow version id</param>
        /// <returns>Returns the Worflow version cache key name</returns>
        public static String GetWorkflowVersionWithTrackingProfileCacheKey(Int32 workflowVersionId)
        {
            return String.Format("RS_Workflow_Version_WithTrackingProfile_{0}", workflowVersionId);
        }
    }
}