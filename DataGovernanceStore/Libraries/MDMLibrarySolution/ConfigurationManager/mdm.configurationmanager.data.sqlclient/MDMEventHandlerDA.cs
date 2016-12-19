using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.ConfigurationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    ///  Specifies the MDMEvent handler data access class
    /// </summary>
    public class MDMEventHandlerDA : SqlClientDataAccessBase
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all the MDMEvent Handlers from the system
        /// </summary>
        /// <param name="eventHandlerIdList">Indicates the list of event Handler Ids</param>
        /// <param name="command">Indicates the Data base command properties</param>
        /// <returns>Returns the list of MDMEvent Handlers</returns>
        public MDMEventHandlerCollection Get(Collection<Int32> eventHandlerIdList, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            MDMEventHandlerCollection mdmHandlers = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");
                parameters = generator.GetParameters("ConfigurationManager_MDMEventHandler_Get_ParametersArray");

                List<SqlDataRecord> eventHandlerIds = new List<SqlDataRecord>();
                SqlMetaData[] metaData = generator.GetTableValueMetadata("ConfigurationManager_MDMEventHandler_Get_ParametersArray", parameters[0].ParameterName);

                if (eventHandlerIdList != null && eventHandlerIdList.Count > 0)
                {
                    foreach (Int32 item in eventHandlerIdList)
                    {
                        SqlDataRecord sqlRecord = new SqlDataRecord(metaData);
                        sqlRecord.SetValue(0, item);
                        eventHandlerIds.Add(sqlRecord);
                    }

                    if (eventHandlerIds.Count > 0)
                    {
                        parameters[0].Value = eventHandlerIds;
                    }
                }

                storedProcedureName = "usp_ConfigurationManager_EventHandler_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                mdmHandlers = this.FillEventHandlersInformations(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return mdmHandlers;
        }

        /// <summary>
        /// Process the requested MDMEvent Handler based on their actions.
        /// </summary>
        /// <param name="mdmEventHandlerCollection">Indicates the list of MDMEvnet Handlers</param>
        /// <param name="userName">Indicates the user Name</param>
        /// <param name="command">Indicates the Data base command properties</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns the operation results</returns>
        public OperationResultCollection Process(MDMEventHandlerCollection mdmEventHandlerCollection, String userName, DBCommandProperties command, CallerContext callerContext)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            OperationResultCollection opResults=new OperationResultCollection();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");
                    parameters = generator.GetParameters("ConfigurationManager_MDMEventHandler_Process_ParametersArray");
                    SqlMetaData[] eventHandlerMetaData = generator.GetTableValueMetadata("ConfigurationManager_MDMEventHandler_Process_ParametersArray", parameters[0].ParameterName);

                    List<SqlDataRecord> eventHandlerRecord = this.FillEventHandlerSqlDataRecord(mdmEventHandlerCollection, eventHandlerMetaData);

                    parameters[0].Value = eventHandlerRecord;
                    parameters[1].Value = userName;
                    parameters[2].Value = callerContext.ProgramName;

                    storedProcedureName = "usp_ConfigurationManager_EventHandler_Process";
                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    this.UpdateOperationResult(reader, opResults);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

                transactionScope.Complete();
            }
            return opResults;
        }

        #endregion Public Methods

        #region Private Methods

        private MDMEventHandlerCollection FillEventHandlersInformations(SqlDataReader reader)
        {
            MDMEventHandlerCollection mdmEventHandlers = null;

            if (reader != null)
            {
                mdmEventHandlers = new MDMEventHandlerCollection();

                while (reader.Read())
                {
                    MDMEventHandler mdmEventHandler = new MDMEventHandler();

                    if (reader["Id"] != null)
                    {
                        mdmEventHandler.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                    }

                    if (reader["EventId"] != null)
                    {
                        mdmEventHandler.EventInfoId = ValueTypeHelper.Int32TryParse(reader["EventId"].ToString(), 0);
                    }

                    if (reader["AssemblyName"] != null)
                    {
                        mdmEventHandler.AssemblyName = reader["AssemblyName"].ToString();
                    }

                    if (reader["FullyQualifiedClassName"] != null)
                    {
                        mdmEventHandler.FullyQualifiedClassName = reader["FullyQualifiedClassName"].ToString();
                    }

                    if (reader["EventHandlerMethodName"] != null)
                    {
                        mdmEventHandler.EventHandlerMethodName = reader["EventHandlerMethodName"].ToString();
                    }

                    if (reader["Sequence"] != null)
                    {
                        mdmEventHandler.Sequence = ValueTypeHelper.Int32TryParse(reader["Sequence"].ToString(), 0);
                    }

                    if (reader["Module"] != null)
                    {
                        MDMCenterExtensionEnum module = MDMCenterExtensionEnum.UnKnown;
                        ValueTypeHelper.EnumTryParse<MDMCenterExtensionEnum>(reader["Module"].ToString(), true, out module);

                        mdmEventHandler.Module = module;
                    }

                    if (reader["SubscribedServiceTypes"] != null)
                    {
                        mdmEventHandler.SubscribedOnServiceTypes = ValueTypeHelper.SplitStringToEnumCollection<MDMServiceType>(reader["SubscribedServiceTypes"].ToString(), ',');
                    }

                    if (reader["IsStatic"] != null)
                    {
                        mdmEventHandler.IsHandlerMethodStatic = ValueTypeHelper.BooleanTryParse(reader["IsStatic"].ToString(), false);
                    }

                    if (reader["Enabled"] != null)
                    {
                        mdmEventHandler.Enabled = ValueTypeHelper.BooleanTryParse(reader["Enabled"].ToString(), false);
                    }

                    if (reader["IsInternal"] != null)
                    {
                        mdmEventHandler.IsInternal = ValueTypeHelper.BooleanTryParse(reader["IsInternal"].ToString(), false);
                    }

                    if (reader["AppConfigKeyName"] != null)
                    {
                        mdmEventHandler.AppConfigKeyName = reader["AppConfigKeyName"].ToString();
                    }

                    if (reader["AppConfigKeyValue"] != null)
                    {
                        mdmEventHandler.AppConfigKeyValue = reader["AppConfigKeyValue"].ToString();
                    }

                    if (reader["FeatureConfigKeyName"] != null)
                    {
                        mdmEventHandler.FeatureConfigKeyName = reader["FeatureConfigKeyName"].ToString();
                    }

                    if (reader["FeatureConfigKeyValue"] != null)
                    {
                        mdmEventHandler.FeatureConfigKeyValue = ValueTypeHelper.BooleanTryParse(reader["FeatureConfigKeyValue"].ToString(), false);
                    }

                    mdmEventHandlers.Add(mdmEventHandler);
                }
            }

            return mdmEventHandlers;
        }

        private void UpdateOperationResult(SqlDataReader reader, OperationResultCollection opResults)
        {
            if (!(reader == null || reader.IsClosed))
            {
                while (reader.Read())
                {
                    String errorCode = String.Empty;
                    OperationResult opResult = new OperationResult();

                    if (reader["Id"] != null)
                    {
                        opResult.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                    }

                    if (reader["ReferenceId"] != null)
                    {
                        opResult.ReferenceId = reader["ReferenceId"].ToString();
                    }

                    if (reader["Action"] != null)
                    {
                        ObjectAction action = ObjectAction.Unknown;
                        ValueTypeHelper.EnumTryParse<ObjectAction>(reader["Action"].ToString(), true, out action);
                        opResult.PerformedAction = action;
                    }

                    opResults.Add(opResult);
                }
            }
        }

        private List<SqlDataRecord> FillEventHandlerSqlDataRecord(MDMEventHandlerCollection mdmEventHandlerCollection, SqlMetaData[] eventHandlerMetaData)
        {
            List<SqlDataRecord> eventHandlerRecordList = new List<SqlDataRecord>();
            Int32 eventHandlerId = -1;

            foreach (MDMEventHandler eventHandler in mdmEventHandlerCollection)
            {
                if (eventHandler != null)
                {
                    if (eventHandler.Action == ObjectAction.Create)
                    {
                        eventHandler.Id = eventHandlerId--;
                    }

                    SqlDataRecord eventHandlerRecord = new SqlDataRecord(eventHandlerMetaData);
                    eventHandlerRecord.SetValue(0, eventHandler.Id);
                    eventHandlerRecord.SetValue(1, eventHandler.EventInfoId);
                    eventHandlerRecord.SetValue(2, eventHandler.AssemblyName);
                    eventHandlerRecord.SetValue(3, eventHandler.FullyQualifiedClassName);
                    eventHandlerRecord.SetValue(4, eventHandler.EventHandlerMethodName);
                    eventHandlerRecord.SetValue(5, eventHandler.Sequence);
                    eventHandlerRecord.SetValue(6, eventHandler.Module.ToString());
                    eventHandlerRecord.SetValue(7, ValueTypeHelper.JoinCollection<MDMServiceType>(eventHandler.SubscribedOnServiceTypes, ","));
                    eventHandlerRecord.SetValue(8, eventHandler.Enabled);
                    eventHandlerRecord.SetValue(9, eventHandler.IsHandlerMethodStatic);
                    eventHandlerRecord.SetValue(10, eventHandler.IsInternal);
                    eventHandlerRecord.SetValue(11, eventHandler.AppConfigKeyName);
                    eventHandlerRecord.SetValue(12, eventHandler.AppConfigKeyValue);
                    eventHandlerRecord.SetValue(13, eventHandler.FeatureConfigKeyName);
                    eventHandlerRecord.SetValue(14, eventHandler.FeatureConfigKeyValue);
                    eventHandlerRecord.SetValue(15, eventHandler.Action.ToString());
                    eventHandlerRecordList.Add(eventHandlerRecord);
                }
            }

            return eventHandlerRecordList;
        }

        #endregion Private Methods

        #endregion Methods
    }
}
