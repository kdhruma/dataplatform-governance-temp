using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Class to perform CURD operation on ConnectorProfile
    /// </summary>
    public class ConnectorProfileBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal;
        private readonly ConnectorProfileDA _connectorProfileDA = new ConnectorProfileDA();

        private readonly ConnectorProfileBufferManager _bufferManager = new ConnectorProfileBufferManager();

        private const String _processMethodName = "MDM.IntegrationManager.Business.ConnectorProfileBL.Process";
        private const String _getAllMethodName = "MDM.IntegrationManager.Business.ConnectorProfileBL.GetAll";
        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.ConnectorProfileBL.GetById";
        private const String _getByNameMethodName = "MDM.IntegrationManager.Business.ConnectorProfileBL.GetByName";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ConnectorProfileBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region CUD Methods

        /// <summary>
        /// Create a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be created</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Create(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            this.Validate(connectorProfile, "Create");
            connectorProfile.Action = ObjectAction.Create;
            return this.Process(connectorProfile, callerContext);
        }

        /// <summary>
        /// Update a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be updated</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Update(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            this.Validate(connectorProfile, "Update");
            connectorProfile.Action = ObjectAction.Update;
            return this.Process(connectorProfile, callerContext);
        }

        /// <summary>
        /// Delete a ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be deleted</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        public OperationResult Delete(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            this.Validate(connectorProfile, "Delete");
            connectorProfile.Action = ObjectAction.Delete;
            return this.Process(connectorProfile, callerContext);
        }

        #endregion CUD Methods

        #region Get

        /// <summary>
        /// Get all ConnectorProfile in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfileCollection GetAll(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getAllMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            ConnectorProfileCollection connectorProfiles = _bufferManager.GetAllConnectorProfilesFromCache();

            if (connectorProfiles == null || !connectorProfiles.Any())
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                connectorProfiles = _connectorProfileDA.Get(-1, null, command);

                if (connectorProfiles != null && connectorProfiles.Any())
                {
                    _bufferManager.UpdateConnectorProfilesInCache(connectorProfiles);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getAllMethodName, MDMTraceSource.Integration);
            }

            return connectorProfiles;
        }

        /// <summary>
        /// Get ConnectorProfile by Id
        /// </summary>
        /// <param name="connectorProfileId">Id of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfile GetById(Int16 connectorProfileId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (connectorProfileId < 0)
            {
                const String message = "ConnectorID must not be less than one.";
                throw new MDMOperationException("112910", message, _getByIdMethodName, String.Empty, _getByIdMethodName);
            }

            ConnectorProfileCollection connectorProfiles = GetAll(callerContext);

            ConnectorProfile connectorProfile = connectorProfiles.FirstOrDefault(profile => profile.Id == connectorProfileId);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return connectorProfile;
        }

        /// <summary>
        /// Get ConnectorProfile by ShortName
        /// </summary>
        /// <param name="connectorProfileShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>ConnectorProfile object collection</returns>
        public ConnectorProfile GetByName(String connectorProfileShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByNameMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (String.IsNullOrWhiteSpace(connectorProfileShortName))
            {
                String message = String.Format("ConnectorShortName must not be empty.");
                throw new MDMOperationException("112911", message, _getByNameMethodName, String.Empty, _getByNameMethodName);
            }

            ConnectorProfileCollection connectorProfiles = GetAll(callerContext);

            ConnectorProfile connectorProfile = connectorProfiles.FirstOrDefault(profile => profile.Name == connectorProfileShortName);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByNameMethodName, MDMTraceSource.Integration);
            }

            return connectorProfile;
        }

        #endregion Get

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
        private void Validate(ConnectorProfile connectorProfile, String methodName)
        {
            if (connectorProfile == null)
            {
                const String message = "ConnectorProfile must not be NULL.";
                throw new MDMOperationException("112912", message, "ConnectorProfileBL." + methodName, String.Empty, methodName);
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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ConnectorProfileBL.Process", String.Empty, "Process");
            }
        }

        private OperationResult CreateOperationResult(ConnectorProfile connectorProfile)
        {
            OperationResult operationResult = new OperationResult();

            if (String.IsNullOrEmpty(connectorProfile.ReferenceId))
            {
                connectorProfile.ReferenceId = "-1";
            }

            OperationResult or = new OperationResult();
            or.ReferenceId = connectorProfile.ReferenceId;
            or.OperationResultStatus = OperationResultStatusEnum.None;
            return operationResult;
        }

        /// <summary>
        /// Process (create / update / delete) ConnectorProfile
        /// </summary>
        /// <param name="connectorProfile">ConnectorProfile to be processed</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Status indicating the result of operation</returns>
        private OperationResult Process(ConnectorProfile connectorProfile, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            ValidateCallerContext(callerContext);

            Validate(connectorProfile, "Process");

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
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
                operationResult = CreateOperationResult(connectorProfile);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _connectorProfileDA.Process(connectorProfile, callerContext.ProgramName, userName, operationResult, command);

                    transactionScope.Complete();
                }

                _bufferManager.RemoveConnectorProfilesFromCache();
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResult;
        }

        #endregion Private Methods
    }
}
