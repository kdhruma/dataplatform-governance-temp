using System;
using System.Diagnostics;
using System.Transactions;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.MonitoringManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Class to perform CURD operation on inbound queue item
    /// </summary>
    public class InboundQueueBL : BusinessLogicBase
    {
        #region Fields

        ServerInfoBL serverInfoBL = new ServerInfoBL();
        private SecurityPrincipal _securityPrincipal = null;
        private LocaleEnum _systemDataLocale = GlobalizationHelper.GetSystemUILocale();
        private InboundQueueDA _inboundQueueDA = new InboundQueueDA();

        private const String _processMethodName = "MDM.IntegrationManager.Business.InboundQueueBL.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Business.InboundQueueBL.Get";
        private const String _markAsProcessedMethodName = "MDM.IntegrationManager.Business.InboundQueueBL.MarkAsProcessed";

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public InboundQueueBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Get Queue items

        /// <summary>
        /// Get inbound queue item for a given range of Ids
        /// </summary>
        /// <param name="logStatus">Inbound queue item status based on which data is to be  fetched</param>
        /// <param name="fromCount">Start of range of values to be fetched</param>
        /// <param name="toCount">End of range of values to be fetched</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns>Inbound queue item object</returns>
        public InboundQueueItemCollection Get(String logStatus, Int64 fromCount, Int64 toCount, CallerContext callerContext)
        {
            InboundQueueItemCollection inboundQueueItems = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            //Parameter Validation
            ValidateCallerContext(callerContext);

            String userName = String.Empty;
            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = _getMethodName;
            }

            if (!String.IsNullOrWhiteSpace(callerContext.ServerName))
            {
                callerContext.ServerId = serverInfoBL.GetServerId(callerContext.ServerName);
            }
            else
            {
                callerContext.ServerId = serverInfoBL.GetServerId(Environment.MachineName);
            }

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            if (fromCount > -1 && toCount > -1 && !String.IsNullOrWhiteSpace(logStatus))
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    inboundQueueItems = _inboundQueueDA.Get(logStatus, fromCount, toCount, callerContext.ProgramName, userName, (Int16)callerContext.ServerId, command);
                    transactionScope.Complete();
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
            }

            return inboundQueueItems;
        }

        #endregion Get Queue items

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
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "InboundQueueBL.Process", String.Empty, "Process");
            }
        }

        #endregion Private Methods
    }
}
