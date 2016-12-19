using System;
using System.Diagnostics;

namespace MDM.AdminManager.Business
{
    using MDM.AdminManager.Data;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using MDM.ConfigurationManager.Business;
    using MDM.MessageManager.Business;

    /// <summary>
    /// Business Logic Layer for MDM Feature Config business object
    /// </summary>
    public class MDMFeatureConfigBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting locale message.
        /// </summary>
        LocaleMessageBL _localeMessageBL = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public MDMFeatureConfigBL()
        {
            _localeMessageBL = new LocaleMessageBL();            
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Gets all MDM Feature Config 
        /// </summary>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDMFeature Config Collection</returns>
        public MDMFeatureConfigCollection GetMDMFeatureConfigCollection(CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MDMFeatureConfigBL.GetMDMFeatureConfigCollection", MDMTraceSource.Configuration, false);

            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            MDMFeatureConfigCollection mdmFeatureConfigs = null;            
                        
            try
            {
                String errorMessage = String.Empty;
                LocaleMessage localeMessage = new LocaleMessage();

                if (callerContext == null)
                {                  
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111846", false, callerContext);
                    throw new MDMOperationException("111846", localeMessage.Message, "AdminManager.Business.MDMFeatureConfigBL", String.Empty, "GetMDMFeatureConfigCollection");
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                MDMFeatureConfigDA mdmFeatureConfigDA = new MDMFeatureConfigDA();
                mdmFeatureConfigs = mdmFeatureConfigDA.GetMDMFeatureConfigCollection(callerContext, command);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested MDM feature configs", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);
                    MDMTraceHelper.StopTraceActivity("MDMFeatureConfigBL.GetMDMFeatureConfigCollection", MDMTraceSource.Configuration);
                }                
                    
            }

            return mdmFeatureConfigs;
        }    
       

        /// <summary>
        /// Gets MDM Feature Config by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>    
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDM Feature Config</returns>
        public MDMFeatureConfig GetFeatureConfig(MDMCenterApplication application, String moduleName, String version, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MDMFeatureConfigBL.GetFeatureConfig", MDMTraceSource.Configuration, false);

            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            MDMFeatureConfig mdmFeatureConfig = null;

            try
            {
                #region Validation

                String errorMessage = String.Empty;
                LocaleMessage localeMessage = new LocaleMessage();

                if (String.IsNullOrWhiteSpace(moduleName))
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113103", false, callerContext);
                    throw new MDMOperationException("113103", localeMessage.Message, "AdminManager.Business.MDMFeatureConfigBL", String.Empty, "GetFeatureConfig");

                }

                if (String.IsNullOrWhiteSpace(version))
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113104", false, callerContext);
                    throw new MDMOperationException("113104", localeMessage.Message, "AdminManager.Business.MDMFeatureConfigBL", String.Empty, "GetFeatureConfig");
                }

                if (callerContext == null)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111846", false, callerContext);
                    throw new MDMOperationException("111846", localeMessage.Message, "AdminManager.Business.MDMFeatureConfigBL", String.Empty, "GetFeatureConfig");
                }

                #endregion Validaton

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                MDMFeatureConfigDA mdmFeatureConfigDA = new MDMFeatureConfigDA();
                mdmFeatureConfig = mdmFeatureConfigDA.GetMDMFeatureConfig(application, moduleName, version, callerContext, command);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested MDM feature config", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);
                    MDMTraceHelper.StopTraceActivity("MDMFeatureConfigBL.GetFeatureConfig", MDMTraceSource.Configuration);
                }

            }

            return mdmFeatureConfig;
        }

        #endregion Public Methods
       
    }
}
