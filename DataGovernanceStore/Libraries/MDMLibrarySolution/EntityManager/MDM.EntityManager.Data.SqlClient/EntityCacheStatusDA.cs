using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Represents the data access layer for the entity cache status.
    /// </summary>
    public class EntityCacheStatusDA : SqlClientDataAccessBase
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
        /// Returns the EntityCacheStatusCollection based on the batch size. 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="batchSize"></param>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public EntityCacheStatusCollection GetNextBatch(Collection<Int64> entityIdList, Int32 batchSize, Int64 fromRecordNumber, 
            Int64 toRecordNumber, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_CacheStatus_Get_ParametersArray");
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_CacheStatus_Get_ParametersArray", parameters[0].ParameterName);
                
                parameters[0].Value = GetEntitySqlDataRecords(entityIdList, entityMetadata);
                parameters[1].Value = batchSize;
                parameters[2].Value = fromRecordNumber;
                parameters[3].Value = toRecordNumber;

                String storedProcedureName = "usp_EntityManager_Entity_CacheStatus_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        EntityCacheStatus entityCacheStatus = BuildEntityCacheStatus(reader, true);
                        entityCacheStatusCollection.Add(entityCacheStatus);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityCacheStatusCollection;
        }

        /// <summary>
        /// Returns the EntityCacheStatusCollection based on the specified entity id list. 
        /// </summary>
        /// <param name="loadRequestCollection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public EntityCacheStatusCollection GetEntityCacheStatusCollection(Collection<EntityCacheStatusLoadRequest> loadRequestCollection, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_CacheStatus_Multiple_Load_ParametersArray");
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_CacheStatus_Multiple_Load_ParametersArray", parameters[0].ParameterName);

                parameters[0].Value = GetEntitySqlDataRecords(loadRequestCollection, entityMetadata);

                String storedProcedureName = "usp_EntityManager_Entity_CacheStatus_Multiple_Load";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        EntityCacheStatus entityCacheStatus = BuildEntityCacheStatus(reader, false);
                        entityCacheStatusCollection.Add(entityCacheStatus);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityCacheStatusCollection;
        }

        /// <summary>
        /// Processes the specified EntityCacheStatusCollection by inserting or updating the cache status for an entity into entity cache status table.
        /// </summary>
        /// <param name="entityCacheStatusCollection">Specifies the entityCacheStatusCollection to be processed</param>
        /// <param name="callerContext">Represents the caller of the API</param>
        /// <param name="command">Represents the database command object</param>
        /// <param name="isCacheProcessorUpdate">Specifies if the API is invoked by the cache processor</param>        
        /// <param name="processingMode">Specifies the processing mode</param>
        /// <returns></returns>
        public Boolean ProcessCacheStatus(EntityCacheStatusCollection entityCacheStatusCollection, CallerContext callerContext, DBCommandProperties command, Boolean isCacheProcessorUpdate,
            ProcessingMode processingMode = ProcessingMode.Sync)
        {
            Boolean isProcessed = false;
            SqlDataReader reader = null;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusDA.Process", false);
                    
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_CacheStatus_Process_ParametersArray");
                    SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("EntityManager_Entity_CacheStatus_Process_ParametersArray", parameters[0].ParameterName);

                    List<SqlDataRecord> entityCacheStatusRecords = GetEntityCacheStatusSqlRecords(entityCacheStatusCollection, sqlMetadata);

                    //if there are no records to be updated just return
                    if (entityCacheStatusRecords.Count <= 0)
                        return false;

                    parameters[0].Value = entityCacheStatusRecords;
                    parameters[1].Value = isCacheProcessorUpdate;
                    
                    String storedProcedureName = "usp_EntityManager_Entity_CacheStatus_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();

                    isProcessed = true;
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusDA.Process");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return isProcessed;
        }

        /// <summary>
        /// Loads the EntityCache context from the EntityActivityLog table and populates the Entity cache table.
        /// </summary>
        /// <param name="entityActivityLog"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Boolean LoadEntityCacheContextForProcess(EntityActivityLog entityActivityLog, CallerContext callerContext, DBCommandProperties command)
        {
            Boolean isProcessed = false;
            SqlDataReader reader = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityManager.EntityCacheStatusDA.LoadEntityCacheContext", false);

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_CacheStatus_Context_Load_ParametersArray");

                parameters[0].Value = entityActivityLog.Id;

                String storedProcedureName = "usp_EntityManager_Entity_CacheStatus_Context_Load";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                isProcessed = true;
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityCacheStatusDA.LoadEntityCacheContext");
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return isProcessed;
        }

        #endregion        

        #region Private Methods

        private List<SqlDataRecord> GetEntitySqlDataRecords(Collection<Int64> entityIdList, SqlMetaData[] entityMetadata)
        {
            List<SqlDataRecord> entityList = null;
            if (entityIdList != null && entityIdList.Count > 0)
            {
                entityList = new List<SqlDataRecord>();

                foreach (Int64 entityId in entityIdList)
                {
                    SqlDataRecord entityRecord = new SqlDataRecord(entityMetadata);
                    entityRecord.SetValue(0, entityId);
                    entityList.Add(entityRecord);
                }
            }
            return entityList;
        }

        private List<SqlDataRecord> GetEntitySqlDataRecords(Collection<EntityCacheStatusLoadRequest> loadRequestCollection, SqlMetaData[] entityMetadata)
        {
            List<SqlDataRecord> entityLoadRequestList = null;
            if (loadRequestCollection != null && loadRequestCollection.Count > 0)
            {
                entityLoadRequestList = new List<SqlDataRecord>();

                foreach (EntityCacheStatusLoadRequest loadRequest in loadRequestCollection)
                {
                    SqlDataRecord cacheStatusLoadRequest = new SqlDataRecord(entityMetadata);
                    cacheStatusLoadRequest.SetValue(0, loadRequest.EntityId);
                    cacheStatusLoadRequest.SetValue(1, loadRequest.ContainerId);
                    cacheStatusLoadRequest.SetValue(2, loadRequest.ParentEntityTreeIdList);
                    cacheStatusLoadRequest.SetValue(3, String.Empty);
                    entityLoadRequestList.Add(cacheStatusLoadRequest);
                }
            }
            return entityLoadRequestList;
        }

        private EntityCacheStatus BuildEntityCacheStatus(SqlDataReader reader, Boolean isBasedOnBatchSize)
        {
            EntityCacheStatus entityCacheStatus = new EntityCacheStatus();

            if (reader["FK_CNode"] != null)
                entityCacheStatus.EntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), -1);

            if (reader["FK_Catalog"] != null)
                entityCacheStatus.ContainerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

            if (reader["Priority"] != null)
                entityCacheStatus.Weightage = ValueTypeHelper.Int16TryParse(reader["Priority"].ToString(), 0);

            if (reader["CacheStatus"] != null)
                entityCacheStatus.CacheStatus = ValueTypeHelper.Int32TryParse(reader["CacheStatus"].ToString(), 0);

            if (reader["IsCacheLoadingRequired"] != null)
                entityCacheStatus.IsCacheLoadingRequired = ValueTypeHelper.BooleanTryParse(reader["IsCacheLoadingRequired"].ToString(), false);

            if (isBasedOnBatchSize)
            {
                if (reader["IsCategory"] != null)
                    entityCacheStatus.IsCategory = ValueTypeHelper.BooleanTryParse(reader["IsCategory"].ToString(), false);

                if (reader["EntityActivityLogId"] != null)
                {
                    Int64 returnedEntityActivityLogId = 0;
                    Int64.TryParse(reader["EntityActivityLogId"].ToString(), out returnedEntityActivityLogId);
                    entityCacheStatus.EntityActivityLogId = returnedEntityActivityLogId;
                }
            }

            entityCacheStatus.IsCacheStatusUpdated = false;

            return entityCacheStatus;
        }

        private List<SqlDataRecord> GetEntityCacheStatusSqlRecords(EntityCacheStatusCollection entityCacheStatusCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> entityCacheStatusSqlRecords = new List<SqlDataRecord>();

            foreach (EntityCacheStatus entityCacheStatus in entityCacheStatusCollection)
            {
                SqlDataRecord entityCacheStatusLogRecord = new SqlDataRecord(sqlMetadata);

                entityCacheStatusLogRecord.SetValue(0, entityCacheStatus.EntityId);
                entityCacheStatusLogRecord.SetValue(1, entityCacheStatus.ContainerId);
                entityCacheStatusLogRecord.SetValue(2, entityCacheStatus.CacheStatus);
                entityCacheStatusLogRecord.SetValue(3, entityCacheStatus.IsCacheLoadingRequired);
                entityCacheStatusLogRecord.SetValue(4, entityCacheStatus.Weightage);
                entityCacheStatusLogRecord.SetValue(5, entityCacheStatus.EntityActivityLogId);
                entityCacheStatusLogRecord.SetValue(6, entityCacheStatus.OverWriteCacheStatus);
                entityCacheStatusLogRecord.SetValue(7, entityCacheStatus.Action.ToString());

                entityCacheStatusSqlRecords.Add(entityCacheStatusLogRecord);
            }

            return entityCacheStatusSqlRecords;
        }

        #endregion

        #endregion
    }
}
