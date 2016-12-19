using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Collections.ObjectModel;


namespace MDM.EntityProcessorManager.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;

    /// <summary>
    /// 
    /// </summary>
    public class EntityProcessorErrorLogDA : SqlClientDataAccessBase
    {
        #region ErrorLog

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection Get(Collection<Int64> entityIdList, Int64 fromRecordNumber, Int64 toRecordNumber, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = new EntityProcessorErrorLogCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Processor_ErrorLog_Get_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> entityList = null;

                if (entityIdList != null && entityIdList.Count > 0)
                {
                    SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_Impacted_ErrorLog_Get_ParametersArray", parameters[0].ParameterName);

                    foreach (Int64 entityId in entityIdList)
                    {
                        SqlDataRecord entityRecord = new SqlDataRecord(entityMetadata);
                        entityRecord.SetValues(entityId);
                        entityList.Add(entityRecord);
                    }
                }

                #endregion


                parameters[0].Value = entityList;
                parameters[1].Value = fromRecordNumber;
                parameters[2].Value = toRecordNumber;

                const string storedProcedureName = "usp_EntityManager_Processor_ErrorLog_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 id = -1;
                        Int64 entityId = -1;
                        String entityName = String.Empty;
                        String entityLongName = String.Empty;
                        Int32 priority = -1;
                        String errrorMessage = String.Empty;
                        String processorName = String.Empty;
                        DateTime? modifiedDateTime = null;
                        String modifiedUser = String.Empty;
                        String modifiedProgram = String.Empty;
                        Int64 entityActivityLogId = 0;
                        Int32 containerId = 0;
                        Int32 actionId = (Int32)EntityActivityList.UnKnown;

                        if (reader["Id"] != null)
                            id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                        if (reader["EntityId"] != null)
                            entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), -1);

                        if (reader["ShortName"] != null)
                            entityName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            entityLongName = reader["LongName"].ToString();

                        if (reader["Priority"] != null)
                            priority = ValueTypeHelper.Int32TryParse(reader["Priority"].ToString(), 0);

                        if (reader["ErrorMessage"] != null)
                            errrorMessage = reader["ErrorMessage"].ToString();

                        if (reader["Action"] != null)
                            actionId = ValueTypeHelper.Int32TryParse(reader["Action"].ToString(), actionId);

                        if (reader["ProcessorName"] != null)
                            processorName = reader["ProcessorName"].ToString();

                        if (reader["ModDateTime"] != null)
                            modifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ModDateTime"].ToString());

                        if (reader["ModUser"] != null)
                            modifiedUser = reader["ModUser"].ToString();

                        if (reader["ModProgram"] != null)
                            modifiedProgram = reader["ModProgram"].ToString();

                        if (reader["EntityActivityLogId"] != null)
                            entityActivityLogId = ValueTypeHelper.Int64TryParse(reader["EntityActivityLogId"].ToString(), 0);

                        if (reader["ContainerId"] != null)
                            containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);

                        EntityProcessorErrorLog entityProcessorErrorLog = new EntityProcessorErrorLog()
                        {
                            Id = id,
                            ImpactedEntityId = entityId,
                            ImpactedEntityName = entityName,
                            ImpactedEntityLongName = entityLongName,
                            Priority = priority,
                            ErrorMessage = errrorMessage,
                            ProcessorName = processorName,
                            ModifiedDateTime = modifiedDateTime,
                            ModifiedUser = modifiedUser,
                            ModifiedProgram = modifiedProgram,
                            EntityActivityLogId = entityActivityLogId,
                            ContainerId = containerId,
                            PerformedAction = (EntityActivityList)actionId
                        };

                        entityProcessorErrorLogCollection.Add(entityProcessorErrorLog);
                    }
                }

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPIDenormResultCollection"></param>
        /// <param name="command"></param>
        /// <param name="programName"></param>
        /// <param name="loginUser"></param>
        public Boolean Process(EntityProcessorErrorLog entityProcessorErrorLog, DBCommandProperties command, String loginUser, String programName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityDA.ProcessEntityProcessorErrorLog", false);
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Processor_ErrorLog_Process_ParametersArray");

                parameters[0].Value = entityProcessorErrorLog.ImpactedEntityId;
                parameters[1].Value = entityProcessorErrorLog.ContainerId;
                parameters[2].Value = entityProcessorErrorLog.Priority;
                parameters[3].Value = entityProcessorErrorLog.ErrorMessage;
                parameters[4].Value = entityProcessorErrorLog.ProcessorName;
                parameters[5].Value = loginUser;
                parameters[6].Value = programName;
                parameters[7].Value = (Byte)entityProcessorErrorLog.PerformedAction;
                parameters[8].Value = entityProcessorErrorLog.EntityActivityLogId;

                const String storedProcedureName = "usp_EntityManager_Processor_ErrorLog_Process";
                ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ImpactedEntityDA-ProcessEntityProcessorErrorLog: " + exception.Message);
                return false;
            }
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityDA.ProcessEntityProcessorErrorLog");
            return true;
        }

        /// <summary>
        /// Pushes impacted entity from Error log to Impacted table or entity Queue
        /// </summary>
        /// <param name="impactedEntityId">impacted entityId</param>
        /// <param name="containerId">containerId for the impacted entity</param>
        /// <param name="processorName">Name of the Processor which picked up this entity for processing</param>
        /// <param name="entityActivityLogId">unique identifier of entity activity log</param>
        /// <param name="performedAction">action performed on this entity</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns>If success return true else false</returns>
        public Boolean Refresh(String entityIdList, Int32 containerId, String processorName, Int64 entityActivityLogId, DBCommandProperties command, Collection<Int32> attributeIdList = null, Collection<Int32> localeIdList = null)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityDA.RefreshEntityProcessorErrorLog", false);
            try
            {
                String strAttributeIdList = String.Empty;
                if (attributeIdList != null && attributeIdList.Count > 0)
                {
                    strAttributeIdList = ValueTypeHelper.JoinCollection(attributeIdList, ",");
                }

                String strLocaleIdList = String.Empty;
                if (localeIdList != null && localeIdList.Count > 0)
                {
                    strLocaleIdList = ValueTypeHelper.JoinCollection(localeIdList, ",");
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_Entity_Processor_ErrorLog_Refresh_ParametersArray");

                parameters[0].Value = entityIdList;
                parameters[1].Value = strAttributeIdList;
                parameters[2].Value = strLocaleIdList;
                parameters[3].Value = containerId;
                parameters[4].Value = processorName;
                parameters[5].Value = entityActivityLogId;

                const String storedProcedureName = "usp_Tool_Entity_Refresh";
                ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ImpactedEntityDA-RefreshEntityProcessorErrorLog: " + exception.Message);
                return false;
            }
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityDA.RefreshEntityProcessorErrorLog");
            return true;
        }

        #endregion
    }
}
