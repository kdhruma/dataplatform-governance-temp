using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.MessageManager.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    ///   Specifies Locale Message Data manager
    /// </summary>
    public class LocaleMessageDA : SqlClientDataAccessBase
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
        /// Process Locale Message based on different locale
        /// </summary>
        /// <param name="localeMessages">This parameter is specifying lists of instances of locale Message to be processed</param>
        /// <param name="loginUser">This parameter is specifying login user name</param>
        /// <param name="programName">This parameter is specifying program name</param>
        /// <param name="command">This parameter is specifying to which server we have to connect</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult Process(LocaleMessageCollection localeMessages, String loginUser, String programName, DBCommandProperties command)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            OperationResult operationResult = new OperationResult();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StartTraceActivity("LocaleMessageDA.LocaleMessage.Process", false);

                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                    parameters = generator.GetParameters("Messagemanager_LocaleMessage_Process_ParametersArray");

                    parameters[0].Value = localeMessages.ToXml();
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;

                    storedProcedureName = "usp_MessageManager_LocaleMessage_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            if (reader["IsError"] != null)
                            {
                                if (reader["IsError"].ToString() == "1")
                                {
                                    if (reader["ErrorMessage"] != null)
                                    {
                                        Error error = new Error();
                                        error.ErrorMessage = reader["ErrorMessage"].ToString();
                                        operationResult.Errors.Add(error);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("LocaleMessageDA.LocaleMessage.Process");
            }

            return operationResult;
        }

        /// <summary>
        ///  Get Locale Messages based on locale and Message code list
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodeList">Indicates the Message Code List</param>
        /// <returns>Returns LocaleMessageCollection</returns>
        public LocaleMessageCollection GetLocaleMessages(LocaleEnum locale, Collection<String> messageCodelist, DBCommandProperties command)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                parameters = generator.GetParameters("Messagemanager_LocaleMessage_Get_ParametersArray");

                parameters[0].Value = (Int32)locale;
                parameters[1].Value = ValueTypeHelper.JoinCollection(messageCodelist, ",");

                storedProcedureName = "usp_MessageManager_LocaleMessage_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);


                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 id = -1;
                        String code = String.Empty;
                        String description = String.Empty;
                        String message = String.Empty;
                        String kbaLink = String.Empty;
                        MessageClassEnum messageClass = MessageClassEnum.UnKnown;
                        LocaleEnum localeEnum = LocaleEnum.UnKnown;

                        if (reader["LocaleMessageId"] != null)
                            id = ValueTypeHelper.Int32TryParse(reader["LocaleMessageId"].ToString(), -1);

                        if (reader["MessageCode"] != null)
                            code = reader["MessageCode"].ToString();

                        if (reader["Description"] != null)
                            description = reader["Description"].ToString();

                        if (reader["Message"] != null)
                            message = reader["Message"].ToString();

                        if (reader["MessageClass"] != null)
                            Enum.TryParse<MessageClassEnum>(reader["MessageClass"].ToString(), out  messageClass);

                        if (reader["KBALink"] != null)
                            kbaLink = reader["KBALink"].ToString();

                        if (reader["LocaleMessageId"] != null)
                            ValueTypeHelper.Int32TryParse(reader["LocaleMessageId"].ToString(), id);

                        if (reader["Locale"] != null)
                        {
                            String localeName = reader["Locale"].ToString().Replace("-", "_");
                            Enum.TryParse<LocaleEnum>(localeName, out  localeEnum);
                        }

                        LocaleMessage localeMessage = new LocaleMessage(id, code, messageClass, message, description, kbaLink, localeEnum);
                        localeMessageCollection.Add(localeMessage);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return localeMessageCollection;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
