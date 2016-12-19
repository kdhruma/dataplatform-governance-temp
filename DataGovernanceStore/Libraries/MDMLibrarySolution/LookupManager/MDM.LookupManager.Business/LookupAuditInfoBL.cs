using System;
using System.Diagnostics;

namespace MDM.LookupManager.Business
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.LookupManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Specifies business logic operations for Lookup with audit information
    /// </summary>
    public class LookupAuditInfoBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Instantiate LookupAuditInfo BL
        /// </summary>
        public LookupAuditInfoBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            GetSecurityPrincipal();
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Populated lookup related audit information
        /// </summary>
        /// <param name="lookup">Indicates lookup for which audit information needs to populate</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        public void PopulateLookupAuditInfo(Lookup lookup, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("LookupAuditInfo.Get", MDMTraceSource.LookupGet, false);
            }

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested audit information for lookup table:{0}, Application:{2} and Service:{3}", lookup.Name, callerContext.Application, callerContext.Module), MDMTraceSource.LookupGet);
                }

                #region Populate lookup audit information data from DB

                PopulateLookupAuditInfoFromDB(lookup, callerContext);

                #endregion Populate lookup audit information data from DB
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("LookupAuditInfo.Get", MDMTraceSource.LookupGet);
                }
            }
        }

        #endregion Public Methods

        #region Private methods

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void PopulateLookupAuditInfoFromDB(Lookup lookup, CallerContext callerContext)
        {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            LookupAuditInfoDA lookupAuditInfoDA = new LookupAuditInfoDA();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading lookup audit info :'{0}' from database...", lookup.Name, MDMTraceSource.LookupGet));
            }

            //Make DB call..
            lookupAuditInfoDA.PopulateLookupAuditInfo(lookup, command);
        }

        #endregion Private methods

        #endregion Methods
    }
}
