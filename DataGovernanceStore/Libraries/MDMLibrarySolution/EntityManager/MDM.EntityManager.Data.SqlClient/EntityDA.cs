using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    public class EntityDA : SqlClientDataAccessBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Entity CUD

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="loginUser"></param>
        /// <param name="systemDataLocale"></param>
        /// <param name="programName"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="command"></param>
        public void Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String loginUser, LocaleEnum systemDataLocale, String programName, EntityOperationResultCollection entityOperationResults, DBCommandProperties command, CallerContext callerContext, Boolean updateEntityResults = true, Boolean updateAttributeResults = true)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            try
            {

                #region Diagnostics & Tracing

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(entityProcessingOptions.ProcessingMode)))
                {
                    SqlDataReader reader = null;
                    String storedProcedureName = "usp_EntityManager_Entity_Process";

                    // String entityDataXml = entities.ToXml(ObjectSerialization.ProcessingOnly, entityProcessingOptions.ProcessOnlyEntities);
                    List<SqlDataRecord> entityTable;
                    List<SqlDataRecord> attributeTable;

                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Process_ParametersArray");
                    SqlMetaData[] entityMetaData = generator.GetTableValueMetadata("EntityManager_Entity_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("EntityManager_Entity_Process_ParametersArray", parameters[1].ParameterName);

                    EntityDataReaderUtility.CreateEntityTable(entities, entityMetaData, attributeMetaData, out entityTable, out attributeTable, MDMCenterModules.Entity);
                    //nothing to update. return
                    if (entityTable != null)
                    {
                        if (currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Completed preparing TVP from Entity Object");
                            DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityTable", entityMetaData, entityTable);
                            DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "AttributeValueTable", attributeMetaData, attributeTable);
                        }

                        ContainerBranchLevel containerBranchLevel = ContainerBranchLevel.Component;

                        //Need to indicate to SP whether we are processing category or entity.
                        //ASSUMPTION::Bulk process will happen only if all entities belongs to the same branch level 
                        //So considering first branch level
                        if (entities.First().BranchLevel == ContainerBranchLevel.Node)
                            containerBranchLevel = ContainerBranchLevel.Node;

                        try
                        {
                            parameters[0].Value = entityTable;
                            parameters[1].Value = attributeTable;
                            parameters[2].Value = entityProcessingOptions.ProcessOnlyEntities;
                            parameters[3].Value = entityProcessingOptions.ProcessDefaultValues;
                            parameters[4].Value = containerBranchLevel;
                            parameters[5].Value = loginUser;
                            parameters[6].Value = (Int32)systemDataLocale;
                            parameters[7].Value = programName;
                            parameters[8].Value = updateEntityResults;
                            parameters[9].Value = updateAttributeResults;

                            if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                                storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                            reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                            if (updateEntityResults)
                            {
                                UpdateEntityOperationResults(reader, entities, entityOperationResults, updateAttributeResults);
                            }
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }
                    }

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Reclassifies Entity Children
        /// </summary>
        /// <param name="entities">collection of valid entities</param>
        /// <param name="entityOperationResults">entity operation results</param>
        /// <param name="entityIds">collection of entity Ids.</param>
        /// <param name="loginUser">logged in user name</param>
        /// <param name="programName">program name</param>
        /// <param name="command">command</param>
        public void ReclassifyChildren(EntityCollection entities, EntityOperationResultCollection entityOperationResults, Collection<Int64> entityIds, String loginUser, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManagaer.EntityDA.ReclassifyChildren", MDMTraceSource.EntityProcess, false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_ReclassifyChildren_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> entityList = null;

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("EntityManager_Entity_ReclassifyChildren_ParametersArray", parameters[0].ParameterName);

                if (entities != null && entities.Count > 0)
                {
                    entityList = new List<SqlDataRecord>();

                    foreach (Entity entity in entities)
                    {
                        SqlDataRecord entityRecord = new SqlDataRecord(sqllocales);

                        entityRecord.SetValue(0, (Int64)entity.Id);
                        entityRecord.SetValue(1, (Int64)entity.EntityMoveContext.FromCategoryId);
                        entityRecord.SetValue(2, (Int64)entity.CategoryId);
                        entityRecord.SetValue(3, (Int32)entity.ContainerId);

                        //If short name mapped attribute id is changed then update entity.Action as Rename.
                        //This is required for procedure to identify action Rename or Reclassify for performance.
                        ObjectAction action = entityIds.Contains(entity.Id) ? ObjectAction.Rename : entity.Action;

                        entityRecord.SetValue(4, action.ToString());

                        entityList.Add(entityRecord);
                    }
                }
                #endregion

                parameters[0].Value = entityList;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_EntityManager_EntityChild_ReClassify";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    UpdateEntityReclassificationOperationResults(reader, entities, entityOperationResults);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManagaer.EntityDA.ReclassifyChildren", MDMTraceSource.EntityProcess);
            }
        }

        #endregion

        #region Get Entity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="categoryName"></param>
        /// <param name="containerName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Int64 GetEntityId(String entityName, String categoryName, String containerName, DBCommandProperties command)
        {
            Int64 entityId = 0;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetEntityIdByName_ParametersArray");

                parameters[0].Value = entityName;
                parameters[1].Value = categoryName;
                parameters[2].Value = containerName;
                parameters[3].Value = LocaleEnum.UnKnown;//TODO:: Remove this parameter. Not required

                storedProcedureName = "usp_EntityManager_Entity_GetEntityIdByName";

                Object value = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                if (value != null)
                {
                    Int64.TryParse(value.ToString(), out entityId);
                }
            }
            finally
            {
            }
            return entityId;
        }

        /// <summary>
        /// Gets an entity for the requested entity id
        /// </summary>
        /// <param name="entityId">Id of entity for which request has been made</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for entity</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="attributeModels">Contains models for the requested attribute Ids</param>
        /// <param name="command">Object having command properties</param>
        /// <param name="callerContext">Specifies caller context</param>
        /// <returns>Entity object fetched from DB</returns>
        public Entity Get(Int64 entityId, EntityContext entityContext, LocaleEnum systemDataLocale, AttributeModelCollection attributeModels, DBCommandProperties command, CallerContext callerContext)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            Entity entity = null;

            try
            {
                Boolean loadAttribute = (entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues);
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Get_ParametersArray");

                #region Populate table value parameters for attributes list

                SqlMetaData[] attributeListMetadata = generator.GetTableValueMetadata("EntityManager_Entity_Get_ParametersArray", parameters[1].ParameterName);
                List<SqlDataRecord> attributeRecordList = EntityDataReaderUtility.CreateAttributeListTable(attributeModels, attributeListMetadata, -1);

                #endregion

                #region Populate table value parameters for Locale

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("EntityManager_Entity_Get_ParametersArray", parameters[2].ParameterName);
                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(entityContext.DataLocales, (Int32)systemDataLocale, sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                #region Populate valuetype string reading EntityContext

                String valueType = "All";
                if (entityContext.LoadOnlyInheritedValues)
                {
                    valueType = "I";
                }
                else if (entityContext.LoadOnlyOverridenValues)
                {
                    valueType = "O";
                }

                #endregion

                parameters[0].Value = entityId;
                parameters[1].Value = attributeRecordList;
                parameters[2].Value = localeList;
                parameters[3].Value = entityContext.ContainerId;
                parameters[4].Value = entityContext.LoadEntityProperties;
                parameters[5].Value = loadAttribute;
                parameters[6].Value = entityContext.LoadBusinessConditions;
                parameters[7].Value = entityContext.IgnoreEntityStatus;
                parameters[8].Value = valueType;
                parameters[9].Value = entityContext.LoadComplexChildAttributes;
                parameters[10].Value = callerContext.ProgramName;

                storedProcedureName = "usp_EntityManager_Entity_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);
          
                // If user has passed this flag as true atleast state validaiton attributes has to come out
                // Since these attributes are part of normal attributes we have to pass this flag as true.
                if(entityContext.LoadStateValidationAttributes)
                {
                    loadAttribute = true;
                }

                EntityCollection entities = EntityDataReaderUtility.ReadEntities(reader, MDMCenterModules.Entity, loadAttribute, false, false, false, attributeModels, entityContext.LoadComplexChildAttributes, entityContext.LoadBusinessConditions);

                if (entities != null)
                {
                    entity = entities.FirstOrDefault();
                }

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entity;
        }

        /// <summary>
        /// Gets entities for the requested entity ids
        /// </summary>
        /// <param name="entityIdList">Entity id list for which request has been made</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for entity</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="attributeModels">Contains models for the requested attribute Ids</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Entity collection fetched from DB</returns>
        public EntityCollection BulkGet(Collection<Int64> entityIdList, EntityContext entityContext, LocaleEnum systemDataLocale, AttributeModelCollection attributeModels, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntityCollection entities = null;

            try
            {
                Boolean loadAttribute = (entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues);

                String storedProcedureName = "usp_EntityManager_Entity_Multiple_Get";

                SqlParameter[] parameters = GetSqlParametersForBulkGet(entityIdList, entityContext, systemDataLocale, attributeModels);
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if(entityContext.LoadStateValidationAttributes)
                {
                    loadAttribute = true;
                }

                entities = EntityDataReaderUtility.ReadEntities(reader, MDMCenterModules.Entity, loadAttribute, false, false, false, attributeModels, entityContext.LoadComplexChildAttributes, entityContext.LoadBusinessConditions);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entities;
        }

        /// <summary>
        /// Gets entities ids for the requested entity guids
        /// </summary>
        /// <param name="entityGuidList">Entity id list for which request has been made</param>
        /// <returns>Entity collection fetched from DB</returns>
        public Collection<Int64> GetEntityGuidsMap(Collection<Guid> entityGuidList)
        {
            SqlDataReader reader = null;
            Collection<Int64> entityIds = new Collection<Int64>();

            try
            {
                String storedProcedureName = "usp_EntityManager_EntityGUID_Map_Get";

                SqlParameter[] parameters = GetSqlParametersForEntityUniqueIdMapGet(entityGuidList);
                reader = ExecuteProcedureReader(AppConfigurationHelper.ConnectionString, parameters, storedProcedureName);

                Dictionary<Guid, Int64> entityGuidsMap = EntityDataReaderUtility.ReadEntityGuidsMap(reader);

                foreach (Guid entityGuid in entityGuidList)
                {
                    entityIds.Add(entityGuidsMap[entityGuid]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return entityIds;
        }

        /// <summary>
        /// Gets entities along with its hierarchy children for the requested entity ids
        /// </summary>
        /// <param name="entityId">Id of entity for which request has been made</param>
        /// <param name="entityContext">Specifies what all information is to be loaded for entity</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="attributeModelsForHierarchy">Contains attribute models for the requested attribute Ids based on the entity type id</param>
        /// <param name="entityTypesForVariant">Contains collection of all entity types requested by the context</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Entity collection fetched from DB</returns>
        public EntityCollection BulkGetWithHierarchy(Int64 entityId, EntityContext entityContext, LocaleEnum systemDataLocale, IDictionary<Int32, AttributeModelCollection> attributeModelsForHierarchy, Dictionary<Int32, EntityType> entityTypesForVariant, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            SqlDataReader reader = null;
            EntityCollection entities = null;

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                String storedProcedureName = "usp_EntityManager_Entity_Hierarchy_Get";

                SqlParameter[] parameters = GetSqlParametersForHierarchyGet(entityId, entityContext, systemDataLocale, attributeModelsForHierarchy, entityTypesForVariant);

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed executing reader for procedure : " + storedProcedureName);
                }

                entities = EntityDataReaderUtility.ReadEntitiesWithHierarchy(reader, MDMCenterModules.Entity, entityContext.IsAttributeLoadingRequired, false,
                    entityContext.LoadHierarchyRelationships, false, attributeModelsForHierarchy, entityContext.LoadComplexChildAttributes, entityContext.LoadBusinessConditions);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">Indicates entity object</param>
        /// <param name="inheritableAttrIdListToBeEnsured">Indicates the inherited attribute id list</param>
        /// <param name="inheritableAttributeModels">Indicates models details of inherited attribute</param>
        /// <param name="systemDataLocale">Indicates system data locale</param>
        /// <param name="command">Object having command properties</param>
        public void EnsureInheritedValues(Entity entity, Collection<Int32> inheritableAttrIdListToBeEnsured, AttributeModelCollection inheritableAttributeModels, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityDA.EnsureInheritedValues", false);
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_InheritedValues_Get_ParametersArray");

                #region Populate table value parameters for ParentEntityDetails

                SqlMetaData[] entityIdMetadata = generator.GetTableValueMetadata("EntityManager_Entity_InheritedValues_Get_ParametersArray", parameters[0].ParameterName);
                List<SqlDataRecord> entityParentRecordList = EntityDataReaderUtility.CreateParentEntityTable(entity, entityIdMetadata);

                #endregion

                #region Populate table value parameters for Attribute Ids

                SqlMetaData[] attributeIdMetadata = generator.GetTableValueMetadata("EntityManager_Entity_InheritedValues_Get_ParametersArray", parameters[1].ParameterName);
                List<SqlDataRecord> attributeIdRecordList = EntityDataReaderUtility.CreateAttributeIdTable(inheritableAttrIdListToBeEnsured, attributeIdMetadata, entity.ReferenceId);

                #endregion

                #region Populate table value parameters for Locale

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("EntityManager_Entity_InheritedValues_Get_ParametersArray", parameters[3].ParameterName);
                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(entity.EntityContext.DataLocales, (Int32)systemDataLocale, sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                parameters[0].Value = entityParentRecordList;
                parameters[1].Value = attributeIdRecordList;
                parameters[2].Value = entity.EntityContext.ContainerId;
                parameters[3].Value = localeList;

                storedProcedureName = "usp_EntityManager_Entity_InheritedValues_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                EntityDataReaderUtility.ReadAttributes(reader, new EntityCollection() { entity }, inheritableAttributeModels, MDMCenterModules.Entity);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityDA.EnsureInheritedValues");
                }
            }
        }

        /// <summary>
        /// Gets entity unique identifier collection based on given entity id, entity context collection.
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <param name="entityContexts">Indicates entity context collection</param>
        /// <param name="command">Indicates DB command properties object</param>
        /// <returns>Returns entity unique identifier collection</returns>
        public EntityUniqueIdentifierCollection GetEntityUniqueIdentifiers(Int64 entityId, EntityContextCollection entityContexts, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            SqlDataReader reader = null;
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = null;

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                String parameterConfigName = "EntityManager_Entity_Extension_Get_ParametersArray";
                String storedProcedureName = "usp_EntityManager_Entity_Extension_Get";

                SqlParameter[] parameters = generator.GetParameters(parameterConfigName);

                #region Populate Entity Id and EntityTypeContainerTable

                //Populate table value parameters for entityId
                parameters[0].Value = entityId;

                //Populate table value parameters for attributes list
                SqlMetaData[] entityTypeContainerListMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[1].ParameterName);

                List<SqlDataRecord> entityTypeContainerTable = null;

                if (entityContexts != null && entityContexts.Count > 0)
                {
                    entityTypeContainerTable = new List<SqlDataRecord>();

                    foreach (EntityContext entityContext in entityContexts)
                    {
                        SqlDataRecord entityRelationshipRecord = new SqlDataRecord(entityTypeContainerListMetadata);

                        entityRelationshipRecord.SetValue(0, entityContext.EntityTypeId);
                        entityRelationshipRecord.SetValue(1, entityContext.ContainerId);

                        entityTypeContainerTable.Add(entityRelationshipRecord);
                    }
                }

                parameters[1].Value = entityTypeContainerTable;

                #endregion

                if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Completed preparing TVPs");
                    DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityTypeContainerTable", entityTypeContainerListMetadata, entityTypeContainerTable);
                }

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                entityUniqueIdentifiers = ReadEntityUniqueIdentifiers(reader);

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed executing reader for procedure : " + storedProcedureName);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityUniqueIdentifiers;
        }

        #endregion

        #region Child Entity

        public Collection<Int64> GetChildrenByEntityType(Int64 entityId, Int32 entityTypeId)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Data.EntityDA.GetChildrenByEntityType", MDMTraceSource.EntityGet, false);

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Collection<Int64> returnValue = new Collection<Int64>();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityDA.GetChildrenByEntityType", MDMTraceSource.EntityGet, false);

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetChildrenByEntityType_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = entityTypeId;


                storedProcedureName = "usp_EntityManager_Entity_GetChildrenByEntityType";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Int64 returnEntityId = 0;
                        if (reader["PK_CNode"] != null)
                        {
                            Int64.TryParse(reader["PK_CNode"].ToString(), out returnEntityId);
                            if (returnEntityId != 0)
                                returnValue.Add(returnEntityId);
                        }
                        else
                        {
                            throw new Exception("Could not find column PK_CNode.");
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.Data.EntityDA.GetChildrenByEntityType", MDMTraceSource.EntityGet);

                if (reader != null)
                    reader.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Get child entity scopes for given parent entity id
        /// </summary>
        /// <param name="parentEntityId"></param>
        /// <param name="catalogId"></param>
        /// <param name="entityTypes"></param>
        /// <returns></returns>
        public EntityScopeCollection GetChildrenByParentEntityId(Int64 parentEntityId, Int32 catalogId, EntityTypeCollection entityTypes)
        {
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityScopeCollection entityScopes = new EntityScopeCollection();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("EntityManager.Data.EntityDA.GetChildrenByParentEntityId Started");
            }

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Enity_GetChildrenByParentEntityId_ParametersArray");

                #region Populate table value parameters for entity list

                SqlMetaData[] entityListMetadata = generator.GetTableValueMetadata("EntityManager_Enity_GetChildrenByParentEntityId_ParametersArray", parameters[0].ParameterName);
                List<SqlDataRecord> entityRecordList = new List<SqlDataRecord>();
                SqlDataRecord entityRecord = new SqlDataRecord(entityListMetadata);
                entityRecord.SetInt64(0, parentEntityId);
                entityRecord.SetInt32(1, catalogId);

                entityRecordList.Add(entityRecord);
                parameters[0].Value = entityRecordList;

                if (entityTypes != null && entityTypes.Count > 0)
                {
                    SqlMetaData[] entityTypeTableMetadata = generator.GetTableValueMetadata("EntityManager_Enity_GetChildrenByParentEntityId_ParametersArray", parameters[1].ParameterName);
                    parameters[1].Value = EntityDataReaderUtility.CreateEntityTypeTable(null, entityTypeTableMetadata);
                }

                #endregion Populate table value parameters for entity list

                storedProcedureName = "usp_EntityManager_Entity_Impacted_Children_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                Int32 entityIdOrdinal = -1;
                Int32 containerIdOrdinal = -1;
                Int32 entityTypeIdOrdinal = -1;
                Int32 categoryIdOrdinal = -1;

                Int32 prevContainerId = 0;
                Int32 prevEntityTypeId = 0;
                Int64 prevCategoryId = 0;
                EntityScope entityScope = null;
                while (reader.Read())
                {
                    if (entityIdOrdinal == -1)
                    {
                        entityIdOrdinal = reader.GetOrdinal("EntityId");
                        containerIdOrdinal = reader.GetOrdinal("ContainerId");
                        entityTypeIdOrdinal = reader.GetOrdinal("EntityType");
                        categoryIdOrdinal = reader.GetOrdinal("Category");
                    }

                    Int64 entityId = reader.GetInt64(entityIdOrdinal);
                    Int32 containerId = reader.GetInt32(containerIdOrdinal);
                    Int32 entityTypeId = reader.GetInt32(entityTypeIdOrdinal);
                    Int64 categoryId = reader.GetInt64(categoryIdOrdinal);

                    if (containerId == prevContainerId && entityTypeId == prevEntityTypeId && categoryId == prevCategoryId)
                    {
                        entityScope.EntityIdList.Add(entityId);
                    }
                    else
                    {
                        entityScope = new EntityScope();
                        entityScope.EntityContextCollection.Add(new EntityContext { ContainerId = containerId, EntityTypeId = entityTypeId, CategoryId = categoryId });
                        entityScope.EntityIdList.Add(entityId);

                        entityScopes.Add(entityScope);

                        prevCategoryId = categoryId;
                        prevContainerId = containerId;
                        prevEntityTypeId = entityTypeId;
                    }
                }

            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("EntityManager.Data.EntityDA.GetChildrenByParentEntityId Completed");
                    diagnosticActivity.Stop();
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
            return entityScopes;
        }

        #endregion

        #region Exists

        public Int32 Exists(Entity entity, Int64 componentId, Int32 catalogId, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Int32 returnValue = 0;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Exists_ParametersArray");

                parameters[0].Value = entity.CategoryId;
                parameters[1].Value = componentId;
                parameters[2].Value = catalogId;
                parameters[3].Value = entity.Name;
                parameters[4].Value = entity.LongName;
                parameters[5].Value = entity.SKU;

                storedProcedureName = "usp_EntityManager_Entity_Exists";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Int32.TryParse(reader[0].ToString(), out returnValue);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnValue;
        }

        #endregion

        #region Workflow Related

        /// <summary>
        /// Gets the workflow invokable entity ids.
        /// </summary>
        /// <param name="entityIds">The entity ids.</param>
        /// <param name="workflowShortName">Indicates workflow short name</param>
        /// <param name="activityLongName">Indicates activity long name</param>
        /// <param name="command">The command.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Returns WorkflowInvokableEntityInfoCollection</returns>
        public WorkflowInvokableEntityInfoCollection GetWorkflowInvokableEntityIds(Collection<Int64> entityIds, String workflowShortName, String activityLongName, DBCommandProperties command, CallerContext callerContext)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            SqlDataReader reader = null;
            WorkflowInvokableEntityInfoCollection workflowInvokableEntityInfoColl = null;

            try
            {
                #region Diagnostics
                if (isTracingEnabled)
                {
                    CallDataContext callDataContext = new CallDataContext();
                    callDataContext.EntityIdList = entityIds;
                    ExecutionContext executionContext = new ExecutionContext(callerContext, callDataContext, new SecurityContext(), String.Empty);
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.Entity);
                    diagnosticActivity.Start(executionContext);
                }
                #endregion Diagnostics

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                String parameterConfigName = "EntityManager_Entity_WorkflowInvokableEntityIds_Get_ParametersArray";
                String storedProcedureName = "usp_EntityManager_Entity_WorkflowInvokableEntityIds_Get";

                SqlParameter[] parameters = generator.GetParameters(parameterConfigName);

                #region Populate Entity Id Table
                
                //Populate table value parameters for entityIds
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);
                List<SqlDataRecord> entityIdRecordList = EntityDataReaderUtility.CreateEntityIdTable(entityIds, entityMetadata);

                parameters[0].Value = entityIdRecordList;
                parameters[1].Value = workflowShortName;
                parameters[2].Value = activityLongName;

                #endregion Populate Entity Id Table

                if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Completed preparing TVPs");
                    DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityTable", entityMetadata, entityIdRecordList);
                }

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                workflowInvokableEntityInfoColl = ReadWorkflowInvokableEntityIds(reader);

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed executing reader for procedure : " + storedProcedureName);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                #region Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return workflowInvokableEntityInfoColl;
        }

        #endregion Workflow Related

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntityOperationResults(SqlDataReader reader, EntityCollection entities, EntityOperationResultCollection entityOperationResults, Boolean updateAttributeResults)
        {
            if (reader != null)
            {
                UpdateEntityResults(reader, entities, entityOperationResults);

                if (entityOperationResults.OperationResultStatus != OperationResultStatusEnum.Failed && updateAttributeResults)
                {
                    //Move reader to next result set
                    reader.NextResult();

                    UpdateAttributeResults(reader, entities);
                }
            }
        }

        private void UpdateEntityResults(SqlDataReader reader, EntityCollection entities, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int64 entityId = 0;
                Int64 auditRefId = -1;
                Int64 entityFamilyId = 0;
                String entityFamilyLongName = String.Empty;
                Int64 entityGlobalFamilyId = 0;
                String entityGlobalFamilyLongName = String.Empty;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String idPath = String.Empty;
                Guid? entityGuid = null;

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), id);
                }
                if (reader["EntityId"] != null)
                {
                    entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), entityId);
                }
                if (reader["AuditRefID"] != null)
                {
                    auditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefID"].ToString(), auditRefId);
                }
                if (reader["EntityFamilyId"] != null)
                {
                    entityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), entityFamilyId);
                }
                if (reader["EntityFamilyLongName"] != null)
                {
                    entityFamilyLongName = reader["EntityFamilyLongName"].ToString();
                }
                if (reader["EntityGlobalFamilyId"] != null)
                {
                    entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), entityGlobalFamilyId);
                }
                if (reader["EntityGlobalFamilyLongName"] != null)
                {
                    entityGlobalFamilyLongName = reader["EntityGlobalFamilyLongName"].ToString();
                }
                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), hasError);
                }
                if (reader["IDPath"] != null)
                {
                    idPath = reader["IdPath"].ToString();
                }
                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }
                if (reader["EntityGUID"] != null)
                {
                    var guidString = reader["EntityGUID"].ToString();

                    if (!String.IsNullOrEmpty(guidString))
                    {
                        entityGuid = new Guid(guidString);
                    }
                }

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
                        entity.EntityFamilyId = entityFamilyId;
                        entity.EntityFamilyLongName = entityFamilyLongName;
                        entity.EntityGlobalFamilyId = entityGlobalFamilyId;
                        entity.EntityGlobalFamilyLongName = entityGlobalFamilyLongName;
                    }

                    // We explicitly not overwrite the GUID if already present. 
                    if (entity.EntityGuid == null && entityGuid != null)
                    {
                        entity.EntityGuid = entityGuid;
                    }

                    //Update the entity Object with the latest AuditRefId
                    entity.AuditRefId = auditRefId;

                    entityOperationResult.EntityLongName = entity.LongName;
                    entityOperationResult.ExternalId = entity.Name;
                    entityOperationResult.PerformedAction = entity.Action;

                    if (hasError)
                    {
                        //Add error
                        entityOperationResults.AddEntityOperationResult(entityOperationResult.EntityId, String.Empty, errorMessage, OperationResultType.Error);
                    }
                    else if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithWarnings &&
                             entityOperationResult.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors &&
                             entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                    {
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
        }

        private void UpdateAttributeResults(SqlDataReader reader, EntityCollection entities)
        {
            while (reader.Read())
            {
                #region Step: Declare Local variables

                Int64 entityId = 0;
                Int32 attributeId = 0;
                Int32 sequence = 0;
                Int64 attributeValueId = 0;
                Int32 localeId = 0;
                LocaleEnum locale = GlobalizationHelper.GetSystemDataLocale();

                #endregion

                #region Step: Read Entity detail from Attribute Row

                if (reader["EntityId"] != null)
                    entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0);

                if (reader["AttributeId"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), 0);

                if (reader["LocaleId"] != null)
                {
                    localeId = ValueTypeHelper.Int32TryParse(reader["LocaleId"].ToString(), 0);
                    locale = (LocaleEnum)localeId;
                }

                //Get the entity
                Entity entity = entities.SingleOrDefault(e => e.Id == entityId);

                #endregion

                if (entity != null && entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    Attribute attribute = null;

                    IAttribute iAttribute = entity.Attributes.GetAttribute(attributeId, locale);
                    if (iAttribute != null)
                    {
                        attribute = (Attribute)iAttribute;

                        //HACK>>THERE IS POSSIBLITY THAT ATTRIBUTE RETURNED IS NOT IN THE LOCALE WE HAVE LOOKED FOR...THUS MAKE SURE THE OBJECT IS EXACT MATCH FOR LOCALE..
                        if (attribute.Locale != locale)
                            attribute = null;
                    }

                    if (attribute != null)
                    {
                        #region Step: Read Attribute value Properties and update value object

                        //Read other parameters
                        if (reader["Sequence"] != null)
                        {
                            Decimal decSeq = 0;
                            decSeq = ValueTypeHelper.DecimalTryParse(reader["Sequence"].ToString(), 0);
                            sequence = (Int32)decSeq;
                        }

                        if (reader["PK_AttrVal"] != null)
                        {
                            attributeValueId = ValueTypeHelper.Int64TryParse(reader["PK_AttrVal"].ToString(), -1);
                        }

                        #endregion

                        IValueCollection values = attribute.GetOverriddenValuesInvariant();
                        IValueCollection deletedValues = null;
                        Boolean isAnyValueDeleted = false;

                        if (values != null && values.Count > 0)
                        {
                            foreach (Value value in values)
                            {
                                if (value.Action == ObjectAction.Delete)
                                {
                                    if (!attribute.IsComplex)
                                    {
                                        if (deletedValues == null)
                                        {
                                            deletedValues = new ValueCollection();
                                        }

                                        deletedValues.Add(value);

                                        isAnyValueDeleted = true;
                                    }
                                }
                                else if (value.Sequence == sequence)
                                {
                                    //Update value object with value Id
                                    value.Id = attributeValueId;
                                }
                            }

                            if (isAnyValueDeleted)
                            {
                                foreach (Value deletedValue in deletedValues)
                                {
                                    values.Remove(deletedValue);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateEntityReclassificationOperationResults(SqlDataReader reader, EntityCollection entities, EntityOperationResultCollection entityOperationResults, Boolean updateErrorAsInfo = false)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String idPath = String.Empty;
                String childEntityName = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["Name"] != null)
                    childEntityName = reader["Name"].ToString();
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
                    if (reader["EntityGUID"] != null)
                    {
                        var guidString = reader["EntityGUID"].ToString();

                        // We overwrite existing guid with one from db.
                        if (!String.IsNullOrEmpty(guidString))
                        {
                            entity.EntityGuid = new Guid(guidString);
                        }
                    }

                    if (hasError)
                    {
                        //Add error as information for Entity Reclassification. This change is required to continue without any interruption for further processing.
                        entityOperationResults.AddEntityOperationResult(entityOperationResult.EntityId, String.Empty, childEntityName + " - " + errorMessage, OperationResultType.Information);
                    }
                    else
                    {
                        //No errors.. update status as Successful.
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
        }

        private SqlParameter[] GetSqlParametersForHierarchyGet(Int64 entityId, EntityContext entityContext, LocaleEnum systemDataLocale, IDictionary<Int32, AttributeModelCollection> attributeModelsForHierarchy, Dictionary<Int32, EntityType> entityTypesForVariant)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

            String parameterConfigName = "EntityManager_Entity_Hierarchy_Get_ParametersArray";
            String storedProcedureName = "usp_EntityManager_Entity_Hierarchy_Get";

            SqlParameter[] parameters = generator.GetParameters(parameterConfigName);

            #region Populate EntityIdRecordList, AttributeRecordList & LocaleList

            //Populate table value parameters for entityId
            parameters[0].Value = entityId;

            //Populate table value parameters for attributes list
            SqlMetaData[] attributeListMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[1].ParameterName);
            List<SqlDataRecord> attributeRecordList = EntityDataReaderUtility.CreateAttributeListTableForHierarchyGet(attributeModelsForHierarchy, attributeListMetadata, -1);
            parameters[1].Value = attributeRecordList;

            //Populate table value parameters for Locale
            SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[2].ParameterName);
            List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(entityContext.DataLocales, (Int32)systemDataLocale, sqlLocalesMetadata);
            parameters[2].Value = localeList;

            #endregion

            //Fill rest of the SqlParameters based on EntityContext
            FillSqlParametersBasedOnContext(parameters, entityContext);

            parameters[10].Value = entityContext.LoadHierarchyRelationships;

            #region Populate table value parameters for entity type

            //Populate table value parameters for entity type
            SqlMetaData[] sqlEntityTypeMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[11].ParameterName);
            List<SqlDataRecord> entityTypeRecordList = EntityDataReaderUtility.CreateEntityTypeTable(entityTypesForVariant, sqlEntityTypeMetadata);
            parameters[11].Value = entityTypeRecordList;

            #endregion

            if (currentTraceSettings != null && currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogInformation("Completed preparing TVPs");
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "AttributeRecordList", attributeListMetadata, attributeRecordList);
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "LocaleList", sqlLocalesMetadata, localeList);
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityTypeRecordList", sqlEntityTypeMetadata, entityTypeRecordList);
                diagnosticActivity.Stop();
            }

            return parameters;
        }

        private SqlParameter[] GetSqlParametersForEntityUniqueIdMapGet(Collection<Guid> entityGuidList)
        {
            SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

            String parameterConfigName = "EntityManager_Entity_EntityGuidMapGet_ParametersArray";

            SqlParameter[] parameters = generator.GetParameters(parameterConfigName);
            
            //Populate table value parameters for entityGuids
            SqlMetaData[] entityMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);
            List<SqlDataRecord> entityIdRecordList = EntityDataReaderUtility.CreateEntityGuidTable(entityGuidList, entityMetadata);
            parameters[0].Value = entityIdRecordList;

            return parameters;
        }

        private SqlParameter[] GetSqlParametersForBulkGet(Collection<Int64> entityIdList, EntityContext entityContext, LocaleEnum systemDataLocale, AttributeModelCollection attributeModels)
        {
            SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

            String parameterConfigName = "EntityManager_Entity_BulkGet_ParametersArray";

            SqlParameter[] parameters = generator.GetParameters(parameterConfigName);

            #region Populate EntityIdRecordList, AttributeRecordList & LocaleList

            //Populate table value parameters for entityIds
            SqlMetaData[] entityMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);
            List<SqlDataRecord> entityIdRecordList = EntityDataReaderUtility.CreateEntityIdTable(entityIdList, entityMetadata);
            parameters[0].Value = entityIdRecordList;

            //Populate table value parameters for attributes list
            SqlMetaData[] attributeListMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[1].ParameterName);
            List<SqlDataRecord> attributeRecordList = EntityDataReaderUtility.CreateAttributeListTable(attributeModels, attributeListMetadata, -1);
            parameters[1].Value = attributeRecordList;

            //Populate table value parameters for Locale
            SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[2].ParameterName);
            List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(entityContext.DataLocales, (Int32)systemDataLocale, sqlLocalesMetadata);
            parameters[2].Value = localeList;

            #endregion

            //Fill rest of the SqlParameters based on EntityContext
            FillSqlParametersBasedOnContext(parameters, entityContext);

            return parameters;
        }

        private void FillSqlParametersBasedOnContext(SqlParameter[] parameters, EntityContext entityContext)
        {
            Boolean loadAttribute = (entityContext.LoadAttributes || entityContext.LoadOnlyInheritedValues || entityContext.LoadOnlyOverridenValues);

            #region Populate value type string reading EntityContext

            String valueType = "All";
            if (entityContext.LoadOnlyInheritedValues)
            {
                valueType = "I";
            }
            else if (entityContext.LoadOnlyOverridenValues)
            {
                valueType = "O";
            }
            else if (entityContext.LoadOnlyCurrentValues)
            {
                valueType = "C";
            }

            #endregion

            parameters[3].Value = entityContext.ContainerId;
            parameters[4].Value = entityContext.LoadEntityProperties;
            parameters[5].Value = loadAttribute;
            parameters[6].Value = entityContext.LoadBusinessConditions;
            parameters[7].Value = entityContext.IgnoreEntityStatus;
            parameters[8].Value = valueType;
            parameters[9].Value = entityContext.LoadComplexChildAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private EntityUniqueIdentifierCollection ReadEntityUniqueIdentifiers(SqlDataReader reader)
        {
            EntityUniqueIdentifierCollection entityUniqueIdentifiers = new EntityUniqueIdentifierCollection();

            while (reader.Read())
            {
                EntityUniqueIdentifier entityUniqueIdentifier = new EntityUniqueIdentifier();

                if (reader["EntityId"] != null)
                {
                    entityUniqueIdentifier.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), entityUniqueIdentifier.EntityId);
                }
            
                if (reader["ContainerId"] != null)
                {
                    entityUniqueIdentifier.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), entityUniqueIdentifier.ContainerId);
                }
                
                if (reader["EntityTypeId"] != null)
                {
                    entityUniqueIdentifier.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entityUniqueIdentifier.EntityTypeId);
                }
                
                if (reader["CategoryId"] != null)
                {
                    entityUniqueIdentifier.CategoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), entityUniqueIdentifier.CategoryId);
                }
                
                if (reader["ContainerName"] != null)
                {
                    entityUniqueIdentifier.ContainerName = reader["ContainerName"].ToString();
                }
                
                if (reader["ContainerQualifier"] != null)
                {
                    entityUniqueIdentifier.ContainerQualifierName = reader["ContainerQualifier"].ToString();
                }

                if (reader["ContainerLevel"] != null)
                {
                    entityUniqueIdentifier.ContainerLevel = ValueTypeHelper.Int32TryParse(reader["ContainerLevel"].ToString(), entityUniqueIdentifier.ContainerLevel);
                }
                
                if (reader["CategoryPath"] != null)
                {
                    entityUniqueIdentifier.CategoryPath = reader["CategoryPath"].ToString();
                }

                entityUniqueIdentifiers.Add(entityUniqueIdentifier);
            }

            return entityUniqueIdentifiers;
        }

        /// <summary>
        /// Reads the workflow invokable entity id info collection.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Returns WorkflowInvokableEntityInfoCollection</returns>
        private WorkflowInvokableEntityInfoCollection ReadWorkflowInvokableEntityIds(SqlDataReader reader)
        {
            WorkflowInvokableEntityInfoCollection workflowInvokableEntityInfoColl = null;

            if (reader != null)
            {
                workflowInvokableEntityInfoColl = new WorkflowInvokableEntityInfoCollection();

                while (reader.Read())
                {
                    // SP input is a collection of distinct entity ids. So SP will never return duplicate records
                    // So directly initializing new object and adding it into collection object without any duplicate check.
                    WorkflowInvokableEntityInfo workflowInvokableEntityInfo = new WorkflowInvokableEntityInfo();

                    if (reader["EntityId"] != null)
                    {
                        workflowInvokableEntityInfo.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), workflowInvokableEntityInfo.EntityId);
                    }

                    if (reader["WorkflowInvokableEntityId"] != null)
                    {
                        workflowInvokableEntityInfo.WorkflowInvokableEntityId = ValueTypeHelper.Int64TryParse(reader["WorkflowInvokableEntityId"].ToString(), workflowInvokableEntityInfo.WorkflowInvokableEntityId);
                    }

                    if (workflowInvokableEntityInfo.WorkflowInvokableEntityId <= 0)
                    {
                        workflowInvokableEntityInfo.WorkflowInvokableEntityId = workflowInvokableEntityInfo.EntityId;
                    }

                    if (reader["EntityInWorkflow"] != null)
                    {
                        workflowInvokableEntityInfo.IsEntityInWorkflow = ValueTypeHelper.BooleanTryParse(reader["EntityInWorkflow"].ToString(), workflowInvokableEntityInfo.IsEntityInWorkflow);
                    }

                    workflowInvokableEntityInfoColl.Add(workflowInvokableEntityInfo);
                } 
            }

            return workflowInvokableEntityInfoColl;
        }
     
        #endregion Private Methods

        #endregion Methods
    }
}