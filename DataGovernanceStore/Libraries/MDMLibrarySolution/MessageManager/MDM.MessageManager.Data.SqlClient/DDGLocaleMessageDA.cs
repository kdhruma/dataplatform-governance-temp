using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Transactions;

namespace MDM.MessageManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies the data access operations for DDG locale message
    /// </summary>
    public class DDGLocaleMessageDA : SqlClientDataAccessBase
    {
        #region Fields

        private DiagnosticActivity diagnosticActivity;
        private TraceSettings currentTraceSettings;
        private Boolean isTracingEnabled;
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DDGLocaleMessageDA()
        {
            diagnosticActivity = new DiagnosticActivity();
            currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        ///  Get DDG Locale Message based on the specified parameter.
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <returns>Returns LocaleMessageCollection</returns>
        public LocaleMessageCollection Get(LocaleEnum locale, Collection<String> messageCodelist, DBCommandProperties command)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_BusinessRuleManager_LocaleMessage_Get";

            List<SqlDataRecord> messageCodeListTable;
            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManagement_SqlParameters");
                parameters = generator.GetParameters("BusinessRuleManagement_LocaleMessage_Get_ParametersArray");
                SqlMetaData[] messageCodeListMetadata = generator.GetTableValueMetadata("BusinessRuleManagement_LocaleMessage_Get_ParametersArray", parameters[1].ParameterName);

                CreateMessageCodeTableParams(messageCodelist, messageCodeListMetadata, out messageCodeListTable);
                parameters[0].Value = (Int32)locale;
                parameters[1].Value = messageCodeListTable;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }

                localeMessageCollection = PopulateSystemMessages(reader);    
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return localeMessageCollection;
        }

        /// <summary>
        ///  Get DDG Locale Message based on the specified parameter.
        /// </summary>
        /// <returns>Returns LocaleMessageCollection</returns>
        public LocaleMessageCollection GetAll(DBCommandProperties command)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters = null;
            String storedProcedureName = "usp_BusinessRuleManager_LocaleMessage_Get";

            try
            {
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }

                localeMessageCollection = PopulateSystemMessages(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return localeMessageCollection;
        }

        /// <summary>
        /// Process DDG locale message
        /// </summary>
        /// <param name="localeMessages">Indicates locale messages to be processed</param>
        /// <param name="businessRuleOperationResults">Indicates business rule operation results of locale message processing</param>
        /// <param name="command">Indicates command properties for the database</param>
        /// <param name="loginUser">Indicates Logged in user</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        public void Process(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, DBCommandProperties command, String loginUser, String programName)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_BusinessRuleManager_LocaleMessage_Process";

            #region Diagnostics & Tracing

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & Tracing

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManagement_SqlParameters");

                    parameters = generator.GetParameters("BusinessRuleManagement_LocaleMessage_Process_ParametersArray");
                    SqlMetaData[] localeMessageTableMetadata = generator.GetTableValueMetadata("BusinessRuleManagement_LocaleMessage_Process_ParametersArray", parameters[0].ParameterName);

                    List<SqlDataRecord> localeMessageRecords = PopulateLocaleMessageSqlDataRecord(localeMessages, localeMessageTableMetadata);

                    parameters[0].Value = localeMessageRecords;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                    }

                    PopulateOperationResult(reader, localeMessages, businessRuleOperationResults);
                    transactionScope.Complete();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                #region Diagnostics & Tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing
            }
        }

       #endregion

        #region Private Methods

        /// <summary>
        /// Populate OperationResult based on the information returned by database
        /// </summary>
        /// <param name="reader">Indicates the SQLdata reader</param>
        /// <param name="localeMessages">Indicates the reader object which reads the data available in the database</param>
        /// <param name="businessRuleOperationResults">Indicates collection of OperationResults for locale messages</param>
        private void PopulateOperationResult(SqlDataReader reader, LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults)
        {
            if (!(reader == null || reader.IsClosed))
            {
                while (reader.Read())
                {
                    Boolean hasError = false;
                    String errorCode = String.Empty, messageCode = String.Empty; 
                    LocaleEnum locale = GlobalizationHelper.GetSystemDataLocale();
                    Int32 referenceId = -1, localeMessageId = -1;
                    

                    BusinessRuleOperationResult businessRuleOperationResult = null;

                    if (reader["Id"] != null)
                    {
                        referenceId = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), -1);
                    }

                    if (reader["MessageId"] != null)
                    {
                        localeMessageId = ValueTypeHelper.Int32TryParse(reader["MessageId"].ToString(), -1);
                    }

                    if (reader["MessageCode"] != null)
                    {
                        messageCode = reader["MessageCode"].ToString();
                    }
                    
                    if (reader["FK_Locale"] != null)
                    {
                        ValueTypeHelper.EnumTryParse<LocaleEnum>(reader["FK_Locale"].ToString(), true, out locale);
                    }

                    if (reader["IsError"] != null)
                    {
                        hasError = ValueTypeHelper.BooleanTryParse(reader["IsError"].ToString(), false);
                    }

                    if (reader["ErrorCode"] != null)
                    {
                        errorCode = reader["ErrorCode"].ToString();
                    }
                    businessRuleOperationResult = businessRuleOperationResults.GetBusinessRuleOperationResultByReferenceId(referenceId);

                    if (businessRuleOperationResult != null)
                    {
                        businessRuleOperationResult.Id = localeMessageId;
                        businessRuleOperationResult.DDGLocaleMessageId = localeMessageId;
                        businessRuleOperationResult.DDGLocaleMessageCode = messageCode;
                        businessRuleOperationResult.Locale = locale;
                        if (hasError & !String.IsNullOrWhiteSpace(errorCode))
                        {
                            businessRuleOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                        else
                        {
                            businessRuleOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Populate DDG locale message collection based on the information returned by database
        /// </summary>
        /// <param name="reader">Indicates the reader object which reads the data available in the database</param>
        private LocaleMessageCollection PopulateSystemMessages(SqlDataReader reader)
        {
            LocaleMessageCollection DDGLocaleMessages = new LocaleMessageCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Int32 id = -1;
                    String code = String.Empty;
                    String description = String.Empty;
                    String message = String.Empty;
                    String helpLink = String.Empty;
                    String whereUsed = String.Empty;
                    MessageClassEnum messageClass = MessageClassEnum.UnKnown;
                    LocaleEnum localeEnum = LocaleEnum.UnKnown;

                    if (reader["LocaleMessageId"] != null)
                    {
                        id = ValueTypeHelper.Int32TryParse(reader["LocaleMessageId"].ToString(), -1);
                    }

                    if (reader["MessageCode"] != null)
                    {
                        code = reader["MessageCode"].ToString();
                    }

                    if (reader["MessageClass"] != null)
                    {
                        Enum.TryParse<MessageClassEnum>(reader["MessageClass"].ToString(), out  messageClass);
                    }

                    if (reader["Message"] != null)
                    {
                        message = reader["Message"].ToString();
                    }

                    if (reader["Description"] != null)
                    {
                        description = reader["Description"].ToString();
                    }

                    if (reader["HelpLink"] != null)
                    {
                        helpLink = reader["HelpLink"].ToString();
                    }

                    if (reader["WhereUsed"] != null)
                    {
                        whereUsed = reader["WhereUsed"].ToString();
                    }

                    if (reader["Locale"] != null)
                    {
                        String localeName = reader["Locale"].ToString().Replace("-", "_");
                        Enum.TryParse<LocaleEnum>(localeName, out  localeEnum);
                    }

                    LocaleMessage localeMessage = new LocaleMessage(id, code, messageClass, message, description, helpLink, localeEnum, whereUsed);
                    DDGLocaleMessages.Add(localeMessage);
                }
            }

            return DDGLocaleMessages;
        }

        /// <summary>
        /// Create Message code Mapping TVP for process
        /// </summary>
        /// <param name="messageCodelist">Indicates collection of message code to be fetched</param>
        /// <param name="messageCodeListMetaData">Metadata of message code collection TVP</param>
        /// <param name="messageCodeListTable">Data record of message code list collection</param>
        private void CreateMessageCodeTableParams(Collection<String> messageCodelist, SqlMetaData[] messageCodeListMetaData,
                                       out List<SqlDataRecord> messageCodeListTable)
        {
            messageCodeListTable = new List<SqlDataRecord>();
            
            foreach (String messageCode in messageCodelist)
            {
                SqlDataRecord messageCodeMappingRecord = new SqlDataRecord(messageCodeListMetaData);
                messageCodeMappingRecord.SetValue(0, messageCode);

                messageCodeListTable.Add(messageCodeMappingRecord);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (messageCodeListTable.Count == 0)
            {
                messageCodeListTable = null;
            }
        }


        private List<SqlDataRecord> PopulateLocaleMessageSqlDataRecord(LocaleMessageCollection localeMessages, SqlMetaData[] localeMessageTableMetadata)
        {
            List<SqlDataRecord> localeMessageList = new List<SqlDataRecord>();

            foreach (LocaleMessage localeMessage in localeMessages)
            {
                if (localeMessage.Action == ObjectAction.Ignore || localeMessage.Action == ObjectAction.Read)
                {
                    continue;
                }

                SqlDataRecord localeMessageDataRecord = new SqlDataRecord(localeMessageTableMetadata);

                localeMessageDataRecord.SetValue(0, ValueTypeHelper.Int32TryParse(localeMessage.ReferenceId,0));//Reference id
                localeMessageDataRecord.SetValue(1, localeMessage.Id);//PK_DDG_LocaleMessage
                localeMessageDataRecord.SetValue(2, localeMessage.Locale);//FK_Locale
                localeMessageDataRecord.SetValue(3, localeMessage.MessageClass);//MessageClass
                localeMessageDataRecord.SetValue(4, localeMessage.Code);//MessageCode
                localeMessageDataRecord.SetValue(5, localeMessage.Message);//Message
                localeMessageDataRecord.SetValue(6, localeMessage.Description);//Description
                localeMessageDataRecord.SetValue(7, localeMessage.HelpLink);//HelpLink
                localeMessageDataRecord.SetValue(8, localeMessage.WhereUsed);//WhereUsed
                localeMessageDataRecord.SetValue(9, localeMessage.MessageClass);//MessageType
                localeMessageDataRecord.SetValue(10, localeMessage.Action.ToString());//Action

                localeMessageList.Add(localeMessageDataRecord);
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (localeMessageList.Count == 0)
            {
                localeMessageList = null;
            }

            return localeMessageList;
        }

        #endregion

        #endregion
    }
}
