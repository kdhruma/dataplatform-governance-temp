using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies the data access operations for entity family Queue
    /// </summary>
    public class EntityFamilyQueueDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityFamilyQueueDA()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region  Methods

        #region Public Methods

        /// <summary>
        /// Process entity family queue with revalidation based on given action
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue to be processed</param>
        /// <param name="callerContext">Indicates caller of the method specifying application and module name.</param>
        /// <param name="loginUser">Indicates name of logged in user</param>
        /// <param name="command">Indicates DBCommand object</param>
        public void Process(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext, String loginUser, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            SqlDataReader reader = null;

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogInformation("Parameters creation related to Entity Family Queue Revalidate Process is started.");
                }

                #endregion Diagnostics & Tracing

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    #region Declaration and Initialization

                    String storedProcedureName = "usp_EntityManager_EntityFamilyQueue_Revalidation_Process";
                    String parameterArrayName = "EntityManager_EntityFamilyQueue_Revalidation_Process_ParametersArray";
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters(parameterArrayName);
                    SqlMetaData[] entityMetadata= generator.GetTableValueMetadata(parameterArrayName, parameters[0].ParameterName);
                    SqlMetaData[] containerMetadata = generator.GetTableValueMetadata(parameterArrayName, parameters[1].ParameterName);
                    SqlMetaData[] entityTypeMetadata = generator.GetTableValueMetadata(parameterArrayName, parameters[2].ParameterName);
                    SqlMetaData[] categoryMetadata = generator.GetTableValueMetadata(parameterArrayName, parameters[3].ParameterName);
                    SqlMetaData[] searchAttributeMetadata = generator.GetTableValueMetadata(parameterArrayName, parameters[4].ParameterName);
                    SqlMetaData[] localeMetadata = generator.GetTableValueMetadata(parameterArrayName, parameters[5].ParameterName);
                   
                    #endregion Declaration and Initialization

                    RevalidateContext revalidateContext = entityFamilyQueue.RevalidateContext;
                    List<SqlDataRecord> entityTable = EntityDataReaderUtility.CreateEntityIdTable(revalidateContext.EntityIds, entityMetadata);

                    List<SqlDataRecord> containerTable = null;
                    List<SqlDataRecord> entityTypeTable = null;
                    List<SqlDataRecord> categoryTable = null;
                    List<SqlDataRecord> searchAttributeTable = null;
                    List<SqlDataRecord> localeTable=  null;

                    List<EntityActivityList> entityActivityList = new List<EntityActivityList>();
                    SearchCriteria searchCriteria= revalidateContext.SearchCriteria;

                    String entityRuleContext = String.Empty;

                    if(revalidateContext.RuleMapContextIds != null && revalidateContext.RuleMapContextIds.Count > 0)
                    {
                        entityRuleContext = ValueTypeHelper.JoinCollection<Int32>(revalidateContext.RuleMapContextIds, ",");
                        entityRuleContext = String.Format("<RuleMapContextIdList>{0}</RuleMapContextIdList>", entityRuleContext);
                    }

                    if (searchCriteria != null)
                    {
                        containerTable = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.ContainerIds, containerMetadata);
                        entityTypeTable = EntityDataReaderUtility.CreateIntegerIdTable(searchCriteria.EntityTypeIds, entityTypeMetadata);
                        categoryTable = EntityDataReaderUtility.CreateEntityIdTable(searchCriteria.CategoryIds, categoryMetadata);
                        localeTable = EntityDataReaderUtility.CreateLocaleTable(searchCriteria.Locales, (Int32)GlobalizationHelper.GetSystemDataLocale(), localeMetadata);

                        searchAttributeTable = EntityDataReaderUtility.CreateSearchAttributeTable(searchCriteria.SearchAttributeRules, searchAttributeMetadata);
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Parameters creation related to Entity Family Queue Revalidate Process is completed.");
                    }

                    parameters[0].Value = entityTable;
                    parameters[1].Value = containerTable;
                    parameters[2].Value = entityTypeTable;
                    parameters[3].Value = categoryTable;
                    parameters[4].Value = searchAttributeTable;
                    parameters[5].Value = localeTable;
                    parameters[6].Value = revalidateContext.ToXml();
                    parameters[7].Value = entityRuleContext;
                    parameters[8].Value = loginUser;
                    parameters[9].Value = callerContext.ProgramName;
                    
                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
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

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity Family Queue Revalidate Processing is completed.");
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Process entity family queue based on given action
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queue collection to be processed</param>
        /// <param name="callerContext">Indicates caller of the method specifying application and module name.</param>
        /// <param name="operationResults">Indicates operation result collection</param>
        /// <param name="loginUser">Indicates name of logged in user</param>
        /// <param name="command">Indicates DBCommand object</param>
        /// <param name="returnResult">Indicates whether to return result from the DB or not</param>
        /// <returns>Returns operation result collection</returns>
        public void Process(EntityFamilyQueueCollection entityFamilyQueues, CallerContext callerContext, OperationResultCollection operationResults, String loginUser, DBCommandProperties command, Boolean returnResult = true)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
            SqlDataReader reader = null;

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                    diagnosticActivity.LogInformation("EntityFamily processing is started.");
                }

                #endregion Diagnostics & Tracing

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    #region Declaration and Initialization

                    List<SqlDataRecord> entityFamilyQueueTable;
                    String storedProcedureName = "usp_EntityMananger_EntityFamilyQueue_Process";
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityFamilyQueue_Process_ParametersArray");
                    SqlMetaData[] entityFamilyQueueMetaData = generator.GetTableValueMetadata("EntityManager_EntityFamilyQueue_Process_ParametersArray", parameters[0].ParameterName);

                    #endregion Declaration and Initialization

                    CreateEntityFamilyQueueTable(entityFamilyQueues, entityFamilyQueueMetaData, out entityFamilyQueueTable);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("EntityFamilyQueue table creation is completed.");
                    }

                    if (entityFamilyQueueTable != null)
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Preparing TVP from Entity Family Queue Object is completed.");
                            DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "EntityFamilyQueueTable", entityFamilyQueueMetaData, entityFamilyQueueTable);
                        }

                        parameters[0].Value = entityFamilyQueueTable;
                        parameters[1].Value = loginUser;
                        parameters[2].Value = callerContext.ProgramName;
                        parameters[3].Value = returnResult;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                        }

                        UpdateOperationResults(reader, entityFamilyQueues, operationResults);
                    }

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("EntityFamily processing is completed.");
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Get entity family queue collection based on given parameters
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates specific entity family to be fetched</param>
        /// <param name="entityActivityList">Indicates entity activity list</param>
        /// <param name="batchSize">Indicates no. of entity family to be fetched</param>
        /// <param name="command">Indicates DB command properties</param>
        /// <returns>Returns entity family queue collection</returns>
        public EntityFamilyQueueCollection Get(Int64 entityGlobalFamilyId, List<EntityActivityList> entityActivityList, Int32 batchSize, DBCommandProperties command)
        {
            EntityFamilyQueueCollection entityFamilyQueues = null;
            SqlDataReader reader = null;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("EntityFamily get is started.");
            }

            #endregion Diagnostics & Tracing

            try
            {
                #region Initial Setup

                String storedProcedureName = "usp_EntityMananger_EntityFamilyQueue_Get";
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityFamilyQueue_Get_ParametersArray");

                #endregion Initial Setup

                #region Populate table value parameters for entity activity list

                SqlMetaData[] sqlEntityActivityMetadata = generator.GetTableValueMetadata("EntityManager_EntityFamilyQueue_Get_ParametersArray", parameters[1].ParameterName);
                List<SqlDataRecord> entityActivityRecordList = CreateEntityActivityListTable(entityActivityList, sqlEntityActivityMetadata);

                #endregion Populate table value parameters for entity activity list

                parameters[0].Value = entityGlobalFamilyId;
                parameters[1].Value = entityActivityRecordList;
                parameters[2].Value = batchSize;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }
                
                entityFamilyQueues = ReadEntityFamilyQueues(reader);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Entity family reading is completed.");
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("EntityFamily loading is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityFamilyQueues;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create table from given SQL meta data records
        /// </summary>
        /// <param name="entityFamilyQueues">Indicates entity family queue collection to be processed</param>
        /// <param name="entityFamilyQueueMetaData">Indicates SQL meta data for entity family queue collection</param>
        /// <param name="entityFamilyQueueTable">Indicates table that will contain converted SQL records from given SQL meta data</param>
        private void CreateEntityFamilyQueueTable(EntityFamilyQueueCollection entityFamilyQueues, SqlMetaData[] entityFamilyQueueMetaData, out List<SqlDataRecord> entityFamilyQueueTable)
        {
            entityFamilyQueueTable = new List<SqlDataRecord>();

            foreach (EntityFamilyQueue entityFamilyQueue in entityFamilyQueues)
            {
                SqlDataRecord entityFamilyQueueRecord = new SqlDataRecord(entityFamilyQueueMetaData);
                EntityFamilyChangeContext entityFamilyChangeContext = entityFamilyQueue.EntityFamilyChangeContexts.FirstOrDefault();

                entityFamilyQueueRecord.SetValue(0, entityFamilyQueue.Id);
                entityFamilyQueueRecord.SetValue(1, entityFamilyQueue.EntityFamilyId);
                entityFamilyQueueRecord.SetValue(2, entityFamilyQueue.EntityGlobalFamilyId);
                entityFamilyQueueRecord.SetValue(3, entityFamilyQueue.ContainerId);
                entityFamilyQueueRecord.SetValue(4, (Int32)entityFamilyQueue.EntityActivityList);
                entityFamilyQueueRecord.SetValue(5, (entityFamilyChangeContext != null) ? entityFamilyChangeContext.ToXml() : String.Empty);
                entityFamilyQueueRecord.SetValue(6, entityFamilyQueue.IsProcessed);
                entityFamilyQueueRecord.SetValue(7, entityFamilyQueue.Action.ToString());

                entityFamilyQueueTable.Add(entityFamilyQueueRecord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityList"></param>
        /// <param name="entityActivityListMetaData"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateEntityActivityListTable(List<EntityActivityList> entityActivityList, SqlMetaData[] entityActivityListMetaData)
        {
            List<SqlDataRecord> entityActivityListTable = null;

            if (entityActivityList != null && entityActivityList.Count > 0)
            {
                entityActivityListTable = new List<SqlDataRecord>();

                foreach (EntityActivityList activityList in entityActivityList)
                {
                    SqlDataRecord entityActivityListRecord = new SqlDataRecord(entityActivityListMetaData);
                    entityActivityListRecord.SetValue(0, (Int32)activityList);
                    entityActivityListTable.Add(entityActivityListRecord);
                }
            }

            return entityActivityListTable;
        }

        /// <summary>
        /// Prepares operation result collection by updating status and other information in it.
        /// </summary>
        /// <param name="reader">Indicates SQL data reader</param>
        /// <param name="entityFamilyQueues">Indicates collection of entity family queue</param>
        /// <param name="operationResults">Indicates operation result collection to be sent out</param>
        private void UpdateOperationResults(SqlDataReader reader, EntityFamilyQueueCollection entityFamilyQueues, OperationResultCollection operationResults)
        {
            if (reader != null)
            {
                while (reader.Read())
                {
                    Int64 id = 0;
                    Int64 entityFamilyId = 0;
                    Boolean hasError = false;
                    String errorCode = String.Empty;

                    if (reader["PK_EntityFamily_Queue"] != null)
                    {
                        id = ValueTypeHelper.Int64TryParse(reader["PK_EntityFamily_Queue"].ToString(), id);
                    }
                    if (reader["EntityFamilyId"] != null)
                    {
                        entityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), entityFamilyId);
                    }
                    if (reader["HasError"] != null)
                    {
                        hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), hasError);
                    }
                    if (reader["ErrorCode"] != null)
                    {
                        errorCode = reader["ErrorCode"].ToString();
                    }

                    //Get the entity operation result
                    OperationResult operationResult = operationResults.SingleOrDefault(or => or.ReferenceId == entityFamilyId.ToString());
                    EntityFamilyQueue entityFamilyQueue = entityFamilyQueues.SingleOrDefault(egq => egq.EntityFamilyId == entityFamilyId);

                    if (operationResult != null && entityFamilyQueue != null)
                    {
                        entityFamilyQueue.Id = id;

                        if (hasError)
                        {
                            operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }
                        else
                        {
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read entity family queue properties from given reader
        /// </summary>
        /// <param name="reader">Indicates SQL data reader that holds entity family queue information</param>
        /// <returns>Returns entity family queueCollection</returns>
        private EntityFamilyQueueCollection ReadEntityFamilyQueues(SqlDataReader reader)
        {
            EntityFamilyQueueCollection entityFamilyQueues = new EntityFamilyQueueCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    EntityFamilyQueue entityFamilyQueue = new EntityFamilyQueue();
                    EntityActivityList entityActivityList = EntityActivityList.UnKnown;
                    Boolean isMasterCollaborationRecord = false;

                    if (reader["Id"] != null)
                    {
                        entityFamilyQueue.Id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);
                    }
                    if (reader["EntityFamilyId"] != null)
                    {
                        entityFamilyQueue.EntityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), -1);
                    }
                    if (reader["EntityGlobalFamilyId"] != null)
                    {
                        entityFamilyQueue.EntityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), -1);
                    }
                    if (reader["Action"] != null)
                    {
                        ValueTypeHelper.EnumTryParse<EntityActivityList>(reader["Action"].ToString(), true, out entityActivityList);
                        entityFamilyQueue.EntityActivityList = entityActivityList;
                    }
                    if (reader["IsMasterEntity"] != null)
                    {
                        isMasterCollaborationRecord = ValueTypeHelper.BooleanTryParse(reader["IsMasterEntity"].ToString(), isMasterCollaborationRecord);
                    }
                    if (reader["EntityChangeContext"] != null)
                    {
                        String changeContextAsXml = reader["EntityChangeContext"].ToString();

                        if (entityActivityList == EntityActivityList.EntityRevalidate)
                        {
                            if (!String.IsNullOrWhiteSpace(changeContextAsXml))
                            {
                                Collection<Int32> ruleMapContextIdList = GetRuleMapContextIdList(changeContextAsXml);

                                RevalidateContext revalidateContext = new RevalidateContext();
                                revalidateContext.RuleMapContextIds = ruleMapContextIdList;
                                revalidateContext.RevalidateMode = RevalidateMode.Delta;
                                revalidateContext.IsMasterCollaborationRecord = isMasterCollaborationRecord;

                                entityFamilyQueue.RevalidateContext = revalidateContext;
                            }
                        }
                        else
                        {
                            EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext(changeContextAsXml);
                            entityFamilyChangeContext.EntityActivityList = entityActivityList;
                            entityFamilyChangeContext.IsMasterCollaborationRecord = isMasterCollaborationRecord;

                            entityFamilyQueue.EntityFamilyChangeContexts.Add(entityFamilyChangeContext);
                        }
                    }
                    if (reader["ContainerId"] != null)
                    {
                        entityFamilyQueue.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), -1);
                    }

                    entityFamilyQueues.Add(entityFamilyQueue);
                }
            }

            return entityFamilyQueues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeContextAsXml"></param>
        /// <returns></returns>
        private Collection<Int32> GetRuleMapContextIdList(String changeContextAsXml)
        {
            Collection<Int32> ruleMapContextIdList = new Collection<Int32>();

            using (XmlTextReader reader = new XmlTextReader(changeContextAsXml, XmlNodeType.Element, null))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RuleMapContextIdList")
                    {
                        String ruleMapContextIds = reader.ReadElementContentAsString();

                        if (!String.IsNullOrWhiteSpace(ruleMapContextIds))
                        {
                            ruleMapContextIdList.AddRange<Int32>(ValueTypeHelper.SplitStringToIntCollection(ruleMapContextIds, ','), true);
                        }
                    }

                    reader.Read();
                }
            }

            return ruleMapContextIdList;
        }

        #endregion Private Methods

        #endregion Methods
    }
}