using System.Linq;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.RelationshipManager.Data
{
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class RelationshipDA : SqlClientDataAccessBase
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
        /// Gets relationships for the requested entities
        /// </summary>
        /// <param name="entities">Entities for which needs to be loaded</param>
        /// <param name="relationshipContext">Relationship context indicates relationship type, locale etc.</param>
        /// <param name="attributeModelsPerRT">Specifies attribute model per relationship type.</param>
        /// <param name="maxLevelToReturn">Specifies max level to return</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <param name="command">Indicates SQL command</param>
        /// <param name="relationshipsDictionary">Specifies relationship as dictionary. Key would be entity id and value would be relationship collection.</param>
        /// <param name="relationshipAttributesDictionary">Specifies relationship attributes as dictionary.Key would be combination with relationship id and locale.</param>
        /// <param name="updateCache">Specifies whether to update the cache after loading</param>
        /// <param name="dataLocaleList"></param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        public Boolean GetRelationships(EntityCollection entities, RelationshipContext relationshipContext, Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT, Int16 maxLevelToReturn, CallerContext callerContext, DBCommandProperties command, out Dictionary<Int64, RelationshipCollection> relationshipsDictionary, out Dictionary<String, AttributeCollection> relationshipAttributesDictionary, Boolean updateCache, Collection<LocaleEnum> dataLocaleList)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipDA.GetRelationships", MDMTraceSource.RelationshipGet, false);
            }

            Boolean returnValue = false;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                parameters = generator.GetParameters("RelationshipManager_Relationship_Get_ParametersArray");

                #region Populate table value parameters for entities relationships

                List<SqlDataRecord> entityRelationshipTable = null;

                SqlMetaData[] entityMetaData = generator.GetTableValueMetadata("RelationshipManager_Relationship_Get_ParametersArray", parameters[0].ParameterName);

                if (entities != null && entities.Count > 0)
                {
                    entityRelationshipTable = new List<SqlDataRecord>();

                    foreach (Entity entity in entities)
                    {
                        foreach (Int32 relationshipTypeId in relationshipContext.RelationshipTypeIdList)
                        {
                            SqlDataRecord entityRelationshipRecord = new SqlDataRecord(entityMetaData);

                            entityRelationshipRecord.SetValue(0, entity.Id);
                            entityRelationshipRecord.SetValue(1, entity.ContainerId);
                            entityRelationshipRecord.SetValue(2, relationshipTypeId);
                            entityRelationshipRecord.SetValue(3, (relationshipContext.RelationshipParentId > 0) ? relationshipContext.RelationshipParentId : 0);
                            entityRelationshipRecord.SetValue(4, ValueTypeHelper.Int32TryParse(relationshipContext.Level.ToString(), 0));

                            entityRelationshipTable.Add(entityRelationshipRecord);
                        }
                    }
                }

                #endregion Populate table value parameters for Locale

                #region Populate table value parameters for Locale

                if (dataLocaleList == null || dataLocaleList.Count < 1)
                {
                    dataLocaleList = new Collection<LocaleEnum>();
                    dataLocaleList.Add(relationshipContext.Locale);
                }

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("RelationshipManager_Relationship_Get_ParametersArray", parameters[2].ParameterName);

                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(dataLocaleList, (Int32)GlobalizationHelper.GetSystemDataLocale(), sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                parameters[0].Value = entityRelationshipTable != null && entityRelationshipTable.Count > 0 ? entityRelationshipTable : null;
                parameters[1].Value = null; //TODO: Need to pass AttributeIdList for future use.
                parameters[2].Value = localeList;
                parameters[3].Value = true; // Always pass LoadRelationships as True.
                parameters[4].Value = relationshipContext.LoadRelationshipAttributes;
                parameters[5].Value = relationshipContext.ReturnRelatedEntityDetails;
                parameters[6].Value = "All";
                parameters[7].Value = maxLevelToReturn;

                storedProcedureName = "usp_RelationshipManager_Relationship_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                AttributeModelCollection flatAttributeModels = new AttributeModelCollection();

                foreach (KeyValuePair<Int32, AttributeModelCollection> pair in attributeModelsPerRT)
                {
                    flatAttributeModels.AddRange(pair.Value);
                }

                relationshipsDictionary = new Dictionary<Int64, RelationshipCollection>();
                relationshipAttributesDictionary = new Dictionary<String, AttributeCollection>();

                EntityDataReaderUtility.ReadRelationships(reader, entities, flatAttributeModels, callerContext.Module, relationshipsDictionary, relationshipAttributesDictionary, updateCache, relationshipContext);

                returnValue = true;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipDA.GetRelationships", MDMTraceSource.RelationshipGet);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relationships"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="processOnlyRelationships"></param>
        /// <param name="loginUser"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <param name="processingMode"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public Boolean Process(EntityCollection entities, Dictionary<Int64,RelationshipCollection> relationships, EntityOperationResultCollection entityOperationResults, Boolean processOnlyRelationships, String loginUser, CallerContext callerContext, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync, Boolean isRecursive = true)
        {
            const String storedProcedureName = "usp_RelationshipManager_Relationship_Process";

            Boolean successFlag = false;
            SqlDataReader reader = null;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    List<SqlDataRecord> relationshipTable = null;
                    List<SqlDataRecord> attributeValueTable = null;

                    SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("RelationshipManager_Relationship_Process_ParametersArray");
                    SqlMetaData[] relationshipMetadata = generator.GetTableValueMetadata("RelationshipManager_Relationship_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeValueMetaData = generator.GetTableValueMetadata("RelationshipManager_Relationship_Process_ParametersArray", parameters[1].ParameterName);

                    EntityDataReaderUtility.CreateEntityRelationshipTable(entities, relationshipMetadata, attributeValueMetaData, out relationshipTable, out attributeValueTable, !processOnlyRelationships, callerContext.Module, relationships, isRecursive);

                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "RelationshipTable", relationshipMetadata, relationshipTable);
                        DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "AttributeValueTable", attributeValueMetaData, attributeValueTable);
                    }

                    const Boolean updateAttributeResults = true; //Asking to return operation results (relationships attributes) always

                    if (relationshipTable != null)
                    {
                        parameters[0].Value = relationshipTable;
                        parameters[1].Value = attributeValueTable;
                        parameters[2].Value = loginUser;
                        parameters[3].Value = callerContext.ProgramName;
                        parameters[4].Value = processOnlyRelationships;
                        parameters[5].Value = true; //Asking to return operation results (relationships) always
                        parameters[6].Value = updateAttributeResults; //Asking to return operation results (relationships attributes)

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (reader != null)
                        {
                            EntityDataReaderUtility.UpdateRelationshipOperationResults(reader, entities, entityOperationResults);

                            if (entityOperationResults.OperationResultStatus != OperationResultStatusEnum.Failed && updateAttributeResults)
                            {
                                //Move reader to next result set
                                reader.NextResult();

                                UpdateRelationshipAttributeResults(reader, entities);
                            }
                        }

                        successFlag = true;
                    }

                    transactionScope.Complete();
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

            return successFlag;
        }
        
        /// <summary>
        /// Get Where Used Relationships for given entity id and relationshipType
        /// </summary>
        /// <param name="entityIds">Indicates Entity Id</param>
        /// <param name="relationshipIds">Indicates Relationship Type Id. Set to '0' if we want where used relationships across relationship types</param>
        /// <param name="dataLocale">Indicates current data locale</param>
        /// <param name="loadCircularRelationship">Indicates whether to load circular relationship or not. Example : Kit is related to SKU and vice versa</param>
        /// <param name="command">Command</param>
        /// <returns>Relationship collection with UP direction relationships</returns>
        public RelationshipCollection GetWhereUsed(Collection<Int64> entityIds, Collection<Int32> relationshipIds, LocaleEnum dataLocale,Boolean loadCircularRelationship, DBCommandProperties command)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDA.GetWhereUsedRelationships", MDMTraceSource.Relationship, false);
            }

            RelationshipCollection relationshipCollection = new RelationshipCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                parameters = generator.GetParameters("RelationshipManager_Relationship_WhereUsed_ParametersArray");

                List<SqlDataRecord> entityTable = null;

                SqlMetaData[] entityMetaData = generator.GetTableValueMetadata("RelationshipManager_Relationship_WhereUsed_ParametersArray", parameters[0].ParameterName);

                if (entityIds != null && entityIds.Count > 0)
                {
                    entityTable = new List<SqlDataRecord>();

                    foreach (Int64 entityId in entityIds)
                    {
                        SqlDataRecord entityRecord = new SqlDataRecord(entityMetaData);
                        entityRecord.SetValue(0, entityId);
                        entityTable.Add(entityRecord);
                    }
                }

                List<SqlDataRecord> relationshipTypeTable = null;

                SqlMetaData[] relationshipTypeMetaData = generator.GetTableValueMetadata("RelationshipManager_Relationship_WhereUsed_ParametersArray", parameters[1].ParameterName);

                if (relationshipIds != null && relationshipIds.Count > 0)
                {
                    relationshipTypeTable = new List<SqlDataRecord>();

                    foreach (Int32 relationshipTypeId in relationshipIds)
                    {
                        //Get Specific relationshipTypeId if relationship-type is other than 'All'
                        if (relationshipTypeId > 0)
                        {
                            SqlDataRecord relationshipTypeIdRecord = new SqlDataRecord(relationshipTypeMetaData);
                            relationshipTypeIdRecord.SetValue(0, relationshipTypeId);
                            relationshipTypeTable.Add(relationshipTypeIdRecord);
                        }
                        else
                        {
                            relationshipTypeTable = null;
                            break;
                        }
                    }
                }

                parameters[0].Value = entityTable;
                parameters[1].Value = relationshipTypeTable;
                parameters[2].Value = loadCircularRelationship;

                storedProcedureName = "usp_RelationshipManager_Relationship_WhereUsed";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 relationshipId = 0;
                        Int64 fromEntityId = 0;
                        Int64 toEntityId = 0;
                        Int32 relationshipTypeId = 0;
                        String relationshipTypeName = String.Empty;
                        Int32 containerId = 0;
                        Int64 relationshipSourceId = 0;
                        Int64 relationshipParentId = 0;

                        if (reader["RelationshipId"] != null)
                            relationshipId = ValueTypeHelper.Int64TryParse(reader["RelationshipId"].ToString(), relationshipId);

                        if (reader["FromEntityId"] != null)
                            fromEntityId =ValueTypeHelper.Int64TryParse(reader["FromEntityId"].ToString(), fromEntityId);

                        if (reader["ToEntityId"] != null)
                            toEntityId = ValueTypeHelper.Int64TryParse(reader["ToEntityId"].ToString(), toEntityId);

                        if (reader["RelationshipTypeId"] != null)
                            relationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), relationshipTypeId);

                        if (reader["RelationshipTypeName"] != null)
                            relationshipTypeName = reader["RelationshipTypeName"].ToString();

                        if (reader["ContainerId"] != null)
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerId);

                        if (reader["SourceEntityId"] != null)
                            relationshipSourceId = ValueTypeHelper.Int64TryParse(reader["SourceEntityId"].ToString(), relationshipSourceId);

                        if (reader["RelationshipParentId"] != null)
                            relationshipParentId = ValueTypeHelper.Int64TryParse(reader["RelationshipParentId"].ToString(), relationshipParentId);

                        //Key Note: As Where Used is always up direction, we need to swap from and to entity id to represent up direction...
                        RelationshipDirection direction = RelationshipDirection.Up;

                        Int64 temp = fromEntityId;
                        fromEntityId = toEntityId;
                        toEntityId = temp;

                        Relationship relationship = new Relationship();

                        relationship.Id = relationshipId;
                        relationship.ContainerId = containerId;
                        relationship.RelationshipTypeName = relationshipTypeName;
                        relationship.RelationshipTypeId = relationshipTypeId;
                        relationship.FromEntityId = fromEntityId;
                        relationship.RelatedEntityId = toEntityId;
                        relationship.Direction = direction;
                        relationship.RelationshipSourceEntityId = relationshipSourceId;
                        relationship.RelationshipParentEntityId = relationshipParentId;
                        relationship.Locale = dataLocale;

                        relationshipCollection.Add(relationship);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipDA.GetWhereUsedRelationships", MDMTraceSource.Relationship);
                }
            }

            return relationshipCollection;
        }

        /// <summary>
        /// Get Relationship Cardinlaity for requested entityType and RelationshipType
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationshipType for which cardinality is requested</param>
        /// <param name="containerId">Indiactes Conatiner Id</param>
        /// <param name="fromEntityTypeId">Indicates EntityTypeId for which cardinality is requested</param>
        /// <param name="command">Indicates SQL command</param>
        /// <returns>Relationship Cardinalities</returns>
        public RelationshipCardinalityCollection GetRelationshipCardinalities(Int32 relationshipTypeId, Int32 containerId, Int32 fromEntityTypeId, DBCommandProperties command)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDA.GetRelationshipCardinalities", MDMTraceSource.Relationship, false);
            }

            RelationshipCardinalityCollection relationshipCardinalities = new RelationshipCardinalityCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                parameters = generator.GetParameters("RelationshipManager_Relationship_Cardinality_Get_ParametersArray");

                storedProcedureName = "usp_Relationship_Cardinality_Get";

                parameters[0].Value = containerId;
                parameters[1].Value = fromEntityTypeId;
                parameters[2].Value = relationshipTypeId;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 relationshipCardinalityId = 0;
                        Int32 relationshipTypeEntityTypeMappingId = 0;
                        Int32 toEntityTypeId = 0;
                        String toEntityTypeLongName = String.Empty;
                        Int32 minRelationships = 0;
                        Int32 maxRelationships = 0;

                        if (reader["PK_CatalogRelTypeCardinality"] != null)
                            Int32.TryParse(reader["PK_CatalogRelTypeCardinality"].ToString(), out relationshipCardinalityId);

                        if (reader["FK_CatalogRelTypeNodeType"] != null)
                            Int32.TryParse(reader["FK_CatalogRelTypeNodeType"].ToString(), out relationshipTypeEntityTypeMappingId);

                        if (reader["FK_NodeType_To"] != null)
                            Int32.TryParse(reader["FK_NodeType_To"].ToString(), out toEntityTypeId);

                        if (reader["NodeTypeToLongName"] != null)
                            toEntityTypeLongName = reader["NodeTypeToLongName"].ToString();

                        if (reader["MinRelationships"] != null)
                            Int32.TryParse(reader["MinRelationships"].ToString(), out minRelationships);

                        if (reader["MaxRelationships"] != null)
                            Int32.TryParse(reader["MaxRelationships"].ToString(), out maxRelationships);

                        RelationshipCardinality relationshipCardinality = new RelationshipCardinality();

                        relationshipCardinality.Id = relationshipCardinalityId;
                        relationshipCardinality.ContainerId = containerId;
                        relationshipCardinality.RelationshipTypeId = relationshipTypeId;
                        relationshipCardinality.RelationshipTypeEntityTypeMappingId = relationshipTypeEntityTypeMappingId;
                        relationshipCardinality.ToEntityTypeId = toEntityTypeId;
                        relationshipCardinality.ToEntityTypeLongName = toEntityTypeLongName;
                        relationshipCardinality.MinRelationships = minRelationships;
                        relationshipCardinality.MaxRelationships = maxRelationships;
                        relationshipCardinality.FromEntityTypeId = fromEntityTypeId;

                        relationshipCardinalities.Add(relationshipCardinality);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipDA.GetRelationshipCardinalities", MDMTraceSource.Relationship);
                }
            }

            return relationshipCardinalities;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entities"></param>
        private void UpdateRelationshipAttributeResults(SqlDataReader reader, EntityCollection entities)
        {
            #region Collecting all relationship instances for future updating

            // First level of recursion
            Dictionary<Int64, List<Relationship>> relationshipsIndex = new Dictionary<Int64, List<Relationship>>();
            Stack<Relationship> relationships = new Stack<Relationship>();
            foreach (Entity entity in entities)
            {
                if (entity.Relationships != null)
                {
                    foreach (Relationship relationship in entity.Relationships)
                    {
                        relationships.Push(relationship);
                    }
                }
            }
            // 2nd and other levels of recursion
            while (relationships.Any())
            {
                Relationship item = relationships.Pop();
                if (item != null)
                {
                    List<Relationship> relationshipInstances = null;
                    if (relationshipsIndex.TryGetValue(item.Id, out relationshipInstances))
                    {
                        Boolean alreadyExist = false;
                        foreach (Relationship instance in relationshipInstances)
                        {
                            if (Object.ReferenceEquals(instance, item))
                            {
                                alreadyExist = true;
                                break;
                            }
                        }
                        if (alreadyExist)
                        {
                            continue;
                        }
                        relationshipInstances.Add(item);
                    }
                    else
                    {
                        relationshipsIndex.Add(item.Id, new List<Relationship>() { item });
                    }
                    if (item.RelationshipCollection != null)
                    {
                        foreach (Relationship relationship in item.RelationshipCollection)
                        {
                            relationships.Push(relationship);
                        }
                    }
                }
            }

            if (!relationshipsIndex.Any())
            {
                return;
            }

            #endregion

            #region Processing stored procedure output

            while (reader.Read())
            {
                #region Step: Declare Local variables

                Int64 relationshipId = 0;
                Int32 attributeId = 0;
                Int32 sequence = 0;
                Int64 attributeValueId = 0; // Final Id (table PK for created rows)
                LocaleEnum locale = GlobalizationHelper.GetSystemDataLocale();

                #endregion

                #region Step: Read relationship attribute value details

                if (reader["FK_Relationship"] != null)
                    relationshipId = ValueTypeHelper.Int64TryParse(reader["FK_Relationship"].ToString(), 0);

                if (reader["AttributeId"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), 0);

                if (reader["LocaleId"] != null)
                {
                    Int32 localeId = 0;
                    localeId = ValueTypeHelper.Int32TryParse(reader["LocaleId"].ToString(), 0);
                    locale = (LocaleEnum)localeId;
                }

                if (reader["Sequence"] != null)
                {
                    Decimal decSeq = 0;
                    decSeq = ValueTypeHelper.DecimalTryParse(reader["Sequence"].ToString(), 0);
                    sequence = (Int32)decSeq;
                }

                if (reader["FK_RelationshipAttrVal"] != null)
                {
                    attributeValueId = ValueTypeHelper.Int64TryParse(reader["FK_RelationshipAttrVal"].ToString(), -1);
                }

                #endregion

                #region Step: Updating relationship attribute value instances by recived data

                if (relationshipId <= 0)
                {
                    continue;
                }

                List<Relationship> relationshipInstances = null;
                if (relationshipsIndex.TryGetValue(relationshipId, out relationshipInstances))
                {
                    foreach (Relationship relationshipInstance in relationshipInstances)
                    {
                        ProcessRelationshipAttributesTreeUpdating(relationshipInstance, relationshipInstance.RelationshipAttributes, attributeId, sequence, attributeValueId, locale);
                    }
                }

                #endregion
            }

            #endregion
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="relationshipAttributes"></param>
        /// <param name="attributeId"></param>
        /// <param name="sequence"></param>
        /// <param name="attributeValueId"></param>
        /// <param name="locale"></param>
        private void ProcessRelationshipAttributesTreeUpdating(Relationship relationship, AttributeCollection relationshipAttributes, Int32 attributeId, Int32 sequence, Int64 attributeValueId, LocaleEnum locale)
        {
            if (relationshipAttributes != null && relationshipAttributes.Any())
            {
                Attribute attribute = null;

                IAttribute iAttribute = relationshipAttributes.GetAttribute(attributeId, locale);
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
                    IValueCollection values = null;
                    values = attribute.GetOverriddenValuesInvariant();
                    if (values != null)
                    {
                        IValueCollection deletedValues = null;
                        Boolean isAnyValueDeleted = false;

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

                    if (attribute.IsComplex)
                    {
                        if (attribute.GetChildAttributes() != null && attribute.GetChildAttributes().Any())
                        {
                            foreach (Attribute childAttribute in attribute.GetChildAttributes())
                            {
                                if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                {
                                    ProcessRelationshipAttributesTreeUpdating(relationship, (AttributeCollection)childAttribute.GetChildAttributes(), attributeId, sequence, attributeValueId, locale);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}