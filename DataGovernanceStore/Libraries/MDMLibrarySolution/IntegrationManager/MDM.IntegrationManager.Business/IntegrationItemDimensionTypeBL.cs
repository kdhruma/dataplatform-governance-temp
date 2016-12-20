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
    /// Class to perform CURD operation on IntegrationItemDimensionType
    /// </summary>
    public class IntegrationItemDimensionTypeBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal;
        private readonly IntegrationItemDimensionTypeDA _integrationItemDimensionTypeDA = new IntegrationItemDimensionTypeDA();

        private readonly IntegrationItemDimensionTypeBufferManager _bufferManager = new IntegrationItemDimensionTypeBufferManager();

        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.IntegrationItemDimensionTypeBL.GetById";
        private const String _getByShortNameMethodName = "MDM.IntegrationManager.Business.IntegrationItemDimensionTypeBL.GetByShortName";
        private const String _getAllMethodName = "MDM.IntegrationManager.Business.IntegrationItemDimensionTypeBL.GetAll";
        private const String _getByConnectorIdMethodName = "MDM.IntegrationManager.Business.IntegrationItemDimensionTypeBL.GetByConnectorId";
        private const String _getByConnectorNameMethodName = "MDM.IntegrationManager.Business.IntegrationItemDimensionTypeBL.GetByConnectorName";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationItemDimensionTypeBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Get all IntegrationItemDimensionType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationItemDimensionType object collection</returns>
        public IntegrationItemDimensionTypeCollection GetAll(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getAllMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext, _getAllMethodName);

            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes =
                _bufferManager.GetAllIntegrationItemDimensionTypesFromCache();

            if (integrationItemDimensionTypes == null || !integrationItemDimensionTypes.Any())
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                integrationItemDimensionTypes = _integrationItemDimensionTypeDA.Get(-1, null, command);

                if (integrationItemDimensionTypes != null && integrationItemDimensionTypes.Any())
                {
                    _bufferManager.UpdateIntegrationItemDimensionTypesInCache(integrationItemDimensionTypes);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getAllMethodName, MDMTraceSource.Integration);
            }

            return integrationItemDimensionTypes;
        }

        /// <summary>
        /// Get IntegrationItemDimensionType by Id
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Id of dimension type for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationItemDimensionType object collection</returns>
        public IntegrationItemDimensionType GetById(Int32 integrationItemDimensionTypeId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext, _getByIdMethodName);

            if (integrationItemDimensionTypeId < 0)
            {
                const String message = "IntegrationItemDimensionType Id must not be less than 0";
                throw new MDMOperationException("112958", message, _getByIdMethodName, null, _getByIdMethodName);
            }
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = GetAll(callerContext);

            IntegrationItemDimensionType integrationItemDimensionType =
                integrationItemDimensionTypes.FirstOrDefault(itemDimensionType => itemDimensionType.Id == integrationItemDimensionTypeId);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return integrationItemDimensionType;
        }
       
        /// <summary>
        /// Get IntegrationItemDimensionType by ShortName
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationItemDimensionType object collection</returns>
        public IntegrationItemDimensionType GetByName(String integrationItemDimensionTypeShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext, _getByShortNameMethodName);

            if (String.IsNullOrWhiteSpace(integrationItemDimensionTypeShortName))
            {
                const String message = "IntegrationItemDimensionType ShortName must not be empty";
                throw new MDMOperationException("112959", message, _getByIdMethodName, null, _getByIdMethodName);
            }

            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = GetAll(callerContext);

            IntegrationItemDimensionType integrationItemDimensionType =
                integrationItemDimensionTypes.FirstOrDefault(itemDimensionType => itemDimensionType.Name == integrationItemDimensionTypeShortName);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration);
            }

            return integrationItemDimensionType;
        }

        /// <summary>
        /// Get IntegrationItemDimensionType by connector name
        /// </summary>
        /// <param name="connectorName">Name of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>IntegrationItemDimensionType object collection</returns>
        public IntegrationItemDimensionTypeCollection GetByConnectorName(String connectorName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByConnectorNameMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext, _getByConnectorNameMethodName);

            if (String.IsNullOrWhiteSpace(connectorName))
            {
                const String message = "ConnectorShortName must not be empty.";
                throw new MDMOperationException("112911", message, _getByConnectorNameMethodName, null, _getByConnectorNameMethodName);
            }
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = GetAll(callerContext);

            IntegrationItemDimensionTypeCollection filteredIntegrationItemDimensionTypes =
                (IntegrationItemDimensionTypeCollection)integrationItemDimensionTypes.GetIntegrationItemDimensionTypesByConnectorName(connectorName);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByConnectorNameMethodName, MDMTraceSource.Integration);
            }

            return filteredIntegrationItemDimensionTypes;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectorId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public IntegrationItemDimensionTypeCollection GetByConnectorId(Int16 connectorId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByConnectorIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext, _getByConnectorIdMethodName);

            if (connectorId < 1)
            {
                const String message = "Connector Id must not be less than one.";
                throw new MDMOperationException("112910", message, _getByConnectorIdMethodName, null, _getByConnectorIdMethodName);
            }

            IntegrationItemDimensionTypeCollection allIntegrationItemDimensionTypes = GetAll(callerContext);
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = new IntegrationItemDimensionTypeCollection();

            if (allIntegrationItemDimensionTypes != null && allIntegrationItemDimensionTypes.Count > 0)
            {
                foreach (IntegrationItemDimensionType dimensionType in allIntegrationItemDimensionTypes)
                {
                    if (dimensionType.ConnectorId == connectorId)
                    {
                        integrationItemDimensionTypes.Add(dimensionType);
                    }
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByConnectorIdMethodName, MDMTraceSource.Integration);
            }

            return integrationItemDimensionTypes;
        }

        #endregion Get Methods

        #endregion

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
        /// <param name="methodName"></param>
        private void ValidateCallerContext(CallerContext callerContext, String methodName)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationItemDimensionTypeBL", String.Empty, methodName);
            }
        }

        #endregion Private Methods

        #endregion
    }
}