using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Data Access for Entity History
    /// </summary>
    public class EntityHistoryDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String SQLParametersGeneratorName = "EntityManager_SqlParameters";
        private const String EntityHistoryProcessName = "EntityManager_EntityHistory_Get_ParametersArray";
        
        #endregion

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties
        
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entity collection history based on entity ids and history context provided.
        /// </summary>
        /// <param name="entityIds">Collection of entity ids for which we need entity history </param>
        /// <param name="entityHistoryContext">Entity history context to get history</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <returns>Entity collection History Object</returns>
        public EntityCollectionHistory GetEntityCollectionHistory(Collection<Int64> entityIds, EntityHistoryContext entityHistoryContext, String excludeAttributeIds, String excludeExtensionRelationshipIds, String excludeExtensionRelationshipTypeIds, String excludeWorkflowIds, String excludeChildEntityTypeIds, List<KeyValuePair<Int32, Int32>> excludeRelationshipIdToAttributeList, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryDA.GetEntityCollectionHistory", MDMTraceSource.Entity, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            EntityCollectionHistory entityCollectionHistory = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);
                parameters = generator.GetParameters(EntityHistoryProcessName);

                #region Populate table value parameters for Entity ids

                List<SqlDataRecord> entityIdList = new List<SqlDataRecord>();
                SqlMetaData[] entityIdMetadata = generator.GetTableValueMetadata(EntityHistoryProcessName, parameters[0].ParameterName);
                SqlDataRecord entityIdRecord = null;

                foreach (Int64 entityId in entityIds)
                {
                    entityIdRecord = new SqlDataRecord(entityIdMetadata);
                    entityIdRecord.SetValue(0, (Int64)entityId);
                    entityIdList.Add(entityIdRecord);
                }

                #endregion

                #region Populate table value parameters for Attribute ids

                List<SqlDataRecord> attributeIdList = null;

                if (entityHistoryContext.AttributeIdList != null && entityHistoryContext.AttributeIdList.Count > 0)
                {
                    attributeIdList = new List<SqlDataRecord>();
                    SqlMetaData[] attributeIdMetadata = generator.GetTableValueMetadata(EntityHistoryProcessName, parameters[1].ParameterName);
                    SqlDataRecord attributeIdRecord = null;

                    foreach (Int32 attributeId in entityHistoryContext.AttributeIdList)
                    {
                        attributeIdRecord = new SqlDataRecord(attributeIdMetadata);
                        attributeIdRecord.SetValue(0, (Int32)attributeId);
                        attributeIdList.Add(attributeIdRecord);
                    }
                }
                #endregion

                #region Populate table value parameters for relationship type ids

                List<SqlDataRecord> relationshipTypeIdList = null;

                if (entityHistoryContext.RelationshipTypeIdList != null && entityHistoryContext.RelationshipTypeIdList.Count > 0)
                {
                    relationshipTypeIdList = new List<SqlDataRecord>();
                    SqlMetaData[] relationshipTypeIdMetadata = generator.GetTableValueMetadata(EntityHistoryProcessName, parameters[2].ParameterName);
                    SqlDataRecord relationshipTypeIdRecord = null;

                    foreach (Int32 relationshipTypeId in entityHistoryContext.RelationshipTypeIdList)
                    {
                        relationshipTypeIdRecord = new SqlDataRecord(relationshipTypeIdMetadata);
                        relationshipTypeIdRecord.SetValue(0, (Int32)relationshipTypeId);
                        relationshipTypeIdList.Add(relationshipTypeIdRecord);
                    }
                }
                #endregion

                #region Populate table value parameters for exclude list relationship type and attribute id list pair

                List<SqlDataRecord> relationshipExcludeList = null;

                if (excludeRelationshipIdToAttributeList != null && excludeRelationshipIdToAttributeList.Count > 0)
                {
                    relationshipExcludeList = new List<SqlDataRecord>();
                    SqlMetaData[] relationshipExcludeListMetadata = generator.GetTableValueMetadata(EntityHistoryProcessName, parameters[19].ParameterName);
                    SqlDataRecord relationshipExcludeListRecord = null;

                    foreach (KeyValuePair<Int32, Int32> relationshipTypeAttributePair in excludeRelationshipIdToAttributeList)
                    {
                        relationshipExcludeListRecord = new SqlDataRecord(relationshipExcludeListMetadata);
                        relationshipExcludeListRecord.SetValue(0, relationshipTypeAttributePair.Key);
                        relationshipExcludeListRecord.SetValue(1, relationshipTypeAttributePair.Value);

                        relationshipExcludeList.Add(relationshipExcludeListRecord);
                    }
                }
                #endregion

                #region Populate table value parameters for Locale ids

                List<SqlDataRecord> localeIdList = null;
                
                // If history for single attribute is requested- i.e. attribute version history then only consider data locale else get history details across all locales.
                if(entityHistoryContext.AttributeIdList!=null && entityHistoryContext.AttributeIdList.Count>0)
                {
                    // Assumption: currently we are passing only  one data locale to get attribute version history. But using locale TVP here for future use/modification.
                    localeIdList = new List<SqlDataRecord>();
                    SqlMetaData[] localeIdMetadata = generator.GetTableValueMetadata(EntityHistoryProcessName, parameters[20].ParameterName);
                    SqlDataRecord localeIdRecord = null;

                    localeIdRecord = new SqlDataRecord(localeIdMetadata);

                    localeIdRecord.SetValue(0, (Int32)entityHistoryContext.CurrentDataLocale);
                    localeIdRecord.SetValue(1, entityHistoryContext.CurrentDataLocale.ToString());

                    LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
                    if (entityHistoryContext.CurrentDataLocale == systemDataLocale)
                    {
                        localeIdRecord.SetValue(2, true);
                    }
                    else
                    {
                        localeIdRecord.SetValue(2, false);                        
                    }

                    localeIdList.Add(localeIdRecord);
                }

                #endregion

                #region Parameter Population

                parameters[0].Value = entityIdList;
                parameters[1].Value = attributeIdList;
                parameters[2].Value = relationshipTypeIdList;
                parameters[3].Value = entityHistoryContext.ContainerId;
                parameters[4].Value = entityHistoryContext.LoadMetadataVersionDetails;
                parameters[5].Value = entityHistoryContext.LoadAttributesVersionDetails;
                parameters[6].Value = entityHistoryContext.LoadRelationshipsVersionDetails;
                parameters[7].Value = entityHistoryContext.LoadExtensionRelationshipsVersionDetails;
                parameters[8].Value = entityHistoryContext.LoadHierarchyRelationshipsVersionDetails;
                parameters[9].Value = entityHistoryContext.LoadWorkflowVersionDetails;
                parameters[10].Value = entityHistoryContext.LoadPromoteVersionDetails;

                if (entityHistoryContext.FromDateTime == null)
                {
                    parameters[11].Value = DBNull.Value;
                }
                else
                {
                    parameters[11].Value = entityHistoryContext.FromDateTime;
                }

                if (entityHistoryContext.ToDateTime == null)
                {
                    parameters[12].Value = DBNull.Value;
                }
                else
                {
                    parameters[12].Value = entityHistoryContext.ToDateTime;
                }

                parameters[13].Value = entityHistoryContext.MaxRecordsToReturn;

                if (excludeExtensionRelationshipTypeIds != null && !String.IsNullOrWhiteSpace(excludeExtensionRelationshipTypeIds))
                {
                    parameters[14].Value = excludeExtensionRelationshipTypeIds;
                }
                else
                {
                    parameters[14].Value = DBNull.Value;
                }

                if (excludeAttributeIds != null && !String.IsNullOrWhiteSpace(excludeAttributeIds))
                {
                    parameters[15].Value = excludeAttributeIds;
                }
                else
                {
                    parameters[15].Value = DBNull.Value;
                }

                if (excludeWorkflowIds != null && !String.IsNullOrWhiteSpace(excludeWorkflowIds))
                {
                    parameters[16].Value = excludeWorkflowIds;
                }
                else
                {
                    parameters[16].Value = DBNull.Value;
                }

                if (excludeChildEntityTypeIds != null && !String.IsNullOrWhiteSpace(excludeChildEntityTypeIds))
                {
                    parameters[17].Value = excludeChildEntityTypeIds;
                }
                else
                {
                    parameters[17].Value = DBNull.Value;
                }

                if (excludeExtensionRelationshipTypeIds != null && !String.IsNullOrWhiteSpace(excludeExtensionRelationshipTypeIds))
                {
                    parameters[18].Value = excludeExtensionRelationshipTypeIds;
                }
                else
                {
                    parameters[18].Value = DBNull.Value;
                }

                parameters[19].Value = relationshipExcludeList;
                parameters[20].Value = localeIdList;
                parameters[21].Value = entityHistoryContext.RelationshipId;

                #endregion

                storedProcedureName = "usp_EntityManager_EntityHistory_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                #region Reading reader data

                entityCollectionHistory = new EntityCollectionHistory();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["EntityId"] != null)
                        {
                            Int64 entityId = -1;
                            Int64.TryParse(reader["EntityId"].ToString(), out entityId);

                            if (entityId > 0)
                            {
                                EntityHistory entityHistory = (EntityHistory)entityCollectionHistory.GetEntityHistory(entityId);
                                EntityHistoryRecord entityHistoryRecord = PopulateEntityHistoryRecord(reader);

                                if (entityHistory != null)
                                {
                                    entityHistory.Add(entityHistoryRecord);
                                }
                                else
                                {
                                    entityHistory = new EntityHistory();
                                    entityHistory.EntityId = entityId;

                                    entityHistory.Add(entityHistoryRecord);
                                    entityCollectionHistory.Add(entityHistory);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryDA.GetEntityCollectionHistory", MDMTraceSource.Entity);
                }
            }

            return entityCollectionHistory;
        }

        /// <summary>
        /// Get all Entity History Details template collection
        /// </summary>
        /// <param name="command">Object having command properties</param>
        /// <returns>Returns collection of entity history details template</returns>
        public EntityHistoryDetailsTemplateCollection GetEntityHistoryDetailsTemplates(DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHistoryDA.GetEntityHistoryDetailsTemplates", MDMTraceSource.Entity, false);
            }

            EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplates = null;

            SqlDataReader reader = null;
            SqlParameter[] parameters = null;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            try
            {
                storedProcedureName = "usp_EntityManager_EntityHistoryDetailsTemplate_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting PopulateEntityHistoryDetailsTemplates...", MDMTraceSource.Entity);
                }

                entityHistoryDetailsTemplates = PopulateEntityHistoryDetailsTemplates(reader);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Populated EntityHistoryDetailsTemplates...", MDMTraceSource.Entity);
                }
            }

            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHistoryDA.GetEntityDetailsTemplate", MDMTraceSource.Entity);
                }
            }

            return entityHistoryDetailsTemplates; 
          }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Populate Entity History Record object from reader
        /// </summary>
        /// <param name="reader">Indicates the reader</param>
        /// <returns>Returns Entity History Record</returns>
        private EntityHistoryRecord PopulateEntityHistoryRecord(SqlDataReader reader)
        {
            EntityHistoryRecord entityHistoryRecord = new EntityHistoryRecord();

            #region Populating Entity history record data from reader

            if (reader["ChangeType"] != null)
            {
                EntityChangeType entityChangeType = EntityChangeType.Unknown;
                Enum.TryParse<EntityChangeType>(reader["ChangeType"].ToString(), out entityChangeType);
                entityHistoryRecord.ChangeType = entityChangeType;
            }

            if (reader["EntityLongName"] != null && !String.IsNullOrWhiteSpace(reader["EntityLongName"].ToString()))
            {
                entityHistoryRecord.ChangedData_EntityLongName = reader["EntityLongName"].ToString();
            }

            if (reader["AuditRefId"] != null)
            {
                Int64 auditRefId = ValueTypeHelper.Int64TryParse(reader["AuditRefId"].ToString(), -1);
                entityHistoryRecord.AuditRefId = auditRefId;
            }

            if (reader["EntityCategoryId"] != null)
            {
                Int64 entityCategoryId = ValueTypeHelper.Int64TryParse(reader["EntityCategoryId"].ToString(), -1);
                entityHistoryRecord.ChangedData_CategoryId = entityCategoryId;
            }

            if (reader["ChildEntityId"] != null || reader["ExtendedEntityId"] != null || reader["RelatedEntityId"] != null)
            {
                Int64 childEntityId = ValueTypeHelper.Int64TryParse(reader["ChildEntityId"].ToString(), -1);
                Int64 extendedEntityId = ValueTypeHelper.Int64TryParse(reader["ExtendedEntityId"].ToString(), -1);
                Int64 relatedEntityId = ValueTypeHelper.Int64TryParse(reader["RelatedEntityId"].ToString(), -1);
                
                // Assumption : In any cases there will be only one value available.  
                if (childEntityId > 0)
                {
                    entityHistoryRecord.ChangedData_RelatedEntityId = childEntityId;
                }
                else if (extendedEntityId > 0)
                {
                    entityHistoryRecord.ChangedData_RelatedEntityId = extendedEntityId;
                }
                else if (relatedEntityId > 0)
                {
                    entityHistoryRecord.ChangedData_RelatedEntityId = relatedEntityId;
                }
            }

            if (reader["ExtensionContainerId"] != null)
            {
                Int32 extensionContainerId = ValueTypeHelper.Int32TryParse(reader["ExtensionContainerId"].ToString(), -1);
                entityHistoryRecord.ChangedData_ExtensionContainerId = extensionContainerId;
            }

            if (reader["ExtensionCategoryId"] != null)
            {
                Int64 extensionCategoryId = ValueTypeHelper.Int64TryParse(reader["ExtensionCategoryId"].ToString(), -1);
                entityHistoryRecord.ChangedData_ExtensionCategoryId = extensionCategoryId;
            }

            if (reader["RelationshipTypeId"] != null)
            {
                Int32 relationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), -1);
                entityHistoryRecord.ChangedData_RelationshipTypeId = relationshipTypeId;
            }

            if (reader["AttributeId"] != null)
            {
                Int32 attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), -1);
                entityHistoryRecord.ChangedData_AttributeId = attributeId;
            }

            if (reader["LocaleId"] != null)
            {
                LocaleEnum localeEnum = LocaleEnum.en_WW;
                Enum.TryParse<LocaleEnum>(reader["LocaleId"].ToString(), out localeEnum);
                entityHistoryRecord.Locale = localeEnum;
            }

            if (reader["UOMId"] != null)
            {
                Int32 UOMId = ValueTypeHelper.Int32TryParse(reader["UOMId"].ToString(), -1);
                entityHistoryRecord.ChangedData_UOMId = UOMId;
            }

            if (reader["Seq"] != null)
            {
                Int32 seq = ValueTypeHelper.Int32TryParse(reader["Seq"].ToString(), -1);
                entityHistoryRecord.ChangedData_Seq = seq;
            }

            if (reader["AttrValue"] != null && reader["AttrValue"].ToString() != null)
            {
                entityHistoryRecord.ChangedData_AttrVal = reader["AttrValue"].ToString();
            }

            if (reader["IsInvalidData"] != null)
            {
                entityHistoryRecord.IsInvalidData = ValueTypeHelper.BooleanTryParse(reader["IsInvalidData"].ToString(), false);
            }

            if (reader["ModDateTime"] != null)
            {
                entityHistoryRecord.ModifiedDateTime = ValueTypeHelper.ConvertToDateTime(reader["ModDateTime"].ToString());
            }

            if (reader["ModUser"] != null && !String.IsNullOrWhiteSpace(reader["ModUser"].ToString()))
            {
                entityHistoryRecord.ModifiedUser = reader["ModUser"].ToString();
            }

            if (reader["ModProgram"] != null && !String.IsNullOrWhiteSpace(reader["ModProgram"].ToString()))
            {
                entityHistoryRecord.ModifiedProgram = reader["ModProgram"].ToString();
            }

            if (reader["Action"] != null)
            {
                entityHistoryRecord.Action = GetAction(reader["Action"].ToString());
            }

			if (reader["Source"] != null && !String.IsNullOrWhiteSpace(reader["Source"].ToString()))
			{
				entityHistoryRecord.Source = reader["Source"].ToString();
			}

			if (reader["WorkflowRuntimeInstanceId"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowRuntimeInstanceId"].ToString()))
            {
                entityHistoryRecord.WorkflowRuntimeInstanceId = reader["WorkflowRuntimeInstanceId"].ToString();
            }

            if (reader["WorkflowActivityLongName"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowActivityLongName"].ToString()))
            {
                entityHistoryRecord.WorkflowActivityLongName = reader["WorkflowActivityLongName"].ToString();
            }

            if (reader["WorkflowVersionName"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowVersionName"].ToString()))
            {
                entityHistoryRecord.WorkflowVersionName = reader["WorkflowVersionName"].ToString();
            }

            if (reader["WorkflowName"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowName"].ToString()))
            {
                entityHistoryRecord.WorkflowName = reader["WorkflowName"].ToString();
            }

            if (reader["WorkflowComments"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowComments"].ToString()))
            {
                entityHistoryRecord.WorkflowComments = reader["WorkflowComments"].ToString();
            }

            if (reader["WorkflowPerformedAction"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowPerformedAction"].ToString()))
            {
                entityHistoryRecord.WorkflowActivityActionTaken = reader["WorkflowPerformedAction"].ToString();
            }

            if (reader["WorkflowPrevAssignedUser"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowPrevAssignedUser"].ToString()))
            {
                entityHistoryRecord.WorkflowPreviousAssignedUser = reader["WorkflowPrevAssignedUser"].ToString();
            }

            if (reader["WorkflowCurrentAssignedUser"] != null && !String.IsNullOrWhiteSpace(reader["WorkflowCurrentAssignedUser"].ToString()))
            {
                entityHistoryRecord.WorkflowCurrentAssignedUser = reader["WorkflowCurrentAssignedUser"].ToString();
            }

            if (reader["PromoteMessageCode"] != null)
            {
                entityHistoryRecord.PromoteMessageCode = reader["PromoteMessageCode"].ToString();
            }

            if (reader["PromoteMessageParam"] != null)
            {
                entityHistoryRecord.PromoteMessageParams = reader["PromoteMessageParam"].ToString();
            }

            #endregion

            return entityHistoryRecord;
        }

        /// <summary>
        /// Populate EntityHistoryDetailsTemplateCollection object from reader
        /// </summary>
        /// <param name="reader">Indicates the reader</param>
        /// <returns>Returns Entity History Details Template Collection</returns>
        private EntityHistoryDetailsTemplateCollection PopulateEntityHistoryDetailsTemplates(SqlDataReader reader)
        {
            EntityHistoryDetailsTemplate entityHistoryDetailsTemplate = null;
            EntityHistoryDetailsTemplateCollection entityHistoryDetailsTemplateCollection = new EntityHistoryDetailsTemplateCollection();

            try
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        entityHistoryDetailsTemplate = new EntityHistoryDetailsTemplate();

                        String strChangeType = String.Empty;
                        EntityChangeType entityChangeType = EntityChangeType.Unknown;

                        if (reader["ChangeType"] != null)
                        {
                            strChangeType = reader["ChangeType"].ToString();
                            Enum.TryParse<EntityChangeType>(strChangeType, out entityChangeType);
                        }

                        entityHistoryDetailsTemplate.ChangeType = entityChangeType;
                        
                        if (reader["TemplateCode"] != null)
                            entityHistoryDetailsTemplate.TemplateCode = reader["TemplateCode"].ToString();

                        if (reader["Action"] != null)
                        {
                            entityHistoryDetailsTemplate.Action = GetAction(reader["Action"].ToString());
                        }

                        if (reader["Description"] != null)
                            entityHistoryDetailsTemplate.Description = reader["Description"].ToString();

                        entityHistoryDetailsTemplateCollection.Add(entityHistoryDetailsTemplate);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityHistoryDetailsTemplateCollection;
        }

        /// <summary>
        /// Get ObjectAction enum from db actions.
        /// </summary>
        /// <param name="action">db action</param>
        /// <returns>MDM Object Action</returns>
        private static ObjectAction GetAction(String action)
        {
            ObjectAction objectAction = ObjectAction.Read;

            if (action != null)
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
                        objectAction = ObjectAction.Update;
                        break;
                    case "PMOVE":
                        objectAction = ObjectAction.Update;
                        break;
                    default:
                        objectAction = ObjectAction.Unknown;
                        break;
                }
            }

            return objectAction;
        }

        #endregion
        
        #endregion
    }
}
