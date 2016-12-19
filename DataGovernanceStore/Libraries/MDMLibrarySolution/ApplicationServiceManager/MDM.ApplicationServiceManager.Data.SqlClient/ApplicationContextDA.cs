using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace MDM.ApplicationServiceManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// This is ApplicationContext Data manager
    /// </summary>
    public class ApplicationContextDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// Indicates the diagnostic activity
        /// </summary>
        private DiagnosticActivity _diagnosticActivity = null;

        /// <summary>
        /// Indicates the current trace setting 
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates whether tracing is enabled or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        /// <summary>
        /// Indicates the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting Category path separator.
        /// </summary>
        private String _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " >> ");

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ApplicationContextDA()
        {
            GetSecurityPrincipal();

            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            _diagnosticActivity = new DiagnosticActivity();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationContextId"></param>
        /// <param name="applicationContextType"></param>
        /// <returns></returns>
        public Collection<ApplicationContext> GetApplicationContext(Int32 applicationContextId, Int32 applicationContextType)
        {
            SqlDataReader reader = null;
            Collection<ApplicationContext> returnValue = new Collection<ApplicationContext>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            StringBuilder spReturn = new StringBuilder();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_Get_ParametersArray");

                parameters[0].Value = applicationContextId;
                parameters[1].Value = applicationContextType;

                storedProcedureName = "usp_ApplicationServiceManager_ApplicationContext_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    spReturn.Append(reader[0]);
                }
                returnValue = ParseCollection(spReturn.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnValue;
        }

        private Collection<ApplicationContext> ParseCollection(String valuesAsXml)
        {
            Collection<ApplicationContext> contexts = new Collection<ApplicationContext>();
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                    {
                        ApplicationContext context = new ApplicationContext(reader.ReadOuterXml());
                        contexts.Add(context);
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return contexts;
        }

        /// <summary>
        /// Get the application context by application context id
        /// </summary>
        /// <param name="applicationContextId">Indicates the applicationcontextId</param>
        /// <param name="command">Indicates the DB command properties</param>
        /// <returns>Returns the application context as result</returns>
        public ApplicationContextCollection Get(Int32 applicationContextId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            ApplicationContextCollection applicationContexts = new ApplicationContextCollection();

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_Get_ParametersArray");

                parameters[0].Value = applicationContextId;

                storedProcedureName = "usp_ApplicationManager_Application_Context_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                PopulateApplicationContexts(reader, applicationContexts);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return applicationContexts;
        }

        /// <summary>
        /// Gets the best match object id for a given entity, application context type and object ids
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="appContextType"></param>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Int32 GetApplicationContextObjectId(Collection<Int32> objectIds, ApplicationContextType appContextType, Entity entity, DBCommandProperties command)
        {
            Int32 objectId = 0;
            SqlDataReader reader = null;

            try
            {
                var generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("ApplicationManager_Application_Context_GetByObjectId_ParametersArray");

                var preparedEntityIds = new List<SqlDataRecord>();
                foreach (Int32 item in objectIds)
                {
                    var preparedItem = new SqlDataRecord(
                        generator.GetTableValueMetadata("ApplicationManager_Application_Context_GetByObjectId_ParametersArray",
                        parameters[0].ParameterName));

                    preparedItem.SetValue(0, item);
                    preparedEntityIds.Add(preparedItem);
                }

                if (preparedEntityIds.Any())
                {
                    parameters[0].Value = preparedEntityIds;
                }

                parameters[1].Value = entity.OrganizationId;
                parameters[2].Value = entity.ContainerId;
                parameters[3].Value = 0; // Set EntityId to 0
                parameters[4].Value = 0; // Set Attributeid to 0
                parameters[5].Value = entity.EntityTypeId;
                parameters[6].Value = 0; // Set RelationshipId to 0
                parameters[7].Value = entity.CategoryId;
                parameters[8].Value = (Int32)appContextType;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, "usp_ApplicationManager_Application_Context_GetByObjectId");

                while (reader.Read())
                {
                    if (reader["FK_ObjectId"] != null)
                    {
                        objectId = ValueTypeHelper.Int32TryParse(reader["FK_ObjectId"].ToString(), objectId);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return objectId;
        }

        /// <summary>
        /// Process application context based on their actions
        /// </summary>
        /// <param name="applicationContexts">Indicates the application context</param>
        /// <param name="programName">Indicates the program Name</param>
        /// <param name="userName">Indicates the user name</param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="command">Indicates the db command properties</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResultCollection Process(ApplicationContextCollection applicationContexts, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {

            SqlDataReader reader = null;
            OperationResultCollection applicationContextOperationResults = new OperationResultCollection();

            if (isTracingEnabled)
            {
                _diagnosticActivity.Start();
            }


            SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

            const String storedProcedureName = "usp_ApplicationManager_Application_Context_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    List<SqlDataRecord> applicationContextTable;

                    SqlParameter[] parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_Process_ParametersArray");

                    SqlMetaData[] applicationContextMetaData = generator.GetTableValueMetadata("ApplicationServiceManager_ApplicationContext_Process_ParametersArray", parameters[0].ParameterName);

                    CreateApplicationContextTables(applicationContexts, applicationContextMetaData, out applicationContextTable);

                    if (isTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation("Completed preparing TVP from application contexts.");
                        DiagnosticsUtility.LogSqlTVPInformation(_diagnosticActivity, storedProcedureName, "ApplicationContextTable", applicationContextMetaData, applicationContextTable);
                    }

                    parameters[0].Value = applicationContextTable;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;
                    parameters[3].Value = true; //ReturnResult = true

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResultForContext(reader, applicationContexts, applicationContextOperationResults);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    if (isTracingEnabled)
                    {
                        _diagnosticActivity.LogError(exception.Message);
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
                        _diagnosticActivity.Stop();
                    }
                }
            }

            return applicationContextOperationResults;
        }


        /// <summary>
        /// Process application context based on their actions
        /// </summary>
        /// <param name="applicationContextObjectMappings">Indicates the application context</param>
        /// <param name="programName">Indicates the program Name</param>
        /// <param name="userName">Indicates the user name</param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="command">Indicates the db command properties</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResultCollection ProcessObjectMapping(ApplicationContextObjectMappingCollection applicationContextObjectMappings, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            OperationResultCollection applicationContextObjectMappingOperationResults = new OperationResultCollection();

            if (isTracingEnabled)
            {
                _diagnosticActivity.Start();
            }

            SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

            const String storedProcedureName = "usp_ApplicationManager_Application_Context_Object_Map_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> applicationContextObjectMappingTable;

                    SqlParameter[] parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContextObjectMapping_Process_ParametersArray");

                    SqlMetaData[] applicationContextObjectMappingMetaData = generator.GetTableValueMetadata("ApplicationServiceManager_ApplicationContextObjectMapping_Process_ParametersArray", parameters[0].ParameterName);

                    CreateApplicationContextObjectMappingTables(applicationContextObjectMappings, applicationContextObjectMappingMetaData, out applicationContextObjectMappingTable);

                    if (isTracingEnabled)
                    {
                        _diagnosticActivity.LogInformation("Completed preparing TVP from application context object mappings.");
                        DiagnosticsUtility.LogSqlTVPInformation(_diagnosticActivity, storedProcedureName, "ApplicationContextObjectMappigTable", applicationContextObjectMappingMetaData, applicationContextObjectMappingTable);
                    }

                    parameters[0].Value = applicationContextObjectMappingTable;
                    parameters[1].Value = userName;
                    parameters[2].Value = programName;
                    parameters[3].Value = true; //ReturnResult = true

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResultForContextObjectMapping(reader, applicationContextObjectMappings, applicationContextObjectMappingOperationResults);

                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    if (isTracingEnabled)
                    {
                        _diagnosticActivity.LogError(exception.Message);
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
                        _diagnosticActivity.Stop();
                    }
                }
            }

            return applicationContextObjectMappingOperationResults;
        }

        /// <summary>
        /// returns the applicationContextId
        /// </summary>
        /// <param name="applicationContext">Application context</param>
        /// <param name="getExactMatch">Whether want exact match or not</param>
        /// <returns>returns the applicationContextId</returns>
        public Int32 GetApplicationContextId(ApplicationContext applicationContext, Boolean getExactMatch)
        {
            SqlDataReader reader = null;
            Int32 applicationContextId = 0;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;


            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_Id_Get_ParametersArray");


                parameters[0].Value = applicationContext.OrganizationId;
                parameters[1].Value = applicationContext.ContainerId;
                parameters[2].Value = applicationContext.EntityId;
                parameters[3].Value = applicationContext.AttributeId;
                parameters[4].Value = applicationContext.EntityTypeId;
                parameters[5].Value = applicationContext.RelationshipTypeId;
                parameters[6].Value = applicationContext.CategoryId;
                parameters[7].Value = (int)ApplicationContextType.DA;// ApplicationContextType for Dependent Attributes
                parameters[8].Value = getExactMatch;

                storedProcedureName = "usp_ApplicationManager_Application_Context_Id_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {

                    applicationContextId = ValueTypeHelper.Int32TryParse(reader[0].ToString(), 0);
                }

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return applicationContextId;
        }

        /// <summary>
        /// Gets the application context ids from application context object
        /// </summary>
        /// <param name="applicationContexts">Indicates the application contexts</param>
        /// <param name="matchContext">Indicates the match context</param>
        /// <param name="command">Indicates the Data base command properties</param>
        /// <returns>Indicates the list of application context Ids</returns>
        public Dictionary<Int64, Collection<Int32>> GetApplicationContextIds(ApplicationContextCollection applicationContexts, ApplicationContextMatchType matchContext, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            Dictionary<Int64,Collection<Int32>> applicationContextIds = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            List<SqlDataRecord> applicationContextTable;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");
                parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_GetApplicationContextIds_ParametersArray");

                SqlMetaData[] applicationContextMetaData = generator.GetTableValueMetadata("ApplicationServiceManager_ApplicationContext_GetApplicationContextIds_ParametersArray", parameters[0].ParameterName);
                CreateApplicationContextTables(applicationContexts, applicationContextMetaData, out applicationContextTable);

                parameters[0].Value = applicationContextTable;
                parameters[1].Value = (Int32)matchContext;

                storedProcedureName = "usp_ApplicationManager_Application_Context_ContextId_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (!(reader == null || reader.IsClosed))
                {
                    applicationContextIds = new Dictionary<Int64,Collection<Int32>>();

                    while (reader.Read())
                    {
                        Int32 applicationContextId = -1;
                        Int64 referenceId = -1;

                        if (reader["ApplicationContextId"] != null)
                        {
                          applicationContextId=ValueTypeHelper.Int32TryParse(reader["ApplicationContextId"].ToString(), 0);
                        }

                        if (reader["ReferenceId"] != null)
                        {
                            referenceId = ValueTypeHelper.Int64TryParse(reader["ReferenceId"].ToString(), 0);
                        }

                        if (applicationContextIds.ContainsKey(referenceId))
                        {
                            var existingIds = applicationContextIds[referenceId];

                            if (existingIds == null || existingIds.Count == 0)
                            {
                                applicationContextIds.Add(referenceId, new Collection<Int32>() { applicationContextId });
                            }
                            else if (existingIds.Contains(applicationContextId) == false)
                            {
                                existingIds.Add(applicationContextId);
                                applicationContextIds[referenceId] = existingIds;
                            }
                        }
                        else
                        {
                            applicationContextIds.Add(referenceId, new Collection<Int32>() { applicationContextId });
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return applicationContextIds;
        }

        /// <summary>
        /// Get application context object mappings based on given object type and match context
        /// </summary>
        /// <param name="applicationContexts">Indicates collection of application context.</param>
        /// <param name="objectTypeId">Indicates object type identifier.</param>
        /// <param name="matchContext">Indicates match context.</param>
        /// <param name="command">Indicates the Data base command properties</param>
        /// <returns>Returns application context object mapping based on given object type and match context.</returns>
        public ApplicationContextObjectMappingCollection GetApplicationContextObjectMappings(ApplicationContextCollection applicationContexts, Int32 objectTypeId, ApplicationContextMatchType matchContext, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            List<SqlDataRecord> applicationContextTable;
            ApplicationContextObjectMappingCollection applicationContextObjectMappings = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");
                parameters = generator.GetParameters("ApplicationServiceManager_ApplicationContext_GetApplicationContextIds_ParametersArray");

                SqlMetaData[] applicationContextMetaData = generator.GetTableValueMetadata("ApplicationServiceManager_ApplicationContext_GetApplicationContextIds_ParametersArray", parameters[0].ParameterName);
                CreateApplicationContextTables(applicationContexts, applicationContextMetaData, out applicationContextTable);

                parameters[0].Value = applicationContextTable;
                parameters[1].Value = (Int32)matchContext;
                parameters[2].Value = objectTypeId;

                storedProcedureName = "usp_ApplicationManager_Application_Context_ContextId_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (!(reader == null || reader.IsClosed))
                {
                    applicationContextObjectMappings = new ApplicationContextObjectMappingCollection();
                    PopulateApplicationContextObjectMappings(reader, applicationContextObjectMappings);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return applicationContextObjectMappings;
        }

        #endregion

        #region Private methods

        private void PopulateOperationResultForContext(SqlDataReader reader, ApplicationContextCollection applicationContexts, OperationResultCollection applicationContextOperationResults)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                Int32 applicationContextId = -1;
                Int32 referenceId = -1;
                ApplicationContext applicationContext = null;

                OperationResult applicationContextOperationResult = new OperationResult();

                if (reader["Id"] != null)
                {
                    referenceId = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), -1);
                }

                if (reader["ApplicationContextId"] != null)
                {
                    applicationContextId = ValueTypeHelper.Int32TryParse(reader["ApplicationContextId"].ToString(), -1);
                }

                if (reader["IsError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["IsError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                applicationContext = applicationContexts.GetApplicationContext(referenceId) as ApplicationContext;

                if (applicationContext != null)
                {
                    applicationContext.Id = applicationContextId;
                    applicationContextOperationResult.Id = applicationContextId;
                    applicationContextOperationResult.PerformedAction = applicationContext.Action;

                    applicationContextOperationResult.ReferenceId = applicationContext.Name;

                    if (hasError & !String.IsNullOrEmpty(errorCode))
                    {
                        applicationContextOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                    }
                    else
                    {
                        applicationContextOperationResult.ReturnValues.Add(applicationContextId);
                        applicationContextOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }

                applicationContextOperationResults.Add(applicationContextOperationResult);
            }
        }

        private void PopulateOperationResultForContextObjectMapping(SqlDataReader reader, ApplicationContextObjectMappingCollection applicationContextObjectMappings, OperationResultCollection applicationContextOperationResults)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                Int32 applicationContextId = -1;
                String referenceId = String.Empty;
                ApplicationContextObjectMapping applicationContextObjectMapping = null;

                OperationResult applicationContextOperationResult = new OperationResult();

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                if (reader["FK_Application_Context_Map"] != null)
                {
                    applicationContextId = ValueTypeHelper.Int32TryParse(reader["FK_Application_Context_Map"].ToString(), -1);
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                applicationContextObjectMapping = applicationContextObjectMappings.GetByReferenceId(referenceId) as ApplicationContextObjectMapping;

                if (applicationContextObjectMapping != null)
                {
                    applicationContextObjectMapping.Id = applicationContextId;
                    applicationContextOperationResult.Id = applicationContextId;
                    applicationContextOperationResult.PerformedAction = applicationContextObjectMapping.Action;

                    applicationContextOperationResult.ReferenceId = referenceId;

                    if (hasError & !String.IsNullOrEmpty(errorCode))
                    {
                        if(String.Compare(errorCode, "-99") == 0)
                        {
                            applicationContextOperationResult.AddOperationResult("114550", String.Empty, OperationResultType.Error); // Unable to update or delete the Entity Variant Definition mapping, as it already has entity(s) generated for the mapping.
                        }
                        else
                        {
                            applicationContextOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                    }
                    else
                    {
                        applicationContextOperationResult.ReturnValues.Add(applicationContextId);
                        applicationContextOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }

                applicationContextOperationResults.Add(applicationContextOperationResult);
            }
        }

        private void CreateApplicationContextTables(ApplicationContextCollection applicationContexts, SqlMetaData[] applicationContextMetaData, out List<SqlDataRecord> applicationContextTable)
        {
            applicationContextTable = null;

            if (applicationContexts != null && applicationContexts.Count > 0)
            {
                applicationContextTable = new List<SqlDataRecord>();

                foreach (ApplicationContext context in applicationContexts)
                {
                    SqlDataRecord contextRecord = new SqlDataRecord(applicationContextMetaData);
                    contextRecord.SetInt32(0, context.Id);
                    contextRecord.SetInt64(1, context.ReferenceId);
                    contextRecord.SetInt32(2, context.ObjectTypeId);
                    contextRecord.SetInt32(3, 0);//TODO :: check what should be ObjectId
                    contextRecord.SetInt32(4, (Int32)context.ContextType);
                    contextRecord.SetString(5, context.Name);
                    contextRecord.SetString(6, context.LongName);
                    contextRecord.SetInt32(7, context.OrganizationId);
                    contextRecord.SetInt32(8, context.ContainerId);
                    contextRecord.SetInt64(9, context.CategoryId);
                    contextRecord.SetInt32(10, context.EntityTypeId);
                    contextRecord.SetInt32(11, context.AttributeId);
                    contextRecord.SetInt32(12, context.RelationshipTypeId);
                    contextRecord.SetInt32(13, context.RoleId);
                    contextRecord.SetInt32(14, context.UserId);
                    contextRecord.SetString(15, context.Action.ToString());

                    applicationContextTable.Add(contextRecord);
                }
            }
        }

        private void CreateApplicationContextObjectMappingTables(ApplicationContextObjectMappingCollection applicationContextObjectMappings, SqlMetaData[] applicationContextObjectMappingMetaData, out List<SqlDataRecord> applicationContextObjectMappingTable)
        {
            applicationContextObjectMappingTable = null;

            if (applicationContextObjectMappings != null && applicationContextObjectMappings.Count > 0)
            {
                applicationContextObjectMappingTable = new List<SqlDataRecord>();

                foreach (ApplicationContextObjectMapping contextObjectMapping in applicationContextObjectMappings)
                {
                    SqlDataRecord contextObjectMappingRecord = new SqlDataRecord(applicationContextObjectMappingMetaData);
                    contextObjectMappingRecord.SetValue(0, ValueTypeHelper.Int32TryParse(contextObjectMapping.ReferenceId, 0));
                    contextObjectMappingRecord.SetValue(1, contextObjectMapping.Id);
                    contextObjectMappingRecord.SetValue(2, contextObjectMapping.ApplicationContextId);
                    contextObjectMappingRecord.SetValue(3, (Int32)contextObjectMapping.ApplicationContextType);
                    contextObjectMappingRecord.SetValue(4, contextObjectMapping.ContextObjectTypeId);
                    contextObjectMappingRecord.SetValue(5, contextObjectMapping.ObjectId);
                    contextObjectMappingRecord.SetValue(6, contextObjectMapping.Action.ToString());

                    applicationContextObjectMappingTable.Add(contextObjectMappingRecord);
                }
            }
        }

        private void PopulateApplicationContexts(SqlDataReader reader, ApplicationContextCollection applicationContexts)
        {
            while (reader.Read())
            {
                Int32 objectTypeId=0;

                if (reader["ObjectType"] != null)
                {
                    objectTypeId = ValueTypeHelper.Int32TryParse(reader["ObjectType"].ToString(), objectTypeId);
                }

                ApplicationContext applicationContext = new ApplicationContext(objectTypeId);


                if (reader["PK_Application_Context"] != null)
                {
                    applicationContext.Id = ValueTypeHelper.Int32TryParse(reader["PK_Application_Context"].ToString(), applicationContext.Id);
                }

                if (reader["ShortName"] != null)
                {
                    applicationContext.Name = reader["ShortName"].ToString();
                }

                if (reader["LongName"] != null)
                {
                    applicationContext.LongName = reader["LongName"].ToString();
                }

                if (reader["OrgId"] != null)
                {
                    applicationContext.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrgId"].ToString(), applicationContext.OrganizationId);
                }

                if (reader["OrgShortName"] != null)
                {
                    applicationContext.OrganizationName = reader["OrgShortName"].ToString();
                }

                if (reader["OrgLongName"] != null)
                {
                    applicationContext.OrganizationLongName = reader["OrgLongName"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    applicationContext.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), applicationContext.ContainerId);
                }

                if (reader["ContainerShortName"] != null)
                {
                    applicationContext.ContainerName = reader["ContainerShortName"].ToString();
                }

                if (reader["ContainerLongName"] != null)
                {
                    applicationContext.ContainerLongName = reader["ContainerLongName"].ToString();
                }

                if (reader["CategoryId"] != null)
                {
                    applicationContext.CategoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), applicationContext.CategoryId);
                }

                if (reader["CategoryShortName"] != null)
                {
                    applicationContext.CategoryName = reader["CategoryShortName"].ToString();
                }

                if (reader["CategoryLongName"] != null)
                {
                    applicationContext.CategoryLongName = reader["CategoryLongName"].ToString();
                }

                if (reader["CategoryPath"] != null)
                {
                    applicationContext.CategoryPath = reader["CategoryPath"].ToString().Replace(Constants.STRING_PATH_SEPARATOR, _categoryPathSeparator);
                }

                if (reader["EntityTypeId"] != null)
                {
                    applicationContext.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), applicationContext.EntityTypeId);
                }

                if (reader["EntityTypeShortName"] != null)
                {
                    applicationContext.EntityTypeName = reader["EntityTypeShortName"].ToString();
                }

                if (reader["EntityTypeLongName"] != null)
                {
                    applicationContext.EntityTypeLongName = reader["EntityTypeLongName"].ToString();
                }

                if (reader["AttributeId"] != null)
                {
                    applicationContext.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), applicationContext.AttributeId);
                }

                if (reader["AttributeShortName"] != null)
                {
                    applicationContext.AttributeName = reader["AttributeShortName"].ToString();
                }

                if (reader["AttributeLongName"] != null)
                {
                    applicationContext.AttributeLongName = reader["AttributeLongName"].ToString();
                }

                if (reader["RelationshipTypeId"] != null)
                {
                    applicationContext.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["RelationshipTypeId"].ToString(), applicationContext.RelationshipTypeId);
                }

                if (reader["RelationshipTypeShortName"] != null)
                {
                    applicationContext.RelationshipTypeName = reader["RelationshipTypeShortName"].ToString();
                }

                if (reader["RelationshipTypeLongName"] != null)
                {
                    applicationContext.RelationshipTypeLongName = reader["RelationshipTypeLongName"].ToString();
                }

                if (reader["SecurityRoleId"] != null)
                {
                    applicationContext.RoleId = ValueTypeHelper.Int32TryParse(reader["SecurityRoleId"].ToString(), applicationContext.RoleId);
                }

                if (reader["SecurityRoleShortName"] != null)
                {
                    applicationContext.RoleName = reader["SecurityRoleShortName"].ToString();
                }

                if (reader["SecurityRoleLongName"] != null)
                {
                    applicationContext.RoleLongName = reader["SecurityRoleLongName"].ToString();
                }

                if (reader["SecurityUserId"] != null)
                {
                    applicationContext.UserId = ValueTypeHelper.Int32TryParse(reader["SecurityUserId"].ToString(), applicationContext.UserId);
                }

                if (reader["SecurityUserLoginName"] != null)
                {
                    applicationContext.UserName = reader["SecurityUserLoginName"].ToString();
                }

                applicationContexts.Add(applicationContext);
            }
        }

        private void PopulateApplicationContextObjectMappings(SqlDataReader reader, ApplicationContextObjectMappingCollection applicationContextObjectMappings)
        {

            while (reader.Read())
            {
                #region local variables

                ApplicationContextObjectMapping applicationContextObjectMapping = new ApplicationContextObjectMapping();

                Int32 Id = 0;
                Int32 applicationContextId = 0;
                ApplicationContextType applicationContextType = ApplicationContextType.CC;
                Int32 objectTypeId = 0;
                Int32 objectId = 0;

                #endregion local variables

                #region Read values

                if (reader["ApplicationContextObjectMapId"] != null)
                {
                    Id = ValueTypeHelper.Int32TryParse(reader["ApplicationContextObjectMapId"].ToString(), Id);
                }

                if (reader["ApplicationContextId"] != null)
                {
                    applicationContextId = ValueTypeHelper.Int32TryParse(reader["ApplicationContextId"].ToString(), applicationContextId);
                }

                if (reader["ApplicationContextTypeId"] != null)
                {
                    Enum.TryParse(reader["ApplicationContextTypeId"].ToString(), out applicationContextType);
                }

                if (reader["ObjectTypeId"] != null)
                {
                    objectTypeId = ValueTypeHelper.Int32TryParse(reader["ObjectTypeId"].ToString(), objectTypeId);
                }

                if (reader["ObjectId"] != null)
                {
                    objectId = ValueTypeHelper.Int32TryParse(reader["ObjectId"].ToString(), Id);
                }

                #endregion Read values

                #region Populate object

                applicationContextObjectMapping.Id = Id;
                applicationContextObjectMapping.ApplicationContextId = applicationContextId;
                applicationContextObjectMapping.ApplicationContextType = applicationContextType;
                applicationContextObjectMapping.ContextObjectTypeId = objectTypeId;
                applicationContextObjectMapping.ObjectId = objectId;

                applicationContextObjectMappings.Add(applicationContextObjectMapping);

                #endregion Populate object
            }
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion Private methods
    }
}
