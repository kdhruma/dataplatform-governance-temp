using System;

namespace MDM.ConfigurationManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Data;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;

    /// <summary>
    /// Specifies the MDMEvent Information Business Layer class
    /// </summary>
    public class MDMEventInfoBL : BusinessLogicBase
    {
        #region Methods
        
        /// <summary>
        /// Get all the MDMEvents from the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvents</returns>
        public MDMEventInfoCollection Get(CallerContext callerContext)
        {
            MDMEventInfoCollection eventInfos = null;
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                ValidateCallerContext(callerContext);

                MDMEventInfoDA mdmEventInfoDA = new MDMEventInfoDA();
                var bufferManager = new CacheBufferManager<MDMEventInfoCollection>(CacheKeyGenerator.GetMDMEventInfoCacheKey(), String.Empty);

                #region Load data from cache

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Loading MDMEvent Information from cache");
                }

                eventInfos = bufferManager.GetAllObjectsFromCache();

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("MDMEvent Information loaded from cache");
                }

                #endregion Load data from cache

                if (eventInfos == null || eventInfos.Count < 1)
                {
                    #region Load data from data base

                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Loading MDMEvent Information from Data base");
                    }

                    DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                    eventInfos = mdmEventInfoDA.Get(command);

                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("MDMEvent Information loaded from Data base");
                    }

                    #endregion Load data from data base

                    if (eventInfos != null && eventInfos.Count > 1)
                    {
                        bufferManager.SetBusinessObjectsToCache(eventInfos, 3);

                        if (currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("MDMEvent Information updated to cache");
                        }
                    }
                    else
                    {
                        if (currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("There is no MDMEvent information available in data base");
                        }
                    }
                }
                else
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("{0} MDMEvent information loaded from cache", eventInfos.Count));
                    }
                }
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return eventInfos;
        }

        #region Private Methods

        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.LogError("111846", "CallerContext cannot be null.");
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ConfigurationManager.Business.MDMEventHandlerBL", String.Empty, String.Empty);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
