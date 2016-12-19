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

    /// <summary>
    /// Represents strongly typed entity cache status data access logic
    /// </summary>
    public class StronglyTypedEntityCacheStatusDA : SqlClientDataAccessBase
    {
        /// <summary>
        /// Updates cache status table for specified entity ids to indicate processor has worked on them
        /// </summary>
        /// <param name="entityIds">Entity ids to be processed</param>
        /// <param name="isCacheLoadingRequired">Indicates is cache loading is required.</param>
        /// <param name="callerContext">Caller details</param>
        /// <param name="command">DB command properties</param>
        /// <param name="processingMode">Processing mode</param>
        /// <returns>Result of the cache status process</returns>
        public Boolean ProcessCacheStatus(Collection<Int64> entityIds, Boolean isCacheLoadingRequired, CallerContext callerContext, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            Boolean isProcessed = false;
            SqlDataReader reader = null;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StartTraceActivity("EntityManager.StronglyTypedEntityCacheStatusDA.ProcessCacheStatus", false);

                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_StronglyTypedEntity_CacheStatus_Process_ParametersArray");

                    #region Populate Entity table value parameters

                    List<SqlDataRecord> entityList = null;

                    if (entityIds != null && entityIds.Count > 0)
                    {
                        entityList = new List<SqlDataRecord>();
                        SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_StronglyTypedEntity_CacheStatus_Process_ParametersArray", parameters[0].ParameterName);

                        SqlDataRecord entityRecord = null;

                        foreach (Int64 entityId in entityIds)
                        {
                            entityRecord = new SqlDataRecord(entityMetadata);
                            entityRecord.SetValues(entityId);
                            entityList.Add(entityRecord);
                        }
                    }

                    #endregion

                    //if there are no records to be updated just return
                    if (entityList.Count <= 0)
                        return false;

                    parameters[0].Value = entityList;
                    parameters[1].Value = isCacheLoadingRequired;

                    String storedProcedureName = "usp_EntityManager_StronglyTypedEntity_CacheStatus_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();

                    isProcessed = true;
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("StronglyTypedEntityCacheStatusDA.ProcessCacheStatus");
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
        /// Gets next batch for Strongly typed entity cache processor for processing
        /// </summary>
        /// <param name="entityIdList">Entity ids</param>
        /// <param name="batchSize">Batch size</param>
        /// <param name="fromRecordNumber">From record number</param>
        /// <param name="toRecordNumber">To record number</param>
        /// <param name="command">DB command properties</param>
        /// <returns>Result of the cache status process</returns>
        public Collection<Int64> GetNextBatch(Collection<Int64> entityIdList, Int32 batchSize, Int64 fromRecordNumber, Int64 toRecordNumber, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            Collection<Int64> entityIdCollection = new Collection<Int64>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_StronglyTypedEntity_CacheStatus_Get_ParametersArray");
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_StronglyTypedEntity_CacheStatus_Get_ParametersArray", parameters[0].ParameterName);

                parameters[0].Value = GetEntitySqlDataRecords(entityIdList, entityMetadata);
                parameters[1].Value = batchSize;
                parameters[2].Value = fromRecordNumber;
                parameters[3].Value = toRecordNumber;

                String storedProcedureName = "usp_EntityManager_StronglyTypedEntity_CacheStatus_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["FK_CNode"] != null)
                            entityIdCollection.Add(ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), -1));
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityIdCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
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
    }
}
