using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace MDM.ActivityLogManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    public class EntityActivityLogDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String SQLParametersGeneratorName = "EntityManager_SqlParameters";

        #endregion

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Processes the impacted entity log
        /// </summary>
        /// <param name="entityActivityLogCollection">Collection of activities</param>
        /// <param name="loginUser">logged in user</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>if operation is success it returns true else false</returns>
        public Boolean Process(EntityActivityLogCollection entityActivityLogCollection, String loginUser, CallerContext callerContext, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters =
                        generator.GetParameters("EntityManager_EntityActivityLog_Process_ParametersArray");

                    SqlMetaData[] sqlMetadata =
                        generator.GetTableValueMetadata("EntityManager_EntityActivityLog_Process_ParametersArray",
                                                        parameters[0].ParameterName);

                    List<SqlDataRecord> entityActivityLogRecords =
                        MDM.Utility.EntityActivityLogUtils.GetSqlRecords(entityActivityLogCollection, sqlMetadata);

                    //if there are no records to be updated just return
                    if (entityActivityLogRecords.Count <= 0)
                        return false;

                    parameters[0].Value = entityActivityLogRecords;
                    parameters[1].Value = callerContext.ProgramName;
                    parameters[2].Value = loginUser;

                    const String storedProcedureName = "usp_EntityManager_EntityActivityLog_Process";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ImpactedEntityDA-ProcessImpactedEntityLog: " + exception.Message);
                return false;
            }

            return true;

        }

        /// <summary>
        /// Gets all the impacted entity logs based on the log status
        /// * LogType.Current -> get all the records from tb_EntityActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_EntityActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_EntityActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="entityActivityList">Indicates the entity activity list type</param>
        /// <param name="logStatus">Indicates the processing status type like LogType.Current,LogType.Past,LogType.Pending</param>
        /// <param name="fromRecordNumber">Indicates the starting index of record to be fetched</param>
        /// <param name="toRecordNumber">Indicates the end index of record to be fetched</param>
        /// <param name="command">Indicates the command object which contains all the db info like connection string</param>
        /// <returns>Returns the impacted entity activity log collection based on the context provided.</returns>
        public EntityActivityLogCollection Get(EntityActivityList entityActivityList, ProcessingStatus logStatus, Int64 fromRecordNumber, Int64 toRecordNumber, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntityActivityLogCollection resultCollection = new EntityActivityLogCollection();
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityActivityLog_Get_ParametersArray");

                parameters[0].Value = logStatus.ToString();
                parameters[1].Value = fromRecordNumber;
                parameters[2].Value = toRecordNumber;
                parameters[3].Value = (Int32)entityActivityList;

                const String storedProcedureName = "usp_EntityManager_EntityActivityLog_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 id = 0;
                        Int64 entityId = 0;
                        Int32 containerId = 0;
                        String entityName = String.Empty;
                        String entityLongName = String.Empty;
                        Collection<Int32> attributeIdList = null;
                        Collection<LocaleEnum> attributeLocaleIdList = null;
                        Collection<Int64> relationshipIdList = null;
                        String entityData = String.Empty;
                        Boolean isLoadingInProgress = false;
                        Boolean isLoaded = false;
                        Boolean isProcessed = false;
                        DateTime? loadStart = null;
                        DateTime? loadEnd = null;
                        DateTime? processStart = null;
                        DateTime? processEnd = null;
                        DateTime? createdDateTime = null;
                        Int64 impactedCount = 0;
                        Int64 pendingImpactedEntityCount = 0;
                        Int32 weightage = 0;
                        String programName = String.Empty;
                        String containerName = String.Empty;
                        Int32 serverId = 0;
                        String serverName = String.Empty;
                        String userName = String.Empty;
                        Int32 actionId = (Int32) ObjectAction.Unknown;
                        Int64 auditRefId = -1;
                        Boolean isDirectChange = true;
                        String context = String.Empty;
                        String mdmRuleContextName = String.Empty;

                        if (reader["Id"] != null)
                        {
                            id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), 0);
                        }

                        if (reader["EntityId"] != null)
                        {
                            entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0);
                        }

                        if (reader["Action"] != null)
                        {
                            actionId = ValueTypeHelper.Int32TryParse(reader["Action"].ToString(), actionId);
                        }

                        if (reader["ShortName"] != null)
                        {
                            entityName = reader["ShortName"].ToString();
                        }

                        if (reader["LongName"] != null)
                        {
                            entityLongName = reader["LongName"].ToString();
                        }

                        if (reader["ContainerId"] != null)
                        {
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);
                        }

                        if (reader["ContainerName"] != null)
                        {
                            containerName = reader["ContainerName"].ToString();
                        }

                        if (reader["AttributeIdList"] != null)
                        {
                            attributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader["AttributeIdList"].ToString(), ',');
                        }

                        if (reader["AttributeLocaleIdList"] != null)
                        {
                            attributeLocaleIdList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader["AttributeLocaleIdList"].ToString(), ',');
                        }

                        if (reader["RelationshipIdList"] != null)
                        {
                            relationshipIdList = ValueTypeHelper.SplitStringToLongCollection(reader["RelationshipIdList"].ToString(), ',');
                        }

                        if (reader["EntityData"] != null)
                        {
                            entityData = reader["EntityData"].ToString();
                        }

                        if (reader["IsLoadingInProgress"] != null)
                        {
                            isLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader["IsLoadingInProgress"].ToString(), false);
                        }

                        if (reader["IsLoaded"] != null)
                        {
                            isLoaded = ValueTypeHelper.BooleanTryParse(reader["IsLoaded"].ToString(), false);
                        }

                        if (reader["IsProcessed"] != null)
                        {
                            isProcessed = ValueTypeHelper.BooleanTryParse(reader["IsProcessed"].ToString(), false);
                        }

                        if (reader["LoadStartTime"] != null)
                        {
                            loadStart = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadStartTime"].ToString());
                        }

                        if (reader["LoadEndTime"] != null)
                        {
                            loadEnd = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadEndTime"].ToString());
                        }

                        if (reader["ProcessStartTime"] != null)
                        {
                            processStart = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessStartTime"].ToString());
                        }

                        if (reader["ProcessEndTime"] != null)
                        {
                            processEnd = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessEndTime"].ToString());
                        }

                        if (reader["CreateDateTime"] != null)
                        {
                            createdDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["CreateDateTime"].ToString());
                        }

                        if (reader["ProgramName"] != null)
                        {
                            programName = reader["ProgramName"].ToString();
                        }

                        if (reader["ServerId"] != null)
                        {
                            Int32.TryParse(reader["ServerId"].ToString(), out serverId);
                        }

                        if (reader["ServerName"] != null)
                        {
                            serverName = reader["ServerName"].ToString();
                        }

                        if (reader["UserName"] != null)
                        {
                            userName = reader["UserName"].ToString();
                        }

                        if (reader["ImpactedCount"] != null)
                        {
                            Int64.TryParse(reader["ImpactedCount"].ToString(), out impactedCount);
                        }

                        if (reader["PendingImpactedEntityCount"] != null)
                        {
                            Int64.TryParse(reader["PendingImpactedEntityCount"].ToString(), out pendingImpactedEntityCount);
                        }

                        if (reader["FK_Audit_Ref"] != null)
                        {
                            Int64.TryParse(reader["FK_Audit_Ref"].ToString(), out auditRefId);
                        }

                        if (reader["Weightage"] != null)
                        {
                            Int32.TryParse(reader["Weightage"].ToString(), out weightage);
                        }

                        if (reader["IsDirectChange"] != null)
                        {
                            isDirectChange = ValueTypeHelper.BooleanTryParse(reader["IsDirectChange"].ToString(), true);
                        }

                        if (reader["Context"] != null)
                        {
                            context = reader["Context"].ToString();
                        }

                        if (reader["RuleMapContextNames"] != null)
                        {
                            mdmRuleContextName = reader["RuleMapContextNames"].ToString();
                        }

                        EntityActivityLog impactedEntityLog = new EntityActivityLog()
                            {
                                Id = id,
                                EntityId = entityId,
                                ContainerId = containerId,
                                ContainerName = containerName,
                                PerformedAction = (EntityActivityList)actionId,
                                EntityName = entityName,
                                EntityLongName = entityLongName,
                                AttributeIdList = attributeIdList,
                                AttributeLocaleIdList = attributeLocaleIdList,
                                RelationshipIdList = relationshipIdList,
                                EntityData = entityData,
                                IsLoadingInProgress = isLoadingInProgress,
                                IsLoaded = isLoaded,
                                IsProcessed = isProcessed,
                                LoadStartTime = loadStart,
                                LoadEndTime = loadEnd,
                                ProcessStartTime = processStart,
                                ProcessEndTime = processEnd,
                                CreatedDateTime = createdDateTime,
                                ImpactedCount = impactedCount,
                                PendingImpactedEntityCount = pendingImpactedEntityCount,
                                Weightage = weightage,
                                ProgramName = programName,
                                UserName = userName,
                                ServerId = serverId,
                                ServerName = serverName,
                                AuditRefId = auditRefId,
                                IsDirectChange = isDirectChange,
                                Context = context,
                                MDMRuleMapContextName = mdmRuleContextName
                            };

                        resultCollection.Add(impactedEntityLog);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return resultCollection;
        }

        /// <summary>
        /// Gets EntityActivityLog Item Status for specified entity activity log id
        /// </summary>
        /// <param name="entityActivityLogId">Entity Activity Log record Id</param>
        /// <param name="command">Command object which contains all the db info like connectionstring</param>
        public EntityActivityLogItemStatus GetEntityActivityLogStatus(Int64 entityActivityLogId, DBCommandProperties command)
        {
            EntityActivityLogItemStatus result = null;
            SqlDataReader reader = null;
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityActivityLog_GetItemStatus_ParametersArray");

                parameters[0].Value = entityActivityLogId;

                const String storedProcedureName = "usp_EntityManager_EntityActivityLog_GetItemStatus";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                result = GetAllEntityActivityLogStatus(reader).FirstOrDefault();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }
        
        /// <summary>
        /// Gets all entity activity log with DQM validations for the current user
        /// If the user is admin it returns entity activity log status data for all users
        /// If the user is not admin it returns entity activity log status data for that user
        /// </summary>
        /// <param name="command">Command object which contains all the db info like connectionstring</param>
        /// <param name="loginUser">User name who launches the validation</param>
        /// <returns></returns>
        public EntityActivityLogItemStatusCollection GetAllEntityActivityLogStatus(DBCommandProperties command, String loginUser)
        {
            EntityActivityLogItemStatusCollection result = null;
            SqlDataReader reader = null;
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityActivityLog_GetAllItemStatus_ParametersArray");
  
                parameters[0].Value = loginUser;

                const String storedProcedureName = "usp_EntityManager_EntityActivityLog_GetAllItemStatus";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                result = GetAllEntityActivityLogStatus(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        #region Private Methods

        private EntityActivityLogItemStatusCollection GetAllEntityActivityLogStatus(SqlDataReader reader)
        {        
            EntityActivityLogItemStatusCollection result = new EntityActivityLogItemStatusCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Int64 id = -1;
                    Boolean? isLoadingInProgress = null;
                    Boolean? isLoaded = null;
                    Boolean? isProcessed = null;
                    DateTime? loadStart = null;
                    DateTime? loadEnd = null;
                    DateTime? processStart = null;
                    DateTime? processEnd = null;
                    DateTime? createdDateTime = null;
                    Int64 processedEntitiesCount = 0;
                    Int64 totalEntitiesCount = 0;
                    String userName = String.Empty;
                    Int32 actionId = (Int32) ObjectAction.Unknown;

                    if (reader["PK_EntityActivityLog"] != null)
                        id = ValueTypeHelper.Int64TryParse(reader["PK_EntityActivityLog"].ToString(), 0);

                    if (reader["Action"] != null)
                        actionId = ValueTypeHelper.Int32TryParse(reader["Action"].ToString(), actionId);

                    if (reader["IsLoadingInProgress"] != null && reader["IsLoadingInProgress"] != DBNull.Value)
                        isLoadingInProgress = (Boolean)reader["IsLoadingInProgress"];

                    if (reader["IsLoaded"] != null && reader["IsLoaded"] != DBNull.Value)
                        isLoaded = (Boolean)reader["IsLoaded"];

                    if (reader["IsProcessed"] != null && reader["IsProcessed"] != DBNull.Value)
                        isProcessed = (Boolean)reader["IsProcessed"];

                    if (reader["LoadStartTime"] != null)
                        loadStart = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadStartTime"].ToString());

                    if (reader["LoadEndTime"] != null)
                        loadEnd = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadEndTime"].ToString());

                    if (reader["ProcessStartTime"] != null)
                        processStart = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessStartTime"].ToString());

                    if (reader["ProcessEndTime"] != null)
                        processEnd = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessEndTime"].ToString());

                    if (reader["CreateDateTime"] != null)
                        createdDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["CreateDateTime"].ToString());

                    if (reader["UserName"] != null)
                        userName = reader["UserName"].ToString();

                    if (reader["ProcessedEntitiesCount"] != null)
                        Int64.TryParse(reader["ProcessedEntitiesCount"].ToString(), out processedEntitiesCount);

                    if (reader["TotalEntitiesCount"] != null)
                        Int64.TryParse(reader["TotalEntitiesCount"].ToString(), out totalEntitiesCount);

                    result.Add(new EntityActivityLogItemStatus
                    {
                        EntityActivityLogId = id,
                        PerformedAction = (EntityActivityList) actionId,
                        IsLoadingInProgress = isLoadingInProgress,
                        IsLoaded = isLoaded,
                        IsProcessed = isProcessed,
                        LoadStartTime = loadStart,
                        LoadEndTime = loadEnd,
                        ProcessStartTime = processStart,
                        ProcessEndTime = processEnd,
                        CreatedDateTime = createdDateTime,
                        UserName = userName,
                        ProcessedEntitiesCount = processedEntitiesCount,
                        TotalEntitiesCount = totalEntitiesCount
                     });
                }
            }        
   
            return result;
        }

        #endregion Private Methods

        #endregion
    }
}
