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
    /// Class to perform CURD operation on MDMObjectType
    /// </summary>
    public class MDMObjectTypeBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal;
        private readonly MDMObjectTypeDA _mdmObjectTypeDA = new MDMObjectTypeDA();

        private readonly MDMObjectTypeBufferManager _bufferManager = new MDMObjectTypeBufferManager();

        private const String _processMethodName = "MDM.IntegrationManager.Business.MDMObjectTypeBL.Process";
        private const String _getByIdMethodName = "MDM.IntegrationManager.Business.MDMObjectTypeBL.GetById";
        private const String _getByShortNameMethodName = "MDM.IntegrationManager.Business.MDMObjectTypeBL.GetByShortName";
        private const String _getAllMethodName = "MDM.IntegrationManager.Business.MDMObjectTypeBL.GetAll";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public MDMObjectTypeBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Get Methods

        /// <summary>
        /// Get all MDMObjectType in the system
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectTypeCollection GetAll(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getAllMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            MDMObjectTypeCollection mdmObjectTypes = _bufferManager.GetAllMDMObjectTypesFromCache();

            if (mdmObjectTypes == null || !mdmObjectTypes.Any())
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                mdmObjectTypes = _mdmObjectTypeDA.Get(-1, null, command);

                if (mdmObjectTypes != null && mdmObjectTypes.Any())
                {
                    _bufferManager.UpdateMDMObjectTypesInCache(mdmObjectTypes);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getAllMethodName, MDMTraceSource.Integration);
            }

            return mdmObjectTypes;
        }

        /// <summary>
        /// Get MDMObjectType by Id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectType GetById(Int16 mdmObjectTypeId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByIdMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (mdmObjectTypeId < 0)
            {
                const String message = "MDMObjectTypeID must not be empty";
                throw new MDMOperationException("112926", message, _getByIdMethodName, null, _getByIdMethodName);
            }

            MDMObjectTypeCollection mdmObjectTypes = GetAll(callerContext);
            
            MDMObjectType mdmObjectType = mdmObjectTypes.FirstOrDefault(objectType => objectType.Id == mdmObjectTypeId);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByIdMethodName, MDMTraceSource.Integration);
            }

            return mdmObjectType;
        }

        /// <summary>
        /// Get MDMObjectType by ShortName
        /// </summary>
        /// <param name="mdmObjectTypeShortName">ShortName of connector for which info needs to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>MDMObjectType object collection</returns>
        public MDMObjectType GetByName(String mdmObjectTypeShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            if (String.IsNullOrWhiteSpace(mdmObjectTypeShortName))
            {
                const String message = "MDMObjectType ShortName must not be empty.";
                throw new MDMOperationException("112927", message, _getByIdMethodName, null, _getByIdMethodName);
            }

            MDMObjectTypeCollection mdmObjectTypes = GetAll(callerContext);
            
            MDMObjectType mdmObjectType = mdmObjectTypes.FirstOrDefault(objectType => objectType.Name == mdmObjectTypeShortName);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getByShortNameMethodName, MDMTraceSource.Integration);
            }

            return mdmObjectType;
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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "MDMObjectTypeBL.Process", String.Empty, "Process");
            }
        }

        #endregion Private Methods
    }
}
