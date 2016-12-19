using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Class to perform CURD operation on integration message
    /// </summary>
    public class IntegrationMessageBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private IntegrationMessageDA _integrationMessageDA = new IntegrationMessageDA();
        
        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationMessageBL.Process";
        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.IntegrationMessageBL.GetById";
        private const String _getMethodName = "MDM.IntegrationManager.Business.IntegrationMessageBL.Get";
        private const String _getNextBatchMethodName = "MDM.IntegrationManager.Business.IntegrationMessageBL.GetNextBatch";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationMessageBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a integration message
        /// </summary>
        /// <param name="integrationMessage">Integration message to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(IntegrationMessage integrationMessage, CallerContext callerContext)
        {
            this.Validate(integrationMessage, "Create");
            integrationMessage.Action = Core.ObjectAction.Create;
            return this.Process(new IntegrationMessageCollection { integrationMessage }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update a integration message
        /// </summary>
        /// <param name="integrationMessage">Integration message to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(IntegrationMessage integrationMessage, CallerContext callerContext)
        {
            this.Validate(integrationMessage, "Update");
            integrationMessage.Action = Core.ObjectAction.Update;
            return this.Process(new IntegrationMessageCollection { integrationMessage }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Delete a integration message
        /// </summary>
        /// <param name="integrationMessage">Integration message to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(IntegrationMessage integrationMessage, CallerContext callerContext)
        {
            this.Validate(integrationMessage, "Delete");
            integrationMessage.Action = Core.ObjectAction.Delete;
            return this.Process(new IntegrationMessageCollection { integrationMessage }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Process (create / update / delete) integration messages
        /// </summary>
        /// <param name="integrationMessageCollection">Integration messages to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResultCollection Process(IntegrationMessageCollection integrationMessageCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            if (integrationMessageCollection == null && integrationMessageCollection.Count < 0)
            {
                String message = "IntegrationMessageCollection must not be NULL.";
                throw new MDMOperationException("112922", message, "IntegrationMessageBL.Process", String.Empty, "Process");
            }

            #endregion Parameter Validation

            try
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = _processMethodName;
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResultCollection = _integrationMessageDA.Process(integrationMessageCollection, callerContext.ProgramName, userName, command);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResultCollection;
        }


        #endregion CUD Methods

        #region Get methods

        /// <summary>
        /// Get integration message by id
        /// </summary>
        /// <param name="integrationMessageId">Integration message id to fetch the value for</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration message object</returns>
        public IntegrationMessage GetById(Int64 integrationMessageId, CallerContext callerContext)
        {
            IntegrationMessage integrationMessage = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (integrationMessageId >0)
            {
                Collection<Int64> integrationMessageIds = new Collection<Int64>();
                integrationMessageIds.Add(integrationMessageId);
                integrationMessage = _integrationMessageDA.Get(integrationMessageIds, 0, command).FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return integrationMessage;
        }

        /// <summary>
        /// Get integration messages by ids
        /// </summary>
        /// <param name="integrationMessageId">Collection of message ids to fetch</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Integration message collection object</returns>
        public IntegrationMessageCollection Get(Collection<Int64> integrationMessageIds, CallerContext callerContext)
        {
            IntegrationMessageCollection integrationMessageCollection = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (integrationMessageIds.Count > 0)
            {
                integrationMessageCollection = _integrationMessageDA.Get(integrationMessageIds, 0, command);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return integrationMessageCollection;
        }

        #endregion Get methods

        #region Private Methods

        /// <summary>
        /// Get security principal for currently logged in user
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// Validate input parameters
        /// </summary>
        private void Validate(IntegrationMessage integrationMessage, String methodName)
        {
            if (integrationMessage == null)
            {
                String message = "IntegrationMessage must not be NULL.";
                throw new MDMOperationException("112923", message, "IntegrationMessageBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// Validate caller context if it is null or not.
        /// </summary>
        /// <param name="callerContext">Caller context to validate</param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationMessageBL.Process", String.Empty, "Process");
            }
        }

        #endregion Private Methods
    }
}
