using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.ConfigurationPrimitivesManager.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// ApplicationConfigurationItemDA data access layer
    /// </summary>
    public class ApplicationConfigurationItemDA : SqlClientDataAccessBase
    {
        #region Constants
        
        private const String ApplicationConfigurationItemGetAllProcessName = "Get";
        private const String ApplicationConfigurationItemCreateUpdateDeleteProcessName = "Process";

        private const String ApplicationConfigurationItemGetSPName = "usp_Application_Config_Raw_Get";
        private const String ApplicationConfigurationItemProcessSPName = "usp_AdminManager_ApplicationConfig_Process";

        private const String ApplicationConfigurationItemGetSPParameters = "ConfigurationManager_ApplicationConfiguration_Raw_Get_ParametersArray";
        private const String ApplicationConfigurationItemProcessSPParameters = "AdminManager_ApplicationConfig_Process_ParametersArray";
        
        private const String ConfigurationManagerSQLParametersGeneratorName = "ConfigurationManager_SqlParameters";
        private const String AdminManagerSQLParametersGeneratorName = "AdminManager_SqlParameters";

        private const String TracingPrefix = "MDM.ConfigurationManager.Data.SqlClient.ApplicationConfigurationItemDA.";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Get application configurations raw
        /// </summary>
        /// <param name="eventSource"></param>
        /// <param name="eventSubscriber"></param>
        /// <param name="command"></param>
        /// <returns>ApplicationConfigurationItemsCollection</returns>
        public ApplicationConfigurationItemCollection Get(EventSource eventSource, EventSubscriber eventSubscriber, DBCommandProperties command)
        {
            ApplicationConfigurationItemCollection result = new ApplicationConfigurationItemCollection();

            StartTraceActivity(ApplicationConfigurationItemGetAllProcessName);
            try
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator(ConfigurationManagerSQLParametersGeneratorName);

                    SqlParameter[] parameters = generator.GetParameters(ApplicationConfigurationItemGetSPParameters);

                    parameters[0].Value = (Int32) eventSource;
                    parameters[1].Value = (Int32) eventSubscriber;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, ApplicationConfigurationItemGetSPName);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ApplicationConfigurationItem item = new ApplicationConfigurationItem();
                            
                            #region Parse reader params to item

                            if (reader["PK_Application_Config"] != null)
                                item.Id = ValueTypeHelper.Int32TryParse(reader["PK_Application_Config"].ToString(), 0);

                            if (reader["FK_Application_ConfigParent"] != null)
                                item.ConfigParentId = ValueTypeHelper.Int32TryParse(reader["FK_Application_ConfigParent"].ToString(), 0);

                            if (reader["FK_Application_ContextDefinition"] != null)
                                item.ContextDefinitionId = ValueTypeHelper.ConvertToNullableInt32(reader["FK_Application_ContextDefinition"].ToString());

                            if (reader["ShortName"] != null)
                                item.Name = reader["ShortName"].ToString();

                            if (reader["LongName"] != null)
                                item.LongName = reader["LongName"].ToString();

                            if (reader["FK_Org"] != null)
                                item.OrganizationId = ValueTypeHelper.Int32TryParse(reader["FK_Org"].ToString(), 0);

                            if (reader["FK_Catalog"] != null)
                                item.ContainerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                            if (reader["FK_Category"] != null)
                                item.CategoryId = ValueTypeHelper.Int64TryParse(reader["FK_Category"].ToString(), 0);

                            if (reader["FK_CNode"] != null)
                                item.EntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);

                            if (reader["FK_Attribute"] != null)
                                item.AttributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), 0);

                            if (reader["FK_NodeType"] != null)
                                item.NodeTypeId = ValueTypeHelper.Int32TryParse(reader["FK_NodeType"].ToString(), 0);

                            if (reader["FK_RelationshipType"] != null)
                                item.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader["FK_RelationshipType"].ToString(), 0);

                            if (reader["FK_Security_Role"] != null)
                                item.SecurityRoleId = ValueTypeHelper.Int32TryParse(reader["FK_Security_Role"].ToString(), 0);

                            if (reader["FK_Security_user"] != null)
                                item.SecurityUserId = ValueTypeHelper.Int32TryParse(reader["FK_Security_user"].ToString(), 0);

                            if (reader["ConfigXML"] != null)
                                item.ConfigXml = reader["ConfigXML"].ToString();

                            if (reader["Description"] != null)
                                item.Description = reader["Description"].ToString();

                            if (reader["Precondition"] != null)
                                item.Precondition = reader["Precondition"].ToString();

                            if (reader["Postcondition"] != null)
                                item.Postcondition = reader["Postcondition"].ToString();

                            if (reader["XSDSchema"] != null)
                                item.XsdSchema = reader["XSDSchema"].ToString();

                            if (reader["SampleXML"] != null)
                                item.SampleXml = reader["SampleXML"].ToString();

                            if (reader["CreateDateTime"] != null)
                                item.CreateDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["CreateDateTime"].ToString());

                            if (reader["ModDateTime"] != null)
                                item.ModDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader["ModDateTime"].ToString());

                            if (reader["CreateUser"] != null)
                                item.CreateUser = reader["CreateUser"].ToString();

                            if (reader["ModUser"] != null)
                                item.ModUser = reader["ModUser"].ToString();

                            if (reader["CreateProgram"] != null)
                                item.CreateProgram = reader["CreateProgram"].ToString();

                            if (reader["ModProgram"] != null)
                                item.ModProgram = reader["ModProgram"].ToString();

                            if (reader["Seq"] != null)
                                item.SequenceNumber = ValueTypeHelper.ConvertToNullableInt32(reader["Seq"].ToString());

                            if (reader["FK_Locale"] != null)
                            {
                                LocaleEnum locale;
                                if (Enum.TryParse(reader["FK_Locale"].ToString(), true, out locale))
                                {
                                    item.Locale = locale;
                                }
                            }
 
	                        #endregion

                            result.Add(item);
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            finally
            {
                StopTraceActivity(ApplicationConfigurationItemGetAllProcessName);
            }

            return result;
        }

        /// <summary>
        /// Insert, Update and Delete operations for Application Configuration Items
        /// </summary>
        /// <param name="applicationConfigurationItems">Collection of ApplicationConfigurationItem to process</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <param name="userLogin">User login</param>
        /// <returns>Operation result</returns>
        public OperationResultCollection Process(ApplicationConfigurationItemCollection applicationConfigurationItems, CallerContext callerContext, DBCommandProperties command, String userLogin)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            StartTraceActivity(ApplicationConfigurationItemCreateUpdateDeleteProcessName);
            try
            {
                SqlDataReader reader = null;

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    try
                    {
                        SqlParametersGenerator generator = new SqlParametersGenerator(AdminManagerSQLParametersGeneratorName);

                        SqlParameter[] parameters = generator.GetParameters(ApplicationConfigurationItemProcessSPParameters);

                        List<SqlDataRecord> preparedItems = new List<SqlDataRecord>();

                        foreach (ApplicationConfigurationItem item in applicationConfigurationItems)
                        {
                            SqlDataRecord preparedItem = new SqlDataRecord(generator.GetTableValueMetadata(ApplicationConfigurationItemProcessSPParameters, parameters[0].ParameterName));

                            preparedItem.SetValue(0, item.Id);
                            preparedItem.SetValue(1, item.ConfigParentId.HasValue ? item.ConfigParentId : (object)System.DBNull.Value);
                            preparedItem.SetValue(2, item.ContextDefinitionId.HasValue ? item.ContextDefinitionId : (object)System.DBNull.Value);
                            preparedItem.SetValue(3, item.Name);
                            preparedItem.SetValue(4, item.LongName);
                            preparedItem.SetValue(5, item.OrganizationId);
                            preparedItem.SetValue(6, item.ContainerId);
                            preparedItem.SetValue(7, item.CategoryId);
                            preparedItem.SetValue(8, item.EntityId);
                            preparedItem.SetValue(9, item.AttributeId);
                            preparedItem.SetValue(10, item.NodeTypeId);
                            preparedItem.SetValue(11, item.RelationshipTypeId);
                            preparedItem.SetValue(12, item.SecurityRoleId);
                            preparedItem.SetValue(13, item.SecurityUserId);
                            preparedItem.SetValue(14, item.ConfigXml);
                            preparedItem.SetValue(15, item.Description);
                            preparedItem.SetValue(16, item.Precondition);
                            preparedItem.SetValue(17, item.Postcondition);
                            preparedItem.SetValue(18, item.XsdSchema);
                            preparedItem.SetValue(19, item.SampleXml);
                            preparedItem.SetValue(20, item.CreateDateTime.HasValue ? item.CreateDateTime : (object)System.DBNull.Value);
                            preparedItem.SetValue(21, item.ModDateTime.HasValue ? item.ModDateTime : (object)System.DBNull.Value);
                            preparedItem.SetValue(22, item.CreateUser);
                            preparedItem.SetValue(23, item.ModUser);
                            preparedItem.SetValue(24, item.CreateProgram);
                            preparedItem.SetValue(25, item.ModProgram);
                            preparedItem.SetValue(26, item.SequenceNumber.HasValue ? item.SequenceNumber.Value : (object)System.DBNull.Value);
                            preparedItem.SetValue(27, item.Locale.HasValue ? (Int32)item.Locale.Value : (object)System.DBNull.Value);
                            preparedItem.SetValue(28, item.Action.ToString());
                            preparedItem.SetValue(29, item.ObjectName.ToString());

                            preparedItems.Add(preparedItem);
                        }

                        if (preparedItems.Any())
                        {
                            parameters[0].Value = preparedItems;
                        }

                        parameters[1].Value = userLogin;
                        parameters[2].Value = callerContext.ProgramName;
                        parameters[3].Value = 1; // ReturnResult = true

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, ApplicationConfigurationItemProcessSPName);

                        if (reader != null)
                        {
                            UpdateOperationResult(reader, applicationConfigurationItems, operationResultCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        OperationResult operationResult = new OperationResult();
                        operationResult.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

                        operationResultCollection.Add(operationResult);
                        operationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                    operationResultCollection.RefreshOperationResultStatus();
                    if (operationResultCollection.OperationResultStatus == OperationResultStatusEnum.Successful)
                        transactionScope.Complete();
                }

                operationResultCollection.RefreshOperationResultStatus();

            }
            finally
            {
                StopTraceActivity(ApplicationConfigurationItemCreateUpdateDeleteProcessName);
            }

            return operationResultCollection;
        }

        #endregion

        #region Private Methods

        private void UpdateOperationResult(SqlDataReader reader, ApplicationConfigurationItemCollection applicationConfigurationItems, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                Int32 sourceId = 0;
                Int32 resultId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String errorCode = String.Empty;
                String errorParam = String.Empty;

                if (reader["SourceId"] != null)
                    Int32.TryParse(reader["SourceId"].ToString(), out sourceId);
                if (reader["ResultId"] != null)
                    Int32.TryParse(reader["ResultId"].ToString(), out resultId);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorMessage"] != null)
                    errorMessage = reader["ErrorMessage"].ToString();
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();
                if (reader["Param1"] != null)
                    errorParam = reader["Param1"].ToString();

                //Get item for this result record
                ApplicationConfigurationItem item = applicationConfigurationItems.FirstOrDefault(e => e.Id == sourceId);

                if (item != null)
                {
                    if (item.Action == ObjectAction.Create)
                    {
                        //Update item id with the new value
                        item.Id = resultId;
                    }

                    OperationResult operationResult = new OperationResult();
                    operationResult.Id = item.Id;
                    operationResult.ReferenceId = String.IsNullOrWhiteSpace(item.ReferenceId) ? item.Name : item.ReferenceId;

                    if (hasError)
                    {
                        //Add error
                        if (String.IsNullOrWhiteSpace(errorCode))
                        {
                            operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                        }
                        else
                        {
                            Collection<Object> parameters = new Collection<Object> { errorParam };
                            operationResult.AddOperationResult(errorCode, parameters, OperationResultType.Error);
                        }
                    }
                    else
                    {
                        //No errors.. Set status as Successful
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                    operationResultCollection.Add(operationResult);
                }
            }
        }
       
        private static Boolean StartTraceActivity(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), MDMTraceSource.Configuration, false) : true;
        }

        private static Boolean StopTraceActivity(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), MDMTraceSource.Configuration) : true;
        }

        private static Boolean TraceInformation(String record)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), MDMTraceSource.Configuration) : true;
        }

        private static Boolean TraceError(String record)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), MDMTraceSource.Configuration);
        }

        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        #endregion
    }
}