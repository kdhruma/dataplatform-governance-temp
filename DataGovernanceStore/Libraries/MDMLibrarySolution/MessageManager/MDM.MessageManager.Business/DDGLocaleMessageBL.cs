using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace MDM.MessageManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.MessageManager.Data.SqlClient;
    using MDM.ConfigurationManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies DDG Locale Message manager
    /// </summary>
    public class DDGLocaleMessageBL : BusinessLogicBase
    {
        #region Fields
        
        private DDGLocaleMessageDA _ddgLocaleMessageDA = new DDGLocaleMessageDA();
        private DBCommandProperties _dbcommand = null;
        private DiagnosticActivity _diagnosticActivity;
        private TraceSettings _currentTraceSettings;
        Boolean _isTracingEnabled;
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();
        private static LocaleEnum _systemUILocale = GlobalizationHelper.GetSystemUILocale();
       
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DDGLocaleMessageBL()
        {
            _diagnosticActivity = new DiagnosticActivity();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            _isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCode">Indicates the Message Code</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="parameters">Indicates the parameters which to be used to format the locale message</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Returns the LocaleMessage based on the specified parameters</returns>
        public LocaleMessage Get(LocaleEnum locale, String messageCode, CallerContext callerContext, Object[] parameters = null, Boolean escapeSpecialCharacters = false)
        {
            LocaleMessage localeMessage = null;

            try
            {
                #region Diagnostics & Tracing

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                LocaleMessageCollection resultLocaleMessages = _ddgLocaleMessageDA.Get(locale, new Collection<String>() { messageCode }, command);

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Fetched locale message from DB.");
                }

                if (resultLocaleMessages != null && resultLocaleMessages.Count > 0)
                {
                    localeMessage = (LocaleMessage)resultLocaleMessages.FirstOrDefault().Clone();
                }

                if (localeMessage != null && parameters != null)
                {
                    localeMessage.Message = String.Format(localeMessage.Message, parameters);
                }

                if (escapeSpecialCharacters)
                {
                    localeMessage.Message = localeMessage.Message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
                }
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }
            }
            return localeMessage;
        }

        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodes">Indicates the Message Code</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the LocaleMessage collection based on the specified parameters.</returns>
        public LocaleMessageCollection Get(LocaleEnum locale, Collection<String> messageCodes, CallerContext callerContext)
        {
            LocaleMessageCollection localeMessages = null;

            try
            {
                #region Diagnostics & Tracing

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                localeMessages = _ddgLocaleMessageDA.Get(locale, messageCodes, command);

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Fetched locale messages from DB.");
                }
            }

            finally
            {
                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }
            }

            return localeMessages;
        }

        /// <summary>
        /// Get all DDG Locale Message collection
        /// </summary>
        /// <returns>Returns all the DDG related locale messages</returns>
        public LocaleMessageCollection GetAll(CallerContext callerContext)
        {
            LocaleMessageCollection localeMessageCollection = new LocaleMessageCollection();

            try
            {
                #region Diagnostics & Tracing

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Start();
                }

                #endregion Diagnostics & Tracing

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                localeMessageCollection = _ddgLocaleMessageDA.GetAll(command);

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Fetched locale messages from DB.");
                }
            }

            finally
            {
                if (_isTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }
            }
            return localeMessageCollection;
        }


        /// <summary>
        /// Process collection of locale messages
        /// </summary>
        /// <param name="localeMessages">Indicates locale message collection to be processed</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of business rule operation results for the locale messages processed</returns>
        public BusinessRuleOperationResultCollection Process(LocaleMessageCollection localeMessages, CallerContext callerContext)
        {
            BusinessRuleOperationResultCollection businessRuleOperationResults = new BusinessRuleOperationResultCollection();
            Object[] parameters = null;
            String errorMessage = String.Empty;

            try
            {
                #region Start Diagnostic

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Start();
                    _diagnosticActivity.LogData("Input DDG LocaleMessageCollection", localeMessages.ToXml());
                }

                #endregion Start Diagnostic

                #region Prepare OperationResults

                if (localeMessages == null || localeMessages.Count < 1)
                {
                    parameters = new Object[] { "DDG LocaleMessageCollection" };
                    errorMessage = String.Format("{0} cannot be null", parameters);
                    _diagnosticActivity.LogError("113907", errorMessage);
                    businessRuleOperationResults.AddOperationResult("113907", errorMessage, parameters, OperationResultType.Error, callerContext);
                    return businessRuleOperationResults;
                }

                PrepareBusinessRuleOperationResultSchema(localeMessages, businessRuleOperationResults, callerContext);

                #endregion Prepare OperationResults

                #region Validations

                ValidateInputParameters(localeMessages, businessRuleOperationResults, callerContext);

                localeMessages = FilterLocaleMessagesBasedOnResults(localeMessages, businessRuleOperationResults);

                #endregion Validations

                #region Load Original And Fill LocaleMessages

                // Does system have anything to process after performing validations
                if (localeMessages == null || localeMessages.Count < 1)
                {
                    _diagnosticActivity.LogError("There are no messages available for processing.");
                    return businessRuleOperationResults;
                }

                LoadOriginalAndFillLocaleMessages(localeMessages, callerContext);

                #endregion Load Original And Fill LocaleMessages

                #region Compare And Merge

                CompareAndMergeLocaleMessages(localeMessages, businessRuleOperationResults, callerContext);

                localeMessages = FilterLocaleMessagesBasedOnResults(localeMessages, businessRuleOperationResults);

                #endregion Compare And Merge

                #region Process LocaleMessages

                if (localeMessages == null || localeMessages.Count < 1)
                {
                    _diagnosticActivity.LogError("There are no messages available for processing.");
                    return businessRuleOperationResults;
                }

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Starting transaction to process DDG locale message in business logic layer.");
                }

                #region Update DDG Locale messages to Database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    ProcessLocaleMessages(localeMessages, businessRuleOperationResults, callerContext);
                    transactionScope.Complete();
                }

                if (_isTracingEnabled)
                {
                    _diagnosticActivity.LogInformation("Completed transaction to process DDG locale message in business logic layer.");
                }

                #endregion Update DDG Locale messages to Database

                #endregion Process LocaleMessages

            }
            finally
            {
                #region Stop Diagnostic

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    _diagnosticActivity.Stop();
                }

                #endregion Stop Diagnostic
            }

            return businessRuleOperationResults;
        }

        #endregion

        #region Private Methods
        private void ProcessLocaleMessages(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
        {
            _dbcommand = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            _ddgLocaleMessageDA.Process(localeMessages, businessRuleOperationResults, _dbcommand, userName, callerContext.ProgramName);

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                _diagnosticActivity.LogData("Processed Locale Messages Operationresult", businessRuleOperationResults.ToXml());
            }

            businessRuleOperationResults.RefreshBusinessRuleOperationResultStatus();

            LocalizeErrors(businessRuleOperationResults,callerContext);
        }

        private void CompareAndMergeLocaleMessages(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
        {
            Object[] parameters = null;
            String errorMessage = String.Empty;
            foreach (LocaleMessage deltaLocaleMessage in localeMessages)
            {
                BusinessRuleOperationResult businessRuleOperationResult = businessRuleOperationResults.GetBusinessRuleOperationResultByReferenceId(ValueTypeHelper.Int64TryParse(deltaLocaleMessage.ReferenceId, -1)) as BusinessRuleOperationResult;

                if (deltaLocaleMessage.Action == ObjectAction.Read || deltaLocaleMessage.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                LocaleMessage originalLocaleMessage = deltaLocaleMessage.OriginalLocaleMessage;

                if (originalLocaleMessage != null)
                {
                    if (deltaLocaleMessage.Action != ObjectAction.Delete)
                    {
                        //If message is there for create in any locale other than SDL;
                        //for which equivalent message already exists in SDL
                        if (originalLocaleMessage.Locale != deltaLocaleMessage.Locale)
                        {
                            deltaLocaleMessage.Action = ObjectAction.Create;
                        }
                        else
                        {
                            originalLocaleMessage.MergeDelta(deltaLocaleMessage, false);
                        }
                    }
                }
                else
                {
                    if (deltaLocaleMessage.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { deltaLocaleMessage.Code };
                        errorMessage = "Failed to update or delete the locale message '{0}', as it does not exist in the system.";
                        AddOperationResult(businessRuleOperationResult, "114313", errorMessage, parameters, OperationResultType.Error, callerContext);
                    }
                    else
                    {
                        deltaLocaleMessage.Action = ObjectAction.Create;
                    }
                }

                businessRuleOperationResult.PerformedAction = deltaLocaleMessage.Action;
            }
        }

        private void LoadOriginalAndFillLocaleMessages(LocaleMessageCollection localeMessages, CallerContext callerContext)
        {
            LocaleMessageCollection originalLocaleMessages = GetAll(callerContext);

            if (originalLocaleMessages != null && originalLocaleMessages.Count > 0)
            {
                foreach (LocaleMessage deltaLocaleMessage in localeMessages)
                {
                    deltaLocaleMessage.OriginalLocaleMessage = (LocaleMessage)originalLocaleMessages.Get(deltaLocaleMessage.Locale, deltaLocaleMessage.Code, GlobalizationHelper.GetSystemUILocale());

                    if (deltaLocaleMessage.OriginalLocaleMessage != null)
                    {
                        deltaLocaleMessage.Id = deltaLocaleMessage.OriginalLocaleMessage.Id;
                    }
                }
            }
        }

        private void PrepareBusinessRuleOperationResultSchema(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
        {
            if (localeMessages != null && localeMessages.Count > 0)
            {
                if (businessRuleOperationResults.Count > 0)
                {
                    businessRuleOperationResults.Clear();
                }

                Int32 localeMessageToBeCreated = -1;

                foreach (LocaleMessage localeMessage in localeMessages)
                {
                    if (localeMessage.Id < 0 && callerContext.Module != MDMCenterModules.DDGImport)
                    {
                        localeMessage.Id = localeMessageToBeCreated;
                        localeMessage.ReferenceId = localeMessageToBeCreated.ToString();
                        localeMessageToBeCreated--;
                    }

                    BusinessRuleOperationResult businessRuleOperationResult = new BusinessRuleOperationResult(localeMessage);
                    businessRuleOperationResults.Add(businessRuleOperationResult);
                }
            }
        }

        private LocaleMessageCollection FilterLocaleMessagesBasedOnResults(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults)
        {
            businessRuleOperationResults.RefreshBusinessRuleOperationResultStatus();

            if (businessRuleOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
            {
                //Nothing to process
                return null;
            }
            else if (localeMessages != null && businessRuleOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                BusinessRuleOperationResultCollection erroredRules = businessRuleOperationResults.GetBusinessRuleOperationResultByOperationResultStatus(OperationResultStatusEnum.Failed);

                if (erroredRules != null && erroredRules.Count > 0)
                {
                    foreach (BusinessRuleOperationResult result in erroredRules)
                    {
                        localeMessages.RemoveByReferenceId(result.ReferenceId);
                    }
                }
            }

            return localeMessages;
        }

        private void ValidateInputParameters(LocaleMessageCollection localeMessages, BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
        {
            Object[] parameters = null;
            String errorMessage = String.Empty;
            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null";
                _diagnosticActivity.LogError("111846", errorMessage);
                businessRuleOperationResults.AddOperationResult("111846", errorMessage, null, OperationResultType.Error, callerContext);
            }
            else
            {
                foreach (LocaleMessage deltalocaleMessage in localeMessages)
                {
                    if (deltalocaleMessage.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    BusinessRuleOperationResult businessRuleOperationResult = businessRuleOperationResults.GetBusinessRuleOperationResultByReferenceId(ValueTypeHelper.Int32TryParse(deltalocaleMessage.ReferenceId, -1));

                    if (deltalocaleMessage.MessageClass != MessageClassEnum.UnKnown && businessRuleOperationResult != null)
                    {
                         // Validate whether message is empty or not
                        if (String.IsNullOrWhiteSpace(deltalocaleMessage.Message))
                        {

                            parameters = new Object[] { "Message" };
                            errorMessage = String.Format("{0} cannot be null or empty.", parameters);
                            _diagnosticActivity.LogError(errorMessage);
                            AddOperationResult(businessRuleOperationResult, "113960", errorMessage, parameters, OperationResultType.Error, callerContext);
                        }

                        // Validate whether message code is specified or not
                        if (String.IsNullOrWhiteSpace(deltalocaleMessage.Code))
                        {
                            parameters = new Object[] { "Message Code" };
                            errorMessage = String.Format("{0} cannot be null or empty.", parameters);
                            _diagnosticActivity.LogError(errorMessage);
                            AddOperationResult(businessRuleOperationResult, "113960", errorMessage, parameters, OperationResultType.Error, callerContext);
                        }
                        else if (!deltalocaleMessage.Code.StartsWith("DDG"))
                        {
                            parameters = new Object[] { deltalocaleMessage.Code };
                            errorMessage = String.Format("Failed to process the locale message, as the message code '{0}' is not prefixed with 'DDG'. You must define this message code as 'DDG{0}'.", parameters);
                            _diagnosticActivity.LogError(errorMessage);

                            AddOperationResult(businessRuleOperationResult, "114441", errorMessage, parameters, OperationResultType.Error, callerContext);
                        }
                        else if(deltalocaleMessage.Code.Length > 10)
                        {
                            parameters = new Object[] { deltalocaleMessage.Code };
                            errorMessage = String.Format("Failed to process the locale message, as the message code '{0}' length is exceeding the maximum length of 10 characters.", parameters);
                            _diagnosticActivity.LogError(errorMessage);
                            AddOperationResult(businessRuleOperationResult, "114443", errorMessage, parameters, OperationResultType.Error, callerContext);
                        }

                        // Validate whether locale is specified or not
                        if (deltalocaleMessage.Locale == LocaleEnum.UnKnown)
                        {
                            if (deltalocaleMessage.Action != ObjectAction.Delete && deltalocaleMessage.Action != ObjectAction.Read && deltalocaleMessage.Action != ObjectAction.Ignore)
                            {
                                parameters = new Object[] { deltalocaleMessage.Code, deltalocaleMessage.Locale };
                                errorMessage = String.Format("Failed to process the locale message '{0}' as the Locale '{1}' does not exist in the system.", parameters);
                                _diagnosticActivity.LogError(errorMessage);
                                AddOperationResult(businessRuleOperationResult, "114532", errorMessage, parameters, OperationResultType.Error, callerContext);
                            }
                        }
                    }
                    else
                    {
                        //for delete scenario no need to validate for message type.
                        if (deltalocaleMessage.Action!= ObjectAction.Delete)
                        {
                            //create locale message
                            parameters = new Object[] { deltalocaleMessage.Code, deltalocaleMessage.MessageClass };
                            errorMessage = String.Format("Failed to process the locale message '{0}' as the MessageType '{1}' does not exist in the system.", parameters);
                            _diagnosticActivity.LogError(errorMessage);
                            AddOperationResult(businessRuleOperationResult, "114307", errorMessage, parameters, OperationResultType.Error, callerContext);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add to the Operation Result
        /// </summary>
        /// <param name="businessRuleOperationResults">Indicates the list of operation results for business rules process</param>
        /// <param name="messageCode">Indicates the locale message code</param>
        /// <param name="message">Indicates the locale message</param>
        /// <param name="parameters">Indicates the list of parameters which needs to be replaced in locale message</param>
        /// <param name="operationResultType">Indicates the operationresult type</param>
        /// <param name="callerContext">Indicates name of application and module which is performing the action</param>
        public void AddOperationResult(BusinessRuleOperationResult businessRuleOperationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DDG);
            }

            LocaleMessage localeMessage = GetLocaleMessageByCode(messageCode, message, parameters, callerContext);

            if (localeMessage != null)
            {
                businessRuleOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// Populate the locale message in operationresults
        /// </summary>
        /// <param name="businessRuleOperationResults">Indicates the list of operation results for business rules process</param>
        /// <param name="callerContext">Indicates the name of the Application and Module which is performing the action</param>
        private void LocalizeErrors(BusinessRuleOperationResultCollection businessRuleOperationResults, CallerContext callerContext)
        {
            LocaleMessage localeMessage = new LocaleMessage();

            foreach (BusinessRuleOperationResult businessRuleOperationResult in businessRuleOperationResults)
            {
                foreach (Error error in businessRuleOperationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                        if (localeMessage != null)
                        {
                            error.ErrorMessage = localeMessage.Message;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Get locale message for given message code or message with given parameters
        /// </summary>
        /// <param name="messageCode">Indicates message code of message</param>
        /// <param name="message">Indicates default message if message code is not available</param>
        /// <param name="parameters">Indicates parameters needs to be appended in locale message</param>
        /// <param name="callerContext">Indicates caller identity</param>
        /// <returns>Returns locale message for given message code or message</returns>
        private LocaleMessage GetLocaleMessageByCode(String messageCode, String message, Object[] parameters, CallerContext callerContext)
        {
            LocaleMessage localeMessage;

            if (parameters != null && parameters.Count() > 0)
            {
                localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            return localeMessage;
        }
        #endregion
    }
        #endregion
}