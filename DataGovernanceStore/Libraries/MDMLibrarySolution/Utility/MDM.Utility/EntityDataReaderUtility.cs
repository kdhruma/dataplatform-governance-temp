using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// This class contains helper methods for entity data reader utility.
    /// </summary>
    public static class EntityDataReaderUtility
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method reads data reader which provides entity data in specified format and creates entity collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static EntityCollection ReadEntities(SqlDataReader reader, MDMCenterModules module)
        {
            return ReadEntitiesMain(reader, module, true, true, true, true, null, true, readEntityBusinessConditions: true);
        }

        /// <summary>
        /// Reads the data reader which provides entity data in specified format and creates entity collection with specified set of attribute components.
        /// </summary>
        /// <param name="reader">Sql reader holding entity data stream</param>
        /// <param name="module"></param>
        /// <param name="readAttributes">Indicate if we need to read attributes</param>
        /// <param name="readRelationships">Indicate if we need to read relationships</param>
        /// <param name="readHiearchyRelationships">Indicate if we need to read hierarchy relationships</param>
        /// <param name="readExtensionRelationships">Indicate if we need to read extension relationships</param>
        /// <param name="attributeModelsForHierarchy">Provide list of attribute models based on the entity hierarchy</param>
        /// <param name="readComplexAttributes">Indicate if we need to read complex attributes</param>
        /// <param name="readBusinessConditions">Indicates if we need to read business conditions</param>
        /// <returns>An entity collection</returns>
        public static EntityCollection ReadEntitiesWithHierarchy(SqlDataReader reader, MDMCenterModules module, Boolean readAttributes, Boolean readRelationships,
            Boolean readHiearchyRelationships, Boolean readExtensionRelationships, IDictionary<Int32, AttributeModelCollection> attributeModelsForHierarchy,
            Boolean readComplexAttributes = true, Boolean readBusinessConditions = false)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            ContextualObjectCollection<AttributeModelCollection> contextualAttributeModels = CreateContextualAttributeModelsForHierarchy(attributeModelsForHierarchy);

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed populating the contextualAttributeModels from attributeModels");
            }

            // Read the SqlDataReader to memory using RSDataReader.
            var dataReader = new RSDataReader(reader);

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed reading the SqlDataReader to memory using RSDataReader");
            }

            EntityCollection entities = ReadEntitiesMain(dataReader, module, readAttributes, readRelationships, readHiearchyRelationships, readExtensionRelationships,
                contextualAttributeModels, readComplexAttributes, true, readBusinessConditions);

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return entities;
        }

        /// <summary>
        /// This method reads data reader which provides entity data in specified format and creates entity collection with specified set of attribute components.
        /// </summary>
        /// <param name="reader">Sql reader holding entity data stream</param>
        /// <param name="module"></param>
        /// <param name="readAttributes">Indicate if we need to read attributes</param>
        /// <param name="readRelationships">Indicate if we need to read relationships</param>
        /// <param name="readHiearchyRelationships"></param>
        /// <param name="readExtensionRelationships"></param>
        /// <param name="attributeModels">Provide list of attribute models to be used for creating object, else system will try to fetch it from the CachedDataModel</param>
        /// <param name="readComplexAttributes"></param>
        /// <param name="readEntityBusinessConditions">Indicates if we need to read business conditions</param>
        /// <returns></returns>
        public static EntityCollection ReadEntities(SqlDataReader reader, MDMCenterModules module, Boolean readAttributes, Boolean readRelationships, Boolean readHiearchyRelationships, Boolean readExtensionRelationships, AttributeModelCollection attributeModels, Boolean readComplexAttributes = true, Boolean readEntityBusinessConditions = false)
        {
            return ReadEntitiesMain(reader, module, readAttributes, readRelationships, readHiearchyRelationships, readExtensionRelationships,
                new ContextualObjectCollection<AttributeModelCollection>(attributeModels), readComplexAttributes, readEntityBusinessConditions: readEntityBusinessConditions);
        }

        /// <summary>
        /// Reads data reader which providing attributes data in specified format
        /// </summary>
        /// <param name="reader">Indicates sql reader holding attribut data stream</param>
        /// <param name="entityCollection">Indicates list of entities to be used for creating object</param>
        /// <param name="attributeModels">Indicates list of attribute models to be used for creating object</param>
        /// <param name="module">Indicates name of the module like entity</param>
        /// <param name="readComplexAttributes">Indicates if we need to read complex attributes or not</param>
        public static void ReadAttributes(SqlDataReader reader, EntityCollection entityCollection, AttributeModelCollection attributeModels, MDMCenterModules module, Boolean readComplexAttributes = true)
        {
            if (reader != null)
            {
                var complexAttributeInstanceDictionary = new Dictionary<String, Collection<AttributeAndAttributeModelPair>>();

                ReadAttributes(reader, entityCollection, new ContextualObjectCollection<AttributeModelCollection>(attributeModels), module, false, complexAttributeInstanceDictionary);

                if (readComplexAttributes)
                {
                    ReadComplexAttributes(reader, entityCollection, module);
                }
            }
        }

        /// <summary>
        /// This method populates relationship and relationship attributes dictionary only if update cache flag is set to True.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        /// <param name="attributeModels"></param>
        /// <param name="module"></param>
        /// <param name="relationshipsDictionary"></param>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="updateCache"></param>
        /// <param name="relationshipContext"></param>
        /// <returns></returns>
        public static RelationshipCollection ReadRelationships(SqlDataReader reader, EntityCollection entityCollection, AttributeModelCollection attributeModels, MDMCenterModules module, Dictionary<Int64, RelationshipCollection> relationshipsDictionary = null, Dictionary<String, AttributeCollection> relationshipsAttributesDictionary = null, Boolean updateCache = false, RelationshipContext relationshipContext = null)
        {
            Boolean isComplexAttributesReadRequired = false;
            return ReadRelationships(reader, entityCollection, attributeModels, module, ref isComplexAttributesReadRequired, relationshipsDictionary, relationshipsAttributesDictionary, updateCache, relationshipContext);
        }

        /// <summary>
        /// extracting the entity and attribute properties and returned as entity table and attributetable
        /// </summary>
        /// <param name="entityCollection">entity collection</param>
        /// <param name="entityMetaData">entity metadata</param>
        /// <param name="attributeMetaData">attribute metadata</param>
        /// <param name="entityTable">Entity table-entity properties extracted and placed in this table</param>
        /// <param name="attributeTable">Attribute table-attribute properties are extracted and placed in this table </param>
        /// <param name="mdmCenterModule">name of the module</param>
        public static void CreateEntityTable(EntityCollection entityCollection, SqlMetaData[] entityMetaData, SqlMetaData[] attributeMetaData, out List<SqlDataRecord> entityTable, out List<SqlDataRecord> attributeTable, MDMCenterModules mdmCenterModule)
        {
            entityTable = new List<SqlDataRecord>();
            attributeTable = new List<SqlDataRecord>();
            Int32 updatedAttributes = 0;
            foreach (Entity entity in entityCollection)
            {
                //1. For Core entity, don't send it to DB if action = read or ignore.
                //2. For Denorm entity, we can't change entity.Action = Update until something actually has changed for Entity (e.g., create,reclassify)
                //      else it will start recording history for entity update and export will have problem.
                if (mdmCenterModule != MDMCenterModules.Denorm && (entity.Action == ObjectAction.Ignore || entity.Action == ObjectAction.Read))
                {
                    continue;
                }

                String entityAction = entity.Action.ToString();

                SqlDataRecord entityRecord = new SqlDataRecord(entityMetaData);
                entityRecord.SetValue(0, entity.Id);
                entityRecord.SetValue(1, entity.Name.Trim());
                entityRecord.SetValue(2, entity.LongName.Trim());
                entityRecord.SetValue(3, entity.Path.Trim());
                entityRecord.SetValue(4, entity.ContainerId);
                entityRecord.SetValue(5, entity.CategoryId);
                entityRecord.SetValue(6, entity.ParentEntityId);
                entityRecord.SetValue(7, entity.ParentExtensionEntityId);
                entityRecord.SetValue(8, entity.EntityTypeId);
                entityRecord.SetValue(9, (Int32)entity.Locale);
                entityRecord.SetValue(10, entity.EntityGuid ?? Guid.NewGuid());
                entityRecord.SetValue(11, entityAction);
                entityRecord.SetValue(12, entity.UserName.Trim());
                entityRecord.SetValue(13, entity.ProgramName.Trim());

                if (entity.Action != ObjectAction.Delete)
                    CreateEntityAttributeTable(entity.Attributes, attributeTable, attributeMetaData, entity.Id, entity.Id, mdmCenterModule);

                //if attributes are unchanged, skip the DB call (attribute.count-updatedAttributes gives the current entity's attribute changes
                //if it is zero then the skip the DB update)
                if (entity.EntityTypeId != Constants.CATEGORY_ENTITYTYPE && entity.Action == ObjectAction.Update && attributeTable.Count - updatedAttributes == 0)
                    continue;

                updatedAttributes = attributeTable.Count;
                entityTable.Add(entityRecord);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (entityTable.Count == 0)
                entityTable = null;

            if (attributeTable.Count == 0)
                attributeTable = null;
        }

        /// <summary>
        /// Extracting the relationship and its attribute properties and returned as relationship table and attribute table
        /// </summary>
        /// <param name="entityCollection">Collection of entities having relationships</param>
        /// <param name="relationshipMetadata">Relationship Meta data</param>
        /// <param name="attributeMetaData">Attribute Meta data</param>
        /// <param name="relationshipTable">Relationship table - Relationship properties extracted and placed in this table</param>
        /// <param name="attributeTable">Attribute table - Attribute properties extracted and placed in this table</param>
        /// <param name="generateAttributesTable">Indicates whether to generate RelationshipAttributes table or not. For Relationship initial load, we process attributes separately. In that case this value will be false.</param>
        /// <param name="module">Name of the module</param>
        /// <param name="relationships">Name of the module</param>
        /// <param name="isRecursive">Name of the module</param>
        public static void CreateEntityRelationshipTable(EntityCollection entityCollection, SqlMetaData[] relationshipMetadata, SqlMetaData[] attributeMetaData, out List<SqlDataRecord> relationshipTable, out List<SqlDataRecord> attributeTable, Boolean generateAttributesTable, MDMCenterModules module, Dictionary<Int64,RelationshipCollection> relationships = null, Boolean isRecursive = true)
        {
            relationshipTable = new List<SqlDataRecord>();
            attributeTable = new List<SqlDataRecord>();

            foreach (Entity entity in entityCollection)
            {
                RelationshipCollection toBeProcessedRelationships = null;

                if (relationships == null || relationships.Count < 1)
                {
                    toBeProcessedRelationships = entity.Relationships;
                }
                else
                {
                    relationships.TryGetValue(entity.Id, out toBeProcessedRelationships);
                }

                if (toBeProcessedRelationships == null)
                {
                    continue;
                }

                CreateRelationshipTable(toBeProcessedRelationships, entity, relationshipMetadata, attributeMetaData, relationshipTable, attributeTable, generateAttributesTable, module, isRecursive);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (relationshipTable.Count == 0)
                relationshipTable = null;

            if (attributeTable.Count == 0)
                attributeTable = null;
}

        /// <summary>
        /// Populates table value parameters for entity context
        /// </summary>
        /// <param name="entityContext">entity context</param>
        /// <param name="entityContextMetadata">entityContext metadata</param>
        public static List<SqlDataRecord> CreateEntityContextTable(EntityContext entityContext, SqlMetaData[] entityContextMetadata)
        {
            List<SqlDataRecord> entiyContextTable = new List<SqlDataRecord>();

            SqlDataRecord entityContextRecord = new SqlDataRecord(entityContextMetadata);
            entityContextRecord.SetValue(0, (Int32)entityContext.ContainerId);
            entityContextRecord.SetValue(1, (Int64)entityContext.CategoryId);
            entityContextRecord.SetValue(2, (Int32)entityContext.EntityTypeId);
            entityContextRecord.SetValue(3, entityContext.Locale.ToString());
            entityContextRecord.SetValue(4, entityContext.LoadEntityProperties);
            entityContextRecord.SetValue(5, entityContext.LoadAttributes);
            entityContextRecord.SetValue(6, ValueTypeHelper.JoinCollection(entityContext.AttributeGroupIdList, ","));
            entityContextRecord.SetValue(7, ValueTypeHelper.JoinCollection(entityContext.AttributeIdList, ","));
            entityContextRecord.SetValue(8, entityContext.LoadRelationships);
            entityContextRecord.SetValue(9, ValueTypeHelper.JoinCollection(entityContext.RelationshipContext.RelationshipTypeIdList, ","));
            entityContextRecord.SetValue(10, entityContext.LoadCreationAttributes);
            entityContextRecord.SetValue(11, entityContext.LoadRequiredAttributes);
            entityContextRecord.SetValue(12, entityContext.AttributeModelType.ToString());
            entityContextRecord.SetValue(13, entityContext.IgnoreEntityStatus);
            entiyContextTable.Add(entityContextRecord);

            return entiyContextTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityParentDetails"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> CreateParentEntityTable(Entity entity, SqlMetaData[] entityParentDetails)
        {
            List<SqlDataRecord> parentEntityTable = new List<SqlDataRecord>();

            SqlDataRecord parentEntityRecord = new SqlDataRecord(entityParentDetails);
            parentEntityRecord.SetValue(0, (Int64)entity.Id);
            parentEntityRecord.SetValue(1, (Int64)entity.ParentEntityId);
            parentEntityRecord.SetValue(2, (Int64)entity.ParentExtensionEntityId);
            parentEntityRecord.SetValue(3, (Int32)entity.ContainerId);
            parentEntityRecord.SetValue(4, (Int32)entity.EntityTypeId);
            parentEntityTable.Add(parentEntityRecord);

            return parentEntityTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> CreateAttributeIdTable(Collection<Int32> attributeIds, SqlMetaData[] attributeIdList, Int64 referenceId)
        {
            List<SqlDataRecord> attributeIdTable = null;

            if (attributeIds != null && attributeIds.Count > 0)
            {
                attributeIdTable = new List<SqlDataRecord>();

                foreach (Int32 id in attributeIds)
                {
                    SqlDataRecord attributeIdRecord = new SqlDataRecord(attributeIdList);
                    attributeIdRecord.SetValue(0, (Int64)referenceId);
                    attributeIdRecord.SetValue(1, (Int32)id);
                    attributeIdTable.Add(attributeIdRecord);
                }
            }

            return attributeIdTable;
        }

        /// <summary>
        /// Extracting the attribute models and its attributes properties and returned as attribute table
        /// </summary>
        /// <param name="attributeModels">Indicates collection of attribute models</param>
        /// <param name="attributeListMetaData">Indicates attribute meta data</param>
        /// <param name="referenceId">Indicates Reference Identifier</param>
        /// <returns>Returns attribute table as a list of sqlDataRecord</returns>
        public static List<SqlDataRecord> CreateAttributeListTable(AttributeModelCollection attributeModels, SqlMetaData[] attributeListMetaData, Int64 referenceId)
        {
            List<SqlDataRecord> attributeListTable = null;

            if (attributeModels != null && attributeModels.Count > 0)
            {
                attributeListTable = new List<SqlDataRecord>();
                var distinctAttrIds = new Collection<Int32>();

                foreach (AttributeModel attrModel in attributeModels)
                {
                    if (!distinctAttrIds.Contains(attrModel.Id) && !attrModel.IsComplexChild)
                    {
                        SqlDataRecord attributeRecord = CreateAttributeDataRecord(attributeListMetaData, attrModel, referenceId);

                        attributeListTable.Add(attributeRecord);
                        distinctAttrIds.Add(attrModel.Id);
                    }
                }
            }

            return attributeListTable;
        }

        /// <summary>
        /// Extracting the attribute models and its attributes properties and returned as attribute table
        /// </summary>
        /// <param name="attributeModelsForHierarchy">Indicates collection of attribute models for hierarchy</param>
        /// <param name="attributeListMetaData">Indicates attribute meta data</param>
        /// <param name="referenceId">Indicates Reference Identifier</param>
        /// <returns>Returns attribute table as a list of sqlDataRecord</returns>
        public static List<SqlDataRecord> CreateAttributeListTableForHierarchyGet(IDictionary<Int32, AttributeModelCollection> attributeModelsForHierarchy,
            SqlMetaData[] attributeListMetaData, Int64 referenceId)
        {
            List<SqlDataRecord> attributeListTable = null;

            if (attributeModelsForHierarchy != null && attributeModelsForHierarchy.Count > 0)
            {
                var distinctAttrIds = new Collection<Int32>();
                AttributeModelCollection attributeModels = null;

                foreach (KeyValuePair<Int32, AttributeModelCollection> keyValuePair in attributeModelsForHierarchy)
                {
                    attributeModels = keyValuePair.Value;
                    if (attributeModels != null && attributeModels.Count > 0)
                    {
                        if (attributeListTable == null)
                        {
                            attributeListTable = new List<SqlDataRecord>();
                        }

                        FillAttributeListTableForHierarchyGet(attributeListTable, attributeListMetaData, attributeModels, distinctAttrIds, referenceId, keyValuePair.Key);
                    }
                }
            }

            return attributeListTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        public static void UpdateEntityOperationResults(SqlDataReader reader, EntityCollection entities, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int64 entityId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String idPath = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["EntityId"] != null)
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["IDPath"] != null)
                    idPath = reader["IdPath"].ToString();
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();

                //Get Entity
                Entity entity = entities.SingleOrDefault(e => e.Id == id);

                //Get the entity operation result
                EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == id);

                if (entity != null && entityOperationResult != null)
                {
                    if (id < 0)
                    {
                        //Update the id with the new entityId
                        entity.Id = entityId;
                        entity.IdPath = idPath;
                        entityOperationResult.EntityId = entityId;
                    }

                    if (hasError)
                    {
                        //Add error
                        entityOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                    }
                    else if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithWarnings)
                    {
                        //No errors.. update status as Successful.
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }

            entityOperationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        public static void UpdateRelationshipOperationResults(SqlDataReader reader, EntityCollection entities, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int64 relationshipId = 0;
                Int64 entityId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["FK_Relationship"] != null)
                    Int64.TryParse(reader["FK_Relationship"].ToString(), out relationshipId);
                if (reader["FK_CNode"] != null)
                    Int64.TryParse(reader["FK_CNode"].ToString(), out entityId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();

                //Get Entity
                Entity entity = entities.SingleOrDefault(e => e.Id == entityId);

                //Get the entity operation result
                EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entityId);

                if (entity != null && entityOperationResult != null)
                {
                    Relationship relationship = (Relationship)entity.Relationships.GetRelationshipById(id);

                    RelationshipOperationResult relOperationResult = entityOperationResult.RelationshipOperationResultCollection.GetRelationshipOperationResult(id);

                    if (relOperationResult != null)
                    {
                        if (id < 0)
                        {
                            //Update the id with the new relationshipId
                            relationship.Id = relationshipId;
                            relOperationResult.RelationshipId = relationshipId;
                        }

                        if (hasError)
                        {
                            //Add error
                            relOperationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                        }
                        else if (relOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithWarnings &&
                                 relOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors &&
                                 relOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                        {
                            //No errors.. update status as Successful.
                            relOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                }
            }

            entityOperationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        /// Create EntityTable which contains only Entity Guids.
        /// </summary>
        /// <param name="entityGuids">Indicates the Entity Guids</param>
        /// <param name="entityMetaData">Indicates the Entity MetaData</param>
        /// <returns>Return Entity Table.</returns>
        public static List<SqlDataRecord> CreateEntityGuidTable(ICollection<Guid> entityGuids, SqlMetaData[] entityMetaData)
        {
            List<SqlDataRecord> entityIdTable = new List<SqlDataRecord>();

            if (entityGuids != null && entityGuids.Count > 0)
            {
                foreach (Guid entityGuid in entityGuids)
                {
                    SqlDataRecord entityRecord = new SqlDataRecord(entityMetaData);
                    entityRecord.SetValue(0, entityGuid);
                    entityIdTable.Add(entityRecord);
                }
            }
            else
            {
                entityIdTable = null;
            }

            return entityIdTable;
        }

        /// <summary>
        /// Create EntityTable which contains only Entity ids.
        /// </summary>
        /// <param name="entityIds">Indicates the Entity Ids</param>
        /// <param name="entityMetaData">Indicates the Entity MetaData</param>
        /// <returns>Return Entity Table.</returns>
        public static List<SqlDataRecord> CreateEntityIdTable(Collection<Int64> entityIds, SqlMetaData[] entityMetaData)
        {
            List<SqlDataRecord> entityIdTable = new List<SqlDataRecord>();

            if (entityIds != null && entityIds.Count > 0)
            {
                foreach (Int64 entityId in entityIds)
                {
                    SqlDataRecord entityRecord = new SqlDataRecord(entityMetaData);
                    entityRecord.SetValue(0, entityId);
                    entityIdTable.Add(entityRecord);
                }
            }
            else
            {
                entityIdTable = null;
            }

            return entityIdTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locales"></param>
        /// <param name="systemDataLocaleId"></param>
        /// <param name="sqlLocalesMetadata"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> CreateLocaleTable(Collection<LocaleEnum> locales, Int32 systemDataLocaleId, SqlMetaData[] sqlLocalesMetadata)
        {
            List<SqlDataRecord> localeList = null;

            if (locales != null && locales.Count > 0)
            {
                localeList = new List<SqlDataRecord>();

                foreach (LocaleEnum locale in locales)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(sqlLocalesMetadata);
                    localeRecord.SetValue(0, (Int32)locale);
                    localeRecord.SetValue(1, locale.ToString());
                    if (systemDataLocaleId == (Int32)locale)
                    {
                        localeRecord.SetValue(2, true);
                    }
                    else
                    {
                        localeRecord.SetValue(2, false);
                    }
                    localeList.Add(localeRecord);
                }
            }

            return localeList;
        }

        /// <summary>
        /// Creates a List of SqlDataRecords for entity type table
        /// </summary>
        /// <param name="entityTypes">The entity types for which the sqlrecords have to be created</param>
        /// <param name="entityTypesMetadata">The sql meta data information for the entity types table</param>
        /// <returns></returns>
        public static List<SqlDataRecord> CreateEntityTypeTable(Dictionary<Int32, EntityType> entityTypes, SqlMetaData[] entityTypesMetadata)
        {
            List<SqlDataRecord> entityTypeRecordList = null;

            if (entityTypes != null && entityTypes.Count > 0)
            {
                entityTypeRecordList = new List<SqlDataRecord>();

                foreach (KeyValuePair<Int32, EntityType> keyValuePair in entityTypes)
                {
                    SqlDataRecord entityTypeRecord = new SqlDataRecord(entityTypesMetadata);
                    entityTypeRecord.SetValue(0, keyValuePair.Value.Id);
                    entityTypeRecord.SetValue(1, null);
                    entityTypeRecord.SetValue(2, (Byte)keyValuePair.Key);
                    entityTypeRecordList.Add(entityTypeRecord);
                }
            }

            return entityTypeRecordList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locales"></param>
        /// <param name="systemDataLocaleId"></param>
        /// <param name="sqlLocalesMetadata"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> CreateLocaleTable(Collection<Locale> locales, Int32 systemDataLocaleId, SqlMetaData[] sqlLocalesMetadata)
        {
            List<SqlDataRecord> localeList = null;

            if (locales != null && locales.Count > 0)
            {
                localeList = new List<SqlDataRecord>();
                foreach (Locale locale in locales)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(sqlLocalesMetadata);
                    localeRecord.SetValue(0, locale.Id);
                    localeRecord.SetValue(1, locale.Name);

                    if (systemDataLocaleId == locale.Id)
                    {
                        localeRecord.SetValue(2, true);
                    }
                    else
                    {
                        localeRecord.SetValue(2, false);
                    }

                    localeList.Add(localeRecord);
                }
            }

            return localeList;
        }

        /// <summary>
        /// Create IntegerTable which contains only ids.
        /// </summary>
        /// <param name="ids">Indicates id list type of Int32</param>
        /// <param name="integerIdMetaData">Indicates the integer meta data</param>
        /// <returns>Returns integer id table.</returns>
        public static List<SqlDataRecord> CreateIntegerIdTable(Collection<Int32> ids, SqlMetaData[] integerIdMetaData)
        {
            List<SqlDataRecord> integerIdTable = new List<SqlDataRecord>();

            if (ids != null && ids.Count > 0)
            {
                foreach (Int32 id in ids)
                {
                    SqlDataRecord integerRecord = new SqlDataRecord(integerIdMetaData);
                    integerRecord.SetValue(0, id);
                    integerIdTable.Add(integerRecord);
                }
            }
            else
            {
                integerIdTable = null;
            }

            return integerIdTable;
        }


        /// <summary>
        /// Creates SearchAttributeTable
        /// </summary>
        /// <param name="searchAttributeRules">Indicates collection of search attribute rules</param>
        /// <param name="sqlAttributeDetailsList">Indicates the metadata of attribute details list</param>
        /// <returns>Returns list of search Attribute Values</returns>
        public static List<SqlDataRecord> CreateSearchAttributeTable(Collection<SearchAttributeRule> searchAttributeRules, SqlMetaData[] sqlAttributeDetailsList)
        {
            List<SqlDataRecord> attributeDetailsList = new List<SqlDataRecord>();

            if (searchAttributeRules != null && searchAttributeRules.Count > 0)
            {
                foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                {
                    ValueCollection values = (ValueCollection)searchAttributeRule.Attribute.GetCurrentValues();

                    foreach (Value value in values)
                    {
                        SqlDataRecord attributeDetailsRecord = new SqlDataRecord(sqlAttributeDetailsList);
                        attributeDetailsRecord.SetValue(0, searchAttributeRule.Attribute.Id);
                        attributeDetailsRecord.SetValue(1, value.AttrVal);
                        attributeDetailsRecord.SetValue(2, null);
                        String searchOperator = String.Empty;

                        if (searchAttributeRule.Operator == SearchOperator.GreaterThan)
                        {
                            searchOperator = ">";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.LessThan)
                        {
                            searchOperator = "<";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.GreaterThanOrEqualTo)
                        {
                            searchOperator = ">=";
                        }
                        else if (searchAttributeRule.Operator == SearchOperator.LessThanOrEqualTo)
                        {
                            searchOperator = "<=";
                        }
                        else
                        {
                            searchOperator = Utility.GetOperatorString(searchAttributeRule.Operator);
                        }

                        attributeDetailsRecord.SetValue(3, searchOperator);
                        attributeDetailsList.Add(attributeDetailsRecord);
                    }
                }
            }

            if (attributeDetailsList.Count < 1)
            {
                attributeDetailsList = null;
            }

            return attributeDetailsList;
        }
        /// <summary>
        /// populates business conditions status for entity
        /// </summary>
        /// <param name="reader">Indicates reader to read DB returned data</param>
        public static Dictionary<Int64, BusinessConditionStatusCollection> ReadBusinessConditions(IDataReader reader)
        {
            Dictionary<Int64, BusinessConditionStatusCollection> businessConditionStatusDictionary = new Dictionary<Int64, BusinessConditionStatusCollection>();
            
            if (reader != null)
            {
                while (reader.Read())
                {
                    Int64 entityId = -1;

                    if (reader["FK_CNode"] != null)
                    {
                        entityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), entityId);
                    }

                    if (entityId > 0)
                    {
                        BusinessConditionStatusCollection businessConditionStatusCollection;
                        if (!businessConditionStatusDictionary.TryGetValue(entityId, out businessConditionStatusCollection))
                        {
                            businessConditionStatusCollection = new BusinessConditionStatusCollection();
                            businessConditionStatusDictionary.Add(entityId, businessConditionStatusCollection);
                        }

                        BusinessConditionStatus businessCondition = new BusinessConditionStatus();

                        if (reader["BusinessConditionId"] != null)
                        {
                            businessCondition.Id = ValueTypeHelper.Int32TryParse(reader["BusinessConditionId"].ToString(), businessCondition.Id);
                        }

                        if (reader["BusinessConditionName"] != null)
                        {
                            businessCondition.Name = reader["BusinessConditionName"].ToString();
                        }

                        if (reader["Status"] != null)
                        {
                            ValidityStateValue status = ValidityStateValue.Unknown;
                            ValueTypeHelper.EnumTryParse<ValidityStateValue>(reader["Status"].ToString(), true, out status);
                            businessCondition.Status = status;
                        }

                        businessConditionStatusCollection.Add(businessCondition);
                    }
                }
            }

            return businessConditionStatusDictionary;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method reads data reader which provides entity data in specified format and creates entity collection with specified set of attribute components.
        /// </summary>
        /// <param name="reader">Sql reader holding entity data stream</param>
        /// <param name="module"></param>
        /// <param name="readAttributes">Indicate if we need to read attributes</param>
        /// <param name="readRelationships">Indicate if we need to read relationships</param>
        /// <param name="readHiearchyRelationships"></param>
        /// <param name="readExtensionRelationships"></param>
        /// <param name="contextualAttributeModels">Represents an attribute model provider which contains attribute models specific to the context</param>
        /// <param name="readComplexAttributes"></param>
        /// <param name="isHierarchyRelationshipGetCall"></param>
        /// <param name="readEntityBusinessConditions">Indicates whether to load business conditions for requested entities or not</param>
        /// <returns></returns>
        private static EntityCollection ReadEntitiesMain(IDataReader reader, MDMCenterModules module, Boolean readAttributes, Boolean readRelationships,
            Boolean readHiearchyRelationships, Boolean readExtensionRelationships, ContextualObjectCollection<AttributeModelCollection> contextualAttributeModels,
            Boolean readComplexAttributes = true, Boolean isHierarchyRelationshipGetCall = false, Boolean readEntityBusinessConditions = false)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            EntityCollection entityCollection = null;

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            if (reader != null)
            {
                entityCollection = new EntityCollection();

                ReadEntityProperties(reader, entityCollection, module);

                if (entityCollection.Count > 0)
                {
                    bool populateComplexAttributeInstanceDictionary = !(readComplexAttributes && module == MDMCenterModules.Export);

                    Boolean isRelationshipComplexAttributesReadRequired = false;

                    var complexAttributeInstanceDictionary = new Dictionary<String, Collection<AttributeAndAttributeModelPair>>();

                    if (readAttributes)
                    {
                        reader.NextResult();
                        ReadAttributes(reader, entityCollection, contextualAttributeModels, module, populateComplexAttributeInstanceDictionary, complexAttributeInstanceDictionary,
                            isHierarchyRelationshipGetCall);
                    }

                    if (readEntityBusinessConditions)
                    {
                        reader.NextResult();
                        ReadEntityBusinessConditions(reader, entityCollection);
                    }

                    if (readRelationships)
                    {
                        reader.NextResult();
                        ReadRelationships(reader, entityCollection, contextualAttributeModels.GetObjectByContext(), module, ref isRelationshipComplexAttributesReadRequired);
                    }

                    if (readComplexAttributes)
                    {
                        if (!populateComplexAttributeInstanceDictionary)
                        {
                            ReadComplexAttributes(reader, entityCollection, module);
                        }
                        else if (complexAttributeInstanceDictionary.Count > 0)
                        {
                            ReadComplexAttributesWithFlatData(reader, complexAttributeInstanceDictionary, module);
                        }
                    }

                    if (readComplexAttributes && isRelationshipComplexAttributesReadRequired) //No point reading of relationship complex attributes
                    {
                        ReadComplexAttributes(reader, entityCollection, module);
                    }
                }
            }

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("Completed preparing entityCollection object for " + entityCollection.Count.ToString() + " records.");
                diagnosticActivity.Stop();
            }

            return entityCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        /// <param name="module"></param>
        private static void ReadEntityProperties(IDataReader reader, EntityCollection entityCollection, MDMCenterModules module)
        {
            while (reader.Read())
            {
                #region Must to return properties for all caller DA

                #region Base properties

                Int64 entityId = 0;
                String name = String.Empty;
                String longName = String.Empty;
                String entityFamilyName = String.Empty;
                String entityGlobalFamilyName = String.Empty;

                Int32 containerId = 0;
                Int32 entityTypeId = 0;
                Int64 categoryId = 0;

                LocaleEnum locale = LocaleEnum.UnKnown;

                String path = String.Empty;
                String idPath = String.Empty;

                Int32 sequenceNumber = -1;
                Int32 sourceClass = 0;
                Int64 entityFamilyId = 0;
                Int64 entityFamilyGroupId = 0;
                Int16 hierarchyLevel = 0;
                Int64 crossReferenceId = 0;

                String entityTreeIdList = String.Empty;

                Int64 auditRefId = -1;
                ObjectAction action = ObjectAction.Read;
                Guid? entityGuid = null;
                Guid? crossReferenceGuid = null;

                if (reader["Id"] != null)
                {
                    entityId = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), entityId);
                }

                if (reader["Name"] != null)
                {
                    name = reader["Name"].ToString();
                }

                if (reader["LongName"] != null)
                {
                    longName = reader["LongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerId);
                }

                if (reader["EntityTypeId"] != null)
                {
                    entityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entityTypeId);
                }

                if (reader["CategoryId"] != null)
                {
                    categoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), categoryId);
                }

                if (module != MDMCenterModules.Denorm && module != MDMCenterModules.Export)
                {
                    if (reader["EntityFamilyId"] != null)
                    {
                        entityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), entityFamilyId);
                    }

                    if (reader["EntityFamilyLongName"] != null)
                    {
                        entityFamilyName = reader["EntityFamilyLongName"].ToString();
                    }

                    if (reader["HierarchyLevel"] != null)
                    {
                        hierarchyLevel = ValueTypeHelper.Int16TryParse(reader["HierarchyLevel"].ToString(), hierarchyLevel);
                    }

                    if (reader["EntityGlobalFamilyId"] != null)
                    {
                        entityFamilyGroupId = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), entityFamilyGroupId);
                    }

                    if (reader["EntityGlobalFamilyLongName"] != null)
                    {
                        entityGlobalFamilyName = reader["EntityGlobalFamilyLongName"].ToString();
                    }

                    if (reader["EntityReferenceId"] != null)
                    {
                        crossReferenceId = ValueTypeHelper.Int64TryParse(reader["EntityReferenceId"].ToString(), crossReferenceId);
                    }

                }

                if (module != MDMCenterModules.Entity && module != MDMCenterModules.Denorm && module != MDMCenterModules.Search)
                {
                    if (reader["Locale"] != null)
                    {
                        String strLocale = reader["Locale"].ToString();

                        if (!String.IsNullOrWhiteSpace(strLocale))
                        {
                            Enum.TryParse<LocaleEnum>(strLocale, out locale);
                        }
                    }
                }
                else
                {
                    if (reader["LocaleId"] != null)
                    {
                        Int32 localeId = ValueTypeHelper.Int32TryParse(reader["LocaleId"].ToString(), 0);
                        locale = (LocaleEnum)localeId;
                    }
                }

                if (reader["Path"] != null)
                {
                    path = reader["Path"].ToString();
                }

                if (reader["IDPath"] != null)
                {
                    idPath = reader["IDPath"].ToString();
                }

                if (reader["AuditRefId"] != null)
                {
                    auditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefId"].ToString(), -1);
                }

                if (module == MDMCenterModules.Export)
                {
                    if (reader["Seq"] != null)
                    {
                        sequenceNumber = ValueTypeHelper.Int32TryParse(reader["Seq"].ToString(), -1);
                    }
                }
                else
                {
                    if (reader["SourceClass"] != null)
                    {
                        sourceClass = ValueTypeHelper.Int32TryParse(reader["SourceClass"].ToString(), 0);
                    }
                }

                if (reader["Action"] != null)
                {
                    String strAction = reader["Action"].ToString();
                    action = GetAction(strAction);
                }

                if (reader["EntityTreeIdList"] != null)
                {
                    entityTreeIdList = reader["EntityTreeIdList"].ToString();
                }

                if (reader["EntityGUID"] != null)
                {
                    String stringGuid = reader["EntityGUID"].ToString();

                    if (!String.IsNullOrEmpty(stringGuid))
                    {
                        entityGuid = new Guid(stringGuid);
                    }
                }

                if (reader["EntityReferenceGUID"] != null)
                {
                    String stringCrossReferenceGuid = reader["EntityReferenceGUID"].ToString();

                    if (!String.IsNullOrEmpty(stringCrossReferenceGuid))
                    {
                        crossReferenceGuid = new Guid(stringCrossReferenceGuid);
                    }
                }

                Entity entity = new Entity
                {
                    Id = entityId,
                    Name = name,
                    ExternalId = name,
                    LongName = longName,
                    SKU = String.Empty,
                    ContainerId = containerId,
                    EntityTypeId = entityTypeId,
                    CategoryId = categoryId,
                    Locale = locale,
                    Path = path,
                    IdPath = idPath,
                    SequenceNumber = sequenceNumber,
                    SourceClass = sourceClass,
                    EntityTreeIdList = entityTreeIdList,
                    AuditRefId = auditRefId,
                    Action = action,
                    EntityFamilyId = entityFamilyId,
                    EntityGlobalFamilyId = entityFamilyGroupId,
                    HierarchyLevel = hierarchyLevel,
                    EntityGuid = entityGuid,
                    CrossReferenceId = crossReferenceId,
                    EntityFamilyLongName = entityFamilyName,
                    EntityGlobalFamilyLongName = entityGlobalFamilyName,
                    CrossReferenceGuid = crossReferenceGuid
                };

                #endregion

                #region Parent Info

                Int64 parentEntityId = 0;
                Int32 parentEntityTypeId = 0;
                String parentEntityName = String.Empty;
                String parentEntityLongName = String.Empty;

                if (reader["ParentEntityId"] != null)
                {
                    parentEntityId = ValueTypeHelper.Int64TryParse(reader["ParentEntityId"].ToString(), parentEntityId);
                }

                if (module != MDMCenterModules.Denorm && module != MDMCenterModules.Export)
                {
                    if (reader["ParentEntityTypeId"] != null)
                    {
                        parentEntityTypeId = ValueTypeHelper.Int32TryParse(reader["ParentEntityTypeId"].ToString(), parentEntityTypeId);
                    }
                }

                if (reader["ParentEntityName"] != null)
                {
                    parentEntityName = reader["ParentEntityName"].ToString();
                }

                if (reader["ParentEntityLongName"] != null)
                {
                    parentEntityLongName = reader["ParentEntityLongName"].ToString();
                }

                entity.ParentEntityId = parentEntityId;
                entity.ParentEntityTypeId = parentEntityTypeId;
                entity.ParentEntityName = parentEntityName;
                entity.ParentExternalId = parentEntityName;
                entity.ParentEntityLongName = parentEntityLongName;

                #endregion

                #region Parent Extension Info

                Int64 parentExtensionEntityId = 0;
                Int64 extensionUniqueId = 0;
                String parentExtensionEntityName = String.Empty;
                String parentExtensionEntityLongName = String.Empty;
                String parentExtensionEntityExternalId = String.Empty;
                Int32 parentExtensionEntityContainerId = 0;
                Int32 parentExtensionEntityCategoryId = 0;

                if (reader["ParentExtensionEntityId"] != null)
                {
                    parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader["ParentExtensionEntityId"].ToString(), parentExtensionEntityId);
                }

                if (reader["ExtensionUniqueId"] != null)
                {
                    extensionUniqueId = ValueTypeHelper.Int64TryParse(reader["ExtensionUniqueId"].ToString(), extensionUniqueId);
                }

                if (reader["ParentExtensionEntityName"] != null)
                {
                    parentExtensionEntityName = reader["ParentExtensionEntityName"].ToString();
                }

                if (reader["ParentExtensionEntityLongName"] != null)
                {
                    parentExtensionEntityLongName = reader["ParentExtensionEntityLongName"].ToString();
                }

                if (reader["ParentExtensionEntityContainerId"] != null)
                {
                    parentExtensionEntityContainerId = ValueTypeHelper.Int32TryParse(reader["ParentExtensionEntityContainerId"].ToString(), 0);
                }

                if (reader["ParentExtensionEntityCategoryId"] != null)
                {
                    parentExtensionEntityCategoryId = ValueTypeHelper.Int32TryParse(reader["ParentExtensionEntityCategoryId"].ToString(), 0);
                }

                entity.ParentExtensionEntityId = parentExtensionEntityId;
                entity.ExtensionUniqueId = extensionUniqueId;
                entity.ParentExtensionEntityName = parentExtensionEntityName;
                entity.ParentExtensionEntityLongName = parentExtensionEntityLongName;
                entity.ParentExtensionEntityExternalId = parentExtensionEntityName;
                entity.ParentExtensionEntityContainerId = parentExtensionEntityContainerId;
                entity.ParentExtensionEntityCategoryId = parentExtensionEntityCategoryId;

                #endregion

                #endregion

                #region Optional properties not being returned by core entity get and denorm get

                if (module != MDMCenterModules.Entity && module != MDMCenterModules.Denorm)
                {
                    #region Parent Extension Container and Category Path

                    if (module != MDMCenterModules.Search)
                    {
                        String parentExtensionEntityContainerName = String.Empty;
                        String parentExtensionEntityCategoryPath = String.Empty;
                        String parentExtensionEntityCategoryLongNamePath = String.Empty;

                        if (reader["ParentExtensionEntityContainerName"] != null)
                        {
                            parentExtensionEntityContainerName = reader["ParentExtensionEntityContainerName"].ToString();
                        }

                        if (reader["ParentExtensionEntityCategoryPath"] != null)
                        {
                            parentExtensionEntityCategoryPath = reader["ParentExtensionEntityCategoryPath"].ToString();
                        }

                        if (reader["ParentExtensionEntityCategoryLongNamePath"] != null)
                        {
                            parentExtensionEntityCategoryLongNamePath = reader["ParentExtensionEntityCategoryLongNamePath"].ToString();
                        }

                        entity.ParentExtensionEntityContainerName = parentExtensionEntityContainerName;
                        entity.ParentExtensionEntityCategoryPath = parentExtensionEntityCategoryPath;
                        entity.ParentExtensionEntityCategoryLongNamePath = parentExtensionEntityCategoryLongNamePath;
                    }

                    #endregion

                    #region Container Info

                    String containerName = String.Empty;
                    String containerLongName = String.Empty;
                    Int32 organizationId = 0;
                    String organizationName = String.Empty;
                    String organizationLongName = String.Empty;

                    if (reader["ContainerName"] != null)
                    {
                        containerName = reader["ContainerName"].ToString();
                    }

                    if (reader["ContainerLongName"] != null)
                    {
                        containerLongName = reader["ContainerLongName"].ToString();
                    }

                    if (module != MDMCenterModules.Search)
                    {
                        if (reader["OrganizationId"] != null)
                        {
                            Int32.TryParse(reader["OrganizationId"].ToString(), out organizationId);
                        }

                        if (reader["OrganizationName"] != null)
                        {
                            organizationName = reader["OrganizationName"].ToString();
                        }

                        if (reader["OrganizationLongName"] != null)
                        {
                            organizationLongName = reader["OrganizationLongName"].ToString();
                        }
                    }

                    entity.ContainerName = containerName;
                    entity.ContainerLongName = containerLongName;
                    entity.OrganizationId = organizationId;
                    entity.OrganizationName = organizationName;
                    entity.OrganizationLongName = organizationLongName;

                    #endregion

                    #region Entity Type Info

                    String entityTypeName = String.Empty;
                    String entityTypeLongName = String.Empty;

                    if (reader["EntityTypeName"] != null)
                    {
                        entityTypeName = reader["EntityTypeName"].ToString();
                    }

                    if (reader["EntityTypeLongName"] != null)
                    {
                        entityTypeLongName = reader["EntityTypeLongName"].ToString();
                    }

                    entity.EntityTypeName = entityTypeName;
                    entity.EntityTypeLongName = entityTypeLongName;

                    #endregion

                    #region Category Info

                    String categoryName = String.Empty;
                    String categoryLongName = String.Empty;
                    String categoryPath = String.Empty;
                    String categoryLongNamePath = String.Empty;

                    if (reader["CategoryLongName"] != null)
                    {
                        categoryLongName = reader["CategoryLongName"].ToString();
                    }

                    if (reader["CategoryLongNamePath"] != null)
                    {
                        categoryLongNamePath = reader["CategoryLongNamePath"].ToString();
                    }

                    if (module != MDMCenterModules.Search)
                    {
                        if (reader["CategoryName"] != null)
                        {
                            categoryName = reader["CategoryName"].ToString();
                        }

                        if (reader["CategoryPath"] != null)
                        {
                            categoryPath = reader["CategoryPath"].ToString();
                        }  
                    }

                    entity.CategoryName = categoryName;
                    entity.CategoryLongName = categoryLongName;
                    entity.CategoryPath = categoryPath;
                    entity.CategoryLongNamePath = categoryLongNamePath;

                    #endregion
                }

                #endregion

                entityCollection.Add(entity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        /// <param name="contextualAttributeModels"></param>
        /// <param name="module"></param>
        /// <param name="populateComplexAttributeInstanceDictionary"></param>
        /// <param name="complexAttributeInstanceDictionary"></param>
        /// <param name="isHiearchyRelationshipGetCall"></param>
        private static void ReadAttributes(IDataReader reader, EntityCollection entityCollection, ContextualObjectCollection<AttributeModelCollection> contextualAttributeModels,
            MDMCenterModules module, Boolean populateComplexAttributeInstanceDictionary, Dictionary<String, Collection<AttributeAndAttributeModelPair>> complexAttributeInstanceDictionary,
            Boolean isHiearchyRelationshipGetCall = false)
        {
            while (reader.Read())
            {
                #region Step: Declare Local variables

                Int64 entityId = 0;
                Int32 attributeId = 0;
                String attributeValue = String.Empty;
                Int32 valueRefId = 0;
                Int32 uomId = -1;
                String uom = String.Empty;
                Int32 sequence = 0;
                AttributeValueSource sourceFlag = AttributeValueSource.Overridden;
                Int64 sourceEntityId = 0;
                Int32 sourceClass = 0;
                Int32 localeId = 0;
                LocaleEnum locale = LocaleEnum.en_WW;
                ObjectAction action = ObjectAction.Read;
                Int64 id = -1;
                Int64 auditRefId = -1;
                Boolean isInvalidData = false;
                #endregion

                #region Step: Read Entity detail from Attirbute Row

                if (reader["FK_Attribute"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), 0);

                if (reader["FK_CNode"] != null)
                    entityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);

                //Get the entity
                var entity = (Entity)entityCollection.GetEntity(entityId);

                AttributeModelCollection attributeModels = GetAttributeModelCollectionForContext(entity, contextualAttributeModels, isHiearchyRelationshipGetCall);

                #endregion

                if (entity != null && attributeModels != null)
                {
                    #region Step: Read Attribute value Properties and create value object

                    //Read other parameters
                    if (reader["Id"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                    if (reader["AttrVal"] != null)
                        attributeValue = reader["AttrVal"].ToString();

                    if (reader["WSID"] != null)
                        valueRefId = ValueTypeHelper.Int32TryParse(reader["WSID"].ToString(), -1);

                    if (reader["FK_UOM"] != null)
                        uomId = ValueTypeHelper.Int32TryParse(reader["FK_UOM"].ToString(), -1);

                    if (reader["UOM"] != null)
                        uom = reader["UOM"].ToString();

                    if (reader["Seq"] != null)
                    {
                        Decimal decSeq = 0;
                        decSeq = ValueTypeHelper.DecimalTryParse(reader["Seq"].ToString(), 0);
                        sequence = (Int32)decSeq;
                    }

                    if (reader["AuditRefId"] != null)
                        auditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefId"].ToString(), -1);

                    if (reader["SRCFlag"] != null)
                    {
                        String strSourceFlag = reader["SRCFlag"].ToString();

                        switch (strSourceFlag.ToUpper())
                        {
                            case "I":
                                sourceFlag = AttributeValueSource.Inherited;
                                break;
                            case "O":
                                sourceFlag = AttributeValueSource.Overridden;
                                break;
                        }
                    }

                    if (module == MDMCenterModules.Denorm || module == MDMCenterModules.Entity)
                    {
                        if (reader["SourceEntityId"] != null)
                            Int64.TryParse(reader["SourceEntityId"].ToString(), out sourceEntityId);

                        if (reader["SourceClass"] != null)
                            Int32.TryParse(reader["SourceClass"].ToString(), out sourceClass);
                    }

                    if (reader["FK_Locale"] != null)
                        localeId = ValueTypeHelper.Int32TryParse(reader["FK_Locale"].ToString(), 0);

                    locale = (LocaleEnum)localeId;

                    if (module == MDMCenterModules.Export)
                    {
                        if (reader["Action"] != null)
                        {
                            String strAction = reader["Action"].ToString();
                            action = GetAction(strAction);
                        }
                    }

                    if (reader["IsInvalidData"] != null)
                    {
                        isInvalidData = ValueTypeHelper.BooleanTryParse(reader["IsInvalidData"].ToString(), false);
                    }

                    //Create the value object
                    Value value = new Value
                    {
                        AttrVal = attributeValue,
                        Locale = locale,
                        Sequence = sequence,
                        Uom = uom,
                        UomId = uomId,
                        ValueRefId = valueRefId,
                        Id = id,
                        HasInvalidValue = isInvalidData
                    };

                    #endregion

                    Attribute attribute = null;
                    Attribute complexAttributeInstance = null;

                    //Check whether the attribute has already been added
                    IAttribute iAttribute = entity.Attributes.GetAttribute(attributeId, locale);

                    if (iAttribute != null)
                    {
                        attribute = (Attribute)iAttribute;

                        //HACK>>THERE IS POSSIBLITY THAT ATTRIBUTE RETURNED IS NOT IN THE LOCALE WE HAVE LOOKED FOR...THUS MAKE SURE THE OBJECT IS EXACT MATCH FOR LOCALE..
                        if (attribute.Locale != locale)
                            attribute = null;
                    }

                    var attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);

                    if (attribute != null)
                    {
                        #region Step: Attribute object already exist in entity object, just add value object in that attribute object's value collection

                        //Yes.. attribute has already been added
                        //Request is for another value.. 
                        if (attribute.IsHierarchical)
                        {
                            if (value.AttrVal != null)
                            {
                                Attribute instanceAttribute = new Attribute(value.AttrVal.ToString(), true, Constants.HIERARCHICAL_ATTRIBUTE_SERIALIZATION);
                                attribute.AddComplexChildRecord(instanceAttribute.Attributes, valueRefId, sourceFlag, locale, id, false);

                                var validateResult = attribute.ValidateHierarchicalAttribute(attributeModel, true);

                                if (validateResult != null && validateResult.Any(attributeOperationResult => attributeOperationResult.PerformedAction == ObjectAction.Exclude))
                                {
                                    attribute.InstanceRefId = -9999; // This will ensure user gets prompted that model changed
                                }
                            }
                            else
                            {
                                IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                                attribute.AddComplexChildRecord(iAttributeCollection, valueRefId, sourceFlag, locale, id, isInvalidData);
                            }
                        }
                        else if (attribute.IsComplex)
                        {
                            if (isInvalidData == false)
                            {
                                IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                                complexAttributeInstance = (Attribute)attribute.AddComplexChildRecordAt(sequence, iAttributeCollection, valueRefId, sourceFlag, locale, id, isInvalidData);
                            }
                            else
                            {
                                if (value.AttrVal != null)
                                {
                                    Attribute invalidInstanceAttribute = new Attribute(value.AttrVal.ToString());
                                    attribute.AddComplexChildRecord(invalidInstanceAttribute.Attributes, valueRefId, sourceFlag, locale, id, true);
                                }
                            }
                        }
                        else
                        {
                            //Add the value
                            if (sourceFlag == AttributeValueSource.Inherited)
                            {
                                if (attribute.OverriddenValues == null || attribute.OverriddenValues.Count < 1)
                                {
                                    attribute.SourceFlag = AttributeValueSource.Inherited;
                                    attribute.AuditRefId = auditRefId;
                                }

                                attribute.AppendInheritedValue(value);
                            }
                            else
                            {
                                attribute.SourceFlag = sourceFlag;
                                attribute.AppendValueInvariant(value);
                                value.Action = action;
                                attribute.AuditRefId = auditRefId;
                            }
                        }

                        attribute.Action = action;
                        value.Action = action;

                        #endregion
                    }
                    else
                    {
                        #region Step: Create new Attribute object and store value object in that

                        if (attributeModel != null)
                        {
                            if (isHiearchyRelationshipGetCall)
                            {
                                // For hierarchy get, AttributeModels are not pre-populated. Hence the same is done here
                                entity.AttributeModels.Add(attributeModel);
                            }

                            //Instantiate attribute object
                            attribute = new Attribute(attributeModel, locale);

                            if (attribute.IsHierarchical)
                            {
                                if (value.AttrVal != null)
                                {
                                    Attribute instanceAttribute = new Attribute(value.AttrVal.ToString(), true, Constants.HIERARCHICAL_ATTRIBUTE_SERIALIZATION);
                                    attribute.AddComplexChildRecord(instanceAttribute.Attributes, valueRefId, sourceFlag, locale, id, false);

                                    var validateResult = attribute.ValidateHierarchicalAttribute(attributeModel, true);

                                    if (validateResult != null && validateResult.Any(attributeOperationResult => attributeOperationResult.PerformedAction == ObjectAction.Exclude))
                                    {
                                        attribute.InstanceRefId = -9999; // This will ensure user gets prompted that model changed
                                    }
                                }
                                else
                                {
                                    IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                                    attribute.AddComplexChildRecord(iAttributeCollection, valueRefId, sourceFlag, locale, id, isInvalidData);
                                }

                            }
                            else if (attribute.IsComplex)
                            {
                                if (isInvalidData == false)
                                {
                                    IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                                    complexAttributeInstance = (Attribute)attribute.AddComplexChildRecord(iAttributeCollection, valueRefId, sourceFlag, locale, id, isInvalidData);
                                }
                                else
                                {
                                    if (value.AttrVal != null)
                                    {
                                        Attribute invalidInstanceAttribute = new Attribute(value.AttrVal.ToString());
                                        attribute.AddComplexChildRecord(invalidInstanceAttribute.Attributes, valueRefId, sourceFlag, locale, id, true);
                                    }
                                }
                            }
                            else
                            {
                                attribute.SourceFlag = sourceFlag;

                                //Add the value
                                if (sourceFlag == AttributeValueSource.Inherited)
                                    attribute.SetInheritedValueInvariant(value);
                                else
                                    attribute.SetValueInvariant(value);
                            }

                            attribute.Action = action;
                            attribute.AuditRefId = auditRefId;
                            value.Action = action;

                            if (entity != null)
                            {
                                if (module == MDMCenterModules.Export && attribute.IsLookup)
                                {
                                    entity.Attributes.Add(attribute, true);
                                }
                                else
                                {
                                    entity.Attributes.Add(attribute);
                                }
                            }
                        }

                        #endregion
                    }

                    #region Step: Set source EntityId and Class for attribute object

                    if (attribute != null)
                    {
                        if (sourceFlag == AttributeValueSource.Inherited)
                        {
                            attribute.SourceEntityIdInherited = sourceEntityId;
                            attribute.SourceClassInherited = sourceClass;
                        }
                        else
                        {
                            attribute.SourceEntityIdOverridden = sourceEntityId;
                            attribute.SourceClassOverridden = sourceClass;
                        }

                        if (attribute.SourceClassOverridden < 1)
                        {
                            attribute.SourceClassOverridden = entity.SourceClass;
                            attribute.SourceEntityIdOverridden = entity.Id;
                        }

                        if (attribute.IsCollection)
                        {
                            if (attribute.AttributeType == AttributeTypeEnum.Simple)
                            {
                                attribute.AttributeType = AttributeTypeEnum.SimpleCollection;
                            }
                            else if (attribute.IsComplex)
                            {
                                attribute.AttributeType = AttributeTypeEnum.ComplexCollection;
                            }
                        }
                    }

                    #endregion

                    #region Step: Add complex attribute instance into dictionary

                    if (populateComplexAttributeInstanceDictionary && complexAttributeInstance != null)
                    {
                        String complexInstanceKey = String.Format("CxAttrInstance_A{0}_RefId{1}", attributeId, valueRefId);

                        if (complexAttributeInstanceDictionary.ContainsKey(complexInstanceKey))
                        {
                            var complexInstances = complexAttributeInstanceDictionary[complexInstanceKey];
                            complexInstances.Add(new AttributeAndAttributeModelPair { Attribute = complexAttributeInstance, AttributeModel = attributeModel });
                        }
                        else
                        {
                            complexAttributeInstanceDictionary.Add(complexInstanceKey, new Collection<AttributeAndAttributeModelPair>() { new AttributeAndAttributeModelPair { Attribute = complexAttributeInstance, AttributeModel = attributeModel } });
                        }
                    }

                    #endregion
                }
            }

            if (module != MDMCenterModules.Denorm && module != MDMCenterModules.Export)
            {
                reader.NextResult();

                ReadSystemAttributes(reader, entityCollection, contextualAttributeModels, module, isHiearchyRelationshipGetCall);
            }
        }

        /// <summary>
        /// Read system attributes from data reader.
        /// </summary>
        /// <param name="reader">Provides a way of reading a forward-only stream of rows from a SQL Server database.</param>
        /// <param name="entities">Indicates the collection of entities.</param>
        /// <param name="contextualAttributeModels">Indicates contextual object collection of attribute models</param>
        /// <param name="module">Specifies module name which is performing action</param>
        /// <param name="isHiearchyRelationshipGetCall">Indicates is hierachy relationships get call</param>
        private static void ReadSystemAttributes(IDataReader reader, EntityCollection entities, ContextualObjectCollection<AttributeModelCollection> contextualAttributeModels, MDMCenterModules module, Boolean isHiearchyRelationshipGetCall)
        {
            Collection<Int32> attributeIds = GetAttributeIdList(reader);
            Int64 entityId = 0;
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            while (reader.Read())
            {
                if (reader["FK_CNode"] != null)
                    entityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);

                //Get the entity
                Entity entity = (Entity)entities.GetEntity(entityId);

                if (entity != null)
                {
                    AttributeModelCollection attributeModels = GetAttributeModelCollectionForContext(entity, contextualAttributeModels, isHiearchyRelationshipGetCall);

                    foreach (Int32 attributeId in attributeIds)
                    {
                        if (attributeId > 0)
                        {
                            /* Create attribute and add it to entity.attributes only if attribute value exists in reader.
                             * System.DBNull.Value represents nonexistent value in database i.e. {}. 
                            */
                            var attributeValueFromReader = reader[attributeId.ToString()];

                            if (attributeValueFromReader != System.DBNull.Value)
                            {
                                String attributeValue = attributeValueFromReader.ToString();
                                Attribute attribute = null;

                                AttributeModel attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, systemDataLocale);

                                if (attributeModel != null)
                                {
                                    //Instantiate attribute object
                                    attribute = new Attribute(attributeModel, systemDataLocale);

                                    //Create the value object
                                    Value value = new Value
                                    {
                                        AttrVal = attributeValue,
                                        Locale = systemDataLocale,
                                        ValueRefId = 0
                                    };

                                    attribute.SourceFlag = AttributeValueSource.Overridden;
                                    attribute.AuditRefId = entity.AuditRefId;
                                    attribute.SourceEntityIdOverridden = entityId;
                                    attribute.SetValue(value);
                                    value.Action = attribute.Action = ObjectAction.Read;

                                    entity.Attributes.Add(attribute);
                                    PopulateEntityValidationStates(entity, attribute);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        /// <param name="attributeModels"></param>
        /// <param name="module"></param>
        /// <param name="isRelationshipComplexAttributesReadRequired"></param>
        /// <param name="relationshipsDictionary"></param>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="updateCache"></param>
        /// <param name="relationshipContext"></param>
        /// <returns></returns>
        private static RelationshipCollection ReadRelationships(IDataReader reader, EntityCollection entityCollection, AttributeModelCollection attributeModels, MDMCenterModules module, ref Boolean isRelationshipComplexAttributesReadRequired, Dictionary<Int64, RelationshipCollection> relationshipsDictionary = null, Dictionary<String, AttributeCollection> relationshipsAttributesDictionary = null, Boolean updateCache = false, RelationshipContext relationshipContext = null)
        {
            RelationshipCollection allRelationshipsInFlatStructure = new RelationshipCollection();
            RelationshipCollection allRelationshipsInHierarchyStructure = new RelationshipCollection();
            Boolean returnRelatedEntityDetails = false;

            if (relationshipContext != null)
            {
                returnRelatedEntityDetails = relationshipContext.ReturnRelatedEntityDetails;
            }

            if (updateCache && (relationshipsDictionary == null || relationshipsAttributesDictionary == null))
            {
                relationshipsDictionary = new Dictionary<Int64, RelationshipCollection>();
                relationshipsAttributesDictionary = new Dictionary<String, AttributeCollection>();
                relationshipContext.DataLocales = new Collection<LocaleEnum>();
            }

            while (reader.Read())
            {
                Relationship relationship = new Relationship();

                #region Step: Read relationship properties

                Int64 entityId = 0;
                String toEntityName = String.Empty;
                String toEntityLongName = String.Empty;
                Int32 toEntityTypeId = 0;
                String toEntityTypeName = String.Empty;
                String toEntityTypeLongName = String.Empty;
                String toCategoryPath = String.Empty;
                Int32 toContainerId = 0;
                String toContainerName = String.Empty;
                String toContainerLongName = String.Empty;
                Int64 toParentEntityId = 0;
                String toParentEntityName = String.Empty;
                String toParentEntityLongName = String.Empty;

                if (reader["FK_CNode"] != null)
                {
                    entityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);

                    relationship.ExtendedProperties = entityId.ToString();
                }

                if (reader["Id"] != null)
                {
                    relationship.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    relationship.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), 0);
                }

                if (reader["RelationshipTypeName"] != null)
                {
                    relationship.RelationshipTypeName = reader["RelationshipTypeName"].ToString();
                }

                if (reader["FromEntityId"] != null)
                {
                    relationship.FromEntityId = ValueTypeHelper.Int64TryParse(reader["FromEntityId"].ToString(), 0);
                }

                if (reader["RelatedEntityId"] != null)
                {
                    relationship.RelatedEntityId = ValueTypeHelper.Int64TryParse(reader["RelatedEntityId"].ToString(), 0);
                }

                if (module != MDMCenterModules.Denorm)
                {
                    if (reader["EntityReferenceId"] != null)
                    {
                        relationship.CrossReferenceId = ValueTypeHelper.Int64TryParse(reader["EntityReferenceId"].ToString(), 0);
                    }

                    if (reader["FromEntityReferenceId"] != null)
                    {
                        relationship.FromCrossReferenceId = ValueTypeHelper.Int64TryParse(reader["FromEntityReferenceId"].ToString(), 0);
                    }

                    if (reader["ToEntityReferenceId"] != null)
                    {
                        relationship.RelatedCrossReferenceId = ValueTypeHelper.Int64TryParse(reader["ToEntityReferenceId"].ToString(), 0);
                    }
                }

                relationship.Direction = RelationshipDirection.Down;

                if (reader["ContainerId"] != null)
                {
                    relationship.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);
                }

                if (reader["RelationshipParentId"] != null)
                {
                    relationship.RelationshipParentId = ValueTypeHelper.Int64TryParse(reader["RelationshipParentId"].ToString(), 0);
                }

                if (module != MDMCenterModules.Export)
                {
                    if (reader["RelationshipLevel"] != null)
                    {
                        relationship.Level = ValueTypeHelper.Int16TryParse(reader["RelationshipLevel"].ToString(), 0);
                    }

                    if (reader["RelationshipSourceId"] != null)
                    {
                        relationship.RelationshipSourceId = ValueTypeHelper.Int64TryParse(reader["RelationshipSourceId"].ToString(), 0);
                    }

                    if (reader["InheritanceMode"] != null)
                    {
                        Int32 inheritanceMode = ValueTypeHelper.Int32TryParse(reader["InheritanceMode"].ToString(), 0);
                        relationship.InheritanceMode = (InheritanceMode)inheritanceMode;
                    }

                    if (reader["SRCFlag"] != null)
                    {
                        String strSourceFlag = reader["SRCFlag"].ToString();
                        relationship.SourceFlag = Utility.GetSourceFlagEnum(strSourceFlag);
                    }

                    if (reader["Status"] != null)
                    {
                        Int32 status = ValueTypeHelper.Int32TryParse(reader["Status"].ToString(), 0);
                        relationship.Status = (RelationshipStatus)status;
                    }

                    if (reader["AuditRefId"] != null)
                    {
                        relationship.AuditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefId"].ToString(), 0);
                    }
                }

                if (module == MDMCenterModules.Export || returnRelatedEntityDetails)
                {
                    if (reader["ToExternalId"] != null)
                    {
                        toEntityName = reader["ToExternalId"].ToString();
                    }

                    if (ColumnExists(reader, "ToLongName"))
                    {
                        toEntityLongName = reader["ToLongName"].ToString();
                    }

                    if (reader["ToEntityTypeId"] != null)
                    {
                        toEntityTypeId = ValueTypeHelper.Int32TryParse(reader["ToEntityTypeId"].ToString(), 0);
                    }

                    if (reader["ToEntityTypeName"] != null)
                    {
                        toEntityTypeName = reader["ToEntityTypeName"].ToString();
                    }

                    if (reader["ToCategoryPath"] != null)
                    {
                        toCategoryPath = reader["ToCategoryPath"].ToString();
                    }

                    if (reader["ToContainerId"] != null)
                    {
                        toContainerId = ValueTypeHelper.Int32TryParse(reader["ToContainerId"].ToString(), 0);
                    }

                    if (reader["ToContainerName"] != null)
                    {
                        toContainerName = reader["ToContainerName"].ToString();
                    }

                    if (returnRelatedEntityDetails)
                    {
                        if (reader["ToEntityTypeLongName"] != null)
                        {
                            toEntityTypeLongName = reader["ToEntityTypeLongName"].ToString();
                        }

                        if (reader["ToContainerLongName"] != null)
                        {
                            toContainerLongName = reader["ToContainerLongName"].ToString();
                        }

                        if (reader["ToParentEntityId"] != null)
                        {
                            toParentEntityId = ValueTypeHelper.Int64TryParse(reader["ToParentEntityId"].ToString(), 0);
                        }

                        if (reader["ToParentEntityName"] != null)
                        {
                            toParentEntityName = reader["ToParentEntityName"].ToString();
                        }

                        if (reader["ToParentEntityLongName"] != null)
                        {
                            toParentEntityLongName = reader["ToParentEntityLongName"].ToString();
                        }

                        relationship.RelatedEntity = new Entity()
                        {
                            Id = relationship.RelatedEntityId,
                            Name = toEntityName,
                            ExternalId = toEntityName,
                            LongName = toEntityLongName,
                            EntityTypeId = toEntityTypeId,
                            EntityTypeName = toEntityTypeName,
                            EntityTypeLongName = toEntityTypeLongName,
                            ContainerId = toContainerId,
                            ContainerName = toContainerName,
                            ContainerLongName = toContainerLongName,
                            ParentEntityId = toParentEntityId,
                            ParentEntityName = toParentEntityName,
                            ParentEntityLongName = toParentEntityLongName
                        };
                    }
                    else if (module == MDMCenterModules.Export)
                    {
                        if (reader["Path"] != null)
                        {
                            relationship.Path = reader["Path"].ToString();
                        }

                        //Relationship path are in format of [Parent]-[Child] so replace underscore(_) with hyphen(-)
                        relationship.Path = relationship.Path.Replace("_", "-");

                        if (reader["Action"] != null)
                        {
                            String strAction = reader["Action"].ToString();
                            relationship.Action = GetAction(strAction);
                        }
                    }

                    relationship.ToExternalId = toEntityName;
                    relationship.ToLongName = toEntityLongName;
                    relationship.ToEntityTypeId = toEntityTypeId;
                    relationship.ToEntityTypeName = toEntityTypeName;
                    relationship.ToEntityTypeLongName = toEntityTypeLongName;
                    relationship.ToCategoryPath = toCategoryPath;
                    relationship.ToContainerId = toContainerId;
                    relationship.ToContainerName = toContainerName;
                    relationship.ToContainerLongName = toContainerLongName;
                    relationship.ToParentEntityId = toParentEntityId;
                    relationship.ToParentEntityName = toParentEntityName;
                    relationship.ToParentEntityLongName = toParentEntityLongName;
                }

                if (entityCollection != null)
                {
                    //Get the entity
                    Entity entity = entityCollection.SingleOrDefault(e => e.Id == entityId);

                    if (entity != null)
                    {
                        if (relationship.RelationshipParentId > 0)
                        {
                            entity.Relationships.AddChildRelationshipForParent(relationship.RelationshipParentId, relationship);

                            if (updateCache && relationshipsDictionary.ContainsKey(entityId))
                            {
                                relationshipsDictionary[entityId].AddChildRelationshipForParent(relationship.RelationshipParentId, relationship.CloneBaseProperties());
                            }
                        }
                        else
                        {
                            entity.Relationships.Add(relationship);

                            if (updateCache)
                            {
                                RelationshipCollection relationships = null;

                                relationshipsDictionary.TryGetValue(entityId, out relationships);

                                if (relationships == null)
                                {
                                    relationshipsDictionary.Add(entityId, new RelationshipCollection() { relationship.CloneBaseProperties() });
                                }
                                else
                                {
                                    relationships.Add(relationship.CloneBaseProperties());
                                }
                            }
                        }
                    }
                }
                else if (module == MDMCenterModules.Denorm)
                {
                    Boolean isRelationshipDeleted = false;
                    Int64 categoryId = 0;
                    Int32 entityTypeId = 0;

                    relationship.RelationshipSourceId = relationship.Id;

                    if (reader["DeleteFlag"] != null)
                    {
                        isRelationshipDeleted = ValueTypeHelper.BooleanTryParse(reader["DeleteFlag"].ToString(), isRelationshipDeleted);
                    }

                    if (reader["FK_EntityType"] != null)
                    {
                        entityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_EntityType"].ToString(), 0);
                    }

                    if (reader["CategoryId"] != null)
                    {
                        categoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), 0);
                    }

                    relationship.RelationshipProcessingContext = new RelationshipProcessingContext(entityId, categoryId, entityTypeId);

                    if (isRelationshipDeleted)
                    {
                        relationship.Action = ObjectAction.Delete;
                    }
                    else
                    {
                        //Relationship is not deleted..
                        //But whenever module says Denorm the relationships being read are going to be used for processing directly.
                        //Hence setting action to 'Update' here.
                        relationship.Action = ObjectAction.Update;
                    }

                    if (relationship.RelationshipParentId > 0)
                    {
                        Relationship parentRelationship = (Relationship)allRelationshipsInHierarchyStructure.GetRelationshipById(relationship.RelationshipParentId);

                        if (parentRelationship != null)
                        {
                            relationship.Level = (Int16)(parentRelationship.Level + 1);
                            parentRelationship.RelationshipCollection.Add(relationship);
                        }
                        else
                        {
                            //Parent relationship is not available
                            //Ignore this relationship
                            relationship.Action = ObjectAction.Ignore;
                        }
                    }
                    else if (relationship.Level == 1)
                    {
                        allRelationshipsInHierarchyStructure.Add(relationship);
                    }
                    else
                    {
                        //Parent relationship Id is not available and also relationship level is greater than one..
                        //This relationship case is not possible..
                        //Hence ignoring
                        relationship.Action = ObjectAction.Ignore;
                    }
                }

                #endregion

                if (relationship.Action != ObjectAction.Ignore)
                {
                    allRelationshipsInFlatStructure.Add(relationship);
                }
            }

            ReadRelationshipAttributes(reader, allRelationshipsInFlatStructure, attributeModels, module, entityCollection, ref isRelationshipComplexAttributesReadRequired, relationshipsAttributesDictionary, updateCache, relationshipContext);

            //TODO:: No point reading complex attributes as relationships 

            return allRelationshipsInHierarchyStructure;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="relationships"></param>
        /// <param name="attributeModels"></param>
        /// <param name="module"></param>
        /// <param name="entityCollection"></param>
        /// <param name="isRelationshipComplexAttributesReadRequired"></param>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="updateCache"></param>
        /// <param name="relationshipContext"></param>
        private static void ReadRelationshipAttributes(IDataReader reader, RelationshipCollection relationships, AttributeModelCollection attributeModels, MDMCenterModules module, EntityCollection entityCollection, ref Boolean isRelationshipComplexAttributesReadRequired, Dictionary<String, AttributeCollection> relationshipsAttributesDictionary = null, Boolean updateCache = false, RelationshipContext relationshipContext = null)
        {
            //KEY NOTE: Relationship attributes do not have complex attributes so no reading for relationship attribute having type = complex

            //Move reader to relationship attribute result set
            reader.NextResult();

            #region Get System Data Locale, use attribute model only from system data locale

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            #endregion

            while (reader.Read())
            {
                #region Step: Declare Local variables

                Int64 id = 1;
                Int64 fromEntityId = 0;
                Int64 relationshipId = 0;
                Int32 attributeId = 0;
                String attributeValue = String.Empty;
                Int32 valueRefId = -1;
                Int32 uomId = -1;
                String uom = String.Empty;
                Int32 sequence = 0;
                AttributeValueSource sourceFlag = AttributeValueSource.Overridden;
                Int32 localeId = 0;
                LocaleEnum locale = LocaleEnum.en_WW;
                ObjectAction action = ObjectAction.Read;
                Boolean isInvalidData = false;
                Int64 auditRefId = -1;

                #endregion

                #region Step: Read relationship detail from attribute Row

                if (reader["Id"] != null)
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), id);

                if (reader["FK_Attribute"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), attributeId);

                if (reader["FK_Relationship"] != null)
                    relationshipId = ValueTypeHelper.Int64TryParse(reader["FK_Relationship"].ToString(), relationshipId);

                if (reader["FK_Cnode"] != null)
                    Int64.TryParse(reader["FK_Cnode"].ToString(), out fromEntityId);

                //For relationship export, Uniqueness was considered to be Relationship Id but in case of an item having relationship and when same item is MDLed,
                //the relationship is also MDLed but Relationship Id is not unique any more.Now for this scenario, Relationship Id and Entity Id should be uniqueness.
                //Get the entity
                Relationship relationship = relationships.SingleOrDefault(r => r.Id == relationshipId && ValueTypeHelper.Int64TryParse(r.ExtendedProperties, 0) == fromEntityId);

                #endregion

                if (relationship != null)
                {
                    #region Step: Read Attribute value Properties and create value object

                    //Read other parameters
                    if (reader["AttrVal"] != null)
                        attributeValue = reader["AttrVal"].ToString();

                    if (reader["WSID"] != null)
                        Int32.TryParse(reader["WSID"].ToString(), out valueRefId);

                    if (reader["FK_UOM"] != null)
                        Int32.TryParse(reader["FK_UOM"].ToString(), out uomId);

                    if (reader["Seq"] != null)
                    {
                        Decimal decSeq = 0;
                        Decimal.TryParse(reader["Seq"].ToString(), out decSeq);
                        sequence = (Int32)decSeq;
                    }

                    if (reader["SRCFlag"] != null)
                    {
                        String strSourceFlag = reader["SRCFlag"].ToString();
                        sourceFlag = Utility.GetSourceFlagEnum(strSourceFlag);
                    }

                    if (reader["FK_Locale"] != null)
                        Int32.TryParse(reader["FK_Locale"].ToString(), out localeId);

                    locale = (LocaleEnum)localeId;

                    if (module == MDMCenterModules.Denorm)
                    {
                        //Whenever module says Denorm the relationship attributes being read are going to be used for processing directly.
                        //Hence setting action to 'Update' here.
                        action = ObjectAction.Update;
                    }

                    if (module != MDMCenterModules.Export)
                    {
                        if (reader["IsInvalidData"] != null)
                        {
                            isInvalidData = ValueTypeHelper.BooleanTryParse(reader["IsInvalidData"].ToString(), false);
                        }

                        if (reader["AuditRefId"] != null)
                        {
                            auditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefId"].ToString(), auditRefId);
                        }
                    }

                    //Create the value object
                    Value value = new Value();
                    value.Id = id;
                    value.AttrVal = attributeValue;
                    value.Locale = locale;
                    value.Sequence = sequence;
                    value.Uom = uom;
                    value.UomId = uomId;
                    value.ValueRefId = valueRefId;
                    value.HasInvalidValue = isInvalidData;

                    #endregion

                    Attribute attribute = null;

                    //Check whether the attribute has already been added
                    IAttribute iAttribute = relationship.RelationshipAttributes.GetAttribute(attributeId, locale);

                    if (iAttribute != null)
                    {
                        attribute = (Attribute)iAttribute;

                        //HACK>>THERE IS POSSIBLITY THAT ATTRIBUTE RETURNED IS NOT IN THE LOCALE WE HAVE LOOKED FOR...THUS MAKE SURE THE OBJECT IS EXACT MATCH FOR LOCALE..
                        if (attribute.Locale != locale)
                        {
                            attribute = null;
                        }
                    }

                    if (attribute != null)
                    {
                        #region Step: Attribute object already exist in entity object, just add value object in that attribute object's value collection

                        //Yes.. attribute has already been added
                        //Request is for another value.. 
                        if (attribute.IsComplex)
                        {
                            IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                            attribute.AddComplexChildRecordAt(sequence, iAttributeCollection, valueRefId, sourceFlag, locale);
                        }
                        else
                        {
                            //Add the value
                            if (sourceFlag == AttributeValueSource.Inherited)
                            {
                                if (attribute.OverriddenValues == null || attribute.OverriddenValues.Count < 1)
                                {
                                    attribute.SourceFlag = AttributeValueSource.Inherited;
                                }

                                attribute.AppendInheritedValue(value);
                            }
                            else
                            {
                                attribute.SourceFlag = sourceFlag;
                                attribute.AppendValue(value);
                                
                            }

                            attribute.AuditRefId = auditRefId;
                            value.Action = action;
                            attribute.Action = action;
                        }

                        PopulateRelationshipAttributesDictionary(relationshipsAttributesDictionary, updateCache, attribute, relationshipContext, relationship.Id);

                        #endregion
                    }
                    else
                    {
                        #region Step: Create new Attribute object and store value object in that

                        //New attribute.. which needs to be added into the entity

                        //Get the attribute model for this attribute Id
                        AttributeModel attributeModel = null;

                        if (attributeModels != null)
                        {
                            if (module == MDMCenterModules.Export || module == MDMCenterModules.Denorm)
                            {
                                attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, systemDataLocale);
                            }
                            else
                            {
                                attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);
                            }
                        }

                        if (attributeModel != null)
                        {
                            //Instantiate attribute object
                            attribute = new Attribute(attributeModel, locale);

                            if (attribute.IsComplex)
                            {
                                isRelationshipComplexAttributesReadRequired = true;
                                IAttributeCollection iAttributeCollection = attribute.NewComplexChildRecord();
                                attribute.AddComplexChildRecord(iAttributeCollection, valueRefId, sourceFlag, locale);
                            }
                            else
                            {
                                attribute.SourceFlag = sourceFlag;

                                //Add the value
                                if (sourceFlag == AttributeValueSource.Inherited)
                                    attribute.SetInheritedValueInvariant(value);
                                else
                                    attribute.SetValueInvariant(value);

                                attribute.Action = ObjectAction.Read;
                            }

                            attribute.Action = action;
                            value.Action = action;
                            attribute.AuditRefId = auditRefId;

                            relationship.RelationshipAttributes.Add(attribute, true);

                            PopulateRelationshipAttributesDictionary(relationshipsAttributesDictionary, updateCache, attribute, relationshipContext, relationship.Id);
                        }

                        #endregion
                    }
                }
            }

            //ExtendedProperties was modified to hold FromEntityId in process to export. This is being used as a temporary place holder. 
            //Hence, we are reassigning value of ExtendedProperties after filling rel attribute values.
            if (relationships != null)
            {
                relationships.ToList().ForEach(r => r.ExtendedProperties = String.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        /// <param name="module"></param>
        private static void ReadComplexAttributes(IDataReader reader, EntityCollection entityCollection, MDMCenterModules module)
        {
            //all complex attributes comes as separate table..
            //keep moving to result sets to reach each complex attribute

            while (reader.NextResult())
            {
                while (reader.Read())
                {
                    #region Step: Declare local variables

                    IEnumerable<Attribute> complexAttributesInReaderContext = null;
                    Int64 entityId = 0;
                    Int32 complexAttributeId = 0;
                    Int32 childAttributeId = 0;
                    String attributeValue = String.Empty;
                    Int32 instanceRefId = 0;
                    Int32 uomId = 0;
                    String uom = String.Empty;
                    Int32 localeId = 0;
                    LocaleEnum locale = LocaleEnum.en_WW;
                    ObjectAction action = ObjectAction.Read;

                    #endregion

                    #region Step: Read Complex attribute info

                    if (reader["FK_AttributeParent"] != null)
                        Int32.TryParse(reader["FK_AttributeParent"].ToString(), out complexAttributeId);
                    if (reader["FK_CNode"] != null)
                        Int64.TryParse(reader["FK_CNode"].ToString(), out entityId);

                    if (complexAttributesInReaderContext == null)
                    {
                        IEntity filterEntity = entityCollection.GetEntity(entityId);

                        if (filterEntity != null)
                        {
                            complexAttributesInReaderContext = filterEntity.GetAttributes().Where(ca => ca.Id == complexAttributeId);
                        }
                    }

                    if (complexAttributesInReaderContext == null || !complexAttributesInReaderContext.Any())
                        break;

                    #endregion

                    #region Step: Loop through each complex child attribute schema and read the data from reader

                    foreach (Attribute complexAttribute in complexAttributesInReaderContext)
                    {
                        //Read instance ref Id
                        if (reader["InstanceRefId"] != null)
                            Int32.TryParse(reader["InstanceRefId"].ToString(), out instanceRefId);

                        Attribute complexAttributeInstance = (Attribute)complexAttribute.GetComplexAttributeInstanceByInstanceRefId(instanceRefId);

                        if (complexAttributeInstance != null) //TODO::Need to check for child attributes present or not.. They should be added while Inserting instance record. If not present what action to take?
                        {
                            //We need to set action of instance record same as main record
                            complexAttributeInstance.Action = complexAttribute.Action;

                            //Read child attribute Id
                            if (reader["FK_Attribute"] != null)
                                Int32.TryParse(reader["FK_Attribute"].ToString(), out childAttributeId);

                            Attribute childAttribute = (Attribute)complexAttributeInstance.GetChildAttributes().GetAttribute(childAttributeId, complexAttributeInstance.Locale);

                            if (childAttribute != null) //TODO:: If attribute is null what action to take?
                            {
                                #region Step: Read Child attribute properties

                                //Read value related properties
                                if (reader["AttrVal"] != null)
                                    attributeValue = reader["AttrVal"].ToString();

                                if (reader["FK_UOM"] != null)
                                    Int32.TryParse(reader["FK_UOM"].ToString(), out uomId);

                                if (reader["UOM"] != null)
                                    uom = reader["UOM"].ToString();

                                if (reader["FK_Locale"] != null)
                                    Int32.TryParse(reader["FK_Locale"].ToString(), out localeId);

                                if (reader["Locale"] != null)
                                {
                                    String strLocale = reader["Locale"].ToString();

                                    if (!String.IsNullOrWhiteSpace(strLocale))
                                        Enum.TryParse<LocaleEnum>(strLocale, out locale);
                                }

                                if (module == MDMCenterModules.Export)
                                {
                                    action = complexAttributeInstance.Action;
                                }

                                #endregion

                                #region Step: Create value object and assign it to child attribute

                                //Create the value object
                                Value value = new Value();
                                value.AttrVal = attributeValue;
                                value.Locale = locale;
                                value.Uom = uom;
                                value.UomId = uomId;
                                value.ValueRefId = 0;

                                if (childAttribute.IsLookup == true)
                                {
                                    if (value.AttrVal != null)
                                    {
                                        value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), value.ValueRefId);
                                    }
                                }

                                //Add value
                                if (childAttribute.SourceFlag == AttributeValueSource.Inherited)
                                    childAttribute.SetInheritedValueInvariant(value);
                                else
                                    childAttribute.SetValueInvariant(value);

                                childAttribute.Action = ObjectAction.Read;

                                childAttribute.Action = action;
                                value.Action = action;

                                #endregion
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityCollection"></param>
        private static void ReadEntityBusinessConditions(IDataReader reader, EntityCollection entityCollection)
        {
            Dictionary<Int64, BusinessConditionStatusCollection> businessConditionStatuses = ReadBusinessConditions(reader);

            foreach (KeyValuePair<Int64, BusinessConditionStatusCollection> businessConditionStatus in businessConditionStatuses)
            {
                Entity entity = (Entity)entityCollection.GetEntity(businessConditionStatus.Key);

                if (entity != null)
                {
                    entity.BusinessConditions = businessConditionStatus.Value;
                }
            }
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="complexAttributeInstanceDictionary"></param>
        /// <param name="module"></param>
        private static void ReadComplexAttributesWithFlatData(IDataReader reader, Dictionary<String, Collection<AttributeAndAttributeModelPair>> complexAttributeInstanceDictionary, MDMCenterModules module)
        {
            //all complex attributes comes as separate table..
            //keep moving to result sets to reach each complex attribute
            while (reader.NextResult())
            {
                while (reader.Read())
                {
                    #region Step: Read Complex attribute record identifying info(attribute id and instance ref id) from reader

                    Int32 complexAttributeId = 0;
                    Int32 instanceRefId = 0;

                    if (reader["FK_Attribute"] != null)
                        Int32.TryParse(reader["FK_Attribute"].ToString(), out complexAttributeId);

                    if (reader["InstanceRefId"] != null)
                        Int32.TryParse(reader["InstanceRefId"].ToString(), out instanceRefId);

                    #endregion

                    String complexInstanceKey = String.Format("CxAttrInstance_A{0}_RefId{1}", complexAttributeId, instanceRefId);

                    if (complexAttributeInstanceDictionary.ContainsKey(complexInstanceKey))
                    {
                        var complexAttributeInstances = complexAttributeInstanceDictionary[complexInstanceKey];

                        if (complexAttributeInstances != null && complexAttributeInstances.Count > 0)
                        {
                            #region Step: Read complex child columns for current instance ref id from the data reader and populate Value object dictionary

                            Dictionary<Int32, Value> complexChildAttributeValues = ReadComplexChildAttributeValues(reader, complexAttributeInstances.First().AttributeModel);

                            #endregion

                            #region Step: Apply value objects(after cloning) to each complex instanace sharing same instance ref id

                            if (complexChildAttributeValues != null && complexChildAttributeValues.Count > 0)
                            {
                                foreach (AttributeAndAttributeModelPair attributeAndAttributeModelPair in complexAttributeInstances)
                                {
                                    #region Step: Get complex attribute instance and attribute model

                                    Attribute complexAttributeInstance = attributeAndAttributeModelPair.Attribute;
                                    AttributeModel complexAttributeModel = attributeAndAttributeModelPair.AttributeModel;

                                    if (complexAttributeInstance == null || complexAttributeModel == null)
                                        continue;

                                    #endregion

                                    #region Step: Loop through child attributes complex attribute instance record and update value object for them

                                    var action = module == MDMCenterModules.Export ? complexAttributeInstance.Action : ObjectAction.Read;

                                    foreach (Attribute childAttribute in complexAttributeInstance.Attributes)
                                    {
                                        Value childValueFromReader = complexChildAttributeValues[childAttribute.Id];

                                        if (childValueFromReader != null)
                                        {
                                            Value value = childValueFromReader.Clone();

                                            if (childAttribute.SourceFlag == AttributeValueSource.Inherited)
                                                childAttribute.SetInheritedValueInvariant(value);
                                            else
                                                childAttribute.SetValueInvariant(value);

                                            value.Locale = complexAttributeInstance.Locale;
                                            value.Action = action;
                                        }

                                        childAttribute.Action = action;
                                    }
                                    #endregion
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads complex child attributes value from attribute model
        /// </summary>
        /// <param name="reader">Indicates sql reader holding attribute model data stream</param>
        /// <param name="complexAttributeModel">Indicates attribute model of complex attribute</param>
        private static Dictionary<Int32, Value> ReadComplexChildAttributeValues(IDataReader reader, AttributeModel complexAttributeModel)
        {
            var childAttributesValues = new Dictionary<Int32, Value>();

            if (complexAttributeModel == null)
                return childAttributesValues;

            foreach (var childAttributeModel in complexAttributeModel.AttributeModels)
            {
                String attributeValue = String.Empty;
                Int32 uomId = -1;
                String uom = String.Empty;

                String columnName = Regex.Replace(childAttributeModel.Name, Constants.ALLOWED_CHAR_REGEX_PATTERN, "_");

                if (complexAttributeModel.IsHierarchical)
                {
                    columnName = "AttrVal"; // All HDT (Hierarchical Data Type) attrs are stored in on table with AttrVal column
                }

                if (reader[columnName] != null)
                {
                    attributeValue = Convert.ToString(reader[columnName], System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrWhiteSpace(childAttributeModel.UomType))
                {
                    String uomIdColumnName = String.Format(Constants.UOM_COLUMN_ID_FORMAT, columnName);

                    //Read value related properties
                    if (ColumnExists(reader, uomIdColumnName))
                        Int32.TryParse(reader[uomIdColumnName].ToString(), out uomId);
                }

                #region Step: Create value object and add it into return dictionary

                //Create the value object
                var value = new Value { AttrVal = attributeValue, Uom = uom, UomId = uomId, ValueRefId = 0 };

                if (childAttributeModel.IsLookup && !String.IsNullOrWhiteSpace(attributeValue))
                {
                    value.ValueRefId = ValueTypeHelper.Int32TryParse(attributeValue, value.ValueRefId);
                }

                if (complexAttributeModel.IsHierarchical)
                {
                    childAttributesValues.Add(complexAttributeModel.Id, value);
                    break;
                }

                childAttributesValues.Add(childAttributeModel.Id, value);

                #endregion
            }

            return childAttributesValues;
        }

        /// <summary>
        /// Reads entity guids map
        /// </summary>
        /// <param name="reader"></param>
        public static Dictionary<Guid, Int64> ReadEntityGuidsMap(IDataReader reader)
        {
            Dictionary<Guid, Int64> entityIdsMap = new Dictionary<Guid, Int64>();

            while (reader.Read())
            {
                Int64 entityId = 0;
                Guid entityGuid;

                if (reader["EntityId"] != null)
                {
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                }

                if (reader["EntityGUID"] != null)
                {
                    entityGuid = new Guid(reader["EntityGUID"].ToString());
                    entityIdsMap.Add(entityGuid, entityId);
                }
            }

            return entityIdsMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static ObjectAction GetAction(String action)
        {
            ObjectAction objectAction = ObjectAction.Read;

            if (!String.IsNullOrWhiteSpace(action))
            {
                switch (action.ToUpperInvariant())
                {
                    case "ADD":
                        objectAction = ObjectAction.Create;
                        break;
                    case "UPDATE":
                        objectAction = ObjectAction.Update;
                        break;
                    case "DELETE":
                        objectAction = ObjectAction.Delete;
                        break;
                    case "NOCHANGE":
                        objectAction = ObjectAction.Read;
                        break;
                    case "MOVE":
                        objectAction = ObjectAction.Reclassify;
                        break;
                    default:
                        Enum.TryParse<ObjectAction>(action, out objectAction);
                        break;
                }
            }

            return objectAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeCollection"></param>
        /// <param name="attributeTable"></param>
        /// <param name="attributeMetaData"></param>
        /// <param name="entityId"></param>
        /// <param name="referenceId"></param>
        /// <param name="mdmCenterModule"></param>
        /// <param name="parentInstanceRefId"></param>
        /// <param name="parentSequence"></param>
        /// <param name="isComplex"></param>
        private static void CreateEntityAttributeTable(IAttributeCollection attributeCollection, ICollection<SqlDataRecord> attributeTable, SqlMetaData[] attributeMetaData, Int64 entityId, Int64 referenceId, MDMCenterModules mdmCenterModule, Int32 parentInstanceRefId = -1, Decimal parentSequence = -1, Boolean isComplex = false)
        {
            foreach (var attribute in attributeCollection)
            {
                if (attribute.Action != ObjectAction.Create && attribute.Action != ObjectAction.Update && attribute.Action != ObjectAction.Delete)
                    continue;

                String attrModelType = attribute.AttributeModelType.ToString();
                if (mdmCenterModule == MDMCenterModules.Denorm)
                {
                    // Take only 'C', or 'T' from the attrmodel type
                    if (attribute.AttributeModelType == AttributeModelType.Common)
                        attrModelType = "C";
                    else if (attribute.AttributeModelType == AttributeModelType.Category)
                        attrModelType = "T";
                    else if (attribute.AttributeModelType == AttributeModelType.Relationship)
                        attrModelType = "R";
                    else if (attribute.AttributeModelType == AttributeModelType.MetaDataAttribute)
                        attrModelType = "W";
                    else if (attribute.AttributeModelType == AttributeModelType.System)
                        attrModelType = "S";
                }

                if (attribute.IsHierarchical)
                {
                    HierarchicalAttribute.UpdateAttributePerAction(attribute);

                    var values = mdmCenterModule == MDMCenterModules.Denorm ? attribute.GetCurrentValuesInvariant() : attribute.GetOverriddenValuesInvariant();

                    if (values == null || values.Count == 0)
                    {
                        attribute.Action = ObjectAction.Delete;
                    }
                }

                //If attribute action is delete then pass the attribute as Action = Delete
                //For Denorm module delete, we need to pass attr record for each value in attribute with their PK DNI Attrval thus it goes to else flow only
                if ((!attribute.IsCollection || attribute.IsComplex == true) && attribute.Action == ObjectAction.Delete && mdmCenterModule != MDMCenterModules.Denorm)
                {
                    IValueCollection iValues = attribute.GetOverriddenValuesInvariant();

                    Int64 valueId = -1;

                    if (iValues != null && iValues.Count > 0)
                    {
                        var firstValue = iValues.FirstOrDefault();

                        if (firstValue != null)
                        {
                            valueId = firstValue.Id;
                        }
                    }

                    var value = new Value(attribute);

                    Decimal? numericValue = value.GetNumericValue();

                    SqlDataRecord attributeRecord = new SqlDataRecord(attributeMetaData);

                    attributeRecord.SetValue(0, valueId);
                    attributeRecord.SetValue(1, entityId);
                    attributeRecord.SetValue(2, attribute.Id);
                    attributeRecord.SetValue(3, attribute.AttributeParentId);
                    attributeRecord.SetValue(4, attribute.InstanceRefId);
                    attributeRecord.SetValue(5, String.Empty);
                    attributeRecord.SetValue(6, String.Empty);
                    attributeRecord.SetValue(7, numericValue);
                    attributeRecord.SetValue(8, default(DateTime?));
                    attributeRecord.SetValue(9, default(Int32));
                    attributeRecord.SetValue(10, (int)attribute.Locale);
                    attributeRecord.SetValue(11, default(Int32));
                    attributeRecord.SetValue(12, String.Empty);
                    attributeRecord.SetValue(13, default(Decimal));
                    attributeRecord.SetValue(14, attribute.IsCollection);
                    attributeRecord.SetValue(15, attribute.IsComplex);
                    attributeRecord.SetValue(16, attribute.Action.ToString());
                    attributeRecord.SetValue(17, attrModelType);
                    attributeRecord.SetValue(18, attribute.SourceEntityId);
                    attributeRecord.SetValue(19, attribute.SourceClass);
                    attributeRecord.SetValue(20, Utility.GetSourceFlagString(attribute.SourceFlag));
                    attributeRecord.SetValue(21, attribute.AuditRefId);
                    attributeRecord.SetValue(22, referenceId);
                    attributeRecord.SetValue(23, attribute.HasInvalidOverriddenValues);
                    attributeRecord.SetValue(24, attribute.UserName);
                    attributeRecord.SetValue(25, attribute.ProgramName);

                    attributeTable.Add(attributeRecord);
                }
                else
                {
                    var values = mdmCenterModule == MDMCenterModules.Denorm ? attribute.GetCurrentValuesInvariant() : attribute.GetOverriddenValuesInvariant();

                    if (values != null && values.Count > 0)
                    {
                        foreach (Value value in values)
                        {
                            if (value.Action == ObjectAction.Ignore || (attribute.IsComplex && attribute.Action != ObjectAction.Delete && value.Action == ObjectAction.Delete))
                            {
                                continue;
                            }

                            ObjectAction valueAction = attribute.Action;

                            if (attribute.IsCollection && !attribute.IsComplex)
                            {
                                valueAction = value.Action;
                            }

                            Int32 valueRefId = isComplex ? parentInstanceRefId : value.ValueRefId;
                            Decimal sequence = isComplex ? parentSequence : value.Sequence;

                            SqlDataRecord attributeRecord = new SqlDataRecord(attributeMetaData);
                            attributeRecord.SetInt64(0, value.Id);
                            attributeRecord.SetInt64(1, entityId);
                            attributeRecord.SetInt32(2, attribute.Id);
                            attributeRecord.SetInt32(3, attribute.AttributeParentId);
                            attributeRecord.SetInt32(4, attribute.InstanceRefId);

                            if (attribute.IsHierarchical)
                            {
                                var childAttributes = attribute.GetComplexChildAttributesBySequence(sequence);

                                valueAction = value.Action;

                                if (childAttributes != null && childAttributes.Count > 0)
                                {
                                    attributeRecord.SetString(5, childAttributes.ToXml(Constants.HIERARCHICAL_ATTRIBUTE_SERIALIZATION));
                                }
                            }
                            else if (attribute.IsComplex == true && mdmCenterModule == MDMCenterModules.Denorm && value.HasInvalidValue == false)
                            {
                                attributeRecord.SetString(5, value.ValueRefId.ToString());
                            }
                            else
                            {
                                if (attribute.IsComplex == true && value.HasInvalidValue == true)
                                {
                                    //attributeRecord.SetString(5, attribute.ToXml());
                                    Attribute instanceAttribute = (Attribute)attribute.GetComplexAttributeInstanceByInstanceRefId(value.ValueRefId);
                                    if (instanceAttribute != null)
                                    {
                                        attributeRecord.SetString(5, instanceAttribute.ToXmlInvariant());
                                    }
                                }
                                else if (value.AttrVal != null)
                                {
                                    attributeRecord.SetString(5, value.AttrVal.ToString());
                                }
                            }

                            if (attribute.IsLookup && !String.IsNullOrEmpty(value.GetExportValue()))
                            {
                                String maskVal = value.GetExportValue();
                                attributeRecord.SetString(6, maskVal);
                            }
                            else
                            {
                                attributeRecord.SetDBNull(6);
                            }

                            if (attribute.AttributeDataType == AttributeDataType.Integer
                                || attribute.AttributeDataType == AttributeDataType.Decimal
                                || attribute.AttributeDataType == AttributeDataType.Fraction)
                            {
                                if (value.NumericVal == null)
                                    attributeRecord.SetDBNull(7);
                                else
                                    attributeRecord.SetDecimal(7, (Decimal)value.NumericVal);
                            }
                            else
                            {
                                attributeRecord.SetDBNull(7);
                            }

                            if (attribute.AttributeDataType == AttributeDataType.Date || attribute.AttributeDataType == AttributeDataType.DateTime)
                            {
                                if (value.DateVal == null)
                                    attributeRecord.SetDBNull(8);
                                else
                                    attributeRecord.SetDateTime(8, (DateTime)value.DateVal);
                            }
                            else
                            {
                                attributeRecord.SetDBNull(8);
                            }

                            attributeRecord.SetInt32(9, valueRefId);
                            attributeRecord.SetInt32(10, (int)attribute.Locale);
                            attributeRecord.SetInt32(11, value.UomId);
                            attributeRecord.SetString(12, value.Uom);
                            attributeRecord.SetDecimal(13, sequence);
                            attributeRecord.SetBoolean(14, attribute.IsCollection);
                            attributeRecord.SetBoolean(15, attribute.IsComplex);
                            attributeRecord.SetString(16, valueAction.ToString());
                            attributeRecord.SetString(17, attrModelType);
                            attributeRecord.SetInt64(18, attribute.SourceEntityId);
                            attributeRecord.SetInt32(19, attribute.SourceClass);
                            attributeRecord.SetString(20, Utility.GetSourceFlagString(attribute.SourceFlag));
                            attributeRecord.SetInt64(21, attribute.AuditRefId);
                            attributeRecord.SetInt64(22, referenceId);
                            attributeRecord.SetBoolean(23, value.HasInvalidValue);
                            attributeRecord.SetValue(24, attribute.UserName);
                            attributeRecord.SetValue(25, attribute.ProgramName);

                            attributeTable.Add(attributeRecord);
                        }
                    }

                    // Move on to next attribute as hierarchical data type attribute 
                    // doesn't require child attribute processing like complex attribute.
                    if (attribute.IsHierarchical)
                    {
                        continue;
                    }

                    if (attribute.IsComplex && mdmCenterModule != MDMCenterModules.Denorm)
                    {
                        var childAttributes = attribute.GetChildAttributes();

                        if (childAttributes != null && childAttributes.Count > 0)
                        {
                            foreach (Attribute childAttribute in childAttributes)
                            {
                                if (childAttribute.HasInvalidValues)
                                    continue;

                                var grandChildAttributes = childAttribute.GetChildAttributes();

                                if (grandChildAttributes != null && grandChildAttributes.Count > 0)
                                {
                                    CreateEntityAttributeTable(grandChildAttributes, attributeTable,
                                                                     attributeMetaData, entityId, referenceId, mdmCenterModule,
                                                                     childAttribute.InstanceRefId,
                                                                     childAttribute.Sequence, true);
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracting the relationship and its attribute properties and returned as relationship table and attribute table
        /// </summary>
        /// <param name="relationships">Collection of relationships</param>
        /// <param name="entity">Entity for given relationships</param>
        /// <param name="relationshipMetadata">Relationship Meta data</param>
        /// <param name="attributeMetaData">Attribute Meta data</param>
        /// <param name="relationshipTable">Relationship table - Relationship properties extracted and placed in this table</param>
        /// <param name="attributeTable">Attribute table - Attribute properties extracted and placed in this table</param>
        /// <param name="generateAttributesTable">Indicates whether to generate RelationshipAttributes table or not. For Relationship initial load, we process attributes separately. In that case this value will be false.</param>
        /// <param name="module">Name of the module</param>
        /// <param name="isRecursive">Name of the module</param>
        private static void CreateRelationshipTable(RelationshipCollection relationships, Entity entity, SqlMetaData[] relationshipMetadata, SqlMetaData[] attributeMetaData, List<SqlDataRecord> relationshipTable, List<SqlDataRecord> attributeTable, Boolean generateAttributesTable, MDMCenterModules module, Boolean isRecursive = true)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.FromEntityId < 1 && relationship.Level == Constants.RELATIONSHIP_LEVEL_ONE)
                {
                    relationship.FromEntityId = entity.Id;
                }

                SqlDataRecord relationshipRecord = new SqlDataRecord(relationshipMetadata);
                relationshipRecord.SetValue(0, relationship.Id);
                relationshipRecord.SetValue(1, entity.Id);
                relationshipRecord.SetValue(2, relationship.ContainerId);
                relationshipRecord.SetValue(3, relationship.RelationshipTypeId);
                relationshipRecord.SetValue(4, relationship.FromEntityId);
                relationshipRecord.SetValue(5, relationship.RelatedEntityId);
                relationshipRecord.SetValue(6, (Int32)relationship.Level);
                relationshipRecord.SetValue(7, relationship.RelationshipSourceId);
                relationshipRecord.SetValue(8, relationship.RelationshipParentId);
                relationshipRecord.SetValue(9, (Byte)relationship.InheritanceMode);
                relationshipRecord.SetValue(10, (Byte)relationship.Status);
                relationshipRecord.SetValue(11, false);
                relationshipRecord.SetValue(12, relationship.Action.ToString());

                if (generateAttributesTable == true &&
                    relationship.Action != ObjectAction.Delete && relationship.Action != ObjectAction.Read)
                {
                    CreateRelationshipAttributeTable(relationship.RelationshipAttributes, relationship, entity, attributeTable, attributeMetaData, module);
                }

                if (relationship.Action != ObjectAction.Read)
                {
                    relationshipTable.Add(relationshipRecord);
                }

                if (isRecursive && relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    CreateRelationshipTable(relationship.RelationshipCollection, entity, relationshipMetadata, attributeMetaData, relationshipTable, attributeTable, generateAttributesTable, module);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipAttributes"></param>
        /// <param name="relationship"></param>
        /// <param name="entity"></param>
        /// <param name="attributeTable"></param>
        /// <param name="attributeMetaData"></param>
        /// <param name="mdmCenterModule"></param>
        /// <param name="parentInstanceRefId"></param>
        /// <param name="parentSequence"></param>
        /// <param name="isComplex"></param>
        private static void CreateRelationshipAttributeTable(AttributeCollection relationshipAttributes, Relationship relationship, Entity entity, ICollection<SqlDataRecord> attributeTable, SqlMetaData[] attributeMetaData, MDMCenterModules mdmCenterModule, Int32 parentInstanceRefId = -1, Decimal parentSequence = -1, Boolean isComplex = false)
        {
            if (relationshipAttributes != null && relationshipAttributes.Count > 0)
            {
                foreach (Attribute relAttr in relationshipAttributes)
                {
                    if (relAttr.Action != ObjectAction.Create && relAttr.Action != ObjectAction.Update && relAttr.Action != ObjectAction.Delete)
                        continue;

                    String attrModelType = relAttr.AttributeModelType.ToString();
                    if (mdmCenterModule == MDMCenterModules.Denorm)
                    {
                        // Take only 'C', or 'T' from the attrmodel type
                        if (relAttr.AttributeModelType == AttributeModelType.Common)
                            attrModelType = "C";
                        else if (relAttr.AttributeModelType == AttributeModelType.Category)
                            attrModelType = "T";
                        else if (relAttr.AttributeModelType == AttributeModelType.Relationship)
                            attrModelType = "R";
                        else if (relAttr.AttributeModelType == AttributeModelType.MetaDataAttribute)
                            attrModelType = "W";
                        else if (relAttr.AttributeModelType == AttributeModelType.System)
                            attrModelType = "S";
                    }

                    //If attribute action is delete then pass the attribute as Action = Delete
                    //For Denorm module delete, we need to pass attr record for each value in attribute with their PK DNI Attrval thus it goes to else flow only
                    if ((!relAttr.IsCollection || relAttr.IsComplex == true) && relAttr.Action == ObjectAction.Delete && mdmCenterModule != MDMCenterModules.Denorm)
                    {
                        IValueCollection iValues = relAttr.GetOverriddenValuesInvariant();

                        Int64 valueId = -1;

                        if (iValues != null && iValues.Count > 0)
                        {
                            var firstValue = iValues.FirstOrDefault();

                            if (firstValue != null)
                            {
                                valueId = firstValue.Id;
                            }
                        }

                        var value = new Value(relAttr);

                        Decimal? numericValue = value.GetNumericValue();
                        SqlDataRecord attributeRecord = new SqlDataRecord(attributeMetaData);
                        attributeRecord.SetValue(0, valueId);
                        attributeRecord.SetValue(1, relationship.Id);
                        attributeRecord.SetValue(2, entity.Id);
                        attributeRecord.SetValue(3, entity.ContainerId);
                        attributeRecord.SetValue(4, relationship.RelationshipTypeId);
                        attributeRecord.SetValue(5, relationship.FromEntityId);
                        attributeRecord.SetValue(6, relationship.RelatedEntityId);
                        attributeRecord.SetValue(7, relAttr.Id);
                        attributeRecord.SetValue(8, relAttr.AttributeParentId);
                        attributeRecord.SetValue(9, (Int32)relAttr.Locale);
                        attributeRecord.SetValue(10, default(Int32));
                        attributeRecord.SetValue(11, String.Empty);
                        attributeRecord.SetValue(12, default(Int32));
                        attributeRecord.SetValue(13, Utility.GetSourceFlagString(relAttr.SourceFlag));
                        attributeRecord.SetValue(14, default(Decimal));
                        attributeRecord.SetValue(15, numericValue);
                        attributeRecord.SetValue(16, default(DateTime?));
                        attributeRecord.SetValue(17, false);
                        attributeRecord.SetValue(18, relAttr.HasInvalidOverriddenValues);
                        attributeRecord.SetValue(19, relAttr.SourceClass);
                        attributeRecord.SetValue(20, relAttr.Action.ToString());
                        attributeRecord.SetValue(21, relAttr.UserName);
                        attributeRecord.SetValue(22, relAttr.ProgramName);
                        attributeRecord.SetValue(23, relAttr.IsCollection);
                        attributeRecord.SetValue(24, relAttr.IsComplex);

                        attributeTable.Add(attributeRecord);
                    }
                    else
                    {
                        IValueCollection values = MDMObjectFactory.GetIValueCollection();

                        if (mdmCenterModule == MDMCenterModules.Denorm)
                        {
                            values = relAttr.GetCurrentValuesInvariant();
                        }
                        else
                        {
                            values = relAttr.GetOverriddenValuesInvariant();
                        }

                        if (values != null)
                        {
                            foreach (Value value in values)
                            {
                                if (value.Action == ObjectAction.Ignore)
                                    continue;

                                ObjectAction valueAction = relAttr.Action;

                                if (relAttr.IsCollection && !relAttr.IsComplex)
                                {
                                    valueAction = value.Action;
                                }

                                Int32 valueRefId = isComplex ? parentInstanceRefId : value.ValueRefId;
                                Decimal sequence = isComplex ? parentSequence : value.Sequence;

                                SqlDataRecord attributeRecord = new SqlDataRecord(attributeMetaData);
                                attributeRecord.SetInt64(0, value.Id);
                                attributeRecord.SetInt64(1, relationship.Id);
                                attributeRecord.SetInt64(2, entity.Id);
                                attributeRecord.SetInt32(3, entity.ContainerId);
                                attributeRecord.SetInt32(4, relationship.RelationshipTypeId);
                                attributeRecord.SetInt64(5, relationship.FromEntityId);
                                attributeRecord.SetInt64(6, relationship.RelatedEntityId);
                                attributeRecord.SetInt32(7, relAttr.Id);
                                attributeRecord.SetInt32(8, relAttr.AttributeParentId);
                                attributeRecord.SetInt32(9, (Int32)relAttr.Locale);
                                attributeRecord.SetInt32(10, value.UomId);

                                if (relAttr.IsComplex == true && mdmCenterModule == MDMCenterModules.Denorm && value.HasInvalidValue == false)
                                {
                                    attributeRecord.SetString(11, value.ValueRefId.ToString());
                                }
                                else
                                {
                                    if (relAttr.IsComplex == true && value.HasInvalidValue == true)
                                    {
                                        IAttribute instanceAttribute = relAttr.GetComplexAttributeInstanceByInstanceRefId(value.ValueRefId);
                                        if (instanceAttribute != null)
                                        {
                                            attributeRecord.SetString(11, instanceAttribute.ToXml());
                                        }
                                    }
                                    else if (value.AttrVal != null)
                                    {
                                        attributeRecord.SetString(11, value.AttrVal.ToString());
                                    }
                                }

                                attributeRecord.SetValue(12, valueRefId);
                                attributeRecord.SetString(13, Utility.GetSourceFlagString(relAttr.SourceFlag));
                                attributeRecord.SetDecimal(14, sequence);

                                if (relAttr.AttributeDataType == AttributeDataType.Integer
                                    || relAttr.AttributeDataType == AttributeDataType.Decimal
                                    || relAttr.AttributeDataType == AttributeDataType.Fraction)
                                {
                                    if (value.NumericVal == null)
                                        attributeRecord.SetDBNull(15);
                                    else
                                        attributeRecord.SetDecimal(15, (Decimal)value.NumericVal);
                                }
                                else
                                {
                                    attributeRecord.SetDBNull(15);
                                }

                                if (relAttr.AttributeDataType == AttributeDataType.Date || relAttr.AttributeDataType == AttributeDataType.DateTime)
                                {
                                    if (value.DateVal == null)
                                        attributeRecord.SetDBNull(16);
                                    else
                                        attributeRecord.SetDateTime(16, (DateTime)value.DateVal);
                                }
                                else
                                {
                                    attributeRecord.SetDBNull(16);
                                }

                                attributeRecord.SetValue(17, false);
                                attributeRecord.SetBoolean(18, value.HasInvalidValue);
                                attributeRecord.SetInt32(19, relAttr.SourceClass);
                                attributeRecord.SetString(20, valueAction.ToString());
                                attributeRecord.SetValue(21, relAttr.UserName);
                                attributeRecord.SetValue(22, relAttr.ProgramName);
                                attributeRecord.SetBoolean(23, relAttr.IsCollection);
                                attributeRecord.SetBoolean(24, relAttr.IsComplex);

                                attributeTable.Add(attributeRecord);
                            }
                        }

                        if (relAttr.IsComplex && mdmCenterModule != MDMCenterModules.Denorm)
                        {
                            if (relAttr.GetChildAttributes() != null && relAttr.GetChildAttributes().Any())
                            {
                                foreach (Attribute childAttribute in relAttr.GetChildAttributes())
                                {
                                    if (childAttribute.HasInvalidValues)
                                        continue;

                                    if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                    {
                                        CreateRelationshipAttributeTable((AttributeCollection)childAttribute.GetChildAttributes(), relationship, entity, attributeTable,
                                                                         attributeMetaData, mdmCenterModule,
                                                                         childAttribute.InstanceRefId,
                                                                         childAttribute.Sequence, true);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static bool ColumnExists(IDataReader reader, String columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="updateCache"></param>
        /// <param name="attribute"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="relationshipId"></param>
        private static void PopulateRelationshipAttributesDictionary(Dictionary<String, AttributeCollection> relationshipsAttributesDictionary, Boolean updateCache, Attribute attribute, RelationshipContext relationshipContext, Int64 relationshipId)
        {
            if (updateCache)
            {
                String key = String.Empty;

                //ASSUMPTION : If attribute localizable then add it in to attribute.locale bucket.
                //If attribute is non localizable then copy same attribute in all requested data locales to cache.
                if (attribute.IsLocalizable)
                {
                    key = CacheKeyGenerator.GetEntityRelationshipsCacheKey(relationshipId, attribute.Locale);

                    AddOrAppendRelationshipAttributesDictionary(relationshipsAttributesDictionary, attribute, key);
                }
                else
                {
                    CopyNonLocalizableAttributeInAllDataLocales(relationshipsAttributesDictionary, attribute, relationshipContext, relationshipId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="attribute"></param>
        /// <param name="relationshipContext"></param>
        /// <param name="relationshipId"></param>
        private static void CopyNonLocalizableAttributeInAllDataLocales(Dictionary<String, AttributeCollection> relationshipsAttributesDictionary, Attribute attribute, RelationshipContext relationshipContext, Int64 relationshipId)
        {
            //Copy non-localizable attribute in all locales.
            if (!attribute.IsLocalizable && relationshipContext != null && relationshipContext.DataLocales.Count > 0)
            {
                foreach (LocaleEnum dataLocale in relationshipContext.DataLocales)
                {
                    String key = CacheKeyGenerator.GetEntityRelationshipsCacheKey(relationshipId, dataLocale);
                    AddOrAppendRelationshipAttributesDictionary(relationshipsAttributesDictionary, attribute, key);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipsAttributesDictionary"></param>
        /// <param name="attribute"></param>
        /// <param name="key"></param>
        private static void AddOrAppendRelationshipAttributesDictionary(Dictionary<String, AttributeCollection> relationshipsAttributesDictionary, Attribute attribute, String key)
        {
            if (relationshipsAttributesDictionary != null)
            {
                AttributeCollection attributes = null;

                relationshipsAttributesDictionary.TryGetValue(key, out attributes);

                if (attributes == null)
                {
                    relationshipsAttributesDictionary.Add(key, new AttributeCollection() { attribute });
                }
                else
                {
                    Attribute filteredAttibute = (Attribute)attributes.GetAttribute(attribute.Id, attribute.Locale);

                    if (filteredAttibute == null)
                    {
                        attributes.Add(attribute);
                    }
                    else
                    {
                        filteredAttibute = attribute;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private struct AttributeAndAttributeModelPair
        {
            public Attribute Attribute;
            public AttributeModel AttributeModel;
        }

        /// <summary>
        /// Gets collection of system attribute ids from data reader.
        /// </summary>
        /// <param name="reader">Provides a way of reading a forward-only stream of rows from a SQL Server database.</param>
        /// <returns>collection of system attribute ids</returns>
        private static Collection<Int32> GetAttributeIdList(IDataReader reader)
        {
            Collection<Int32> attributeIds = new Collection<Int32>();

            for (Int32 i = 0; i < reader.FieldCount; i++)
            {
                String columName = reader.GetName(i);

                if (columName.Equals("FK_CNode"))
                    continue;

                attributeIds.Add(ValueTypeHelper.Int32TryParse(columName, 0));
            }

            return attributeIds;
        }

        private static void FillAttributeListTableForHierarchyGet(List<SqlDataRecord> attributeListTable, SqlMetaData[] attributeListMetaData, AttributeModelCollection attributeModels,
            Collection<Int32> distinctTechnicalAttrIds, Int64 referenceId, Int32 entityTypeId)
        {
            var distinctAttrIds = new Collection<Int32>();

            Int32 attrModelId;
            foreach (AttributeModel attrModel in attributeModels)
            {
                attrModelId = attrModel.Id;

                if (distinctAttrIds.Contains(attrModelId) || attrModel.IsComplexChild || distinctTechnicalAttrIds.Contains(attrModelId))
                {
                    continue;
                }

                SqlDataRecord attributeRecord = CreateAttributeDataRecord(attributeListMetaData, attrModel, referenceId, entityTypeId);

                attributeListTable.Add(attributeRecord);
                distinctAttrIds.Add(attrModelId);

                if (attrModel.AttributeModelType == AttributeModelType.Category)
                {
                    distinctTechnicalAttrIds.Add(attrModelId);
                }
            }
        }

        private static SqlDataRecord CreateAttributeDataRecord(SqlMetaData[] attributeListMetaData, AttributeModel attrModel, Int64 referenceId, Int32 entityTypeId = -1)
        {
            var attributeRecord = new SqlDataRecord(attributeListMetaData);
            attributeRecord.SetValue(0, (Int64)referenceId);
            attributeRecord.SetValue(1, attrModel.Id);

            if (attrModel.AttributeModelType == AttributeModelType.System)
                attributeRecord.SetValue(2, attrModel.Name);
            else
                attributeRecord.SetValue(2, null);

            attributeRecord.SetValue(3, attrModel.AttributeModelType.ToString());
            attributeRecord.SetValue(4, attrModel.IsLocalizable);
            attributeRecord.SetValue(5, attrModel.IsCollection);
            attributeRecord.SetValue(6, attrModel.IsComplex);
            attributeRecord.SetValue(7, attrModel.AttributeModelType == Core.AttributeModelType.System); // IsSystemAttribute
            attributeRecord.SetValue(8, attrModel.IsLookup);
            attributeRecord.SetValue(9, attrModel.Inheritable);

            if (attrModel.IsComplex)
            {
                attributeRecord.SetValue(10, attrModel.ComplexTableName);
                attributeRecord.SetValue(11, ValueTypeHelper.JoinColumnCollectionWithEscaping(attrModel.ComplexTableColumnNameList, ","));
            }
            else
            {
                attributeRecord.SetValue(10, String.Empty);
                attributeRecord.SetValue(11, String.Empty);
            }

            if (entityTypeId > 0)
            {
                attributeRecord.SetValue(12, entityTypeId);
            }

            if (!String.IsNullOrEmpty(attrModel.ExtendedProperties) &&
                (String.Compare(attrModel.ExtendedProperties, Constants.ATTRIBUTE_MODEL_ONLY_FOR_INHERITED_VALUE_CALCULATION) == 0))
            {
                attributeRecord.SetValue(13, true);
            }

            return attributeRecord;
        }

        private static ContextualObjectCollection<AttributeModelCollection> CreateContextualAttributeModelsForHierarchy(IDictionary<Int32, AttributeModelCollection> attributeModelsForHierarchy)
        {
            var contextualAttributeModels = new ContextualObjectCollection<AttributeModelCollection>();

            foreach (KeyValuePair<Int32, AttributeModelCollection> keyValuePair in attributeModelsForHierarchy)
            {
                contextualAttributeModels.SetObject(keyValuePair.Value, entityTypeId: keyValuePair.Key);
            }

            return contextualAttributeModels;
        }

        private static AttributeModelCollection GetAttributeModelCollectionForContext(Entity entity, ContextualObjectCollection<AttributeModelCollection> contextualAttributeModels,
            Boolean readHiearchyRelationships)
        {
            Int32 entityTypeId = readHiearchyRelationships ? entity.EntityTypeId : 0;
            AttributeModelCollection attributeModels = contextualAttributeModels.GetObjectByContext(entityTypeId: entityTypeId);
            return attributeModels;
        }

        /// <summary>
        /// Populate the entity validation states.
        /// </summary>
        /// <param name="entity">Indicates the entity which needs to be processed.</param>
        /// <param name="attribute">Indicates the validation state attribute.</param>
        private static void PopulateEntityValidationStates(Entity entity, Attribute attribute)
        {
            switch ((SystemAttributes)attribute.Id)
            {
                case SystemAttributes.EntityMetaDataValid:
                case SystemAttributes.EntityCommonAttributesValid:
                case SystemAttributes.EntityCategoryAttributesValid:
                case SystemAttributes.EntityRelationshipsValid:
                case SystemAttributes.EntityVariantValid:
                case SystemAttributes.EntityExtensionsValid:
                case SystemAttributes.EntitySelfValid:
                    SetEntityValidationStates(entity, attribute);
                    break;
            }
        }

        /// <summary>
        /// Sets the entity validation states.
        /// </summary>      
        /// <param name="entity">Indicates the entity which needs to be processed.</param>
        /// <param name="attribute">Indicates the validation state attribute.</param>
        private static void SetEntityValidationStates(Entity entity, Attribute attribute)
        {
            if (attribute.HasValue())
            {
                ValidityStateValue validationStateValue = ValidityStateValue.NotChecked;
                object validationStatesObj = attribute.GetCurrentValue();
                Boolean validationStateflag;
                if (Boolean.TryParse(validationStatesObj.ToString(), out validationStateflag))
                {
                    if (validationStateflag)
                    {
                        validationStateValue = ValidityStateValue.Valid;
                    }
                    else
                    {
                        validationStateValue = ValidityStateValue.InValid;
                    }
                }

                switch ((SystemAttributes)attribute.Id)
                {
                    case SystemAttributes.EntityMetaDataValid:
                        entity.ValidationStates.IsMetaDataValid = validationStateValue;
                        break;

                    case SystemAttributes.EntityCommonAttributesValid:
                        entity.ValidationStates.IsCommonAttributesValid = validationStateValue;
                        break;

                    case SystemAttributes.EntityCategoryAttributesValid:
                        entity.ValidationStates.IsCategoryAttributesValid = validationStateValue;
                        break;

                    case SystemAttributes.EntityRelationshipsValid:
                        entity.ValidationStates.IsRelationshipsValid = validationStateValue;
                        break;

                    case SystemAttributes.EntityVariantValid:
                        entity.ValidationStates.IsEntityVariantValid = validationStateValue;
                        break;

                    case SystemAttributes.EntityExtensionsValid:
                        entity.ValidationStates.IsExtensionsValid = validationStateValue;
                        break;

                    case SystemAttributes.EntitySelfValid:
                        entity.ValidationStates.IsSelfValid = validationStateValue;
                        break;
                }
            }
        }

        #endregion

        #endregion Methods
    }
}