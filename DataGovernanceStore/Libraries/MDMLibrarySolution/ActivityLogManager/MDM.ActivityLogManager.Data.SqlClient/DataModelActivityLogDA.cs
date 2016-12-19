using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using MDM.BusinessObjects.Diagnostics;

namespace MDM.ActivityLogManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    public class DataModelActivityLogDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String SqlParametersGeneratorName = "ActivityLogManager_SqlParameters";

        #endregion

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region public method

        /// <summary>
        /// Processes the impacted entity log
        /// </summary>
        /// <param name="DataModelActivityLogCollection">Collection of activities</param>
        /// <param name="loginUser">logged in user</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>if operation is success it returns true else false</returns>
        public OperationResult Process(DataModelActivityLogCollection DataModelActivityLogCollection, String loginUser, CallerContext callerContext, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            #region initialization

            OperationResult processResult = new OperationResult();
            DiagnosticActivity activity = new DiagnosticActivity();

            #endregion initialization
            
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("ActivityLogManager_SqlParameters");

                    SqlParameter[] parameters =
                        generator.GetParameters("DataModelManager_ActivityLog_Process_ParametersArray");

                    SqlMetaData[] sqlMetadata =
                        generator.GetTableValueMetadata("DataModelManager_ActivityLog_Process_ParametersArray",
                                                        parameters[0].ParameterName);

                    List<SqlDataRecord> dataModelActivityLogRecords =
                        MDM.Utility.DataModelActivityLogUtils.GetSqlRecords(DataModelActivityLogCollection, sqlMetadata);

                    //if there are no records to be updated just return
                    if (dataModelActivityLogRecords.Count <= 0)
                    {
                        processResult.AddOperationResult("-1", "No DataModelActivityLog item found for processing",
                            OperationResultType.Error);
                        processResult.RefreshOperationResultStatus();
                        return processResult;
                    }

                    parameters[0].Value = dataModelActivityLogRecords;
                    parameters[1].Value = callerContext.ProgramName;
                    parameters[2].Value = loginUser;

                    const String storedProcedureName = "usp_ActivityLogManager_DataModel_ActivityLog_Process";

                    ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                activity.LogError("DataModelActivityLogDA: " + exception.Message);
                processResult.AddOperationResult("-1", exception.Message, OperationResultType.Error);
                return processResult;
            }

            return processResult;

        }

        /// <summary>
        /// Gets all the impacted logs based on the log status
        /// * LogType.Current -> get all the records from tb_DataModelActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_DataModelActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_DataModelActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="logStatus">LogType.Current,LogType.Past,LogType.Pending</param>
        /// <param name="toRecordNumber"></param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <param name="fromRecordNumber"></param>
        /// <returns></returns>
        public DataModelActivityLogCollection Get(ProcessingStatus logStatus, Int64 fromRecordNumber, Int64 toRecordNumber, DBCommandProperties command)
        {

            //            Tables:
            //tb_DataModel_ActivityLog
            //tb_DataModel_ActivityLog_HS

            //TVP:
            //DataModelActivityLogTable

            //Trigger:
            //trg_DataModel_ActivityLog_Delete

            //Procedures:
            //usp_ActivityLogManager_DataModel_ActivityLog_Get
            //usp_ActivityLogManager_DataModel_ActivityLog_Process


            SqlDataReader reader = null;
            DataModelActivityLogCollection resultCollection = new DataModelActivityLogCollection();
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ActivityLogManager_SqlParameters");

                SqlParameter[] parameters =
                    generator.GetParameters("ActivityLogManager_DataModel_ActivityLog_Get_ParametersArray");

                parameters[0].Value = logStatus.ToString();
                parameters[1].Value = fromRecordNumber;
                parameters[2].Value = toRecordNumber;

                const String storedProcedureName = "usp_ActivityLogManager_DataModel_ActivityLog_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 id = 0;
                        Int32 fk_Org = 0;
                        Int32 fk_Catalog = 0;
                        Int32 fk_NodeType = 0;
                        Int32 fk_RelationshipType = 0;
                        Collection<Int32> attributeIdList;
                        Int32 mdmObjectId = 0;
                        ObjectAction action = ObjectAction.Unknown;
                        DataModelActivityList activityLogAction = DataModelActivityList.Unknown;
                        Boolean isLoadingInProgress = false;
                        Boolean isLoaded = false;
                        Boolean isProcessed = false;
                        DateTime? loadStartTime = null;
                        DateTime? loadEndTime = null;
                        DateTime? processStartTime = null;
                        DateTime? processEndTime = null;
                        Int32 impactedCount = 0;
                        Int32 fk_Server = 0;
                        String changedData;
                        String programName = String.Empty;
                        String userName = String.Empty;
                        String serverName = String.Empty;
                        DateTime? createdDateTime = null;
                        Int32 weightage = 0;
                        Int64 auditRefId = -1;

                        if (reader["Id"] != null)
                            id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);

                        if (reader["FK_Org"] != null)
                            fk_Org = ValueTypeHelper.Int32TryParse(reader["FK_Org"].ToString(), 0);

                        if (reader["FK_Catalog"] != null)
                            fk_Catalog = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                        if (reader["FK_NodeType"] != null)
                            fk_NodeType = ValueTypeHelper.Int32TryParse(reader["FK_NodeType"].ToString(), 0);

                        if (reader["FK_RelationshipType"] != null)
                            fk_RelationshipType = ValueTypeHelper.Int32TryParse(reader["EntityId"].ToString(), 0);

                        if (reader["AttributeIdList"] != null)
                            attributeIdList =
                                ValueTypeHelper.SplitStringToIntCollection(reader["AttributeIdList"].ToString(), ',');

                        if (reader["MDMObjectId"] != null)
                            mdmObjectId = ValueTypeHelper.Int32TryParse(reader["MDMObjectId"].ToString(), 0);

                        //TODO ENUM TRY PARSE
                        if (reader["activityLogAction"] != null)
                            ValueTypeHelper.EnumTryParse(reader["activityLogAction"].ToString(), true, out activityLogAction);

                        //if (reader["PerformedAction"] != null)
                        //    action = ValueTypeHelper.Int32TryParse(reader["Action"].ToString(), 0);

                        if (reader["IsLoadingInProgress"] != null)
                            isLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader["IsLoadingInProgress"].ToString(), false);

                        if (reader["IsLoaded"] != null)
                            isLoaded = ValueTypeHelper.BooleanTryParse(reader["IsLoaded"].ToString(), false);

                        if (reader["IsProcessed"] != null)
                            isProcessed = ValueTypeHelper.BooleanTryParse(reader["IsProcessed"].ToString(), false);

                        if (reader["LoadStartTime"] != null)
                            loadStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadStartTime"].ToString());

                        if (reader["LoadEndTime"] != null)
                            loadEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader["LoadEndTime"].ToString());

                        if (reader["ProcessStartTime"] != null)
                            processStartTime =
                                ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessStartTime"].ToString());

                        if (reader["ProcessEndTime"] != null)
                            processEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ProcessEndTime"].ToString());

                        if (reader["ImpactedCount"] != null)
                            impactedCount = ValueTypeHelper.Int32TryParse(reader["ImpactedCount"].ToString(), 0);

                        if (reader["ServerId"] != null)
                            fk_Server = ValueTypeHelper.Int32TryParse(reader["ServerId"].ToString(), 0);

                        if (reader["ChangedData"] != null)
                            changedData = reader["ChangedData"].ToString();

                        if (reader["ProgramName"] != null)
                            programName = reader["ProgramName"].ToString();

                        if (reader["CreateDateTime"] != null)
                            createdDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["CreateDateTime"].ToString());

                        if (reader["Weightage"] != null)
                            weightage = ValueTypeHelper.Int32TryParse(reader["Weightage"].ToString(), 0);

                        if (reader["FK_Audit_Ref"] != null)
                            auditRefId = ValueTypeHelper.Int32TryParse(reader["FK_Audit_Ref"].ToString(), 0);

                        DataModelActivityLog impactedEntityLog = new DataModelActivityLog()
                        {
                            Id = id,
                            ContainerId = -fk_Catalog,
                            DataModelActivityLogAction = (DataModelActivityList)action,
                            IsLoadingInProgress = isLoadingInProgress,
                            IsLoaded = isLoaded,
                            IsProcessed = isProcessed,
                            LoadStartTime = loadStartTime,
                            LoadEndTime = loadEndTime,
                            ProcessStartTime = processStartTime,
                            ProcessEndTime = processEndTime,
                            CreatedDateTime = createdDateTime,
                            ImpactedCount = impactedCount,
                            Weightage = weightage,
                            ProgramName = programName,
                            UserName = userName,
                            ServerId = fk_Server,
                            AuditRefId = auditRefId
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

        #endregion 

        #region Private Methods

        #endregion Private Methods

    }
}
