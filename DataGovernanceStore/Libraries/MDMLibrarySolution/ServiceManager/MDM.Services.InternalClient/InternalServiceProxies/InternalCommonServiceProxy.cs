using System;

namespace MDM.Services.InternalServiceProxies
{
    using MDM.BusinessObjects;
    using BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;
    using BusinessObjects.DataModel;

    /// <summary>
    /// Represents internal common service proxy class for Internal common service
    /// </summary>
    public class InternalCommonServiceProxy : InternalCommonServiceClient.InternalCommonServiceClient, MDM.WCFServiceInterfaces.IInternalCommonService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public InternalCommonServiceProxy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public InternalCommonServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public InternalCommonServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }
        #endregion Constructors


        /// <summary>
        /// Get Locale Message based on locale and message code
        /// </summary>
        /// <param name="locale">Indicates the Locale</param>
        /// <param name="messageCodes">Indicates the Message Code</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the LocaleMessage collection based on the specified parameters.</returns>
        public LocaleMessageCollection GetDDGLocaleMessages(LocaleEnum locale, System.Collections.ObjectModel.Collection<string> messageCodes, CallerContext callerContext)
        {
            return base.GetDDGLocaleMessagesByMessageCodesAndLocale(locale, messageCodes, callerContext);
        }

        /// <summary>
        /// Gets MDMRule Keywords for specified MDMRule Keyword Group
        /// </summary>
        /// <param name="mdmRuleKeywordGroupId">Indicates the MDMRule Keyword Group Id</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns the MDMRuleKeywordCollection object</returns>
        public MDMRuleKeywordCollection GetMDMRuleKeywords(Int32 mdmRuleKeywordGroupId, CallerContext callerContext)
        {
            return base.GetMDMRuleKeywordsByKeywordGroupId(mdmRuleKeywordGroupId, callerContext);
        }
    }
}
