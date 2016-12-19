using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    public class ImpactedEntityDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Get Impacted Entities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLog"></param>
        /// <param name="impactType"></param>
        /// <param name="impactedEntityCount"></param>
        /// <param name="command"></param>
        /// <param name="programName"></param>
        /// <param name="getDirectChildren"></param>
        /// <param name="getMDLChildren"></param>
        /// <param name="getMDLHierarchyChildren"></param>
        /// <param name="getParent"></param>
        /// <param name="getMDLParent"></param>
        /// <param name="getMDLsParent"></param>
        /// <param name="getParentsMDL"></param>
        /// <param name="returnResult"></param>
        /// <returns></returns>
        public EntityCollection LoadImpactedEntities(EntityActivityLog entityActivityLog, ImpactType impactType, out Int64 impactedEntityCount, DBCommandProperties command, 
                            String programName, Boolean getDirectChildren = true, Boolean getMDLChildren = true, Boolean getMDLHierarchyChildren = true, Boolean getParent = true, 
                            Boolean getMDLParent = true, Boolean getMDLsParent = true, Boolean getParentsMDL = true, Boolean returnResult = true)
        {
            SqlDataReader reader = null;
            impactedEntityCount = 0;

            var impactedEntities = new EntityCollection();

            try
            {
                var generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Impacted_Load_ParametersArray");

                String attributeIdListAsString = String.Empty;
                String attributeLocaleIdListAsString = String.Empty;
                String relationshipIdListAsString = String.Empty;

                if (entityActivityLog.AttributeIdList != null && entityActivityLog.AttributeIdList.Count > 0)
                {
                    attributeIdListAsString = ValueTypeHelper.JoinCollection(entityActivityLog.AttributeIdList, ",");
                    attributeLocaleIdListAsString = ValueTypeHelper.JoinCollectionGetLocaleIdList(entityActivityLog.AttributeLocaleIdList, ",");
                }

                if (entityActivityLog.RelationshipIdList != null && entityActivityLog.RelationshipIdList.Count > 0)
                {
                    relationshipIdListAsString = ValueTypeHelper.JoinCollection(entityActivityLog.RelationshipIdList, ","); //RelationshipIdList
                }

                parameters[0].Value = entityActivityLog.Id;
                parameters[1].Value = entityActivityLog.EntityId;
                parameters[2].Value = entityActivityLog.ContainerId;
                parameters[3].Value = (Byte)entityActivityLog.PerformedAction;
                parameters[4].Value = attributeIdListAsString;
                parameters[5].Value = attributeLocaleIdListAsString;
                parameters[6].Value = entityActivityLog.RelationshipProcessMode;
                parameters[7].Value = relationshipIdListAsString;
                parameters[8].Value = getDirectChildren;
                parameters[9].Value = getMDLChildren;
                parameters[10].Value = getMDLHierarchyChildren;
                parameters[11].Value = getParent;
                parameters[12].Value = getMDLParent;
                parameters[13].Value = getMDLsParent;
                parameters[14].Value = getParentsMDL;
                parameters[15].Value = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.MDMCenter, "ContextBasedExclusion", "1");
                parameters[16].Value = returnResult;
                parameters[17].Value = programName;

                String storedProcedureName = "usp_EntityManager_Entity_Impacted_Load";

                if (returnResult == true)
                {
                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Int64 sourceEntityId = 0;
                            Int64 impactedEntityId = 0;
                            String name = String.Empty;
                            String longName = String.Empty;
                            Int64 parentEntityId = 0;
                            Int32 containerId = 0;
                            Int64 categoryId = 0;
                            Int32 entityTypeId = 0;

                            if (reader["SourceEntityId"] != null)
                                Int64.TryParse(reader["SourceEntityId"].ToString(), out sourceEntityId);

                            if (reader["Id"] != null)
                                Int64.TryParse(reader["Id"].ToString(), out impactedEntityId);

                            if (reader["ParentEntityId"] != null)
                                Int64.TryParse(reader["ParentEntityId"].ToString(), out parentEntityId);

                            if (reader["ContainerId"] != null)
                                Int32.TryParse(reader["ContainerId"].ToString(), out containerId);

                            if (reader["CategoryId"] != null)
                                Int64.TryParse(reader["CategoryId"].ToString(), out categoryId);

                            if (reader["EntityTypeId"] != null)
                                Int32.TryParse(reader["EntityTypeId"].ToString(), out entityTypeId);

                            var impactedEntity = new Entity(impactedEntityId, 0, name, longName, String.Empty, String.Empty, parentEntityId, String.Empty, 0, String.Empty, containerId, String.Empty, 0, entityTypeId, String.Empty);
                            impactedEntity.BranchLevel = ContainerBranchLevel.Component;
                            impactedEntities.Add(impactedEntity);
                        }
                    }
                }
                else
                {
                    Object result = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                    if (result != null)
                    {
                        impactedEntityCount = ValueTypeHelper.Int64TryParse(result.ToString(), 0);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return impactedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="entityActivityLogId"></param>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public ImpactedEntityCollection GetImpactedEntities(Int32 batchSize, Int64 entityActivityLogId, Int64 fromRecordNumber, Int64 toRecordNumber, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            ImpactedEntityCollection impactedEntities = new ImpactedEntityCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Impacted_Get_ParametersArray");

                parameters[0].Value = batchSize;
                parameters[1].Value = entityActivityLogId;
                parameters[2].Value = fromRecordNumber;
                parameters[3].Value = toRecordNumber;

                storedProcedureName = "usp_EntityManager_Entity_Impacted_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        //Int64 id = -1;
                        Int64 impactedEntityId = -1;
                        Int32 containerId = 0;
                        String impactedEntityName = String.Empty;
                        String impactedEntityLongName = String.Empty;
                        Int16 priority = -1;
                        Boolean isCacheDirty = false;
                        String context = String.Empty;
                        Boolean isAttributeDenormRequired = false;
                        Collection<EntityActivityList> performedActionList = new Collection<EntityActivityList>();
                        Collection<Int32> impactedAttributes = new Collection<Int32>();
                        Collection<LocaleEnum> impactedAttributeLocales = new Collection<LocaleEnum>();
                        Boolean isAttributeDenormInProcess = false;
                        Collection<EntityActivityList> shelvedPerformedActionList = new Collection<EntityActivityList>();
                        Collection<Int64> impactedRelationships = new Collection<Int64>();
                        Collection<Int32> shelvedAttributes = new Collection<Int32>();
                        Collection<LocaleEnum> shelvedAttributeLocales = new Collection<LocaleEnum>();
                        Collection<Int64> shelvedRelationships = new Collection<Int64>();

                        ImpactedEntity impactedEntity = new ImpactedEntity();
                        Int64 returnedEntityActivityLogId = 0;

                        //if(reader["Id"] != null)
                        //    id = ValueTypeHelper.Int64TryParse( reader["Id"].ToString(),-1);

                        if (reader["FK_CNode"] != null)
                            impactedEntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), -1);

                        if (reader["ContainerId"] != null)
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);

                        if (reader["ActionList"] != null)
                        {
                            String strPerformedActionList = reader["ActionList"].ToString();

                            if (!string.IsNullOrEmpty(strPerformedActionList))
                            {
                                string[] strOutput = strPerformedActionList.Split(',');
                                foreach (String s in strOutput)
                                {
                                    Int32 actionId = ValueTypeHelper.Int32TryParse(s, 0);
                                    EntityActivityList entityActivityAction = (EntityActivityList)actionId;
                                    performedActionList.Add(entityActivityAction);
                                }
                            }
                        }

                        if (reader["ShortName"] != null)
                            impactedEntityName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            impactedEntityLongName = reader["LongName"].ToString();

                        if (reader["Priority"] != null)
                            priority = ValueTypeHelper.Int16TryParse(reader["Priority"].ToString(), 0);

                        if (reader["IsCacheDirty"] != null)
                            isCacheDirty = ValueTypeHelper.BooleanTryParse(reader["IsCacheDirty"].ToString(), false);

                        if (reader["Context"] != null)
                        {
                            context = reader["Context"].ToString();
                        }

                        if (reader["IsAttributeDenormRequired"] != null)
                            isAttributeDenormRequired = ValueTypeHelper.BooleanTryParse(reader["IsAttributeDenormRequired"].ToString(), false);

                        if (reader["ShelvedActionList"] != null)
                        {
                            String strShelvedPerformedActionList = reader["ShelvedActionList"].ToString();

                            if (!string.IsNullOrEmpty(strShelvedPerformedActionList))
                            {
                                string[] strOutput = strShelvedPerformedActionList.Split(',');
                                foreach (String s in strOutput)
                                {
                                    Int32 actionId = ValueTypeHelper.Int32TryParse(s, 0);
                                    EntityActivityList entityActivityAction = (EntityActivityList)actionId;
                                    shelvedPerformedActionList.Add(entityActivityAction);
                                }
                            }
                        }

                        if (reader["AttributeList"] != null)
                            impactedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader["AttributeList"].ToString(), ',');

                        if (reader["AttributeLocaleList"] != null)
                            impactedAttributeLocales = ValueTypeHelper.SplitLocaleIdStringToLocaleEnumCollection(reader["AttributeLocaleList"].ToString(), ',');

                        if (reader["IsAttributeDenormInProgress"] != null)
                            isAttributeDenormInProcess = ValueTypeHelper.BooleanTryParse(reader["IsAttributeDenormInProgress"].ToString(), false);

                        if (reader["ShelvedAttributeList"] != null)
                            shelvedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader["ShelvedAttributeList"].ToString(), ',');

                        if (reader["ShelvedAttributeLocaleList"] != null)
                            shelvedAttributeLocales = ValueTypeHelper.SplitLocaleIdStringToLocaleEnumCollection(reader["ShelvedAttributeLocaleList"].ToString(), ',');

                        if (reader["RelationshipIdList"] != null)
                            impactedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader["RelationshipIdList"].ToString(), ',');

                        if (reader["ShelvedRelationshipIdList"] != null)
                            shelvedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader["ShelvedRelationshipIdList"].ToString(), ',');

                        if (reader["EntityActivityLogId"] != null)
                            Int64.TryParse(reader["EntityActivityLogId"].ToString(), out returnedEntityActivityLogId);

                        //impactedEntity.Id = id;
                        impactedEntity.EntityId = impactedEntityId;
                        impactedEntity.EntityName = impactedEntityName;
                        impactedEntity.EntityLongName = impactedEntityLongName;
                        impactedEntity.Priority = priority;
                        impactedEntity.IsCacheDirty = isCacheDirty;
                        impactedEntity.Context = context;
                        impactedEntity.PerformedActionList = performedActionList;
                        impactedEntity.IsEntityDenormRequired = isAttributeDenormRequired;
                        impactedEntity.ImpactedAttributes = impactedAttributes;
                        impactedEntity.ImpactedAttributeLocales = impactedAttributeLocales;
                        impactedEntity.ImpactedRelationships = impactedRelationships;
                        impactedEntity.IsEntityDenormInProcess = isAttributeDenormInProcess;
                        impactedEntity.ShelvedPerformedActionList = shelvedPerformedActionList;
                        impactedEntity.ShelvedAttributes = shelvedAttributes;
                        impactedEntity.ShelvedAttributeLocales = shelvedAttributeLocales;
                        impactedEntity.ShelvedRelationships = shelvedRelationships;
                        impactedEntity.ContainerId = containerId;
                        impactedEntity.EntityActivityLogId = returnedEntityActivityLogId;

                        impactedEntities.Add(impactedEntity);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return impactedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public ImpactedEntityCollection GetImpactedEntities(Collection<Int64> entityIdList, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            List<SqlDataRecord> entityList = null;
            String storedProcedureName = String.Empty;
            var impactedEntities = new ImpactedEntityCollection();

            try
            {
                var generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("usp_EntityManager_Entity_Impacted_GetByEntityIdList_ParametersArray");

                #region Populate entityIdList table value parameters

                if (entityIdList != null)
                {
                    entityList = new List<SqlDataRecord>();

                    var entityMetadata = generator.GetTableValueMetadata("usp_EntityManager_Entity_Impacted_GetByEntityIdList_ParametersArray", parameters[0].ParameterName);

                    if (entityIdList.Count > 0)
                    {
                        foreach (Int64 entityId in entityIdList)
                        {
                            var entityRecord = new SqlDataRecord(entityMetadata);
                            entityRecord.SetValue(0, entityId); // entityId
                            entityList.Add(entityRecord);
                        }
                    }
                    else
                    {
                        entityList = null;
                    }
                }

                #endregion Populate entityIdList table value parameters

                parameters[0].Value = entityList;

                storedProcedureName = "usp_EntityManager_Entity_Impacted_GetByEntityIdList";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        //Int64 id = -1;
                        Int64 impactedEntityId = -1;
                        Int32 containerId = 0;
                        String impactedEntityName = String.Empty;
                        String impactedEntityLongName = String.Empty;
                        Int16 priority = -1;
                        Boolean isCacheDirty = false;
                        Boolean isAttributeDenormRequired = false;
                        Collection<EntityActivityList> performedActionList = new Collection<EntityActivityList>();
                        Collection<Int32> impactedAttributes = new Collection<Int32>();
                        Collection<LocaleEnum> impactedAttributeLocales = new Collection<LocaleEnum>();
                        Boolean isAttributeDenormInProcess = false;
                        Collection<EntityActivityList> shelvedPerformedActionList = new Collection<EntityActivityList>();
                        Collection<Int64> impactedRelationships = new Collection<Int64>();
                        Collection<Int32> shelvedAttributes = new Collection<Int32>();
                        Collection<LocaleEnum> shelvedAttributeLocales = new Collection<LocaleEnum>();
                        Collection<Int64> shelvedRelationships = new Collection<Int64>();

                        ImpactedEntity impactedEntity = new ImpactedEntity();
                        Int64 returnedEntityActivityLogId = 0;

                        //if(reader["Id"] != null)
                        //    id = ValueTypeHelper.Int64TryParse( reader["Id"].ToString(),-1);

                        if (reader["FK_CNode"] != null)
                            impactedEntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), -1);

                        if (reader["ContainerId"] != null)
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);

                        if (reader["ActionList"] != null)
                        {
                            String strPerformedActionList = reader["ActionList"].ToString();

                            if (!string.IsNullOrEmpty(strPerformedActionList))
                            {
                                string[] strOutput = strPerformedActionList.Split(',');
                                foreach (String s in strOutput)
                                {
                                    Int32 actionId = ValueTypeHelper.Int32TryParse(s, 0);
                                    EntityActivityList entityActivityAction = (EntityActivityList)actionId;
                                    performedActionList.Add(entityActivityAction);
                                }
                            }
                        }

                        if (reader["ShortName"] != null)
                            impactedEntityName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            impactedEntityLongName = reader["LongName"].ToString();

                        if (reader["Priority"] != null)
                            priority = ValueTypeHelper.Int16TryParse(reader["Priority"].ToString(), 0);

                        if (reader["IsCacheDirty"] != null)
                            isCacheDirty = ValueTypeHelper.BooleanTryParse(reader["IsCacheDirty"].ToString(), false);

                        if (reader["IsAttributeDenormRequired"] != null)
                            isAttributeDenormRequired = ValueTypeHelper.BooleanTryParse(reader["IsAttributeDenormRequired"].ToString(), false);

                        if (reader["ShelvedActionList"] != null)
                        {
                            String strShelvedPerformedActionList = reader["ShelvedActionList"].ToString();

                            if (!string.IsNullOrEmpty(strShelvedPerformedActionList))
                            {
                                string[] strOutput = strShelvedPerformedActionList.Split(',');
                                foreach (String s in strOutput)
                                {
                                    Int32 actionId = ValueTypeHelper.Int32TryParse(s, 0);
                                    EntityActivityList entityActivityAction = (EntityActivityList)actionId;
                                    shelvedPerformedActionList.Add(entityActivityAction);
                                }
                            }
                        }

                        if (reader["AttributeList"] != null)
                            impactedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader["AttributeList"].ToString(), ',');

                        if (reader["AttributeLocaleList"] != null)
                            impactedAttributeLocales = ValueTypeHelper.SplitLocaleIdStringToLocaleEnumCollection(reader["AttributeLocaleList"].ToString(), ',');

                        if (reader["IsAttributeDenormInProgress"] != null)
                            isAttributeDenormInProcess = ValueTypeHelper.BooleanTryParse(reader["IsAttributeDenormInProgress"].ToString(), false);

                        if (reader["ShelvedAttributeList"] != null)
                            shelvedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader["ShelvedAttributeList"].ToString(), ',');

                        if (reader["ShelvedAttributeLocaleList"] != null)
                            shelvedAttributeLocales = ValueTypeHelper.SplitLocaleIdStringToLocaleEnumCollection(reader["ShelvedAttributeLocaleList"].ToString(), ',');

                        if (reader["RelationshipIdList"] != null)
                            impactedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader["RelationshipIdList"].ToString(), ',');

                        if (reader["ShelvedRelationshipIdList"] != null)
                            shelvedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader["ShelvedRelationshipIdList"].ToString(), ',');

                        if (reader["EntityActivityLogId"] != null)
                            Int64.TryParse(reader["EntityActivityLogId"].ToString(), out returnedEntityActivityLogId);

                        //impactedEntity.Id = id;
                        impactedEntity.EntityId = impactedEntityId;
                        impactedEntity.EntityName = impactedEntityName;
                        impactedEntity.EntityLongName = impactedEntityLongName;
                        impactedEntity.Priority = priority;
                        impactedEntity.IsCacheDirty = isCacheDirty;
                        impactedEntity.PerformedActionList = performedActionList;
                        impactedEntity.IsEntityDenormRequired = isAttributeDenormRequired;
                        impactedEntity.ImpactedAttributes = impactedAttributes;
                        impactedEntity.ImpactedAttributeLocales = impactedAttributeLocales;
                        impactedEntity.ImpactedRelationships = impactedRelationships;
                        impactedEntity.IsEntityDenormInProcess = isAttributeDenormInProcess;
                        impactedEntity.ShelvedPerformedActionList = shelvedPerformedActionList;
                        impactedEntity.ShelvedAttributes = shelvedAttributes;
                        impactedEntity.ShelvedAttributeLocales = shelvedAttributeLocales;
                        impactedEntity.ShelvedRelationships = shelvedRelationships;
                        impactedEntity.ContainerId = containerId;
                        impactedEntity.EntityActivityLogId = returnedEntityActivityLogId;

                        impactedEntities.Add(impactedEntity);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return impactedEntities;
        }

        #endregion Get Impacted Entities

        #region Process Impacted Entities

        public EntityOperationResultCollection UpdateEntityStatus(ImpactedEntityCollection impactedEntities, DBCommandProperties command, String impactAction)
        {
            EntityOperationResultCollection entityOperationResults = null;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityDA.Process", false);

                SqlDataReader reader = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Impacted_Process_ParametersArray");

                try
                {
                    entityOperationResults = PrepareEntityOperationResultsSchema(impactedEntities);

                    foreach (ImpactedEntity impactedEntity in impactedEntities)
                    {
                        String attributeIdList = String.Empty;
                        String attributeLocaleIdList = String.Empty;
                        String relationhipIdList = String.Empty;
                        String performedActionList = String.Empty;

                        #region Create comma separated string from collection

                        if (impactedEntity.ImpactedAttributes != null && impactedEntity.ImpactedAttributes.Count > 0)
                        {
                            attributeIdList = ValueTypeHelper.JoinCollection(impactedEntity.ImpactedAttributes, ",");
                        }

                        if (impactedEntity.ImpactedAttributeLocales != null && impactedEntity.ImpactedAttributeLocales.Count > 0)
                        {
                            StringBuilder sbLocaleIdList = new StringBuilder();

                            foreach (LocaleEnum locale in impactedEntity.ImpactedAttributeLocales)
                            {
                                if (sbLocaleIdList.Length > 0)
                                {
                                    sbLocaleIdList.Append(",");
                                }
                                sbLocaleIdList.Append((Int32)locale);
                            }

                            attributeLocaleIdList = sbLocaleIdList.ToString();
                        }

                        if (impactedEntity.ImpactedRelationships != null && impactedEntity.ImpactedRelationships.Count > 0)
                        {
                            relationhipIdList = ValueTypeHelper.JoinCollection(impactedEntity.ImpactedRelationships, ",");
                        }

                        if (impactedEntity.PerformedActionList != null && impactedEntity.PerformedActionList.Count > 0)
                        {
                            StringBuilder sbEntityActivityList = new StringBuilder();

                            foreach (EntityActivityList entityActivityList in impactedEntity.PerformedActionList)
                            {
                                if (sbEntityActivityList.Length > 0)
                                {
                                    sbEntityActivityList.Append(",");
                                }
                                sbEntityActivityList.Append((Int32)entityActivityList);
                            }

                            performedActionList = sbEntityActivityList.ToString();
                        }

                        #endregion Create comma separated string from collection

                        parameters[0].Value = impactedEntity.EntityId;
                        parameters[1].Value = impactedEntity.ContainerId;
                        parameters[2].Value = performedActionList;
                        parameters[3].Value = attributeIdList;
                        parameters[4].Value = attributeLocaleIdList;
                        parameters[5].Value = relationhipIdList;
                        parameters[6].Value = impactAction;

                        String storedProcedureName = "usp_EntityManager_Entity_Impacted_Process";

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (reader != null)
                        {
                            UpdateEntityOperationResults(reader, impactedEntities, entityOperationResults);
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityDA.Process");
            }

            return entityOperationResults;
        }

        /// <summary>
        /// Inserts Impacted Entities in Bulk
        /// </summary>
        /// <param name="impactedEntities">collection of impacted entities</param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean ImpactedEntityBulkProcess(EntityCollection impactedEntities, DBCommandProperties command)
        {
            Boolean result = false;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityDA.ImpactedEntityBulkProcess", false);

                SqlDataReader reader = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Impacted_Bulk_Process_ParametersArray");

                try
                {
                    List<SqlDataRecord> impactedEntitylList = new List<SqlDataRecord>();
                    String storedProcedureName = "usp_EntityManager_Entity_Impacted_InitialLoad";
                    SqlMetaData[] impactedEntityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_Impacted_Bulk_Process_ParametersArray", parameters[0].ParameterName);

                    foreach (Entity impactedEntity in impactedEntities)
                    {
                       
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for impacted Entity starting...");

                        SqlDataRecord impactedEntityRecord = new SqlDataRecord(impactedEntityMetadata);
                        impactedEntityRecord = FillSqlDataRecord(impactedEntityRecord, impactedEntity);
                        impactedEntitylList.Add(impactedEntityRecord);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for impacted Entity completed...");
                    }
                    
                    parameters[0].Value = impactedEntitylList;
                    
                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    while (reader.Read())
                    {
                        if (reader != null)
                        {
                            if (reader["Result"] != null)
                            {
                                Int32 intResult = 0;
                                Int32.TryParse(reader["Result"].ToString(), out intResult);
                                result = ValueTypeHelper.ConvertToBooleanFromInteger(intResult);
                            }
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                     if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityDA.Process");
                }

                if (result)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Committing Transaction");

                    transactionScope.Complete();
                }

                return result;
             }
        }

        #endregion Process Impacted Entities

        #region Check cache status

        /// <summary>
        /// Get cache status of given entity. 
        /// This will return value based on if entity is available in tb_Impacted_Entity if impacted entities are loaded or based on available parent in tb_EntityActivityLog.
        /// </summary>
        /// <param name="entityId">Entity for which cache status is to be checked</param>
        /// <param name="containerId">Container Id of entity</param>
        /// <param name="entityTreeIdList">Parent entity id list</param>
        /// <param name="command">Object having connection string and other information</param>
        /// <returns>
        /// Tuple indicating cache status. Values in tuple are as following:
        /// Item1 -> CurrentEntityId
        /// Item2 -> CacheStatus enum indicating cache status
        /// ITem3 -> If CacheStatus = DirectDirty then PK_Impacted_Entity. Else 0
        /// </returns>
        public Tuple<Int64, CacheStatus, Int64> GetEntityCacheStatus(Int64 entityId, Int32 containerId, String entityTreeIdList, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            List<SqlDataRecord> entityIdList = null;

            Tuple<Int64, CacheStatus, Int64> entityCacheStatus = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Impacted_GetCacheStatus_ParametersArray");

                #region Populate entity tree id list table value parameters

                if (!String.IsNullOrWhiteSpace(entityTreeIdList))
                {
                    entityIdList = new List<SqlDataRecord>();

                    SqlMetaData[] metaData = generator.GetTableValueMetadata("EntityManager_Entity_Impacted_GetCacheStatus_ParametersArray", parameters[2].ParameterName);

                    foreach (String id in entityTreeIdList.Split(','))
                    {
                        SqlDataRecord entityRecord = new SqlDataRecord(metaData);

                        entityRecord.SetValue(0, ValueTypeHelper.Int64TryParse(id, -1)); // entityId

                        entityIdList.Add(entityRecord);
                    }
                }
                else
                {
                    entityIdList = null;
                }

                #endregion Populate entityIdList table value parameters

                parameters[0].Value = entityId;
                parameters[1].Value = containerId;
                parameters[2].Value = entityIdList;

                storedProcedureName = "usp_EntityManager_Entity_Impacted_CacheStatus_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    CacheStatus cacheStatus = CacheStatus.Unknown;
                    Int64 referenceId = -1;

                    while (reader.Read())
                    {
                        if (reader["CacheStatus"] != null)
                        {
                            cacheStatus = (CacheStatus)ValueTypeHelper.Int32TryParse(reader["CacheStatus"].ToString(), 0);
                        }

                        if (reader["PK_Impacted_Entity"] != null)
                        {
                            referenceId = ValueTypeHelper.Int64TryParse(reader["PK_Impacted_Entity"].ToString(), -1);
                        }

                        entityCacheStatus = new Tuple<Int64, CacheStatus, Int64>(entityId, cacheStatus, referenceId);
                    }
                }

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityCacheStatus;
        }

        #endregion Check cache status

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impactedEntities"></param>
        /// <returns></returns>
        private EntityOperationResultCollection PrepareEntityOperationResultsSchema(ImpactedEntityCollection impactedEntities)
        {
            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();
            Int64 impactedEntityIdToBeCreated = -1;

            foreach (ImpactedEntity impactedEntity in impactedEntities)
            {
                EntityOperationResult entityOperationResult = new EntityOperationResult(impactedEntity.Id, impactedEntity.Id, impactedEntity.LongName);

                if (impactedEntity.Id < 1)
                {
                    impactedEntity.Id = impactedEntityIdToBeCreated;
                    entityOperationResult.EntityId = impactedEntityIdToBeCreated;
                    impactedEntityIdToBeCreated--;
                }

                entityOperationResults.Add(entityOperationResult);
            }

            return entityOperationResults;
        }

        /// <summary>
        /// Fills sqldata record with values in all columns like entity id, container id
        /// </summary>
        /// <param name="attributeModelMetadata"></param>
        /// <param name="attributeModel"></param>
        /// <returns></returns>
        private SqlDataRecord FillSqlDataRecord(SqlDataRecord impactedEntitySqlDataRecord, Entity entity)
        {
            impactedEntitySqlDataRecord.SetValue(0, entity.Id);
            impactedEntitySqlDataRecord.SetValue(1, entity.ContainerId);
            impactedEntitySqlDataRecord.SetValue(2, String.Empty);
            impactedEntitySqlDataRecord.SetValue(3, String.Empty);
            return impactedEntitySqlDataRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="impactedEntities"></param>
        /// <param name="entityOperationResults"></param>
        public static void UpdateEntityOperationResults(SqlDataReader reader, ImpactedEntityCollection impactedEntities, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int64 entityId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["EntityId"] != null)
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();



                //Get Entity
                ImpactedEntity impactedEntity = impactedEntities.SingleOrDefault(e => e.EntityId == id);

                //Get the entity operation result
                EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == id);

                if (impactedEntity != null && entityOperationResult != null)
                {
                    if (id < 0)
                    {
                        //Update the id with the new entityId
                        impactedEntity.Id = entityId;
                        entityOperationResult.EntityId = entityId;
                    }

                    if (hasError)
                    {
                        //Add error
                        entityOperationResults.AddEntityOperationResult(entityOperationResult.EntityId, String.Empty, errorMessage, OperationResultType.Error);
                    }
                    else
                    {
                        //No errors.. update status as Successful.
                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion
    }

}