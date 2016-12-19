using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies the data access operations for Entity Queue
    /// </summary>
    public class EntityQueueDA : SqlClientDataAccessBase
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
        /// 
        /// </summary>
        /// <param name="entityActivityLogCollection"></param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="queuedEntityCount"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean Load(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext, out Int64 queuedEntityCount, DBCommandProperties command)
        {
            queuedEntityCount = 0;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityQueue_Load_ParametersArray");

                    SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("EntityManager_EntityQueue_Load_ParametersArray", parameters[0].ParameterName);

                    List<SqlDataRecord> entityActivityLogRecords = MDM.Utility.EntityActivityLogUtils.GetSqlRecords(entityActivityLogCollection, sqlMetadata);

                    //if there are no records to be updated just return
                    if (entityActivityLogRecords.Count <= 0)
                        return false;

                    parameters[0].Value = entityActivityLogRecords;
                    parameters[1].Value = callerContext.ProgramName;

                    const String storedProcedureName = "usp_EntityManager_EntityQueue_Load";

                    Object result = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                    if (result != null)
                    {
                        queuedEntityCount = ValueTypeHelper.Int64TryParse(result.ToString(), 0);
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "EntityQueueDA-Load: " + exception.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivities"></param>
        /// <param name="batchSize"></param>
        /// <param name="command"></param>
        /// <param name="entityActivityLogId"></param>
        /// <returns></returns>
        public QueuedEntityCollection Get(List<EntityActivityList> entityActivities, Int64 entityActivityLogId, Int32 batchSize, DBCommandProperties command)
        {
            SqlDataReader reader = null;

            QueuedEntityCollection resultCollection = new QueuedEntityCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityQueue_Get_ParametersArray");

                parameters[0].Value = String.Join(",", entityActivities.ConvertAll(ConvertEntityActivityListToInt));
                parameters[1].Value = entityActivityLogId;
                parameters[2].Value = batchSize;

                const String storedProcedureName = "usp_EntityManager_EntityQueue_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 id = 0;
                        Int64 entityId = 0;
                        Int32 containerId = 0;
                        Int32 actionId = (Int32)EntityActivityList.UnKnown;
                        String entityName = String.Empty;
                        String entityLongName = String.Empty;
                        Collection<Int32> attributeIdList = null;
                        Collection<LocaleEnum> attributeLocaleIdList = null;
                        Collection<Int32> relationshipIdList = null;
                        String entityData = String.Empty;
                        Boolean isInProgress = false;
                        Int32 weightage = 0;
                        Int64 returnedEntityActivityLogId = 0;
                        Int32 serverId = 0;
                        String serverName = String.Empty;
                        DateTime? lastModDateTime = null;
                        Int32 auditRefId = -1;
                        Boolean isDirectChange = true;
                        String context = String.Empty;

                        if (reader["Id"] != null)
                            id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), 0);

                        if (reader["EntityId"] != null)
                            entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0);

                        if (reader["ContainerId"] != null)
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);

                        if (reader["Action"] != null)
                            actionId = ValueTypeHelper.Int32TryParse(reader["Action"].ToString(), actionId);

                        if (reader["ShortName"] != null)
                            entityName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            entityLongName = reader["LongName"].ToString();

                        if (reader["AttributeIdList"] != null)
                            attributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader["AttributeIdList"].ToString(), ',');

                        if (reader["AttributeLocaleIdList"] != null)
                            attributeLocaleIdList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader["AttributeLocaleIdList"].ToString(), ',');

                        if (reader["RelationshipIdList"] != null)
                            relationshipIdList = ValueTypeHelper.SplitStringToIntCollection(reader["RelationshipIdList"].ToString(), ',');

                        if (reader["EntityData"] != null)
                            entityData = reader["EntityData"].ToString();

                        if (reader["InProgress"] != null)
                            isInProgress = ValueTypeHelper.ConvertToBoolean(reader["InProgress"].ToString());

                        if (reader["Weightage"] != null)
                            Int32.TryParse(reader["Weightage"].ToString(), out weightage);

                        if (reader["EntityActivityLogId"] != null)
                            Int64.TryParse(reader["EntityActivityLogId"].ToString(), out returnedEntityActivityLogId);

                        if (reader["ServerId"] != null)
                            Int32.TryParse(reader["ServerId"].ToString(), out serverId);

                        if (reader["ServerName"] != null)
                            serverName = reader["ServerName"].ToString();

                        if (reader["LastModDateTime"] != null)
                            lastModDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["LastModDateTime"].ToString());

                        if (reader["FK_Audit_Ref"] != null)
                            Int32.TryParse(reader["FK_Audit_Ref"].ToString(), out auditRefId);

                        if (reader["IsDirectChange"] != null)
                        {
                            String value = reader["IsDirectChange"].ToString();
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                Boolean.TryParse(value, out isDirectChange);
                            }
                        }

                        if (reader["Context"] != null)
                            context = reader["Context"].ToString();

                        QueuedEntity queuedEntity = new QueuedEntity()
                        {
                            Id = id,
                            EntityId = entityId,
                            ContainerId = containerId,
                            PerformedAction = (EntityActivityList)actionId,
                            EntityName = entityName,
                            EntityLongName = entityLongName,
                            AttributeIdList = attributeIdList,
                            AttributeLocaleIdList = attributeLocaleIdList,
                            RelationshipIdList = relationshipIdList,
                            EntityData = entityData,
                            IsInProgress = isInProgress,
                            Weightage = weightage,
                            EntityActivityLogId = returnedEntityActivityLogId,
                            ServerId = serverId,
                            ServerName = serverName,
                            LastModifiedDateTime = lastModDateTime,
                            AuditRefId = auditRefId,
                            IsDirectChange = isDirectChange,
                            Context = context
                        };

                        resultCollection.Add(queuedEntity);
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
        /// 
        /// </summary>
        /// <param name="queueRecordId"></param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <returns></returns>
        public Boolean Remove(Int64 queueRecordId, CallerContext callerContext, DBCommandProperties command)
        {
            Boolean returnFlag = false;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityQueue_Remove_ParametersArray");

                    parameters[0].Value = queueRecordId;
                    parameters[1].Value = callerContext.ProgramName;

                    const String storedProcedureName = "usp_EntityManager_EntityQueue_Remove";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();

                    returnFlag = true;
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "EntityQueueDA-Remove: " + exception.Message);
                returnFlag = false;
            }

            return returnFlag;
        }

        /// <summary>
        /// Performs the process operation on Entity Queue based on the PK_EntityQueue specified
        /// </summary>
        /// <param name="queueRecordId">Entity Queue item primary key value</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <returns></returns>
        public Boolean Process(Int64 queueRecordId, CallerContext callerContext, DBCommandProperties command)
        {
            return Process(new Collection<Int64> {queueRecordId}, callerContext, command);
        }

        /// <summary>
        /// Performs the process operation on Entity Queue based on the PK_EntityQueue list specified
        /// </summary>
        /// <param name="queueRecordIds">Entity Queue item primary key values list</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <returns></returns>
        public Boolean Process(Collection<Int64> queueRecordIds, CallerContext callerContext, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StartTraceActivity("EntityManager.EntityQueueDA.Process", false);
            Boolean returnFlag = false;

            try
            {
                using (
                    TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions() {IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL}))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    SqlParameter[] parameters =
                        generator.GetParameters("EntityManager_EntityQueue_Process_ParametersArray");

                    List<SqlDataRecord> preparedItemsIds = new List<SqlDataRecord>();

                    foreach (Int64 item in queueRecordIds)
                    {
                        SqlDataRecord preparedItem =
                            new SqlDataRecord(
                                generator.GetTableValueMetadata("EntityManager_EntityQueue_Process_ParametersArray",
                                    parameters[0].ParameterName));

                        preparedItem.SetValue(0, item);
                        preparedItemsIds.Add(preparedItem);
                    }

                    if (preparedItemsIds.Any())
                    {
                        parameters[0].Value = preparedItemsIds;
                    }
                    parameters[1].Value = "Update";
                    parameters[2].Value = callerContext.ProgramName;

                    const String storedProcedureName = "usp_EntityManager_EntityQueue_Process";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();

                    returnFlag = true;
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityQueueDA.Process");
            }
            return returnFlag;
        }

        /// <summary>
        /// check requested entity Id and container Id are available in EntityQueue or not.
        /// </summary>
        /// <param name="entityId">This parameter is specifying entity id.</param>
        /// <param name="containerId">This parameter is specifying container id.</param>
        /// <param name="entityActivityList">This parameter is specifying entity Activity List.</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>True : If requested entity id and container id is available EntityQueue else False.</returns>
        public Boolean IsEntityInQueue(Int64 entityId, Int32 containerId, EntityActivityList entityActivityList, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            MDMTraceHelper.StartTraceActivity("EntityQueueDA.IsEntityInQueue", MDMTraceSource.General, false);

            Boolean isEntityInQueue = false;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityQueue_IsEntityInQueue_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = containerId;
                parameters[2].Value = (Int32)entityActivityList;

                const String storedProcedureName = "usp_EntityManager_EntityQueue_EntityExists";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["IsExists"] != null)
                        {
                            isEntityInQueue = (reader["IsExists"].ToString().Equals("1")) ? true : false;
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityQueueDA.IsEntityInQueue", MDMTraceSource.General);

                if (reader != null)
                    reader.Close();
            }

            return isEntityInQueue;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the Enum Value
        /// </summary>
        /// <param name="entityActivityList"></param>
        /// <returns></returns>
        private Int32 ConvertEntityActivityListToInt(EntityActivityList entityActivityList)
        {
            return (Int32)entityActivityList;
        }

        #endregion

        #endregion
    }
}
