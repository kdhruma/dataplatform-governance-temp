using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

using Microsoft.SqlServer.Server;

namespace MDM.DataModelManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies data logic operations for entity variant definition.
    /// </summary>
    public class EntityVariantDefinitionDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        #endregion

        #region Constructors

        public EntityVariantDefinitionDA()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all entity variant definition collection
        /// </summary>
        /// <returns>Returns all entity variant definition collection</returns>
        public EntityVariantDefinitionCollection GetAll(DBCommandProperties command, CallerContext callerContext)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();
            SqlDataReader reader = null;
            StringBuilder sBuilder = new StringBuilder();

            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = command.ConnectionString;

            var currentSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();

            if (currentSettings.IsBasicTracingEnabled)
            {
                currentActivity.Start();
            }

            try
            {
                storedProcedureName = "usp_DataModelManager_EntityVariantDefinition_Get";

                reader = ExecuteProcedureReader(connectionString, null, storedProcedureName);

                entityVariantDefinitions = this.GetEntityVariantCollection(reader, callerContext);

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (currentSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return entityVariantDefinitions;
        }

        /// <summary>
        /// Process entity variant definition based on the specified application context
        /// </summary>
        /// <param name="entityVariantDefinitions">Indicates the entity variant definitions collections to be processed</param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext">Indicates the context of the caller specifying the calleer application and module.</param>
        /// <param name="command">Contains all the database related properties such as connection string</param>
        /// <param name="loginUser">Indicates the login user name</param>
        /// <param name="programName">Indicates the program name which initiates the processing of entity variant definition</param>
        /// <returns>Returns a boolean value indicating if the process is successful or not</returns>
        public void Process(EntityVariantDefinitionCollection entityVariantDefinitions, OperationResultCollection operationResults, DBCommandProperties command, String loginUser, String programName)
        {
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = command.ConnectionString;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion

            try
            {

                storedProcedureName = "usp_DataModelManager_EntityVariantDefinition_Process";

                PopulateParamtersForProcess(entityVariantDefinitions, loginUser, programName, storedProcedureName, out parameters);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    PopulateOperationResult(reader, operationResults, entityVariantDefinitions);
                }

            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

        }

        /// <summary>
        /// Process entity variant definition
        /// </summary>
        /// <param name="entityVariantDefinitions">Indicates the entity variant definitions collections to be processed</param>
        /// <param name="callerContext">Indicates the context of the caller specifying the calleer application and module.</param>
        /// <param name="command">Contains all the database related properties such as connection string</param>
        /// <param name="loginUser">Indicates the login user name</param>
        /// <param name="programName">Indicates the program name which initiates the processing of entity variant definition</param>
        public void Process(EntityVariantDefinitionCollection entityVariantDefinitions, DataModelOperationResultCollection operationResults, DBCommandProperties command, String loginUser, String programName)
        {
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = command.ConnectionString;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion

            try
            {

                storedProcedureName = "usp_DataModelManager_EntityVariantDefinition_Process";

                PopulateParamtersForProcess(entityVariantDefinitions, loginUser, programName, storedProcedureName, out parameters);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    PopulateDataModelOperationResult(reader, operationResults, entityVariantDefinitions);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create entity variant definition TVP for process
        /// </summary>
        /// <param name="entityVariantDefinitions">Indicates entity variant definition collection</param>
        /// <param name="entityVariantDefinitionMetaData">Metadata of entity variant definition TVP</param>
        /// <param name="entityVariantLevelMetaData">Metadata of entity variant level TVP</param>
        /// <param name="entityVariantAttributeMetaData">Metadata of entity variant attribute TVP</param>
        /// <param name="entityVariantDefinitionTable">Data record of entity variant definition</param>
        /// <param name="entityVariantLevelTable">Data record of entity variant definition</param>
        /// <param name="entityVariantAAttributeTable">Data record of entity variant attribute</param>
        private void CreateEntityVariantDefinitionTableParams(EntityVariantDefinitionCollection entityVariantDefinitions, SqlMetaData[] entityVariantDefinitionMetaData,
                                        SqlMetaData[] entityVariantLevelMetaData, SqlMetaData[] entityVariantAttributeMetaData, out List<SqlDataRecord> entityVariantDefinitionTable,
                                        out List<SqlDataRecord> entityVariantLevelTable, out List<SqlDataRecord> entityVariantAttributeTable)
        {
            entityVariantDefinitionTable = new List<SqlDataRecord>();
            entityVariantLevelTable = new List<SqlDataRecord>();
            entityVariantAttributeTable = new List<SqlDataRecord>();

            if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                {
                    SqlDataRecord entityVariantDefinitionRecord = new SqlDataRecord(entityVariantDefinitionMetaData);
                    entityVariantDefinitionRecord.SetValue(0, ValueTypeHelper.Int32TryParse(entityVariantDefinition.ReferenceId, 0));
                    entityVariantDefinitionRecord.SetValue(1, entityVariantDefinition.Id);
                    entityVariantDefinitionRecord.SetValue(2, entityVariantDefinition.Name);
                    entityVariantDefinitionRecord.SetValue(3, entityVariantDefinition.RootEntityTypeId);
                    entityVariantDefinitionRecord.SetValue(4, entityVariantDefinition.HasDimensionAttributes);
                    entityVariantDefinitionRecord.SetValue(5, entityVariantDefinition.Action.ToString());

                    entityVariantDefinitionTable.Add(entityVariantDefinitionRecord);

                    if (entityVariantDefinition.EntityVariantLevels != null && entityVariantDefinition.EntityVariantLevels.Count > 0)
                    {
                        foreach (EntityVariantLevel entityVariantLevel in entityVariantDefinition.EntityVariantLevels)
                        {
                            SqlDataRecord entityVariantLevelRecord = new SqlDataRecord(entityVariantLevelMetaData);
                            entityVariantLevelRecord.SetValue(0, ValueTypeHelper.Int32TryParse(entityVariantLevel.ReferenceId, 0));
                            entityVariantLevelRecord.SetValue(1, ValueTypeHelper.Int32TryParse(entityVariantDefinition.ReferenceId, 0));
                            entityVariantLevelRecord.SetValue(2, entityVariantLevel.EntityTypeId);
                            entityVariantLevelRecord.SetValue(3, entityVariantLevel.Rank);

                            entityVariantLevelTable.Add(entityVariantLevelRecord);

                            if (entityVariantDefinition.HasDimensionAttributes)
                            {
                                foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in entityVariantLevel.RuleAttributes)
                                {
                                    SqlDataRecord entityVariantAttributeRecord = new SqlDataRecord(entityVariantAttributeMetaData);
                                    entityVariantAttributeRecord.SetValue(0, ValueTypeHelper.Int32TryParse(entityVariantRuleAttribute.ReferenceId, 0));
                                    entityVariantAttributeRecord.SetValue(1, ValueTypeHelper.Int32TryParse(entityVariantDefinition.ReferenceId, 0));
                                    entityVariantAttributeRecord.SetValue(2, ValueTypeHelper.Int32TryParse(entityVariantLevel.ReferenceId, 0));
                                    entityVariantAttributeRecord.SetValue(3, entityVariantRuleAttribute.SourceAttributeId);
                                    entityVariantAttributeRecord.SetValue(4, entityVariantRuleAttribute.TargetAttributeId);
                                    entityVariantAttributeRecord.SetValue(5, entityVariantRuleAttribute.IsOptional);

                                    entityVariantAttributeTable.Add(entityVariantAttributeRecord);
                                }
                            }
                        }
                    }
                }
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (entityVariantDefinitionTable.Count == 0)
            {
                entityVariantDefinitionTable = null;
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (entityVariantLevelTable.Count == 0)
            {
                entityVariantLevelTable = null;
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (entityVariantAttributeTable.Count == 0)
            {
                entityVariantAttributeTable = null;
            }
        }

        /// <summary>
        /// Create application Context TVP for Get
        /// </summary>
        /// <param name="applicationContext">Indicates application context information</param>
        /// <param name="applicationContextMetadata">Metadata of application context TVP</param>
        /// <param name="applicationContextTable">Data record of application context</param>
        private void CreateApplicationContextTableParams(ApplicationContext applicationContext, SqlMetaData[] applicationContextMetadata,
                                       out List<SqlDataRecord> applicationContextTable)
        {
            applicationContextTable = new List<SqlDataRecord>();

            Int32 objectId = 0; //Not using in SP, need to remove it 

            SqlDataRecord applicationContextRecord = new SqlDataRecord(applicationContextMetadata);
            applicationContextRecord.SetValue(0, applicationContext.Id);
            applicationContextRecord.SetValue(1, applicationContext.ObjectTypeId);
            applicationContextRecord.SetValue(2, objectId);
            applicationContextRecord.SetValue(3, objectId);//ApplicationContext type
            applicationContextRecord.SetValue(4, applicationContext.Name);
            applicationContextRecord.SetValue(5, applicationContext.LongName);
            applicationContextRecord.SetValue(6, applicationContext.OrganizationId);
            applicationContextRecord.SetValue(7, applicationContext.ContainerId);
            applicationContextRecord.SetValue(8, applicationContext.CategoryId);
            applicationContextRecord.SetValue(9, applicationContext.EntityTypeId);
            applicationContextRecord.SetValue(10, applicationContext.AttributeId);
            applicationContextRecord.SetValue(11, applicationContext.RelationshipTypeId);
            applicationContextRecord.SetValue(12, applicationContext.RoleId);
            applicationContextRecord.SetValue(13, applicationContext.UserId);
            applicationContextRecord.SetValue(14, applicationContext.Action.ToString());

            applicationContextTable.Add(applicationContextRecord);

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (applicationContextTable.Count == 0)
                applicationContextTable = null;
        }

        private EntityVariantDefinitionCollection GetEntityVariantCollection(SqlDataReader reader, CallerContext context = null)
        {

            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (context != null)
                    {
                        ExecutionContext executionContext = new ExecutionContext();
                        executionContext.CallerContext = context;

                        diagnosticActivity.Start(executionContext);
                    }
                    else
                        diagnosticActivity.Start();
                }

                ReadEntityVariantDefinitions(reader, entityVariantDefinitions);

                if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
                {
                    reader.NextResult();
                    ReadVariantLevelProperties(reader, entityVariantDefinitions, context);

                    reader.NextResult();
                    ReadVariantAttributesProperties(reader, entityVariantDefinitions, context);
                }
            }

            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityVariantDefinitions;
        }

        private void ReadEntityVariantDefinitions(SqlDataReader reader, EntityVariantDefinitionCollection entityVariantDefinitions)
        {
            while (reader.Read())
            {
                Int32 entityVariantDefinitionId = 0;
                String name = String.Empty;
                String longName = String.Empty;
                Int32 rootEntityTypeId = 0;
                String rootEntityTypeName = String.Empty;
                String rootEntityTypeLongName = String.Empty;
                Boolean hasDimensionAttributes = true;

                if (reader["PK_EntityVariant_Definition"] != null) ///TODO: this name to be changed when returned by DB
                {
                    entityVariantDefinitionId = ValueTypeHelper.Int32TryParse(reader["PK_EntityVariant_Definition"].ToString(), 0);
                }

                if (reader["ShortName"] != null)
                {
                    name = reader["ShortName"].ToString();
                }

                if (reader["FK_EntityType"] != null)
                {
                    rootEntityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_EntityType"].ToString(), 0);
                }

                if (reader["EntityTypeShortName"] != null)
                {
                    rootEntityTypeName = reader["EntityTypeShortName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    rootEntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["HasDimensionAttributes"] != null)
                {
                    hasDimensionAttributes = ValueTypeHelper.BooleanTryParse(reader["HasDimensionAttributes"].ToString(), true);
                }

                EntityVariantDefinition entityVariantDefinition = new EntityVariantDefinition
                {
                    Id = entityVariantDefinitionId,
                    Name = name,
                    RootEntityTypeId = rootEntityTypeId,
                    RootEntityTypeName = rootEntityTypeName,
                    RootEntityTypeLongName = rootEntityTypeLongName,
                    HasDimensionAttributes = hasDimensionAttributes
                };

                entityVariantDefinitions.Add(entityVariantDefinition);
            }
        }

        private void ReadVariantLevelProperties(SqlDataReader reader, EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            while (reader.Read())
            {
                Int32 entityVariantDefinitionId = 0;
                Int32 entityVariantLevelId = 0;
                Int32 entityVariantLevelParentId = 0;
                String entityTypeName = String.Empty;
                String entityTypeLongName = String.Empty;
                Int32 rank = 0;
                Int32 entityTypeId = 0;

                if (reader["FK_EntityVariant_Definition"] != null)
                {
                    entityVariantDefinitionId = ValueTypeHelper.Int32TryParse(reader["FK_EntityVariant_Definition"].ToString(), 0);
                }

                if (reader["PK_EntityVariant_Level"] != null) //TODO: Need to change this in db
                {
                    entityVariantLevelId = ValueTypeHelper.Int32TryParse(reader["PK_EntityVariant_Level"].ToString(), 0);
                }

                if (reader["FK_EntityVariant_LevelParent"] != null)
                {
                    entityVariantLevelParentId = ValueTypeHelper.Int32TryParse(reader["FK_EntityVariant_LevelParent"].ToString(), 0);
                }

                if (reader["FK_EntityType"] != null)
                {
                    entityTypeId = ValueTypeHelper.Int32TryParse(reader["FK_EntityType"].ToString(), 0);
                }

                if (reader["EntityTypeShortName"] != null)
                {
                    entityTypeName = reader["EntityTypeShortName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    entityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["Rank"] != null)
                {
                    rank = ValueTypeHelper.Int32TryParse(reader["Rank"].ToString(), 0);
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                EntityVariantDefinition entityVariantDefinition = entityVariantDefinitions.Get(entityVariantDefinitionId);

                EntityVariantLevel entityVariantLevel = new EntityVariantLevel
                {
                    Id = entityVariantLevelId,
                    ParentLevelId = entityVariantLevelParentId,
                    EntityTypeId = entityTypeId,
                    EntityTypeName = entityTypeName,
                    EntityTypeLongName = entityTypeLongName,
                    Rank = rank
                };

                entityVariantDefinition.EntityVariantLevels.Add(entityVariantLevel);
            }
        }

        private void ReadVariantAttributesProperties(SqlDataReader reader, EntityVariantDefinitionCollection entityVariantDefinitions, CallerContext callerContext)
        {
            while (reader.Read())
            {
                Int32 entityVariantDefinitionId = 0;
                Int32 entityVariantLevelId = 0;
                Int32 targetAttributeId = 0;
                Boolean isOptional = false;
                Int32 sourceAttributeId = 0;
                String sourceAttributeName = String.Empty;
                String targetAttributeName = String.Empty;
                String sourceAttributeLongName = String.Empty;
                String targetAttributeLongName = String.Empty;

                if (reader["FK_EntityVariant_Definition"] != null)
                {
                    entityVariantDefinitionId = ValueTypeHelper.Int32TryParse(reader["FK_EntityVariant_Definition"].ToString(), 0);
                }

                if (reader["FK_EntityVariant_Level"] != null)
                {
                    entityVariantLevelId = ValueTypeHelper.Int32TryParse(reader["FK_EntityVariant_Level"].ToString(), 0);
                }

                if (reader["FK_Source_Attribute"] != null)
                {
                    sourceAttributeId = ValueTypeHelper.Int32TryParse(reader["FK_Source_Attribute"].ToString(), 0);
                }

                if (reader["SourceAttributeShortName"] != null)
                {
                    sourceAttributeName = reader["SourceAttributeShortName"].ToString();
                }

                if (reader["SourceAttributeLongName"] != null)
                {
                    sourceAttributeLongName = reader["SourceAttributeLongName"].ToString();
                }

                if (reader["FK_Target_Attribute"] != null)
                {
                    targetAttributeId = ValueTypeHelper.Int32TryParse(reader["FK_Target_Attribute"].ToString(), 0);
                }

                if (reader["TargetAttributeShortName"] != null)
                {
                    targetAttributeName = reader["TargetAttributeShortName"].ToString();
                }

                if (reader["TargetAttributeLongName"] != null)
                {
                    targetAttributeLongName = reader["TargetAttributeLongName"].ToString();
                }

                if (reader["IsOptional"] != null)
                {
                    isOptional = ValueTypeHelper.BooleanTryParse(reader["IsOptional"].ToString(), false);
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                EntityVariantDefinition entityVariantDefinition = entityVariantDefinitions.Get(entityVariantDefinitionId);

                if (entityVariantDefinition != null)
                {
                    foreach (EntityVariantLevel entityVariantLevel in entityVariantDefinition.EntityVariantLevels)
                    {
                        if (entityVariantLevel.Id == entityVariantLevelId)
                        {
                            EntityVariantRuleAttribute entityVariantRuleAttribute = new EntityVariantRuleAttribute();
                            entityVariantRuleAttribute.SourceAttributeId = sourceAttributeId;
                            entityVariantRuleAttribute.SourceAttributeName = sourceAttributeName;
                            entityVariantRuleAttribute.SourceAttributeLongName = sourceAttributeLongName;
                            entityVariantRuleAttribute.TargetAttributeId = targetAttributeId;
                            entityVariantRuleAttribute.TargetAttributeName = targetAttributeName;
                            entityVariantRuleAttribute.TargetAttributeLongName = targetAttributeLongName;
                            entityVariantRuleAttribute.IsOptional = isOptional;

                            entityVariantLevel.RuleAttributes.Add(entityVariantRuleAttribute);

                            break;
                        }
                    }
                }
            }
        }

        private void PopulateOperationResult(SqlDataReader reader, OperationResultCollection operationResults, EntityVariantDefinitionCollection entityVariantDefinitions)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String entityVariantDefinitionId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                OperationResult entityVariantDefinitionOperationResult = operationResults.GetOperationResultByReferenceId(referenceId);

                if (reader["PK_EntityVariant_Definition"] != null)
                {
                    entityVariantDefinitionId = reader["PK_EntityVariant_Definition"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                // In case of delete reference id will be 0
                if (entityVariantDefinitionOperationResult == null && referenceId == "0")
                {
                    entityVariantDefinitionOperationResult = new OperationResult();
                    operationResults.Add(entityVariantDefinitionOperationResult);
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        if (errorCode == "-99") // -99 is an error code which is return by DB when entity variant definition getting changed if it has entities inside. 
                        {
                            EntityVariantDefinition entityVariantDefinition = entityVariantDefinitions.GetById(ValueTypeHelper.Int32TryParse(entityVariantDefinitionId, 0));
                            entityVariantDefinitionOperationResult.AddOperationResult("114509", String.Empty, OperationResultType.Error); // Entity variant definition cannot be modified because it has associated entities.
                        }
                        else
                        {
                            entityVariantDefinitionOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                    }

                    entityVariantDefinitionOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    entityVariantDefinitionOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

                operationResults.RefreshOperationResultStatus();
            }
        }

        private void PopulateDataModelOperationResult(SqlDataReader reader, DataModelOperationResultCollection operationResults, EntityVariantDefinitionCollection entityVariantDefinitions)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String entityVariantDefinitionId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                OperationResult entityVariantDefinitionOperationResult = (OperationResult)operationResults.GetByReferenceId(referenceId);

                if (reader["PK_EntityVariant_Definition"] != null)
                {
                    entityVariantDefinitionId = reader["PK_EntityVariant_Definition"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        if (errorCode == "-99") // -99 is an error code which is return by DB when entity variant definition getting changed if it has entities inside. 
                        {
                            EntityVariantDefinition entityVariantDefinition = entityVariantDefinitions.GetById(ValueTypeHelper.Int32TryParse(entityVariantDefinitionId, 0));
                            entityVariantDefinitionOperationResult.AddOperationResult("114509", String.Empty, OperationResultType.Error); // Entity variant definition cannot be modified because it has associated entities.
                        }
                        else
                        {
                            entityVariantDefinitionOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                    }

                    entityVariantDefinitionOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    entityVariantDefinitionOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        private void PopulateParamtersForProcess(EntityVariantDefinitionCollection entityVariantDefinitions, String loginUser, String programName, String storedProcedureName, out SqlParameter[] paramters)
        {
            SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");

            List<SqlDataRecord> entityVariantDefinitionTable;
            List<SqlDataRecord> entityVariantLevelTable;
            List<SqlDataRecord> entityVariantAttributeTable;

            paramters = generator.GetParameters("DataModelManager_EntityVariantDefinition_Process_ParametersArray");

            SqlMetaData[] entityVariantDefinitionMetaData = generator.GetTableValueMetadata("DataModelManager_EntityVariantDefinition_Process_ParametersArray", paramters[0].ParameterName);
            SqlMetaData[] entityVariantLevelMetaData = generator.GetTableValueMetadata("DataModelManager_EntityVariantDefinition_Process_ParametersArray", paramters[1].ParameterName);
            SqlMetaData[] entityVariantAttributeMetaData = generator.GetTableValueMetadata("DataModelManager_EntityVariantDefinition_Process_ParametersArray", paramters[2].ParameterName);

            CreateEntityVariantDefinitionTableParams(entityVariantDefinitions, entityVariantDefinitionMetaData, entityVariantLevelMetaData,
                                                    entityVariantAttributeMetaData, out entityVariantDefinitionTable, out entityVariantLevelTable, out entityVariantAttributeTable);

            if (isTracingEnabled)
            {
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                diagnosticActivity.LogInformation("Completed preparing TVP from entity variant definitions.");
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityVariantDefinitionTable", entityVariantDefinitionMetaData, entityVariantDefinitionTable);
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityVariantLevelTable", entityVariantLevelMetaData, entityVariantLevelTable);
                DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityVariantAttributeTable", entityVariantAttributeMetaData, entityVariantAttributeTable);
            }

            paramters[0].Value = entityVariantDefinitionTable;
            paramters[1].Value = entityVariantLevelTable;
            paramters[2].Value = entityVariantAttributeTable;
            paramters[3].Value = loginUser;
            paramters[4].Value = programName;

        }

        #endregion Private Methods

        #endregion
    }
}