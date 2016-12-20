using System;
using System.Collections.ObjectModel;


namespace MDM.Services.ServiceClientDataProviders
{
    using Core;
    using Interfaces;
    using DataProviderInterfaces;
    using Utility;
    using Services;
    
    /// <summary>
    /// Represents an implementation of the IMDMEventHandlerDataProvider using WCF service call
    /// </summary>
    public class MDMEventHandlerDataProviderUsingService : IMDMEventHandlerDataProvider
    {
        #region IMDMEventHandlerDataProvider Methods

        /// <summary>
        /// Returns the MDMEventHandlerCollection based on the input identifiers specified
        /// </summary>
        /// <param name="eventHandlerIdList">The MDMEventHandler identifiers for which data has to be retrieved</param>
        /// <param name="callerContext">Context indicating the caller of the method</param>
        /// <returns>A collection of MDMEventHandler objects</returns>
        public IMDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, ICallerContext callerContext)
        {
            InternalCommonService internalCommonService = GetInternalCommonService();
            return internalCommonService.GetMDMEventHandlers(eventHandlerIdList, callerContext);
        }

        #endregion IMDMEventHandlerDataProvider Methods

        #region Private Functions

        /// <summary>
        /// Returns an instance of the InternalCommonService.
        /// </summary>
        /// <returns></returns>
        private InternalCommonService GetInternalCommonService()
        {
            InternalCommonService internalCommonService = null;

            if (SecurityPrincipalHelper.IsSecurityPrincipalAvailableForCurrentUser())
            {
                internalCommonService = new InternalCommonService();
            }
            else
            {
                String bindingConfigName = String.Format("{0}_I{1}", WCFBindingType.WSHttpBinding.ToString(), MDMWCFServiceList.InternalCommonService.ToString());
                String userName = "superuser";
                String internalSuperUserPassword = "super user with special powers - 1234567890-=+_)(*&^%$#@!";

                internalCommonService = new InternalCommonService(bindingConfigName, userName, internalSuperUserPassword);
            }

            return internalCommonService;
        }

        #endregion Private Functions
    }
}
