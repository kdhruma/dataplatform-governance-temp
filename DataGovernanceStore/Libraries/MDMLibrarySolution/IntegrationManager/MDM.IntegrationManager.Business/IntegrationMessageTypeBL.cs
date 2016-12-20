using System;
using System.Diagnostics;
using System.Linq;

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
    /// Class to perform CURD operation on integration message type
    /// </summary>
    public class IntegrationMessageTypeBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal;
        private readonly IntegrationMessageTypeDA _integrationMessageTypeDA = new IntegrationMessageTypeDA();

        private readonly IntegrationMessageTypeBufferManager _bufferManager = new IntegrationMessageTypeBufferManager();

        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationMessageTypeBL.Process";
        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.IntegrationMessageTypeBL.GetById";
        private const String _getByShortNameMethodName = "MDM.IntegrationManager.Business.IntegrationMessageTypeBL.GetByShortName";
        private const String _getAllMethodName = "MDM.IntegrationManager.Business.IntegrationMessageTypeBL.GetAll";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationMessageTypeBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Get Methods

        /// <summary>
        /// Get all IntegrationMessageType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageTypeCollection GetAll(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getAllMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            IntegrationMessageTypeCollection integrationMessageTypes = _bufferManager.GetAllIntegrationMessageTypesFromCache();

            if (integrationMessageTypes == null || !integrationMessageTypes.Any())
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                integrationMessageTypes = _integrationMessageTypeDA.Get(-1, null, command);

                if (integrationMessageTypes != null && integrationMessageTypes.Any())
                {
                    _bufferManager.UpdateIntegrationMessageTypesInCache(integrationMessageTypes);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getAllMethodName, MDMTraceSource.Integration);
            }

            return integrationMessageTypes;
        }

        /// <summary>
        /// Get IntegrationMessageType by Id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageType GetById(Int16 integrationMessageTypeId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (integrationMessageTypeId < 0)
            {
                const String message = "IntegrationMessageTypeID must not be less than one.";
                throw new MDMOperationException("112924", message, _getByIdMethodName, null, _getByIdMethodName);
            }

            IntegrationMessageTypeCollection integrationMessageTypes = GetAll(callerContext);
            
            IntegrationMessageType integrationMessageType = integrationMessageTypes.FirstOrDefault(messageType => messageType.Id == integrationMessageTypeId);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return integrationMessageType;
        }

        /// <summary>
        /// Get IntegrationMessageType by ShortName
        /// </summary>
        /// <param name="integrationMessageTypeShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationMessageType object collection</returns>
        public IntegrationMessageType GetByName(String integrationMessageTypeShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (String.IsNullOrWhiteSpace(integrationMessageTypeShortName))
            {
                const String message = "IntegrationMessageType ShortName must not be empty.";
                throw new MDMOperationException("112925", message, _getByShortNameMethodName, null, _getByShortNameMethodName);
            }

            IntegrationMessageTypeCollection integrationMessageTypes = GetAll(callerContext);
            
            IntegrationMessageType integrationMessageType = integrationMessageTypes.FirstOrDefault(messageType => messageType.Name == integrationMessageTypeShortName);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration);
            }

            return integrationMessageType;
        }
        
        #endregion Get Methods

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
        /// Validate caller context if it is null or not.
        /// </summary>
        /// <param name="callerContext">Caller context to validate</param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationMessageTypeBL.Process", String.Empty, "Process");
            }
        }

        #endregion Private Methods
    }
}
