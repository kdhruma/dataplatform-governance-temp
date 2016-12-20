using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.DiagnosticData.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.Interfaces.Diagnostics;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies DiagnosticData DA layer
    /// </summary>
    public class DiagnosticDataDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String SQLParametersGeneratorName = "InstrumentationManager_SqlParameters";

        private const String DiagnosticDataProcessSPName = "usp_InstrumentationManager_DiagnosticData_Process";
        private const String DiagnosticDataProcessSPParameters = "InstrumentationManager_DiagnosticData_Process_ParametersArray";

        private const String DiagnosticDataGetSPName = "usp_InstrumentationManager_DiagnosticData_Get";
        private const String DiagnosticDataGetSPParameters = "InstrumentationManager_DiagnosticData_Get_ParametersArray";

        #endregion

        #region Fields

        private String MessageClassEnumStaticTextCode = ((Int32)MessageClassEnum.StaticText).ToString();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #region Public Methods
        
        /// <summary>
        /// Process DA
        /// </summary>
        /// <param name="diagnosticDataElement"></param>
        /// <param name="processingMode"></param>
        public void Process(IDiagnosticDataElement[] diagnosticDataElement, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            #region Diagnostics or Tracing
            
            // Should we do Activity based tracing here? as this may generate infinite msgs Posted to DiagDataProcessor
            //var traceSettings = MDMOperationContextHelper.GetRequestContextData().TraceSettings;
            //DiagnosticActivity activity = new DiagnosticActivity();

            //if (traceSettings.IsTracingEnabled) activity.Start();

            #endregion

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
                {
                    #region Create DiagnosticData and DiagnosticExecutionContext TVP

                    SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);
                    SqlParameter[] parameters = generator.GetParameters(DiagnosticDataProcessSPParameters);
                    SqlMetaData[] diagnosticDataData = generator.GetTableValueMetadata(DiagnosticDataProcessSPParameters, parameters[0].ParameterName);
                    SqlMetaData[] diagnosticExecutionContextData = generator.GetTableValueMetadata(DiagnosticDataProcessSPParameters, parameters[1].ParameterName);

                    List<SqlDataRecord> diagnosticDataTable = new List<SqlDataRecord>();
                    List<SqlDataRecord> diagnosticExecutionContextTable = new List<SqlDataRecord>();

                    FillTableValues(diagnosticDataElement, diagnosticDataData, diagnosticExecutionContextData, diagnosticDataTable, diagnosticExecutionContextTable);

                    #endregion

                    #region Execute proc

                    if (diagnosticDataTable != null && diagnosticDataTable.Count > 0)
                    {
                        if (diagnosticExecutionContextTable != null && diagnosticExecutionContextTable.Count < 1)
                        {
                            diagnosticExecutionContextTable = null;
                        }

                        parameters[0].Value = diagnosticDataTable;
                        parameters[1].Value = diagnosticExecutionContextTable;

                        ExecuteProcedureScalar(AppConfigurationHelper.ConnectionString, parameters, DiagnosticDataProcessSPName);
                    }

                    transactionScope.Complete();

                    #endregion
                }
            }
            finally
            {
                //if (traceSettings.IsTracingEnabled) activity.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportSettings"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DiagnosticActivityCollection GetActivities(DiagnosticReportSettings diagnosticReportSettings, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            DiagnosticActivityCollection activitiesInHierarchyStructure = new DiagnosticActivityCollection();
            Dictionary<Guid, DiagnosticActivity> activitiesInFlatStructure = new Dictionary<Guid, DiagnosticActivity>();
            DiagnosticRecordCollection records = new DiagnosticRecordCollection();
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                SqlParameter[] parameters = generator.GetParameters(DiagnosticDataGetSPParameters);

                #region Populate table value parameters

                SqlMetaData[] keyValueTableMetadata = generator.GetTableValueMetadata(DiagnosticDataGetSPParameters, parameters[2].ParameterName);
                List<SqlDataRecord> keyValueTableList = PopulateKeyValueTableList(keyValueTableMetadata, diagnosticReportSettings);

                #endregion

                parameters[0].Value = diagnosticReportSettings.DataRequestType;
                parameters[1].Value = diagnosticReportSettings.Level;
                parameters[2].Value = keyValueTableList;

                if (diagnosticReportSettings.FromDateTime.HasValue)
                {
                    parameters[3].Value = diagnosticReportSettings.FromDateTime.Value;
                }
                
                if (diagnosticReportSettings.ToDateTime.HasValue)
                {
                    parameters[4].Value = diagnosticReportSettings.ToDateTime.Value;
                }

                if (diagnosticReportSettings.MaxRecordsToReturn.HasValue)
                {
                    parameters[5].Value = diagnosticReportSettings.MaxRecordsToReturn.Value;
                }

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, DiagnosticDataGetSPName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 diagnosticDataId = -1;
                        Int32 diagnosticObjectType = 0;
                        Guid diagnosticActivityId = Guid.Empty;
                        Guid parentActivityId = Guid.Empty;
                        Guid returnedOperationId = Guid.Empty;
                        String activityName = String.Empty;
                        DateTime startDateTime = DateTime.MinValue;
                        DateTime endDateTime = DateTime.MinValue;
                        DateTime timestamp = DateTime.MinValue;
                        Int32 threadId = -1;
                        Double durationInMilliseconds = 0;
                        MessageClassEnum messageClass = MessageClassEnum.UnKnown;
                        String message = String.Empty;
                        String messageCode = String.Empty;
                        Int32 referenceId = -1;
                        Int64 executionContextId = -1;
                        String dataXml = String.Empty;
                        Boolean hasActivityExtendedData = false;
                        
                        if (diagnosticReportSettings.IncludeActivityExtendedData && reader["Data"] != null && reader["Data"] != DBNull.Value)
                        {
                            Object dataXmlObj = reader["Data"];
                            Byte[] dataXmlAsByteArray = dataXmlObj as Byte[];
                            dataXml = ProtoBufSerializationHelper.Deserialize<String>(dataXmlAsByteArray);
                        }

                        if (reader["PK_DiagnosticData"] != null)
                        {
                            diagnosticDataId = ValueTypeHelper.Int64TryParse(reader["PK_DiagnosticData"].ToString(), -1);
                        }

                        if (reader["DiagnosticObjectType"] != null)
                        {
                            diagnosticObjectType = ValueTypeHelper.Int32TryParse(reader["DiagnosticObjectType"].ToString(), 0);
                        }

                        if (reader["ActivityId"] != null)
                        {
                            String strActivityId = reader["ActivityId"].ToString();

                            Guid.TryParse(strActivityId, out diagnosticActivityId);
                        }

                        if (reader["ParentActivityId"] != null)
                        {
                            String strParentActivityId = reader["ParentActivityId"].ToString();

                            Guid.TryParse(strParentActivityId, out parentActivityId);
                        }

                        if (reader["OperationId"] != null)
                        {
                            String strReturnedOperationId = reader["OperationId"].ToString();

                            Guid.TryParse(strReturnedOperationId, out returnedOperationId);
                        }

                        if (reader["ActivityName"] != null)
                        {
                            activityName = reader["ActivityName"].ToString();
                        }

                        if (reader["Message"] != null)
                        {
                            message = reader["Message"].ToString();
                        }

                        if (reader["Duration"] != null)
                        {
                            durationInMilliseconds = ValueTypeHelper.DoubleTryParse(reader["Duration"].ToString(), 0);
                        }

                        if (reader["StartDateTime"] != null && reader["StartDateTime"] != DBNull.Value)
                        {
                            startDateTime = (DateTime) reader["StartDateTime"];
                            if (startDateTime.Kind == DateTimeKind.Unspecified)
                            {
                                startDateTime = new DateTime(startDateTime.Ticks, DateTimeKind.Local);
                            }
                        }

                        if (reader["EndDateTime"] != null && reader["EndDateTime"] != DBNull.Value)
                        {
                            endDateTime = (DateTime) reader["EndDateTime"];
                            if (endDateTime.Kind == DateTimeKind.Unspecified)
                            {
                                endDateTime = new DateTime(endDateTime.Ticks, DateTimeKind.Local);
                            }
                        }

                        if (reader["Timestamp"] != null && reader["Timestamp"] != DBNull.Value)
                        {
                            timestamp = (DateTime) reader["Timestamp"];
                            if (timestamp.Kind == DateTimeKind.Unspecified)
                            {
                                timestamp = new DateTime(timestamp.Ticks, DateTimeKind.Local);
                            }
                        }

                        if (reader["ThreadId"] != null)
                        {
                            threadId = ValueTypeHelper.Int32TryParse(reader["ThreadId"].ToString(), -1);
                        }

                        if (reader["MessageClass"] != null)
                        {
                            String value = reader["MessageClass"].ToString();
                            if (value == MessageClassEnumStaticTextCode)
                            {
                                // Backward compatibility for deprecated MessageClassEnum.StaticText
                                messageClass = MessageClassEnum.Information;
                            }
                            else
                            {
                                ValueTypeHelper.EnumTryParse<MessageClassEnum>(value, true, out messageClass);
                            }
                        }

                        if (reader["MessageCode"] != null)
                        {
                            messageCode = reader["MessageCode"].ToString();
                        }

                        if (reader["ReferenceId"] != null)
                        {
                            referenceId = ValueTypeHelper.Int32TryParse(reader["ReferenceId"].ToString(), -1);
                        }

                        if (reader["FK_DiagnosticExecutionContext"] != null)
                        {
                            executionContextId = ValueTypeHelper.Int64TryParse(reader["FK_DiagnosticExecutionContext"].ToString(), -1);
                        }

                        if (reader["HasActivityExtendedData"] != null)
                        {
                            hasActivityExtendedData = ValueTypeHelper.BooleanTryParse(reader["HasActivityExtendedData"].ToString(), false);
                        }

                        ExecutionContext executionContext = null;
                        if (diagnosticReportSettings.IncludeContextData || diagnosticReportSettings.IncludeExecutionContextExtendedData)
                        {
                            executionContext = GetExecutionContextFromReader(reader, diagnosticReportSettings.IncludeContextData, diagnosticReportSettings.IncludeExecutionContextExtendedData);
                        }

                        if (diagnosticObjectType == 1)
                        {
                            DiagnosticActivity activity = new DiagnosticActivity(executionContext)
                            {
                                Id = diagnosticDataId,
                                ActivityId = diagnosticActivityId,
                                ParentActivityId = parentActivityId,
                                OperationId = returnedOperationId,
                                ActivityName = activityName,
                                ReferenceId = referenceId,
                                //StartDateTime = startDateTime, Please see comment below
                                EndDateTime = endDateTime,
                                TimeStamp = timestamp,
                                ThreadId = threadId,
                                DurationInMilliSeconds = durationInMilliseconds,
                                ExecutionContextId = executionContextId
                            };
                            // TimeStamp setter changes StartdDateTime value, so we need to set StartdDateTime after TimeStamp
                            activity.StartDateTime = startDateTime;

                            DiagnosticActivity existActivity;
                            if (activitiesInFlatStructure.TryGetValue(activity.ActivityId, out existActivity))
                            {
                                if (existActivity.TimeStamp > activity.TimeStamp)
                                {
                                    activitiesInFlatStructure[activity.ActivityId] = activity;
                                }
                            }
                            else
                            {
                                activitiesInFlatStructure.Add(activity.ActivityId, activity);
                            }
                        }
                        else if (diagnosticObjectType == 2)
                        {
                            DiagnosticActivity parentActivity = new DiagnosticActivity()
                            {
                                ActivityId = parentActivityId
                            };

                            DiagnosticRecord record = new DiagnosticRecord(executionContext, hasActivityExtendedData)
                            {
                                Id = diagnosticDataId,
                                ActivityId = diagnosticActivityId,
                                OperationId = returnedOperationId,
                                MessageCode = messageCode,
                                MessageClass = messageClass,
                                Message = message,
                                TimeStamp = timestamp,
                                ThreadId = threadId,
                                ReferenceId = referenceId,
                                ExecutionContextId = executionContextId,
                                ParentActivity = parentActivity,
                                DurationInMilliSeconds = durationInMilliseconds,
                                DataXml = dataXml
                            };

                            records.Add(record);
                        }
                    }
                }

                #region Diagnostic Activities tree generation

                if (activitiesInFlatStructure.Count > 0)
                {
                    foreach (DiagnosticActivity activity in activitiesInFlatStructure.Values)
                    {
                        DiagnosticActivity parentActivity;
                        if (activitiesInFlatStructure.TryGetValue(activity.ParentActivityId, out parentActivity))
                        {
                            parentActivity.DiagnosticActivities.Add(activity);
                        }
                        else
                        {
                            activitiesInHierarchyStructure.Add(activity);
                        }
                    }
                }

                if (records.Count > 0)
                {
                    //Create a dummy activity for records who are not having any parent...
                    //And add to Hierarchy structure
                    DiagnosticActivity dummyActivity = new DiagnosticActivity();
                    dummyActivity.OperationId = Guid.Empty;
                    dummyActivity.ActivityName = "Others";

                    foreach (DiagnosticRecord record in records)
                    {
                        DiagnosticActivity parentActivity;
                        if (activitiesInFlatStructure.TryGetValue(record.ParentActivity.ActivityId, out parentActivity))
                        {
                            parentActivity.DiagnosticRecords.Add(record);
                        }
                        else
                        {
                            dummyActivity.DiagnosticRecords.Add(record);
                        } 
                    }

                    if (dummyActivity.DiagnosticRecords.Count > 0)
                    {
                        activitiesInHierarchyStructure.Add(dummyActivity);
                    }
                }

                #endregion
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return activitiesInHierarchyStructure;
        }

        /// <summary>
        /// Get Records DA
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="command"></param>
        public DiagnosticRecordCollection GetRecords(Guid operationId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            DiagnosticRecordCollection resultCollection = new DiagnosticRecordCollection();
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);

                SqlParameter[] parameters = generator.GetParameters(DiagnosticDataGetSPParameters);

                SqlMetaData[] keyValueTableMetadata = generator.GetTableValueMetadata(DiagnosticDataGetSPParameters, parameters[2].ParameterName);
                List<SqlDataRecord> keyValueTableList = new List<SqlDataRecord>();
                
                AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "OperationId", operationId.ToString());

                parameters[0].Value = 0;
                parameters[1].Value = -1;
                parameters[2].Value = keyValueTableList;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, DiagnosticDataGetSPName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 diagnosticObjectType = 0;
                        Guid activityId = Guid.Empty; //
                        Guid operationId2 = Guid.Empty; //
                        String messageCode = String.Empty; //
                        MessageClassEnum messageClass = MessageClassEnum.UnKnown; //
                        String message = String.Empty; //
                        DateTime timestamp = DateTime.MinValue; //
                        Int32 threadId = -1; //
                        Int32 referenceId = -1; //
                        String dataXml = String.Empty;
                        Boolean hasActivityExtendedData = false;

                        if (reader["DiagnosticObjectType"] != null)
                            diagnosticObjectType = ValueTypeHelper.Int32TryParse(reader["DiagnosticObjectType"].ToString(), 0);

                        if (diagnosticObjectType == 2)
                        {
                            if (reader["ActivityId"] != null)
                                activityId = (Guid)reader["ActivityId"];

                            if (reader["OperationId"] != null)
                                operationId2 = (Guid)reader["OperationId"];

                            if (reader["MessageCode"] != null)
                                messageCode = reader["MessageCode"].ToString();

                            if (reader["MessageClass"] != null)
                            {
                                String value = reader["MessageClass"].ToString();
                                if (value == MessageClassEnumStaticTextCode)
                                {
                                    // Backward compatibility for deprecated MessageClassEnum.StaticText
                                    messageClass = MessageClassEnum.Information;
                                }
                                else
                                {
                                    ValueTypeHelper.EnumTryParse<MessageClassEnum>(value, true, out messageClass);
                                }
                            }

                            if (reader["Message"] != null)
                                message = reader["Message"].ToString();

                            if (reader["Timestamp"] != null)
                                timestamp = ValueTypeHelper.ConvertToDateTime(reader["Timestamp"].ToString());

                            if (reader["ThreadId"] != null)
                                threadId = ValueTypeHelper.Int32TryParse(reader["ThreadId"].ToString(), -1);

                            if (reader["ReferenceId"] != null)
                                referenceId = ValueTypeHelper.Int32TryParse(reader["ReferenceId"].ToString(), -1);

                            if (reader["Data"] != null && reader["Data"] != DBNull.Value)
                            {
                                Object dataXmlObj = reader["Data"];
                                Byte[] dataXmlAsByteArray = dataXmlObj as Byte[];
                                dataXml = ProtoBufSerializationHelper.Deserialize<String>(dataXmlAsByteArray);
                            }

                            if (reader["HasActivityExtendedData"] != null)
                            {
                                hasActivityExtendedData = ValueTypeHelper.BooleanTryParse(reader["HasActivityExtendedData"].ToString(), false);
                            }

                            DiagnosticRecord record = new DiagnosticRecord(null, hasActivityExtendedData)
                            {
                                ActivityId = activityId,
                                OperationId = operationId2,
                                MessageCode = messageCode,
                                MessageClass = messageClass,
                                Message = message,
                                TimeStamp = timestamp,
                                ThreadId = threadId,
                                ReferenceId = referenceId,
                                DataXml = dataXml
                            };

                            resultCollection.Add(record);
                        }
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
        /// Delete all existing traces based on operationId
        /// </summary>
        /// <param name="operationId">Indicates the operation Id</param>
        /// <param name="command">Indicates the db command properties</param>
        /// <rereturns>Returns successful row count</rereturns>
        public Int32 DeleteTraces(Guid operationId, DBCommandProperties command)
        {
            Int32 result = -1;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);
                SqlParameter[] parameters = generator.GetParameters("InstrumentationManager_DiagnosticData_TraceDelete_ParametersArray");

                parameters[0].Value = operationId;

                result = ExecuteProcedureNonQuery(command.ConnectionString, parameters, "usp_InstrumentationManager_DiagnosticData_Delete");

                transactionScope.Complete();
            }

            return result;
        }

        #endregion

        #region Private Methods

        private void FillTableValues(IDiagnosticDataElement[] diagnosticDataElements, SqlMetaData[] diagnosticDataData, SqlMetaData[] diagnosticExecutionContextData, List<SqlDataRecord> diagnosticDataTable, List<SqlDataRecord> diagnosticExecutionContextTable)
        {
            List<Guid> activityIds = new List<Guid>();

            Int32 messageMaxLength = 2000;

            foreach (IDiagnosticDataElement diagnosticData in diagnosticDataElements)
            {
                #region Initialization

                DiagnosticActivity activity = diagnosticData as DiagnosticActivity;
                DiagnosticRecord record = null;

                #endregion

                if (activity != null)
                {
                    if (activity.DoNotPersist)
                    {
                        continue;
                    }

                    #region Create diagnostic data table from activity

                    SqlDataRecord activityDataRecord = new SqlDataRecord(diagnosticDataData);

                    activityDataRecord.SetValue(0, (Int16)1);                                           //DiagnosticObjectType (short)1DiagnosticObjectType.DiagnosticActivity
                    activityDataRecord.SetValue(1, activity.ActivityId);                                //ActivityId
                    activityDataRecord.SetValue(2, activity.ActivityName);                              //ActivityName
                    activityDataRecord.SetValue(3, activity.ParentActivityId);                          //ParentActivityId
                    activityDataRecord.SetValue(4, activity.OperationId);                               //OperationId
                    activityDataRecord.SetValue(5, activity.ReferenceId);                               //ReferenceId
                    activityDataRecord.SetValue(6, activity.TimeStamp);                                 //TimeStamp
                    activityDataRecord.SetValue(7, activity.StartDateTime);                             //StartDateTime
                    activityDataRecord.SetValue(8, activity.EndDateTime);                               //EndDateTime

                    if (activity.ComputeDurationOnSqlServerLevel)
                    {
                        // Special case. Duration will be computed on SQL Server level (difference between current EndDateTime and already exists DB record's StartDateTime).
                        activityDataRecord.SetValue(9, (Int64)(-1));                                    //Duration
                    }
                    else
                    {
                        activityDataRecord.SetValue(9, (Int64)activity.DurationInMilliSeconds);         //Duration
                    }

                    activityDataRecord.SetValue(10, String.Empty);                                      //MessageCode
                    activityDataRecord.SetValue(11, (Int16)MessageClassEnum.Information);               //MessageClass
                    activityDataRecord.SetValue(12, String.Empty);                                      //Message
                    activityDataRecord.SetValue(13, activity.ThreadId);                                 //ThreadId
                    activityDataRecord.SetValue(14, 0);                                                 //ThreadNumber
                    activityDataRecord.SetValue(15, DBNull.Value);                                      //Data
                    activityDataRecord.SetValue(16, "Binary");                                          //DataObjectType

                    diagnosticDataTable.Add(activityDataRecord);

                    #endregion

                    #region Create execution context data table for this activity if not done so already

                    if (!activityIds.Contains(activity.ActivityId))
                    {
                        // Create Execution Context Record for this activity
                        FillExecutionContextTableValues(activity.ExecutionContext, activity.ActivityId, activity.OperationId, diagnosticExecutionContextData, diagnosticExecutionContextTable);
                        activityIds.Add(activity.ActivityId);
                    }

                    #endregion
                }
                else
                {
                    #region Create diagnostic data table from DiagnosticRecord

                    record = diagnosticData as DiagnosticRecord;
                    Guid recordActivityId = Guid.NewGuid();

                    if (record != null)
                    {
                        SqlDataRecord diagnosticRecord = new SqlDataRecord(diagnosticDataData);

                        String message = record.Message.Length <= messageMaxLength ? record.Message : record.Message.Substring(0, messageMaxLength);

                        diagnosticRecord.SetValue(0, (Int16)2);                                //DiagnosticObjectType (short)1DiagnosticObjectType.DiagnosticActivity
                        diagnosticRecord.SetValue(1, recordActivityId);                        //ActivityId
                        diagnosticRecord.SetValue(2, String.Empty);                            //ActivityName
                        diagnosticRecord.SetValue(3, record.ActivityId);                       //ParentActivityId
                        diagnosticRecord.SetValue(4, record.OperationId);                      //OperationId
                        diagnosticRecord.SetValue(5, record.ReferenceId);                      //ReferenceId
                        diagnosticRecord.SetValue(6, record.TimeStamp);                        //TimeStamp
                        diagnosticRecord.SetValue(7, DBNull.Value);                            //StartDateTime
                        diagnosticRecord.SetValue(8, DBNull.Value);                            //EndDateTime
                        diagnosticRecord.SetValue(9, (Int64)record.DurationInMilliSeconds);    //Duration
                        diagnosticRecord.SetValue(10, record.MessageCode);                     //MessageCode
                        diagnosticRecord.SetValue(11, (Int16)record.MessageClass);             //MessageClass
                        diagnosticRecord.SetValue(12, message);                                //Message
                        diagnosticRecord.SetValue(13, record.ThreadId);                        //ThreadId
                        diagnosticRecord.SetValue(14, 0);                                      //ThreadNumber

                        Byte[] dataXmlBytes = null;
                        if (!String.IsNullOrEmpty(record.DataXml))
                        {
                            dataXmlBytes = ProtoBufSerializationHelper.Serialize<String>(record.DataXml);
                        }

                        diagnosticRecord.SetValue(15, dataXmlBytes != null ? (Object) dataXmlBytes : DBNull.Value); //Data
                        diagnosticRecord.SetValue(16, "Binary");                                                    //DataObjectType

                        diagnosticDataTable.Add(diagnosticRecord);

                        FillExecutionContextTableValues(record.ExecutionContext, recordActivityId, record.OperationId, diagnosticExecutionContextData, diagnosticExecutionContextTable);
                    }

                    #endregion
                }
            }
        }

        private void FillExecutionContextTableValues(ExecutionContext executionContext, Guid activityId, Guid operationId, SqlMetaData[] diagnosticExecutionContextData, List<SqlDataRecord> diagnosticExecutionContextTable)
        {
            #region Initialization

            SqlDataRecord executionContextRecord = new SqlDataRecord(diagnosticExecutionContextData);

            CallerContext callerContext = null;
            SecurityContext securityContext = null;
            CallDataContext callDataContext = null;
            Collection<MDMTraceSource> legacyMDMTraceSources = null;

            if (executionContext != null)
            {
                callerContext = executionContext.CallerContext;
                securityContext = executionContext.SecurityContext;
                callDataContext = executionContext.CallDataContext;
                legacyMDMTraceSources = executionContext.LegacyMDMTraceSources;
            }

            #endregion

            #region Create execution context data table

            executionContextRecord.SetValue(0, activityId);
            executionContextRecord.SetValue(1, callerContext != null ? (Int16)callerContext.Application : (Int16)0);     //"ApplicationId"
            executionContextRecord.SetValue(2, callerContext != null ? (Int32)callerContext.Module : 0);                 //"ModuleId"
            executionContextRecord.SetValue(3, callerContext != null ? callerContext.ProgramName : String.Empty);        //"ProgramName"
            executionContextRecord.SetValue(4, callerContext != null ? (Int32)callerContext.MDMSource : 0);              //"MDMSourceId"
            executionContextRecord.SetValue(5, callerContext != null ? (Int32)callerContext.MDMSubscriber : 0);          //"MDMSubscriberId"
            executionContextRecord.SetValue(6, callerContext != null ? callerContext.JobId : 0);                         //"JobId"
            executionContextRecord.SetValue(7, callerContext != null ? callerContext.ProfileId : 0);                     //"ProfileId"
            executionContextRecord.SetValue(8, callerContext != null ? callerContext.ProfileName : String.Empty);        //"ProfileName"
            executionContextRecord.SetValue(9, callerContext != null ? (Int32)callerContext.ServerId : 0);               //"ServerId"
            executionContextRecord.SetValue(10, callerContext != null ? callerContext.ServerName : String.Empty);        //"ServerName"
            executionContextRecord.SetValue(11, operationId);                                                            //"OperationId"
			executionContextRecord.SetValue(12, securityContext != null ? securityContext.UserId : 0);                   //"UserId"
			executionContextRecord.SetValue(13, securityContext != null ? securityContext.UserLoginName : String.Empty); //"UserName"
            executionContextRecord.SetValue(14, securityContext != null ? securityContext.UserRoleId : 0);               //"UserRoleId"
            executionContextRecord.SetValue(15, securityContext != null ? securityContext.UserRoleName : String.Empty);  //"UserRoleName"
            executionContextRecord.SetValue(16, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.OrganizationIdList, ",") : String.Empty);     //"OrganizationIdList"
            executionContextRecord.SetValue(17, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.ContainerIdList, ",") : String.Empty);        //"ContainerIdList"
            executionContextRecord.SetValue(18, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.EntityTypeIdList, ",") : String.Empty);       //"EntityTypeIdList"
			executionContextRecord.SetValue(19, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.RelationshipTypeIdList, ",") : String.Empty); //"RelationshipTypeIdList"
            executionContextRecord.SetValue(20, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.CategoryIdList, ",") : String.Empty);         //"CategoryIdList"
			executionContextRecord.SetValue(21, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.AttributeIdList, ",") : String.Empty);        //"AttributeIdList"
            executionContextRecord.SetValue(22, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.LocaleList, ",")  : String.Empty);            //"LocaleList"
            executionContextRecord.SetValue(23, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.LookupTableNameList, ",") : String.Empty);    //"LookupTableNameList"
            executionContextRecord.SetValue(24, callDataContext != null ? ValueTypeHelper.JoinCollection(callDataContext.EntityIdList, ",") : String.Empty);           //"EntityIdList"
            executionContextRecord.SetValue(25, legacyMDMTraceSources != null ? ValueTypeHelper.JoinCollection(legacyMDMTraceSources, ",") : String.Empty);            // legacyMDMTraceSources
            executionContextRecord.SetValue(26, executionContext != null ? executionContext.AdditionalContextData : String.Empty); //"AdditionalContextData"
            executionContextRecord.SetValue(27, callerContext != null ? callerContext.MDMPublisher : 0); //"MDMPublisher"

            diagnosticExecutionContextTable.Add(executionContextRecord);

            #endregion
        }

        private void AddSingleToKeyValueTableList(SqlMetaData[] keyValueTableMetadata, List<SqlDataRecord> keyValueTableList, String key, String item)
        {
            SqlDataRecord record = new SqlDataRecord(keyValueTableMetadata);
            record.SetValue(0, key);
            record.SetValue(1, item);
            keyValueTableList.Add(record);
        }

        private void AddToKeyValueTableList(SqlMetaData[] keyValueTableMetadata, List<SqlDataRecord> keyValueTableList, String key, IEnumerable<String> items)
        {
            foreach (var item in items)
            {
                SqlDataRecord record = new SqlDataRecord(keyValueTableMetadata);
                record.SetValue(0, key);
                record.SetValue(1, item);
                keyValueTableList.Add(record);
            }
        }

        private List<SqlDataRecord> PopulateKeyValueTableList(SqlMetaData[] keyValueTableMetadata, DiagnosticReportSettings diagnosticReportSettings)
        {
            List<SqlDataRecord> keyValueTableList = new List<SqlDataRecord>();

            #region Diagnostic Report 1st level settings

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "OperationId",
                diagnosticReportSettings.CallerContextFilter.OperationIdList.Where(x => x != Guid.Empty).Select(x => x.ToString()));

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ActivityId",
                diagnosticReportSettings.CallerContextFilter.ActivityIdList.Where(x => x != Guid.Empty).Select(x => x.ToString()));

            if (diagnosticReportSettings.Duration.HasValue && diagnosticReportSettings.DurationOperator.HasValue)
            {
                AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "Duration", diagnosticReportSettings.Duration.Value.ToString());
                AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "DurationOperator", GetDurationOperatorSign(diagnosticReportSettings.DurationOperator.Value));
            }

            if (diagnosticReportSettings.HasActivityExtendedData.HasValue)
            {
                AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "HasActivityExtendedData", diagnosticReportSettings.HasActivityExtendedData.Value.ToString());
            }

            AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "IncludeActivityExtendedData", diagnosticReportSettings.IncludeActivityExtendedData.ToString());
            AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "IncludeContextData", diagnosticReportSettings.IncludeContextData.ToString());
            AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "IncludeExecutionContextExtendedData", diagnosticReportSettings.IncludeExecutionContextExtendedData.ToString());

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "MessageClass", diagnosticReportSettings.MessageClasses.Select(x => ((Int32)x).ToString()));
            if (diagnosticReportSettings.MessageClasses.Contains(MessageClassEnum.Information))
            {
                // Backward compatibility for deprecated MessageClassEnum.StaticText
                AddSingleToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "MessageClass", MessageClassEnumStaticTextCode);
            }

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "LegacyTraceSource", diagnosticReportSettings.LegacyMDMTraceSources.Select(x => x.ToString()));

            if (diagnosticReportSettings.SearchColumns != null && diagnosticReportSettings.SearchColumns.Count > 0 &&
                diagnosticReportSettings.SearchKeywords != null && diagnosticReportSettings.SearchKeywords.Count > 0
               )
            {
                if (diagnosticReportSettings.SearchKeywords.Count != 1 || diagnosticReportSettings.SearchKeywords[0] != String.Empty)
                {
                    AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "SearchKeyword", diagnosticReportSettings.SearchKeywords);
                    AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "SearchColumn", diagnosticReportSettings.SearchColumns.Select(x => x.ToString()));
                }
            }

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "Message", diagnosticReportSettings.Messages);
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ThreadId", diagnosticReportSettings.ThreadIds.Select(x => x.ToString()));
            // 'ThreadNumber' filter is not supported for now (because it is empty on DB level, not populated by storage API)

            #endregion

            #region Security Context Filter

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "UserId", diagnosticReportSettings.SecurityContextFilter.UserIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "UserName", diagnosticReportSettings.SecurityContextFilter.UserLoginNameList);

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "UserRoleId", diagnosticReportSettings.SecurityContextFilter.UserRoleIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "UserRoleName", diagnosticReportSettings.SecurityContextFilter.UserRoleNameList);

            #endregion

            #region Call Data Context Filter

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "OrganizationId", diagnosticReportSettings.CallDataContext.OrganizationIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ContainerId", diagnosticReportSettings.CallDataContext.ContainerIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "EntityTypeId", diagnosticReportSettings.CallDataContext.EntityTypeIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "RelationshipTypeId", diagnosticReportSettings.CallDataContext.RelationshipTypeIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "CategoryId", diagnosticReportSettings.CallDataContext.CategoryIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "AttributeId", diagnosticReportSettings.CallDataContext.AttributeIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "EntityId", diagnosticReportSettings.CallDataContext.EntityIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "Locale", diagnosticReportSettings.CallDataContext.LocaleList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "LookupTableName", diagnosticReportSettings.CallDataContext.LookupTableNameList);
            
            #endregion

            #region Caller Context Filter

            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "MDMSourceId", diagnosticReportSettings.CallerContextFilter.MDMSourceList.Select(x => ((Int32)x).ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "MDMSubscriberId", diagnosticReportSettings.CallerContextFilter.MDMSubscriberList.Select(x => ((Int32)x).ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "MDMPublisherId", diagnosticReportSettings.CallerContextFilter.MDMPublisherList.Select(x => ((Int32)x).ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ApplicationId", diagnosticReportSettings.CallerContextFilter.ApplicationList.Select(x => ((Int32)x).ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ModuleId", diagnosticReportSettings.CallerContextFilter.ModuleList.Select(x => ((Int32)x).ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ServerId", diagnosticReportSettings.CallerContextFilter.ServerIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ServerName", diagnosticReportSettings.CallerContextFilter.ServerNameList);
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ProfileId", diagnosticReportSettings.CallerContextFilter.ProfileIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ProfileName", diagnosticReportSettings.CallerContextFilter.ProfileNameList);
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ProgramName", diagnosticReportSettings.CallerContextFilter.ProgramNameList);
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "JobId", diagnosticReportSettings.CallerContextFilter.JobIdList.Select(x => x.ToString()));
            AddToKeyValueTableList(keyValueTableMetadata, keyValueTableList, "ActivityName", diagnosticReportSettings.CallerContextFilter.ActivityNameList);

            #endregion

            return keyValueTableList;
        }

        private String GetDurationOperatorSign(SearchOperator durationOperator)
        {
            String durationOperatorSign = ">";

            switch(durationOperator)
            {
                case SearchOperator.GreaterThan:
                    durationOperatorSign = ">";
                    break;
                case SearchOperator.GreaterThanOrEqualTo:
                    durationOperatorSign = ">=";
                    break;
                case SearchOperator.EqualTo:
                    durationOperatorSign = "=";
                    break;
                case SearchOperator.LessThan:
                    durationOperatorSign = "<";
                    break;
                case SearchOperator.LessThanOrEqualTo:
                    durationOperatorSign = "<=";
                    break;
            }

            return durationOperatorSign;
        }

        private ExecutionContext GetExecutionContextFromReader(SqlDataReader reader, Boolean includeContextData, Boolean includeExecutionContextExtendedData)
        {
            ExecutionContext executionContext = new ExecutionContext();
            CallerContext callerContext = new CallerContext();
            SecurityContext securityContext = new SecurityContext();
            CallDataContext callDataContext = new CallDataContext();

            if (includeContextData)
            {
                #region Read Caller Context Data

                if (reader["ActivityId"] != null)
                {
                    Guid activityId = Guid.Empty;
                    Guid.TryParse(reader["ActivityId"].ToString(), out activityId);
                    callerContext.ActivityId = activityId;
                }

                if (reader["ApplicationId"] != null)
                {
                    callerContext.Application =
                        (MDMCenterApplication) ValueTypeHelper.Int32TryParse(reader["ApplicationId"].ToString(), 0);
                }

                if (reader["ModuleId"] != null)
                {
                    callerContext.Module =
                        (MDMCenterModules) ValueTypeHelper.Int32TryParse(reader["ModuleId"].ToString(), 0);
                }

                if (reader["ProgramName"] != null)
                {
                    callerContext.ProgramName = reader["ProgramName"].ToString();
                }

                if (reader["MDMSourceId"] != null)
                {
                    callerContext.MDMSource =
                        (EventSource) ValueTypeHelper.Int32TryParse(reader["MDMSourceId"].ToString(), 0);
                }

                if (reader["MDMSubscriberId"] != null)
                {
                    callerContext.MDMSubscriber =
                        (EventSubscriber) ValueTypeHelper.Int32TryParse(reader["MDMSubscriberId"].ToString(), 0);
                }

                if (reader["MDMPublisherId"] != null && reader["MDMPublisherId"] != DBNull.Value)
                {
                    callerContext.MDMPublisher =
                        (MDMPublisher) ValueTypeHelper.Int32TryParse(reader["MDMPublisherId"].ToString(), 0);
                }

                if (reader["JobId"] != null)
                {
                    callerContext.JobId = ValueTypeHelper.Int32TryParse(reader["JobId"].ToString(), 0);
                }

                if (reader["ProfileId"] != null)
                {
                    callerContext.ProfileId = ValueTypeHelper.Int32TryParse(reader["ProfileId"].ToString(), 0);
                }

                if (reader["ProfileName"] != null)
                {
                    callerContext.ProfileName = reader["ProfileName"].ToString();
                }

                if (reader["ServerId"] != null)
                {
                    callerContext.ServerId = ValueTypeHelper.Int32TryParse(reader["ServerId"].ToString(), 0);
                }

                if (reader["ServerName"] != null)
                {
                    callerContext.ServerName = reader["ServerName"].ToString();
                }

                if (reader["OperationId"] != null)
                {
                    Guid operationId = Guid.Empty;
                    Guid.TryParse(reader["OperationId"].ToString(), out operationId);
                    callerContext.OperationId = operationId;
                }

                #endregion

                #region Read Security Context Data

                if (reader["UserId"] != null)
                {
                    securityContext.UserId = ValueTypeHelper.Int32TryParse(reader["UserId"].ToString(), 0);
                }

                if (reader["UserName"] != null)
                {
                    securityContext.UserLoginName = reader["UserName"].ToString();
                }

                if (reader["UserRoleId"] != null)
                {
                    securityContext.UserRoleId = ValueTypeHelper.Int32TryParse(reader["UserRoleId"].ToString(), 0);
                }

                if (reader["UserRoleName"] != null)
                {
                    securityContext.UserRoleName = reader["UserRoleName"].ToString();
                }

                #endregion

                #region Call Data Context

                if (reader["OrganizationIdList"] != null)
                {
                    callDataContext.OrganizationIdList =
                        ValueTypeHelper.SplitStringToIntCollection(reader["OrganizationIdList"].ToString(), ',');
                }

                if (reader["ContainerIdList"] != null)
                {
                    callDataContext.ContainerIdList =
                        ValueTypeHelper.SplitStringToIntCollection(reader["ContainerIdList"].ToString(), ',');
                }

                if (reader["EntityTypeIdList"] != null)
                {
                    callDataContext.EntityTypeIdList =
                        ValueTypeHelper.SplitStringToIntCollection(reader["EntityTypeIdList"].ToString(), ',');
                }

                if (reader["RelationshipTypeIdList"] != null)
                {
                    callDataContext.RelationshipTypeIdList =
                        ValueTypeHelper.SplitStringToIntCollection(reader["RelationshipTypeIdList"].ToString(), ',');
                }

                if (reader["CategoryIdList"] != null)
                {
                    callDataContext.CategoryIdList =
                        ValueTypeHelper.SplitStringToLongCollection(reader["CategoryIdList"].ToString(), ',');
                }

                if (reader["AttributeIdList"] != null)
                {
                    callDataContext.AttributeIdList =
                        ValueTypeHelper.SplitStringToIntCollection(reader["AttributeIdList"].ToString(), ',');
                }

                if (reader["LocaleList"] != null)
                {
                    callDataContext.LocaleList =
                        ValueTypeHelper.SplitStringToLocaleEnumCollection(reader["LocaleList"].ToString(), ',');
                }

                if (reader["LookupTableNameList"] != null)
                {
                    callDataContext.LookupTableNameList =
                        ValueTypeHelper.SplitStringToStringCollection(reader["LookupTableNameList"].ToString(), ',');
                }

                if (reader["EntityIdList"] != null)
                {
                    callDataContext.EntityIdList =
                        ValueTypeHelper.SplitStringToLongCollection(reader["EntityIdList"].ToString(), ',');
                }

                #endregion

                if (reader["LegacyTraceSource"] != null)
                {
                    executionContext.LegacyMDMTraceSources = ValueTypeHelper.SplitStringToMDMTraceSourcesEnumCollection(reader["LegacyTraceSource"].ToString(), ',');
                }
            }

            if (includeExecutionContextExtendedData)
            {
                var additionalContext = reader["AdditionalContextData"];
                if (additionalContext != null && additionalContext != DBNull.Value)
                {
                    executionContext.AdditionalContextData = additionalContext.ToString();
                }
            }

            executionContext.CallerContext = callerContext;
            executionContext.SecurityContext = securityContext;
            executionContext.CallDataContext = callDataContext;

            return executionContext;
        }

        #endregion

        #endregion
    }
}