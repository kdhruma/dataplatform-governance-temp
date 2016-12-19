using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace MDM.DiagnosticManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Data Access class for Diagnostic Information
    /// </summary>
    public class DiagnosticDA : SqlClientDataAccessBase
    {
        #region Fields

        private TraceSettings _traceSettings = new TraceSettings();

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves the application related diagnostic information
        /// </summary>
        /// <param name="applicationDiagnosticType">Indicates the application diagnostic type</param>
        /// <param name="startDateTime">Indicates start date time to get information from datetime</param>
        /// <param name="entityId">Indicates the Id of an entity</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="command">Indicates the connection details</param>
        /// <returns>Returns JSON String</returns>
        public JObject GetApplicationDiagnostic(ApplicationDiagnosticType applicationDiagnosticType, DateTime startDateTime, Int64 entityId, Int64 count, DBCommandProperties command)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DiagnosticDA.GetApplicationDiagnostic", MDMTraceSource.Application, false);

            JObject applicationDiagnostic = new JObject();
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DiagnosticManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("DiagnosticManager_MDMCenter_ApplicationDashBoard_Get_ParametersArray");

                parameters[0].Value = applicationDiagnosticType.GetDescription();
                parameters[1].Value = startDateTime;
                parameters[2].Value = entityId;
                parameters[3].Value = count;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, "usp_MDMCenter_ApplicationDashBoard_Get");

                if (reader != null)
                {
                    applicationDiagnostic = LoadDiagnostic(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DiagnosticDA.GetApplicationDiagnostic", MDMTraceSource.Application);
            }

            return applicationDiagnostic;
        }

        /// <summary>
        /// Retrieves the system related diagnostic information
        /// </summary>
        /// <param name="systemDiagnosticType">Indicates the system diagnostic type</param>
        /// <param name="systemDiagnosticSubType">Indicates the system diagnostic sub type</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="command">Indicates the DB command properties information</param>
        /// <returns>Returns JSON String</returns>
        public JObject GetSystemDiagnostic(SystemDiagnosticType systemDiagnosticType, SystemDiagnosticSubType systemDiagnosticSubType, Int64 count, DBCommandProperties command)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DiagnosticDA.GetSystemDiagnostic", MDMTraceSource.Application, false);

            JObject systemDiagnostic = new JObject();
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DiagnosticManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("DiagnosticManager_MDMCenter_SystemDashBoard_Get_ParametersArray");

                parameters[0].Value = systemDiagnosticType.ToString();
                parameters[1].Value = systemDiagnosticSubType.GetDescription();
                parameters[2].Value = count;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, "usp_MDMCenter_SystemDashBoard_Get");

                if (reader != null)
                {
                    systemDiagnostic = LoadDiagnostic(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DiagnosticDA.GetSystemDiagnostic", MDMTraceSource.Application);
            }

            return systemDiagnostic;
        }

        /// <summary>
        /// Retrieves the related diagnostic record data based on context.
        /// </summary>
        /// <param name="relativeDataReferanceId">Indicates relative data reference id for diagnostic record.</param>
        /// <param name="diagnosticRelativeDataType">Indicates relative data type for diagnostic record.</param>
        /// <param name="command">Indicates the DB command properties information</param>
        /// <returns>returns related diagnostic record data as string</returns>
        public String GetRelatedDiagnosticRecordData(Int64 relativeDataReferanceId, DiagnosticRelativeDataType diagnosticRelativeDataType, DBCommandProperties command)
        {
            String relatedDiagnosticRecordData = String.Empty;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("DiagnosticManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("DiagnosticManager_RelatedDiagnosticRecordData_Get_ParametersArray");

                parameters[0].Value = relativeDataReferanceId;
                parameters[1].Value = (Int32)diagnosticRelativeDataType;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, "usp_InstrumentationManager_RelatedDiagnosticData_Get");

                if (reader != null)
                {
                    if (diagnosticRelativeDataType == DiagnosticRelativeDataType.MessageData)
                    {
                        relatedDiagnosticRecordData = GetMessageDataFromReader(reader);
                    }
                    else if (diagnosticRelativeDataType == DiagnosticRelativeDataType.ContextData)
                    {
                        relatedDiagnosticRecordData = GetContextDataFromReader(reader);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return relatedDiagnosticRecordData;
        }

        /// <summary>
        /// get report result set from db for diagnostic report tools 
        /// based on report type
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="inputXml"></param>
        /// <param name="currentUser"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ProcessDiagnosticToolsReport(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, String inputXml, String currentUser, CallerContext callerContext, DBCommandProperties command)
        {
            #region tracing initialization

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            #endregion tracing initialization

            #region initialization

            DataSet reportResultDataSet = new DataSet();

            #endregion intialization

            try
            {
                #region making db call

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogVerbose(String.Format("DA receives report type : {0}" , reportType.ToString()));
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("DiagnosticManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("InstrumentationManager_DiagnosticTool_Report_Get_ParametersArray");
                parameters[0].Value = reportType.ToString();
                parameters[1].Value = String.Empty;
                parameters[2].Value = inputXml;
                parameters[3].Value = currentUser;
                parameters[4].Value = "Test"; //program name
                parameters[5].Value = 0; //debug on/off

                reportResultDataSet = ExecuteDataSet(command.ConnectionString, parameters, "usp_InstrumentationManager_DiagnosticTool_Report_Get");
                #endregion making db call
            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }

            return reportResultDataSet;
        }

        /// <summary>
        /// Get default input xml from tb_profiles when requested by UI
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="callerContext"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet GetReportTemplate(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, CallerContext callerContext, DBCommandProperties command)
        {
            #region tracing initialization

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            #endregion tracing initialization

            #region initialization

            DataSet reportTemplateDataSet = new DataSet();

            #endregion intialization

            try
            {
                #region making db call

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogVerbose(String.Format("DA receives report type : {0}", reportType.ToString()));
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("DiagnosticManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("InstrumentationManager_DiagnosticTool_Report_Template_Get_ParametersArray");
                parameters[0].Value = reportType.ToString();
                parameters[1].Value = String.Empty;
                parameters[2].Value = 0; //debug on/off

                reportTemplateDataSet = ExecuteDataSet(command.ConnectionString, parameters, "usp_InstrumentationManager_DiagnosticTool_Report_Template_Get");

                #endregion making db call

            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }

            return reportTemplateDataSet;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private List<String> GetColumnNameListFromReader(SqlDataReader dataReader)
        {
            var columns = new List<String> { "id" };

            for (Int32 i = 0; i < dataReader.FieldCount; i++)
            {
                columns.Add(dataReader.GetName(i));
            }
            return columns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private JObject LoadDiagnostic(SqlDataReader reader)
        {
            JObject diagnostic = new JObject();

            if (reader != null)
            {
                List<String> columnNames = GetColumnNameListFromReader(reader);

                if (columnNames != null && columnNames.Count > 0)
                {
                    JProperty jProperty = new JProperty("Columns",
                        new JArray(
                            from column in columnNames
                            select new JObject(
                                new JProperty("name", column.Trim()),
                                new JProperty("caption", column))));

                    diagnostic.Add(new JProperty("Config", new JObject(jProperty)));
                }

                Int32 id = 0;
                JArray jarray = new JArray();

                while (reader.Read())
                {
                    id++;
                    List<String> data = new List<String>();
                    JObject jObject = new JObject();

                    foreach (String column in columnNames)
                    {
                        if (column != "id")
                        {
                            data.Add(reader[column].ToString());
                        }
                    }

                    jObject.Add(new JProperty("id", id));
                    jObject.Add(new JProperty("cell", data));

                    jarray.Add(jObject);
                }

                diagnostic.Add(new JProperty("Data", jarray));
            }

            return diagnostic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private String GetMessageDataFromReader(SqlDataReader reader)
        {
            String relatedDiagnosticRecordData = String.Empty;

            while (reader.Read())
            {
                if (reader["MessageData"] != null && reader["MessageData"] != DBNull.Value)
                {
                    Object dataXmlObj = reader["MessageData"];
                    var dataXmlAsByteArray = dataXmlObj as Byte[];
                    relatedDiagnosticRecordData = ProtoBufSerializationHelper.Deserialize<String>(dataXmlAsByteArray);
                }
            }

            return relatedDiagnosticRecordData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private String GetContextDataFromReader(SqlDataReader reader)
        {
            ExecutionContext executionContext = new ExecutionContext();
            CallerContext callerContext = new CallerContext();
            SecurityContext securityContext = new SecurityContext();
            CallDataContext callDataContext = new CallDataContext();

            while (reader.Read())
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
                    callerContext.Application = (MDMCenterApplication)ValueTypeHelper.Int32TryParse(reader["ApplicationId"].ToString(), 0);
                }

                if (reader["ModuleId"] != null)
                {
                    callerContext.Module = (MDMCenterModules)ValueTypeHelper.Int32TryParse(reader["ModuleId"].ToString(), 0);
                }

                if (reader["ProgramName"] != null)
                {
                    callerContext.ProgramName = reader["ProgramName"].ToString();
                }

                if (reader["MDMSourceId"] != null)
                {
                    callerContext.MDMSource = (EventSource)ValueTypeHelper.Int32TryParse(reader["MDMSourceId"].ToString(), 0);
                }

                if (reader["MDMSubscriberId"] != null)
                {
                    callerContext.MDMSubscriber = (EventSubscriber)ValueTypeHelper.Int32TryParse(reader["MDMSubscriberId"].ToString(), 0);
                }

                if (reader["MDMPublisherId"] != null && reader["MDMPublisherId"] != DBNull.Value)
                {
                    callerContext.MDMPublisher = (MDMPublisher)ValueTypeHelper.Int32TryParse(reader["MDMPublisherId"].ToString(), 0);
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
                    callDataContext.OrganizationIdList = ValueTypeHelper.SplitStringToIntCollection(reader["OrganizationIdList"].ToString(), ',');
                }

                if (reader["ContainerIdList"] != null)
                {
                    callDataContext.ContainerIdList = ValueTypeHelper.SplitStringToIntCollection(reader["ContainerIdList"].ToString(), ',');
                }

                if (reader["EntityTypeIdList"] != null)
                {
                    callDataContext.EntityTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader["EntityTypeIdList"].ToString(), ',');
                }

                if (reader["RelationshipTypeIdList"] != null)
                {
                    callDataContext.RelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader["RelationshipTypeIdList"].ToString(), ',');
                }

                if (reader["CategoryIdList"] != null)
                {
                    callDataContext.CategoryIdList = ValueTypeHelper.SplitStringToLongCollection(reader["CategoryIdList"].ToString(), ',');
                }

                if (reader["AttributeIdList"] != null)
                {
                    callDataContext.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader["AttributeIdList"].ToString(), ',');
                }

                if (reader["LocaleList"] != null)
                {
                    callDataContext.LocaleList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader["LocaleList"].ToString(), ',');
                }

                if (reader["LookupTableNameList"] != null)
                {
                    callDataContext.LookupTableNameList = ValueTypeHelper.SplitStringToStringCollection(reader["LookupTableNameList"].ToString(), ',');
                }

                if (reader["EntityIdList"] != null)
                {
                    callDataContext.EntityIdList = ValueTypeHelper.SplitStringToLongCollection(reader["EntityIdList"].ToString(), ',');
                }

                #endregion

                if (reader["LegacyTraceSource"] != null)
                {
                    executionContext.LegacyMDMTraceSources = ValueTypeHelper.SplitStringToMDMTraceSourcesEnumCollection(reader["LegacyTraceSource"].ToString(), ',');
                }

                if (reader["AdditionalContextData"] != null)
                {
                    executionContext.AdditionalContextData = reader["AdditionalContextData"].ToString();
                }
            }

            executionContext.CallerContext = callerContext;
            executionContext.SecurityContext = securityContext;
            executionContext.CallDataContext = callDataContext;

            return executionContext.ToXml();
        }

        #endregion

        #endregion

    }
}