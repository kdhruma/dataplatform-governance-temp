using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for Entity State Validation Management.
    /// </summary>
    public class EntityStateValidationDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityStateValidationDA()
        {
            this._currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the state of the entity validation for single or family entities
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="command">Indicates the database command properties.</param>
        /// <returns>Result of the entity state validation</returns>
        public EntityStateValidationCollection Get(Collection<Int64> entityIds, Boolean needGlobalFamilyErrors, Boolean needVariantFamilyErrors, DBCommandProperties command)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            EntityStateValidationCollection entityStateValidations = null;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_EntityManager_EntityValidation_Error_Get";
            String connectionString = command.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                parameters = generator.GetParameters("EntityManager_Entity_StateValidation_Get_ParametersArray");

                #region Populate table value parameters

                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_StateValidation_Get_ParametersArray", parameters[0].ParameterName);

                List<SqlDataRecord> entityList = EntityDataReaderUtility.CreateEntityIdTable(entityIds, entityMetadata);

                #endregion

                parameters[0].Value = entityList;
                parameters[1].Value = needGlobalFamilyErrors;
                parameters[2].Value = needVariantFamilyErrors;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                entityStateValidations = this.PopulateEntityStateValidations(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityStateValidations;
        }

        /// <summary>
        /// Gets the entity validation score records for requested entity ids.
        /// </summary>
        /// <param name="entityIdList">Indicates the entity ids</param>
        /// <param name="command">Indicates the database command properties.</param>
        /// <returns>Returns EntityValidationScoreCollection.</returns>
        public EntityStateValidationScoreCollection GetEntityStateValidationScores(Collection<Int64> entityIdList, DBCommandProperties command)
        {
            #region Initial Setup

            EntityStateValidationScoreCollection entityStateValidationScores = null;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_EntityManager_EntityScore_Get";
            String connectionString = command.ConnectionString;

            #endregion Initial Setup

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("EntityStateValidationScores get is started.");
            }

            #endregion Diagnostics & Tracing

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_StateValidation_Score_Get_ParametersArray");
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_StateValidation_Score_Get_ParametersArray", parameters[0].ParameterName);

                List<SqlDataRecord> entityIds = EntityDataReaderUtility.CreateEntityIdTable(entityIdList, entityMetadata);

                parameters[0].Value = entityIds;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed.", storedProcedureName));
                }

                entityStateValidationScores = this.ReadEntityStateValidationScores(reader);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity validation score reading is completed.");
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("EntityValidationScore Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityStateValidationScores;
        }

        /// <summary>
        /// Gets entity business conditions based on given entity ids.
        /// </summary>
        /// <param name="entityIdList">Indicates the entity ids</param>
        /// <param name="command">Indicates the database command properties.</param>
        /// <returns>Returns EntityBusinessConditionCollection</returns>
        public EntityBusinessConditionCollection GetEntityBusinessConditions(Collection<Int64> entityIdList, DBCommandProperties command)
        {
            #region Initial Setup

            EntityBusinessConditionCollection entityBusinessConditions = null;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_EntityManager_Entity_BusinessCondition_Get";
            String connectionString = command.ConnectionString;

            #endregion Initial Setup

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & Tracing

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_BusinessConditions_Get_ParametersArray");
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_BusinessConditions_Get_ParametersArray", parameters[0].ParameterName);

                List<SqlDataRecord> entityIds = EntityDataReaderUtility.CreateEntityIdTable(entityIdList, entityMetadata);

                parameters[0].Value = entityIds;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed.", storedProcedureName));
                }

                entityBusinessConditions = this.ReadEntityBusinessConditions(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("EntityBusinessConditons Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityBusinessConditions;
        }

        /// <summary>
        /// Gets lock type for given entity global family id list
        /// </summary>
        /// <param name="entityGlobalFamilyIdList">Indicates entity global family id list to check lock type for them</param>
        /// <param name="command">Indicates the database command properties.</param>
        /// <returns>Returns dictionary pointing to lock type for each requested entity global family id</returns>
        public Dictionary<Int64, LockType> GetEntityLocks(Collection<Int64> entityGlobalFamilyIdList, DBCommandProperties command)
        {
            #region Initial Setup

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_EntityManager_EntityFamilyLock_Get";
            String parameterConfigName = "EntityManager_Entity_EntityFamilyLock_Get_ParametersArray";
            String connectionString = command.ConnectionString;
            Dictionary<Int64, LockType> lockTypeForEntityFamilies = null;

            #endregion Initial Setup

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & Tracing

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                parameters = generator.GetParameters(parameterConfigName);
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);
                List<SqlDataRecord> entityGlobalFamilyIds = EntityDataReaderUtility.CreateEntityIdTable(entityGlobalFamilyIdList, entityMetadata);

                parameters[0].Value = entityGlobalFamilyIds;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed.", storedProcedureName));
                }

                lockTypeForEntityFamilies = this.ReadLockType(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entities' lock status Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return lockTypeForEntityFamilies;
        }

        /// <summary>
        /// Processes the state of the entity validation.
        /// </summary>
        /// <param name="entities">Indicates the entity collection.</param>
        /// <param name="entityValidationStates">Indicates the entity validation states.</param>
        /// <param name="entityOperationResults">Indicates the entity operation resutls.</param>
        /// <param name="loginUser">Indicates the login user </param>
        /// <param name="programName">Indicates the program name </param>
        /// <param name="command">Indicates the database command properties.</param>
        public void Process(EntityCollection entities, EntityStateValidationCollection entityValidationStates,EntityOperationResultCollection entityOperationResults,  String loginUser, String programName, DBCommandProperties command)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlDataReader reader = null;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_StateValidation_Process_ParametersArray");

                    String paramKeyName = "EntityManager_Entity_StateValidation_Process_ParametersArray";

                    #region Populate table value parameters

                    SqlMetaData[] sqlMetaData = generator.GetTableValueMetadata(paramKeyName, parameters[0].ParameterName);
                    List<SqlDataRecord> entityValidationStatesList = this.CreateEntityValidationStatesTable(entityValidationStates, sqlMetaData);

                    sqlMetaData = generator.GetTableValueMetadata(paramKeyName, parameters[1].ParameterName);
                    List<SqlDataRecord> entityAttributeList = this.CreateEntityAttributeListTable(entities, sqlMetaData);

                    sqlMetaData = generator.GetTableValueMetadata(paramKeyName, parameters[2].ParameterName);
                    List<SqlDataRecord> passedBusinessConditionList = this.CreatePassedBusinessConditionsTable(entityOperationResults, sqlMetaData);

                    #endregion Populate table value parameters

                    parameters[0].Value = entityValidationStatesList;
                    parameters[1].Value = entityAttributeList;
                    parameters[2].Value = passedBusinessConditionList;
                    parameters[3].Value = loginUser;
                    parameters[4].Value = programName;

                    String storedProcedureName = "usp_EntityManager_EntityValidation_Error_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        #endregion Public Methods

        #region Private Methods

        #region Entity State Validation Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="sqlMetadata"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateEntityAttributeListTable(EntityCollection entities, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> entityAttributeListTable = null;

            if (entities != null && entities.Count > 0)
            {
                entityAttributeListTable = new List<SqlDataRecord>();

                foreach (Entity entity in entities)
                {
                    if (entity.ValidationStates.IsMetaDataValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityMetaDataValid, entity.ValidationStates.IsMetaDataValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsCommonAttributesValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityCommonAttributesValid, entity.ValidationStates.IsCommonAttributesValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsCategoryAttributesValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityCategoryAttributesValid, entity.ValidationStates.IsCategoryAttributesValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsRelationshipsValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityRelationshipsValid, entity.ValidationStates.IsRelationshipsValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsExtensionsValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityExtensionsValid, entity.ValidationStates.IsExtensionsValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsEntityVariantValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntityVariantValid, entity.ValidationStates.IsEntityVariantValid, sqlMetadata));
                    }

                    if (entity.ValidationStates.IsSelfValid != ValidityStateValue.NotChecked)
                    {
                        entityAttributeListTable.Add(this.SetEntityAttributeValue(entity.Id, SystemAttributes.EntitySelfValid, entity.ValidationStates.IsSelfValid, sqlMetadata));
                    }
                }
            }

            return entityAttributeListTable;
        }

        /// <summary>
        /// Creates the entity validation states table.
        /// </summary>
        /// <param name="entityStateValidationCollection">Indicates the entity validation state collection.</param>
        /// <param name="sqlMetadata">Indicates the SQL locales metadata.</param>
        /// <returns>Returns data records.</returns>
        private List<SqlDataRecord> CreateEntityValidationStatesTable(EntityStateValidationCollection entityStateValidationCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> entityValidationStatesList = null;

            if (entityStateValidationCollection != null && entityStateValidationCollection.Count > 0)
            {
                entityValidationStatesList = new List<SqlDataRecord>();

                foreach (EntityStateValidation entityValidationState in entityStateValidationCollection)
                {
                    SqlDataRecord entityValidationStatesRecord = new SqlDataRecord(sqlMetadata);

                    String attributeTypeValue = String.Empty;
                    string operationResultTypeValue = String.Empty;

                    switch (entityValidationState.AttributeModelType)
                    {
                        case AttributeModelType.Common:
                            attributeTypeValue = "CA";
                            break;
                        case AttributeModelType.Category:
                            attributeTypeValue = "CC";
                            break;
                        case AttributeModelType.Relationship:
                            attributeTypeValue = "RA";
                            break;
                    }

                    switch (entityValidationState.OperationResultType)
                    {
                        case OperationResultType.Error:
                            operationResultTypeValue = "E";
                            break;
                        case OperationResultType.Information:
                            operationResultTypeValue = "I";
                            break;
                        case OperationResultType.Warning:
                            operationResultTypeValue = "W";
                            break;
                    }

                    entityValidationStatesRecord.SetValue(0, (Int32)entityValidationState.SystemValidationStateAttribute);
                    entityValidationStatesRecord.SetValue(1, (Int32)entityValidationState.ReasonType);
                    entityValidationStatesRecord.SetValue(2, entityValidationState.EntityId);
                    entityValidationStatesRecord.SetValue(3, entityValidationState.ContainerId);
                    entityValidationStatesRecord.SetValue(4, entityValidationState.RelationshipId);
                    entityValidationStatesRecord.SetValue(5, entityValidationState.JobId);
                    entityValidationStatesRecord.SetValue(6, attributeTypeValue);
                    entityValidationStatesRecord.SetValue(7, entityValidationState.AttributeId);
                    entityValidationStatesRecord.SetValue(8, (Int32)entityValidationState.Locale);
                    entityValidationStatesRecord.SetValue(9, operationResultTypeValue);
                    entityValidationStatesRecord.SetValue(10, entityValidationState.MessageCode);
                    entityValidationStatesRecord.SetValue(11, (entityValidationState.MessageParameters != null && entityValidationState.MessageParameters.Count > 0) ? String.Join("#@#", entityValidationState.MessageParameters) : String.Empty);
                    entityValidationStatesRecord.SetValue(12, entityValidationState.RuleMapContextId);
                    entityValidationStatesRecord.SetValue(13, entityValidationState.RuleId);
                    entityValidationStatesRecord.SetValue(14, entityValidationState.Action.ToString());
                    entityValidationStatesList.Add(entityValidationStatesRecord);
                }
            }

            return entityValidationStatesList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResults"></param>
        /// <param name="sqlMetadata"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreatePassedBusinessConditionsTable(EntityOperationResultCollection entityOperationResults, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> passedBusinessConditionListTable = null;

            if (entityOperationResults != null && entityOperationResults.Count > 0)
            {
                passedBusinessConditionListTable = new List<SqlDataRecord>();

                foreach (EntityOperationResult entityOperationResult in entityOperationResults)
                {
                    Int64 entityId = entityOperationResult.EntityId;

                    if (entityOperationResult.PassedBusinessConditionIdList.Count > 0)
                    {
                        foreach (Int32 businessConditionId in entityOperationResult.PassedBusinessConditionIdList)
                        {
                            SqlDataRecord passedBusinessConditionRecord = new SqlDataRecord(sqlMetadata);

                            passedBusinessConditionRecord.SetValue(0, entityId);
                            passedBusinessConditionRecord.SetValue(1, businessConditionId);

                            passedBusinessConditionListTable.Add(passedBusinessConditionRecord);
                        }
                    }
                }

                if (passedBusinessConditionListTable.Count == 0)
                {
                    passedBusinessConditionListTable = null;
                }
            }

            return passedBusinessConditionListTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="validationStateAttribute"></param>
        /// <param name="validationStateValue"></param>
        /// <param name="sqlLocalesMetadata"></param>
        /// <returns></returns>
        private SqlDataRecord SetEntityAttributeValue(Int64 entityId, SystemAttributes validationStateAttribute, ValidityStateValue validationStateValue, SqlMetaData[] sqlLocalesMetadata)
        {
            SqlDataRecord entityAttributeValueRecord = new SqlDataRecord(sqlLocalesMetadata);
            entityAttributeValueRecord.SetValue(0, entityId);
            entityAttributeValueRecord.SetValue(1, (Int32)validationStateAttribute);
            entityAttributeValueRecord.SetValue(2, (validationStateValue == ValidityStateValue.Valid) ? true : false);
            return entityAttributeValueRecord;
        }

        /// <summary>
        /// Population of entity validation state details
        /// </summary>
        /// <param name="reader">Indicates the SqlDataReader</param>
        /// <returns>
        /// Returns entity validation state collection
        /// </returns>
        private EntityStateValidationCollection PopulateEntityStateValidations(SqlDataReader reader)
        {
            EntityStateValidationCollection entityStateValidations = new EntityStateValidationCollection();

            while (reader.Read())
            {
                EntityStateValidation entityStateValidation = new EntityStateValidation();

                if (reader["SystemStateAttribute"] != null)
                {
                    entityStateValidation.SystemValidationStateAttribute = (SystemAttributes)(ValueTypeHelper.Int32TryParse(reader["SystemStateAttribute"].ToString(), 0));
                }

                if (reader["EntityId"] != null)
                {
                    entityStateValidation.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), -1);
                }

                if (reader["EntityName"] != null)
                {
                    entityStateValidation.EntityName = reader["EntityName"].ToString();
                }

                if (reader["EntityLongName"] != null)
                {
                    entityStateValidation.EntityLongName = reader["EntityLongName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    entityStateValidation.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["CategoryLongName"] != null)
                {
                    entityStateValidation.CategoryLongName = reader["CategoryLongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    entityStateValidation.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), -1);
                }

                if (reader["ContainerName"] != null)
                {
                    entityStateValidation.ContainerName = reader["ContainerName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    entityStateValidation.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["Relationship"] != null)
                {
                    entityStateValidation.RelationshipId = ValueTypeHelper.Int64TryParse(reader["Relationship"].ToString(), -1);
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    entityStateValidation.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["RelatedEntityId"] != null)
                {
                    entityStateValidation.RelatedEntityId = ValueTypeHelper.Int64TryParse(reader["RelatedEntityId"].ToString(), -1);
                }

                if (reader["RelatedEntityName"] != null)
                {
                    entityStateValidation.RelatedEntityName = reader["RelatedEntityName"].ToString();
                }

                if (reader["RelatedEntityLongName"] != null)
                {
                    entityStateValidation.RelatedEntityLongName = reader["RelatedEntityLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    entityStateValidation.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), -1);
                }

                if (reader["AttributeName"] != null)
                {
                    entityStateValidation.AttributeName = reader["AttributeName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    entityStateValidation.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["AttrType"] != null)
                {
                    String attrType = reader["AttrType"].ToString().Trim();
                    AttributeModelType attributeModelType = AttributeModelType.Unknown;

                    if (String.Compare(attrType, "CA", true) == 0)
                    {
                        attributeModelType = AttributeModelType.Common;
                    }

                    if (String.Compare(attrType, "CC", true) == 0)
                    {
                        attributeModelType = AttributeModelType.Category;
                    }

                    if (String.Compare(attrType, "RA", true) == 0)
                    {
                        attributeModelType = AttributeModelType.Relationship;
                    }

                    entityStateValidation.AttributeModelType = attributeModelType;
                }

                if (reader["MessageCode"] != null)
                {
                    entityStateValidation.MessageCode = reader["MessageCode"].ToString();
                }

                if (reader["MessageParameter"] != null)
                {
                    entityStateValidation.MessageParameters = ValueTypeHelper.SplitStringToGenericCollection<Object>(reader["MessageParameter"].ToString(), "#@#", StringSplitOptions.RemoveEmptyEntries);
                }

                if (reader["RuleMapContext"] != null)
                {
                    entityStateValidation.RuleMapContextId = ValueTypeHelper.Int32TryParse(reader["RuleMapContext"].ToString(), entityStateValidation.RuleMapContextId);
                }

                if (reader["RuleMapContextName"] != null)
                {
                    entityStateValidation.RuleMapContextName = reader["RuleMapContextName"].ToString();
                }

                if (reader["RuleId"] != null)
                {
                    entityStateValidation.RuleId = ValueTypeHelper.Int32TryParse(reader["RuleId"].ToString(), entityStateValidation.RuleId);
                }

                if (reader["RuleName"] != null)
                {
                    entityStateValidation.RuleName = reader["RuleName"].ToString();
                }

                if (reader["OperationResultType"] != null)
                {
                    String operationResult = reader["OperationResultType"].ToString().Trim();
                    OperationResultType operationResultType = OperationResultType.Ignore;

                    if (String.Compare(operationResult, "E", true) == 0)
                    {
                        operationResultType = OperationResultType.Error;
                    }

                    if (String.Compare(operationResult, "W", true) == 0)
                    {
                        operationResultType = OperationResultType.Warning;
                    }

                    if (String.Compare(operationResult, "I", true) == 0)
                    {
                        operationResultType = OperationResultType.Information;
                    }

                    entityStateValidation.OperationResultType = operationResultType;
                }

                if (reader["LocaleId"] != null)
                {
                    entityStateValidation.Locale = (LocaleEnum)ValueTypeHelper.Int32TryParse(reader["LocaleId"].ToString(), 0);
                }

                if (reader["ReasonTypeId"] != null)
                {
                    entityStateValidation.ReasonType = (ReasonType)ValueTypeHelper.Int32TryParse(reader["ReasonTypeId"].ToString(), 1);
                }

                if (reader["JobId"] != null)
                {
                    entityStateValidation.JobId = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), 0);
                }

                if (reader["ModDateTime"] != null)
                {
                    entityStateValidation.AuditTimeStamp = ValueTypeHelper.DateTimeTryParse(reader["ModDateTime"].ToString(), DateTime.MinValue, entityStateValidation.Locale);
                }

                entityStateValidations.Add(entityStateValidation);
            }

            return entityStateValidations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        private Dictionary<Int64, LockType> ReadLockType(SqlDataReader reader)
        {
            Int64 key = -1;
            LockType lockType = LockType.Unknown;
            Dictionary<Int64, LockType> lockTypePerEntityGlobalFamilyId = new Dictionary<Int64, LockType>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader["EntityGlobalFamilyId"] != null)
                    {
                        key = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), key);
                    }

                    if (reader["LockType"] != null)
                    {
                        ValueTypeHelper.EnumTryParse<LockType>(reader["LockType"].ToString(), false, out lockType);
                    }

                    if (key > 0 && !lockTypePerEntityGlobalFamilyId.ContainsKey(key))
                    {
                        lockTypePerEntityGlobalFamilyId.Add(key, lockType);
                    }
                }
            }

            return lockTypePerEntityGlobalFamilyId;
        }

        #endregion Entity State Validation Helper Methods

        #region  Entity State  Validation Score Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private EntityStateValidationScoreCollection ReadEntityStateValidationScores(SqlDataReader reader)
        {
            EntityStateValidationScoreCollection entityValidationScores = new EntityStateValidationScoreCollection();

            if (reader != null)
            {
                this.PopulateEntityStateValidationScores(entityValidationScores, reader);

                reader.NextResult();

                this.PopulateEntityStateValidationAttributeScores(entityValidationScores, reader); 
            }

            return entityValidationScores;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private void PopulateEntityStateValidationScores(EntityStateValidationScoreCollection eVScoreCollection, SqlDataReader reader)
        {
            while(reader.Read())
            {
                EntityStateValidationScore entityValidationScore = new EntityStateValidationScore();

                if (reader["EntityId"] != null)
                {
                    entityValidationScore.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), entityValidationScore.EntityId);
                }

                if (reader["OverAllScore"] != null)
                {
                    entityValidationScore.OverallScore = ValueTypeHelper.DoubleTryParse(reader["OverAllScore"].ToString(), entityValidationScore.OverallScore);
                }

                eVScoreCollection.Add(entityValidationScore);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityValidationScoreCollection"></param>
        /// <param name="reader"></param>
        private void PopulateEntityStateValidationAttributeScores(EntityStateValidationScoreCollection entityValidationScoreCollection, SqlDataReader reader)
        {
            Int64 entityId = -1;
            EntityStateValidationScore entityValidationScore = new EntityStateValidationScore();

            SystemAttributes entityStateValidationAttribute = SystemAttributes.EntitySelfValid;

            EntityStateValidationAttributeScore attributeScore = null;
            EntityStateValidationAttributeScoreCollection attributeScoreCollection = new EntityStateValidationAttributeScoreCollection();

            while (reader.Read())
            {
                #region Read entity state validation attribute score properties

                attributeScore = new EntityStateValidationAttributeScore();

                if (reader["EntityId"] != null)
                {
                    entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), entityId);
                }

                if (reader["AttributeId"] != null)
                {
                    ValueTypeHelper.EnumTryParse<SystemAttributes>(reader["AttributeId"].ToString(), false, out entityStateValidationAttribute);
                    attributeScore.StateValidationAttribute = entityStateValidationAttribute;
                }

                if (reader["ScorePercentage"] != null)
                {
                    attributeScore.Score = ValueTypeHelper.DoubleTryParse(reader["ScorePercentage"].ToString(), attributeScore.Score);
                }

                if (reader["Weightage"] != null)
                {
                    attributeScore.Weightage = ValueTypeHelper.Int32TryParse(reader["Weightage"].ToString(), attributeScore.Weightage);
                }

                #endregion Read entity state validation attribute score properties

                #region Prepare respective entity validation score object

                if (entityValidationScore.EntityId != entityId)
                {
                    entityValidationScore = (EntityStateValidationScore)entityValidationScoreCollection.GetByEntityId(entityId);
                }

                if (entityValidationScore != null)
                {
                    entityValidationScore.EntityStateValidationAttributeScores.Add(attributeScore);
                }

                #endregion Prepare respective entity validation score object
            }
        }

        #endregion  Entity State  Validation Score Helper Methods

        #region  Entity Business Conditions Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private EntityBusinessConditionCollection ReadEntityBusinessConditions(SqlDataReader reader)
        {
            EntityBusinessConditionCollection entityBusinessConditions = null;

            if (reader != null)
            {
                entityBusinessConditions = new EntityBusinessConditionCollection();

                while (reader.Read())
                {
                    EntityBusinessCondition entityBusinessCondition = new EntityBusinessCondition();

                    if (reader["PK_CNode"] != null)
                    {
                        entityBusinessCondition.EntityId = ValueTypeHelper.Int64TryParse(reader["PK_CNode"].ToString(), entityBusinessCondition.EntityId);
                    }

                    if (reader["EntityFamilyId"] != null)
                    {
                        entityBusinessCondition.EntityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), entityBusinessCondition.EntityFamilyId);
                    }

                    if (reader["EntityGlobalFamilyId"] != null)
                    {
                        entityBusinessCondition.EntityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), entityBusinessCondition.EntityGlobalFamilyId);
                    }

                    entityBusinessConditions.Add(entityBusinessCondition);
                }

                reader.NextResult();

                if (entityBusinessConditions.Count > 0)
                {
                    Dictionary<Int64, BusinessConditionStatusCollection> businessConditionStatuses = EntityDataReaderUtility.ReadBusinessConditions(reader);

                    foreach (KeyValuePair<Int64, BusinessConditionStatusCollection> businessConditionStatus in businessConditionStatuses)
                    {
                        EntityBusinessCondition entityBusinessCondition = (EntityBusinessCondition)entityBusinessConditions.GetByEntityId(businessConditionStatus.Key);

                        if (entityBusinessCondition != null)
                        {
                            entityBusinessCondition.BusinessConditions = businessConditionStatus.Value;
                        }
                    }
                }
            }

            return entityBusinessConditions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessConditions"></param>
        /// <param name="columns"></param>
        /// <param name="reader"></param>
        private void PopulateBusinessConditions(BusinessConditionStatusCollection businessConditions, Collection<String> columns, SqlDataReader reader)
        {
            BusinessConditionStatus businessCondition = null;

            if (reader != null)
            {
                if (columns != null && columns.Count > 0)
                {
                    foreach (String columnName in columns)
                    {
                        businessCondition = new BusinessConditionStatus();

                        businessCondition.Id = ValueTypeHelper.Int32TryParse(columnName.Replace("BC", String.Empty), businessCondition.Id);
                        
                        if (reader[columnName] != null)
                        {
                            if (reader[columnName] == DBNull.Value)
                            {
                                businessCondition.Status = ValidityStateValue.NotChecked;
                            }
                            else
                            {
                                bool valid = ValueTypeHelper.BooleanTryParse(reader[columnName].ToString(), false);
                                if (valid)
                                {
                                    businessCondition.Status = ValidityStateValue.Valid;
                                }
                                else
                                {
                                    businessCondition.Status = ValidityStateValue.InValid;
                                }
                            }
                        }

                        businessConditions.Add(businessCondition);
                    }
                }
            }
        }

        #endregion  Entity Business Conditions Helper Methods

        #endregion Private Methods

        #endregion Methods
    }
}